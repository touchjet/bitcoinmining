<%@ Page Language="C#" Inherits="fusionweb.Default" %>
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
       </ul>
  </div>

  <div class="content container_12">
	<form id="formSettings" runat="server">
    <div class="box grid_12">
       <div class="box-head"><h2>Status</h2></div>
       <div class="box-content">
 		  <div class="form-row"><p class="form-label">Total Miners:</p><div class="form-item"><asp:Label id="labelTotalMiners" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Active Miners:</p><div class="form-item"><asp:Label id="labelActiveMiners" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Total Speed:</p><div class="form-item"><asp:Label id="labelTotalSpeed" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Average Speed:</p><div class="form-item"><asp:Label id="labelAverageSpeed" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Oldest Version:</p><div class="form-item"><asp:Label id="labelOldestVersion" runat="server" /></div></div>
 		  <div class="form-row"><p class="form-label">Newest Version:</p><div class="form-item"><asp:Label id="labelNewestVersion" runat="server" /></div></div>
         <div class="clear"></div>
       </div>
    </div>
     <div class="box grid_12">
        <div class="box-head"><h2>Slow Miners</h2></div>
        <div class="box-content no-pad">
	    <asp:Table class="display" id="tableSlowMiners" runat="server"></asp:Table>
        <br/>
        </div>
     </div>
    </form>
  </div>

</body>
</html>
	