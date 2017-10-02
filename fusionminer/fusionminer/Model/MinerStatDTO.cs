using System;

namespace FusionMiner.Domain.DTO
{
	public class MinerStatDTO
	{
		public MinerDTO Miner { get; set; }
		public int UpTime { get; set; }
		public long NonceFound { get; set; }
		public long HardwareError { get; set; }
		public int SpeedAvg { get; set; }
		public int SpeedCur { get; set; }
		public float MaxTempBoard { get; set; }
		public float MaxTempChip { get; set; }
		public string LocalIP { get; set; }
		public string PoolUrl { get; set; }
		public string PoolUser { get; set; }
	}
}

