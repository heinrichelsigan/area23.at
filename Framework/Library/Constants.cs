using System;

namespace Area23.At.Framework.Library
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
        public const string LOG_EXT = ".log";
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
        /// DateArea23 - UTC Date Formatted
        /// </summary>
        public static string DateArea23 { get => DateTime.UtcNow.Area23Date(); }

        /// <summary>
        /// UT DateTime @area23.at including seconds
        /// </summary>
        public static string DateTimeArea23Seconds { get => DateTime.UtcNow.Area23DateTimeWithSeconds(); }

        /// <summary>
        /// DateTimeArea23 
        /// </summary>
        public static string DateTimeArea23 { get => DateTime.UtcNow.Area23DateTime(); }

        /// <summary>
        /// AppLogFile - logfile with <see cref="At.Framework.Library.Extensions.Area23Date(DateTime)"/> prefix
        /// </summary>
        public static string AppLogFile { get => DateTime.UtcNow.Area23Date() + UNDER_SCORE + APP_NAME + LOG_EXT; }

        #endregion properties
    }
}