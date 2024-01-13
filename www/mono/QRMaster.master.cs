using area23.at.www.mono.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace area23.at.www.mono
{
    public partial class QRMaster : System.Web.UI.MasterPage
    {
        public global::System.Web.UI.HtmlControls.HtmlForm MasterForm { get => ((Area23)(this.Master)).MasterForm; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterHead { get => ((Area23)(this.Master)).MasterHead; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterBody { get => ((Area23)(this.Master)).MasterBody; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterQrHead { get => this.QrHead; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterQrBody { get => this.QrBody; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                NavFolderHandler(sender, e);
            }
        }

        protected void NavFolderHandler(object sender, EventArgs args)
        {
            headerLeft.Style["background-color"] = "#ffccdd";
            headerLeftCenter.Style["background-color"] = "#ffccdd";
            headerCenter.Style["background-color"] = "#ffccdd";
            headerRightCenter.Style["background-color"] = "#ffccdd";
            // headerRight.Style["background-color"] = "#ffccdd";

            try
            {
                if (this.Request != null && this.Request.RawUrl != null)
                {
                    if (this.Request.RawUrl.Contains("QRCodeGen.aspx"))
                    {
                        headerLeft.Style["background-color"] = "#ffdfef";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("Qrc.aspx"))
                    {
                        headerLeftCenter.Style["background-color"] = "#ffdfef";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("Qr.aspx"))
                    {
                        headerCenter.Style["background-color"] = "#ffdfef";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("QRGen.aspx"))
                    {
                        headerRightCenter.Style["background-color"] = "#ffdfef";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("trans"))
                    {
                        // headerRight.Style["background-color"] = "#ffdfef";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }            
        }
    }
}