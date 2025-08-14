using Area23.At.Framework.Library.Static;
using NLog;
using System;

namespace Area23.At.Framework.Library.Util
{

    /// <summary>
    /// simple singelton logger via NLog
    /// </summary>
    public class Area23Log
    {

        #region static fields and properties

        private static readonly object _lock = new object();
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

        public string AppName { get; private set; } = string.Empty;

        /// <summary>
        /// LogFile
        /// </summary>
        public string LogFile { get; private set; }

        #endregion properties

        #region ctor

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

        public static void LogStatic(string msg, string appName = "") => Area23Log.Logger.LogOriginMsg(appName, msg);

        public static void LogStatic(string prefix, Exception xZpd, string appName) => Area23Log.Logger.LogOriginMsgEx(appName, prefix, xZpd);

        public static void LogStatic(Exception ex, string appName = "") => Area23Log.Logger.LogOriginEx(appName, ex);

        #endregion static members

        #region members

        /// <summary>
        /// InitNLog init NLog configuration
        /// </summary>
        /// <param name="appName">application name</param>
        protected internal void InitNLog(string appName = "")
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
        public void Log(string msg, int logLevel = 3)
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
            Log(msg, LogLevel.Debug.Ordinal);
        }

        public void LogInfo(string msg)
        {
            Log(msg, LogLevel.Info.Ordinal);
        }

        public void LogWarn(string msg)
        {
            Log(msg, LogLevel.Warn.Ordinal);
        }

        public void LogError(string msg)
        {
            Log(msg, LogLevel.Error.Ordinal);
        }

        #endregion LogLevelLogger members

        /// <summary>
        /// log Exception
        /// </summary>
        /// <param name="ex">Exception ex to log</param>
        /// <param name="level">log level: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public void Log(Exception ex, int level = 2)
        {
            Log(ex.Message, level);
            if (level < 4)
                Log(ex.ToString(), level);
            if (level < 2)
                Log(ex.StackTrace, level);
        }

        /// <summary>
        /// Log origin with message to NLog
        /// </summary>
        /// <param name="origin">origin of message</param>
        /// <param name="message">enabler message to log</param>
        /// <param name="level">log level: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public void LogOriginMsg(string origin, string message, int level = 2)
        {
            string logMsg = (string.IsNullOrEmpty(origin) ? "  \t" : origin + " \t") + message;
            Log(logMsg, level);
        }

        public void LogOriginEx(string origin, Exception ex, int level = 2)
        {
            string logPrefix = string.IsNullOrEmpty(origin) ? "   " : origin;
            Log($"{logPrefix} \tException {ex.GetType()}: \t{ex.Message}", level);
            if (level < 4)
                Log($"{logPrefix} \tException {ex.GetType()}: \t{ex.ToString()}", level);
            if (level < 2)
                Log($"{logPrefix} \t{ex.GetType()} StackTrace: \t{ex.StackTrace}", level);
        }

        /// <summary>
        /// Log origin with message and thrown exception to NLog
        /// </summary>
        /// <param name="origin">origin of message</param>
        /// <param name="message">logging <see cref="string">string message</see></param>
        /// <param name="ex">logging <see cref="Exception">Exception ex</see></param>
        /// <param name="level"><see cref="int">int log level</see>: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public void LogOriginMsgEx(string origin, string message, Exception ex, int level = 2)
        {
            string logPrefix = string.IsNullOrEmpty(origin) ? "   " : origin;
            Log($"{logPrefix} \t{message} {ex.GetType()}: \t{ex.Message}", level);
            if (level < 4)
                Log($"{logPrefix} \tException {ex.GetType()}: \t{ex.ToString()}", level);
            if (level < 2)
                Log($"{logPrefix} \t{ex.GetType()} StackTrace: \t{ex.StackTrace}", level);
        }

        public void SetLogFileByAppName(string appName = "") => InitNLog(appName);

        #endregion members

    }

}