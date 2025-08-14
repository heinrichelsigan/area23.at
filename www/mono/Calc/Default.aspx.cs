using System;

namespace Area23.At.Mono.Calc
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("RpnCalc.aspx");
        }
    }
}