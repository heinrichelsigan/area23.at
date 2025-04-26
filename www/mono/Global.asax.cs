using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Util;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Area23.At.Mono
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Init(object sender, EventArgs e)
        {
            string msg = String.Format("application init at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            HostLogHelper.LogRequest(sender, e, msg);
            HostLogHelper.DeleteFilesInTmpDirectory(LibPaths.SystemDirTmpPath);
        }

        protected void Application_Start(object sender, EventArgs e)
        {            
            string msg = String.Format("application started at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            HostLogHelper.LogRequest(sender, e, msg);                        
            Area23Log.LogStatic("logging to logfile = " + SLog.LogFile);
        }

        protected void Application_Disposed(object sender, EventArgs e) 
        {
            string msg = String.Format("application disposed at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            HostLogHelper.LogRequest(sender, e, msg);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            string msg = String.Format("application ended at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            HostLogHelper.LogRequest(sender, e, msg);
            HostLogHelper.DeleteFilesInTmpDirectory(LibPaths.SystemDirTmpPath);
        }


        /*
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string msg = String.Format("application begin request at {0} object sender = {1}, EventArgs e = {2}",
                DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                (sender == null) ? "(null)" : sender.ToString(),
                (e == null) ? "(null)" : e.ToString());
            Area23Log.LogStatic(msg);
        }


        protected void Application_EndRequest(object sender, EventArgs e)
        {
            string msg = String.Format("application end request at {0} object sender = {1}, EventArgs e = {2}",
                DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                (sender == null) ? "(null)" : sender.ToString(),
                (e == null) ? "(null)" : e.ToString());
            Area23Log.LogStatic(msg);
        }
        */

       
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            string path = "N/A";
            if (sender is HttpApplication)
                path = ((HttpApplication)sender).Request.Url.PathAndQuery;

            Area23Log.LogStatic($"Application_Error: sender  {sender?.GetType()} {sender?.ToString()} EventArgs {e?.GetType()} {e?.ToString()}");

            CqrException appException = new CqrException(
                string.Format("[0}: {1} thrown with path {2}", ex.GetType(), ex.Message, path),
                ex);


            // Response.Redirect(Request.ApplicationPath + "/Error.aspx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            string msg = String.Format("new session started at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));            
            HostLogHelper.LogRequest(sender, e, msg);            
        }


        protected void Session_End(object sender, EventArgs e)
        {           
            string msg = String.Format("session ended at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));            
            HostLogHelper.LogRequest(sender, e, msg);
        }

    }
}