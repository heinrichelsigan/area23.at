using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Win32Api;
using Area23.At.ChatQ.Util;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using System.Windows.Media.Imaging;
using System.Windows;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Data.SqlTypes;
using Area23.At.Framework.Library.Util;
using static QRCoder.QRCodeGenerator;

namespace Area23.At.ChatQ
{
    public abstract class ChatQBasePage : System.Web.UI.Page
    {
        protected System.Collections.Generic.Queue<string> mqueue = new Queue<string>();
        protected Uri area23URL = new Uri("https://area23.at/");
        protected Uri darkstarURL = new Uri("https://darkstar.work/");
        protected Uri gitURL = new Uri("https://github.com/heinrichelsigan/area23.at/");
        /// <summary>
        /// ImageQr control.
        /// </summary>
        protected global::System.Web.UI.WebControls.Image ImageQr;
        protected System.Globalization.CultureInfo locale;
        /// <summary>
        /// HrefShort control.
        /// </summary>
        protected global::System.Web.UI.HtmlControls.HtmlAnchor HrefShort;

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

        public string SepChar { get => System.IO.Path.DirectorySeparatorChar.ToString(); }

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