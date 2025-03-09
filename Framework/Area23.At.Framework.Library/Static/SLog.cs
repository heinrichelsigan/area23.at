using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.IO;

namespace Area23.At.Framework.Library.Static
{

    public static class SLog
    {
        private static readonly object _lock = new object();
        private static readonly Lazy<Area23Log> _instance = new Lazy<Area23Log>(() => new Area23Log());

        private static int checkedToday = DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Day;

        /// <summary>
        /// LogFile
        /// </summary>
        public static string LogFile { get; private set; }

        public static void SetLogFileByAppName(string appName = "")
        {
            LogFile = (!string.IsNullOrEmpty(appName)) ? LibPaths.GetLogFilePath(appName) : LibPaths.LogFileSystemPath;
        }

        /// <summary>
        /// Get the Logger
        /// </summary>
        public static Area23Log N23Log { get => _instance.Value; }


        /// <summary>
        /// static Checked today if logfiles and other needed resources exist for today
        /// </summary>
        public static bool CheckedToday
        {
            get
            {
                if (DateTime.UtcNow.Day == checkedToday)
                    return true;

                checkedToday = DateTime.UtcNow.Day;
                return false;
            }
        }

        /// <summary>
        /// Log - static logging method
        /// </summary>
        /// <param name="msg">message to log</param>
        /// <param name="appName">application name</param>
        public static void Log(string msg, string appName = "")
        {
            string logMsg = string.Empty;

            if (string.IsNullOrEmpty(LogFile) || !CheckedToday)
            {
                if (!string.IsNullOrEmpty(appName))
                    LogFile = LibPaths.GetLogFilePath(appName);
                else
                    LogFile = LibPaths.LogFileSystemPath;

                if (!File.Exists(LogFile))
                {
                    lock (_lock)
                    {
                        try
                        {
                            File.Create(LogFile);
                        }
                        catch (Exception exLogFiteCreate)
                        {
                            ; // throw
                            Console.Error.WriteLine("Exception creating logfile: " + exLogFiteCreate.ToString());
                        }
                    }
                }
            }


            // LogFile = (string.IsNullOrEmpty(LogFile)) ? LibPaths.LogFileSystemPath : LogFile;           
            try
            {
                msg = msg ?? "";
                logMsg = $"{DateTime.Now.Area23DateTimeWithSeconds()} \t {msg} {Environment.NewLine}";
                File.AppendAllText(LogFile, logMsg);
            }
            catch (Exception exLogWrite)
            {
                System.AppDomain.CurrentDomain.SetData("LogExceptionStatic",
                    DateTime.Now.Area23DateTimeWithSeconds() + $" \tWriting to file {LogFile} Exception {exLogWrite.GetType()} {exLogWrite.Message} \n" + exLogWrite.ToString());

                Console.Error.WriteLine(DateTime.Now.Area23DateTimeWithSeconds() + $" \tException: {exLogWrite.GetType()} {exLogWrite.Message} writing to logfile: {LogFile}");

                string logFile1 = (string.IsNullOrEmpty(LogFile)) ? LibPaths.LogFileSystemPath : LogFile;
                logFile1 = logFile1.Replace(".log", "_1.log");
                try
                {
                    logMsg = $"{DateTime.Now.Area23DateTimeWithSeconds()} \t {msg ?? string.Empty} {Environment.NewLine}";
                    File.AppendAllText(logFile1, logMsg);
                }
                catch (Exception exLog)
                {
                    System.AppDomain.CurrentDomain.SetData("LogExceptionStaticFile1",
                        DateTime.Now.Area23DateTimeWithSeconds() + $" \tWriting to file {logFile1} Exception {exLog.GetType()} {exLog.Message} \n {exLog.ToString()}");

                    Console.Error.WriteLine(DateTime.Now.Area23DateTimeWithSeconds() + $" \tWriting to file {logFile1} Exception {exLog.GetType()} {exLog.Message} \n {exLog.ToString()}");
                }
            }

        }



        /// <summary>
        /// Log - static logging method
        /// </summary>
        /// <param name="exLog"><see cref="Exception"/> to log</param>
        /// <param name="appName">application name</param>
        public static void Log(Exception exLog, string appName = "")
        {
            string excMsg = String.Format("Exception {0} ⇒ {1}\t{2}\t{3}\n{4}",
                exLog.GetType(),
                exLog.Message,
                exLog.ToString().Replace("\r", "").Replace("\n", " "),
                exLog.StackTrace.Replace("\r", "").Replace("\n", " "), 
                Environment.NewLine);

            Log(excMsg, appName);
        }

        /// <summary>
        /// private static ctor of SLog
        /// </summary>
        static SLog()
        {
            ;
        }



    }

}
