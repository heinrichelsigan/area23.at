using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Util;
using System;
using System.Linq;
using System.Web;

namespace Area23.At.Mono
{
    public class Global : System.Web.HttpApplication
    {
        protected internal static StringComparer OrdCmp { get => StringComparer.OrdinalIgnoreCase; }
        protected internal static StringComparison StrComp { get => StringComparison.OrdinalIgnoreCase; }

        protected internal static readonly object _locker = new object();

        protected void Application_Init(object sender, EventArgs e)
        {
            lock (_locker)
            {
                try
                {
                    Area23Log.InitLog("");
                }
                catch (Exception) { }
                try
                {
                    string msg = String.Format("application init at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
                    Area23Log.LogOriginMsg("Global.asax: Application_Init", msg);
                    HostLogHelper.DeleteFilesInTmpDirectory(LibPaths.SystemDirTmpPath);
                }
                catch (Exception) { }
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            lock (_locker)
            {
                try
                {
                    Area23Log.InitLog("");
                }
                catch (Exception) { }
                try
                {
                    string msg = String.Format("application started at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
                    Area23Log.LogOriginMsg("Global.asax", msg + "\tlogging to logfile = " + Area23Log.LogFile);
                }
                catch (Exception) { }
            }
        }

        protected void Application_Disposed(object sender, EventArgs e)
        {
            try
            {
                string msg = String.Format("application disposed at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
                Area23Log.LogOriginMsg("Global.asax", msg);
            }
            catch (Exception) { }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            try
            {
                string msg = String.Format("application ended at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
                Area23Log.LogOriginMsg("Global.asax", msg);
                HostLogHelper.DeleteFilesInTmpDirectory(LibPaths.SystemDirTmpPath);
            }
            catch (Exception) { }
        }
        
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                string url = HttpContext.Current.Request.Url.ToString().ToLower();
                string page = Request.Url.LocalPath;
                string pageQuery = Request.Url.PathAndQuery;

                if (page.HasString("SAES") && url.EndsWith(".aspx", StrComp) &&
                        !url.HasString(Constants.CRYPT_DIR))                   
                {
                    Response.Redirect(LibPaths.EncodeAppPath + "AesImprove.aspx", false);
                    return;
                }

                if (url.HasString("/bin/") ||
                    url.HasString("/obj/") ||
                    url.HasString("/json/") ||
                    url.HasString("/text/") ||
                    url.HasString("/tmp/") ||
                    url.HasString("/uu/"))
                {
                    Area23Log.LogOriginMsg("Global.asax", "Application_BeginRequest: Url = " + url + " \n\tError.aspx?attack=DirectoryTraversal");
                    Response.Redirect(LibPaths.AppPath + "Error.aspx?attack=DirectoryTraversal");
                    return;
                }

                if (url.HasString("/res/css/") ||
                    url.HasString("/res/gamez") ||
                    url.HasString("/res/img/") ||
                    url.HasString("/res/js/") ||
                    url.HasString("/res/Qr/"))
                {
                    if (!url.HasString(".css") &&
                        !url.HasString(".js") &&
                        !url.HasString(".png") &&
                        !url.HasString(".gif") &&
                        !url.HasString(".jpg") &&
                        !url.HasString(".ogg") &&
                        !url.HasString(".mp3") &&
                        !url.HasString(".m4a") &&
                        !url.HasString(".wav"))
                    {
                        Area23Log.LogOriginMsg("Global.asax", "\"Application_BeginRequest: Url = " + url + " \n\tError.aspx?attack=WrongFileType");
                        Response.Redirect(LibPaths.AppPath + "Error.aspx?attack=WrongFileType");
                        return;
                    }
                }

                if (url.ToLower().Contains("/res/out/"))
                {
                    if (page.HasString(".as") || url.HasString(".razor") || url.HasString(".master") || url.HasString(".cshtml") ||
                        page.EndsWith(".aspx", StrComp) || page.EndsWith(".ascx", StrComp) || page.EndsWith(".asax", StrComp) || page.EndsWith(".asmx", StrComp))
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

                    if (!Utils.AllowExtensionInOut(url))
                    {
                        Area23Log.LogOriginMsg("Global.asax", "Application_BeginRequest: Url = " + url + " seemed to be denied.");
                    }

                }
                
            }
            
        }


        // protected void Application_EndRequest(object sender, EventArgs e) { }
        

       
        protected void Application_Error(object sender, EventArgs e)
        {
            bool redirect = true;
            Exception ex = Server.GetLastError();
            string path = "N/A";
            if (sender is HttpApplication)
                path = ((HttpApplication)sender).Request.Url.PathAndQuery;

            string appLogErr = string.Format("Application_Error: {0}: {1} thrown at path {2}",
                ex.GetType(), ex.Message, path);
            Application[Constants.APP_ERROR] = appLogErr;
            Area23Log.LogOriginMsg("Global.asax", appLogErr);

            if (System.Configuration.ConfigurationSettings.AppSettings["LastError"] != null)
                redirect = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings["RedirectError"]);

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
                msg = String.Format("Session_End: session ended at {0}.",
                    DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            }
            catch
            {
                msg = "Session_End: session ended at.";
            }

            Area23Log.LogOriginMsg("Global.asax", msg);
        }

    }
}