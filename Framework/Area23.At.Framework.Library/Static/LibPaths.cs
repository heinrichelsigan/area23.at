using Area23.At.Framework.Library.Util;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;

namespace Area23.At.Framework.Library.Static
{

    /// <summary>
    /// LibPaths provides filesystem paths & directories for different needed locations, e.g. log & config files
    /// </summary>
    public static class LibPaths
    {
        // private static string appPath = "";
        private static string appUrlPath = "";
        private static string baseAppPath = "";
        private static string systemDirPath = "";
        private static string systemDirResPath = "";
        private static string logDirPath = "";
        private static string logFilePath = "";
        private static string cqrServiceSoap = "";
        private static string cqrServiceSoap12 = "";
        private static int daysave = -1;


        public static char SepCh { get => Path.DirectorySeparatorChar; }

        public static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        #region Web App Paths

        public static string AppUrlPath
        {
            get
            {
                if (string.IsNullOrEmpty(appUrlPath))
                {                    
                    try
                    {
                        string appUrl = HttpContext.Current.Request.Url.ToString();
                        string reqAppPath = HttpContext.Current.Request.ApplicationPath.ToString();
                        int idx = appUrl.IndexOf(reqAppPath);
                        if (idx > -1)
                            appUrlPath = appUrl.Substring(0, idx);
                    } 
                    catch (Exception)
                    {
                        appUrlPath = "";
                    }

                    if (string.IsNullOrEmpty(appUrlPath) && System.Configuration.ConfigurationManager.AppSettings["AppUrl"] != null)
                        appUrlPath = System.Configuration.ConfigurationManager.AppSettings["AppUrl"].ToString();

                    if (!appUrlPath.EndsWith("/"))
                        appUrlPath += "/";
                }

                return appUrlPath;
            }
        }

        public static string AppPath => AppUrlPath;       

        public static string BaseAppPath
        {
            get
            {
                if (String.IsNullOrEmpty(baseAppPath))
                {
                    string basApPath = "";
                    
                    if (Constants.UNIX && System.Configuration.ConfigurationManager.AppSettings["BaseAppPathUnix"] != null)
                        basApPath = System.Configuration.ConfigurationManager.AppSettings["BaseAppPathUnix"];
                    else if (Constants.WIN32 && System.Configuration.ConfigurationManager.AppSettings["BaseAppPathWin"] != null)
                        basApPath = System.Configuration.ConfigurationManager.AppSettings["BaseAppPathWin"];

                    if (String.IsNullOrEmpty(basApPath))
                    {
                        basApPath = AppUrlPath;
                    }

                    baseAppPath = (!basApPath.EndsWith("/")) ? basApPath + "/" : basApPath;                    
                }

                return baseAppPath;
            }
        }

        public static string ResAppPath { get => BaseAppPath + Constants.RES_DIR + "/"; }

        public static string CalcAppPath { get => BaseAppPath + Constants.CALC_DIR + "/"; }

        public static string EncodeAppPath { get => BaseAppPath + Constants.CRYPT_DIR + "/"; }

        public static string GamesAppPath { get => BaseAppPath + Constants.GAMES_DIR + "/"; }

        public static string QrAppPath { get => BaseAppPath + Constants.QR_DIR + "/"; }

        public static string UnixAppPath { get => BaseAppPath + Constants.UNIX_DIR + "/"; }

        public static string MyIpAppPath { get => BaseAppPath + Constants.MYIP_DIR + "/"; }

        public static string CssAppPath { get => ResAppPath + Constants.CSS_DIR + "/"; }

        public static string JsAppPath { get => ResAppPath + Constants.JS_DIR + "/"; }

        public static string OutAppPath { get => ResAppPath + Constants.OUT_DIR + "/"; }

        public static string TextAppPath { get => ResAppPath + Constants.TEXT_DIR + "/"; }

        #endregion Web App Paths

        #region WebServices

        public static string CqrServiceSoap
        {
            get
            {
                if (string.IsNullOrEmpty(cqrServiceSoap))
                {
                    if (ConfigurationManager.AppSettings["CqrServiceSoap"] != null)
                        cqrServiceSoap = ConfigurationManager.AppSettings["CqrServiceSoap"].ToString();
                    else
                        cqrServiceSoap = "https://srv.cqrxs.eu/v1.1/CqrService.asmx";
                }
                return cqrServiceSoap;
            }
        }

        public static string CqrServiceSoap12
        {
            get
            {
                if (string.IsNullOrEmpty(cqrServiceSoap12))
                {
                    if (ConfigurationManager.AppSettings["CqrServiceSoap12"] != null)
                        cqrServiceSoap12 = ConfigurationManager.AppSettings["CqrServiceSoap12"].ToString();
                    else
                        cqrServiceSoap12 = "https://srv.cqrxs.eu/v1.1/CqrService.asmx";
                }
                return cqrServiceSoap12;
            }
        }

        #endregion WebServices

        #region directory & file paths

        /// <summary>
        /// SystemDirPath return system directory path, 
        /// if defined in App.Config, 
        /// otherwise applcation directory of base exe.
        /// </summary>
        public static string SystemDirPath
        {
            get
            {
                if (string.IsNullOrEmpty(systemDirPath))
                {
                    for (int sysDirTry = 0; sysDirTry < 6; sysDirTry++)
                    {
                        switch (sysDirTry)
                        {
                            case 0: if (AppContext.BaseDirectory != null) systemDirPath = AppContext.BaseDirectory; break;
                            case 1: if (AppDomain.CurrentDomain != null) systemDirPath = AppDomain.CurrentDomain.BaseDirectory; break;
                            case 2: systemDirPath = Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().Location); break;
                            case 3:
                            default: systemDirPath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location); break;
                        }

                        if (!string.IsNullOrEmpty(systemDirPath) && Directory.Exists(systemDirPath))
                            break;
                    }

                    if (!systemDirPath.EndsWith(SepChar))
                        systemDirPath += SepChar;

                    string sysDir = systemDirPath;
                    if (sysDir.EndsWith($"{SepChar}{Constants.WIN_X86}{SepChar}") || sysDir.EndsWith($"{SepChar}{Constants.WIN_X64}{SepChar}"))
                        sysDir = sysDir.Replace($"{SepChar}{Constants.WIN_X86}{SepChar}", SepChar).Replace($"{SepChar}{Constants.WIN_X64}{SepChar}", SepChar);
                    if (sysDir.EndsWith($"{SepChar}{Constants.NET9_WINDOWS7}{SepChar}") || sysDir.EndsWith($"{SepChar}{Constants.NET9_WINDOWS8}{SepChar}"))
                        sysDir = sysDir.Replace($"{SepChar}{Constants.NET9_WINDOWS7}{SepChar}", SepChar).Replace($"{SepChar}{Constants.NET9_WINDOWS8}{SepChar}", SepChar);
                    if (sysDir.EndsWith($"{SepChar}{Constants.RELEASE_DIR}{SepChar}") || sysDir.EndsWith($"{SepChar}{Constants.DEBUG_DIR}{SepChar}"))
                        sysDir = sysDir.Replace($"{SepChar}{Constants.RELEASE_DIR}{SepChar}", SepChar).Replace($"{SepChar}{Constants.DEBUG_DIR}{SepChar}", SepChar);
                    if (sysDir.EndsWith($"{SepChar}{Constants.BIN_DIR}{SepChar}") || sysDir.EndsWith($"{SepChar}{Constants.OBJ_DIR}{SepChar}"))
                        sysDir = sysDir.Replace($"{SepChar}{Constants.BIN_DIR}{SepChar}", SepChar).Replace($"{SepChar}{Constants.OBJ_DIR}{SepChar}", SepChar);

                    if (Directory.Exists(sysDir))
                        systemDirPath = sysDir;

                }

                return systemDirPath;
            }
        }


        public static string SystemDirResPath
        {
            get
            {
                if (String.IsNullOrEmpty(systemDirResPath))
                {
                    systemDirResPath = SystemDirPath + Constants.RES_DIR + SepChar;
                    if (!Directory.Exists(systemDirResPath))
                    {
                        try
                        {
                            string dirNotFoundMsg = String.Format("out directory {0} doesn't exist, creating it!", systemDirResPath);
                            Area23Log.LogOriginMsg("LibPaths", dirNotFoundMsg);
                            Directory.CreateDirectory(systemDirResPath);
                        }
                        catch (Exception ex)
                        {
                            Area23Log.LogOriginMsgEx("LibPaths", "SystemDirResPath.get throwed Exception " + ex.GetType(), ex);
                        }
                    }
                }
                return systemDirResPath;
            }
        }

        public static string SystemDirBinPath { get => SystemDirResPath + Constants.BIN_DIR + SepChar; }

        public static string AdditionalBinDir { get => SystemDirResPath + Constants.BIN_DIR + SepChar; }

        public static string TextDirPath { get => SystemDirResPath + Constants.TEXT_DIR + SepChar; }

        public static string SytemDirUuPath { get => SystemDirResPath + Constants.UU_DIR + SepChar; }

        public static string SystemDirOutPath { get => SystemDirResPath + Constants.OUT_DIR + SepChar; }

        public static string SystemDirTmpPath { get => SystemDirResPath + Constants.TMP_DIR + SepChar; }

        public static string SystemDirQUtf8Path { get => SystemDirResPath + Constants.UTF8_DIR + SepChar; }


        public static string SystemDirQrPath { get => SystemDirPath + Constants.QR_DIR + SepChar; }

        public static string SystemDirJsonPath { get => SystemDirResPath + Constants.JSON_DIR + SepChar; }



        public static string AttachmentFilesDir { get => SystemDirPath + Constants.ATTACH_FILES_DIR + SepChar; }


        #region LogFiles and LogPaths

        /// <summary>
        /// SystemDirLogPath gets the default full path to logfile in file system
        /// </summary>
        public static string SystemDirLogPath
        {
            get
            {
                if (string.IsNullOrEmpty(logDirPath))
                {
                    logDirPath = SystemDirPath + Constants.LOG_DIR + SepChar;

                    if (!Directory.Exists(logDirPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(logDirPath);
                        }
                        catch { }
                    }
                }
                return logDirPath;
            }
        }

        /// <summary>
        /// GetLogFilePath - gets individual named logfile with substring appName
        /// </summary>
        /// <param name="appName">application name to customize logfile name</param>
        /// <returns>Full file path to log file in file system</returns>
        public static string GetLogFilePath(string appName)
        {
            int day = System.DateTime.UtcNow.DayOfYear;
            if (daysave != day)
            {
                daysave = day;
                logFilePath = "";
            }
            if (string.IsNullOrEmpty(logFilePath))
            {
                logFilePath = SystemDirLogPath + DateTime.UtcNow.Area23Date() + Constants.UNDER_SCORE + appName + Constants.LOG_EXT;
                if (!File.Exists(logFilePath))
                {
                    try
                    {
                        File.Create(logFilePath);
                    }
                    catch { }
                }
            }
            return logFilePath;
        }

        public static string LogFileSystemPath { get => SystemDirLogPath + Constants.AppLogFile; }

        #endregion LogFiles and LogPaths

        #endregion directory & file paths

        #region other properties 

        public static bool CqrEncrypt
        {
            get
            {
                bool _cqrEncrypt = Constants.CQR_ENCRYPT;
                try
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["CqrEncrypt"] != null)
                        _cqrEncrypt = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["CqrEncrypt"].ToString());
                }
                catch
                {
                    _cqrEncrypt = true;
                }
                return _cqrEncrypt;
            }
        }

        #endregion other properties 

    }

}