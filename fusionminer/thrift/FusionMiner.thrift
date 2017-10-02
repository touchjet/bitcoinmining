namespace csharp FusionMiner.Thrift

enum HardwareType {
  DigBigSerial,
  DigBigSPI,
  DigBigSPIV2,
}

enum MaintStepType {
	AddFile,
	RemoveFile,
	MinerCommand,
	SystemCommand,
}

enum PoolType {
  Test,
  Stratum,
  GetWork,
}

enum PoolStrategyType {
  FailOver,
  LoadBalance,
}

/* Maintenance Data Structures */
struct MaintStep {
  1: MaintStepType Type,
  2: string Path,
  3: string Command,
  4: binary FileData,
  5: string md5,
}

struct Maintenance {
  1: string SoftwareVersion,
  2: list<MaintStep> Steps,
  3: string Checksum,
}
/*******************************/

/* Miner Information */
struct MinerInfo {
  1: string MAC,
  2: i64 UniqueId,
  3: string NickName,
  4: string Version,
  5: string SN,
}
/*******************************/

/* Miner Status */
struct CoreStatus {
  1: bool Available,
  2: i64 NonceFound,
  3: i64 NonceNotFound,
  4: i64 HardwareError,
}

struct ChipStatus {
  1: list<CoreStatus> Core,
  2: double Temperature,
}

struct BoardStatus {
  1: list<ChipStatus> Chip,
  2: double Temperature,
}

struct HardwareStatus {
  1: i32 DeviceNumber,
  2: string DeviceName,
  3: list<BoardStatus> Board,
  4: i32 Difficulty,
}

struct PoolStatus {
  1: string Url,
  2: string UserName,
  3: bool Alive,
  4: i64 Accepted,
  5: i64 Rejected,
  6: i64 Stale,
  7: double Difficulty,
}

struct MinerStatus {
  1: MinerInfo Miner,
  2: i32 Utc,
  3: i64 NonceFound,
  4: i64 HardwareError,
  5: i32 SpeedAvg,
  6: i32 SpeedCur,
  7: double MaxTempBoard,
  8: double MaxTempChip,
  9: string CurrentPool,
}

struct MinerDetail {
  1: MinerStatus Status,
  2: list<HardwareStatus> Hardware,
  3: list<PoolStatus> Pool,
}

struct MinerHostStatus {
  1: i64 UniqueId,
  2: i32 Utc,
  3: i64 NonceFound,
  4: i64 HardwareError,
  5: i32 SpeedAvg,
  6: i32 SpeedCur,
  7: double MaxTempBoard,
  8: double MaxTempChip,
  9: string LocalIP,
  10: string RemoteIP,
}


/*******************************/

/* Configurations */
struct PoolConfig {
  1: PoolType Type,
  2: string Url,
  3: i32 Port,
  4: string UserName,
  5: string Password,
  6: i32 Quota,
}

struct HardwareConfig {
  1: HardwareType Type,
  2: i32 DeviceNumber,
  3: string DeviceName,
  4: i32 Frequency,
  5: i32 Voltage,
}

struct NetworkConfig {
  1: bool Enabled,
  2: bool DHCP,
  3: string IP,
  4: string SubnetMask,
  5: string Router,
  6: string DNS1,
  7: string DNS2,
  8: string SSID,
  9: string Security,
  10: string Password,
}

struct MinerConfig {
  1: string MAC,
  2: i64 UniqueId,
  3: string NickName,
  4: string Version,
  5: list<PoolConfig> Pools,
  6: PoolStrategyType PoolStrategy,
  7: list<HardwareConfig> Hardwares,
  8: NetworkConfig WiredNetwork,
  9: NetworkConfig WirelessNetwork,
  10: string Password,
  11: string SN,
}

/*******************************/

service HostService {
  string Ping(),
  i64 GetUniqueId(1: MinerInfo minerInfo),
  string GetSN(1: MinerInfo minerInfo),
  Maintenance QueryMaintenanceTask(1: i64 uniqueId, 2: string version, 3: string key),
  void SubmitHostStatus(1: MinerHostStatus status),
}

service MinerService {
  string Ping(),
  bool Validate(1: string password),
  MinerStatus GetStatus(),
  MinerDetail GetDetail(),
  MinerConfig GetConfig(),
  void SetConfig(1: MinerConfig config),
  void Blink(),
  string GetLog(),
  void Shutdown(),
}
