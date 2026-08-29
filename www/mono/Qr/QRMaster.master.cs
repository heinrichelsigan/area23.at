using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.App_Data;
using System;
using System.Web.UI;

namespace Area23.At.Mono.Qr
{
    public partial class QRMaster : System.Web.UI.MasterPage
    {
        // public global::System.Web.UI.HtmlControls.HtmlForm MasterForm { get => ((Area23)(this.Master)).MasterForm; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterHead { get => ((Area23)(this.Master)).MasterHead; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterBody { get => ((Area23)(this.Master)).MasterBody; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterQrHead { get => this.QrHead; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterQrBody { get => this.QrBody; }

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