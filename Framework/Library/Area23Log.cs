using Area23.At.Framework.Library;
using NLog;
using System;
using System.IO;

namespace Area23.At.Framework.Library
{
    /// <summary>
    /// simple singelton logger via NLog
    /// </summary>
    public class Area23Log
    {
        private static readonly Lazy<Area23Log> instance = new Lazy<Area23Log>(() => new Area23Log());
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// LogFile
        /// </summary>
        public static string LogFile { get => LibPaths.LogFile; }

        /// <summary>
        /// Get the Logger
        /// </summary>
        public static Area23Log Logger { get => instance.Value; }

        /// <summary>
        /// LogStatic - static logger without Area23Log.Logger singelton
        /// </summary>
        /// <param name="msg">message to log</param>
        public static void LogStatic(string msg)
        {
            string logMsg = string.Empty;
            if (!File.Exists(LogFile))
            {
                try
                {
                    File.Create(LogFile);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Area23.At.Framework.Library.Area23Log Exception: \n" + ex.ToLogMsg());
                }
            }
            try
            {
                logMsg = String.Format("{0} \t{1}\r\n",
                        Constants.DateTimeArea23Seconds,
                        msg);
                File.AppendAllText(LogFile, logMsg);
            }
            catch (Exception e)
            {
                Console.WriteLine("Area23.At.Framework.Library.Area23Log Exception: \n" + e.ToLogMsg());
            }
        }

        /// <summary>
        /// LogStatic - static logger without Area23Log.Logger singelton
        /// </summary>
        /// <param name="exLog"><see cref="Exception"/> to log</param>
        public static void LogStatic(Exception exLog)
        {
            string exLogMsg = exLog.ToLogMsg();
            LogStatic(exLogMsg);
        }

        /// <summary>
        /// private Singelton constructor
        /// </summary>
        private Area23Log()
        {
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
            logger.Log(nlogLvl, msg);
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
        /// LogOriginMsg - logs a msg by origin
        /// </summary>
        /// <param name="origin">Exception source origin, class, instance</param>
        /// <param name="msg">message to log</param>
        /// <param name="level">logging level, default to 2</param>
        public void LogOriginMsg(string origin, string msg, int level = 2)
        {
            Log($"{origin}:\t{msg}", level);
        }

        /// <summary>
        /// LogOriginMsgEx - logs an exception by origin with seperate msg
        /// </summary>
        /// <param name="origin">Exception source origin, class, instance</param>
        /// <param name="msg">message to log</param>
        /// <param name="ex">Exception to log</param>
        /// <param name="level">logging level, default to 2</param>
        public void LogOriginMsgEx(string origin, string msg, Exception ex, int level = 2)
        {
            if (level >= 4)
                Log($"{origin}:\t{msg}\tException: {ex.Message}", level);
            else if (level >= 2 && level < 4)
                Log($"{origin}:\t{msg}\tException: {ex.Message}\r\n{ex}", level);
            else if (level < 2)
                Log($"{origin}:\t{msg}\tException: {ex.Message}\r\n{ex}\r\n{ex.StackTrace}", level);
        }

    }

}