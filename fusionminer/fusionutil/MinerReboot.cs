using System;
using System.Linq;
using FusionMiner;
using fusiondash;

namespace fusionutil
{
	public class MinerReboot
	{
		public static void RebootMiner ()
		{
			Utility.Log (LogLevel.Warning, "Searching for all local miners...");
			MinerSearch.SearchMiner (true);
			var items = from pair in MinerSearch.MinersConfig
			            orderby pair.Key
			            select pair;
			foreach (var m in items) {
				try {
					Utility.Log(LogLevel.Warning,"Rebooting {0}",m.Key);
					MinerInterface miner = new MinerInterface(m.Key);
					miner.SetConfig(m.Value);
				} catch {
				}
			}
		}
	}
}

