using System;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;
using FusionMiner.Thrift;
using System.Collections.Generic;

namespace fusiondash
{
	public class MinerInterface
	{
		private TSocket _transport;
		private TBinaryProtocol _protocol;
		private MinerService.Client _client;
		private object _minerLock = new object ();
		private static MinerInterface _instance;

		public MinerInterface (string ip="127.0.0.1")
		{
			_transport = new TSocket (ip, 2087);
			_transport.Timeout = 600;
			_protocol = new TBinaryProtocol (_transport);
			_client = new MinerService.Client (_protocol);
		}

		public void Blink ()
		{
			lock (_minerLock) {
				try {
					_transport.Open ();
					_client.Blink ();
				} finally {
					_transport.Close ();
				}
			}
		}

		public string Ping ()
		{
			lock (_minerLock) {
				string result = "";
				try {
					_transport.Open ();
					result = _client.Ping ();
				} finally {
					_transport.Close ();
				}
				return result;
			}
		}

		public void Shutdown ()
		{
			lock (_minerLock) {
				try {
					_transport.Open ();
					_client.Shutdown ();
				} finally {
					_transport.Close ();
				}
			}
		}

		public bool Validate (string password)
		{
			lock (_minerLock) {
				bool result = true;
				try {
					_transport.Open ();
					result = _client.Validate (password);
				} catch {
				} finally {
					_transport.Close ();
				}
				return result;
			}
		}

		public MinerStatus GetStatus ()
		{
			MinerStatus result=null;
			lock (_minerLock) {
				try {
					_transport.Open ();
					result = _client.GetStatus ();
					_transport.Close ();
				} catch {
				}
				return result;
			}
		}

		public MinerDetail GetDetail ()
		{
			MinerDetail result;
			lock (_minerLock) {
				try {
					_transport.Open ();
					result = _client.GetDetail ();
					_transport.Close ();
				} catch {
					result = GetDummyDetail ();
				}
				return result;
			}
		}

		public MinerConfig GetConfig ()
		{
			MinerConfig result=null;
			lock (_minerLock) {
				try {
					_transport.Open ();
					result = _client.GetConfig ();
					_transport.Close ();
				} catch {
				}
				return result;
			}
		}

		public void SetConfig (MinerConfig config)
		{
			lock (_minerLock) {
				try {
					_transport.Open ();
					_client.SetConfig (config);
				} catch (Exception e){
					Console.WriteLine (e.ToString());
				} finally {
					_transport.Close ();
				}
			}
		}

		public static MinerInterface Instance {
			get {
				if (_instance == null) {
					_instance = new MinerInterface ();
				}
				return _instance;
			}
		}

		private static MinerStatus GetDummyStatus ()
		{
			MinerStatus result;
			result = new MinerStatus ();
			result.Miner = new MinerInfo (){ NickName = "OffLine", MAC = "", UniqueId = 0, Version = "0" };
			result.CurrentPool = "";
			result.Utc = 0;
			return result;
		}

		private static MinerDetail GetDummyDetail ()
		{
			MinerDetail result;
			result = new MinerDetail ();
			result.Status = GetDummyStatus ();
			return result;
		}

		private MinerConfig GetDummyConfig ()
		{
			MinerConfig result;
			result = new MinerConfig ();
			result.Hardwares = new List<HardwareConfig> ();
			result.Pools = new List<PoolConfig> ();
			result.NickName = "OffLine";
			result.WiredNetwork = new NetworkConfig ();
			result.WirelessNetwork = new NetworkConfig ();
			return result;
		}
	}
}

