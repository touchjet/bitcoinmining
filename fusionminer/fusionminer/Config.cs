using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using FusionMiner.Thrift;
using System.Reflection;

namespace FusionMiner
{
	public static class Config
	{
		public const string HostApiUrl = "http://api.fusionminer.net/";

		public static bool NoDashAPI = false;
		public static bool PrintExtraDebug = false;
	
		private static FusionMiner.Thrift.MinerConfig _config;
		private const string _configFileName = "settings.xml";
		private static object _settingsLock = new object ();

		public static void SaveConfig ()
		{
			SaveConfig (_config);
		}

		public static void SaveConfig (MinerConfig config)
		{
			lock (_settingsLock) {
				XmlSerializer serializer = 
					new XmlSerializer (typeof(MinerConfig));
				using (TextWriter writer = new StreamWriter (_configFileName)) {
					serializer.Serialize (writer, config);
					writer.Flush ();
					writer.Close ();
				}
			}
			Utility.Log (LogLevel.Info, "Config Saved To File");
		}

		private static string GetLocalMAC ()
		{
			if (File.Exists (@"/sys/class/net/eth0/address")) {
				return System.IO.File.ReadAllText (@"/sys/class/net/eth0/address").Replace (":", "").Replace ("\n", "");
			} else {
				return "000000000000";
			}
		}

		public static MinerConfig Data { 
			get {
				if (_config == null) {
					lock (_settingsLock) {
						XmlSerializer serializer;
						if (File.Exists (_configFileName)) {
							try {
								serializer = new XmlSerializer (typeof(MinerConfig));
								using (FileStream fs = new FileStream (_configFileName, FileMode.Open)) {
									_config = (MinerConfig)serializer.Deserialize (fs);
								}
								string mac = GetLocalMAC ();
								if (!_config.MAC.Equals (mac)) {
									_config.MAC = mac;
									_config.UniqueId = 0;
									_config.SN = "";
								}
								_config.Version = Assembly.GetExecutingAssembly ().GetName ().Version.ToString ();
							} catch {
							}
						}
					}
				}
				if (_config == null) {
					string mac = GetLocalMAC ();
					_config = new MinerConfig ();
					_config.MAC = mac;
					_config.UniqueId = 0;
					_config.SN = "";
					_config.NickName = "FusionMiner";
					_config.Password = "fusionminer";

					_config.Version = Assembly.GetExecutingAssembly ().GetName ().Version.ToString ();
					_config.WiredNetwork = new NetworkConfig () {
						Enabled = true,
						DHCP = true,
					};
					_config.WirelessNetwork = new NetworkConfig () {
						Enabled = false,
					};

					_config.Hardwares = new List<HardwareConfig> ();
					_config.Hardwares.Add (new HardwareConfig () {
						Type = HardwareType.DigBigSPI,
						DeviceNumber = 0,
						DeviceName = @"SPI",
						Frequency = 400,
						Voltage = 90
					}); //0.9V

					_config.PoolStrategy = PoolStrategyType.FailOver;

					_config.Pools = new List<PoolConfig> ();
					_config.Pools.Add (new PoolConfig () {
						Type = PoolType.Stratum,
						Url = @"mining.eligius.st",
						Port = 3334,
						UserName = "",
						Password = "",
					});
					SaveConfig ();
				}
				return _config;
			}
		}
	}
}

