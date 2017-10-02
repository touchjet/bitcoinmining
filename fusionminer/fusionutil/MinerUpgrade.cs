using System;
using System.Collections.Concurrent;
using FusionMiner;
using System.Threading;
using FSM.DotNetSSH;

namespace fusionutil
{
	public static class MinerUpgrade
	{
		public static ConcurrentQueue<string> miners;

		public static void SearchMiner ()
		{
			Utility.Log (LogLevel.Warning, "Searching for all local miners, it will take 20 seconds...");
			miners = new ConcurrentQueue<string> ();
			string localIp = Utility.LocalIPAddress ();
			localIp = localIp.Remove (localIp.LastIndexOf ('.') + 1);
			for (int i = 0; i < 256; i++) {
				new Thread (new ThreadStart (() => {
					try {
						string ip = localIp + i.ToString ();
						var miner = new SimpleMinerIntf (ip);
						if (miner.Ping ()) {
							miners.Enqueue (ip);
						}
					} catch {
					}
				})).Start ();
			}
			Thread.Sleep (15000);
		}

		public static void UpgradeAllLocalMiners ()
		{
			int count = 0;
			SearchMiner ();
			string localIp = Utility.LocalIPAddress ();
			string ip;
			while (!miners.IsEmpty) {
				if (miners.TryDequeue (out ip)) {
					if (!ip.Equals (localIp)) {
						UpgradeLocalMiner (ip);
						count++;
					}
				}
			}
			Utility.Log (LogLevel.Warning, "{0} Miners Upgraded.", count);
		}

		public static void UpgradeLocalMiner (string ip)
		{
			Utility.Log (LogLevel.Warning, "Upgrading Miner: {0}", ip);
			try {
				SshTransferProtocolBase sshCp;

				sshCp = new Scp (ip, "root");

				sshCp.Password = "";
				sshCp.OnTransferStart += sshCp_OnTransferStart;
				sshCp.OnTransferProgress += sshCp_OnTransferProgress;
				sshCp.OnTransferEnd += sshCp_OnTransferEnd;

				Console.Write ("Connecting...");
				sshCp.Connect ();
				Console.WriteLine ("OK");

				sshCp.Put ("fusionminer/fusionminer.exe", "/usr/local/bin/fusionminer.exe");
				Console.Write ("Disconnecting...");
				sshCp.Close ();
				Console.WriteLine ("OK");
			} catch (Exception e) {
				Console.WriteLine (e.Message);
			}
		}

		public static void Reboot (string ip)
		{
			try {
				var exec = new SshExec (ip, "root");
				exec.Password = "w6f4j5xp2R4qysHt7777";

				Console.Write ("Connecting...");
				exec.Connect ();
				Console.WriteLine ("OK");
				exec.RunCommand ("reboot");
				Console.Write ("Disconnecting...");
				exec.Close ();
				Console.WriteLine ("OK");
			} catch (Exception e) {
				Console.WriteLine (e.Message);
			}
		}

		private static void sshCp_OnTransferStart (string src, string dst, int transferredBytes, int totalBytes, string message)
		{
			Console.WriteLine ("Start transfer {0} to {1}", src, dst);
		}

		private static void sshCp_OnTransferProgress (string src, string dst, int transferredBytes, int totalBytes, string message)
		{
			Console.Write (".");
		}

		private static void sshCp_OnTransferEnd (string src, string dst, int transferredBytes, int totalBytes, string message)
		{
			Console.WriteLine ("");
		}
	}

}

