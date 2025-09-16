using Newtonsoft.Json;
// using StackExchange.Redis;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using Area23.At.Framework.Core.Static;

namespace Area23.At.Framework.Core.Cache
{

    /// <summary>
    /// CacheTypVal any cached value.
    /// Use default empty ctor <see cref="CacheTypVal()"/> and
    /// <see cref="SetValue{T}(T)"/> to set the cached value;
    /// setting cache value via <see cref="CacheTypVal(object, Type)"/> ctor is obsolete.
    /// Use <see cref="GetValue{T}"/> to get the cached value
    /// </summary>
    [Serializable]
    public class CacheTypVal
    {

        public object? _Value { get; protected internal set; }
        public Type? _Type { get; protected internal set; }

        /// <summary>
        /// Empty default ctor
        /// </summary>
        public CacheTypVal()
        {
            _Type = null;
            _Value = null;
        }

        /// <summary>
        /// Obsolete ctor, please use default empty ctor <see cref="CacheTypVal()"/>
        /// and then <see cref="SetValue{T}(T)"/> to set a cached value instead.
        /// </summary>
        /// <param name="ovalue"><see cref="object" /> ovalue</param>
        /// <param name="atype"><see cref="Type"/> atype</param>
        [Obsolete("Don't use ctor CacheTypeValue(object, Type) to set a cache value, use SetValue<T>(T tvalue) instead.", false)]
        public CacheTypVal(object ovalue, Type atype)
        {
            _Type = atype;
            _Value = ovalue;
        }

        /// <summary>
        /// gets the <see cref="Type"/> of generic cached value
        /// </summary>
        /// <returns><see cref="Type"/> of generic value or null if cached value is <see cref="null"/></returns>
        public Type? GetType() => _Type;


        /// <summary>
        /// Get a value from cache
        /// </summary>
        /// <typeparam name="T">generic type of value passed by typeparameter</typeparam>
        /// <returns>generic T value</returns>
        /// <exception cref="InvalidOperationException">thrown, when cached value isn't of typeof(T)</exception>
        public T? GetValue<T>()
        {
            T? tvalue;
            if (typeof(T) == _Type || typeof(T).IsSubclassOf(_Type))
                tvalue = (T?)_Value;
            else
                throw new InvalidOperationException($"typeof(T) = {typeof(T)} while _type = {_Type}");

            return tvalue ?? default(T);
        }

        /// <summary>
        /// Sets a generic cached value
        /// </summary>
        /// <typeparam name="T">generic type of value passed by typeparameter</typeparam>
        /// <param name="tvalue">generic value to set cached</param>
        public void SetValue<T>(T tvalue)
        {
            _Type = typeof(T);
            _Value = (object)tvalue;
        }

    }


    /// <summary>
    /// MemCache an application cache implemented saved in memory only at runtime
    /// derive from <see cref="MemCache"/> and implement your own cache by implementing a new variant
    /// </summary>
    public abstract class MemCache
    {

        public const string APP_CONCURRENT_DICT = "APP_CONCURRENT_DICT";
        protected internal readonly Lock _lock = new Lock();

        protected internal static readonly Lazy<MemCache> _instance;
        public static MemCache CacheDict => _instance.Value;

        public static readonly string CacheVariant = "MemCache";

        /// <summary>
        /// private <see cref="ConcurrentDictionary{string, CacheTypeValue}"/>
        /// </summary>
        protected internal static ConcurrentDictionary<string, CacheTypVal> _appCache = new ConcurrentDictionary<string, CacheTypVal>();

        /// <summary>
        /// public property get accessor for <see cref="_appCache"/> stored in <see cref="AppDomain.CurrentDomain"/>
        /// </summary>
        protected virtual ConcurrentDictionary<string, CacheTypVal> AppCache
        {
            get
            {
                // _appCache =  (ConcurrentDictionary<string, CacheTypVal>) get it where to get it
                if (_appCache == null)
                {
                    _appCache = new ConcurrentDictionary<string, CacheTypVal>();
                    // where to set it _appCache
                }
                return _appCache;
            }
            set
            {
                if (value != null && value.Count > 0)
                {
                    _appCache = value;
                    // if (_appCache != null && _appCache.Count > 0)
                    //      set it where to set it _appCache
                }
            }
        }

        public object? this[string ckey]
        {
            get => (AppCache.ContainsKey(ckey) && AppCache.TryGetValue(ckey, out CacheTypVal? cvalue)) ? cvalue._Value : null;
            set
            {
                object? ovalue = value;
                Type? otype = value?.GetType();
                if (AppCache.ContainsKey(ckey) && AppCache.TryGetValue(ckey, out CacheTypVal oldValue))
                    _appCache.TryRemove(ckey, out oldValue);

                _appCache.TryAdd(ckey, new CacheTypVal(ovalue, otype));
                AppCache = _appCache;
            }
        }

        /// <summary>
        /// Get all keys from <see cref="AppCache"/> which is implemented as a <see cref="ConcurrentDictionary{string, CacheTypVal}"/>
        /// </summary>
        public virtual string[] AllKeys { get => AppCache.Keys.ToArray(); }

        /// <summary>
        /// static ctor
        /// </summary>
        static MemCache()
        {
            string persistMsgIn = "JsonFile";

            if (ConfigurationManager.AppSettings[Constants.PERSIST_MSG_IN] != null)
            {
                persistMsgIn = (string)ConfigurationManager.AppSettings[Constants.PERSIST_MSG_IN];
            }

            switch (persistMsgIn)
            {
                case "JsonFile":
                    CacheVariant = "JsonFile";
                    _instance = new Lazy<MemCache>(() => new JsonCache());
                    break;
                case "Redis":
                    // TODO: Redis
                    CacheVariant = "Redis";
                    _instance = new Lazy<MemCache>(() => new RedIsCache());
                    break;
                case "ApplicationState":
                case "AppDomain":
                default:
                    CacheVariant = "AppDomain";
                    _instance = new Lazy<MemCache>(() => new AppCurrentDomainCache());
                    break;
            }
        }


        /// <summary>
        /// Static constructor
        /// </summary>
        public MemCache()
        {
            _appCache = new ConcurrentDictionary<string, CacheTypVal>();
        }

        /// <summary>
        /// Gets a value from <see cref="ConcurrentDictionary<string, CacheTypVal>"/> stored <see cref="System.AppDomain.CurrentDomain"/>
        /// </summary>
        /// <typeparam name="T">generic type of cached value</typeparam>
        /// <param name="ckey">cache key</param>
        /// <returns>generic cached value stored at key</returns>
        public virtual T GetValue<T>(string ckey)
        {
            T tvalue = (AppCache.ContainsKey(ckey) && AppCache.TryGetValue(ckey, out var cvalue)) ? cvalue.GetValue<T>() : default(T);

            return tvalue;
        }

        /// <summary>
        /// Sets a generic value to <see cref="ConcurrentDictionary<string, CacheTypVal>"/> stored <see cref="System.AppDomain.CurrentDomain"/>
        /// </summary>
        /// <typeparam name="T">generic type of cached value</typeparam>
        /// <param name="ckey">cache key</param>
        /// <param name="cvalue">generic value to stored at key in cache</param>
        /// <returns>true, if add or updated succeeded, otherwise false</returns>
        public virtual bool SetValue<T>(string ckey, T tvalue)
        {
            bool addedOrUpdated = false;

            if (string.IsNullOrEmpty(ckey) || tvalue == null)
                return addedOrUpdated;

            CacheTypVal cvalue = new CacheTypVal();
            cvalue.SetValue<T>(tvalue);

            if (!AppCache.ContainsKey(ckey))
                addedOrUpdated = AppCache.TryAdd(ckey, cvalue);
            else if (AppCache.TryGetValue(ckey, out CacheTypVal oldValue))
                addedOrUpdated = _appCache.TryUpdate(ckey, cvalue, oldValue);

            // MAYBE SHORTER BUT NOBODY CAN QUICK READ AND UNDERSTAND THIS
            // addedOrUpdated = (!AppCache.ContainsKey(ckey)) ? AppCache.TryAdd(ckey, cvalue) :
            //    (AppCache.TryGetValue(ckey, out CacheTypVal oldValue)) ? _appCache.TryUpdate(ckey, cvalue, oldValue) : false;

            if (addedOrUpdated)
                AppCache = _appCache;  // saves the modified ConcurrentDictionary{string, CacheTypVal} back to AppDomain

            return addedOrUpdated;
        }

        /// <summary>
        /// Looks, if  <see cref="ConcurrentDictionary{string, CacheTypVal}"/>  contains the key
        /// </summary>
        /// <param name="ckey">lookup key</param>
        /// <returns>true, if ckey is not null or empty and <see cref="AppCache"/> contains ckey, otherwise false</returns>
        public virtual bool ContainsKey(string ckey)
        {
            return (!string.IsNullOrEmpty(ckey) && AppCache.ContainsKey(ckey));
        }

        /// <summary>
        /// RemoveKey removes a key value pair from <see cref="AppCache"/>
        /// </summary>
        /// <param name="ckey">key to remove</param>
        /// <returns>true, if key value pair was successfully removed or <see cref="AppCache"/> doesn't contain anymore ckey;
        /// false if ckey is <see cref="null"/> or <see cref="string.Empty"/> or removing ckey from <see cref="ConcurrentDictionary{string, CacheTypVal}"/> failed.</returns>
        public virtual bool RemoveKey(string ckey)
        {
            bool success = false;
            if (string.IsNullOrEmpty(ckey))
                return success;

            if ((success = !AppCache.ContainsKey(ckey)) == false)
                if ((success = AppCache.TryRemove(ckey, out CacheTypVal cvalue)) == true)
                    AppCache = _appCache; // saves the modified ConcurrentDictionary{string, CacheTypVal} back to AppDomain

            return success;
        }

    }



    /// <summary>
    /// JsonCache an application cache implemented with <see cref="ConcurrentDictionary{string, CacheTypVal}"/> serialized with json    
    /// </summary>
    public class JsonCache : MemCache
    {

        //protected internal static readonly Lazy<MemCache> _instance = new Lazy<MemCache>(() => new JsonCache());
        //public static MemCache CacheDict => _instance.Value;

        const int INIT_SEM_COUNT = 1;
        const int MAX_SEM_COUNT = 1;
        const string JSON_APPCACHE_FILE = "AppCache.json";
        readonly static string JsonFullDirPath = Path.Combine(Environment.GetEnvironmentVariable("LOCALAPPDATA"), "TEMP");
        readonly static string JsonFullFilePath = Path.Combine(JsonFullDirPath, JSON_APPCACHE_FILE);

        protected static SemaphoreSlim ReadWriteSemaphore = new SemaphoreSlim(INIT_SEM_COUNT, MAX_SEM_COUNT);

        protected static JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            MaxDepth = 16,
            // NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTime,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        /// <summary>
        /// public property get accessor for <see cref="_appCache"/> stored in <see cref="AppDomain.CurrentDomain"/>
        /// </summary>
        protected override ConcurrentDictionary<string, CacheTypVal> AppCache
        {
            get
            {
                int semCnt = 0;
                try
                {
                    ReadWriteSemaphore.Wait(64);
                    // if (mutex.WaitOne(250, false)) 
                    if (_appCache == null || _appCache.Count == 0)
                    {
                        lock (_lock)
                        {
                            if (!Directory.Exists(JsonFullDirPath))
                                Directory.CreateDirectory(JsonFullDirPath);

                            string jsonSerializedAppDict = (System.IO.File.Exists(JsonFullFilePath)) ? System.IO.File.ReadAllText(JsonFullFilePath) : "";
                            if (!string.IsNullOrEmpty(jsonSerializedAppDict))
                                _appCache = (ConcurrentDictionary<string, CacheTypVal>)JsonConvert.DeserializeObject<ConcurrentDictionary<string, CacheTypVal>>(jsonSerializedAppDict);
                        }
                        if (_appCache == null || _appCache.Count == 0)
                            _appCache = new ConcurrentDictionary<string, CacheTypVal>();
                    }
                }
                catch (Exception exGetRead)
                {
                    Console.WriteLine($"Exception {exGetRead.GetType()}: {exGetRead.Message} \r\n\t{exGetRead}");
                }
                finally
                {
                    if (ReadWriteSemaphore.CurrentCount > 0)
                        semCnt = ReadWriteSemaphore.Release();
                    // mutex.ReleaseMutex();
                }
                return _appCache;
            }
            set
            {
                int semCnt = 0;
                try
                {
                    semCnt = ReadWriteSemaphore.CurrentCount;
                    ReadWriteSemaphore.Wait(64);

                    string jsonDeserializedAppDict = "";
                    if (value != null && value.Count > 0)
                    {
                        // if (mutex.WaitOne(250, false)) 
                        lock (_lock)
                        {
                            _appCache = value;

                            // set it, where to set it _appCache
                            jsonDeserializedAppDict = JsonConvert.SerializeObject(_appCache, Formatting.Indented, JsonSettings);
                            System.IO.File.WriteAllText(JsonFullFilePath, jsonDeserializedAppDict, Encoding.UTF8);
                        }
                    }
                }
                catch (Exception exSetWrite)
                {
                    Console.WriteLine($"Exception {exSetWrite.GetType()}: {exSetWrite.Message} \r\n\t{exSetWrite}");
                }
                finally
                {
                    if (ReadWriteSemaphore.CurrentCount > 0)
                        semCnt = ReadWriteSemaphore.Release();
                }
            }
        }
    }


    /// <summary>
    /// AppCurrentDomainCache an application cache implemented with a <see cref="ConcurrentDictionary{string, CacheTypVal}"/>
    /// </summary>
    public class AppCurrentDomainCache : MemCache
    {

        /// <summary>
        /// public property get accessor for <see cref="_appCache"/> stored in <see cref="AppDomain.CurrentDomain"/>
        /// </summary>
        protected override ConcurrentDictionary<string, CacheTypVal> AppCache
        {
            get
            {
                _appCache = (ConcurrentDictionary<string, CacheTypVal>)AppDomain.CurrentDomain.GetData(APP_CONCURRENT_DICT);
                if (_appCache == null)
                {
                    lock (_lock)
                    {
                        _appCache = new ConcurrentDictionary<string, CacheTypVal>();
                        AppDomain.CurrentDomain.SetData(APP_CONCURRENT_DICT, _appCache);
                    }
                }

                return _appCache;
            }
            set
            {
                if (value != null && value.Count > 0)
                {
                    lock (_lock)
                    {
                        _appCache = value;
                        AppDomain.CurrentDomain.SetData(APP_CONCURRENT_DICT, _appCache);
                    }
                }
            }
        }

        public AppCurrentDomainCache()
        {
            if (AppCache == null) ;
        }
    }


    /// <summary>
    /// RedisCache AWS elastic valkey cache singelton connector
    /// </summary>
    public class RedIsCache : MemCache
    {

        const string VALKEY_CACHE_HOST_PORT = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        const string VALKEY_CACHE_APP_KEY = "RedisValkeyCache";
        const string ALL_KEYS = "AllKeys";


        ConnectionMultiplexer connMux;
        ConfigurationOptions options;
        string endpoint = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        StackExchange.Redis.IDatabase db;

        public static MemCache ValKey => _instance.Value;

        private static HashSet<string> _allKeys = new HashSet<string>();
        public override string[] AllKeys { get => GetAllKeys().ToArray(); }

        private static string _endPoint = VALKEY_CACHE_HOST_PORT;
        public static string EndPoint
        {
            get
            {
                if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings[VALKEY_CACHE_APP_KEY] != null)
                    _endPoint = (string)ConfigurationManager.AppSettings[VALKEY_CACHE_APP_KEY];
                if (string.IsNullOrEmpty(_endPoint))
                    _endPoint = VALKEY_CACHE_HOST_PORT; // back to default
                return _endPoint;
            }
        }

        public static StackExchange.Redis.IDatabase Db
        {
            get
            {
                if (((RedIsCache)(_instance.Value)).db == null)
                    ((RedIsCache)(_instance.Value)).db = ConnMux.GetDatabase();

                return ((RedIsCache)(_instance.Value)).db;
            }
        }

        public static StackExchange.Redis.ConnectionMultiplexer ConnMux
        {
            get
            {
                if (((RedIsCache)(_instance.Value)).connMux == null)
                {
                    if (((RedIsCache)(_instance.Value)).options == null)
                        ((RedIsCache)(_instance.Value)).options = new ConfigurationOptions
                        {
                            EndPoints = { EndPoint },
                            Ssl = true
                        };
                    ((RedIsCache)(_instance.Value)).connMux = ConnectionMultiplexer.Connect(((RedIsCache)(_instance.Value)).options);
                }
                return ((RedIsCache)(_instance.Value)).connMux;
            }
        }


        /// <summary>
        /// default parameterless constructor for RedisCacheValKey cache singleton
        /// </summary>
        public RedIsCache()
        {
            endpoint = VALKEY_CACHE_HOST_PORT; // "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
            if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings[VALKEY_CACHE_APP_KEY] != null)
                endpoint = (string)ConfigurationManager.AppSettings[VALKEY_CACHE_APP_KEY];
            options = new ConfigurationOptions
            {
                EndPoints = { endpoint },
                Ssl = true
            };
            if (connMux == null)
                connMux = ConnectionMultiplexer.Connect(options);
            if (db == null)
                db = connMux.GetDatabase();
        }


        /// <summary>
        /// GetString gets a string value by RedisCache key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"><see cref="CommandFlags"/></param>
        /// <returns>(<see cref="string"/>) value for key redIsKey</returns>
        public string GetString(string redIsKey, CommandFlags flags = CommandFlags.None)
        {
            return Db.StringGet(redIsKey, flags);
        }

        /// <summary>
        /// SetString set key with string value
        /// </summary>
        /// <param name="redIsKey">key for string/param>
        /// <param name="redIsString"></param>
        /// <param name="expiry"></param>
        /// <param name="keepTtl"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        public bool SetString(string redIsKey, string redIsString, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            bool success = false;
            lock (_lock)
            {
                var allRedIsKeys = GetAllKeys();
                success = Db.StringSet(redIsKey, redIsString, expiry, when, flags);

                if (success && !allRedIsKeys.Contains(redIsKey))
                {
                    allRedIsKeys.Add(redIsKey);
                    string jsonVal = JsonConvert.SerializeObject(AllKeys);
                    success = Db.StringSet(ALL_KEYS, jsonVal, null, keepTtl, When.Always, CommandFlags.None);
                    _allKeys = allRedIsKeys;
                }
            }

            return success;
        }

        /// <summary>
        /// SetValue sets value to cache
        /// </summary>
        /// <typeparam name="T">typeparameter</typeparam>
        /// <param name="ckey">key to set</param>
        /// <param name="tvalue">generic value</param>
        /// <returns>success on true</returns>
        public override bool SetValue<T>(string ckey, T tvalue)
        {
            TimeSpan? expiry = null;
            bool keepTtl = false;
            When when = When.Always;
            CommandFlags flags = CommandFlags.None;
            string jsonVal = JsonConvert.SerializeObject(tvalue);
            bool success = SetString(ckey, jsonVal, expiry, keepTtl, when, flags);

            return success;
        }

        /// <summary>
        /// gets a generic class type T from redis cache with key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ckey">rediskey</param>
        /// <returns>T value/returns>
        public override T GetValue<T>(string ckey)
        {
            CommandFlags flags = CommandFlags.None;
            string jsonVal = Db.StringGet(ckey, flags);
            T tval = default(T);
            if (jsonVal != null)
            {
                tval = JsonConvert.DeserializeObject<T>(jsonVal);
            }

            return tval;
        }

        /// <summary>
        /// DeleteKey delete entry referenced at key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"><see cref="CommandFlags.FireAndForget"/> as default</param>
        public override bool RemoveKey(string redIsKey)
        {
            CommandFlags flags = CommandFlags.FireAndForget;
            lock (_lock)
            {
                var allRedIsKeys = GetAllKeys();
                if (allRedIsKeys.Contains(redIsKey))
                {
                    allRedIsKeys.Remove(redIsKey);
                    string jsonVal = JsonConvert.SerializeObject(allRedIsKeys.ToArray());
                    Db.StringSet("AllKeys", jsonVal, null, false, When.Always, flags);
                    _allKeys = allRedIsKeys;
                }
                try
                {
                    TimeSpan span = new TimeSpan(0, 0, 1);
                    Db.StringGetDelete(redIsKey, flags);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exception {ex.GetType()}: {ex.Message}\r\n\t{ex}");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// ContainsKey check if <see cref="Constants.ALL_KEYS">AllKeys</see> key contains element redIsKey
        /// </summary>
        /// <param name="ckey">redIsKey to search</param>
        /// <returns>true, if cache contains key, otherwise false</returns>
        public override bool ContainsKey(string ckey)
        {
            if (GetAllKeys().Contains(ckey))
            {
                string redIsString = Db.StringGet(ckey, CommandFlags.None);
                if (!string.IsNullOrEmpty(redIsString))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// GetAllKeys returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/>
        /// </summary>
        /// <returns>returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/></returns>
        public static HashSet<string> GetAllKeys()
        {
            if (_allKeys == null || _allKeys.Count == 0)
            {
                string jsonVal = Db.StringGet(ALL_KEYS, CommandFlags.None);
                string[] keys = (jsonVal != null) ? JsonConvert.DeserializeObject<string[]>(jsonVal) : new string[0];
                if (keys != null && keys.Length > 0)
                    _allKeys = new HashSet<string>(keys);
            }

            return _allKeys;
        }
    }


    [Serializable]
    public class CacheData
    {
        static readonly byte[] buffer = new byte[4096];
        static Random random = new Random((DateTime.Now.Millisecond + 1) * (DateTime.Now.Second + 1));
        [JsonIgnore]
        protected internal int CIndex { get; set; }
        public string CKey { get; set; }
        public string CValue { get; set; }
        public int CThreadId { get; set; }
        public DateTime CTime { get; set; }

        static CacheData()
        {
            random.NextBytes(buffer);
        }

        public CacheData()
        {
            CIndex = 0;
            CValue = string.Empty;
            CKey = string.Empty;
            CTime = DateTime.MinValue;
            CThreadId = -1;
        }

        public CacheData(string ckey) : this()
        {
            CKey = ckey;
            CIndex = Int32.Parse(ckey.Replace("Key_", ""));
            CValue = GetRandomString(CIndex);
            CTime = DateTime.Now;
        }

        public CacheData(string ckey, int cThreadId) : this(ckey)
        {
            CThreadId = cThreadId;
        }

        public CacheData(int cThreadId) : this(string.Concat("Key_", cThreadId))
        {
            CThreadId = cThreadId;
        }


        internal string GetRandomString(int ix)
        {
            byte[] restBytes = new byte[64];
            Array.Copy(buffer, ix, restBytes, 0, 64);
            return Convert.ToBase64String(restBytes, 0, 64);
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            RunTasks(256);
            RunSerial(256);

            Console.WriteLine($"\nPress any key to continue...\n");
            Console.ReadKey();
        }

        static void RunTasks(int numberOfTasks, short maxKexs = 16)
        {
            string parallelCache = MemCache.CacheVariant;
            Console.WriteLine($"RunTasks(int numberOfTasks = {numberOfTasks}) cache = {parallelCache}.");
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
                        CacheData data = obj as CacheData;
                        if (data == null)
                            data = new CacheData(ckey, Thread.CurrentThread.ManagedThreadId);

                        data.CThreadId = Thread.CurrentThread.ManagedThreadId;
                        MemCache.CacheDict.SetValue<CacheData>(ckey, data);
                        // Console.WriteLine($"Task set cache key #{data.CKey} created at {data.CTime} on thread #{data.CThreadId}.");
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

                        CacheData data = (CacheData)MemCache.CacheDict[strkey];
                        // Console.WriteLine($"Task get cache key #{strkey} => {data.CValue} created at {data.CTime} original thread {data.CThreadId} on current thread #{Thread.CurrentThread.ManagedThreadId}.");
                    },
                    new StringBuilder(string.Concat("Key_", (i % maxKexs).ToString())).ToString());
                }
            }

            Task.WaitAll(taskArray);

            TimeSpan ts = DateTime.Now.Subtract(now);
            double doublePerSecond = numberOfTasks / ts.TotalSeconds;
            if (numberOfTasks > ts.TotalSeconds)
                doublePerSecond = (1000000 * numberOfTasks) / ts.TotalMicroseconds;
            ulong perSecond = (ulong)doublePerSecond;
            Console.WriteLine($"Finished {numberOfTasks} parallel tasks in {ts.Minutes:d2}:{ts.Seconds:d2}.{ts.Milliseconds:d3}.{ts.Microseconds:d3}\n\t{perSecond} tasks per second.\n");
        }

        static void RunSerial(int iterationsCount, short maxKexs = 16)
        {
            string serialSache = MemCache.CacheVariant;
            Console.WriteLine($"RunSerial(int iterationsCount = {iterationsCount}) cache = {serialSache}.");

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
                    MemCache.CacheDict.SetValue<CacheData>(ckey, data);
                    // Console.WriteLine($"Task set cache key #{data.CKey} created at {data.CTime} on thread #{data.CThreadId}.");
                }
                else if ((i >= quater && i < half) || i >= threequater)
                {
                    string strkey = "Key_" + (i % maxKexs).ToString();
                    CacheData cacheData = (CacheData)MemCache.CacheDict[strkey];
                    // Console.WriteLine($"Task get cache key #{strkey} => {cacheData.CValue} created at {cacheData.CTime} original thread {cacheData.CThreadId} on current thread #{Thread.CurrentThread.ManagedThreadId}.");
                }
            }

            // var tasks = new List<Task>(taskArray);            
            // Parallel.ForEach(tasks, task => { task.Start(); });
            //Task.WhenAll(tasks).ContinueWith(done => { Console.WriteLine("done"); });

            TimeSpan ts = DateTime.Now.Subtract(now);
            double doublePerSecond = iterationsCount / ts.TotalSeconds;
            if (iterationsCount > ts.TotalSeconds)
                doublePerSecond = (1000000 * iterationsCount) / ts.TotalMicroseconds;
            ulong perSecond = (ulong)doublePerSecond;
            Console.WriteLine($"Finished {iterationsCount} iterations in {ts.Minutes:d2}:{ts.Seconds:d2}.{ts.Milliseconds:d3}.{ts.Microseconds:d3}\n\t{perSecond} iterations per second.\n");

        }

    }

}
