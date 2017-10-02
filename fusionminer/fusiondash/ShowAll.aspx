<%@ Page Language="C#" Inherits="fusiondash.ShowAll" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html lang="en">
<head>
  <meta charset="utf-8"/>
  <meta http-equiv="refresh" content="30;url=/ShowAll.aspx" />
  <title>FusionMiner - Show All Local Miners</title>
  <!---CSS Files-->
  <link rel="stylesheet" href="css/master.css"/>
  <link rel="stylesheet" href="css/tables.css"/>
</head>
<body>
	<form id="form1" runat="server">

  <div class="top-bar">
        <ul id="top-nav">
         <li class="nav-item"><a href="Default.aspx"><img src="img/nav/dash-active.png" alt="" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Settings</a></li>
         <li class="nav-item"><a href="Details.aspx"><img src="img/nav/anlt-active.png" alt="" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Dashboard</a></li>
         <li class="nav-item"><a href="ShowAll.aspx"><img src="img/nav/grid-active.png" alt="" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Discovery</a></li>
         <li class="nav-item"><a href="BatchChange.aspx"><img src="img/nav/batch-active.png" alt="" />&nbsp;&nbsp;Batch Change</a></li>
       </ul>
  </div>

  <div class="content container_12">
     <div class="box grid_12">
        <div class="box-head"><h2>Local Miners</h2></div>
        <div class="box-content no-pad">
	    <asp:Table class="display" id="tableMiners" runat="server"></asp:Table>
        <br/>
        </div>
     </div>
   </div>
  </form>
</body>
</html>
	