
namespace fusiondash
{
	using System;
	using System.Web;
	using System.Web.UI;

	public partial class Restarting : System.Web.UI.Page
	{
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			if (Session ["RebootedList"] != null) {
				labelMiners.Text = Session ["RebootedList"].ToString ();
			}
		}
	}
}

