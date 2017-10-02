using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using FusionMiner.Thrift;
using System.Net.Sockets;

namespace fusiondash
{
	public class MinerSearch
	{
		public static  ConcurrentDictionary<string,MinerStatus> Miners = new ConcurrentDictionary<string, MinerStatus> ();

		public static ConcurrentDictionary<string,MinerConfig> MinersConfig = new ConcurrentDictionary<string, MinerConfig> ();

		private static object _minersearchlock = new object ();

		private static DateTime _nextSearch = DateTime.UtcNow;

		public static string LocalIPAddress ()
		{
			IPHostEntry host;
			string localIP = "";
			try {
				host = Dns.GetHostEntry (Dns.GetHostName ());
				foreach (IPAddress ip in host.AddressList) {
					if (ip.AddressFamily == AddressFamily.InterNetwork) {
						localIP = ip.ToString ();
						break;
					}
				}
			} catch {
			}
			return localIP;
		}

		public static void SearchMiner (bool withConfig = false)
		{
			long counter = 0;
			if ((!withConfig) && (DateTime.UtcNow < _nextSearch)) {
				return;
			}
			_nextSearch = DateTime.UtcNow.AddSeconds (30);
			lock (_minersearchlock) {
				Miners.Clear ();
				MinersConfig.Clear ();
				string localIp = LocalIPAddress ();
				localIp = localIp.Remove (localIp.LastIndexOf ('.') + 1);
				for (int i = 0; i < 32; i++) {
					new Thread (new ParameterizedThreadStart ((n) => {
						for (int j = 0; j < 8; j++) {
							try {
								string ip = localIp + (((int)n * 8) + j).ToString ();
								var miner = new MinerInterface (ip);
								var stat = miner.GetStatus ();
								if ((stat != null) && (!stat.Miner.NickName.Equals ("OffLine"))) {
									Miners.TryAdd (ip, stat);
									if (withConfig) {
										var config = miner.GetConfig ();
										if (config != null) {
											MinersConfig.TryAdd (ip, config);
										}
									}
								}
							} catch {
							}
						}
						Interlocked.Increment (ref counter);
					})).Start (i);
				}
				int sleepCounter = 0;
				while ((Interlocked.Read (ref counter) < 32) && (sleepCounter < 200)) {
					Thread.Sleep (100);
					sleepCounter++;
				}
			}
		}
	}
}

