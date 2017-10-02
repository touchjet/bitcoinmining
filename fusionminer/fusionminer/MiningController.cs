using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using FusionMiner.Thrift;
using Thrift.Transport;
using Thrift.Server;
using System.Net;
using System.Net.Sockets;
using FusionMiner.Domain.DTO;

namespace FusionMiner
{
    public class MiningController
    {
        private static FDriver _hardware;
        private static List<MiningPool> _pool = new List<MiningPool>();

        private const UInt16 WORK_CACHE_SIZE = 16384;
        private const byte MAX_HASH_DATA_NTIME_ROLL = 8;
        private static HashData[] _workCache;
        private static object _workCacheLock = new object();
        private static UInt16 _workCacheReadPtr = 0;
        private static UInt16 _workCacheReadBlockPtr = 0;
        private static UInt16 _workCacheWritePtr = 0;

        private static bool _stopping = false;
        private static bool _stopped = false;
        private static UInt32 _poolDifficulty = 1;
        private static MinerStatus _minerStatus = new MinerStatus();
        private static object _minerStatusLock = new object();
        private static MinerInfo _minerInfo = new MinerInfo();
        private static string _localIP = "";
        private static MinerStatDTO _minerStatDTO;

        private static bool _poolRefresh = false;

        public static string LocalIP
        {
            get { return _localIP; }
        }

        private static bool _hostConnected = false;

        public static bool HostConnected
        {
            get { return _hostConnected; }
        }

        public static void Start()
        {
            _workCache = new HashData[WORK_CACHE_SIZE];
            for (_workCacheWritePtr = 0; _workCacheWritePtr < WORK_CACHE_SIZE; _workCacheWritePtr++)
            {
                _workCache[_workCacheWritePtr] = new HashData();
            }
            _workCacheWritePtr = 0;

            _minerInfo.UniqueId = Config.Data.UniqueId;
            _minerInfo.MAC = Config.Data.MAC;
            _minerInfo.Version = Config.Data.Version;
            _minerInfo.NickName = Config.Data.NickName;
            _minerInfo.SN = Config.Data.SN;
            _minerStatus.Miner = _minerInfo;

            var m = Config.Data.Hardwares.FirstOrDefault();
            switch (m.Type)
            {
                case HardwareType.DigBigSPI:
                    _hardware = new DigBigSPIDriver(m);
                    _hardware.OnHashResult += HandleHashResult;
                    _hardware.OnGetSingleHashData += GetSingleHashData;
                    break;
                case HardwareType.DigBigSPIV2:
                    _hardware = new DigBigSPIV2Driver(m);
                    _hardware.OnHashResult += HandleHashResult;
                    _hardware.OnGetBlockHashData += GetBlockHashData;
                    break;
                default:
                    throw new NotImplementedException("Hardware driver not implemented - " + m.Type.ToString());
            }

            DateTime noIpRebootTime = DateTime.UtcNow.AddMinutes(1);
            do
            {
                _localIP = Utility.LocalIPAddress();
                Thread.Sleep(1000);
                if (DateTime.UtcNow > noIpRebootTime)
                {
                    Utility.Log(LogLevel.Error, "Can't Get IP Address In 1 Minutes, Reboot!");
                    ProgramLauncher.Execute("/usr/bin/reboot", "");
                }
            } while (_localIP.Equals("127.0.0.1"));

            _minerStatDTO = new MinerStatDTO()
            {
                Miner = new MinerDTO()
                {
                    SN = Config.Data.SN,
                    MAC = Config.Data.MAC,
                    NickName = Config.Data.NickName,
                    Version = Config.Data.Version
                },
                UpTime = 0,
                NonceFound = 0,
                HardwareError = 0,
                SpeedAvg = 0,
                SpeedCur = 0,
                MaxTempBoard = 0,
                MaxTempChip = 0,
                LocalIP = _localIP,
                PoolUrl = Config.Data.Pools[0].Url,
                PoolUser = Config.Data.Pools[0].UserName,
            };

            Thread.Sleep(3000);

            foreach (var p in Config.Data.Pools)
            {
                switch (p.Type)
                {
                    case PoolType.Test:
                        _pool.Add(new TestDataPool());
                        break;
                    case PoolType.Stratum:
                        _pool.Add(new StratumPool(p.Url, p.Port, p.UserName, p.Password));
                        _pool.Last().OnPoolRefresh += (MiningPool pool) =>
                        {
                            if (pool == _currentPool)
                            { //Only refresh board buffer on requests from the primary pool
                                _poolRefresh = true;
                            }
                        };
                        break;
                    default:
                        throw new NotImplementedException("Pool interface not implemented - " + p.Type.ToString());
                }
            }


            if (!Config.NoDashAPI)
            {
                MinerServiceHandler minerServiceHandler = new MinerServiceHandler();
                MinerService.Processor processor = new MinerService.Processor(minerServiceHandler);
                TServerTransport minerTransport = new TServerSocket(new TcpListener(System.Net.IPAddress.Any, 2087), 1000);
                TServer minerserver = new TSimpleServer(processor, minerTransport);
                new Thread(new ThreadStart(() =>
                {
                    minerserver.Serve();
                })).Start();
            }

            new Thread(new ThreadStart(() =>
            {   //House keeping thread
                DateTime lastPoolAvailable = DateTime.UtcNow;
                DateTime nextCleanupTime = DateTime.UtcNow.AddMinutes(5);
                DateTime _nextHostStatusTime = DateTime.UtcNow;
                DateTime _nextHostMaintCheckTime = DateTime.UtcNow;
                DateTime _lastDifficultyChange = DateTime.UtcNow;
                HostInterface _host = null;

                try
                {
                    Utility.Log(LogLevel.Debug, "Activating Mod_Mono");
                    WebRequest.Create("http://127.0.0.1/").GetResponse();
                }
                catch
                {
                }

                while (!_stopping)
                {
                    Thread.Sleep(15000);
                    try
                    {
                        if (DateTime.UtcNow > nextCleanupTime)
                        {
                            nextCleanupTime = DateTime.UtcNow.AddHours(8);
                            ProgramLauncher.Execute("/usr/bin/rm", "-rf /var/log/journal/*");
                        }

                        // Reactivate problem hardware
                        if (_hardware.State == HardwareState.Stopped)
                        {
                            _hardware.Start();
                        }

                        // Reconnect dead pool
                        foreach (var p in _pool)
                        {
                            if (p.Dead)
                            {
                                int reconnectDelay = Math.Min(p.ConnectionFailCount * 15, 300);
                                if ((DateTime.UtcNow.Subtract(p.LastConnectionTime).TotalSeconds > reconnectDelay) || (CurrentPool == null))
                                {
                                    if (p.ReceiveTimeOut)
                                    {
                                        Utility.Log(LogLevel.Error, "Pool Receive Time Out.");
                                    }
                                    Utility.Log(LogLevel.Warning, "Reconnecting Pool -- {0} Previously Failed: {1}", p.Url, p.ConnectionFailCount);
                                    p.ConnectPool();
                                }
                            }
                        }

                        // Check pool difficulty updates
                        if ((CurrentPool != null) && (_currentPool.NominizedDifficulty != _poolDifficulty))
                        {
                            if ((_poolDifficulty > CurrentPool.NominizedDifficulty) || (_lastDifficultyChange.AddMinutes(10) < DateTime.UtcNow))
                            {
                                _lastDifficultyChange = DateTime.UtcNow;
                                _poolDifficulty = _currentPool.NominizedDifficulty;
                                _hardware.UpdateDifficulty(_poolDifficulty);
                            }
                        }

                        if (CurrentPool == null)
                        {
                            Utility.Log(LogLevel.Error, "No Available Pool Connection");
                            if (DateTime.UtcNow.Subtract(lastPoolAvailable).TotalMinutes > 5)
                            {
                                Utility.Log(LogLevel.Error, "Can't Connect To Any Pool In 5 Minutes, Reboot!");
                                ProgramLauncher.Execute("/usr/bin/reboot", "");
                            }
                        }
                        else
                        {
                            lastPoolAvailable = DateTime.UtcNow;
                        }
                    }
                    catch (Exception e)
                    {
                        Utility.Log(LogLevel.Error, e.ToString());
                    }
                    try
                    {
                        if (DateTime.UtcNow > _nextHostStatusTime)
                        {
                            _nextHostStatusTime = DateTime.UtcNow.AddMinutes(10);
                            if (!_hostConnected)
                            {
                                Utility.Log(LogLevel.Debug, "Creating Host Interface...");
                                _host = new HostInterface();
                                _hostConnected = true;
                                Utility.Log(LogLevel.Warning, "Host Interface Created.");
                                if (Config.Data.UniqueId == 0)
                                {
                                    long serverId = _host.GetUniqueId(_minerInfo);
                                    if (!serverId.Equals(Config.Data.SN))
                                    {
                                        Utility.Log(LogLevel.Warning, "Unique Id Updated to:{0}", serverId);
                                        Config.Data.UniqueId = serverId;
                                        _minerInfo.UniqueId = serverId;
                                        Config.SaveConfig();
                                    }
                                }
                                if ((Config.Data.SN == null) || (Config.Data.SN.Equals("")))
                                {
                                    string serverSN = _host.GetSN(_minerInfo);
                                    if (!serverSN.Equals(""))
                                    {
                                        Utility.Log(LogLevel.Warning, "SN Updated to:{0}", serverSN);
                                        Config.Data.SN = serverSN;
                                        _minerInfo.SN = serverSN;
                                        Config.SaveConfig();
                                    }
                                }
                            }
                            else
                            {
                                if (DateTime.UtcNow > _nextHostMaintCheckTime)
                                {
                                    _nextHostMaintCheckTime = DateTime.UtcNow.AddHours(2);
                                    _host.QueryMaintenanceTask(_minerInfo);
                                }
                            }
                            GetMinerStatus();
                            //							await HostApiClient.SubmitStatus (_minerStatDTO);
                        }
                    }
                    catch (Exception e)
                    {
                        _hostConnected = false;
                        _host = null;
                        Utility.Log(LogLevel.Error, e.ToString());
                    }
                }

                _hardware.Stop();
                _stopped = true;
            })).Start();

        }

        private static MiningPool _currentPool = null;
        private static DateTime _currentPoolSwitchTime = DateTime.UtcNow;

        public static MiningPool GetFirstAvailablePool()
        {
            foreach (var p in _pool)
            {
                if (p.OK)
                {
                    return p;
                }
            }
            return null;
        }

        public static MiningPool CurrentPool
        {
            get
            {
                if ((_currentPool != null) && (_currentPool.OK))
                {
                    if (DateTime.UtcNow > _currentPoolSwitchTime)
                    {
                        _currentPoolSwitchTime = DateTime.UtcNow.AddSeconds(30);
                        var firstavailablepool = GetFirstAvailablePool();
                        if ((firstavailablepool != null) && (firstavailablepool != _currentPool))
                        {
                            _currentPool = firstavailablepool;
                        }
                    }
                    return _currentPool;
                }
                else
                {
                    _currentPool = GetFirstAvailablePool();
                    return _currentPool;
                }
            }
        }

        private static bool GetNewBlock()
        {
            bool result = false;
            if (CurrentPool != null)
            {
                result = _currentPool.GetHashJob(ref _workCache[_workCacheWritePtr]);
                _workCache[_workCacheWritePtr].Pool = _currentPool;
            }
            return result;
        }

        private static void GetSpecificJob(UInt16 cachePtr, UInt16 blockPtr, ref HashData hashdata, bool copyMidState = true)
        {
            hashdata.JobId = _workCache[cachePtr].JobId;
            hashdata.ExtraNonce2 = _workCache[cachePtr].ExtraNonce2;
            hashdata.Pool = _workCache[cachePtr].Pool;
            Buffer.BlockCopy(_workCache[cachePtr].BlockHeaderBin, 0, hashdata.BlockHeaderBin, 0, 76);
            if (copyMidState)
            {
                Buffer.BlockCopy(_workCache[cachePtr].MidStateBin, 0, hashdata.MidStateBin, 0, 32);
            }
            hashdata.NTime = _workCache[cachePtr].NTime + blockPtr;
            hashdata.BlockHeaderBin[68] = (byte)(hashdata.NTime & 0x000000ff);
            hashdata.BlockHeaderBin[69] = (byte)((hashdata.NTime >> 8) & 0x000000ff);
            hashdata.BlockHeaderBin[70] = (byte)((hashdata.NTime >> 16) & 0x000000ff);
            hashdata.BlockHeaderBin[71] = (byte)((hashdata.NTime >> 24) & 0x000000ff);
            hashdata.UniqueId = (uint)((cachePtr << 16) + blockPtr);
        }

        private static bool GetHashJob(ref HashData hashdata)
        {
            lock (_workCacheLock)
            {
                if (_workCacheReadPtr == _workCacheWritePtr)
                {
                    if (GetNewBlock())
                    {
                        _workCacheWritePtr++;
                        _workCacheWritePtr %= WORK_CACHE_SIZE;
                    }
                }
                if (_workCacheReadPtr != _workCacheWritePtr)
                {
                    GetSpecificJob(_workCacheReadPtr, _workCacheReadBlockPtr, ref hashdata);
                    _workCacheReadBlockPtr++;
                    if (_workCacheReadBlockPtr >= MAX_HASH_DATA_NTIME_ROLL)
                    {
                        _workCacheReadBlockPtr = 0;
                        _workCacheReadPtr++;
                        _workCacheReadPtr %= WORK_CACHE_SIZE;
                    }
                    return true;
                }
            }
            return false;
        }

        private static void HandleHashResult(FDriver miner, byte board, byte core, UInt32 uniqueId, UInt32 nonce, UInt32 difficulty)
        {
            HashData hashdata = miner.RxHashData;
            if (difficulty > 0)
            {
                GetSpecificJob((UInt16)(uniqueId >> 16), (UInt16)(uniqueId & 0x0000ffff), ref hashdata, false);
                if ((hashdata.Pool != null) && (hashdata.Pool.OK))
                {
                    hashdata.Nonce = nonce;
                    byte[] hash = hashdata.MidStateBin;
                    hashdata.BlockHeaderBin[76] = (byte)(hashdata.Nonce >> 24 & 0xff);
                    hashdata.BlockHeaderBin[77] = (byte)(hashdata.Nonce >> 16 & 0xff);
                    hashdata.BlockHeaderBin[78] = (byte)(hashdata.Nonce >> 8 & 0xff);
                    hashdata.BlockHeaderBin[79] = (byte)(hashdata.Nonce >> 0 & 0xff);

                    Sha256LibManaged.DoubleSha256(hashdata.BlockHeaderBin, 80, hash);
                    double resultdifficulty = CalculateDifficulty(hash);
                    if (resultdifficulty >= hashdata.Pool.Difficulty)
                    {
                        hashdata.Pool.SubmitHashResult(hashdata);
                    }
                    else if (resultdifficulty < 1)
                    {
                        if (Utility.DebugMode)
                        {
                            Utility.Log(LogLevel.Debug, "Hardware Error: \n    Unique Id: {0:X8} \n    Block Header: {1}", new object[] {
                                uniqueId,
                                Utility.ByteArrayToHexString (hashdata.BlockHeaderBin)
                            });
                        }
                        miner.Status.Board[board].Chip[core / 4].Core[core % 4].HardwareError++;
                        miner.Status.Board[board].Chip[core / 4].Core[core % 4].NonceFound--;
                    }
                }
            }
        }

        private static void GetSingleHashData(FDriver miner, byte board, byte core, ref HashData hashData, out bool success)
        {
            if (CurrentPool != null)
            {
                success = GetHashJob(ref hashData);
            }
            else
            {
                success = false;
            }
        }

        private static void GetBlockHashData(FDriver miner, byte board, byte core, ref HashData hashData, out byte maxRoll, out bool refresh, out bool success)
        {
            maxRoll = MAX_HASH_DATA_NTIME_ROLL;
            refresh = _poolRefresh;
            _poolRefresh = false;
            if (CurrentPool != null)
            {
                lock (_workCacheLock)
                {
                    if (GetNewBlock())
                    {
                        hashData = _workCache[_workCacheWritePtr];
                        hashData.UniqueId = (uint)_workCacheReadPtr << 16;
                        _workCacheReadPtr++;
                        _workCacheReadPtr %= WORK_CACHE_SIZE;
                        _workCacheWritePtr++;
                        _workCacheWritePtr %= WORK_CACHE_SIZE;
                        success = true;
                        return;
                    }
                }
            }
            success = false;
        }

        public static void Stop()
        {
            Utility.Log(LogLevel.Info, "Stopping Mining Controller");
            _stopping = true;
            while (!_stopped)
            {
                Thread.Sleep(100);
            }
            Utility.Log(LogLevel.Info, "Mining Controller Stopped!");
        }

        private static DateTime _nextStatusTime = DateTime.UtcNow;

        public static MinerStatus GetMinerStatus()
        {
            lock (_minerStatusLock)
            {
                if (DateTime.UtcNow > _nextStatusTime)
                {
                    _nextStatusTime = DateTime.UtcNow.AddSeconds(5);
                    _minerStatus.Utc = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                    _minerStatus.MaxTempBoard = 0;
                    _minerStatus.MaxTempChip = 0;
                    _minerStatus.SpeedAvg = 0;
                    _minerStatus.SpeedCur = 0;
                    if (_hardware != null)
                    {
                        _minerStatus.MaxTempBoard = _hardware.TempBoardMax;
                        _minerStatus.MaxTempChip = _hardware.TempChipMax;
                        _minerStatus.SpeedAvg = _hardware.SpeedAvg;
                        _minerStatus.SpeedCur = _hardware.SpeedCur;
                        _minerStatus.NonceFound = _hardware.NonceFound;
                        _minerStatus.HardwareError = _hardware.HardwareError;
                        if (CurrentPool == null)
                        {
                            _minerStatus.CurrentPool = "No Pool Alive!";
                            _minerStatus.SpeedCur = 0;
                            _minerStatus.SpeedAvg = 0;
                        }
                        else
                        {
                            _minerStatus.CurrentPool = CurrentPool.Url;
                        }
                    }

                    _minerStatDTO.UpTime = _minerStatus.Utc;
                    _minerStatDTO.NonceFound = _minerStatus.NonceFound;
                    _minerStatDTO.HardwareError = _minerStatus.HardwareError;
                    _minerStatDTO.SpeedAvg = _minerStatus.SpeedAvg;
                    _minerStatDTO.SpeedCur = _minerStatus.SpeedCur;
                    _minerStatDTO.MaxTempBoard = (float)_minerStatus.MaxTempBoard;
                    _minerStatDTO.MaxTempChip = (float)_minerStatus.MaxTempChip;
                }
            }
            return _minerStatus;
        }

        public static MinerDetail GetMinerDetail()
        {
            GetMinerStatus();
            MinerDetail minerDetail = new MinerDetail();
            minerDetail.Status = _minerStatus;
            minerDetail.Hardware = new List<HardwareStatus>();
            minerDetail.Pool = new List<PoolStatus>();
            minerDetail.Hardware.Add(_hardware.Status);
            foreach (var p in _pool)
            {
                minerDetail.Pool.Add(new PoolStatus()
                {
                    Alive = p.OK,
                    Url = p.Url,
                    UserName = p.Url,
                    Difficulty = p.Difficulty,
                    Accepted = p.Accepted,
                    Rejected = p.Rejected,
                    Stale = p.Stale
                });
            }
            return minerDetail;
        }

        public static void Blink()
        {
            _hardware.Blink();
        }

        public static void ShowMessage(string msg)
        {
            _hardware.ShowMessage(msg);
        }

        private static readonly double[] bits = {
            1.0,
            18446744073709551616.0,
            340282366920938463463374607431768211456.0,
            6277101735386680763835789423207666416102355444464034512896.0,
        };

        private static double Be256ToDouble(byte[] hash)
        {
            UInt64 data64;
            double dcut64 = 0.0;
            int i, j, shift;
            for (j = 0; j < 4; j++)
            {
                shift = 0;
                data64 = 0;
                for (i = 0; i < 8; i++)
                {
                    data64 += (UInt64)hash[i + (j * 8)] << shift;
                    shift += 8;
                }
                dcut64 += (double)data64 * bits[j];
            }
            return dcut64;
        }

        private static double CalculateDifficulty(byte[] hash)
        {
            double dcut64;
            dcut64 = Be256ToDouble(hash);
            if (dcut64 == 0)
            {
                dcut64 = 1;
            }
            return 26959535291011309493156476344723991336010898738574164086137773096960.0 / dcut64;
        }
    }
}

