using System.Threading;
using System.Net;
using System.Net.Sockets;
using Thrift.Transport;
using Thrift.Protocol;
using FusionMiner.Thrift;
using System.Collections.Concurrent;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

namespace fusiondash
{
	using System;
	using System.Web;
	using System.Web.UI;

	public partial class ShowAll : System.Web.UI.Page
	{

		protected override void OnLoad (EventArgs e)
		{
			TableRow row;
			base.OnLoad (e);

			bool oddRow = false;

			MinerSearch.SearchMiner ();
			int TotalAvg = 0;
			int TotalCur = 0;

			row = new TableRow ();
			row.Cells.Add (new TableCell (){ Text = "N" });
			row.Cells.Add (new TableCell (){ Text = "Nick Name" });
			row.Cells.Add (new TableCell (){ Text = "IP" });
			row.Cells.Add (new TableCell (){ Text = "Serial Number" });
			row.Cells.Add (new TableCell (){ Text = "Version" });
			row.Cells.Add (new TableCell (){ Text = "Up Time" });
			row.Cells.Add (new TableCell (){ Text = "Current Pool" });
			row.Cells.Add (new TableCell (){ Text = "Average Speed" });
			row.Cells.Add (new TableCell (){ Text = "Current Speed" });
			row.Cells.Add (new TableCell (){ Text = "Accepted" });
			row.Cells.Add (new TableCell (){ Text = "Hardware Error" });
			row.Cells.Add (new TableCell (){ Text = "Error Rate" });
			row.Cells.Add (new TableCell (){ Text = "Board Temp" });
			row.Cells.Add (new TableCell (){ Text = "Chip Temp" });
			row.TableSection = TableRowSection.TableHeader;
			tableMiners.Rows.Add (row);
			int n = 1;
			var items = from pair in MinerSearch.Miners
			            orderby pair.Value.Miner.NickName, pair.Key
			            select pair;
			foreach (var m in items) {
				TotalAvg += m.Value.SpeedAvg;
				TotalCur += m.Value.SpeedCur;

				row = new TableRow ();
				row.Cells.Add (new TableCell (){ Text = n.ToString () });
				row.Cells.Add (new TableCell (){ Text = m.Value.Miner.NickName });
				var cell = new TableCell ();
				cell.Controls.Add (new HyperLink (){ NavigateUrl = @"http://" + m.Key + "/", Text = m.Key });
				row.Cells.Add (cell);
				row.Cells.Add (new TableCell (){ Text = m.Value.Miner.SN });
				row.Cells.Add (new TableCell (){ Text = m.Value.Miner.Version });
				row.Cells.Add (new TableCell (){ Text = new TimeSpan (0, 0, m.Value.Utc).ToString () });
				row.Cells.Add (new TableCell (){ Text = m.Value.CurrentPool });
				row.Cells.Add (new TableCell (){ Text = String.Format ("{0:0,0}", m.Value.SpeedAvg) });
				row.Cells.Add (new TableCell (){ Text = String.Format ("{0:0,0}", m.Value.SpeedCur) });
				row.Cells.Add (new TableCell (){ Text = String.Format ("{0:0,0}", m.Value.NonceFound) });
				row.Cells.Add (new TableCell (){ Text = String.Format ("{0:0,0}", m.Value.HardwareError) });
				double errrate = m.Value.NonceFound == 0 ? 0.0 : m.Value.HardwareError * 100 / m.Value.NonceFound;
				row.Cells.Add (new TableCell (){ Text = String.Format ("{0:0.0}%", errrate) });
				row.Cells.Add (new TableCell (){ Text = m.Value.MaxTempBoard.ToString () });
				row.Cells.Add (new TableCell (){ Text = m.Value.MaxTempChip.ToString () });

				if (oddRow) {
					oddRow = false;
					row.CssClass = "odd gradeA";
				} else {
					oddRow = true;
					row.CssClass = "even gradeA";
				}
				tableMiners.Rows.Add (row);
				n++;
			}

			row = new TableRow ();
			row.Cells.Add (new TableCell (){ Text = "" });
			row.Cells.Add (new TableCell (){ Text = "" });
			row.Cells.Add (new TableCell (){ Text = "" });
			row.Cells.Add (new TableCell (){ Text = "" });
			row.Cells.Add (new TableCell (){ Text = "" });
			row.Cells.Add (new TableCell (){ Text = "" });
			row.Cells.Add (new TableCell (){ Text = "" });
			row.Cells.Add (new TableCell (){ Text = String.Format ("{0:0,0}", TotalAvg) });
			row.Cells.Add (new TableCell (){ Text = String.Format ("{0:0,0}", TotalCur) });
			row.Cells.Add (new TableCell (){ Text = "" });
			row.Cells.Add (new TableCell (){ Text = "" });
			row.TableSection = TableRowSection.TableFooter;
			tableMiners.Rows.Add (row);

		}
	}
}

