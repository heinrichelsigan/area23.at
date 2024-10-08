﻿using Area23.At.Framework.Library;
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
            this.aSlash.HRef = LibPaths.BaseAppPath;
            this.aUnix.HRef = LibPaths.UnixAppPath + "UnixMain.aspx";
            this.aQr.HRef = LibPaths.QrAppPath + "QRCodeGen.aspx";
            this.aJson.HRef = LibPaths.BaseAppPath + "json.aspx";
            // this.aByteTransColor.HRef = LibPaths.BaseAppPath + "ByteTransColor.aspx";
            this.aAesCrypt.HRef = LibPaths.EncodeAppPath + "AesImprove.aspx";
            this.aRpnCalc.HRef = LibPaths.CalcAppPath + "RpnCalc.aspx";
            this.aFroga.HRef = LibPaths.BaseAppPath + "froga.aspx";
            this.aSchnapsNet.HRef = "/mono/SchnapsNet/";
        }

        protected void NavFolderHandler(object sender, EventArgs args)
        {
            spanLeft.Attributes["class"] = "headerLeft";
            spanLeftCenter.Attributes["class"] = "headerLeftCenter";
            spanCenter0.Attributes["class"] = "headerCenter";
            spanCenter1.Attributes["class"] = "headerCenter";
            // spanCenter2.Attributes["class"] = "headerCenter";
            spanCenter3.Attributes["class"] = "headerCenter";
            spanCenter4.Attributes["class"] = "headerCenter";
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
                    //if (Request.RawUrl.ToLower().Contains("trans"))
                    //{
                    //    spanCenter2.Attributes["class"] = "headerCenterSelect";
                    //    return;
                    //}
                    if (Request.RawUrl.ToLower().Contains("encode"))
                    {
                        spanCenter3.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("rpn") || Request.RawUrl.ToLower().Contains("calc"))
                    {
                        spanCenter4.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    if (Request.RawUrl.ToLower().Contains("frog"))
                    {
                        spanRightCenter.Attributes["class"] = "headerRightCenterSelect";
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