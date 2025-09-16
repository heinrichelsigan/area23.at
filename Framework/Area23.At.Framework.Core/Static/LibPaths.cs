using Area23.At.Framework.Core.Util;
// using NLog;
using System.Configuration;
using System.Reflection;

namespace Area23.At.Framework.Core.Static
{


    /// <summary>
    /// LibPaths provides filesystem paths & directories for different needed locations, e.g. log & config files
    /// </summary>
    public static class LibPaths
    {
        private static string appPath = "";
        private static string baseAppPath = "";
        private static string systemDirPath = "";
        private static string systemDirResPath = "";
        private static string logDirPath = "";
        private static string logFilePath = "";
        private static string cqrServiceSoap = "", cqrServiceSoap12 = "", cqrSrvSoap = "", cqrSrvSoap12 = "";
        private static readonly char _sepCh;
        private static int daysave = -1;

        public static bool LogDebug { get; private set; }  

        public static char SepCh { get => _sepCh; }

        public static string SepChar { get => _sepCh.ToString(); }

        /// <summary>
        /// static constructor
        /// </summary>
        static LibPaths()
        {
            _sepCh = Path.DirectorySeparatorChar;
            if (Constants.DirCreate)
            {
                if (Directory.Exists(LibPaths.SystemDirResPath))
                    try
                    {
                        Directory.CreateDirectory(LibPaths.SystemDirResPath);
                    }
                    catch { }
                if (Directory.Exists(LibPaths.SytemDirUuPath))
                    try
                    {
                        Directory.CreateDirectory(LibPaths.SytemDirUuPath);
                    }
                    catch { }
                if (Directory.Exists(LibPaths.SystemDirOutPath))
                    try
                    {
                        Directory.CreateDirectory(LibPaths.SystemDirOutPath);
                    }
                    catch { }
                if (Directory.Exists(LibPaths.SystemDirTmpPath))
                    try
                    {
                        Directory.CreateDirectory(LibPaths.SystemDirTmpPath);
                    }
                    catch { }
                if (Directory.Exists(LibPaths.SystemDirLogPath))
                    try
                    {
                        Directory.CreateDirectory(LibPaths.SystemDirLogPath);
                    }
                    catch { }
            }
        }


        #region Web App Paths

        // TODO: not only via Configuration
        public static string AppPath
        {
            get
            {
                if (string.IsNullOrEmpty(appPath))
                {
                    try
                    {
                        if (ConfigurationManager.AppSettings["AppPath"] != null)
                            appPath = ConfigurationManager.AppSettings["AppPath"].ToString();
                        if (ConfigurationManager.AppSettings["AppUrlPath"] != null)
                            appPath = ConfigurationManager.AppSettings["AppUrlPath"].ToString();
                        if (ConfigurationManager.AppSettings["AppDir"] != null)
                            appPath = ConfigurationManager.AppSettings["AppDir"].ToString();
                    }
                    catch (Exception appFolderEx)
                    {
                        Area23Log.LogOriginMsgEx("LibPaths", "AppPath.get", appFolderEx);     
                    }
                    if (string.IsNullOrEmpty(appPath))
                        appPath = Constants.APP_DIR;
                }
                return appPath;
            }
        }

        public static string BaseAppPath
        {
            get
            {
                if (string.IsNullOrEmpty(baseAppPath))
                {
                    string basApPath = "";
                    if (SepCh == '/' &&
                        ConfigurationManager.AppSettings[Constants.BASE_APP_PATH_UNIX] != null &&
                        ConfigurationManager.AppSettings[Constants.BASE_APP_PATH_UNIX] != "")
                        basApPath = (string)ConfigurationManager.AppSettings[Constants.BASE_APP_PATH_UNIX];
                    else if (ConfigurationManager.AppSettings[Constants.BASE_APP_PATH_WIN] != null)
                        basApPath = (string)ConfigurationManager.AppSettings[Constants.BASE_APP_PATH_WIN];

                    baseAppPath = !basApPath.EndsWith("/") ? basApPath + "/" : basApPath;
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
                    if (ConfigurationManager.AppSettings[Constants.CQR_SERVICE_SOAP] != null)
                        cqrServiceSoap = ConfigurationManager.AppSettings[Constants.CQR_SERVICE_SOAP].ToString();
                    else
                        cqrServiceSoap = "https://srv.cqrxs.eu/v1.2/CqrService.asmx";
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
                    if (ConfigurationManager.AppSettings[Constants.CQR_SERVICE_SOAP12] != null)
                        cqrServiceSoap12 = ConfigurationManager.AppSettings[Constants.CQR_SERVICE_SOAP12].ToString();
                    else
                        cqrServiceSoap12 = "https://srv.cqrxs.eu/v1.2/CqrService.asmx";
                }
                return cqrServiceSoap12;
            }
        }

        public static string CqrServiceSoapV6 { get => CqrServiceSoap12.Replace("://srv.", "://ipv6."); }

        public static string CqrSrvSoap
        {
            get
            {
                if (string.IsNullOrEmpty(cqrSrvSoap))
                {
                    if (ConfigurationManager.AppSettings[Constants.CQR_SRV_SOAP] != null)
                        cqrSrvSoap = (string)ConfigurationManager.AppSettings[Constants.CQR_SRV_SOAP].ToString();
                    else
                        cqrSrvSoap = "https://srv.cqrxs.eu/v1.3/CqrService.asmx";
                }
                return cqrSrvSoap;
            }
        }

        public static string CqrSrvSoap12
        {
            get
            {
                if (string.IsNullOrEmpty(cqrSrvSoap12))
                {
                    if (ConfigurationManager.AppSettings[Constants.CQR_SRV_SOAP12] != null)
                        cqrSrvSoap12 = (string)ConfigurationManager.AppSettings[Constants.CQR_SRV_SOAP12].ToString();
                    else
                        cqrSrvSoap12 = "https://srv.cqrxs.eu/v1.3/CqrService.asmx";
                }
                return cqrSrvSoap12;
            }
        }

        public static string CqrSrvSoapV6 { get => CqrSrvSoap12.Replace("://srv.", "://ipv6."); }

        #endregion WebServices

        #region other properties 

        public static bool CqrEncrypt
        {
            get
            {
                bool _cqrEncrypt = Constants.CQR_ENCRYPT;
                try
                {
                    if (ConfigurationManager.AppSettings["CqrEncrypt"] != null)
                        _cqrEncrypt = Convert.ToBoolean(ConfigurationManager.AppSettings["CqrEncrypt"].ToString());
                }
                catch
                {
                    _cqrEncrypt = true;
                }
                return _cqrEncrypt;
            }
        }

        #endregion other properties 

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
                    for (int sysDirTry = 0; sysDirTry < 8; sysDirTry++)
                    {
                        try
                        {
                            switch (sysDirTry)
                            {
                                case 0:
                                    if (SepChar == "/" && Path.DirectorySeparatorChar == '/' && SepCh == Path.DirectorySeparatorChar &&
                                            ConfigurationManager.AppSettings[Constants.APP_DIR_PATH_UNIX] != null &&
                                            ConfigurationManager.AppSettings[Constants.APP_DIR_PATH_UNIX] != "")
                                        systemDirPath = ConfigurationManager.AppSettings[Constants.APP_DIR_PATH_UNIX];
                                    break;
                                case 1:
                                    if (ConfigurationManager.AppSettings[Constants.APP_DIR_PATH_WIN] != null)
                                        systemDirPath = ConfigurationManager.AppSettings[Constants.APP_DIR_PATH_WIN];
                                    break;
                                case 2: systemDirPath = Path.GetFullPath(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName); break;
                                case 3: systemDirPath = Path.GetFullPath(System.Environment.ProcessPath); break;
                                case 4: systemDirPath = Path.GetFullPath(Assembly.GetExecutingAssembly().Location); break;
                                case 5: systemDirPath = AppContext.BaseDirectory; break;
                                case 6: if (AppDomain.CurrentDomain != null) systemDirPath = AppDomain.CurrentDomain.BaseDirectory; break;
                                case 7: systemDirPath = Path.GetFullPath(Assembly.GetExecutingAssembly().CodeBase); break;
                                case 8: systemDirPath = Path.GetFullPath(System.Environment.GetCommandLineArgs()[0]); break;
                                default:
                                    systemDirPath = Path.GetFullPath(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName); break;
                            }
                        }
                        catch { }

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


        /// <summary>
        /// SystemDirResPath returns path to subdirector <see cref="Constants.RES_DIR"/> of base directory <see cref="SystemDirPath"/>.
        /// If subdirectory <see cref="Constants.RES_DIR"/> will be created, if it not allready exist inside directory <see cref="SystemDirPath"/>.        
        /// </summary>
        public static string SystemDirResPath
        {
            get
            {
                if (string.IsNullOrEmpty(systemDirResPath))
                {
                    systemDirResPath = SystemDirPath + Constants.RES_DIR + SepChar;
                    if (!Directory.Exists(systemDirResPath))
                    {
                        try
                        {
                            string dirNotFoundMsg = string.Format("out directory {0} doesn't exist, creating it!", systemDirResPath);
                            Area23Log.LogOriginMsg("LibPaths", "SystemDirResPath.get: " + dirNotFoundMsg);
                            if (Constants.DirCreate)
                                Directory.CreateDirectory(systemDirResPath);
                        }
                        catch (Exception ex)
                        {
                            Area23Log.LogOriginMsgEx("LibPaths", "SystemDirResPath.get", ex);
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

        #endregion directory & file paths

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
                            if (Constants.DirCreate && !Constants.NOLog)
                                Directory.CreateDirectory(logDirPath);

                        }
                        catch { }
                    }
                }
                return logDirPath;
            }
        }

        public static string LogFileSystemPath { get => SystemDirLogPath + Constants.AppLogFile; }

        /// <summary>
        /// GetLogFilePath - gets individual named logfile with substring appName
        /// </summary>
        /// <param name="appName">application name to customize logfile name</param>
        /// <returns>Full file path to log file in file system</returns>
        public static string GetLogFilePath(string appName)
        {
            int day = DateTime.UtcNow.DayOfYear;
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
                        if (!Constants.NOLog)
                            File.Create(logFilePath);
                    }
                    catch { }
                }
            }
            return logFilePath;
        }


        #endregion LogFiles and LogPaths


    }

}