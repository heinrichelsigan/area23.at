using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.App_Data;
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
                this.errorDiv.Visible = false;
            }            
        }

        protected void InitAHrefs()
        {
            this.LiteralVersion.Text = " v" + Assembly.GetExecutingAssembly().GetName().Version.ToString() + " ";
            this.aSlash.HRef = LibPaths.BaseAppPath;
            this.aUnix.HRef = LibPaths.UnixAppPath + "Default.aspx";
            this.aQr.HRef = LibPaths.QrAppPath + "ContactQrGenerator.aspx";
            this.aJson2Xml.HRef = LibPaths.BaseAppPath + "Json2Xml.aspx";
            // this.aByteTransColor.HRef = LibPaths.BaseAppPath + "ByteTransColor.aspx";
            this.aAesCrypt.HRef = LibPaths.EncodeAppPath + "CoolCrypt.aspx";
            this.aRpnCalc.HRef = LibPaths.CalcAppPath + "CCalc.aspx";
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
                    if (Request.RawUrl.ToLower().Contains("json2xml") || Request.RawUrl.ToLower().Contains("json"))
                    {
                        spanCenter1.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("crypt"))
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
                        spanLeftCenter.Style["background-color"] = "#ffddee";
                        return;
                    }                    
                    if (Page.Title.ToLower().Contains("qr"))
                    {
                        spanCenter0.Style["background-color"] = "#ffddee";
                        return;
                    }                    
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }
        }

           
        public void SetInfoMsg(string message, Severity severity = Severity.None)
        {                        
            switch (severity)
            {                                    
                case Severity.Error:
                    this.imgError.Src = "res/img/symbol/master_error.gif";
                    this.LiteralError.Text = "<span style='color:red;'>" + message + "</span>";
                    break;
                case Severity.Warn:
                    this.imgError.Src = "res/img/symbol/master_warn.gif";
                    this.LiteralError.Text = "<span style='color:orange;'>" + message + "</span>";
                    break;
                case Severity.Info:
                    this.imgError.Src = "res/img/symbol/master_info.gif";
                    this.LiteralError.Text = "<span style='color:blue;'>" + message + "</span>";
                    break;
                case Severity.Ask:
                    this.imgError.Src = "res/img/symbol/master_question.gif";
                    this.LiteralError.Text = "<span style='color:purple;'>" + message + "</span>";
                    break;
                case Severity.OK:
                    this.imgError.Src = "res/img/symbol/master_ok.gif";
                    this.LiteralError.Text = "<span style='color:darkgreen;'>" + message + "</span>";
                    break;
                case Severity.None:
                default:
                    this.LiteralError.Text = "";
                    this.imgError.Src = "res/img/symbol/master_none.gif";
                    // this.errorDiv.Visible = false;
                    return;
            }
            
            this.errorDiv.Visible = true;

        }


    }
}