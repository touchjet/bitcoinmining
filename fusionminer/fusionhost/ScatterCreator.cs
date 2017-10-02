using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;

namespace fusionhost
{
	public static class ScatterCreator
	{
		private const string cs = @"server=localhost;userid=fusionminer;password=;database=fusionminer";

		public static void GenerateScatterData ()
		{
			MySqlConnection conn = null;
			var map = new Dictionary <string,int> ();
			char[] chars = new char[16]{ '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
			Random rand = new Random ();
			int line = 0;
			do {
				StringBuilder sn = new StringBuilder ();
				for (int j = 0; j < 10; j++) {
					sn.Append (chars [rand.Next () % 16]);
				}
				if (!map.ContainsKey (sn.ToString ())) {
					map.Add (sn.ToString (), line);
					line++;
					Console.WriteLine (sn.ToString ());
				}
			} while (line < 100000);

			try {
				conn = new MySqlConnection (cs);
				conn.Open ();
				var stm = "insert into miner_scatter (id,scatter) values (@id,@scatter)"; 
				var cmd = new MySqlCommand (stm, conn);
				cmd.Parameters.Add("id",MySqlDbType.Int64);
				cmd.Parameters.Add("scatter",MySqlDbType.String);
				foreach (var s in map) {
					cmd.Parameters["id"].Value = s.Value;
					cmd.Parameters["scatter"].Value = s.Key;
					cmd.ExecuteNonQuery ();
				}
			} catch (Exception ex) {
				Console.WriteLine ("Error: {0}", ex.ToString ());

			} finally {          
				if (conn != null) {
					conn.Close ();
				}
			}

			using (StreamWriter sf = new StreamWriter ("scatter.csv", false)) {
				foreach (var sn in map) {
					sf.WriteLine ("{0},{1}", sn.Value, sn.Key);
				}
				sf.Flush ();
				sf.Close ();
			}
		}
	}
}

