using Area23.At.Www.Common;
using Area23.At.Framework.Library;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Area23.At.Www.Common
{
    public static class Paths
    {
        private static string appPath = null;
        private static string appDirPath = null;
        private static string baseAppPath = null;
        private static string resAppPath = null;
        private static string qrAppPath = null;
        private static string unixAppPath = null;
        private static string cardPicsPath = null;
        private static string cardPicsDir = null;
        

        public static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        public static string AppPath
        {
            get
            {
                if (String.IsNullOrEmpty(appPath))
                {
                    string apPath = HttpContext.Current.Request.Url.ToString().Replace("/Unix/", "/").Replace("/Qr/", "/").
                        Replace("/res/", "/").Replace("/js/", "/").Replace("/image/", "/").Replace("/css/", "/");
                    // appPath = HttpContext.Current.Request.ApplicationPath;
                    appPath = apPath.Substring(0, apPath.LastIndexOf("/"));
                    if (!appPath.EndsWith("/"))
                        appPath += "/";
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
                    string basApPath = HttpContext.Current.Request.Url.ToString().Replace("/Unix/", "/").Replace("/Qr/", "/").
                        Replace("/res/", "/").Replace("/js/", "/").Replace("/image/", "/").Replace("/css/", "/");
                    baseAppPath = basApPath.Substring(0, basApPath.LastIndexOf("/"));
                    if (!baseAppPath.EndsWith("/"))
                        baseAppPath += "/";
                }
                return baseAppPath;
            }
        }

        public static string ResAppPath
        {
            get
            {
                if (String.IsNullOrEmpty(resAppPath))
                {                    
                    resAppPath = BaseAppPath;
                    if (!resAppPath.Contains("/" + Constants.RES_FOLDER + "/"))
                        resAppPath += Constants.RES_FOLDER + "/";
                }
                return resAppPath;
            }
        }

        public static string JsAppPath { get => ResAppPath + Constants.JS_DIR + "/"; }

        public static string CssAppPath { get => ResAppPath + Constants.CSS_DIR + "/"; }    

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
        

        public static string AppFolder
        {
            get
            {
                try
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["AppFolder"] != null)
                        return System.Configuration.ConfigurationManager.AppSettings["AppFolder"];
                }
                catch (Exception appFolderEx) 
                {
                    Area23Log.LogStatic(appFolderEx);
                }
                return Constants.APP_DIR;
            }
        }

        public static string AppDirPath
        {
            get
            {
                if (String.IsNullOrEmpty(appDirPath))
                {
                    appDirPath = Area23.At.Framework.Library.LibPaths.AppDirPath;
                                       
                    if (!Directory.Exists(appDirPath))
                    {
                        string dirNotFoundMsg = String.Format("application directory {0} doesn't exist, creating it!", appDirPath);
                        Area23Log.LogStatic(dirNotFoundMsg);                        
                    }
                }

                return appDirPath;
            }
        }

        public static string OutDir
        {
            get
            {
                string resPath = AppDirPath;
                
                if (!resPath.Contains(Constants.RES_FOLDER))
                    resPath += Constants.RES_FOLDER + SepChar;

                if (!Directory.Exists(resPath))
                {
                    string dirNotFoundMsg = String.Format("res directory {0} doesn't exist, creating it!", resPath);
                    Area23Log.LogStatic(dirNotFoundMsg);
                    Directory.CreateDirectory(resPath);                    
                }
                return resPath;
            }
        }

        public static string BinDir { get => Area23.At.Framework.Library.LibPaths.BinDir; }

        public static string LogPathDir { get => Area23.At.Framework.Library.LibPaths.LogPathDir; }

        public static string LogFile { get => Area23.At.Framework.Library.LibPaths.LogFile; }

        public static string QrDirPath { get => AppDirPath + Constants.QR_DIR + SepChar; }

        public static string Utf8PathDir { get => AppDirPath + Constants.UTF8_DIR + SepChar; }
    }
}