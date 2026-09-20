using Area23.At.Mono.App_Data;
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

        public virtual void SetInfoMsg(string message, Severity severity = Severity.None)
        {
            ((Area23)(this.Master)).SetInfoMsg(message, severity);
        }

    }

}