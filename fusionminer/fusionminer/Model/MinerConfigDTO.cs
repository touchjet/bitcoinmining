using System;
using System.Collections.Generic;

namespace FusionMiner.Domain.DTO
{
	public class MinerConfigDTO
	{
		public string SecurityToken { get; set; }

		public string Password { get; set; }

		public List<HardwareConfigDTO> HardwareConfig { get; set; }

		public List<PoolConfigDTO> PoolConfig { get; set; }
	}

	public class HardwareConfigDTO
	{
		public MinerModel Model { get; set; }

		public int Frequency { get; set; }

		public int Voltage { get; set; }
	}

	public class PoolConfigDTO
	{
		public string Url { get; set; }

		public string UserName { get; set; }

		public string Password { get; set; }
	}
	public enum MinerModel
	{
		Digbig1T = 0,
		Digbig3T = 1,
	}
}

