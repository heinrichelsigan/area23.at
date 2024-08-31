using System;
using System.Text;
using System.Web;

namespace Area23.At.Framework.Library.Core
{

    /// <summary>
    /// static Constants including static application settings
    /// </summary>
    public static class Constants
    {
        #region public const
        public const string APP_NAME = "Area23.At.Mono";
        public const string APP_DIR = "net";
        public const string VERSION = "v2.24.831";
        public const string AREA23_URL = "https://area23.at";
        public const string APP_PATH = "https://area23.at/net/";
        public const string RPN_URL = "https://area23.at/net/RpnCalc.aspx";

        public const string GIT_URL = "https://github.com/heinrichelsigan/area23.at";
        public const string URL_PIC = "https://area23.at/net/res/img/";
        public const string URL_PREFIX = "https://area23.at/net/res/";
        public const string AREA23_S = "https://area23.at/s/";
        public const string URL_SHORT = "https://area23.at/s/?";
        public const string AREA23_UTF8_URL = "https://area23.at/u/";

        public const string LOG_DIR = "log";
        public const string LOG_EXT = ".log";
        public const string QR_DIR = "Qr";
        public const string AUTHOR = "Heinrich Elsigan";
        public const string AUTHOR_EMAIL = "heinrich.elsigan@gmail.com";
        public const string AREA23_EMAIL = "zen@area23.at";
        public const string AUTHOR_SIGNATURE = "-- \nHeinrich G.Elsigan\nTheresianumgasse 6/28, A-1040 Vienna\n phone: +43 650 752 79 28 \nmobile: +43 670 406 89 83 \nemails: heinrich.elsigan @gmail.com\n        root@darkstar.work he@area23.at\n        heinrich.elsigan @live.at\n        sites: darkstar.work area23.at\nweblog: blog.darkstar.work\n   wko: https://firmen.wko.at/DetailsKontakt.aspx?FirmaID=19800fbd-84a2-456d-890e-eb1fa213100f";

        public const string UTF8_DIR = "Utf8";
        public const string UNIX_DIR = "Unix";
        public const string RES_FOLDER = "res";
        public const string CALC_DIR = "Calc";
        public const string RES_DIR = "res";
        public const string OUT_DIR = "out";
        public const string TEXT_DIR = "text";
        public const string BIN_DIR = "bin";
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

        public const string ROACH_DESKTOP_WINDOW = "Roach.Desktop.Window";

        public const string EXE_COMMAND_CMD = "cmd";
        public const string EXE_POWER_SHELL = "powershell";

        public const string EXE_WIN_INIT = "wininit";
        public const string EXE_SERVICES = "services";
        public const string EXE_SVC_HOST = "svchost";
        public const string EXE_TASK_HOST = "taskhostw";
        public const string EXE_DLL_HOST = "dllhost";
        public const string EXE_SCHEDULER = "scheduler";
        public const string EXE_VM_COMPUTE = "vmcompute";
        public const string EXE_WIN_DEFENDER = "MsMpEng";
        public const string EXE_LASS = "lsass";                     // local Security Authority Subsystem Service. 
        public const string EXE_CSRSS = "csrss";                    // hosts the server side of the Win32 subsystem

        public const string EXE_WIN_LOGON = "winlogon";             // windows logon handler for current logon
        public const string EXE_DESKTOP_WINDOW_MANAGER = "dwm";     // window manager for current logon

        public const string SNULL = "(null)";

        public static readonly string[] EXE_WIN_SYSTEM = { EXE_WIN_INIT, EXE_SERVICES,
            EXE_SVC_HOST, EXE_TASK_HOST, EXE_DLL_HOST,
            EXE_SCHEDULER, EXE_VM_COMPUTE, EXE_WIN_DEFENDER, EXE_LASS, EXE_CSRSS,
            EXE_WIN_LOGON, EXE_DESKTOP_WINDOW_MANAGER
        };

        public const string AES_ENVIROMENT_KEY = "APP_ENCRYPTION_SECRET_KEY";
        public static readonly string AES_KEY = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("AesKey"));
        public static readonly string AES_IV = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("AesIv4"));
        public static readonly string DES3_KEY = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("DesKey"));
        public static readonly string DES3_IV = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("3DesIv"));
        // public static readonly string SERPENT_KEY = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("BOUNCE"));
        // public static readonly string SERPENT_IV = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("CASTLE"));
        public static readonly string BOUNCEK = Convert.ToBase64String(Encoding.UTF8.GetBytes("BOUNCE"));
        public static readonly string BOUNCE4 = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes("CASTLE"));


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

        #endregion public const

        #region properties

        /// <summary>
        /// AppLogFile - logfile with <see cref="At.Framework.Library.Extensions.Area23Date(DateTime)"/> prefix
        /// </summary>
        public static string AppLogFile { get => DateTime.UtcNow.Area23Date() + UNDER_SCORE + APP_NAME + LOG_EXT; }


        public static string Json_Example { get => ResReader.GetValue("json_sample0"); }

        private static System.Globalization.CultureInfo locale = null;
        private static String defaultLang = "en";

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
                        //if (HttpContext.Current.Request != null && HttpContext.Current.Request.Headers != null &&
                        //    HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE] != null)
                        //{
                        //    string firstLang = HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE].
                        //        ToString().Split(',')[0];
                        //    defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                        //}

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
            get
            {
                if (System.AppDomain.CurrentDomain.GetData(BACK_COLOR_STRING) != null)
                    return (string)System.AppDomain.CurrentDomain.GetData(BACK_COLOR_STRING).ToString();
                
                return backColorString;
            }
            set
            {               
                System.AppDomain.CurrentDomain.SetData(BACK_COLOR, ColorFrom.FromHtml(value));
                System.AppDomain.CurrentDomain.SetData(BACK_COLOR_STRING, value);
            }
        }

        private static readonly string qrColorString = "#000000";
        public static string QrColorString
        {
            get
            {
                if (System.AppDomain.CurrentDomain.GetData(QR_COLOR_STRING) != null)
                    return (string)System.AppDomain.CurrentDomain.GetData(QR_COLOR_STRING).ToString();
                
                return qrColorString;
            } 
            set
            {
                System.AppDomain.CurrentDomain.SetData(QR_COLOR, ColorFrom.FromHtml(value));
                System.AppDomain.CurrentDomain.SetData(QR_COLOR_STRING, value);
            }
        }

        public static System.Drawing.Color BackColor
        {
            get
            {
                if (System.AppDomain.CurrentDomain.GetData(BACK_COLOR) != null)
                    return (System.Drawing.Color)System.AppDomain.CurrentDomain.GetData(BACK_COLOR);
                
                if (System.AppDomain.CurrentDomain.GetData(BACK_COLOR_STRING) != null)
                {
                    string _backColorString = (string)System.AppDomain.CurrentDomain.GetData(BACK_COLOR_STRING).ToString();
                    return ColorFrom.FromHtml(_backColorString);
                }
                
                return ColorFrom.FromHtml(backColorString);
            } 
            set
            {
                if (value != null)
                {
                    System.AppDomain.CurrentDomain.SetData(BACK_COLOR, value);
                    System.AppDomain.CurrentDomain.SetData(BACK_COLOR_STRING, value.ToXrgb());
                }
                else
                {
                    System.AppDomain.CurrentDomain.SetData(BACK_COLOR_STRING, backColorString);
                    System.AppDomain.CurrentDomain.SetData(BACK_COLOR, ColorFrom.FromHtml(backColorString));
                }
            }
        }

        public static System.Drawing.Color QrColor
        {
            get
            {
                if (System.AppDomain.CurrentDomain.GetData(QR_COLOR) != null)
                    return (System.Drawing.Color)System.AppDomain.CurrentDomain.GetData(QR_COLOR);
                
                if (System.AppDomain.CurrentDomain.GetData(QR_COLOR_STRING) != null)
                {
                    string _qrColorString = (string)System.AppDomain.CurrentDomain.GetData(QR_COLOR_STRING).ToString();
                    return ColorFrom.FromHtml(_qrColorString);
                }

                return ColorFrom.FromHtml(qrColorString);
            }            
            set
            {
                if (value != null)
                {
                    System.AppDomain.CurrentDomain.SetData(QR_COLOR, value);
                    System.AppDomain.CurrentDomain.SetData(QR_COLOR_STRING, value.ToXrgb());
                }
                else
                {
                    System.AppDomain.CurrentDomain.SetData(QR_COLOR_STRING, qrColorString);
                    System.AppDomain.CurrentDomain.SetData(QR_COLOR, ColorFrom.FromHtml(qrColorString));
                }
            }
        }

        public static bool FortuneBool
        {
            get
            {
                if (System.AppDomain.CurrentDomain.GetData(FORTUNE_BOOL) == null) 
                    System.AppDomain.CurrentDomain.SetData(FORTUNE_BOOL, false);
                else
                {
                    bool fortuneBool = (bool)System.AppDomain.CurrentDomain.GetData(FORTUNE_BOOL);
                    System.AppDomain.CurrentDomain.SetData(FORTUNE_BOOL, !fortuneBool);
                }

                return (bool)System.AppDomain.CurrentDomain.GetData(FORTUNE_BOOL);
            }
        }

        #endregion properties
    }

}