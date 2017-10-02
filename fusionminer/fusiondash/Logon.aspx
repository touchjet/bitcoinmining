<%@ Page Language="C#" Inherits="fusiondash.Logon" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html lang="en">
<head>
  <meta charset="utf-8"/>
  <title>FusionMiner - Login</title>
  <!---CSS Files-->
  <link rel="stylesheet" href="css/master.css"/>
  <link rel="stylesheet" href="css/login.css"/>
</head>
<body>
	<div class="wrapper">
   <div class="lg-body">
     <div class="inner">
       <div id="lg-head">
         <p><asp:Label id="labelError" runat="server" Text="Please login"/></p>
         <div class="separator"></div>
       </div>
       <div class="login">
         <form id="lgform" runat="server" >
           <fieldset>
              <ul>
                 <li id="psw-field">
                  <asp:TextBox id="textBoxPassword" runat="server" TextMode="Password" class="input required"/>
                 </li>
                 <li>
                  <asp:Button id="buttonLogon" runat="server" OnClick="buttonLogonClicked"  class="submit"/>
                 </li>
              </ul>
           </fieldset>
          </form>
        </div>
     </div>
    </div>
	</div>
</body>
</html>
