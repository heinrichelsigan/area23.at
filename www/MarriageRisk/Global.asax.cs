using Area23.At.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Area23.At.Web.S
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Init(object sender, EventArgs e)
        {

        }

        protected void Application_Start(object sender, EventArgs e)
        {
            //string msg = String.Format("application started at {0} object sender = {1}, EventArgs e = {2}",
            //    DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
            //    (sender == null) ? "(null)" : sender.ToString(),
            //    (e == null) ? "(null)" : e.ToString());
            //Area23Log.Logger.Log(msg);
            //Area23Log.Logger.Log("logging to logfile = " + Area23Log.LogFile);
        }

        protected void Application_Disposed(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //string msg = String.Format("application ended at {0} object sender = {1}, EventArgs e = {2}",
            //    DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
            //    (sender == null) ? "(null)" : sender.ToString(),
            //    (e == null) ? "(null)" : e.ToString());
            //Area23Log.Logger.Log(msg);
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
            string msg = String.Format("application error at {0} object sender = {1}, EventArgs e = {2}",
                DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                (sender == null) ? "(null)" : sender.ToString(),
                (e == null) ? "(null)" : e.ToString());
            Area23Log.LogStatic(msg);
        }


        protected void Session_Start(object sender, EventArgs e)
        {
            string msg = String.Format("new session started at {0} object sender = {1}, EventArgs e = {2}",
                DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                (sender == null) ? "(null)" : sender.ToString(),
                (e == null) ? "(null)" : e.ToString());
            Area23Log.LogStatic(msg);
        }


        protected void Session_End(object sender, EventArgs e)
        {
            string msg = String.Format("session ended at {0} object sender = {1}, EventArgs e = {2}",
                DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                (sender == null) ? "(null)" : sender.ToString(),
                (e == null) ? "(null)" : e.ToString());
            Area23Log.LogStatic(msg);
        }
    }
}