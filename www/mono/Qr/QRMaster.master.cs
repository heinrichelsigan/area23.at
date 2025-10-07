using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
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
                InitAHrefs();
                NavFolderHandler(sender, e);
            }
        }

        protected void InitAHrefs()
        {
            this.aContactQrGenerator.HRef = LibPaths.QrAppPath + "ContactQrGenerator.aspx";
            this.aContactPrefilled.HRef = LibPaths.QrAppPath + "ContactPrefilled.aspx";
            this.aGenericQr.HRef = LibPaths.QrAppPath + "GenericQr.aspx";
            this.aQrRedirect.HRef = LibPaths.QrAppPath + "QrRedirect.aspx";
            this.aUrlShortner.HRef = LibPaths.QrAppPath + "S.aspx";
        }

        protected void NavFolderHandler(object sender, EventArgs args)
        {
            headerLeft.Attributes["class"] = "headerLeft";
            headerLeftCenter.Attributes["class"] = "headerLeftCenter";
            headerCenter.Attributes["class"] = "headerCenter";
            headerRightCenter.Attributes["class"] = "headerRightCenter";
            headerRight.Attributes["class"] = "headerRight";
            // headerRight.Style["class"] = "headerRight";

            try
            {
                if (this.Request != null && this.Request.RawUrl != null)
                {
                    if (this.Request.RawUrl.Contains("ContactQrGenerator.aspx"))
                    {
                        headerLeft.Attributes["class"] = "headerLeftSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("ContactPrefilled.aspx"))
                    {
                        headerLeftCenter.Attributes["class"] = "headerLeftCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("GenericQr.aspx"))
                    {
                        headerCenter.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("QRGen.aspx"))
                    {
                        headerRightCenter.Attributes["class"] = "headerRightCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("S.aspx"))
                    {
                        headerRight.Attributes["class"] = "headerRightSelect";
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