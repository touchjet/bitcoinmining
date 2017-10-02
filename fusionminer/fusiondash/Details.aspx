<%@ Page Language="C#" Inherits="fusiondash.Details" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html lang="en">
<head>
  <meta charset="utf-8"/>
  <meta http-equiv="refresh" content="15;url=/Details.aspx" />
  <title>FusionMiner - Dashboard</title>
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
        <div class="box-head"><h2>Status</h2></div>
        <div class="box-content no-pad">
        <table class="display" id="example">
		  <tr class="odd gradeA">
			<td>Up Time:</td>
			<td><asp:Label id="labelUpTime" runat="server" /></td>
			<td/>
			<td/>
		  </tr>
          <tr class="even gradeA">						
			<td>Average Speed:</td>
			<td><asp:Label id="labelSpeedAvg" runat="server" /></td>
			<td>Current Speed:</td>
			<td><asp:Label id="labelSpeedCur" runat="server" /></td>
		  </tr>
		  <tr class="odd gradeA">
			<td>Board Temp:</td>
			<td><asp:Label id="labelBoardTemp" runat="server" /></td>
			<td>Chip Temp:</td>
			<td><asp:Label id="labelChipTemp" runat="server" /></td>
	  	  </tr>
	    </table>
       </div>
     </div>
      
     <div class="box grid_12">
        <div class="box-head"><h2>Pools</h2></div>
        <div class="box-content no-pad">
	    <asp:Table class="display" id="tablePool" runat="server"></asp:Table>
        </div>
     </div>

     <div class="box grid_12">
        <div class="box-head"><h2>Chip Temperature</h2></div>
        <div class="box-content no-pad">
   	    <asp:Table class="display" id="tableChipTemp" runat="server"></asp:Table>
        </div>
     </div>

     <div class="box grid_12">
        <div class="box-head"><h2>Accepted: <asp:Label id="labelAccepted" runat="server"/></h2></div>
        <div class="box-content no-pad">
		<asp:Table class="display" id="tableAccepted" runat="server"></asp:Table>
        </div>
     </div>
     <div class="box grid_12">
        <div class="box-head"><h2>Hardware Error: <asp:Label id="labelHwError" runat="server"/></h2></div>
        <div class="box-content no-pad">
	    <asp:Table class="display" id="tableHwError" runat="server"></asp:Table>
        </div>
     </div>
     <div class="box grid_12">
        <div class="box-head"><h2>Total</h2></div>
        <div class="box-content no-pad">
	    <asp:Table class="display" id="tableTotal" runat="server"></asp:Table>
        </div>
     </div>
   </div>
  </form>
</body>
</html>
