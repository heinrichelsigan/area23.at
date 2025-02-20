﻿using NLog;
using System;
using System.IO;

namespace Area23.At.Framework.Core
{

    /// <summary>
    /// simple singelton logger via NLog
    /// </summary>
    public class Area23Log
    {
        private static readonly Lock _atomicLock = new Lock(), _spinLock = new Lock();
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
            instance.Value.InitNLog(appName);
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
                if (!File.Exists(LogFile))
                {
                    lock (_atomicLock)
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
            lock (_spinLock)
            {
                try
                {
                    logMsg = String.Format("{0} \t{1}\r\n",
                            Constants.DateArea23Seconds,
                            msg);
                    File.AppendAllText(LogFile, logMsg);
                }
                catch (Exception exLogWrite)
                {
                    Console.Error.WriteLine(Constants.DateArea23Seconds + " Area23.At.Mono Exception writing to logfile: " + exLogWrite.ToString());
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
                exLog.StackTrace?.Replace("\r", "").Replace("\n", " "));

            LogStatic(excMsg, appName);
        }

        /// <summary>
        /// private Singelton constructor
        /// </summary>
        private Area23Log() : this("")
        {
        }

        /// <summary>
        /// private Singelton constructor
        /// </summary>
        private Area23Log(string appName = "")
        {
            InitNLog(appName);
        }

        private void InitNLog(string appName = "")
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