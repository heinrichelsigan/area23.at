using Area23.At.Framework.Core.Cqr;
using Area23.At.Framework.Core.Util;
using System.Diagnostics;
using System.Reflection;

namespace Area23.At.Framework.Core.Static
{

    /// <summary>
    /// Most simple & tiny static logger
    /// </summary>
    public static class SLog
    {
        private static readonly Lock _lock = new Lock(), _outerLock = new Lock();
        private static int checkedToday = DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)).Day;

        /// <summary>
        /// LogFile
        /// </summary>
        public static string LogFile { get; private set; }

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
        /// private static ctor of SLog
        /// </summary>
        static SLog() {; }


        public static void SetLogFileByAppName(string appName = "")
        {
            LogFile = (!string.IsNullOrEmpty(appName)) ? LibPaths.GetLogFilePath(appName) : LibPaths.LogFileSystemPath; 
        }

        /// <summary>
        /// Log - static logging method
        /// </summary>
        /// <param name="msg">message to log</param>
        /// <param name="appName">application name</param>
        public static void Log(string msg, string appName = "")
        {
            Area23Log.LogOriginMsg("SLog", msg);
            //if (Constants.NOLog)
            //{
            //    Console.Error.WriteLine(msg);
            //    return;
            //}

            //string logMsg = string.Empty, errMsg = string.Empty;

            //lock (_outerLock)
            //{
            //    if (string.IsNullOrEmpty(LogFile) || !CheckedToday || !File.Exists(LogFile))
            //    {
            //        LogFile = (!string.IsNullOrEmpty(appName)) ? LibPaths.GetLogFilePath(appName) : LibPaths.LogFileSystemPath;

            //        if (!File.Exists(LogFile))
            //        {
            //            lock (_lock)
            //            {
            //                try
            //                {
            //                    File.Create(LogFile);
            //                }
            //                catch (Exception exLogFiteCreate)
            //                {
            //                    ; // throw
            //                    Console.Error.WriteLine("Exception creating logfile: " + exLogFiteCreate.ToString());
            //                }
            //            }
            //        }
            //    }

            //    string logFile1 = "";
            //    logMsg = DateTime.Now.Area23DateTimeWithSeconds() + "\t " + (string.IsNullOrEmpty(msg) ? string.Empty : (msg.EndsWith("\n") ? msg : msg + "\n"));
            //    try
            //    {
            //        lock (_lock)
            //        {
            //            File.AppendAllText(LogFile, logMsg, System.Text.Encoding.UTF8);
            //        }
            //    }
            //    catch (Exception exLogWrite)
            //    {
            //        errMsg = String.Format("{0} \tWriting to file {1} Exception {2} {3} \n{4}\n",
            //                DateTime.Now.Area23DateTimeWithSeconds(), LogFile, exLogWrite.GetType(), exLogWrite.Message, exLogWrite.ToString());
            //        System.AppDomain.CurrentDomain.SetData(Constants.LOG_EXCEPTION_STATIC, errMsg);
            //        Console.Error.WriteLine(errMsg);

            //        logFile1 = (string.IsNullOrEmpty(LogFile)) ? LibPaths.LogFileSystemPath : LogFile;
            //        logFile1 = logFile1.Replace(".log", "_1.log");
            //    }

            //    if (!string.IsNullOrEmpty(logFile1))
            //    {
            //        try
            //        {
            //            if ((AppDomain.CurrentDomain.GetData(Constants.LOG_EXCEPTION_STATIC) != null) && 
            //                ((errMsg = AppDomain.CurrentDomain.GetData(Constants.LOG_EXCEPTION_STATIC).ToString()) != null && errMsg != ""))
            //            {
            //                lock (_lock)
            //                {
            //                    File.AppendAllText(logFile1, errMsg, System.Text.Encoding.UTF8);
            //                    errMsg = "";
            //                }
            //            }
            //            lock (_lock)
            //            {
            //                File.AppendAllText(logFile1, logMsg, System.Text.Encoding.UTF8);
            //            }
            //        }
            //        catch (Exception exLog)
            //        {
            //            errMsg = String.Format("{0} \tWriting to file {1} Exception {2} {3} \n{4}\n",
            //                DateTime.Now.Area23DateTimeWithSeconds(), logFile1, exLog.GetType(), exLog.Message, exLog.ToString());
            //            System.AppDomain.CurrentDomain.SetData(Constants.LOG_EXCEPTION_STATIC, errMsg);
            //            Console.Error.WriteLine(errMsg);
            //        }
            //    }
            //}

        }

        /// <summary>
        /// Log - static logging method
        /// </summary>
        /// <param name="exLog"><see cref="Exception"/> to log</param>
        /// <param name="appName">application name</param>
        public static void Log(Exception exLog, string appName = "")
        {
            string methodBase = "unknown";
            try
            {
                MethodBase mBase = (new StackFrame(1))?.GetMethod();
                methodBase = mBase.ToString();
            }
            catch
            {
                methodBase = "unknown";
            }

            string excMsg = String.Format("{0}throwed {1} ⇒ {2}\t{3}\nStacktrace: \t{4}\n",
                methodBase,
                exLog.GetType(),
                exLog.Message,
                exLog.ToString().Replace("\r", "").Replace("\n", " "),
                exLog.StackTrace?.Replace("\r", "").Replace("\n", " "));

            Area23Log.LogOriginMsg("SLog", excMsg);
            // Log(excMsg, appName);
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
            string methodBase = "";
            string stackTrace = (exLog != null && !string.IsNullOrEmpty(exLog.StackTrace)) ? exLog.StackTrace.Replace("\r", "").Replace("\n", " ") : "";
            string exLogType = (exLog != null && !string.IsNullOrEmpty(exLog.GetType().ToString())) ? exLog.GetType().ToString() : "(NULL)";
            string exLogMsg = (exLog != null && !string.IsNullOrEmpty(exLog.Message)) ? exLog.Message : "(NULL)";
            string exLogString = (exLog != null) ? exLog.ToString().Replace("\r", "").Replace("\n", " ") : "(NULL)";


            try
            {
                MethodBase mBase = (new StackFrame(1))?.GetMethod();
                methodBase = mBase.ToString();
            }
            catch
            {
                methodBase = "unknown";
            }


            string msgPrefix = String.Format("{0}{1} throwed Exception {2}",
                methodBase,
                (string.IsNullOrEmpty(prefix) ? "" : prefix),
                exLogType);

            string exMsg = String.Format("{0} ⇒ \t{1}\t{2}\nStacktrace: \t{3}",
                exLogType,
                exLogMsg,
                exLogString,
                stackTrace);

            Area23Log.LogOriginMsg("SLog", string.Concat(msgPrefix, "\n \t", exMsg));
            // Log(string.Concat(msgPrefix, "\n \t", exMsg), appName);
        }

    }

}
