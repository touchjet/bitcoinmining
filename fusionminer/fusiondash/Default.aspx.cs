using FusionMiner.Thrift;
using System.Collections.Generic;

namespace fusiondash
{
	using System;
	using System.Web;
	using System.Web.UI;

	public partial class Default : System.Web.UI.Page
	{
		private MinerConfig _minerConfig;
		private MinerStatus _minerStatus;

		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			_minerStatus = Global.Miner.GetStatus ();

			labelSN.Text = _minerStatus.Miner.SN;
			labelUpTime.Text = new TimeSpan (0, 0, _minerStatus.Utc).ToString ();
			labelSpeedAvg.Text = String.Format ("{0:0,0}", _minerStatus.SpeedAvg);
			labelSpeedCur.Text = String.Format ("{0:0,0}", _minerStatus.SpeedCur);
			labelBoardTemp.Text = String.Format ("{0:0.0}", _minerStatus.MaxTempBoard);
			labelChipTemp.Text = String.Format ("{0:0.0}", _minerStatus.MaxTempChip);

			if (!IsPostBack) {
				textBoxNick.Text = _minerStatus.Miner.NickName;
				_minerConfig = Global.Miner.GetConfig ();
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
				dropdownListWiredDHCP.SelectedValue = _minerConfig.WiredNetwork.DHCP ? "true" : "false";
				textBoxWiredIP.Text = _minerConfig.WiredNetwork.IP;
				textBoxWiredSubnetMask.Text = _minerConfig.WiredNetwork.SubnetMask;
				textBoxWiredRouter.Text = _minerConfig.WiredNetwork.Router;
				textBoxWiredDNS1.Text = _minerConfig.WiredNetwork.DNS1;
				textBoxWiredDNS2.Text = _minerConfig.WiredNetwork.DNS2;
				textBoxMinerPass.Text = "";
				textBoxMinerPass2.Text = "";
			} else {
				_minerConfig = new MinerConfig ();
				if (textBoxNick.Text.Length > 0) {
					_minerConfig.NickName = textBoxNick.Text;
				}
				_minerConfig.Hardwares = new List<HardwareConfig> ();
				_minerConfig.Hardwares.Add (new HardwareConfig ());
				_minerConfig.Hardwares [0].Frequency = Convert.ToInt32 (dropdownListFreq.SelectedValue);
				_minerConfig.Hardwares [0].Voltage = Convert.ToInt32 (dropdownListVolt.SelectedValue);

				_minerConfig.Pools = new List<PoolConfig> ();
				if (textBoxPool1Url.Text.Length > 0) {
					var pool = new PoolConfig ();
					pool.Type = PoolType.Stratum;
					pool.Url = textBoxPool1Url.Text.Trim();
					pool.Port = Convert.ToInt32 (textBoxPool1Port.Text);
					pool.UserName = textBoxPool1User.Text.Trim();
					pool.Password = textBoxPool1Pass.Text;
					_minerConfig.Pools.Add (pool);
				}
				if (textBoxPool2Url.Text.Length > 0) {
					var pool = new PoolConfig ();
					pool.Type = PoolType.Stratum;
					pool.Url = textBoxPool2Url.Text.Trim();
					pool.Port = Convert.ToInt32 (textBoxPool2Port.Text);
					pool.UserName = textBoxPool2User.Text.Trim();
					pool.Password = textBoxPool2Pass.Text;
					_minerConfig.Pools.Add (pool);
				} 
				if (textBoxPool3Url.Text.Length > 0) {
					var pool = new PoolConfig ();
					pool.Type = PoolType.Stratum;
					pool.Url = textBoxPool3Url.Text.Trim();
					pool.Port = Convert.ToInt32 (textBoxPool3Port.Text);
					pool.UserName = textBoxPool3User.Text.Trim();
					pool.Password = textBoxPool3Pass.Text;
					_minerConfig.Pools.Add (pool);
				} 

				_minerConfig.WiredNetwork = new NetworkConfig ();
				_minerConfig.WiredNetwork.Enabled = true;
				_minerConfig.WiredNetwork.DHCP = dropdownListWiredDHCP.SelectedValue.Equals ("true");
				if (_minerConfig.WiredNetwork.DHCP) {
					_minerConfig.WiredNetwork.IP = "";
					_minerConfig.WiredNetwork.SubnetMask = "";
					_minerConfig.WiredNetwork.Router = "";
					_minerConfig.WiredNetwork.DNS1 = "";
					_minerConfig.WiredNetwork.DNS2 = "";
				} else {
					_minerConfig.WiredNetwork.IP = textBoxWiredIP.Text;
					_minerConfig.WiredNetwork.SubnetMask = textBoxWiredSubnetMask.Text;
					_minerConfig.WiredNetwork.Router = textBoxWiredRouter.Text;
					_minerConfig.WiredNetwork.DNS1 = textBoxWiredDNS1.Text;
					_minerConfig.WiredNetwork.DNS2 = textBoxWiredDNS2.Text;
				}

				_minerConfig.WirelessNetwork = new NetworkConfig ();
				_minerConfig.WirelessNetwork.Enabled = false;
				if ((textBoxMinerPass.Text.Length > 0) && (textBoxMinerPass.Text.Equals (textBoxMinerPass2.Text))) {
					_minerConfig.Password = textBoxMinerPass.Text;
				}
			}
		}

		public void buttonBlinkClick(object sender, EventArgs args)
		{
			Global.Miner.Blink ();
		}

		public void buttonShutdownClick(object sender, EventArgs args)
		{
			Global.Miner.Shutdown ();
		}

		public void buttonSaveClicked (object sender, EventArgs args)
		{
			Global.Miner.SetConfig (_minerConfig);
			Page.Response.Redirect ("Restarting.aspx");
		}
	}
}

