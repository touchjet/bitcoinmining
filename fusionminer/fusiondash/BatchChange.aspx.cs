using System.Web.UI.WebControls;
using FusionMiner.Thrift;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;

namespace fusiondash
{
	using System;
	using System.Web;
	using System.Web.UI;

	
	public partial class BatchChange : System.Web.UI.Page
	{
		private MinerConfig _minerConfig;

		protected void Page_Init (object sender, EventArgs e)
		{
			bool oddRow = false;

			MinerSearch.SearchMiner (true);
			int TotalAvg = 0;
			int TotalCur = 0;

			TableRow row;
			row = new TableRow ();

			var cellAll = new TableCell ();
			var chkbAll = new CheckBox ();
			chkbAll.ID = "CheckBoxAll";
			chkbAll.Attributes.Add ("onclick", "selectall(this.checked)");
			cellAll.Controls.Add (chkbAll);
			row.Cells.Add (cellAll);

			row.Cells.Add (new TableCell (){ Text = "IP" });
			row.Cells.Add (new TableCell (){ Text = "Nick Name" });
			row.Cells.Add (new TableCell (){ Text = "Serial Number" });
			row.Cells.Add (new TableCell (){ Text = "Voltage" });
			row.Cells.Add (new TableCell (){ Text = "Frequency" });
			row.Cells.Add (new TableCell (){ Text = "Pool 1" });
			row.Cells.Add (new TableCell (){ Text = "Pool 2" });
			row.Cells.Add (new TableCell (){ Text = "Pool 3" });
			row.Cells.Add (new TableCell (){ Text = "Current Speed" });
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
				TableCell cell = new TableCell ();
				CheckBox chkb = new CheckBox ();
				chkb.ID = "chkb" + m.Key;
				chkb.EnableViewState = true;
				chkb.Attributes.Add ("name", "chkb");
				cell.Controls.Add (chkb);
				row.Cells.Add (cell);
				row.Cells.Add (new TableCell (){ Text = m.Key });
				row.Cells.Add (new TableCell (){ Text = m.Value.Miner.NickName });
				row.Cells.Add (new TableCell (){ Text = m.Value.Miner.SN });
				if ((MinerSearch.MinersConfig [m.Key].Hardwares != null) && (MinerSearch.MinersConfig [m.Key].Hardwares.Count > 0)) {
					row.Cells.Add (new TableCell (){ Text = MinerSearch.MinersConfig [m.Key].Hardwares [0].Voltage.ToString () });
					row.Cells.Add (new TableCell (){ Text = MinerSearch.MinersConfig [m.Key].Hardwares [0].Frequency.ToString () });
				} else {
					row.Cells.Add (new TableCell (){ Text = "" });
					row.Cells.Add (new TableCell (){ Text = "" });
				}
				if ((MinerSearch.MinersConfig [m.Key].Pools != null) && (MinerSearch.MinersConfig [m.Key].Pools.Count > 0)) {
					row.Cells.Add (new TableCell (){ Text = MinerSearch.MinersConfig [m.Key].Pools [0].UserName + "@" + MinerSearch.MinersConfig [m.Key].Pools [0].Url });
				} else {
					row.Cells.Add (new TableCell (){ Text = "" });
				}
				if ((MinerSearch.MinersConfig [m.Key].Pools != null) && (MinerSearch.MinersConfig [m.Key].Pools.Count > 1)) {
					row.Cells.Add (new TableCell (){ Text = MinerSearch.MinersConfig [m.Key].Pools [1].UserName + "@" + MinerSearch.MinersConfig [m.Key].Pools [1].Url });
				} else {
					row.Cells.Add (new TableCell (){ Text = "" });
				}
				if ((MinerSearch.MinersConfig [m.Key].Pools != null) && (MinerSearch.MinersConfig [m.Key].Pools.Count > 2)) {
					row.Cells.Add (new TableCell (){ Text = MinerSearch.MinersConfig [m.Key].Pools [2].UserName + "@" + MinerSearch.MinersConfig [m.Key].Pools [2].Url });
				} else {
					row.Cells.Add (new TableCell (){ Text = "" });
				}
				row.Cells.Add (new TableCell (){ Text = String.Format ("{0:0,0}", m.Value.SpeedCur) });
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
			Session ["MinersTable"] = tableMiners;
		}

		protected void Page_Load (object sender, EventArgs e)
		{
			tableMiners.ID = "tableMiners";
			tableMiners.EnableViewState = true;

			if (!IsPostBack) {
				_minerConfig = Global.Miner.GetConfig ();
				if ((_minerConfig != null) && (_minerConfig.Hardwares != null) && (_minerConfig.Pools != null)) {
					if (_minerConfig.Hardwares.Count > 0) {
						dropdownListFreq.SelectedValue = _minerConfig.Hardwares [0].Frequency.ToString ();
						dropdownListVolt.SelectedValue = _minerConfig.Hardwares [0].Voltage.ToString ();
					}
					if (_minerConfig.Pools.Count > 0) {
						textBoxPool1Url.Text = _minerConfig.Pools [0].Url;
						textBoxPool1Port.Text = _minerConfig.Pools [0].Port.ToString ();
						textBoxPool1User.Text = _minerConfig.Pools [0].UserName;
						textBoxPool1Pass.Text = _minerConfig.Pools [0].Password;
					}
					if (_minerConfig.Pools.Count > 1) {
						textBoxPool2Url.Text = _minerConfig.Pools [1].Url;
						textBoxPool2Port.Text = _minerConfig.Pools [1].Port.ToString ();
						textBoxPool2User.Text = _minerConfig.Pools [1].UserName;
						textBoxPool2Pass.Text = _minerConfig.Pools [1].Password;
					}
					if (_minerConfig.Pools.Count > 2) {
						textBoxPool3Url.Text = _minerConfig.Pools [2].Url;
						textBoxPool3Port.Text = _minerConfig.Pools [2].Port.ToString ();
						textBoxPool3User.Text = _minerConfig.Pools [2].UserName;
						textBoxPool3Pass.Text = _minerConfig.Pools [2].Password;
					}
				}
			} else {
				tableMiners = (Table)Session ["MinersTable"];
				_minerConfig = new MinerConfig ();
				_minerConfig.Hardwares = new List<HardwareConfig> ();
				if (dropdownListFreq.SelectedIndex != 0) {
					_minerConfig.Hardwares.Add (new HardwareConfig ());
					_minerConfig.Hardwares [0].Frequency = Convert.ToInt32 (dropdownListFreq.SelectedValue);
					_minerConfig.Hardwares [0].Voltage = Convert.ToInt32 (dropdownListVolt.SelectedValue);
				}
				if (_minerConfig.Hardwares.Count == 0) {
					_minerConfig.Hardwares = null;
				}
				_minerConfig.Pools = new List<PoolConfig> ();
				if (textBoxPool1Url.Text.Length > 0) {
					var pool = new PoolConfig ();
					pool.Type = PoolType.Stratum;
					pool.Url = textBoxPool1Url.Text;
					pool.Port = Convert.ToInt32 (textBoxPool1Port.Text);
					pool.UserName = textBoxPool1User.Text;
					pool.Password = textBoxPool1Pass.Text;
					_minerConfig.Pools.Add (pool);
				}
				if (textBoxPool2Url.Text.Length > 0) {
					var pool = new PoolConfig ();
					pool.Type = PoolType.Stratum;
					pool.Url = textBoxPool2Url.Text;
					pool.Port = Convert.ToInt32 (textBoxPool2Port.Text);
					pool.UserName = textBoxPool2User.Text;
					pool.Password = textBoxPool2Pass.Text;
					_minerConfig.Pools.Add (pool);
				} 
				if (textBoxPool3Url.Text.Length > 0) {
					var pool = new PoolConfig ();
					pool.Type = PoolType.Stratum;
					pool.Url = textBoxPool3Url.Text;
					pool.Port = Convert.ToInt32 (textBoxPool3Port.Text);
					pool.UserName = textBoxPool3User.Text;
					pool.Password = textBoxPool3Pass.Text;
					_minerConfig.Pools.Add (pool);
				} 
				if (_minerConfig.Pools.Count == 0) {
					_minerConfig.Pools = null;
				}
			}
		}

		public void buttonBlinkClick (object sender, EventArgs args)
		{
			MinerInterface miner;
			foreach (TableRow row in tableMiners.Rows) {
				if ((row.Cells [0].Controls.Count > 0) && (row.Cells [0].Controls [0] is CheckBox)) {
					if (((CheckBox)row.Cells [0].Controls [0]).Checked) {
						string ip = row.Cells [1].Text;
						miner = new MinerInterface (ip);
						miner.Blink ();
					}
				}
			}
		}

		public void buttonSaveClicked (object sender, EventArgs args)
		{
			StringBuilder minerList = new StringBuilder ();
			minerList.AppendLine ("<br/>");
			foreach (TableRow row in tableMiners.Rows) {
				if ((row.Cells [0].Controls.Count > 0) && (row.Cells [0].Controls [0] is CheckBox)) {
					if (((CheckBox)row.Cells [0].Controls [0]).Checked) {
						string ip = row.Cells [1].Text;
						new Thread (new ParameterizedThreadStart ((p) => {
							MinerInterface miner = new MinerInterface ((string)p);
							miner.SetConfig (_minerConfig);
						})).Start (ip);
						minerList.AppendLine (ip + "<br/>");
					}
				}
			}
			Thread.Sleep (2000);
			Session ["RebootedList"] = minerList.ToString ();
			Page.Response.Redirect ("Restarting.aspx");
		}
	}
}

