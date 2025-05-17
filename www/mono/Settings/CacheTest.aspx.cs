using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Cache;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Area23.At.Mono.Settings
{

    public partial class CacheTest : System.Web.UI.Page
    {
        bool parallel = false, serial = false;
        int iterations = 128;
        PersistType persistType = PersistType.AppDomain;

        protected void Page_Load(object sender, EventArgs e)
        {
            Enum.TryParse<PersistType>(this.DropDownList_CacheVariant.SelectedValue, out persistType);

            string persistString = this.DropDownList_CacheVariant.SelectedItem.Text;
            if (persistString.StartsWith("AppDomain"))
                persistType = PersistType.AppDomain;
            if (persistString.StartsWith("ApplicationState"))
                persistType = PersistType.ApplicationState;
            if (persistString.StartsWith("JsonFile"))
                persistType = PersistType.JsonFile;
            if (persistString.StartsWith("Redis"))
                persistType = PersistType.Redis;


            if (!Int32.TryParse(this.DropDownList_Iterations.SelectedValue, out iterations))
                iterations = 128;
            foreach (ListItem item in CheckBoxList_TestType.Items)
            {
                if (item.Text == "Parallel")
                    parallel = item.Selected;
                if (item.Text == "Serial")
                    serial = item.Selected;
            }

            if (!IsPostBack)
            {
                OnInit(e);
                // BuildSettingsTable();
                PerformTests(persistType, iterations, parallel, serial);
            }

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                try
                {
                    DivTest0.InnerHtml = string.Empty;
                    DivTest1.InnerHtml = string.Empty;
                    DivTest2.InnerHtml = string.Empty;
                    string log0 = "Log test from " + Request.UserHostAddress + " " + Request.UserAgent + " " + Request.ClientCertificate.Issuer.ToString();
                    Area23Log.LogStatic(log0);
                    DivTest0.InnerHtml += $"<p>{DateTime.Now.Area23DateTimeWithMillis()}: {log0} -> LogStatic to {SLog.LogFile}  successfull!</p>";
                }
                catch (Exception exStart)
                {
                    DivTest0.InnerHtml += $"<p>Exception ${exStart.GetType()} {exStart.Message}</p><pre>\n{exStart.ToString()}</pre>";
                }
            }
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
                settings.Add("SystemDirPath", LibPaths.SystemDirPath);
                settings.Add("SystemDirJsonPath", LibPaths.SystemDirJsonPath);
                settings.Add("SystemDirLogPath", LibPaths.SystemDirLogPath);
                settings.Add("LogFileSystemPath", LibPaths.LogFileSystemPath);
                settings.Add("AppLogFile", Constants.AppLogFile);
                settings.Add("SLog.LogFile", SLog.LogFile);
                settings.Add("UserHostAddress", Request.UserHostAddress);
                settings.Add("RawUrl", Request.RawUrl);
                settings.Add("UserAgent", Request.UserAgent);
            }
            catch (Exception ex0)
            {
                DivTest0.InnerHtml = $"<p>Exception ${ex0.GetType()} {ex0.Message}</p><pre>\n{ex0.ToString()}</pre>";
            }
            try
            {
                long lenSess = 0;
                settings.Add("SesionID", HttpContext.Current.Session.SessionID.ToString());
                settings.Add("Session state all items count ", HttpContext.Current.Session.Keys.Count.ToString());
                for (int i = 0; i < HttpContext.Current.Session.Count; i++)
                {
                    string skey = HttpContext.Current.Session.Keys[i].ToString();
                    string sv = HttpContext.Current.Session[skey].ToString();
                    lenSess += sv.Length;
                    settings.Add(skey, sv);
                }
                settings.Add("Session state all items size ", lenSess.ToString());

                long lenApp = 0;
                settings.Add("Application state all items count ", HttpContext.Current.Application.AllKeys.Length.ToString());
                foreach (string akey in HttpContext.Current.Application.AllKeys)
                {
                    string av = HttpContext.Current.Application[akey].ToString();
                    lenApp += av.Length;
                    settings.Add(akey, av);
                }
                settings.Add("Application state all items size ", lenApp.ToString());

                settings.Add("Headers count", HttpContext.Current.Request.Headers.AllKeys.Length.ToString());
                foreach (string hkey in HttpContext.Current.Request.Headers.Keys)
                {
                    string hval = HttpContext.Current.Request.Headers[hkey].ToString();
                    settings.Add(hkey, hval);
                }

                settings.Add("Cookies count ", HttpContext.Current.Request.Cookies.AllKeys.Length.ToString());
                foreach (string ckey in HttpContext.Current.Request.Cookies.AllKeys)
                {
                    string cval = HttpContext.Current.Request.Cookies[ckey].ToString();
                    settings.Add(ckey, cval);
                }

                settings.Add("AppSettings keys count ", ConfigurationManager.AppSettings.AllKeys.Length.ToString());
                foreach (string appSetKey in ConfigurationManager.AppSettings.AllKeys)
                {
                    string appSetVal = ConfigurationManager.AppSettings[appSetKey].ToString();
                    settings.Add(appSetKey, appSetVal);
                }
            }
            catch (Exception ex1)
            {
                DivTest0.InnerHtml += $"<p>Exception ${ex1.GetType()} {ex1.Message}</p><pre>\n{ex1.ToString()}</pre>";
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
                DivTest0.InnerHtml += $"<p>Exception ${ex3.GetType()} {ex3.Message}</p><pre>\n{ex3.ToString()}</pre>";
            }

        }


        protected void PerformTests(PersistType persistType, int iterations = 128, bool parallel = true, bool serial = true)
        {

            Dictionary<string, string> cachTests = new Dictionary<string, string>();

            try
            {
                if (serial)
                {
                    string serials = RunSerial(persistType, iterations);
                    string serialt = "";
                    if (serials.Contains("\nFinished"))
                        serialt = serials.Substring(serials.LastIndexOf("\nFinished"));
                    cachTests.Add($" Serial  {persistType.ToString()}/{iterations}", serialt);
                    DivTest1.InnerHtml += $"<pre> Serial  {persistType.ToString()}/{iterations}\n" + serials + " </pre>";
                }
            }
            catch (Exception exSerial)
            {
                DivTest2.InnerHtml += $"<p>Exception ${exSerial.GetType()} {exSerial.Message}</p><pre>\n{exSerial.ToString()}</pre>";
            }
            try
            {
                if (parallel)
                {
                    string parallels = RunTasks(persistType, iterations);
                    string parallelt = "";
                    if (parallels.Contains("\nFinished"))
                        parallelt = parallels.Substring(parallels.LastIndexOf("\nFinished"));
                    cachTests.Add($"Parallel {persistType.ToString()}/{iterations}", parallelt);
                    DivTest1.InnerHtml += $"<pre>Parallel {persistType.ToString()}/{iterations}\n" + parallels + "</pre>";
                }
            }
            catch (Exception exParallel)
            {
                DivTest2.InnerHtml += $"<p>Exception ${exParallel.GetType()} {exParallel.Message}</p>";
                DivTest2.InnerHtml += $"<pre>\n{exParallel.ToString()}</pre>";
            }

            foreach (string key in cachTests.Keys)
            {
                try
                {
                    TableRow row = new TableRow();
                    TableCell cellName = new TableCell();
                    cellName.Text = key.ToString();
                    TableCell cellValue = new TableCell();
                    cellValue.Text = cachTests[key].ToString();
                    row.Cells.Add(cellName);
                    row.Cells.Add(cellValue);
                    TableCacheTest.Rows.Add(row);
                }
                catch (Exception exTableRow)
                {
                    try
                    {
                        DivTest2.InnerHtml += $"<p>Exception ${exTableRow.GetType()} {exTableRow.Message}</p>";
                    }
                    catch (Exception exTableRow0)
                    {
                        Area23Log.LogStatic(exTableRow0);
                    }
                    try
                    {
                        DivTest2.InnerHtml += $"<pre>\n{exTableRow.ToString()}</pre>";
                    }
                    catch (Exception exTableRow1)
                    {
                        Area23Log.LogStatic(exTableRow1);
                    }

                }
            }
        }


        static string RunTasks(PersistType persitVariant, int numberOfTasks, short maxKexs = 16)
        {
            string s = "";
            MemoryCache memoryCache = null;
            switch (persitVariant)
            {
                case PersistType.JsonFile:
                    JsonFileCache jsonFileCache = new JsonFileCache(persitVariant);
                    memoryCache = (MemoryCache)jsonFileCache;
                    break;
                case PersistType.ApplicationState:
                    ApplicationStateCache appStateCache = new ApplicationStateCache(persitVariant);
                    memoryCache = (MemoryCache)appStateCache;
                    break;
                case PersistType.Redis:
                    RedisCache redisCache = new RedisCache(persitVariant);
                    memoryCache = (MemoryCache)redisCache;
                    break;
                case PersistType.AppDomain:
                default:
                    AppDomainCache appDomainCache = new AppDomainCache(persitVariant);
                    memoryCache = (MemoryCache)appDomainCache;
                    break;
            }
            string parallelCache = MemoryCache.CacheVariant;
            s += $"RunTasks(int numberOfTasks = {numberOfTasks}) cache = {parallelCache}.\n";
            DateTime now = DateTime.Now;
            if (numberOfTasks <= 0)
                numberOfTasks = 16;
            if ((numberOfTasks % 4) != 0)
                numberOfTasks += (4 - (numberOfTasks % 4));

            int quater = numberOfTasks / 4;
            int half = numberOfTasks / 2;
            int threequater = quater + half;

            Task[] taskArray = new Task[numberOfTasks];
            for (int i = 0; i < numberOfTasks; i++)
            {
                if (i < quater || (i >= half && i < threequater))
                {
                    taskArray[i] = Task.Factory.StartNew((object obj) =>
                    {
                        string ckey = string.Concat("Key_", (i % maxKexs).ToString());
                        CacheData data = null;
                        try
                        {
                            data = obj as CacheData;
                            if (data == null)
                                data = new CacheData(ckey, Thread.CurrentThread.ManagedThreadId);
                        }
                        catch (Exception exCastData)
                        {
                            Area23Log.LogStatic(exCastData);
                        }
                        if (data == null)
                            data = new CacheData() { CKey = ckey, CThreadId = Thread.CurrentThread.ManagedThreadId, CTime = DateTime.Now };


                        data.CThreadId = Thread.CurrentThread.ManagedThreadId;
                        MemoryCache.CacheDict.SetValue<CacheData>(ckey, data);
                        s += $"Task set cache key #{data.CKey} created at {data.CTime} on thread #{data.CThreadId}.\n";
                    },
                    new CacheData("Key_" + (i % maxKexs).ToString()));
                }
                else if ((i >= quater && i < half) || i >= threequater)
                {
                    taskArray[i] = Task.Factory.StartNew((object obj) =>
                    {
                        string ckey = string.Concat("Key_", (i % maxKexs).ToString());
                        string strkey = obj as string;
                        if (string.IsNullOrEmpty(strkey))
                            strkey = ckey;

                        CacheData data = (CacheData)MemoryCache.CacheDict.GetValue<CacheData>(strkey);
                        s += $"Task get cache key #{strkey} => {data.CValue} created at {data.CTime} original thread {data.CThreadId} on current thread #{Thread.CurrentThread.ManagedThreadId}.\n";
                    },
                    new StringBuilder(string.Concat("Key_", (i % maxKexs).ToString())).ToString());
                }
            }

            Task.WaitAll(taskArray);

            TimeSpan ts = DateTime.Now.Subtract(now);
            double doublePerSecond = numberOfTasks / ts.TotalSeconds;
            if (numberOfTasks > ts.TotalSeconds)
                doublePerSecond = (1000 * numberOfTasks) / ts.TotalMilliseconds;
            ulong perSecond = (ulong)doublePerSecond;

            s += $"\nFinished {numberOfTasks} parallel tasks in {ts.Minutes:d2}:{ts.Seconds:d2}.{ts.Milliseconds:d3}\n\t{perSecond} tasks per second.";
            return s;
        }

        static string RunSerial(PersistType persitVariant, int iterationsCount, short maxKexs = 16)
        {
            string s = "";
            MemoryCache memoryCache = null;
            switch (persitVariant)
            {
                case PersistType.JsonFile:
                    JsonFileCache jsonFileCache = new JsonFileCache(persitVariant);
                    memoryCache = (MemoryCache)jsonFileCache;
                    break;
                case PersistType.ApplicationState:
                    ApplicationStateCache appStateCache = new ApplicationStateCache(persitVariant);
                    memoryCache = (MemoryCache)appStateCache;
                    break;
                case PersistType.Redis:
                    RedisCache redisCache = new RedisCache(persitVariant);
                    memoryCache = (MemoryCache)redisCache;
                    break;
                case PersistType.AppDomain:
                default:
                    AppDomainCache appDomainCache = new AppDomainCache(persitVariant);
                    memoryCache = (MemoryCache)appDomainCache;
                    break;
            }
            string serialSache = MemoryCache.CacheVariant;
            s += $"RunSerial(int iterationsCount = {iterationsCount}) cache = {serialSache}.\n";

            if (iterationsCount <= 0)
                iterationsCount = 16;
            if ((iterationsCount % 4) != 0)
                iterationsCount += (4 - (iterationsCount % 4));
            int quater = iterationsCount / 4;
            int half = iterationsCount / 2;
            int threequater = quater + half;

            DateTime now = DateTime.Now;
            for (int i = 0; i < iterationsCount; i++)
            {
                if (i < quater || (i >= half && i < threequater))
                {
                    string ckey = string.Concat("Key_", (i % maxKexs).ToString());
                    CacheData data = new CacheData(ckey, Thread.CurrentThread.ManagedThreadId);
                    MemoryCache.CacheDict.SetValue<CacheData>(ckey, data);
                    s += $"Task set cache key #{data.CKey} created at {data.CTime} on thread #{data.CThreadId}.\n";
                }
                else if ((i >= quater && i < half) || i >= threequater)
                {
                    string strkey = "Key_" + (i % maxKexs).ToString();
                    CacheData cacheData = (CacheData)MemoryCache.CacheDict[strkey];
                    s += $"Task get cache key #{strkey} => {cacheData.CValue} created at {cacheData.CTime} original thread {cacheData.CThreadId} on current thread #{Thread.CurrentThread.ManagedThreadId}.\n";
                }
            }

            // var tasks = new List<Task>(taskArray);            
            // Parallel.ForEach(tasks, task => { task.Start(); });
            //Task.WhenAll(tasks).ContinueWith(done => { Console.WriteLine("done"); });

            TimeSpan ts = DateTime.Now.Subtract(now);
            double doublePerSecond = iterationsCount / ts.TotalSeconds;
            if (iterationsCount > ts.TotalSeconds)
                doublePerSecond = (1000 * iterationsCount) / ts.TotalMilliseconds;
            ulong perSecond = (ulong)doublePerSecond;

            s += $"\nFinished {iterationsCount} iterations in {ts.Minutes:d2}:{ts.Seconds:d2}.{ts.Milliseconds:d3}\n\t{perSecond} iterations per second.";
            return s;
        }


        protected void Button_TestCache_Click(object sender, EventArgs e)
        {
            bool parallel = false, serial = false;
            if (!Enum.TryParse<PersistType>(this.DropDownList_CacheVariant.SelectedValue, out PersistType persistType))
                persistType = PersistType.AppDomain;
            if (!Int32.TryParse(this.DropDownList_Iterations.SelectedValue, out int iterations))
                iterations = 128;
            foreach (ListItem item in CheckBoxList_TestType.Items)
            {
                if (item.Text == "Parallel")
                    parallel = item.Selected;
                if (item.Text == "Serial")
                    serial = item.Selected;
            }
            if (serial || parallel)
                PerformTests(persistType, iterations, parallel, serial);
        }
    }

}