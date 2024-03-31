using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area23.At.Www.Common
{
    /// <summary>
    /// static Constants including static application settings
    /// </summary>
    public static class Constants
    {
        #region public const
        public const string APP_NAME = "area23.at.mono";
        public const string APP_DIR = "mono";
        public const string VERSION = "v2.2023.4";
        public const string APP_PATH = "https://area23.at/mono/test/";
        public const string RPN_URL = "https://area23.at/mono/test/RpnCalc.aspx";
        public const string GIT_URL = "https://github.com/heinrichelsigan/area23.at";
        public const string URL_PIC = "https://area23.at/mono/test/res/img/";
        public const string URL_PREFIX = "https://area23.at/mono/test/res/";
        public const string AREA23_S = "https://area23.at/s/";
        public const string URL_SHORT = "https://area23.at/s/?";
        public const string AREA23_UTF8_URL = "https://area23.at/u/";        
        public const string LOG_DIR = "log";
        public const string QR_DIR = "Qr";
        public const string UTF8_DIR = "Utf8";
        public const string UNIX_DIR = "Unix";
        public const string RES_FOLDER = "res";
        public const string JS_DIR = "js";
        public const string CSS_DIR = "css";        
        public const string JSON_SAVE_FILE = "urlshort.json";
        public const string UTF8_JSON = "utf8symol.json";

        public const string ACCEPT_LANGUAGE = "Accept-Language";
        public const string FORTUNE_BOOL = "FORTUNE_BOOL";
        public const string UNKNOWN = "UnKnown";
        public const string DEFAULT_MIMETYPE = "application/octet-stream";
        public const string RPN_STACK = "rpnStack";
        public const string CHANGE_CLICK_EVENTCNT = "change_Click_EventCnt";
        public const string BC_START_MSG = "bc 1.07.1\r\nCopyright 1991-1994, 1997, 1998, 2000, 2004, 2006, 2008, 2012-2017 Free Software Foundation, Inc.\r\nThis is free software with ABSOLUTELY NO WARRANTY.\r\nFor details type `warranty'.\r\n";
        public const char ANNOUNCE = ':';
        public const char DATE_DELIM = '-';
        public const char WHITE_SPACE = ' ';
        public const char UNDER_SCORE = '_';

        public const string BACK_COLOR = "BackColor";
        public const string QR_COLOR = "QrColor";
        public const string BACK_COLOR_STRING = "BackColorString";
        public const string QR_COLOR_STRING = "QrColorString";

        public const string SNULL = "(null)";

        #endregion public const

        #region properties

        public const string JSON_SAMPLE = @"{ 
 	""quiz"": { 
 		""sport"": { 
 			""q1"": { 
 				""question"": ""Which one is correct team name in NBA?"", 
 					""options"": [ 
 						""New York Bulls"", 
 							""Los Angeles Kings"", 
 							""Golden State Warriros"", 
 							""Huston Rocket"" 
 						], 
 					""answer"": ""Huston Rocket"" 
 				} 
 			}, 
 		""maths"": { 
 			""q1"": { 
 				""question"": ""5 + 7 = ?"", 
 					""options"": [ 
 						""10"", 
 						""11"", 
 						""12"", 
 						""13"" 
 					], 
 					""answer"": ""12"" 
				}, 
 			""q2"": { 
 				""question"": ""12 - 8 = ?"", 
 				""options"": [ 
 						""1"", 
 						""2"", 
 						""3"", 
 						""4"" 
 						], 
 					""answer"": ""4"" 
 				}, 
 		} 
 	} 
 }";

        private static System.Globalization.CultureInfo locale = null;
        private static String defaultLang = null;

        /// <summary>
        /// Culture Info from HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE]
        /// </summary>
        public static System.Globalization.CultureInfo Locale
        {
            get
            {
                if (locale == null)
                {
                    defaultLang = "en";
                    try
                    {
                        if (HttpContext.Current.Request != null && HttpContext.Current.Request.Headers != null &&
                            HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE] != null)
                        {
                            string firstLang = HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE].
                                ToString().Split(',').FirstOrDefault();
                            defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                        }

                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                    catch (Exception)
                    {
                        defaultLang = "en";
                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                }
                return locale;
            }
        }

        public static string ISO2Lang { get => Locale.TwoLetterISOLanguageName; }

        /// <summary>
        /// UT DateTime @area23.at including seconds
        /// </summary>
        public static string DateArea23Seconds { get => DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"); }

        /// <summary>
        /// UTC DateTime Formated
        /// </summary>
        public static string DateArea23
        {
            get => DateTime.UtcNow.ToString("yyyy") + Constants.DATE_DELIM +
                DateTime.UtcNow.ToString("MM") + Constants.DATE_DELIM +
                DateTime.UtcNow.ToString("dd") + Constants.WHITE_SPACE +
                DateTime.UtcNow.ToString("HH") + Constants.ANNOUNCE +
                DateTime.UtcNow.ToString("mm") + Constants.ANNOUNCE + Constants.WHITE_SPACE;
        }

        /// <summary>
        /// UTC DateTime File Prefix
        /// </summary>
        public static string DateFile { get => DateArea23.Replace(WHITE_SPACE, UNDER_SCORE).Replace(ANNOUNCE, UNDER_SCORE); }

        private static readonly string backColorString = "#ffffff";
        public static string BackColorString
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[BACK_COLOR_STRING] != null) ?
                    (string)HttpContext.Current.Session[BACK_COLOR_STRING] : backColorString;                
            set
            {
                HttpContext.Current.Session[BACK_COLOR] = ColorFrom.FromHtml(value);
                HttpContext.Current.Session[BACK_COLOR_STRING] = value;
            }
        }

        private static readonly string qrColorString = "#000000";
        public static string QrColorString
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[QR_COLOR_STRING] != null) ?
                    (string)HttpContext.Current.Session[QR_COLOR_STRING] : qrColorString;
            set
            {
                HttpContext.Current.Session[QR_COLOR] = ColorFrom.FromHtml(value);
                HttpContext.Current.Session[QR_COLOR_STRING] = value;
            }
        }

        public static System.Drawing.Color BackColor
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[BACK_COLOR] != null) ?
                    (System.Drawing.Color)HttpContext.Current.Session[BACK_COLOR] : ColorFrom.FromHtml(backColorString);
            set
            {
                if (value != null)
                {
                    HttpContext.Current.Session[BACK_COLOR] = value;
                    HttpContext.Current.Session[BACK_COLOR_STRING] = value.ToXrgb();
                }
                else
                {
                    HttpContext.Current.Session[BACK_COLOR_STRING] = backColorString;
                    HttpContext.Current.Session[BACK_COLOR] = ColorFrom.FromHtml(backColorString);
                }
            }
        }

        public static System.Drawing.Color QrColor
        {
            get => (HttpContext.Current.Session != null && HttpContext.Current.Session[QR_COLOR] != null) ?
                    (System.Drawing.Color)HttpContext.Current.Session[QR_COLOR] : ColorFrom.FromHtml(qrColorString);           
            set
            {
                if (value != null)
                {                                        
                    HttpContext.Current.Session[QR_COLOR] = value;
                    HttpContext.Current.Session[QR_COLOR_STRING] = value.ToXrgb();
                }
                else
                {
                    HttpContext.Current.Session[QR_COLOR_STRING] = qrColorString;
                    HttpContext.Current.Session[QR_COLOR] = ColorFrom.FromHtml(qrColorString);
                }
            }                
        } 

        public static bool FortuneBool
        {
            get
            {
                if (HttpContext.Current.Session[FORTUNE_BOOL] == null)
                    HttpContext.Current.Session[FORTUNE_BOOL] = false;
                else
                    HttpContext.Current.Session[FORTUNE_BOOL] = !((bool)HttpContext.Current.Session[FORTUNE_BOOL]);
                
                return (bool)HttpContext.Current.Session[FORTUNE_BOOL];
            }
        }        

        #endregion properties
    }
}