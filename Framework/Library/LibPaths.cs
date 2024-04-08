using Area23.At.Framework.Library;
using NLog;
using System;
using System.IO;

namespace Area23.At.Framework.Library
{
    public static class LibPaths
    {
        private static string appDirPath = null;
        
        public static string SepChar { get => Path.DirectorySeparatorChar.ToString(); }

        public static string AppDirPath
        {
            get
            {
                if (String.IsNullOrEmpty(appDirPath))
                {
                    appDirPath = "." + SepChar;

                    if (AppContext.BaseDirectory != null)
                        appDirPath = AppContext.BaseDirectory + SepChar;
                    else if (AppDomain.CurrentDomain != null)
                        appDirPath = AppDomain.CurrentDomain.BaseDirectory + SepChar;                    
                }

                return appDirPath;
            }
        }

        public static string BinDir { get => AppDirPath + "bin" + SepChar; }

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

        public static string LogFile { get => LogPathDir + Constants.AppLogFile; }

    }
}