using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using OneLogin.Saml;


namespace OneLogin.Saml
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AccountSettings accountSettings = new AccountSettings();


            AuthRequest req = new AuthRequest(new AppSettings(), accountSettings);


            string redir = accountSettings.idp_sso_target_url + "?SAMLRequest=" + Server.UrlEncode(req.GetRequest(
               AuthRequest.AuthRequestFormat.Base64));
            if (!Page.IsPostBack)
            {
                Response.Redirect(redir, true);
                //   Server.Transfer(redir);
            }
        }
    }
}