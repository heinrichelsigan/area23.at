using Area23.At.Framework.Library;
using Area23.At.Mono.Calc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Calc
{
    public partial class CalcMaster : System.Web.UI.MasterPage
    {
        // public global::System.Web.UI.HtmlControls.HtmlForm MasterForm { get => ((Area23)(this.Master)).MasterForm; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterHead { get => ((Area23)(this.Master)).MasterHead; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterBody { get => ((Area23)(this.Master)).MasterBody; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterQrHead { get => this.CalcHead; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterQrBody { get => this.CalcBody; }

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
            this.aCCalc.HRef = LibPaths.CalcAppPath + "CCalc.aspx";
            this.aRpnCalc.HRef = LibPaths.CalcAppPath + "RpnCalc.aspx";
            this.aBc.HRef = LibPaths.UnixAppPath + "Bc.aspx";
            this.aMatrixCalc.HRef = LibPaths.CalcAppPath + "MatrixCalc.aspx";
        }

        protected void NavFolderHandler(object sender, EventArgs args)
        {
            headerLeft.Attributes["class"] = "headerLeft";
            headerLeftCenter.Attributes["class"] = "headerLeftCenter";
            headerCenter.Attributes["class"] = "headerCenter";
            headerRightCenter.Attributes["class"] = "headerRightCenter";            
            // headerRight.Style["class"] = "headerRight";

            try
            {
                if (this.Request != null && this.Request.RawUrl != null)
                {
                    if (this.Request.RawUrl.Contains("CCalc.aspx"))
                    {
                        headerLeft.Attributes["class"] = "headerLeftSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("RpnCalc.aspx"))
                    {
                        headerLeftCenter.Attributes["class"] = "headerLeftCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("MatrixCalc.aspx"))
                    {
                        headerCenter.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("Bc.aspx"))
                    {
                        headerCenter.Attributes["class"] = "headerRightCenterSelect";
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