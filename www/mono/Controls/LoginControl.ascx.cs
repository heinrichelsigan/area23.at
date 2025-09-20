using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Area23.At.Mono.Controls
{
    public partial class LoginControl : System.Web.UI.UserControl
    {

        protected internal readonly object _lock = new object();
        protected internal string _directoryPath = "";

        public string PhysicalDirectoryPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_directoryPath) && Directory.Exists(_directoryPath))
                    return _directoryPath;

                int lastPathSeperator = -1;
                string phyAppPath = Request.PhysicalPath;

                if ((phyAppPath.Contains(SepChar)) && ((lastPathSeperator = phyAppPath.LastIndexOf(SepChar)) > 0))
                    _directoryPath = phyAppPath.Substring(0, lastPathSeperator);
                if (string.IsNullOrEmpty(_directoryPath))
                {
                    var dirInfo = Directory.GetParent(phyAppPath);
                    _directoryPath = dirInfo.FullName;
                }

                return _directoryPath;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (Session[Constants.AUTH_INFO] != null && !string.IsNullOrEmpty(Session[Constants.AUTH_INFO].ToString()))
            {
                DivLoginBox.Visible = false;
                preOut.InnerHtml = "<b>Authenticated</b>";
            } 
            else
            {
                DivLoginBox.Visible = true;
                preOut.InnerHtml = "<b><i>Not</b> Authenticated</i>";
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// AuthHtPasswd - authenticates a user against .htpasswd in apache2
        /// </summary>
        /// <param name="user"><see cref="string"/> username</param>
        /// <param name="passwd"><see cref="string"/>password</param>
        /// <returns>true on successful authentificaton, otherwise false</returns>
        protected virtual bool AuthHtPasswd(string user, string passwd)
        {

            bool authTypeBasic = false, authBasicProviderFile = false;
            string directoryPath = "", htAccessFile = "", authFile = "", requireUser = "";


            if (!Directory.Exists(PhysicalDirectoryPath))
            {
                Area23Log.LogStatic("return false! \tdirectory " + directoryPath + " does not exist!\n");
                return false;
            }

            htAccessFile = Path.Combine(directoryPath, ".htaccess");
            if (!File.Exists(htAccessFile))
            {
                Area23Log.LogStatic("return true; \t.htaccess file " + htAccessFile + " does not exist!\n");
                return true;
            }

            List<string> lines = File.ReadLines(htAccessFile).ToList();
            foreach (string line in lines)
            {
                if (line.StartsWith("AuthType Basic", StringComparison.CurrentCultureIgnoreCase))
                    authTypeBasic = true;
                if (line.StartsWith("AuthBasicProvider file", StringComparison.CurrentCultureIgnoreCase))
                    authBasicProviderFile = true;
                if (line.StartsWith("AuthUserFile ", StringComparison.CurrentCultureIgnoreCase))
                    authFile = line.Replace("AuthUserFile ", "").Replace("\"", "");
                if (line.StartsWith("Require user ", StringComparison.CurrentCultureIgnoreCase))
                    requireUser = line.Replace("Require user ", "");
            }

            if (!authTypeBasic && !authBasicProviderFile)
            {
                Area23Log.LogStatic("return false! \tauthTypeBasic = " + authTypeBasic + "; authBasicProviderFile = " + authBasicProviderFile + ";\n");
                return false;
            }

            if (!string.IsNullOrEmpty(requireUser) && !user.Equals(requireUser, StringComparison.CurrentCultureIgnoreCase))
            {
                Area23Log.LogStatic("return false! \trequireUser = " + requireUser + " NOT EQUALS user = " + user + "!\n");
                return false;
            }


            if (!string.IsNullOrEmpty(authFile) && File.Exists(authFile))
            {
                string consoleOut = "", consoleError = "";
                string passedthrough = ProcessCmd.ExecuteWithOutAndErr(
                    "htpasswd",
                    String.Format(" -b -v {0} {1} {2} ", authFile, user, passwd),
                    out consoleOut,
                    out consoleError,
                    false);

                Area23Log.LogStatic("passedthrough = \t$(htpasswd" + String.Format(" -b -v {0} {1} {2})", authFile, user, passwd));
                Area23Log.LogStatic("passedthrough = \t" + passedthrough);
                string userMatch = string.Format("Password for user {0} correct.", user);
                if (passedthrough.EndsWith(userMatch, StringComparison.CurrentCultureIgnoreCase) ||
                    passedthrough.Contains(userMatch))
                {
                    Area23Log.LogStatic("return true; \t[" + passedthrough + "] matches {" + userMatch + "}.\n");
                    return true;
                }
                else
                {
                    Area23Log.LogStatic("return false! \t[" + passedthrough + "] not matching {" + userMatch + "}.\n");
                    return false;
                }

            }

            Area23Log.LogStatic("return true; \tfall through.\n");
            return true;
        }


        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.TextBoxUserName.Text) && AuthHtPasswd(this.TextBoxUserName.Text, this.TextBoxPassword.Text))
            {
                Session[Constants.AUTH_INFO] = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.TextBoxUserName.Text + "\n" + this.TextBoxPassword.Text));
                Response.Redirect(Request.Url.ToString());
            }
            else
                Session[Constants.AUTH_INFO] = null;
        }

        #region Logging

        public string SepChar { get => LibPaths.SepChar.ToString(); }

        public string LogFile { get => LibPaths.LogFileSystemPath; }

        public virtual void Log(string msg)
        {
            Area23Log.LogStatic(msg);
        }

        #endregion Logging

    

    }
}