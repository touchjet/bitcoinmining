using System.Web.UI.WebControls;
using System.Drawing;

namespace fusiondash
{
	using System;
	using System.Web;
	using System.Web.UI;
	using System.Linq;
	using FusionMiner.Thrift;

	public partial class Details : System.Web.UI.Page
	{
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			TableRow row; 
			TableCell cell;
			int boardcounter = 0;
			var detail = Global.Miner.GetDetail ();

			labelUpTime.Text = new TimeSpan (0, 0, detail.Status.Utc).ToString ();
			labelSpeedAvg.Text = String.Format ("{0:0,0}", detail.Status.SpeedAvg);
			labelSpeedCur.Text = String.Format ("{0:0,0}", detail.Status.SpeedCur);
			labelBoardTemp.Text = String.Format ("{0:0.0}", detail.Status.MaxTempBoard);
			labelChipTemp.Text = String.Format ("{0:0.0}", detail.Status.MaxTempChip);

			bool oddRow = true;

			row = new TableRow ();
			row.Cells.Add (new TableCell (){ Text = "URL" });
			row.Cells.Add (new TableCell (){ Text = "Status" });
			row.Cells.Add (new TableCell (){ Text = "Difficulty" });
			row.Cells.Add (new TableCell (){ Text = "Accepted" });
			row.Cells.Add (new TableCell (){ Text = "Rejected" });
			if (oddRow) {
				oddRow = false;
				row.CssClass = "odd gradeA";
			} else {
				oddRow = true;
				row.CssClass = "even gradeA";
			}
			tablePool.Rows.Add (row);
			foreach (var p in detail.Pool) {
				row = new TableRow ();
				cell = new TableCell ();
				cell.Text = p.Url;
				cell.CssClass = "wide-column";
				row.Cells.Add (cell);
				cell = new TableCell ();
				if (p.Alive) {
					cell.Text = "Alive";
				} else {
					cell.Text = "Dead";
				}
				row.Cells.Add (cell);
				row.Cells.Add (new TableCell (){ Text = p.Difficulty.ToString () });
				row.Cells.Add (new TableCell (){ Text = p.Accepted.ToString () });
				row.Cells.Add (new TableCell (){ Text = p.Rejected.ToString () });
				if (oddRow) {
					oddRow = false;
					row.CssClass = "odd gradeA";
				} else {
					oddRow = true;
					row.CssClass = "even gradeA";
				}
				tablePool.Rows.Add (row);
			}
			long totalAccepted = 0;
			long totalHwError = 0;
			foreach (var h in detail.Hardware) {
				foreach (var b in h.Board) {
					row = new TableRow ();
					cell = new TableCell ();
					cell.Text = String.Format ("Board {0:D}", boardcounter);
					cell.Width = 60;
					row.Cells.Add (cell);
					foreach (var c in b.Chip) {
						cell = new TableCell (); 
						cell.Text = c.Temperature.ToString ();
						if (c.Temperature >= 110) {
							cell.ForeColor = Color.Red;
						}
						row.Cells.Add (cell);
					}
					if (oddRow) {
						oddRow = false;
						row.CssClass = "odd gradeA";
					} else {
						oddRow = true;
						row.CssClass = "even gradeA";
					}
					tableChipTemp.Rows.Add (row);

					row = new TableRow ();
					cell = new TableCell ();
					cell.Text = String.Format ("Board {0:D}", boardcounter);
					cell.Width = 60;
					row.Cells.Add (cell);
					foreach (var c in b.Chip) {
						cell = new TableCell ();
						long accepted = c.Core.Sum (o => o.NonceFound);
						totalAccepted += accepted;
						cell.Text = accepted.ToString ();
						row.Cells.Add (cell);
					}
					if (oddRow) {
						oddRow = false;
						row.CssClass = "odd gradeA";
					} else {
						oddRow = true;
						row.CssClass = "even gradeA";
					}
					tableAccepted.Rows.Add (row);

					row = new TableRow ();
					cell = new TableCell ();
					cell.Text = String.Format ("Board {0:D}", boardcounter);
					cell.Width = 60;
					row.Cells.Add (cell);
					foreach (var c in b.Chip) {
						cell = new TableCell ();
						long hwerror = c.Core.Sum (o => o.HardwareError);
						totalHwError += hwerror;
						cell.Text = hwerror.ToString ();
						row.Cells.Add (cell);
					}
					if (oddRow) {
						oddRow = false;
						row.CssClass = "odd gradeA";
					} else {
						oddRow = true;
						row.CssClass = "even gradeA";
					}
					tableHwError.Rows.Add (row);

					row = new TableRow ();
					cell = new TableCell ();
					cell.Text = String.Format ("Board {0:D}", boardcounter);
					cell.Width = 60;
					row.Cells.Add (cell);
					foreach (var c in b.Chip) {
						cell = new TableCell ();
						cell.Text = (c.Core.Sum (o => o.NonceFound) + c.Core.Sum (o => o.NonceNotFound)).ToString ();
						row.Cells.Add (cell);
					}
					if (oddRow) {
						oddRow = false;
						row.CssClass = "odd gradeA";
					} else {
						oddRow = true;
						row.CssClass = "even gradeA";
					}
					tableTotal.Rows.Add (row);

					boardcounter++;
				}
			}
			labelAccepted.Text = totalAccepted.ToString ();
			labelHwError.Text = totalHwError.ToString ();
		}
	}
}

