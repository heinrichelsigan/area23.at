using Area23.At.Framework.Library.Util;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Area23.At.Framework.Library.Static
{

    /// <summary>
    /// a simple static loggrt
    /// </summary>
    public static class SLog
    {

        /// <summary>
        /// Log - static logging method
        /// </summary>
        /// <param name="msg">message to log</param>
        /// <param name="appName">application name</param>
        public static void Log(string msg, string appName = "") => Area23Log.Log(msg, appName);
        




        /// <summary>
        /// Log - static logging method
        /// </summary>
        /// <param name="exLog"><see cref="Exception"/> to log</param>
        /// <param name="appName">application name</param>
        public static void Log(Exception exLog, string appName = "") => Area23Log.Log(exLog, appName);


        /// <summary>
        /// static log with <see cref="string">string prefix</see>, downcasted generic <see cref="Exception/">Exception xZpd</see> 
        /// and <see cref="string">string appName</see>
        /// </summary>
        /// <param name="prefix"><see cref="string"/> prefix</param>
        /// <param name="exLog"><see cref="Exception/">xZpd</param>
        /// <param name="appName"><see cref="string"/> appName</param>
        public static void Log(string prefix, Exception exLog, string appName = "") => Area23Log.LogOriginMsgEx(appName, prefix, exLog);
        

        /// <summary>
        /// private static ctor of SLog
        /// </summary>
        static SLog()
        {
            ;
        }



    }

}
