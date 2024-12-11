using Area23.At.Framework.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area23.At.Www.S.Util
{
    internal static class HostLogHelper
    {
        internal static string UserHost
        {
            get
            {
                string userHost = Constants.UNKNOWN;
                try
                {
                    userHost = (!string.IsNullOrEmpty(HttpContext.Current.Request.UserHostName)) ?
                        HttpContext.Current.Request.UserHostName :
                        (HttpContext.Current.Request.UserHostAddress ?? "unknown");
                }
                catch (Exception ex)
                {
                    userHost = Constants.UNKNOWN;
                    Area23Log.LogStatic(ex);
                }

                return userHost;
            }
        }


        internal static void LogRequest(object sender, EventArgs e, string preMsg = "")
        {
            object oNull = (object)(Constants.SNULL);
            string logMsg = String.Format("{0} {1} object sender={2}, EventArgs e={3}",
                UserHost,
                preMsg ?? "",
                (sender ?? oNull).ToString(),
                (e ?? oNull).ToString());

            Area23Log.LogStatic(logMsg);
        }

        internal static string LogRequest(object sender, EventArgs e)
        {
            object oNull = (object)(Constants.SNULL);
            string logReq = String.Format("from {0} object sender={1}, EventArgs e={2}",
                UserHost,
                (sender ?? oNull).ToString(),
                (e ?? oNull).ToString());

            return logReq;
        }
    }
}