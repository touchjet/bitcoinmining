using System;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Collections.Generic;

namespace fusionweb
{
	public class MinerStatReport
	{
		private const string cs = @"server=localhost;userid=fusionminer;password=;database=fusionminer";

		public IEnumerable<MinerStatReportRow> AllMinersCurrentStat ()
		{
			using (MySqlConnection conn = new MySqlConnection (cs))
			using (MySqlConnection connstatus = new MySqlConnection (cs))
				try {
					conn.Open ();
					connstatus.Open ();

					using (MySqlCommand cmd = new MySqlCommand ())
					using (MySqlCommand cmdstatus = new MySqlCommand ()) {
						cmd.Connection = conn;
						cmdstatus.Connection = connstatus;
						cmdstatus.CommandText = "select * from miner_status where Id=@Id";
						cmdstatus.Parameters.Add ("Id", MySqlDbType.Int64);
						cmd.CommandText = "SELECT m . * , s.scatter FROM  `miner` m LEFT JOIN  `miner_scatter` s ON s.id = m.Id where m.LastStatusTime > @StatusTime";
						cmd.Parameters.Add (new MySqlParameter ("StatusTime", DateTime.UtcNow.AddMinutes (-8)));
						using (var reader = cmd.ExecuteReader ()) {
							while (reader.Read ()) {
								cmdstatus.Parameters [0].Value = reader.GetInt64 ("LastStatusId");
								using (var readerstatus = cmdstatus.ExecuteReader ()) {
									while (readerstatus.Read ()) {
										MinerStatReportRow row = new MinerStatReportRow ();
										row.UniqueId = reader.GetInt64 ("Id");
										row.SN = reader.GetString ("scatter");
										row.NickName = reader.GetString ("NickName");
										row.Version = reader.GetString ("Version");
										row.LastMaintTime = reader.GetDateTime ("LastMaintCheck");
										row.LastStatusTime = reader.GetDateTime ("LastStatusTime");
										row.UpTime = new TimeSpan (0, 0, readerstatus.GetInt32 ("Up_Time"));
										row.LocalIp = reader.GetString ("LocalIP");
										row.InternetIp = reader.GetString ("InternetIp");
										row.CurrentSpeed = readerstatus.GetInt64 ("Current_Speed");
										row.AverageSpeed = readerstatus.GetInt32 ("Average_Speed");
										row.Accepted = readerstatus.GetInt64 ("NonceFound");
										row.HardwareError = readerstatus.GetInt64 ("HardwareError");
										yield return row;
									}
								}
							}
						}
						conn.Close ();
						connstatus.Close ();
					}
				} finally {          
				}

		}
	}
}

