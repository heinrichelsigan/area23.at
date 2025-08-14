using System;

namespace Area23.At.Mono.Qr
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("ContactQrGenerator.aspx");
        }
    }
}