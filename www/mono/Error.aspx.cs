using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Static;
using Area23.At.Mono.App_Data;
using System;
using System.Web.Services.Description;

namespace Area23.At.Mono
{
    public partial class Error : System.Web.UI.Page
    {
        const string dlm = "-";
        Exception ex = null;

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!this.IsPostBack)
            {                
                ex = CqrException.LastException;
                if (ex != null || Application[Constants.APP_ERROR] != null || Request.QueryString["attack"] != null)
                {
                    this.DivException.Visible = true;

                    DivException.InnerText = (ex != null) ?
                        (ex.GetType().Name + ": " + ex.Message + "\r\n") : "";
                    DivException.InnerText += (Application[Constants.APP_ERROR] != null) ?
                        (string)Application[Constants.APP_ERROR] + "\r\n" : "";
                    DivException.InnerText += "Attac: " + (string)Request.QueryString["attack"] + "\r\n";
                }
            }

            if ((ex = CqrException.LastException) != null)
            {
                ((Area23)((this.Master))).SetInfoMsg(ex.GetType() + ": " + ex.Message, Severity.Error);
            }

        }

    }

}