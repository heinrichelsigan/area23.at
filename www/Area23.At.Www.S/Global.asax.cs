using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Www.S.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Area23.At.Www.S
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Init(object sender, EventArgs e)
        {            
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Dictionary<string, Uri> shortenMap = Area23.At.Www.S.Util.JsonHelper.ShortenMapJson; 
            HostLogHelper.LogRequest(sender, e, "Application_Start loaded shortenMap with " + shortenMap.Count + " entries.");
            if (shortenMap == null || shortenMap.Count == 0)
                shortenMap = Area23.At.Www.S.Util.JsonHelper.ShortenMapJson;
        }

        protected void Application_Disposed(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {            
        }


        
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HostLogHelper.LogRequest(sender, e, "begin request");
            
            string url = HttpContext.Current.Request.Url.ToString();            
                
            if (url.Contains('?') || url.Contains(Constants.JSON_SAVE_FILE))
            {
                string hash = (url.IndexOf("?") > -1) ? url.Substring(url.IndexOf("?") + 1) : "#";
                if (hash.Contains('&'))
                {
                    hash = hash.Substring(0, hash.IndexOf("&")); 
                    hash = hash.TrimEnd('&');
                }

                Dictionary<string, Uri> shortenMap = new Dictionary<string, Uri>();
                if (HttpContext.Current.Application[Constants.APP_NAME] != null) 
                    shortenMap = (Dictionary<string, Uri>)(HttpContext.Current.Application[Constants.APP_NAME]);

                if (shortenMap == null || shortenMap.Count == 0)
                    shortenMap = Area23.At.Www.S.Util.JsonHelper.ShortenMapJson;

                if (shortenMap.ContainsKey(hash))
                {
                    Uri redirUri = shortenMap[hash];
                    String msg = String.Format("Hash = {0}, redirecting to {1} ...", hash, redirUri.ToString());
                    Area23Log.LogStatic(msg);
                    if (redirUri.IsAbsoluteUri)
                    {                        
                        Response.Redirect(redirUri.ToString());
                        return;
                    }
                } else
                {
                    String msg = String.Format("Shortenmap with {0} entries, does not contain Hash = {1}!", shortenMap.Keys.Count, hash);
                    Area23Log.LogStatic(msg);
                }

                Response.Redirect(Constants.AREA23_S);
                return;
            }
        }


        //protected void Application_EndRequest(object sender, EventArgs e)
        //{
        //    string msg = String.Format("application end request at {0} object sender = {1}, EventArgs e = {2}",
        //        DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
        //        (sender == null) ? "(null)" : sender.ToString(),
        //        (e == null) ? "(null)" : e.ToString());
        //    Area23Log.LogStatic(msg);
        //}


        protected void Application_Error(object sender, EventArgs e)
        {
            bool redirect = false;
            if (ConfigurationManager.AppSettings["RedirectError"] != null &&
                Convert.ToBoolean(ConfigurationManager.AppSettings["RedirectError"]))
                redirect = true;

            Exception ex = Server.GetLastError();
            string path = "N/A";
            if (sender is HttpApplication)
                path = ((HttpApplication)sender).Request.Url.PathAndQuery;

            if (ex.Message.Contains("does not exist") || ex.Message.ToLower().Contains("not exist"))
            {
                int ix = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf(HttpContext.Current.Request.ApplicationPath);
                if (ix > -1)
                {
                    string redir404 = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, ix);
                    redir404 += (redir404.EndsWith("/")) ? "" : "/";
                    redir404 += "Default.aspx?code=404";
                    Response.Redirect(redir404);
                }
                Response.Redirect(Request.ApplicationPath + "/Default.aspx");
                return;
            }

            string appLogErr = string.Format("Application_Error: {0}: {1} thrown at path {2}",
                ex.GetType(), ex.Message, path);
            Application[Constants.APP_ERROR] = appLogErr;
            Area23Log.LogOriginMsg("Global.asax", appLogErr);


            if (System.Configuration.ConfigurationManager.AppSettings["RedirectError"] != null)
                redirect = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["RedirectError"]);

            CqrException appException = new CqrException(string.Format("Application_Error: {0}: {1} thrown with path {2}",
                ex.GetType(), ex.Message, path), ex);
            CqrException.SetLastException(appException);

            int idx = HttpContext.Current.Request.Url.AbsoluteUri.IndexOf(HttpContext.Current.Request.ApplicationPath);
            if (idx > -1)
            {
                string redir = HttpContext.Current.Request.Url.AbsoluteUri.Substring(0, idx);
                redir += (redir.EndsWith("/")) ? "Error.aspx?event=appError" : "/Error.aspx?event=appError";
                if (redirect)
                    Response.Redirect(redir);
            }

            if (redirect)
                Response.Redirect(Request.ApplicationPath + "/Error.aspx");
        }


        protected void Session_Start(object sender, EventArgs e)
        {
            HostLogHelper.LogRequest(sender, e, "new Session started");
        }


        protected void Session_End(object sender, EventArgs e)
        {
            HostLogHelper.LogRequest(sender, e, "Session ended");            
        }
    }
}