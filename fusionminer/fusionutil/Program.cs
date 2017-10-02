using System;
using FSM.DotNetSSH;

namespace fusionutil
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args [0].Equals ("upgradeall")) {
				MinerUpgrade.UpgradeAllLocalMiners ();
			} else if (args [0].Equals ("upgrade")) {
				MinerUpgrade.UpgradeLocalMiner (args [1]);
			} else if (args [0].Equals ("list")) {
				MinerList.ListMiner ();
			}
		}

	}
}
