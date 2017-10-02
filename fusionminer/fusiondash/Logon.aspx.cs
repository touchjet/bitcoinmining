using System.Web.Security;


namespace fusiondash
{
	using System;
	using System.Web;
	using System.Web.UI;

	public partial class Logon : System.Web.UI.Page
	{
		public void buttonLogonClicked (object sender, EventArgs args)
		{
			if (Global.Miner.Validate(textBoxPassword.Text)) {
				FormsAuthentication.RedirectFromLoginPage ("fusionminer", false);
			} else {
				labelError.Text = "Incorrect Password!";
			}

		}
	}
}

