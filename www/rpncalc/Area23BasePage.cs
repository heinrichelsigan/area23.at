using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using area23.at.mono.rpncalc.ConstEnum;
using area23.at.mono.rpncalc.Models;

namespace area23.at.mono.rpncalc
{
    public partial class Area23BasePage : System.Web.UI.Page
    {
        protected Uri gitUrl = new Uri(Constants.GITURL);       
        protected Uri backUrl = new Uri("https://area23.at/mono/rpncalc/");

        protected Models.GlobalAppSettings globalVariable;
        protected System.Globalization.CultureInfo locale;
        public Mutex rpncalcMutex;

        public System.Globalization.CultureInfo Locale
        {
            get
            {
                if (locale == null)
                {
                    try
                    {
                        string defaultLang = Request.Headers["Accept-Language"].ToString();
                        string firstLang = defaultLang.Split(',').FirstOrDefault();
                        defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                    catch (Exception)
                    {
                        locale = new System.Globalization.CultureInfo("en");
                    }
                }
                return locale;
            }
        }

        public string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        public string LogFile
        {
            get
            {
                string logAppPath = MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
                if (!logAppPath.Contains("RPNCalc.Web"))
                    logAppPath += "RPNCalc.Web" + SepChar;
                logAppPath += "log" + SepChar + DateTime.UtcNow.ToString("yyyyMMdd") + "_" + "rpncalcweb.log";
                return logAppPath;
            }
        }


        public void RefreshGlobalVariableSession()
        {
            this.Context.Session[Constants.APPNAME] = globalVariable;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            locale = Locale;
            InitURLBase();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (globalVariable == null)
            {
                if (this.Context.Session[Constants.APPNAME] == null)
                {
                    string initMsg = "New connection started from " + Request.UserHostAddress + " " + Request.UserHostName + " with " + Request.UserAgent + "!";
                    Log(initMsg);
                    Log("AppPath=" + HttpContext.Current.Request.ApplicationPath + " logging to " + LogFile);
                    globalVariable = new Models.GlobalAppSettings(this.Context, this.Session);
                    this.Context.Session[Constants.APPNAME] = globalVariable;
                }
                else
                {
                    globalVariable = (GlobalAppSettings)this.Context.Session[Constants.APPNAME];
                }
            }
        }

        public virtual void InitURLBase()
        {
            gitUrl = new Uri(Constants.GITURL);
            backUrl = new Uri(Constants.APPURL);            
        }

        public virtual void Log(string msg)
        {
            string preMsg = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss \t");
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string fn = this.LogFile;
            try
            {
                File.AppendAllText(fn, preMsg + msg + "\r\n");
            }
            catch (Exception e)
            {
                if (globalVariable != null)
                    globalVariable.LastException = e;
            }
        }

    }

}