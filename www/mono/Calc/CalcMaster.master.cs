using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI;

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
                List<HeaderLink> headerLinks = menuControlId.BuildMenu(new String[] { "Default.aspx", "MatrixSO.aspx" });
                for (int i = 0; i < headerLinks.Count; i++) 
                {
                    if (headerLinks[i].UHref.EndsWith("MatrixMCalc.aspx"))
                        headerLinks[i].UTitle = "Matrix x Matrix Calc";
                    if (headerLinks[i].UHref.EndsWith("MatrixVCalc.aspx"))
                        headerLinks[i].UTitle = "Vector x Matrix Calc";
                    if (headerLinks[i].UHref.EndsWith("CCalc.aspx"))
                        headerLinks[i].UTitle = "CC Calc";
                }
                menuControlId.BindMenu(headerLinks);                
            }
        }
     
    }
}