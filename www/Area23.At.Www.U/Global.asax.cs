using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace OneLogin.Saml
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Init(object sender, EventArgs e)
        {

        }

        protected void Application_Start(object sender, EventArgs e)
        {
            string msg = String.Format("application started at {0} object sender = {1}, EventArgs e = {2}",
                DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                (sender == null) ? "(null)" : sender.ToString(),
                (e == null) ? "(null)" : e.ToString());
            Area23Log.LogStatic(msg);
            Area23Log.LogStatic("logging to logfile = " + Area23Log.LogFile);
        }

        protected void Application_Disposed(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
            string msg = String.Format("application ended at {0} object sender = {1}, EventArgs e = {2}",
                DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                (sender == null) ? "(null)" : sender.ToString(),
                (e == null) ? "(null)" : e.ToString());
            Area23Log.LogStatic(msg);
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            string url = HttpContext.Current.Request.Url.ToString();
            Area23Log.LogStatic(url);

        }


        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            string path = "N/A";
            if (sender is HttpApplication)
                path = ((HttpApplication)sender).Request.Url.PathAndQuery;

            string appLogErr = string.Format("Application_Error: {0}: {1} thrown at path {2}",
                ex.GetType(), ex.Message, path);
            Application[Constants.APP_ERROR] = appLogErr;
            Area23Log.LogOriginMsg("Global.asax", appLogErr);


            CqrException appException = new CqrException(string.Format("Application_Error: {0}: {1} thrown with path {2}",
                ex.GetType(), ex.Message, path), ex);
            CqrException.SetLastException(appException);

            int idx = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf(HttpContext.Current.Request.ApplicationPath);
            if (idx > -1)
            {
                string redir = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, idx);
                redir += (redir.EndsWith("/")) ? "Error.aspx?event=appError" : "/Error.aspx?event=appError";
                Response.Redirect(redir);
            }

            // Response.Redirect(Request.ApplicationPath + "/Error.aspx");
        }


        protected void Session_Start(object sender, EventArgs e)
        {
            string url = HttpContext.Current.Request.Url.ToString();
            Area23Log.LogStatic("Session Start: " + url);
        }


        protected void Session_End(object sender, EventArgs e)
        {
            string url = HttpContext.Current.Request.Url.ToString();
            Area23Log.LogStatic("Session End: " + url);

        }

    }
}