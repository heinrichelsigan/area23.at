using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;

namespace Area23.At.Mono.Settings
{

    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BuildTests();
            BuildSettingsTable();
            BuildTableRuntime();
            
        }

        protected void BuildSettingsTable()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            try
            {
                settings.Add("APP_DIR", Constants.APP_DIR);
                settings.Add("AppPath", LibPaths.AppPath);
                settings.Add("BaseAppPath", LibPaths.BaseAppPath);
                settings.Add("ResAppPath", LibPaths.ResAppPath);
                settings.Add("CssAppPath", LibPaths.CssAppPath);
                settings.Add("OutAppPath", LibPaths.OutAppPath);
            }
            catch (Exception ex0)
            {
                DivTest0.InnerHtml = $"<p>Exception ${ex0.GetType()} {ex0.Message}</p><pre>\n{ex0.ToString()}</pre>";
            }
            try
            {
                settings.Add("SystemDirPath", LibPaths.SystemDirPath);
                settings.Add("SystemDirResPath", LibPaths.SystemDirResPath);
                settings.Add("SystemDirOutPath", LibPaths.SystemDirOutPath);
                settings.Add("SystemDirQrPath", LibPaths.SystemDirQrPath);
                settings.Add("SystemDirJsonPath", LibPaths.SystemDirJsonPath);
                settings.Add("AttachmentFilesDir", LibPaths.AttachmentFilesDir);
                settings.Add("SystemDirLogPath", LibPaths.SystemDirLogPath);
                settings.Add("LogFileSystemPath", LibPaths.LogFileSystemPath);
                settings.Add("AppLogFile", Constants.AppLogFile);
            }
            catch (Exception ex1)
            {
                DivTest1.InnerHtml = $"<p>Exception ${ex1.GetType()} {ex1.Message}</p><pre>\n{ex1.ToString()}</pre>";
            }
            try
            {                
                settings.Add("Area23Log.LogFile", SLog.LogFile);
                settings.Add("UserHostAddress", Request.UserHostAddress);
                settings.Add("RawUrl", Request.RawUrl);
                settings.Add("UserAgent", Request.UserAgent);
            } 
            catch (Exception ex2)
            {
                DivTest2.InnerHtml = $"<p>Exception ${ex2.GetType()} {ex2.Message}</p><pre>\n{ex2.ToString()}</pre>";
            }

            try
            {
                foreach (string key in settings.Keys)
                {
                    TableRow row = new TableRow();
                    TableCell cellName = new TableCell();
                    cellName.Text = key.ToString();
                    TableCell cellValue = new TableCell();
                    cellValue.Text = settings[key].ToString();
                    row.Cells.Add(cellName);
                    row.Cells.Add(cellValue);
                    TableSettings.Rows.Add(row);
                }
            }
            catch (Exception ex3)
            {
                DivTest2.InnerHtml += $"<p>Exception ${ex3.GetType()} {ex3.Message}</p><pre>\n{ex3.ToString()}</pre>";
            }

        }


        protected void BuildTableRuntime()
        {
            Dictionary<string, string> rtSettings = new Dictionary<string, string>();

            rtSettings.Add("SesionID", HttpContext.Current.Session.SessionID.ToString());
            for (int i = 0; i < HttpContext.Current.Session.Count; i++) 
            {
                string skey = HttpContext.Current.Session.Keys[i].ToString();
                string sv = HttpContext.Current.Session[skey].ToString();
                rtSettings.Add(skey, sv);
            }

            rtSettings.Add("ApplicationID", "");
            foreach (string akey in HttpContext.Current.Application.AllKeys)
            {
                string av = HttpContext.Current.Application[akey].ToString();
                rtSettings.Add(akey, av);
            }

            foreach (string ck in ConfigurationManager.AppSettings.AllKeys)
            {
                string cv = ConfigurationManager.AppSettings[ck].ToString();
                rtSettings.Add(ck, cv);
            }

            foreach (string rtKey in rtSettings.Keys)
            {
                TableRow row = new TableRow();
                TableCell cellName = new TableCell();
                cellName.Text = rtKey.ToString();
                TableCell cellValue = new TableCell();
                cellValue.Text = rtSettings[rtKey].ToString();
                row.Cells.Add(cellName);
                row.Cells.Add(cellValue);
                TableRuntime.Rows.Add(row);
            }
        }


        protected void BuildTests()
        {
            try
            {
                Area23Log.LogStatic("Log test from " + Request.UserHostAddress + " " + Request.UserAgent);
                DivTest1.InnerHtml = $"<p>{DateTime.Now.Area23DateTimeWithMillis()} LogStatic to {SLog.LogFile} successfull!</p>";
            }
            catch (Exception ex1)
            {
                DivTest1.InnerHtml = $"<p>Exception ${ex1.GetType()} {ex1.Message}</p><pre>\n{ex1.ToString()}</pre>";
            }
            try
            {
                Area23Log.LogStatic("Log test from " + Request.UserHostAddress + " " + Request.UserAgent);
                DivTest2.InnerHtml = $"<p>{DateTime.Now.Area23DateTimeWithMillis()} Logger.Log to {SLog.LogFile} successfull!</p>";
            }
            catch (Exception ex2)
            {
                DivTest1.InnerHtml = $"<p>Exception ${ex2.GetType()} {ex2.Message}</p><pre>\n{ex2.ToString()}</pre>";
            }
        }

    }


}