using Area23.At.Framework.Library.Core;
using Microsoft.AspNetCore.Http.Extensions;
using NLog;
using System;
using System.IO;
using System.Web;

namespace Area23.At.Framework.Library.Core
{

    /// <summary>
    /// LibPaths provides filesystem paths & directories for different needed locations, e.g. log & config files
    /// </summary>
    public static class LibPaths
    {
        private static string appPath = null;
        private static string baseAppPath = null;
        private static string resAppPath = null;
        private static string qrAppPath = null;
        private static string encodeAppPath = null;
        private static string unixAppPath = null;
        private static string calcAppPath = null;
        private static string appDirPath = null;
        private static string outDirPath = null;
        private static string resDirPath = null;


        public static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        public static string AppPath
        {
            get
            {
                if (String.IsNullOrEmpty(appPath))
                {
                    try
                    {
                        if (System.Configuration.ConfigurationManager.AppSettings["AppDir"] != null)
                            appPath = System.Configuration.ConfigurationManager.AppSettings["AppDir"];
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


        public static string AppDirPath
        {
            get
            {
                if (String.IsNullOrEmpty(appDirPath))
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["AppDirPath"] != null)
                        appDirPath = (string)System.Configuration.ConfigurationManager.AppSettings["AppDirPath"];

                    if (!Directory.Exists(appDirPath))
                    {
                        if (AppContext.BaseDirectory != null)
                            appDirPath = AppContext.BaseDirectory + SepChar;
                        else if (AppDomain.CurrentDomain != null)
                            appDirPath = AppDomain.CurrentDomain.BaseDirectory + SepChar;
                    }

                    if (!appDirPath.EndsWith(SepChar))
                        appDirPath += SepChar;
                }

                return appDirPath;
            }
        }

        public static string BaseAppPath
        {
            get
            {
                if (String.IsNullOrEmpty(baseAppPath))
                {
                    string basApPath = HttpContextWrapper.Current.Request.GetDisplayUrl().ToString().
                        Replace("/Unix/", "/").Replace("/Qr/", "/").
                        Replace("/Calc/", "/").Replace("/Enc/", "/").
                        Replace("/res/", "/").Replace("/audio/", "/").Replace("/bin/", "/").
                        Replace("/css/", "/").Replace("/img/", "/").Replace("/js/", "/").
                        Replace("/out/", "/").Replace("/text/", "/").Replace("/fortune.u8", "/").
                        Replace("/log/", "/").Replace("/c/", "/");
                    baseAppPath = basApPath.Substring(0, basApPath.LastIndexOf("/"));
                    if (!baseAppPath.EndsWith("/"))
                        baseAppPath += "/";
                }
                return baseAppPath;
            }
        }

        public static string CalcAppPath
        {
            get
            {
                if (String.IsNullOrEmpty(calcAppPath))
                {
                    calcAppPath = BaseAppPath;
                    if (!calcAppPath.Contains(Constants.CALC_DIR))
                        calcAppPath += Constants.CALC_DIR + "/";
                }
                return calcAppPath;
            }
        }

        public static string CssAppPath { get => ResAppPath + Constants.CSS_DIR + "/"; }

        public static string JsAppPath { get => ResAppPath + Constants.JS_DIR + "/"; }

        public static string ResAppPath
        {
            get
            {
                if (String.IsNullOrEmpty(resAppPath))
                {
                    resAppPath = BaseAppPath;
                    if (!resAppPath.Contains("/" + Constants.RES_DIR + "/"))
                        resAppPath += Constants.RES_DIR + "/";
                }
                return resAppPath;
            }
        }

        public static string ResDirPath
        {
            get
            {
                if (String.IsNullOrEmpty(resDirPath))
                {
                    resDirPath = AppDirPath;
                    if (!resDirPath.Contains(Constants.RES_DIR))
                        resDirPath += Constants.RES_DIR + SepChar;

                    if (!Directory.Exists(resDirPath))
                    {
                        try
                        {
                            string dirNotFoundMsg = String.Format("out directory {0} doesn't exist, creating it!", resDirPath);
                            Area23Log.LogStatic(dirNotFoundMsg);
                            Directory.CreateDirectory(resDirPath);
                        }
                        catch (Exception ex)
                        {
                            Area23Log.LogStatic(ex);
                        }
                    }
                }
                return resDirPath;
            }
        }

        public static string UnixAppPath
        {
            get
            {
                if (String.IsNullOrEmpty(unixAppPath))
                {
                    unixAppPath = BaseAppPath;
                    if (!unixAppPath.Contains("/" + Constants.UNIX_DIR + "/"))
                        unixAppPath += Constants.UNIX_DIR + "/";
                }
                return unixAppPath;
            }
        }

        public static string QrAppPath
        {
            get
            {
                if (String.IsNullOrEmpty(qrAppPath))
                {
                    qrAppPath = BaseAppPath;
                    if (!qrAppPath.Contains("/" + Constants.QR_DIR + "/"))
                        qrAppPath += Constants.QR_DIR + "/";
                }
                return qrAppPath;
            }
        }

        public static string EncodeAppPath
        {
            get
            {
                if (String.IsNullOrEmpty(encodeAppPath))
                {
                    encodeAppPath = BaseAppPath;
                    if (!encodeAppPath.Contains("/" + Constants.ENCODE_DIR + "/"))
                        encodeAppPath += Constants.ENCODE_DIR + "/";
                }
                return encodeAppPath;
            }
        }


        public static string AdditionalBinDir { get => ResDirPath + Constants.BIN_DIR + SepChar; }

        public static string TextDirPath { get => ResDirPath + Constants.TEXT_DIR + SepChar; }

        public static string TextAppPath { get => ResAppPath + Constants.TEXT_DIR + "/"; }


        public static string LogPathDir
        {
            get
            {
                string logPath = AppDirPath;

                if (!logPath.Contains(Constants.LOG_DIR))
                    logPath += Constants.LOG_DIR + SepChar;

                if (!Directory.Exists(logPath))
                {
                    string dirNotFoundMsg = String.Format("{0} directory {1} doesn't exist, creating it!", Constants.LOG_DIR, logPath);
                    Area23Log.LogStatic(dirNotFoundMsg);
                    Directory.CreateDirectory(logPath);
                }
                return logPath;
            }
        }

        public static string LogPathFile { get => LogPathDir + Constants.AppLogFile; }

        public static string OutAppPath { get => ResAppPath + Constants.OUT_DIR + "/"; }

        public static string OutDirPath
        {
            get
            {
                if (String.IsNullOrEmpty(outDirPath))
                {
                    outDirPath = AppDirPath;
                    if (!outDirPath.Contains(Constants.RES_DIR))
                        outDirPath += Constants.RES_DIR + SepChar;
                    if (!outDirPath.Contains(Constants.OUT_DIR))
                        outDirPath += Constants.OUT_DIR + SepChar;

                    if (!Directory.Exists(outDirPath))
                    {
                        try
                        {
                            string dirNotFoundMsg = String.Format("out directory {0} doesn't exist, creating it!", outDirPath);
                            Area23Log.LogStatic(dirNotFoundMsg);
                            Directory.CreateDirectory(outDirPath);
                        }
                        catch (Exception ex)
                        {
                            Area23Log.LogStatic(ex);
                        }
                    }
                }
                return outDirPath;
            }
        }

        public static string BinDir { get => OutDirPath + "bin" + SepChar; }

        public static string QrDirPath { get => AppDirPath + Constants.QR_DIR + SepChar; }

        public static string Utf8PathDir { get => AppDirPath + Constants.UTF8_DIR + SepChar; }


        //public static string LogFile
        //{
        //    get
        //    {
        //        string logAppPath = "." + SepChar;
        //        if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Request.ApplicationPath == null)
        //        {
        //            if (AppContext.BaseDirectory != null)
        //            {
        //                logAppPath = AppContext.BaseDirectory + SepChar;
        //            }
        //            else if (AppDomain.CurrentDomain != null)
        //            {
        //                logAppPath = AppDomain.CurrentDomain.BaseDirectory + SepChar;
        //            }
        //        }
        //        else
        //        {
        //            logAppPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
        //        }

        //        logAppPath += String.Format("{0}{1}{2}_{3}.log",
        //            Constants.LOG_DIR, SepChar, DateTime.UtcNow.ToString("yyyyMMdd"), Constants.APP_NAME);
        //        // if (Directory.Exists(logAppPath))
        //        return logAppPath;
        //    }
        //}

    }

}