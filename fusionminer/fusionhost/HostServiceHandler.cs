using System;
using FusionMiner.Thrift;
using FusionMiner;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace fusionhost
{
	public class HostServiceHandler : HostService.Iface
	{
		private const string cs = @"server=localhost;userid=fusionminer;password=HQcU8js3nftR8f2h;database=fusionminer";
		private object upgradeLock = new object ();

		private Dictionary<long, string> miners = new Dictionary<long, string>();
		private long idCounter = 100000;
		public string Ping ()
		{
			Utility.Log (LogLevel.Info, "Ping");
			return "Pong";
		}

		public string GetSN (MinerInfo minerInfo)
		{
			Utility.Log (LogLevel.Info, "GetSN {0} {1}", minerInfo.MAC, minerInfo.NickName);
			string result = "";
			MySqlConnection conn = null;
			try {
				conn = new MySqlConnection (cs);
				conn.Open ();
				var stm = String.Format ("select scatter from miner_scatter where id='{0}'", minerInfo.UniqueId); 
				using (MySqlCommand cmd = new MySqlCommand (stm, conn)) {
					result = (string)cmd.ExecuteScalar ();
				}
			} catch (Exception ex) {
				Utility.Log (LogLevel.Error, "Error: {0}", ex.ToString ());
			} finally {          
				if (conn != null) {
					conn.Close ();
				}
			}
			Utility.Log (LogLevel.Info, "SN={0}", result);
			return result;
		}

		public long GetUniqueId (MinerInfo minerInfo)
		{
			long result = 0;
			MySqlConnection conn = null;
			try {
				conn = new MySqlConnection (cs);
				conn.Open ();
				string stm = String.Format ("select count(*) from miner where mac='{0}'", minerInfo.MAC);   
				using (MySqlCommand cmd = new MySqlCommand (stm, conn)) {
					UInt64 count = Convert.ToUInt64 (cmd.ExecuteScalar ());
					if (count == 0) {
						cmd.CommandText = String.Format ("INSERT INTO miner (MAC,NickName,Version,LastStatusTime) VALUES ('{0}', '{1}','{2}',CURRENT_TIME())", minerInfo.MAC, minerInfo.NickName, minerInfo.Version);
						cmd.ExecuteNonQuery ();
					}
					cmd.CommandText = String.Format ("select id from miner where mac='{0}'", minerInfo.MAC); 
					result = Convert.ToInt64 (cmd.ExecuteScalar ());
				}
				Utility.Log (LogLevel.Info, result.ToString ());

			} catch (Exception ex) {
				Utility.Log (LogLevel.Error, "Error: {0}", ex.ToString ());

			} finally {          
				if (conn != null) {
					conn.Close ();
				}
			}
			return result;
		}

		public Maintenance QueryMaintenanceTask (long uniqueId, string version, string key)
		{
			Utility.Log (LogLevel.Info, "CheckUpgrade {0} Ver:{1}", uniqueId, version);
			Maintenance result;
			MySqlConnection conn = null;
			string mac = "";
			lock (upgradeLock) {
				try {
					using (conn = new MySqlConnection (cs)) {
						conn.Open ();
						string stm = "update miner set LastMaintCheck=@LastMaintCheck, Version=@Version where Id=@Id";
						using (MySqlCommand cmd = new MySqlCommand (stm, conn)) {
							cmd.Parameters.Add (new MySqlParameter ("Id", uniqueId));
							cmd.Parameters.Add (new MySqlParameter ("LastMaintCheck", DateTime.UtcNow));
							cmd.Parameters.Add (new MySqlParameter ("Version", version));
							cmd.ExecuteNonQuery ();
							cmd.CommandText = "select MAC from miner where Id=@Id";
							cmd.Parameters.Clear ();
							cmd.Parameters.Add (new MySqlParameter ("Id", uniqueId));
							mac = Convert.ToString (cmd.ExecuteScalar ());
						}
						conn.Close ();
					}
				} catch (Exception ex) {
					Utility.Log (LogLevel.Error, "Error: {0}", ex.ToString ());
				}
				result = UpgradePacker.GetMaintenancePack (version, mac);
			}
			return result;
		}

		private static DateTime _nextCleanUpTime = DateTime.Now;

		public void SubmitHostStatus (MinerHostStatus status)
		{
			Utility.Log (LogLevel.Info, "SubmitStatus {0} MHS:{1} Temp:{2}", new object[] {
				status.UniqueId,
				status.SpeedCur,
				status.MaxTempChip,
			});

			MySqlConnection conn = null;
			try {
				using (conn = new MySqlConnection (cs)) {
					conn.Open ();
					MySqlTransaction trans;
					trans = conn.BeginTransaction ();
					string stm = "insert into miner_status (Miner_Id,Status_Time,Up_Time,Average_Speed,Current_Speed,NonceFound,HardwareError,Board_Temp,Chip_Temp) values (@Miner_Id,@Status_Time,@Up_Time,@Average_Speed,@Current_Speed,@NonceFound,@HardwareError,@Board_Temp,@Chip_Temp)";   
					using (MySqlCommand cmd = new MySqlCommand (stm, conn, trans)) {
						cmd.Parameters.Add (new MySqlParameter ("Miner_Id", status.UniqueId));
						cmd.Parameters.Add (new MySqlParameter ("Status_Time", DateTime.UtcNow));
						cmd.Parameters.Add (new MySqlParameter ("Up_Time", status.Utc));
						cmd.Parameters.Add (new MySqlParameter ("Average_Speed", status.SpeedAvg));
						cmd.Parameters.Add (new MySqlParameter ("Current_Speed", status.SpeedCur));
						cmd.Parameters.Add (new MySqlParameter ("NonceFound", status.NonceFound));
						cmd.Parameters.Add (new MySqlParameter ("HardwareError", status.HardwareError));
						cmd.Parameters.Add (new MySqlParameter ("Board_Temp", status.MaxTempBoard));
						cmd.Parameters.Add (new MySqlParameter ("Chip_Temp", status.MaxTempChip));
						cmd.ExecuteNonQuery ();

						cmd.CommandText = "update miner set LastStatusTime=@LastStatusTime, LocalIP=@LocalIP, InternetIP=@InternetIP, LastStatusId=LAST_INSERT_ID() where Id=@Id";
						cmd.Parameters.Clear ();
						cmd.Parameters.Add (new MySqlParameter ("Id", status.UniqueId));
						cmd.Parameters.Add (new MySqlParameter ("LastStatusTime", DateTime.UtcNow));
						cmd.Parameters.Add (new MySqlParameter ("LocalIP", status.LocalIP == null ? "" : status.LocalIP));
						cmd.Parameters.Add (new MySqlParameter ("InternetIP", status.RemoteIP == null ? "" : status.RemoteIP));
						cmd.ExecuteNonQuery ();

						if (DateTime.Now > _nextCleanUpTime) {
							Utility.Log (LogLevel.Warning, "Clean Up Miner Status Table");
							_nextCleanUpTime = DateTime.Now.AddHours (24);
							cmd.CommandText = "delete from miner_status where Status_Time < @Status_Time";
							cmd.Parameters.Clear ();
							cmd.Parameters.Add (new MySqlParameter ("Status_Time", DateTime.UtcNow.AddDays (-60)));
							cmd.ExecuteNonQuery ();
							Utility.Log (LogLevel.Warning, "Finished Clean Up Miner Status Table");
						}
					}
					trans.Commit ();
					conn.Close ();
				}
			} catch (Exception ex) {
				Utility.Log (LogLevel.Error, "Error: {0}", ex.ToString ());

			}
		}
	}
}

