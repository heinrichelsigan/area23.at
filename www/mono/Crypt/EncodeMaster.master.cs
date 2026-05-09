using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Web.UI;

namespace Area23.At.Mono.Crypt
{
    public partial class EncodeMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var headerLinks = menuControlId.BuildMenu(true);
                menuControlId.BindMenu(headerLinks);             
            }
        }

    }
}