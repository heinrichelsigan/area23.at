﻿using Area23.At.Framework.Library.Static;
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
            this.aQrCodeGen.HRef = LibPaths.QrAppPath + "QRCodeGen.aspx";
            this.aQrc.HRef = LibPaths.QrAppPath + "Qrc.aspx";
            this.aQr.HRef = LibPaths.QrAppPath + "Qr.aspx";
            // this.aQrGen.HRef = LibPaths.QrAppPath + "QRGen.aspx";
            this.aUrlShortner.HRef = Constants.AREA23_S;          
        }

        protected void NavFolderHandler(object sender, EventArgs args)
        {
            headerLeft.Attributes["class"] = "headerLeft";
            headerLeftCenter.Attributes["class"] = "headerLeftCenter";
            headerCenter.Attributes["class"] = "headerCenter";
            // headerRightCenter.Attributes["class"] = "headerRightCenter";
            // headerRight.Style["class"] = "headerRight";

            try
            {
                if (this.Request != null && this.Request.RawUrl != null)
                {
                    if (this.Request.RawUrl.Contains("QRCodeGen.aspx"))
                    {
                        headerLeft.Attributes["class"] = "headerLeftSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("Qrc.aspx"))
                    {
                        headerLeftCenter.Attributes["class"] = "headerLeftCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("Qr.aspx"))
                    {
                        headerCenter.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    // if (this.Request.RawUrl.Contains("QRGen.aspx"))
                    // {
                    // headerRightCenter.Attributes["class"] = "headerRightCenterSelect";
                    // return;
                    // }
                    // if (this.Request.RawUrl.Contains("trans"))
                    // {
                    // headerRight.Attributes["background-color"] = "headerRightSelect";
                    // return;
                    // }
                }
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }            
        }
    }
}