using System;
using System.Collections.Generic;

namespace FusionMiner.Domain.DTO
{
	public class MinerMaintPackDTO
	{
		public string SecurityToken { get; set; }

		public List<MaintStepDTO> Step { get; set; }
	}

	public class MaintStepDTO
	{
		public MaintStepType MaintType { get; set; }

		public string Path { get; set; }

		public string Command { get; set; }

		public string Extra { get; set; }

		public byte[] FileData { get; set; }

		public string MD5 { get; set; }
	}

	public enum MaintStepType
	{
		AddFile = 0,
		RemoveFile = 1,
		MinerCommand = 2,
		SystemCommand = 3,
		UpdateConfig = 4,
	}
}

