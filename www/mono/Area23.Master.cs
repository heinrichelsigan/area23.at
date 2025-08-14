using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Reflection;
using System.Web.UI;

namespace Area23.At.Mono
{
    public partial class Area23 : System.Web.UI.MasterPage
    {

        // public global::System.Web.UI.HtmlControls.HtmlForm MasterForm { get => Area23MasterForm; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterHead { get => HeadContentPlaceHolder; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterBody { get => BodyContentPlaceHolder; }


        protected void Page_Load(object sender, EventArgs e)
        {
            InitAHrefs();
            if (!Page.IsPostBack)
            {
                NavFolderHandler(sender, e);
            }            
        }


        protected void InitAHrefs()
        {
            this.LiteralVersion.Text = " v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ";
            this.aSlash.HRef = LibPaths.BaseAppPath;
            this.aUnix.HRef = LibPaths.UnixAppPath + "Default.aspx";
            this.aQr.HRef = LibPaths.QrAppPath + "ContactQrGenerator.aspx";
            this.aJson.HRef = LibPaths.BaseAppPath + "Json.aspx";
            // this.aByteTransColor.HRef = LibPaths.BaseAppPath + "ByteTransColor.aspx";
            this.aAesCrypt.HRef = LibPaths.EncodeAppPath + "CoolCrypt.aspx";
            this.aRpnCalc.HRef = LibPaths.CalcAppPath + "RpnCalc.aspx";
            this.aGames.HRef = LibPaths.GamesAppPath + "froga.aspx";
        }

        protected void NavFolderHandler(object sender, EventArgs args)
        {
            spanLeft.Attributes["class"] = "headerLeft";
            spanLeftCenter.Attributes["class"] = "headerLeftCenter";
            spanCenter0.Attributes["class"] = "headerCenter";
            spanCenter1.Attributes["class"] = "headerCenter";
            spanCenter2.Attributes["class"] = "headerCenter";            
            spanRightCenter.Attributes["class"] = "headerRightCenter";
            spanRight.Attributes["class"] = "headerRightCenter";

            try
            {
                if (Request != null && Request.RawUrl != null)
                {                    
                    if (Request.RawUrl.ToLower().Contains("unix"))
                    {
                        spanLeftCenter.Attributes["class"] = "headerLeftCenterSelect";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("qr"))
                    {
                        spanCenter0.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("json"))
                    {
                        spanCenter1.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("encode"))
                    {
                        spanCenter2.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("rpn") || Request.RawUrl.ToLower().Contains("calc"))
                    {
                        spanRightCenter.Attributes["class"] = "headerRightCenterSelect";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("game") || Request.RawUrl.ToLower().Contains("rog"))
                    {
                        spanRight.Attributes["class"] = "headerRightSelect";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("/"))
                    {
                        spanLeft.Attributes["class"] = "headerLeftSelect";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }

            try
            {
                if (Page != null && Page.Title != null)
                {
                    if (Page.Title.ToLower().StartsWith("fortune"))
                    {
                        spanLeft.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (Page.Title.ToLower().StartsWith("hex"))
                    {
                        spanLeftCenter.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (Page.Title.ToLower().Contains("qr"))
                    {
                        spanCenter0.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (Page.Title.ToLower().Contains("json"))
                    {
                        spanCenter1.Style["background-color"] = "#ffddee";
                        return;
                    }
                    //if (Page.Title.ToLower().Contains("trans"))
                    //{
                    //    spanCenter2.Style["background-color"] = "#ffddee";
                    //    return;
                    //}
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }

           
        }
    }
}