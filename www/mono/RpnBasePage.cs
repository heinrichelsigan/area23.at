using Area23.At.Mono.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Area23.At.Mono
{
    public partial class RpnBasePage : System.Web.UI.Page
    {
        protected Uri gitUrl = new Uri(Constants.GIT_URL);
        protected Uri backUrl = new Uri(Constants.RPN_URL);

        protected System.Globalization.CultureInfo locale;
        public Mutex mutex;

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

        public string SepChar { get => Paths.SepChar.ToString(); }

        public string LogFile
        {
            get => Paths.LogFile;
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            locale = Locale;
            InitURLBase();
        }
       

        public virtual void InitURLBase()
        {
            gitUrl = new Uri(Constants.GIT_URL);
            backUrl = new Uri(Constants.RPN_URL);
        }

        public virtual void Log(string msg)
        {
            Area23Log.LogStatic(msg);
        }

    }

}