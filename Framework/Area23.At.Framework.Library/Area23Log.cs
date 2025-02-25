using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Util;
using NLog;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Area23.At.Framework.Library
{

    /// <summary>
    /// simple singelton logger via NLog
    /// </summary>
    public class Area23Log
    {
        private static readonly object _lock = new object();
        private static readonly Lazy<Area23Log> instance = new Lazy<Area23Log>(() => new Area23Log());
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static int checkedToday = DateTime.Today.Day;

        private static readonly string area23LogFile = LibPaths.LogFileSystemPath; 
        
        /// <summary>
        /// LogFile
        /// </summary>
        public static string LogFile { get; private set; }

        public static void SetLogFileByAppName(string appName = "")
        {
            LogFile = (!string.IsNullOrEmpty(appName)) ? LibPaths.GetLogFilePath(appName) : area23LogFile;  
            InitNLog(appName);
        }

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

        /// <summary>
        /// LogStatic - static logger without Area23Log.Logger singelton
        /// </summary>
        /// <param name="msg">message to log</param>
        /// <param name="appName">application name</param>
        public static void LogStatic(string msg, string appName = "")
        {
            string logMsg = string.Empty;
            if (!string.IsNullOrEmpty(appName))
                LogFile = LibPaths.GetLogFilePath(appName);

            if (!CheckedToday)
            {
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
                logMsg = DateTime.Now.Area23DateTimeWithSeconds() + " \t" + msg ?? string.Empty + "\r\n";                  
                File.AppendAllText(LogFile, logMsg);
            }
            catch (Exception exLogWrite)
            {
                if (!HttpContext.Current.Application.AllKeys.Contains("LogExceptionStatic"))
                    HttpContext.Current.Application.Add("LogExceptionStatic",
                        DateTime.Now.Area23DateTimeWithSeconds() + $" \tWriting to file {LogFile} Exception {exLogWrite.GetType()} {exLogWrite.Message} \n" + exLogWrite.ToString());
                else
                    HttpContext.Current.Application.Set("LogExceptionStatic",
                        DateTime.Now.Area23DateTimeWithSeconds() + $" \tWriting to file {LogFile} Exception {exLogWrite.GetType()} {exLogWrite.Message} \n" + exLogWrite.ToString());

                Console.Error.WriteLine(DateTime.Now.Area23DateTimeWithSeconds() + $" \tException: {exLogWrite.GetType()} {exLogWrite.Message} writing to logfile: {LogFile}");
                
                string logFile1 = (string.IsNullOrEmpty(LogFile)) ? LibPaths.LogFileSystemPath : LogFile;                
                logFile1 = logFile1.Replace(".log", "_1.log");
                try
                {
                    logMsg = DateTime.Now.Area23DateTimeWithSeconds() + " \t" + msg ?? string.Empty + "\r\n";
                    File.AppendAllText(logFile1, logMsg);
                }
                catch (Exception exLog)
                {
                    HttpContext.Current.Application["LogExceptionStaticFile1"] =
                        DateTime.Now.Area23DateTimeWithSeconds() + $" \tWriting to file {logFile1} Exception {exLog.GetType()} {exLog.Message} \n {exLog.ToString()}";
                    
                    Console.Error.WriteLine(DateTime.Now.Area23DateTimeWithSeconds() + $" \tWriting to file {logFile1} Exception {exLog.GetType()} {exLog.Message} \n {exLog.ToString()}");
                }
            }

        }



        /// <summary>
        /// LogStatic - static logger without Area23Log.Logger singelton
        /// </summary>
        /// <param name="exLog"><see cref="Exception"/> to log</param>
        /// <param name="appName">application name</param>
        public static void LogStatic(Exception exLog, string appName = "")
        {
            string excMsg = String.Format("Exception {0} ⇒ {1}\t{2}\t{3}",
                exLog.GetType(),
                exLog.Message,
                exLog.ToString().Replace("\r", "").Replace("\n", " "),
                exLog.StackTrace.Replace("\r", "").Replace("\n", " "));

            LogStatic(excMsg, appName);
        }

        /// <summary>
        /// private Singelton constructor
        /// </summary>
        static Area23Log() 
        {
            InitNLog("");
        }

        /// <summary>
        /// private Singelton constructor
        /// </summary>
        public Area23Log(string appName = "")
        {
            InitNLog(appName);
        }

        static void InitNLog(string appName = "")
        {
            if (string.IsNullOrEmpty(appName))
            {
                appName = Constants.APP_NAME;
                LogFile = area23LogFile;
            }
            else
                LogFile = LibPaths.GetLogFilePath(appName);

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
            NLog.LogManager.Configuration = config; // Apply config
        }

        /// <summary>
        /// log - logs to NLog
        /// </summary>
        /// <param name="msg">debug msg to log</param>
        /// <param name="logLevel">log level: 0 for Trace, 1 for Debug, ..., 4 for Error, 5 for Fatal</param>
        public void Log(string msg, int logLevel = 3)
        {
            NLog.LogLevel nlogLvl = NLog.LogLevel.FromOrdinal(logLevel);
            try
            {
                logger.Log(nlogLvl, msg);
            } 
            catch (Exception exLog)
            {
                HttpContext.Current.Application["LogExceptionNLog"] = 
                    DateTime.Now.Area23DateTimeWithSeconds() + $" \tException: {exLog.GetType()} {exLog.Message} \n" + exLog.ToString();

                Console.Error.WriteLine(Constants.DateArea23Seconds + $" \tException: {exLog.GetType()} {exLog.Message} writing to logfile: {LogFile}\n{exLog}\n");
            }
        }


        #region LogLevelLogger members

        public void LogDebug(string msg)
        {
            Log(msg, NLog.LogLevel.Debug.Ordinal);
        }

        public void LogInfo(string msg)
        {
            Log(msg, NLog.LogLevel.Info.Ordinal);
        }

        public void LogWarn(string msg)
        {
            Log(msg, NLog.LogLevel.Warn.Ordinal);
        }

        public void LogError(string msg)
        {
            Log(msg, NLog.LogLevel.Error.Ordinal);
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
    }


}