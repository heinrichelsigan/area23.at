using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace area23.at.www.mono
{
    public partial class Area23 : System.Web.UI.MasterPage
    {

        public global::System.Web.UI.HtmlControls.HtmlForm MasterForm { get => this.Area23MasterForm; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterHead { get => this.HeadContentPlaceHolder; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterBody { get => this.BodyContentPlaceHolder; }


        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
                NavFolderHandler(sender, e);
            //}
            
        }


        protected void NavFolderHandler(object sender, EventArgs args)
        {
            spanLeft.Style["background-color"] = "#ffccdd";
            spanLeftCenter.Style["background-color"] = "#ffccdd";
            spanCenter0.Style["background-color"] = "#ffccdd";
            spanCenter1.Style["background-color"] = "#ffccdd";
            spanCenter2.Style["background-color"] = "#ffccdd";
            spanRightCenter.Style["background-color"] = "#ffccdd";
            spanRight.Style["background-color"] = "#ffccdd";

            try
            {
                if (this.Request != null && this.Request.RawUrl != null)
                {
                    if (this.Request.RawUrl.ToLower().Contains("fortun"))
                    {
                        spanLeft.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (this.Request.RawUrl.ToLower().Contains("hex"))
                    {
                        spanLeftCenter.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (this.Request.RawUrl.ToLower().Contains("qr"))
                    {
                        spanCenter0.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (this.Request.RawUrl.ToLower().Contains("json"))
                    {
                        spanCenter1.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (this.Request.RawUrl.ToLower().Contains("trans"))
                    {
                        spanCenter2.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (this.Request.RawUrl.ToLower().Contains("frog")) {
                        spanRightCenter.Style["background-color"] = "#ffddee";
                        return;
                    }
                }
            }
            catch (Exception ex) { }

            try
            {          
                if (this.Page != null && this.Page.Title != null)
                {
                    if (this.Page.Title.ToLower().StartsWith("fortune"))
                    {
                        spanLeft.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (this.Page.Title.ToLower().StartsWith("hex"))
                    {
                        spanLeftCenter.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (this.Page.Title.ToLower().Contains("qr"))
                    {
                        spanCenter0.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (this.Page.Title.ToLower().Contains("json"))
                    {
                        spanCenter1.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (this.Page.Title.ToLower().Contains("trans"))
                    {
                        spanCenter2.Style["background-color"] = "#ffddee";
                        return;
                    }
                }
            }
            catch (Exception ex) { }

           
        }
    }
}