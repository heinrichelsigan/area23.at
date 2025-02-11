using Area23.At.Framework.Library;
using Area23.At.CqrJd.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Area23.At.CqrJd
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Init(object sender, EventArgs e)
        {            
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Dictionary<string, Uri> shortenMap = JsonHelper.ShortenMapJson;
            HostLogHelper.LogRequest(sender, e, "Application_Start loaded shortenMap with " + shortenMap.Count + " entries.");           
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

                if (shortenMap.Count == 0)
                    shortenMap = JsonHelper.ShortenMapJson;

                if (shortenMap.ContainsKey(hash))
                {
                    Uri redirUri = shortenMap[hash];
                    if (redirUri.IsAbsoluteUri)
                    {
                        String msg = String.Format("Hash = {0}, redirecting to {1} ...", hash, redirUri.ToString());
                        Area23Log.LogStatic(msg);
                        Response.Redirect(redirUri.ToString());
                        return;
                    }
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
            HostLogHelper.LogRequest(sender, e, "Application Error");
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