﻿using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Crypt
{
    public partial class EncodeMaster : System.Web.UI.MasterPage
    {
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
            this.aUueMime.HRef = LibPaths.EncodeAppPath + "UueMime.aspx";
            this.aAes.HRef = LibPaths.EncodeAppPath + "AesImprove.aspx";
            this.aImgCrypt.HRef = LibPaths.EncodeAppPath + "ImgPngCrypt.aspx";
            this.aCoolCrypt.HRef = LibPaths.EncodeAppPath + "CoolCrypt.aspx";
            this.aUrlZenMatrix.HRef = LibPaths.EncodeAppPath + "ZenMatrixVisualize.aspx";
        }

        protected void NavFolderHandler(object sender, EventArgs args)
        {
            headerLeft.Attributes["class"] = "headerLeft";
            headerLeftCenter.Attributes["class"] = "headerLeftCenter";
            headerCenter.Attributes["class"] = "headerCenter";
            headerRightCenter.Attributes["class"] = "headerRightCenter";
            headerRight.Attributes["class"] = "headerRight";

            try
            {
                if (this.Request != null && this.Request.RawUrl != null)
                {
                    if (this.Request.RawUrl.Contains("UueMime.aspx"))
                    {
                        headerLeft.Attributes["class"] = "headerLeftSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("AesImprove.aspx"))
                    {
                        headerLeftCenter.Attributes["class"] = "headerLeftCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("ImgPngCrypt.aspx"))
                    {
                        headerCenter.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("CoolCrypt.aspx"))
                    {
                        headerRightCenter.Attributes["class"] = "headerRightCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("ZenMatrixVisualize.aspx"))
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