using area23.at.mono.test.Util;
using System;
using System.IO;
using System.Web;

namespace area23.at.mono.test.Util
{
    public static class Logger
    { 
        public static string LogFile
        {
            get => Paths.LogFile;
        }

        public static void Log(string msg)
        {
            string preMsg = DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss \t");
            string fn = Logger.LogFile;
            File.AppendAllText(fn, preMsg + msg + "\r\n");
        }

    }
}