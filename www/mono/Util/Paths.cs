using Area23.At.Mono;
using Area23.At.Mono.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Util
{
    public static class Paths
    {
        private static string appPath = null;
        private static string resAppPath = null;
        private static string cardPicsPath = null;
        private static string cardPicsDir = null;
        

        public static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        public static string AppPath
        {
            get
            {
                if (String.IsNullOrEmpty(appPath))
                {
                    appPath = HttpContext.Current.Request.ApplicationPath;
                    if (!appPath.EndsWith("/"))
                        appPath += "/";
                }
                return appPath;
            }
        }

        public static string ResAppPath 
        {
            get
            {
                if (String.IsNullOrEmpty(resAppPath))
                {
                    string rsAppPath = HttpContext.Current.Request.Url.ToString().Replace("/Master/", "/").Replace("/Page/", "/").Replace("/Control/", "/");
                    resAppPath = rsAppPath.Substring(0, rsAppPath.LastIndexOf("/"));
                    if (!resAppPath.EndsWith("/"))
                        resAppPath += "/";
                    if (!resAppPath.Contains("/res/"))
                        resAppPath += "res/";
                }
                return resAppPath;
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
   
        public static string OutDir
        {
            get
            {                
                string resPath = "." + SepChar;                

                if (AppContext.BaseDirectory != null)
                    resPath = AppContext.BaseDirectory + SepChar;
                else if (AppDomain.CurrentDomain != null)
                    resPath = AppDomain.CurrentDomain.BaseDirectory + SepChar;

                try
                {
                    if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.ApplicationPath != null)
                    {
                        resPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
                    }
                } 
                catch (Exception ex)
                {
                    Area23Log.LogStatic(ex);
                }

                // if (!resPath.Contains(AppFolder))
                //      resPath += AppFolder + SepChar;
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

        public static string LogFile
        {
            get
            {
                string logAppPath = "." + SepChar;
                if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Request.ApplicationPath == null)
                {
                    if (AppContext.BaseDirectory != null)
                    {
                        logAppPath = AppContext.BaseDirectory + SepChar;
                    }
                    else if (AppDomain.CurrentDomain != null)
                    {
                        logAppPath = AppDomain.CurrentDomain.BaseDirectory + SepChar;
                    }
                }
                else
                {
                    logAppPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + SepChar;
                }

                // if (!logAppPath.Contains(AppFolder))
                //     logAppPath += AppFolder + SepChar;

                logAppPath += String.Format("{0}{1}{2}_{3}.log",
                    Constants.LOG_DIR, SepChar, DateTime.UtcNow.ToString("yyyyMMdd"), Constants.APP_NAME);
                // if (Directory.Exists(logAppPath))
                return logAppPath;
            }
        }

    

    }
}