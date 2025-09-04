using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Web.UI;

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
            this.aCoolCrypt.HRef = LibPaths.EncodeAppPath + "CoolCrypt.aspx";
            this.aAes.HRef = LibPaths.EncodeAppPath + "AesImprove.aspx";
            this.aHashKey.HRef = LibPaths.EncodeAppPath + "HashKey.aspx";
            this.aUrlZenMatrix.HRef = LibPaths.EncodeAppPath + "ZenMatrixVisualize.aspx";
        }

        protected void NavFolderHandler(object sender, EventArgs args)
        {
            headerLeft.Attributes["class"] = "headerLeft";
            headerLeftCenter.Attributes["class"] = "headerLeftCenter";
            headerCenter.Attributes["class"] = "headerCenter";
            headerRightCenter.Attributes["class"] = "headerRightCenter";

            try
            {
                if (this.Request != null && this.Request.RawUrl != null)
                {
                    if (this.Request.RawUrl.Contains("CoolCrypt.aspx"))
                    {
                        headerLeft.Attributes["class"] = "headerLeftSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("AesImprove.aspx"))
                    {
                        headerLeftCenter.Attributes["class"] = "headerLeftCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("HashKey.aspx"))
                    {
                        headerCenter.Attributes["class"] = "headerCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.Contains("ZenMatrixVisualize.aspx"))
                    {
                        headerRightCenter.Attributes["class"] = "headerRightCenterSelect";
                        return;
                    }
                    //if (this.Request.RawUrl.Contains("ZenMatrixVisualize.aspx"))
                    //{
                    //    headerRight.Attributes["class"] = "headerRightSelect";
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