<%@ Page Language="C#" Inherits="fusiondash.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html lang="en">

<head>
  <meta charset="utf-8"/>
  <title>FusionMiner - Dashboard</title>
  <!---CSS Files-->
  <link rel="stylesheet" href="css/master.css"/>
  <link rel="stylesheet" href="css/tables.css"/>
</head>
<body>

  <div class="top-bar">
        <ul id="top-nav">
         <li class="nav-item"><a href="Default.aspx"><img src="img/nav/dash-active.png" alt="" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Settings</a></li>
         <li class="nav-item"><a href="Details.aspx"><img src="img/nav/anlt-active.png" alt="" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Dashboard</a></li>
         <li class="nav-item"><a href="ShowAll.aspx"><img src="img/nav/grid-active.png" alt="" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Discovery</a></li>
         <li class="nav-item"><a href="BatchChange.aspx"><img src="img/nav/batch-active.png" alt="" />&nbsp;&nbsp;Batch Change</a></li>
       </ul>
  </div>

  <div class="content container_12">
	<form id="formSettings" runat="server">
    <div class="box grid_12">
       <div class="box-head"><h2>Status</h2></div>
       <div class="box-content">
 		  <div class="form-row"><p class="form-label">Up Time:</p><div class="form-item"><asp:Label id="labelUpTime" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Serial Number:</p><div class="form-item"><asp:Label id="labelSN" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Nick Name:</p><div class="form-item"><asp:TextBox id="textBoxNick" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Average Speed:</p><div class="form-item"><asp:Label id="labelSpeedAvg" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Current Speed:</p><div class="form-item"><asp:Label id="labelSpeedCur" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Board Temp:</p><div class="form-item"><asp:Label id="labelBoardTemp" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Chip Temp:</p><div class="form-item"><asp:Label id="labelChipTemp" runat="server" /></div></div>
         <div class="clear"></div>
       </div>
    </div>

    <div class="box grid_12">
       <div class="box-head"><h2>Security</h2></div>
       <div class="box-content">
 		  <div class="form-row"><p class="form-label">Admin Password:</p><div class="form-item"><asp:TextBox id="textBoxMinerPass" runat="server" TextMode="Password"/></div></div>
 		  <div class="form-row"><p class="form-label">Retype Admin Password:</p><div class="form-item"><asp:TextBox id="textBoxMinerPass2" runat="server" TextMode="Password"/></div></div>
         <div class="clear"></div>
       </div>
    </div>

    <div class="box grid_12">
       <div class="box-head"><h2>Miner Setting</h2></div>
       <div class="box-content">
 		  <div class="form-row"><p class="form-label">Voltage:</p><div class="form-item"><asp:DropDownList id="dropdownListVolt" runat="server">
					<asp:Listitem value="85">0.850V</asp:Listitem>
					<asp:Listitem value="87">0.875V</asp:Listitem>
					<asp:Listitem value="90">0.900V</asp:Listitem>
					<asp:Listitem value="92">0.925V</asp:Listitem>
					<asp:Listitem value="95">0.950V</asp:Listitem>
					<asp:Listitem value="97">0.975V</asp:Listitem>
					<asp:Listitem value="100">1.000V</asp:Listitem>
					<asp:Listitem value="102">1.025V</asp:Listitem>
					<asp:Listitem value="105">1.050V</asp:Listitem>
					<asp:Listitem value="107">1.075V</asp:Listitem>
					<asp:Listitem value="110">1.100V</asp:Listitem>
				</asp:DropDownList></div></div>
 		  <div class="form-row"><p class="form-label">Frequency:</p><div class="form-item"><asp:DropDownList id="dropdownListFreq" runat="server">
					<asp:Listitem value="368">368MHz</asp:Listitem>
					<asp:Listitem value="384">384MHz</asp:Listitem>
					<asp:Listitem value="400">400MHz</asp:Listitem>
					<asp:Listitem value="416">416MHz</asp:Listitem>
					<asp:Listitem value="432">432MHz</asp:Listitem>
					<asp:Listitem value="448">448MHz</asp:Listitem>
					<asp:Listitem value="464">464MHz</asp:Listitem>
					<asp:Listitem value="480">480MHz</asp:Listitem>
					<asp:Listitem value="496">496MHz</asp:Listitem>
					<asp:Listitem value="512">512MHz</asp:Listitem>
					<asp:Listitem value="528">528MHz</asp:Listitem>
					<asp:Listitem value="544">544MHz</asp:Listitem>
				</asp:DropDownList></div></div>
 		  <div class="form-row"><p class="form-label">Pool 1 Url:</p><div class="form-item"><asp:TextBox id="textBoxPool1Url" runat="server" style="width:180px"/>:<asp:TextBox id="textBoxPool1Port" runat="server" MaxLength="5" style="width:40px"/></div></div>
 		  <div class="form-row"><p class="form-label">Pool 1 User:</p><div class="form-item"><asp:TextBox id="textBoxPool1User" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Pool 1 Password:</p><div class="form-item"><asp:TextBox id="textBoxPool1Pass" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Pool 2 Url:</p><div class="form-item"><asp:TextBox id="textBoxPool2Url" runat="server" style="width:180px" />:<asp:TextBox id="textBoxPool2Port" runat="server" MaxLength="5" style="width:40px"/></div></div>
 		  <div class="form-row"><p class="form-label">Pool 2 User:</p><div class="form-item"><asp:TextBox id="textBoxPool2User" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Pool 2 Password:</p><div class="form-item"><asp:TextBox id="textBoxPool2Pass" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Pool 3 Url:</p><div class="form-item"><asp:TextBox id="textBoxPool3Url" runat="server" style="width:180px" />:<asp:TextBox id="textBoxPool3Port" runat="server" MaxLength="5" style="width:40px"/></div></div>
 		  <div class="form-row"><p class="form-label">Pool 3 User:</p><div class="form-item"><asp:TextBox id="textBoxPool3User" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Pool 3 Password:</p><div class="form-item"><asp:TextBox id="textBoxPool3Pass" runat="server" /></div></div>
         <div class="clear"></div>
       </div>
    </div>

    <div class="box grid_12">
       <div class="box-head"><h2>Network Setting</h2></div>
       <div class="box-content">
 		  <div class="form-row"><p class="form-label">Network Type:</p><div class="form-item"><asp:DropDownList id="dropdownListWiredDHCP" runat="server">
					<asp:Listitem value="true">DHCP</asp:Listitem>
					<asp:Listitem value="false">Static IP</asp:Listitem>
				</asp:DropDownList></div></div>
 		  <div class="form-row"><p class="form-label">IP:</p><div class="form-item"><asp:TextBox id="textBoxWiredIP" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Subnet Mask:</p><div class="form-item"><asp:TextBox id="textBoxWiredSubnetMask" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Router:</p><div class="form-item"><asp:TextBox id="textBoxWiredRouter" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">DNS 1:</p><div class="form-item"><asp:TextBox id="textBoxWiredDNS1" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">DNS 2:</p><div class="form-item"><asp:TextBox id="textBoxWiredDNS2" runat="server" /></div></div>
         <div class="clear"></div>
       </div>
    </div>
     
    <div class="box grid_12">
       <div class="box-content">
       <div align="center">
         <asp:Button id="buttonBlink" runat="server" Text="&nbsp;Identify Miner&nbsp;" OnClick="buttonBlinkClick" />
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <asp:Button id="buttonShutdown" runat="server" Text="&nbsp;Shutdown Miner&nbsp;" OnClick="buttonShutdownClick" />
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <asp:Button id="buttonSave" runat="server" Text="&nbsp;Save Settings&nbsp;" OnClick="buttonSaveClicked" />
       </div>
       </div>
    </div>

    </form>
  </div>

</body>
</html>
