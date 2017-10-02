using System;
using System.Threading;
using Thrift.Transport;
using Thrift.Protocol;
using FusionMiner.Thrift;
using System.Reflection;
using System.IO;
using System.Net;

namespace FusionMiner
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			try {
				if (File.Exists ("/var/log/fusionminer.bak")) {
					File.Delete ("/var/log/fusionminer.bak");
				}
				if (File.Exists ("/var/log/fusionminer.log")) {
					File.Move ("/var/log/fusionminer.log", "/var/log/fusionminer.bak");
				}
				if (File.Exists ("/var/log/httpd/access_log")) {
					File.Delete ("/var/log/httpd/access_log");
				}
				if (File.Exists ("/var/log/httpd/error_log")) {
					File.Delete ("/var/log/httpd/error_log");
				}
			} catch {
			}

			foreach (var s in args) {
				if (s.ToLower ().Equals ("debug")) {
					Utility.DebugMode = true;
				} else if (s.ToLower ().Equals ("silent")) {
					Utility.SilentMode = true;
				} else if (s.ToLower ().Equals ("nodashapi")) {
					Config.NoDashAPI = true;
				}
			}
			Utility.Log (LogLevel.Warning, "Starting FusionMiner {0}", Config.Data.Version);
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
			MiningController.Start ();

			while (true) {
				ConsoleKeyInfo key = Console.ReadKey ();
				if (key.Key == ConsoleKey.P) {
					Config.PrintExtraDebug = !Config.PrintExtraDebug;
				}
				Thread.Sleep (1000);
			}
		}

	}
}
