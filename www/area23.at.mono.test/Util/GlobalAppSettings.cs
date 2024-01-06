using System;
using System.Linq;
using System.Web;
using System.Globalization;
using area23.at.mono.test;
using System.Web.SessionState;

namespace area23.at.mono.test.Util
{
    [Serializable]
    public class GlobalAppSettings
    {
        public int ccard = -1;
        public String pictureUrl = Constants.URLPIC;
        public string prefixUrl = Constants.URLPREFIX;
        public Uri prefixUri = null;
        public Uri pictureUri = null;
        public CultureInfo systemLocale, locale;
        // private DIALOGS dialogOpened = DIALOGS.None;
        private static HttpContext context;
        private static HttpApplicationState application;
        private HttpSessionState session;

        #region properties

        #region PictureUrl
        public Uri PictureUri { get { InitPictureUrl(); return this.pictureUri; } }

        public String PictureUrl
        {
            get { InitPictureUrl(); return this.pictureUrl; }
            set
            {
                try
                {
                    this.pictureUri = new Uri(value);
                    this.pictureUrl = value;
                }
                catch (Exception exi)
                {
                    Console.Error.WriteLine(exi.StackTrace);
                }
            }
        }

        public String PrefixUrl { get { InitPrefixUrl(); return this.prefixUrl; } }

        public Uri PrefixUri { get { InitPrefixUrl(); return this.prefixUri; } }
        #endregion PictureUrl

        #region CultureLanguage
        public CultureInfo Locale { get { InitLocale(); return locale; } set => locale = value; }

        public CultureInfo SystemLLocale { get { InitLocale(); return systemLocale; } }

        public String LocaleString { get => Locale.DisplayName; set => locale = new CultureInfo(value); }

        public String ISO2Lang { get => Locale.TwoLetterISOLanguageName; }
        #endregion CultureLanguage

        public Exception LastException { get; set; }

        public String InnerPreText { get; internal set; }

        #endregion properties

        #region ctor

        public GlobalAppSettings() : this(HttpContext.Current) { }

        public GlobalAppSettings(HttpContext c) : this(c, c.Application, c.Session) { }

        public GlobalAppSettings(HttpContext c, HttpSessionState hses) : this(c, c.Application, hses) { }

        public GlobalAppSettings(HttpContext c, HttpApplicationState haps, HttpSessionState hses)
        {
            context = c;
            application = haps;
            session = hses;
            InitLocale();
            InitPrefixUrl();
            InitPictureUrl();
        }

        #endregion ctor

        #region members

        public HttpContext getContext()
        {
            context = HttpContext.Current;            
            return context;
        }

        public HttpApplicationState getApplication()
        {
            application = HttpContext.Current.Application;
            return application;
        }

        public HttpSessionState getSession()
        {
            session = HttpContext.Current.Session;
            return session;
        }

        public void InitLocale()
        {
            if (systemLocale == null)
            {
                try
                {                    
                    string defaultLang = getContext().Request.Headers["Accept-Language"].ToString();
                    string firstLang = defaultLang.Split(',').FirstOrDefault();
                    defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                    systemLocale = new CultureInfo(defaultLang);
                }
                catch (Exception e)
                {
                    LastException = e;
                    systemLocale = new CultureInfo("en");
                }
            }
            if (locale == null)
            {
                try
                {
                    string defaultLang = getContext().Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault();
                    if (string.IsNullOrEmpty(defaultLang)) defaultLang = "en";
                    locale = new CultureInfo(defaultLang);
                }
                catch (Exception e)
                {
                    LastException = e;
                    locale = new CultureInfo(systemLocale.TwoLetterISOLanguageName.ToLower());
                }
            }
        }

        public void InitPrefixUrl()
        {
            try
            {
                if (prefixUri == null)
                    prefixUri = new Uri(prefixUrl);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        public void InitPictureUrl()
        {
            try
            {
                if (pictureUri == null)
                    pictureUri = new Uri(pictureUrl);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        
        // public DIALOGS getDialog() { return dialogOpened; }
        // public void setDialog(DIALOGS dia) { dialogOpened = dia; }

        #endregion members
        
    }
}