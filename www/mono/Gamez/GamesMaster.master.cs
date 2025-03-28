using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Web.UI;

namespace Area23.At.Mono.Gamez
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
            this.aTicTacToe.HRef = LibPaths.GamesAppPath + "TicTacToe.aspx";
            this.aGameSchnapsen.HRef = "/mono/SchnapsNet/";
        }

        protected void NavFolderHandler(object sender, EventArgs args)
        {
            headerLeft.Attributes["class"] = "headerLeft";
            headerLeftCenter.Attributes["class"] = "headerLeftCenter";
            headerCenter.Attributes["class"] = "headerCenter";
            headerRight.Attributes["class"] = "headerRight";

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
                        headerLeftCenter.Attributes["class"] = "headerLeftCenterSelect";
                        return;
                    }
                    if (this.Request.RawUrl.ToLower().Contains("tictactoe.aspx"))
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