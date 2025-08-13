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
                InitAHrefs();
                NavFolderHandler(sender, e);
            }
        }


        protected void InitAHrefs()
        {
            this.aUnixMain.HRef = LibPaths.UnixAppPath + "Default.aspx";
            this.aFortunAsp.HRef = LibPaths.UnixAppPath + "FortunAsp.aspx";
            this.aHexDump.HRef = LibPaths.UnixAppPath + "HexDump.aspx";
            this.aBc.HRef = LibPaths.UnixAppPath + "Bc.aspx";
            this.aPdfMerge.HRef = LibPaths.UnixAppPath + "PdfMerge.aspx";
        }

        public static string GetFinalUrl(string suffixUrl = "")
        {
            string rawUrlString = HttpContext.Current.Request.RawUrl.ToString();
            int lastSlash = rawUrlString.LastIndexOf('/');
            string requestPrefix = rawUrlString.Substring(0, lastSlash);
            requestPrefix = requestPrefix.Replace("/Qr", "");
            requestPrefix = requestPrefix.Replace("/Unix", "");

            string finalUrl = requestPrefix;            
            if (!finalUrl.EndsWith("/")) finalUrl += "/";
            
            finalUrl += suffixUrl;
            return finalUrl;
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
                    if (this.Request.RawUrl.Contains("Default.aspx"))
                    {
                        headerLeft.Attributes["class"] = "headerLeftSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("FortunAsp.aspx"))
                    {
                        headerLeftCenter.Attributes["class"] = "headerLeftCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("HexDump.aspx"))
                    {
                        headerCenter.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("Bc.aspx"))
                    {
                        headerRightCenter.Attributes["class"] = "headerRightCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("PdfMerge.aspx"))
                    {
                        headerRight.Attributes["class"] = "headerRightSelect";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Area23Log.Logger.LogOriginMsgEx("UnixMaster.master.cs", "Error when setting up masterpage for unix.", ex);
            }            
        }
    }
}