using Area23.At.Framework.Library.Static;
using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Area23.At.Framework.Library.Util
{

    /// <summary>
    /// simple singelton logger via NLog
    /// </summary>
    public class Area23Log
    {

        #region static fields and properties

        private static readonly object _lock = new object(), _outerLock = new object();
        private static readonly Lazy<Area23Log> instance = new Lazy<Area23Log>(() => new Area23Log());
        private static Logger nlogger = LogManager.GetCurrentClassLogger();

        private static int checkedToday = DateTime.UtcNow.Date.Day;

        /// <summary>
        /// Get the Logger
        /// </summary>
        public static Area23Log Logger { get => instance.Value; }

        /// <summary>
        /// Checked today if logfiles and other needed resources exist
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

        #endregion static fields and properties

        #region properties

        public static string AppName { get; private set; } = string.Empty;

        /// <summary>
        /// LogFile
        /// </summary>
        public static string LogFile { get; private set; }

        #endregion properties

        #region ctor

        static Area23Log()
        {
            InitNLog("");
        }

        /// <summary>
        /// private Singelton constructor
        /// </summary>
        public Area23Log()
        {
            InitNLog("");
        }

        /// <summary>
        /// private Singelton constructor
        /// </summary>
        public Area23Log(string appName = "")
        {
            if (!string.IsNullOrEmpty(appName))
                AppName = appName;

            InitNLog(AppName);
        }


        #endregion ctor

        #region static members


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
            string logMsg = string.Empty, errMsg = string.Empty, allLogMsg = string.Empty;

            lock (_outerLock)
            {
                if (string.IsNullOrEmpty(LogFile) || !CheckedToday || !File.Exists(LogFile))
                {
                    LogFile = (!string.IsNullOrEmpty(appName)) ? LibPaths.GetLogFilePath(appName) : LibPaths.LogFileSystemPath;

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

                try
                {
                    if ((AppDomain.CurrentDomain.GetData(Constants.ALL_KEYS) != null) &&
                        ((allLogMsg = AppDomain.CurrentDomain.GetData(Constants.ALL_KEYS).ToString()) != null && allLogMsg != ""))
                    {
                        lock (_lock)
                        {
                            File.AppendAllText(LogFile, allLogMsg, System.Text.Encoding.UTF8);
                            allLogMsg = "";
                            AppDomain.CurrentDomain.SetData(Constants.ALL_KEYS, allLogMsg);
                        }
                    }
                }
                catch (Exception exLog)
                {
                    errMsg = String.Format("{0} \tWriting to file {1} Exception {2} {3} \n{4}\n",
                        DateTime.Now.Area23DateTimeWithSeconds(), LogFile, exLog.GetType(), exLog.Message, exLog.ToString());
                    AppDomain.CurrentDomain.SetData(Constants.LOG_EXCEPTION_STATIC, errMsg);
                    Console.Error.WriteLine(errMsg);
                }

                logMsg = DateTime.Now.Area23DateTimeWithSeconds() + "\t " + (string.IsNullOrEmpty(msg) ? string.Empty : (msg.EndsWith("\n") ? msg : msg + "\n"));
                try
                {
                    lock (_lock)
                    {
                        File.AppendAllText(LogFile, logMsg, System.Text.Encoding.UTF8);
                    }
                }
                catch (Exception exLogWrite)
                {
                    errMsg = String.Format("{0} \tWriting to file {1} Exception {2} {3} \n{4}\n",
                            DateTime.Now.Area23DateTimeWithSeconds(), LogFile, exLogWrite.GetType(), exLogWrite.Message, exLogWrite.ToString());
                    AppDomain.CurrentDomain.SetData(Constants.LOG_EXCEPTION_STATIC, errMsg);
                    if (AppDomain.CurrentDomain.GetData(Constants.ALL_KEYS) != null)
                        allLogMsg = (string)AppDomain.CurrentDomain.GetData(Constants.ALL_KEYS);
                    allLogMsg += "\n" + logMsg + "\n" + errMsg;
                    AppDomain.CurrentDomain.SetData(Constants.ALL_KEYS, allLogMsg);
                    Console.Error.WriteLine(errMsg);
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

            string excMsg = String.Format("{0} throwed {1} ⇒ {2}\t{3}\nStacktrace: \t{4}\n",
                methodBase,
                exLog.GetType(),
                exLog.Message,
                exLog.ToString().Replace("\r", "").Replace("\n", " "),
                exLog.StackTrace.Replace("\r", "").Replace("\n", " "));

            Log(excMsg, appName);
        }

        public static void LogStatic(string msg, string appName = "") => Area23Log.Log(msg, appName);

        public static void LogStatic(string prefix, Exception xZpd, string appName) => Area23Log.LogOriginMsgEx(appName, prefix, xZpd);

        public static void LogStatic(Exception ex, string appName = "") => Area23Log.Log(ex, appName);

        #endregion static members

        #region members

        /// <summary>
        /// InitNLog init NLog configuration
        /// </summary>
        /// <param name="appName">application name</param>
        protected internal static void InitNLog(string appName = "")
        {
            if (!string.IsNullOrEmpty(appName))
                AppName = appName;

            if (!string.IsNullOrEmpty(AppName))
                LogFile = LibPaths.GetLogFilePath(AppName);
            else
                LogFile = LibPaths.LogFileSystemPath;

            var config = new NLog.Config.LoggingConfiguration();
            // Targets where to log to: File and Console            

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = LogFile };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Trace, LogLevel.Trace, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Debug, logfile);
            config.AddRule(LogLevel.Info, LogLevel.Info, logfile);
            config.AddRule(LogLevel.Warn, LogLevel.Warn, logfile);
            config.AddRule(LogLevel.Error, LogLevel.Error, logfile);
            config.AddRule(LogLevel.Fatal, LogLevel.Fatal, logfile);
            LogManager.Configuration = config; // Apply config
        }

        /// <summary>
        /// log - logs to NLog
        /// </summary>
        /// <param name="msg">debug msg to log</param>
        /// <param name="logLevel">log level: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public void NLog(string msg, int logLevel = 3)
        {
            if (string.IsNullOrEmpty(LogFile) || !CheckedToday)
            {
                lock (_lock)
                {
                    InitNLog(AppName);
                }
            }

            LogLevel nlogLvl = LogLevel.FromOrdinal(logLevel);
            try
            {
                nlogger.Log(nlogLvl, msg);
            }
            catch (Exception exLog)
            {
                AppDomain.CurrentDomain.SetData("LogExceptionNLog",
                    DateTime.Now.Area23DateTimeWithSeconds() + $" \tException: {exLog.GetType()} {exLog.Message} \n" + exLog.ToString());

                Console.Error.WriteLine(Constants.DateArea23Seconds + $" \tException: {exLog.GetType()} {exLog.Message} writing to logfile: {LogFile}\n{exLog}\n");
            }
        }

        #region LogLevelLogger members

        public void LogDebug(string msg)
        {
            NLog(msg, LogLevel.Debug.Ordinal);
        }

        public void LogInfo(string msg)
        {
            NLog(msg, LogLevel.Info.Ordinal);
        }

        public void LogWarn(string msg)
        {
            NLog(msg, LogLevel.Warn.Ordinal);
        }

        public void LogError(string msg)
        {
            NLog(msg, LogLevel.Error.Ordinal);
        }

        #endregion LogLevelLogger members

        /// <summary>
        /// log Exception
        /// </summary>
        /// <param name="ex">Exception ex to log</param>
        /// <param name="level">log level: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public static void Log(Exception ex, int level = 2)
        {
            LogStatic(ex);
        }

        /// <summary>
        /// Log origin with message to NLog
        /// </summary>
        /// <param name="origin">origin of message</param>
        /// <param name="message">enabler message to log</param>
        /// <param name="level">log level: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public static void LogOriginMsg(string origin, string message, int level = 2)
        {
            string logMsg = (string.IsNullOrEmpty(origin) ? "  \t" : origin + " \t") + message;
            LogStatic(logMsg);
        }

        public static void LogOriginEx(string origin, Exception ex, int level = 2)
        {
            string logPrefix = string.IsNullOrEmpty(origin) ? "   " : origin;
            LogStatic($"{logPrefix} \tException {ex.GetType()}: \t{ex.Message}");
            LogStatic($"{logPrefix} \tException {ex.GetType()}: \t{ex.ToString()}");
            if (level < 2)
                LogStatic($"{logPrefix} \t{ex.GetType()} StackTrace: \t{ex.StackTrace}");
        }

        /// <summary>
        /// Log origin with message and thrown exception to NLog
        /// </summary>
        /// <param name="origin">origin of message</param>
        /// <param name="message">logging <see cref="string">string message</see></param>
        /// <param name="ex">logging <see cref="Exception">Exception ex</see></param>
        /// <param name="level"><see cref="int">int log level</see>: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public static void LogOriginMsgEx(string origin, string message, Exception ex, int level = 2)
        {
            string logPrefix = string.IsNullOrEmpty(origin) ? "   " : origin;
            LogStatic($"{logPrefix} \t{message} {ex.GetType()}: \t{ex.Message}");
            LogStatic($"{logPrefix} \tException {ex.GetType()}: \t{ex.ToString()}");
            if (level < 2)
                LogStatic($"{logPrefix} \t{ex.GetType()} StackTrace: \t{ex.StackTrace}");
        }

        #endregion members

    }

}