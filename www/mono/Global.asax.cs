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

        protected void Application_Init(object sender, EventArgs e)
        {
            string msg = String.Format("application init at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            Area23Log.Logger.LogOriginMsg("Global.asax", msg);
            HostLogHelper.DeleteFilesInTmpDirectory(LibPaths.SystemDirTmpPath);
        }

        protected void Application_Start(object sender, EventArgs e)
        {            
            string msg = String.Format("application started at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));                      
            Area23Log.Logger.LogOriginMsg("Global.asax", msg + "\tlogging to logfile = " + SLog.LogFile);
        }

        protected void Application_Disposed(object sender, EventArgs e) 
        {
            string msg = String.Format("application disposed at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            Area23Log.Logger.LogOriginMsg("Global.asax", msg);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            string msg = String.Format("application ended at {0} ", DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"));
            Area23Log.Logger.LogOriginMsg("Global.asax", msg);
            HostLogHelper.DeleteFilesInTmpDirectory(LibPaths.SystemDirTmpPath);
        }


        
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                string url = HttpContext.Current.Request.Url.ToString().ToLower();
                string page = Request.Url.LocalPath.ToLower();
                string pageQuery = Request.Url.PathAndQuery;

                if (url.Contains("/res/bin/") ||
                    url.Contains("/res/json/") ||
                    url.Contains("/res/text/") ||
                    url.Contains("/res/tmp/") ||
                    url.Contains("/res/uu/"))
                {
                    Area23Log.Logger.LogOriginMsg("Global.asax", "Url: " + url + " \tError.aspx?attack=DirectoryTraversal");
                    Response.Redirect(LibPaths.AppPath + "Error.aspx?attack=DirectoryTraversal");
                    return;
                }

                if (url.Contains("/res/css/") ||
                    url.Contains("/res/gamez") ||
                    url.Contains("/res/img/") ||
                    url.Contains("/res/js/") ||
                    url.Contains("/res/Qr/"))
                {
                    if (!page.Contains(".css") &&
                        !page.Contains(".js") &&
                        !page.Contains(".png") &&
                        !page.Contains(".gif") &&
                        !page.Contains(".jpg") &&
                        !page.Contains(".ogg") &&
                        !page.Contains(".mp3") &&
                        !page.Contains(".m4a") &&
                        !page.Contains(".wav"))
                    {
                        Area23Log.Logger.LogOriginMsg("Global.asax", "Url: " + url + " \tError.aspx?attack=WrongFileType");
                        Response.Redirect(LibPaths.AppPath + "Error.aspx?attack=WrongFileType");
                        return;
                    }
                }
                if (url.Contains("/res/out/") &&

                    !page.Contains(".asc") &&
                    !page.Contains(".md") &&
                    !page.Contains(".txt") &&
                    !page.Contains(".text") &&
                    !page.Contains(".cfg") &&

                    !page.Contains(".css") &&
                    !page.Contains(".js") &&
                    !page.Contains(".htm") &&
                    !page.Contains(".html") &&
                    !page.Contains(".xhtml") &&
                    !page.Contains(".xml") &&
                    !page.Contains(".json") &&
                    !page.Contains(".rdf") &&

                    !page.Contains(".avif") &&
                    !page.Contains(".bmp") &&
                    !page.Contains(".exif") &&
                    !page.Contains(".gif") &&
                    !page.Contains(".png") &&
                    !page.Contains(".ico") &&
                    !page.Contains(".ief") &&
                    !page.Contains(".jpg") &&
                    !page.Contains(".jpeg") &&
                    !page.Contains(".pcx") &&
                    !page.Contains(".pic") &&
                    !page.Contains(".psd") &&
                    !page.Contains(".tif") &&
                    !page.Contains(".xcf") &&
                    !page.Contains(".xif") &&

                    !page.Contains(".3pg") &&
                    !page.Contains(".3g2") &&
                    !page.Contains(".aif") &&
                    !page.Contains(".au") &&
                    !page.Contains(".m3u") &&
                    !page.Contains(".mid") &&
                    !page.Contains(".mp4") &&
                    !page.Contains(".mpeg") &&
                    !page.Contains(".ogg") &&
                    !page.Contains(".webm") &&
                    !page.Contains(".wav") &&
                    !page.Contains(".wax") &&
                    !page.Contains(".wma") &&
                    !page.Contains(".mp3") &&

                    !page.Contains(".avi") &&
                    !page.Contains(".f4v") &&
                    !page.Contains(".flx") &&
                    !page.Contains(".m4u") &&
                    !page.Contains(".m4v") &&
                    !page.Contains(".mov") &&
                    !page.Contains(".mpg") &&
                    !page.Contains(".wmv") &&

                    !page.Contains(".pdf") &&
                    !page.Contains(".ps") &&
                    !page.Contains(".gs") &&
                    !page.Contains(".dvi") &&
                    !page.Contains(".tex") &&
                    !page.Contains(".ods") &&
                    !page.Contains(".odt") &&
                    !page.Contains(".rtf") &&

                    !page.Contains(".doc") &&
                    !page.Contains(".dot") &&
                    !page.Contains(".xls") &&
                    !page.Contains(".xlt") &&
                    !page.Contains(".csv") &&
                    !page.Contains(".mdb") &&
                    !page.Contains(".ppt") &&
                    !page.Contains(".vsx") &&
                    !page.Contains(".vst") &&
                    !page.Contains(".mpp") &&

                    !page.Contains(".ttf") &&
                    !page.Contains(".woff") &&

                    !page.Contains(".eml") &&
                    !page.Contains(".mbox") &&
                    !page.Contains(".vcs") &&
                    !page.Contains(".vcf") &&

                    !page.Contains(".zip") &&
                    !page.Contains(".tar") &&
                    !page.Contains(".tgz") &&
                    !page.Contains(".tbz") &&
                    !page.Contains(".rar") &&
                    !page.Contains(".arj") &&
                    !page.Contains(".arc") &&
                    !page.Contains(".z") &&
                    !page.Contains(".gz") &&
                    !page.Contains(".bz") &&
                    !page.Contains(".bz2") &&
                    !page.Contains(".7z") &&
                    !page.Contains(".xz") &&
                    !page.Contains(".uu") &&
                    !page.Contains(".base") &&
                    !page.Contains(".mime") &&

                    !page.Contains(".pki") &&
                    !page.Contains(".cer") &&
                    !page.Contains(".der") &&
                    !page.Contains(".crl") &&
                    !page.Contains(".p10") &&
                    !page.Contains(".p7c") &&
                    !page.Contains(".p7s") &&

                    !page.Contains(".exe") &&
                    !page.Contains(".dll") &&
                    !page.Contains(".oct") &&
                    !page.Contains(".bin") &&
                    !page.Contains(".tmp") &&
                    !page.Contains(".img"))
                {
                    Area23Log.Logger.LogOriginMsg("Global.asax", "Url: " + url + " \tError.aspx?attack=wrongOutType");
                    Response.Redirect(LibPaths.AppPath + "Error.aspx?attack=wrongOutType");
                    return;
                }
            }
            
        }


        protected void Application_EndRequest(object sender, EventArgs e)
        {
            Area23Log.Logger.LogOriginMsg("Global.asax", "EndRequest Url: " + HttpContext.Current.Request.Url.ToString());
            
        }
        

       
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            string path = "N/A";
            if (sender is HttpApplication)
                path = ((HttpApplication)sender).Request.Url.PathAndQuery;

            Area23Log.Logger.LogOriginMsg("Global.asax", string.Format("Application_Error: {0}: {1} thrown with path {2}", 
                ex.GetType(), ex.Message, path));
            
            CqrException appException = new CqrException(string.Format("Application_Error: {0}: {1} thrown with path {2}",
                ex.GetType(), ex.Message, path), ex);
            CqrException.SetLastException(appException);

            // Response.Redirect(Request.ApplicationPath + "/Error.aspx");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            string msg = String.Format("new session started at {0} from {1} browser {2}",
                DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                HttpContext.Current.Request.UserHostAddress,
                HttpContext.Current.Request.UserAgent);

            Area23Log.Logger.LogOriginMsg("Global.asax", msg);
        }


        protected void Session_End(object sender, EventArgs e)
        {           
            string msg = String.Format("session ended at {0} from {1} browser {2}",
                DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss"),
                HttpContext.Current.Request.UserHostAddress,
                HttpContext.Current.Request.UserAgent);
            Area23Log.Logger.LogOriginMsg("Global.asax", msg);
        }

    }
}