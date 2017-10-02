using MySql.Data.MySqlClient;
using System.Web.UI.WebControls;
using System.Drawing;
using FusionMiner;

namespace fusionweb
{
	using System;
	using System.Web;
	using System.Web.UI;

	
	public partial class Default : System.Web.UI.Page
	{
		private const string cs = @"server=localhost;userid=fusionminer;password=HQcU8js3nftR8f2h;database=fusionminer";

		protected void Page_Load (object sender, EventArgs e)
		{
			int totalMiners;

			TableRow row;
			row = new TableRow ();
			row.Cells.Add (new TableCell (){ Text = "N" });
			row.Cells.Add (new TableCell (){ Text = "ID" });
			row.Cells.Add (new TableCell (){ Text = "Serial Number" });
			row.Cells.Add (new TableCell (){ Text = "Version" });
			row.Cells.Add (new TableCell (){ Text = "Up Time" });
			row.Cells.Add (new TableCell (){ Text = "Current Speed" });
			row.Cells.Add (new TableCell (){ Text = "Average Speed" });
			row.Cells.Add (new TableCell (){ Text = "Accepted" });
			row.Cells.Add (new TableCell (){ Text = "Hardware Error" });
			row.Cells.Add (new TableCell (){ Text = "Error Rate" });
			row.Cells.Add (new TableCell (){ Text = "Board Temp" });
			row.Cells.Add (new TableCell (){ Text = "Chip Temp" });
			row.Cells.Add (new TableCell (){ Text = "IP" });
			row.Cells.Add (new TableCell (){ Text = "Location" });
			row.TableSection = TableRowSection.TableHeader;
			tableSlowMiners.Rows.Add (row);

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
						cmd.CommandText = "select count(*) from miner";
						totalMiners = Convert.ToInt32 (cmd.ExecuteScalar ());
						cmd.CommandText = "SELECT m . * , s.scatter FROM  `miner` m LEFT JOIN  `miner_scatter` s ON s.id = m.Id where m.LastStatusTime > @StatusTime";
						cmd.Parameters.Add (new MySqlParameter ("StatusTime", DateTime.UtcNow.AddMinutes (-8)));
						int activeMiner = 0;
						string newestVersion = "0.0.0.0";
						string oldestVersion = "999.999.999.999";
						Int64 totalSpeed = 0;
						int nslow = 0;
						using (var reader = cmd.ExecuteReader ()) {
							while (reader.Read ()) {
								activeMiner++;
								string version = reader.GetString ("Version");
								if (RemoteIsOldVersion (version, oldestVersion)) {
									oldestVersion = version;
								}
								if (!RemoteIsOldVersion (version, newestVersion)) {
									newestVersion = version;
								}
								cmdstatus.Parameters [0].Value = reader.GetInt64 ("LastStatusId");
								using (var readerstatus = cmdstatus.ExecuteReader ()) {
									while (readerstatus.Read ()) {
										int upTime = readerstatus.GetInt32 ("Up_Time");
										long currentSpeed = readerstatus.GetInt64 ("Current_Speed");
										long averageSpeed = readerstatus.GetInt32 ("Average_Speed");
										long accepted = readerstatus.GetInt64 ("NonceFound");
										long hwerror = readerstatus.GetInt64 ("HardwareError");
										totalSpeed += currentSpeed;
										long errrate = accepted == 0 ? 0 : hwerror * 100 / (accepted + hwerror);
//										if ((currentSpeed < 800000) || ((averageSpeed < 780000) && (upTime > 3600)) || ((errrate > 5) && (upTime > 3600))) {
										if (true) {
											nslow++;
											row = new TableRow ();
											row.Cells.Add (new TableCell (){ Text = nslow.ToString () });
											row.Cells.Add (new TableCell (){ Text = reader.GetInt64 ("Id").ToString () });
											row.Cells.Add (new TableCell (){ Text = reader.GetString ("scatter") });
											row.Cells.Add (new TableCell (){ Text = reader.GetString ("Version") });
											row.Cells.Add (new TableCell (){ Text = new TimeSpan (0, 0, upTime).ToString () });
											row.Cells.Add (new TableCell () {
												Text = String.Format ("{0:0,0}", currentSpeed),
												ForeColor = currentSpeed < 800000 ? Color.Red : Color.Black
											});
											row.Cells.Add (new TableCell () { Text = String.Format ("{0:0,0}", averageSpeed),
												ForeColor = averageSpeed < 800000 ? Color.Red : Color.Black
											});
											row.Cells.Add (new TableCell (){ Text = String.Format ("{0:0,0}", accepted) });
											row.Cells.Add (new TableCell () {
												Text = String.Format ("{0:0,0}", hwerror),
												ForeColor = errrate > 5 ? Color.Red : Color.Black
											});
											row.Cells.Add (new TableCell () {
												Text = String.Format ("{0}%", errrate),
												ForeColor = errrate > 5 ? Color.Red : Color.Black
											});
											row.Cells.Add (new TableCell (){ Text = readerstatus.GetDouble ("Board_Temp").ToString () });
											var chipTemp = readerstatus.GetDouble ("Chip_Temp");
											row.Cells.Add (new TableCell () { Text = chipTemp.ToString (),
												ForeColor = chipTemp > 125.0 ? Color.Red : Color.Black
											});
											row.Cells.Add (new TableCell (){ Text = reader.GetString ("InternetIp") });
											row.Cells.Add (new TableCell (){ Text = Utility.FindCityByIP (reader.GetString ("InternetIp")) });
											tableSlowMiners.Rows.Add (row);
										}
									}
								}
							}
						}
						conn.Close ();
						connstatus.Close ();
						labelTotalMiners.Text = totalMiners.ToString ();
						labelTotalSpeed.Text = String.Format ("{0:0,0}", totalSpeed);
						labelAverageSpeed.Text = activeMiner == 0 ? "" : String.Format ("{0:0,0}", totalSpeed / activeMiner);
						labelActiveMiners.Text = activeMiner.ToString ();
						labelOldestVersion.Text = oldestVersion;
						labelNewestVersion.Text = newestVersion;
					}
				} finally {          
				}

		}

		public static bool RemoteIsOldVersion (string remoteVer, string localVer)
		{
			int rver, lver;
			try {
				rver = Convert.ToInt32 (remoteVer.Substring (0, remoteVer.IndexOf ('.')));
				lver = Convert.ToInt32 (localVer.Substring (0, localVer.IndexOf ('.')));
				if (rver > lver) {
					return false;
				} else if (rver < lver) {
					return true;
				}
				remoteVer = remoteVer.Substring (remoteVer.IndexOf ('.') + 1);
				localVer = localVer.Substring (localVer.IndexOf ('.') + 1);

				rver = Convert.ToInt32 (remoteVer.Substring (0, remoteVer.IndexOf ('.')));
				lver = Convert.ToInt32 (localVer.Substring (0, localVer.IndexOf ('.')));
				if (rver > lver) {
					return false;
				} else if (rver < lver) {
					return true;
				}

				remoteVer = remoteVer.Substring (remoteVer.IndexOf ('.') + 1);
				localVer = localVer.Substring (localVer.IndexOf ('.') + 1);

				rver = Convert.ToInt32 (remoteVer.Substring (0, remoteVer.IndexOf ('.')));
				lver = Convert.ToInt32 (localVer.Substring (0, localVer.IndexOf ('.')));
				if (rver > lver) {
					return false;
				} else if (rver < lver) {
					return true;
				}

				remoteVer = remoteVer.Substring (remoteVer.IndexOf ('.') + 1);
				localVer = localVer.Substring (localVer.IndexOf ('.') + 1);

				rver = Convert.ToInt32 (remoteVer);
				lver = Convert.ToInt32 (localVer);
				if (rver > lver) {
					return false;
				} else if (rver < lver) {
					return true;
				}
				return false;
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return false;
			}
		}
	}
}

