using Area23.At.Framework.Library;
using Area23.At.Mono.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Games
{
    public partial class GamesMaster : System.Web.UI.MasterPage
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
            this.aGameFroga.HRef = LibPaths.GamesAppPath + "froga.aspx";
            this.aGameFrogb.HRef = LibPaths.GamesAppPath + "frogb.aspx";
            this.aGameSchnapsen.HRef = "/mono/SchnapNet/";
        }

        protected void NavFolderHandler(object sender, EventArgs args)
        {
            headerLeft.Attributes["class"] = "headerLeft";
            headerCenter.Attributes["class"] = "headerCenter";
            headerRight.Style["class"] = "headerRight";

            try
            {
                if (this.Request != null && this.Request.RawUrl != null)
                {
                    if (this.Request.RawUrl.Contains("froga.aspx"))
                    {
                        headerLeft.Attributes["class"] = "headerLeftSelect";
                        return;
                    }                    
                    if (this.Request.RawUrl.Contains("frogb.aspx"))
                    {
                        headerCenter.Attributes["class"] = "headerCenterSelect";
                        return;
                    }                    
                    if (this.Request.RawUrl.ToLower().Contains("schnaps"))
                    {
                        headerRight.Attributes["background-color"] = "headerRightSelect";
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