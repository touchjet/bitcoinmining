using System;
using System.Threading;
using System.IO;
using FusionMiner.Thrift;

namespace FusionMiner
{
	public class MinerServiceHandler : MinerService.Iface
	{
		public string Ping ()
		{
			return Utility.LocalIPAddress ();
		}

		public bool Validate (string password)
		{
			return Config.Data.Password.Equals (password);
		}

		public MinerStatus GetStatus ()
		{
			Utility.Log (LogLevel.Debug, "MinerService.GetStatus");
			return MiningController.GetMinerStatus ();
		}

		public MinerDetail GetDetail ()
		{
			Utility.Log (LogLevel.Debug, "MinerService.GetDetail");
			return MiningController.GetMinerDetail ();
		}

		public MinerConfig GetConfig ()
		{
			Utility.Log (LogLevel.Debug, "MinerService.GetConfig");
			return Config.Data;
		}

		public void SetConfig (MinerConfig config)
		{
			Utility.Log (LogLevel.Debug, "MinerService.SetConfig");
			new Thread (new ThreadStart (() => {
				try {
					if ((config.NickName != null) && (config.NickName.Length > 0) && (!Config.Data.NickName.Equals (config.NickName))) {
						Utility.Log (LogLevel.Warning, "Set NickName to {0}", config.NickName);
						Config.Data.NickName = config.NickName;
						try {
							using (StreamWriter outfile = new StreamWriter (@"/etc/hostname", false)) {
								outfile.WriteLine (config.NickName);
								outfile.Close ();
							}
						} catch (Exception e) {
							Utility.Log (LogLevel.Debug, e.ToString ());
						}
					}
					if ((config.Password != null) && (config.Password.Length > 0)) {
						Utility.Log (LogLevel.Warning, "Set Password to {0}", config.Password);
						Config.Data.Password = config.Password;
					}
					if ((config.Hardwares != null) && (config.Hardwares.Count > 0)) {
						Config.Data.Hardwares.ForEach (h => h.Frequency = config.Hardwares [0].Frequency);
						Config.Data.Hardwares.ForEach (h => h.Voltage = config.Hardwares [0].Voltage);
						Utility.Log (LogLevel.Info, "Set Frequency To: {0}", Config.Data.Hardwares [0].Frequency);
						Utility.Log (LogLevel.Info, "Set Voltage To: {0}", Config.Data.Hardwares [0].Voltage);
					}
					if ((config.Pools != null) && (config.Pools.Count > 0)) {
						Config.Data.Pools = config.Pools;
					}
//					Config.Data.PoolStrategy = config.PoolStrategy;
					if (config.WiredNetwork != null) {
						Config.Data.WiredNetwork = config.WiredNetwork;
						string cstr;
						if (config.WiredNetwork.DHCP) {
							Utility.Log (LogLevel.Info, "Set Wired Network to DHCP");
							Config.Data.WiredNetwork.IP = "";
							Config.Data.WiredNetwork.SubnetMask = "";
							Config.Data.WiredNetwork.Router = "";
							Config.Data.WiredNetwork.DNS1 = "";
							Config.Data.WiredNetwork.DNS2 = "";
							cstr = "Description='DHCP ethernet connection'\nInterface=eth0\nConnection=ethernet\nIP=dhcp\nTimeoutDHCP=30";
						} else {
							Utility.Log (LogLevel.Info, "Set Wired Network to Static IP:{0}, Router:{1}, DNS:{2}", config.WiredNetwork.IP, config.WiredNetwork.Router, config.WiredNetwork.DNS1);
							int maskbit = 24;
							cstr = String.Format ("Description='Static ethernet connection'\nInterface=eth0\nConnection=ethernet\nIP=static\nAddress=('{0}/{1}')\nGateway='{2}'\nDNS=('{3}')", config.WiredNetwork.IP, maskbit, config.WiredNetwork.Router, config.WiredNetwork.DNS1);
						}
						try {
							using (StreamWriter outfile = new StreamWriter (@"/etc/netctl/eth0", false)) {
								outfile.WriteLine (cstr);
								outfile.Close ();
							}
						} catch (Exception e) {
							Utility.Log (LogLevel.Debug, e.ToString ());
						}
					}
//					Config.Data.WirelessNetwork = config.WirelessNetwork;
					Config.SaveConfig ();
					for (int i = 0; i < 10; i++) {
						MiningController.ShowMessage ("Rebooting...");
						Thread.Sleep (2000);
					}
					Utility.Log (LogLevel.Info, "Reboot");
					ProgramLauncher.Execute ("/usr/bin/reboot", "");
				} catch (Exception e) {
					Utility.Log (LogLevel.Error, e.ToString ());
				}
			})).Start ();
		}

		public void Blink ()
		{
			Utility.Log (LogLevel.Debug, "MinerService.Blink");
			MiningController.Blink ();
		}

		public void Shutdown ()
		{
			MiningController.ShowMessage ("Shutting Down...");
			Thread.Sleep (5000);
			ProgramLauncher.Execute ("/usr/bin/shutdown", "-h now");
		}

		public string GetLog ()
		{
			Utility.Log (LogLevel.Debug, "MinerService.GetLog");
			string result = "";
			try {
				if (File.Exists ("/var/log/fusionminer.log")) {
					result = File.ReadAllText ("/var/log/fusionminer.log");
				}
			} catch {
			}
			return result;
		}
	}
}

