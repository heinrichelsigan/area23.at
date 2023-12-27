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
using Area23.At.Test.MarriageRisk.ConstEnum;
using Area23.At.Test.MarriageRisk.Models;

namespace Area23.At.Test.MarriageRisk
{
    public partial class Area23BasePage : System.Web.UI.Page
    {
        protected System.Collections.Generic.Queue<string> mqueue = new Queue<string>();
        protected Uri area23URL = new Uri("https://area23.at/");
        protected Uri darkstarURL = new Uri("https://darkstar.work/");
        protected Uri gitURL = new Uri("https://github.com/heinrichelsigan/area23.at/");

        protected Models.GlobalAppSettings globalVariable;
        protected System.Globalization.CultureInfo locale;

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
                if (!logAppPath.Contains("MarriageRisk"))
                    logAppPath += "MarriageRisk" + SepChar;
                logAppPath += "log" + SepChar + DateTime.UtcNow.ToString("yyyyMMdd") + "_" + "marriage_risk.log";
                return logAppPath;
            }
        }

        public virtual void InitURLBase()
        {
            area23URL = new Uri("https://area23.at/");
            darkstarURL = new Uri("https://darkstar.work/");
            gitURL = new Uri("https://github.com/heinrichelsigan/area23.at/");
        }

        public virtual void Log(string msg)
        {
            string preMsg = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss \t");
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string fn = this.LogFile;
            File.AppendAllText(fn, preMsg + msg + "\r\n");
        }

    }

}