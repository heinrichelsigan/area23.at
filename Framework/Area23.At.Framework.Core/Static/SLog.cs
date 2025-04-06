using Area23.At.Framework.Core.Util;
using Microsoft.VisualBasic.Logging;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Interop;

namespace Area23.At.Framework.Core.Static
{

    /// <summary>
    /// Most simple & tiny static logger
    /// </summary>
    public static class SLog
    {
        private static readonly Lock _lock = new Lock();
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
            if (Constants.NOLog)
            {
                Console.Error.WriteLine(msg);
                return;
            }
                
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

            string logFile1 = "";
            lock (_lock)
            {
                try
                {
                    logMsg = DateTime.Now.Area23DateTimeWithSeconds() + " \t" + msg ?? string.Empty + "\n";
                    File.AppendAllText(LogFile, logMsg);
                }
                catch (Exception exLogWrite)
                {
                    System.AppDomain.CurrentDomain.SetData("LogExceptionStatic",
                    DateTime.Now.Area23DateTimeWithSeconds() + $" \tWriting to file {LogFile} Exception {exLogWrite.GetType()} {exLogWrite.Message} \n" + exLogWrite.ToString());

                    Console.Error.WriteLine(DateTime.Now.Area23DateTimeWithSeconds() + $" \tException: {exLogWrite.GetType()} {exLogWrite.Message} writing to logfile: {LogFile}");

                    logFile1 = (string.IsNullOrEmpty(LogFile)) ? LibPaths.LogFileSystemPath : LogFile;
                    logFile1 = logFile1.Replace(".log", "_1.log");
                }
            }

            lock (_lock)
            {
                if (!string.IsNullOrEmpty(logFile1))
                {
                    try
                    {
                        logMsg = DateTime.Now.Area23DateTimeWithSeconds() + " \t" + msg ?? string.Empty + "\n";
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

        }


        /// <summary>
        /// Log - static logging method
        /// </summary>
        /// <param name="exLog"><see cref="Exception"/> to log</param>
        /// <param name="appName">application name</param>
        public static void Log(Exception exLog, string appName = "")
        {
            MethodBase? mBase = (new StackFrame(1))?.GetMethod();

            string excMsg = String.Format("{0}throwed {1} ⇒ {2}\t{3}\nStacktrace: \t{4}\n",
                (mBase != null) ? mBase.ToString() + " " : "",
                exLog.GetType(),
                exLog.Message,
                exLog.ToString().Replace("\r", "").Replace("\n", " "),
                exLog.StackTrace?.Replace("\r", "").Replace("\n", " "));

            if (Constants.NOLog)
            {
                Console.Error.WriteLine(excMsg);
                return; 
            }
               
            Log(excMsg, appName);                
        }

        /// <summary>
        /// static log with <see cref="string">string prefix</see>, downcasted generic <see cref="Exception/">Exception xZpd</see> 
        /// and <see cref="string">string appName</see>
        /// </summary>
        /// <param name="prefix"><see cref="string"/> prefix</param>
        /// <param name="exLog"><see cref="Exception/">xZpd</param>
        /// <param name="appName"><see cref="string"/> appName</param>
        public static void Log(string prefix, Exception exLog, string appName = "")
        {
            MethodBase? mBase = (new StackFrame(1))?.GetMethod();

            string msgPrefix = String.Format("{0}{1} throwed Exception {2}",
                (mBase != null) ? mBase.ToString() + " " : "",
                prefix ?? "",
                exLog.GetType());

            string exMsg = String.Format("{0} ⇒ \t{1}\t{2}\nStacktrace: \t{3}",
                exLog.GetType(),
                exLog.Message,
                exLog.ToString().Replace("\r", "").Replace("\n", " "),
                exLog.StackTrace?.Replace("\r", "").Replace("\n", " "));

            if (Constants.NOLog)
            {
                Console.Error.WriteLine(string.Concat(msgPrefix, "\n \t", exMsg));
                return;
            }
            
            Log(string.Concat(msgPrefix, "\n \t", exMsg), appName);                
        }


        /// <summary>
        /// private static ctor of SLog
        /// </summary>
        static SLog() { ; }

    }
}
