using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Static;
using System;

namespace Area23.At.Mono
{
    public partial class Error : System.Web.UI.Page
    {
        const string dlm = "-";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {                
                Exception ex = CqrException.LastException;
                if (ex != null || Application[Constants.APP_ERROR] != null || Request.QueryString["attack"] != null) 
                {
                    this.PreException.Visible = true;
                    PreException.InnerText = (ex != null) ? 
                        (ex.GetType().Name + ": " + ex.Message + "\r\n") : "";
                    PreException.InnerText += (Application[Constants.APP_ERROR] != null) ?
                        (string)Application[Constants.APP_ERROR] + "\r\n" : "";
                    PreException.InnerText += "Attac: " + (string)Request.QueryString["attack"] + "\r\n";
                }
            }
            

        }

    }
}