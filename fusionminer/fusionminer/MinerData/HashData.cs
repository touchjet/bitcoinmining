using System;

namespace FusionMiner
{
	public class HashData
	{
		public uint UniqueId;
		public string JobId;
		public string ExtraNonce2;
		public uint NTime;
		public byte[] BlockHeaderBin = new byte[80];
		public byte[] MidStateBin = new byte[32];
		public uint Nonce;
		public MiningPool Pool;
	}
}

