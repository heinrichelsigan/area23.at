using Area23.At.Mono.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono
{
    public partial class Area23 : System.Web.UI.MasterPage
    {

        public global::System.Web.UI.HtmlControls.HtmlForm MasterForm { get => Area23MasterForm; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterHead { get => HeadContentPlaceHolder; }
        public global::System.Web.UI.WebControls.ContentPlaceHolder MasterBody { get => BodyContentPlaceHolder; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                NavFolderHandler(sender, e);
            }
            
        }


        protected void NavFolderHandler(object sender, EventArgs args)
        {
            spanLeft.Style["background-color"] = "#ffccdd";
            spanLeftCenter.Style["background-color"] = "#ffccdd";
            spanCenter0.Style["background-color"] = "#ffccdd";
            spanCenter1.Style["background-color"] = "#ffccdd";
            spanCenter2.Style["background-color"] = "#ffccdd";
            spanCenter3.Style["background-color"] = "#ffccdd";
            spanRightCenter.Style["background-color"] = "#ffccdd";
            spanRight.Style["background-color"] = "#ffccdd";

            try
            {
                if (Request != null && Request.RawUrl != null)
                {
                    if (Request.RawUrl.ToLower().Contains("fortun"))
                    {
                        spanLeft.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("hex"))
                    {
                        spanLeftCenter.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("qr"))
                    {
                        spanCenter0.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("json"))
                    {
                        spanCenter1.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("trans"))
                    {
                        spanCenter2.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("rpn"))
                    {
                        spanCenter3.Style["background-color"] = "#ffddee";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("frog"))
                    {
                        spanRightCenter.Style["background-color"] = "#ffddee";
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
                    if (Page.Title.ToLower().Contains("trans"))
                    {
                        spanCenter2.Style["background-color"] = "#ffddee";
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