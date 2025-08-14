using System;

namespace Area23.At.Mono.Crypt
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("CoolCrypt.aspx");
        }
    }
}