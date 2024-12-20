using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono
{
    /// <summary>
    /// only to get my ip-address
    /// </summary>
    public partial class R : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string userHostName;
            string userHostAddr = Request.UserHostAddress;

            literalUserHost.Text = userHostAddr;

            if (!this.IsPostBack)
            {
                userHostName = Request.UserHostName;
                title.Text = userHostName;
                // header.InnerHtml = "<title>" + userHostName + "</title>";
            }

        }

    }

}