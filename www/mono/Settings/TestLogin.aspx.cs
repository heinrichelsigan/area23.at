using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
// using Area23.At.Framework.Library.Util;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI;

namespace Area23.At.Mono.Settings
{
    public partial class TestLogin : UIPage
    {

        protected internal string _myServerKey = string.Empty;
        protected CqrFacade facade;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _myServerKey = GetServerKey();
            facade = new CqrFacade(_myServerKey);
            if (!Page.IsPostBack)
            {
                this.TextBoxPassword.Text = string.Empty;
                this.TextBoxUserName.Text = string.Empty;
                this.preOut.InnerHtml = string.Empty;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // base.Page_Load(sender, e);
            facade = new CqrFacade(_myServerKey);

            if (!Page.IsPostBack)
            {
                if (Application["KeepSignIn"] != null)
                    this.CheckBoxKeepSignIn.Checked = (bool)Application["KeepSignIn"];

                if (Application["ServerKey"] != null)
                    _myServerKey = Application["ServerKey"].ToString();
                
                facade = new CqrFacade(_myServerKey);               
            }
        }

        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.TextBoxUserName.Text))
            {
                this.preOut.InnerHtml = "You entered an <i style=\"color: purple\">empty</i> <b style=\"color: red\">username</b>.\n";
                return;
            }
            if (String.IsNullOrEmpty(this.TextBoxPassword.Text))
            {
                this.preOut.InnerHtml = "<i style=\"color: yellow\">Warning, empty password.</i>.\n";
            }

            Application["KeepSignIn"] = (bool)this.CheckBoxKeepSignIn.Checked;

            bool authenticated = this.AuthHtPasswd(this.TextBoxUserName.Text, this.TextBoxPassword.Text);
            if (!authenticated)
            {
                this.Title = "Login for \"" + TextBoxUserName.Text + "\" failed at " + DateTime.Now.ToString();
                this.preOut.InnerHtml = "Login for <b style=\"color: purple\">" + TextBoxUserName.Text + "</b> <b style=\"color: red\">failed</b> at " + DateTime.Now.ToString() + "\n";
            }
            else
            {
                this.Title = "Login for \"" + TextBoxUserName.Text + "\" successful at " + DateTime.Now.ToString();
                this.preOut.InnerHtml = "Login for <b style=\"color: purple\">" + TextBoxUserName.Text + "</b> <b style=\"color: green\">successful</b> at " + DateTime.Now.ToString() + "\n";
            }
           
        }


        protected string GetServerKey()
        {
            // _serverKey = Constants.AUTHOR_EMAIL;            
            if (ConfigurationManager.AppSettings[Constants.EXTERNAL_CLIENT_IP] != null)
                _myServerKey = (string)ConfigurationManager.AppSettings[Constants.EXTERNAL_CLIENT_IP];
            else
                _myServerKey = HttpContext.Current.Request.UserHostAddress;
            _myServerKey += Constants.APP_NAME;

            return _myServerKey;
        }
    }

}