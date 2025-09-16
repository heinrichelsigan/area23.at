using Area23.At.Framework.Core.Cqr;
using Area23.At.Framework.Core.Static;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Text;

namespace Area23.At.Framework.Core.Cache
{

    /// <summary>
    /// JsonFileCache an application cache implemented with <see cref="ConcurrentDictionary{string, CacheValue}"/> serialized with json    
    /// </summary>
    public class JsonFileCache : MemoryCache
    {

        protected internal static readonly object _smartLock = new object();
        const string JSON_APPCACHE_FILE = "AppCache.json";
        readonly static string JsonFullDirPath = LibPaths.SystemDirJsonPath;
        readonly static string JsonFullFilePath = Path.Combine(JsonFullDirPath, JSON_APPCACHE_FILE);

        public static new string CacheVariant = "JsonFileCache";

        protected static JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            MaxDepth = 16,
            NullValueHandling = NullValueHandling.Include,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTime,
            PreserveReferencesHandling = PreserveReferencesHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
        };

        /// <summary>
        /// public property get accessor for <see cref="_appDict"/> stored in <see cref="AppDomain.CurrentDomain"/>
        /// </summary>
        protected override ConcurrentDictionary<string, CacheValue> AppDict
        {
            get => LoadDictionaryCache(true);
            set => SaveDictionaryToCache(value);
        }

        /// <summary>
        /// get, where to get it (_appDict from cache)
        /// </summary>
        /// <param name="repeatLoadingPeriodically">if true, _appDict will be repeatedly loaded from cache <see cref="CACHE_READ_UPDATE_INTERVAL" /> in seconds</param>
        /// <returns><see cref="ConcurrentDictionary{string, CacheValue}"/> _appDict</returns>        
        public override ConcurrentDictionary<string, CacheValue> LoadDictionaryCache(bool repeatLoadingPeriodically = false)
        {
            lock (_smartLock)
            {
                _timePassedSinceLastRW = DateTime.Now.Subtract(_lastCacheRW);

                try
                {
                    if (_appDict == null || _appDict.Count == 0 || (repeatLoadingPeriodically && _timePassedSinceLastRW.TotalSeconds > CACHE_READ_UPDATE_INTERVAL))
                    {
                        var mutex = StaticMutalExclusion.TheMutalExclusion;
                        if (mutex != null && mutex.WaitOne(1024, false))
                        {
                            throw new SynchronizationLockException("Mutex " + mutex.ToString() + " blocks loading serialized json from " + JsonFullFilePath + ".");
                        }

                        lock (_lock)
                        {
                            if (!Directory.Exists(JsonFullDirPath))
                                Directory.CreateDirectory(JsonFullDirPath);

                            string jsonSerializedAppDict = (System.IO.File.Exists(JsonFullFilePath)) ? System.IO.File.ReadAllText(JsonFullFilePath) : "";
                            if (!string.IsNullOrEmpty(jsonSerializedAppDict))
                            {
                                _appDict = (ConcurrentDictionary<string, CacheValue>)JsonConvert.DeserializeObject<ConcurrentDictionary<string, CacheValue>>(jsonSerializedAppDict);
                                _lastCacheRW = DateTime.Now;
                            }
                        }
                    }
                }
                catch (Exception exGetRead)
                {
                    CqrException.SetLastException(exGetRead);
                    // Console.WriteLine($"Exception {exGetRead.GetType()}: {exGetRead.Message} \r\n\t{exGetRead}");
                }
                finally
                {
                    if (_appDict == null || _appDict.Count == 0)
                    {
                        _appDict = new ConcurrentDictionary<string, CacheValue>();
                        _lastCacheRW = DateTime.Now;
                    }
                }
                return _appDict;
            }

        }

        /// <summary>
        /// set where to set <see cref="ConcurrentDictionary{string, CacheValue}">it</see>  
        /// (value to _appDict to cache)
        /// </summary>
        /// <param name="cacheDict"><see cref="ConcurrentDictionary{string, CacheValue}"/></param>
        public override void SaveDictionaryToCache(ConcurrentDictionary<string, CacheValue> cacheDict)
        {
            lock (_outerlock)
            {
                string jsonDeserializedAppDict = "";
                var mutex = StaticMutalExclusion.TheMutalExclusion;
                if (cacheDict == null) //  && value.Count > 0
                    return;

                try
                {
                    if (mutex != null && mutex.WaitOne(1024, false))
                    {
                        throw new SynchronizationLockException("Mutex " + mutex.ToString() + " blocks writing serialized json to " + JsonFullFilePath + ".");
                    }

                    StaticMutalExclusion.CreateMutalExlusion("CacheWrite", false);

                    lock (_lock)
                    {
                        _appDict = cacheDict;

                        // set it, where to set it _appDict
                        jsonDeserializedAppDict = JsonConvert.SerializeObject(_appDict, Formatting.Indented, JsonSettings);
                        System.IO.File.WriteAllText(JsonFullFilePath, jsonDeserializedAppDict, Encoding.UTF8);
                        _lastCacheRW = DateTime.Now;
                    }

                }
                catch (Exception exSetWrite)
                {
                    CqrException.SetLastException(exSetWrite);
                    // Console.WriteLine($"Exception {exSetWrite.GetType()}: {exSetWrite.Message} \r\n\t{exSetWrite}");
                }
                finally
                {
                    StaticMutalExclusion.ReleaseCloseDisposeMutex();
                }
            }
        }


        /// <summary>
        /// Gets a value from <see cref="ConcurrentDictionary{string, CacheValue}"/> stored <see cref="System.AppDomain.CurrentDomain"/>
        /// </summary>
        /// <typeparam name="T">generic type of cached value</typeparam>
        /// <param name="ckey">cache key</param>
        /// <returns>generic cached value stored at key</returns>
        public override T GetValue<T>(string ckey)
        {
            T tvalue = default;

            if (!string.IsNullOrEmpty(ckey))
            {
                lock (_lock)
                {

                    if (_appDict.ContainsKey(ckey) && _appDict.TryGetValue(ckey, out var cvalue))
                    {
                        if (cvalue != null)
                        {
                            tvalue = cvalue.GetValue<T>();
                        }
                    }
                }
            }

            return tvalue;
        }


        public override Nullable<T> GetNullableValue<T>(string ckey)
        {
            T tvalue = default(T);

            if (!string.IsNullOrEmpty(ckey))
            {
                lock (_lock)
                {

                    if (_appDict.ContainsKey(ckey) && _appDict.TryGetValue(ckey, out var cvalue))
                    {
                        if (cvalue != null)
                        {
                            var nilvalue = cvalue.GetNullableValue<T>();
                            if (nilvalue != null && nilvalue.HasValue)
                                tvalue = nilvalue.Value;
                        }
                    }
                }
            }

            return tvalue;
        }


        public JsonFileCache() : this(PersistType.JsonFile) { }

        public JsonFileCache(PersistType cacheType)
        {
            _persistType = (cacheType == PersistType.JsonFile) ? cacheType : PersistType.JsonFile;

            lock (_lock)
            {

                try
                {
                    if (!Directory.Exists(JsonFullDirPath))
                        Directory.CreateDirectory(JsonFullDirPath);
                }
                catch (Exception ioEx)
                {
                    CqrException.SetLastException(ioEx);
                }
                _appDict = LoadDictionaryCache(true);
            }

        }

    }

}
