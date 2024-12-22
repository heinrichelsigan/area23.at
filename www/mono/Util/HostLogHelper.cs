using Area23.At.Framework.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media.Animation;

namespace Area23.At.Mono.Util
{
    /// <summary>
    /// HostLogHelper
    /// </summary>
    public static class HostLogHelper
    {
        private static string userHost = "";
        public static string UserHost
        {
            get
            {                
                if (!string.IsNullOrEmpty(userHost)) 
                    return userHost;
                try
                {
                    userHost = (!string.IsNullOrEmpty(HttpContext.Current.Request.UserHostName)) ?
                        HttpContext.Current.Request.UserHostName :
                        (HttpContext.Current.Request.UserHostAddress ?? Constants.UNKNOWN);
                }
                catch (Exception ex)
                {
                    userHost = Constants.UNKNOWN;
                    Area23Log.LogStatic(ex);
                }

                return userHost;
            }
        }


        public static void LogRequest(object sender, EventArgs e, string preMsg = "")
        {
            object oNull = (object)(Constants.SNULL);
            string logMsg = String.Format("{0} {1} object sender={2}, EventArgs e={3}",
                UserHost,
                preMsg ?? "",
                (sender ?? oNull).ToString(),
                (e ?? oNull).ToString());

            Area23Log.LogStatic(logMsg);
        }

        public static string LogRequest(object sender, EventArgs e)
        {
            object oNull = (object)(Constants.SNULL);
            string logReq = String.Format("from {0} object sender={1}, EventArgs e={2}",
                UserHost,
                (sender ?? oNull).ToString(),
                (e ?? oNull).ToString());

            return logReq;
        }

        public static int DeleteFilesInTmpDirectory(string tmpDir = null)
        {
            int cnt = 0;
            if (string.IsNullOrEmpty(tmpDir))
                tmpDir = LibPaths.SystemDirTmpPath;

            if (Directory.Exists(tmpDir))
            {
                foreach (string file in Directory.GetFiles(tmpDir))
                {
                    try
                    {
                        if (!file.Contains("temp.tmp"))
                        {
                            File.Delete(file);
                            cnt++;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            return cnt;
        }
    }

}