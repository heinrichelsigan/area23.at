using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Util;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Controls;

namespace Area23.At.Mono
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Init(object sender, EventArgs e)
        {
            string msg = String.Format("application init at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            Area23Log.LogOriginMsg("Global.asax", msg);
            HostLogHelper.DeleteFilesInTmpDirectory(LibPaths.SystemDirTmpPath);
        }

        protected void Application_Start(object sender, EventArgs e)
        {            
            string msg = String.Format("application started at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));                      
            Area23Log.LogOriginMsg("Global.asax", msg + "\tlogging to logfile = " + Area23Log.LogFile);
        }

        protected void Application_Disposed(object sender, EventArgs e) 
        {
            string msg = String.Format("application disposed at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            Area23Log.LogOriginMsg("Global.asax", msg);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            string msg = String.Format("application ended at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            Area23Log.LogOriginMsg("Global.asax", msg);
            HostLogHelper.DeleteFilesInTmpDirectory(LibPaths.SystemDirTmpPath);
        }


        
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                string url = HttpContext.Current.Request.Url.ToString().ToLower();
                string page = Request.Url.LocalPath.ToLower();
                string pageQuery = Request.Url.PathAndQuery;

                if (url.Contains("/bin/") ||
                    url.Contains("/obj/") ||
                    url.Contains("/json/") ||
                    url.Contains("/text/") ||
                    url.Contains("/tmp/") ||
                    url.Contains("/uu/"))
                {
                    Area23Log.LogOriginMsg("Global.asax", "Application_BeginRequest: Url = " + url + " \n\tError.aspx?attack=DirectoryTraversal");
                    Response.Redirect(LibPaths.AppPath + "Error.aspx?attack=DirectoryTraversal");
                    return;
                }

                if (url.Contains("/res/css/") ||
                    url.Contains("/res/gamez") ||
                    url.Contains("/res/img/") ||
                    url.Contains("/res/js/") ||
                    url.Contains("/res/Qr/"))
                {
                    if (!url.Contains(".css") &&
                        !url.Contains(".js") &&
                        !url.Contains(".png") &&
                        !url.Contains(".gif") &&
                        !url.Contains(".jpg") &&
                        !url.Contains(".ogg") &&
                        !url.Contains(".mp3") &&
                        !url.Contains(".m4a") &&
                        !url.Contains(".wav"))
                    {
                        Area23Log.LogOriginMsg("Global.asax", "\"Application_BeginRequest: Url = " + url + " \n\tError.aspx?attack=WrongFileType");
                        Response.Redirect(LibPaths.AppPath + "Error.aspx?attack=WrongFileType");
                        return;
                    }
                }

                if (url.ToLower().Contains("/res/out/"))
                {
                    if (page.ToLower().Contains(".as") || url.ToLower().Contains(".as") || url.ToLower().Contains(".master") ||
                        page.ToLower().EndsWith(".aspx") || page.ToLower().EndsWith(".ascx"))
                    {
                        Area23Log.LogOriginMsg("Global.asax", "Application_BeginRequest: Url = " + url + " \n\tError.aspx?attack=codeInjectionUpload");
                        Response.Redirect(LibPaths.AppPath + "Error.aspx?attack=codeInjectionUpload");
                        return;
                    }
                    int idx = url.IndexOf("/res/out/");
                    string restUrl = url.Substring(idx).Replace("/res/out/", "").Replace("res/out/", "");
                    if (restUrl.Contains("/"))
                    {
                        string wrongDir = restUrl.Substring(0, restUrl.IndexOf("/"));
                        Area23Log.LogOriginMsg("Global.asax", "Application_BeginRequest: Url = " + url + " \n\tError.aspx?attack=subDirInOut");
                        Response.Redirect(LibPaths.AppPath + "Error.aspx?attack=subDirInOut");
                        return;
                    }

                    if (!Utils.AllowUrlExtensionInOut(url))
                    {
                        Area23Log.LogOriginMsg("Global.asax", "Application_BeginRequest: Url = " + url + " seemed to be denied.");
                    }

                }
                
            }
            
        }


        protected void Application_EndRequest(object sender, EventArgs e)
        {
            Area23Log.LogOriginMsg("Global.asax", "EndRequest Url: " + HttpContext.Current.Request.Url.ToString());
            
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
            string msg = "";

            try
            {
                msg = String.Format("Session_Start: new session started at {0} from {1} browser {2}",
                    DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                    HttpContext.Current.Request.UserHostAddress,
                    HttpContext.Current.Request.UserAgent);
            }
            catch
            {
                msg = String.Format("Session_Start: new session started at {0}.",
                    DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            }

            Area23Log.LogOriginMsg("Global.asax", msg);
        }


        protected void Session_End(object sender, EventArgs e)
        {
            string msg = "";

            try
            {
                msg = String.Format("Session_End: session ended at {0} from {1} browser {2}",
                    DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                    HttpContext.Current.Request.UserHostAddress,
                    HttpContext.Current.Request.UserAgent);
            }
            catch
            {
                msg = String.Format("Session_End: session ended at {0}.",
                    DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            }

            Area23Log.LogOriginMsg("Global.asax", msg);
        }

    }
}