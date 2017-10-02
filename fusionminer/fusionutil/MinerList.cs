using System;
using System.Collections.Concurrent;
using FusionMiner;
using System.Linq;
using FusionMiner.Thrift;
using System.IO;
using fusiondash;

namespace fusionutil
{
	public static class MinerList
	{
		public static void ListMiner ()
		{
			Utility.Log (LogLevel.Warning, "Searching for all local miners...");
			MinerSearch.SearchMiner ();
			var items = from pair in MinerSearch.Miners
			            orderby pair.Value.Miner.UniqueId
			            select pair;
			int counter = 1;
			try {
				using (StreamWriter w = File.CreateText ("miners.csv")) {
					w.WriteLine ("No,IP,Version,UniqueId,SN,Speed");
					foreach (var m in items) {
						Console.WriteLine ("{0} - [{1}]   {2}   {3}    {4}", counter, m.Value.Miner.UniqueId, m.Key, m.Value.Miner.Version, m.Value.SpeedCur);
						w.WriteLine ("{0},{1},{2},{3},{4},{5}", counter, m.Key, m.Value.Miner.Version, m.Value.Miner.UniqueId, m.Value.Miner.SN, m.Value.SpeedCur);
						counter++;
					}
					w.Close ();
				}
			} catch {
			}
		}
	}
}

