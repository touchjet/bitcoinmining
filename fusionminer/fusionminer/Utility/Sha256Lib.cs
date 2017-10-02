using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace FusionMiner
{
	public static class Sha256LibManaged
	{
		private static SHA256Extended _sha256 = new SHA256Extended ();

		public static void DoubleSha256 (byte[] inputdata, int len, byte[] result)
		{
			_sha256.DoubleSha256 (inputdata, len, result);
		}

		public static void CalcMidstate (byte[] inputdata, byte[] result)
		{
			_sha256.ComputeMidstate (inputdata, result);
		}
	}
}

