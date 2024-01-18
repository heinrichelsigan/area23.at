using System;
using System.Linq;
using System.Web;
using System.Globalization;
using Area23.At.Mono;
using System.Web.SessionState;

namespace Area23.At.Mono.Util
{
    /// <summary>
    /// GlobalAppSettings global App Settings
    /// </summary>
    [Serializable]
    public class GlobalAppSettings
    {
        public static String pictureUrl = Constants.URL_PIC;
        public static string prefixUrl = Constants.URL_PREFIX;
        public static Uri prefixUri = null;
        public static Uri pictureUri = null;
        public static CultureInfo systemLocale, locale;
        // private DIALOGS dialogOpened = DIALOGS.None;
        private static HttpContext context;
        private static HttpApplicationState application;
        private static HttpSessionState session;

        #region properties

        #region PictureUrl

        public static void InitPictureUrl()
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

        public static void InitPrefixUrl()
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

        public static Uri PictureUri { get { InitPictureUrl(); return pictureUri; } }

        public static String PictureUrl
        {
            get { InitPictureUrl(); return pictureUrl; }
            set
            {
                try
                {
                    pictureUri = new Uri(value);
                    pictureUrl = value;
                }
                catch (Exception exi)
                {
                    Console.Error.WriteLine(exi.StackTrace);
                }
            }
        }

        public static String PrefixUrl { get { InitPrefixUrl(); return prefixUrl; } }

        public static Uri PrefixUri { get { InitPrefixUrl(); return prefixUri; } }

        #endregion PictureUrl

        #region CultureLanguage

        public static void InitLocale()
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

        public static CultureInfo Locale { get { InitLocale(); return locale; } set => locale = value; }

        public static CultureInfo SystemLLocale { get { InitLocale(); return systemLocale; } }

        public static string LocaleString { get => Locale.DisplayName; set => locale = new CultureInfo(value); }

        public static string ISO2Lang { get => Locale.TwoLetterISOLanguageName; }
        #endregion CultureLanguage

        public static Exception LastException { get; set; }

        #endregion properties

        #region static ctor

        static GlobalAppSettings()
        {
            InitLocale();
            InitPrefixUrl();
            InitPictureUrl();
        }

        #endregion static ctor

        #region members

        public static HttpContext getContext()
        {
            context = HttpContext.Current;            
            return context;
        }

        public static HttpApplicationState getApplication()
        {
            application = HttpContext.Current.Application;
            return application;
        }

        public static HttpSessionState getSession()
        {
            session = HttpContext.Current.Session;
            return session;
        }



        
        // public DIALOGS getDialog() { return dialogOpened; }
        // public void setDialog(DIALOGS dia) { dialogOpened = dia; }

        #endregion members
        
    }
}