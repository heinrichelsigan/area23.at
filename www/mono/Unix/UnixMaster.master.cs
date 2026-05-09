using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Web;
using System.Web.UI;

namespace Area23.At.Mono.Unix
{
    public partial class UnixMaster : System.Web.UI.MasterPage
    {
        // public global::System.Web.UI.HtmlControls.HtmlForm MasterForm { get => ((Area23)(this.Master)).MasterForm; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterHead { get => ((Area23)(this.Master)).MasterHead; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterBody { get => ((Area23)(this.Master)).MasterBody; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterQrHead { get => this.UnixHead; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterQrBody { get => this.UnixBody; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var headerLinks = menuControlId.BuildMenu();
                menuControlId.BindMenu(headerLinks);
            }
        }
    }
}