using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono
{
    public partial class R : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string userHostAddr = Request.UserHostAddress;
            string userHostName = Request.UserHostName;

            header.InnerHtml = "<title>{userHostName}</title>";
            literalUserHost.Text = userHostAddr;
        }
    }
}