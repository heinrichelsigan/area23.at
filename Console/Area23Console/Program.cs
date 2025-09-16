using Area23.At.Framework.Core.Cache;
using Area23.At.Framework.Core.Util;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Area23Console
{
    internal class Program
    {

        static Lock _lock = new Lock();

        static void Main(string[] args)
        {
            PersistType persistType = PersistType.ApplicationState;
            Console.WriteLine("Hello, World!");
            PerformTests(PersistType.Memory, 256, true, false);
            PerformTests(PersistType.ApplicationState, 256, true, false);
            PerformTests(PersistType.JsonFile, 256, true, false);

        }


        protected static void PerformTests(PersistType persistType, int iterations = 128, bool parallel = true, bool serial = true)
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
                    Console.WriteLine($"<pre> Serial  {persistType.ToString()}/{iterations}\n" + serials + " </pre>");
                }
            }
            catch (Exception exSerial)
            {
                Console.WriteLine($"<p>Exception ${exSerial.GetType()} {exSerial.Message}</p><pre>\n{exSerial.ToString()}</pre>");
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
                    Console.WriteLine($"<pre>Parallel {persistType.ToString()}/{iterations}\n" + parallels + "</pre>");
                }
            }
            catch (Exception exParallel)
            {
                Console.WriteLine($"<p>Exception ${exParallel.GetType()} {exParallel.Message}</p>");
                Console.WriteLine($"<pre>\n{exParallel.ToString()}</pre>");
            }

            foreach (string key in cachTests.Keys)
            {
                try
                {
                    Console.WriteLine($"{key} \t => \t{cachTests[key]}");
                }
                catch (Exception exTableRow)
                {
                    try
                    {
                        Console.WriteLine($"<p>Exception ${exTableRow.GetType()} {exTableRow.Message}</p>");
                    }
                    catch (Exception exTableRow0)
                    {
                        Area23Log.LogStatic(exTableRow0);
                    }
                    try
                    {
                        Console.WriteLine($"<pre>\n{exTableRow.ToString()}</pre>");
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
            string s = "", mutexName = "Cache0";
            MemoryCache memoryCache = null;
            switch (persitVariant)
            {
                case PersistType.JsonFile:
                    JsonFileCache jsonFileCache = new JsonFileCache(persitVariant);
                    memoryCache = (MemoryCache)jsonFileCache;
                    break;
                //case PersistType.ApplicationState:
                //    ApplicationStateCache appStateCache = new ApplicationStateCache(persitVariant);
                //    memoryCache = (MemoryCache)appStateCache;
                //    break;
                case PersistType.Memory:
                    MemoryCache memCache = new MemoryCache(persitVariant);
                    memoryCache = (MemoryCache)memCache;
                    break;
                //case PersistType.RedisValkey:
                //    RedisValkeyCache redisValkeyCache = new RedisValkeyCache(persitVariant);
                //    memoryCache = (MemoryCache)redisValkeyCache;
                //    break;
                //case PersistType.SessionState:
                //    SessionStateCache sessionCache = new SessionStateCache(persitVariant);
                //    memoryCache = (MemoryCache)sessionCache;
                //    break;
                case PersistType.AppDomain:
                default:
                    AppDomainCache appDomainCache = new AppDomainCache(persitVariant);
                    memoryCache = (MemoryCache)appDomainCache;
                    break;
            }
            string parallelCache = memoryCache.CacheType;

            s += $"RunTasks(int numberOfTasks = {numberOfTasks}) cache = {parallelCache}.\n";
            DateTime now = DateTime.Now;
            if (numberOfTasks <= 0)
                numberOfTasks = 16;
            if ((numberOfTasks % 4) != 0)
                numberOfTasks += (4 - (numberOfTasks % 4));

            int thid = 0;
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
                        thid = Thread.CurrentThread.ManagedThreadId;
                        mutexName = $"Cache{i:d3}_Thread{thid}";
                        if (StaticMutalExclusion.TheMutalExclusion != null && StaticMutalExclusion.TheMutalExclusion.WaitOne(50))
                        {
                            Thread.Sleep(50);
                        }
                        StaticMutalExclusion.CreateMutalExlusion(mutexName, true);

                        string ckey = string.Concat("Key_", (i % maxKexs).ToString());
                        CacheData data = null;
                        try
                        {
                            data = obj as CacheData;
                            if (data == null)
                                data = new CacheData(ckey, thid);
                        }
                        catch (Exception exCastData)
                        {
                            Area23Log.LogStatic(exCastData);
                        }
                        if (data == null)
                            data = new CacheData() { CKey = ckey, CThreadId = Thread.CurrentThread.ManagedThreadId, CTime = DateTime.Now };


                        data.CThreadId = Thread.CurrentThread.ManagedThreadId;
                        memoryCache.SetValue<CacheData>(ckey, data);
                        Console.WriteLine($"{DateTime.Now.Millisecond} Task set cache key #{data.CKey} created at {data.CTime} on thread #{data.CThreadId}.\n");
                        StaticMutalExclusion.ReleaseCloseDisposeMutex();
                    },
                    new CacheData("Key_" + (i % maxKexs).ToString()));

                }
                else if ((i >= quater && i < half) || i >= threequater)
                {
                    Thread.Sleep(10);
                    taskArray[i] = Task.Factory.StartNew((object obj) =>
                    {
                        thid = Thread.CurrentThread.ManagedThreadId;
                        mutexName = $"Cache{i:d3}_Thread{thid}";
                        if (StaticMutalExclusion.TheMutalExclusion != null && StaticMutalExclusion.TheMutalExclusion.WaitOne(50))
                        {
                            Thread.Sleep(50);
                        }
                        StaticMutalExclusion.CreateMutalExlusion(mutexName, true);

                        string ckey = string.Concat("Key_", (i % maxKexs).ToString());
                        string strkey = obj as string;
                        if (string.IsNullOrEmpty(strkey))
                            strkey = ckey;

                        CacheData data = (CacheData)memoryCache.GetValue<CacheData>(strkey);
                    if (data == null)
                        Console.WriteLine($"{DateTime.Now.Millisecond} Task get cache key #{strkey} => (nil)\n");
                    else
                        Console.WriteLine($"{DateTime.Now.Millisecond} Task get cache key #{strkey} => {data.CValue} created at {data.CTime} original thread {data.CThreadId} on current thread #{Thread.CurrentThread.ManagedThreadId}.\n");

                        StaticMutalExclusion.ReleaseCloseDisposeMutex();
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

            s += $"\n{DateTime.Now} Finished {numberOfTasks} parallel tasks in {ts.Minutes:d2}:{ts.Seconds:d2}.{ts.Milliseconds:d3}\n\t{perSecond} tasks per second.";
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
                //case PersistType.ApplicationState:
                //    ApplicationStateCache appStateCache = new ApplicationStateCache(persitVariant);
                //    memoryCache = (MemoryCache)appStateCache;
                //    break;
                case PersistType.Memory:
                    MemoryCache memCache = new MemoryCache(persitVariant);
                    memoryCache = (MemoryCache)memCache;
                    break;
                //case PersistType.RedisValkey:
                //    RedisValkeyCache redisValkeyCache = new RedisValkeyCache(persitVariant);
                //    memoryCache = (MemoryCache)redisValkeyCache;
                //    break;
                //case PersistType.SessionState:
                //    SessionStateCache sessionCache = new SessionStateCache(persitVariant);
                //    memoryCache = (MemoryCache)sessionCache;
                //    break;
                case PersistType.AppDomain:
                default:
                    AppDomainCache appDomainCache = new AppDomainCache(persitVariant);
                    memoryCache = (MemoryCache)appDomainCache;
                    break;
            }
            string serialSache = memoryCache.CacheType;
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
                    memoryCache.SetValue<CacheData>(ckey, data);
                    s += $"Task set cache key #{data.CKey} created at {data.CTime} on thread #{data.CThreadId}.\n";
                }
                else if ((i >= quater && i < half) || i >= threequater)
                {
                    string strkey = "Key_" + (i % maxKexs).ToString();
                    CacheData cacheData = (CacheData)memoryCache.GetValue<CacheData>(strkey);
                    if (cacheData == null)
                        s += $"Task get cache key #{strkey} => (nil)\n";
                    else
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
 

    }
}
