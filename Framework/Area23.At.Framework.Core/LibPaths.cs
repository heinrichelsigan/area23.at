﻿using Area23.At.Framework.Core.Util;
using NLog;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Web;

namespace Area23.At.Framework.Core
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

        public static char SepCh { get => Path.DirectorySeparatorChar; }

        public static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        #region Web App Paths

        public static string AppPath
        {
            get
            {
                if (String.IsNullOrEmpty(appPath))
                {
                    try
                    {
                        if (System.Configuration.ConfigurationManager.AppSettings["AppPath"] != null)
                            appPath = System.Configuration.ConfigurationManager.AppSettings["AppPath"].ToString();
                        if (System.Configuration.ConfigurationManager.AppSettings["AppUrlPath"] != null)
                            appPath = System.Configuration.ConfigurationManager.AppSettings["AppUrlPath"].ToString();
                        if (System.Configuration.ConfigurationManager.AppSettings["AppDir"] != null)
                            appPath = System.Configuration.ConfigurationManager.AppSettings["AppDir"].ToString();
                    }
                    catch (Exception appFolderEx)
                    {
                        Area23Log.LogStatic(appFolderEx);
                    }
                    if (String.IsNullOrEmpty(appPath))
                        appPath = Constants.APP_DIR;
                }
                return appPath;
            }
        }

        public static string BaseAppPath
        {
            get
            {
                if (String.IsNullOrEmpty(baseAppPath))
                {
                    string basApPath = "";
                    if ((SepCh == '/') && (System.Configuration.ConfigurationManager.AppSettings["BaseAppPathUnix"] != null))
                        basApPath = System.Configuration.ConfigurationManager.AppSettings["BaseAppPathUnix"];
                    else if (System.Configuration.ConfigurationManager.AppSettings["BaseAppPathWin"] != null)
                        basApPath = System.Configuration.ConfigurationManager.AppSettings["BaseAppPathWin"];

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


        public static string CssAppPath { get => ResAppPath + Constants.CSS_DIR + "/"; }

        public static string JsAppPath { get => ResAppPath + Constants.JS_DIR + "/"; }

        public static string OutAppPath { get => ResAppPath + Constants.OUT_DIR + "/"; }


        public static string TextAppPath { get => ResAppPath + Constants.TEXT_DIR + "/"; }

        #endregion Web App Paths

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
                            case 0:
                                if (SepChar == "/" && Path.DirectorySeparatorChar == '/' && SepCh == Path.DirectorySeparatorChar &&
                                            ConfigurationManager.AppSettings["AppDirPathUnix"] != null &&
                                            ConfigurationManager.AppSettings["AppDirPathUnix"] != "")
                                    systemDirPath = (string)ConfigurationManager.AppSettings["AppDirPathUnix"]; break;
                            case 1:
                                if (ConfigurationManager.AppSettings["AppDirPathWin"] != null)
                                    systemDirPath = (string)ConfigurationManager.AppSettings["AppDirPathWin"]; break;
                            case 2: systemDirPath = Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().Location); break;
                            case 3: if (AppContext.BaseDirectory != null) systemDirPath = AppContext.BaseDirectory; break;
                            case 4: if (AppDomain.CurrentDomain != null) systemDirPath = AppDomain.CurrentDomain.BaseDirectory; break;
                            case 5:
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


        /// <summary>
        /// SystemDirResPath returns path to subdirector <see cref="Constants.RES_DIR"/> of base directory <see cref="SystemDirPath"/>.
        /// If subdirectory <see cref="Constants.RES_DIR"/> will be created, if it not allready exist inside directory <see cref="SystemDirPath"/>.        
        /// </summary>
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
                            Area23Log.LogStatic(dirNotFoundMsg);
                            Directory.CreateDirectory(systemDirResPath);
                        }
                        catch (Exception ex)
                        {
                            Area23Log.LogStatic(ex);
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


        public static string AttachmentFilesDir { get => SystemDirPath + Constants.ATTACH_FILES_DIR + SepChar; }

        public static string SystemDirLogPath
        {
            get
            {
                if (string.IsNullOrEmpty(logDirPath))
                {
                    logDirPath = SystemDirPath + Constants.LOG_DIR + SepChar;

                    if (!Directory.Exists(logDirPath))
                    {
                        string dirNotFoundMsg = String.Format("{0} directory {1} doesn't exist, creating it!", Constants.LOG_DIR, logDirPath);
                        // Area23Log.LogStatic(dirNotFoundMsg);
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

        public static string LogFileSystemPath { get => SystemDirLogPath + Constants.AppLogFile; }

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