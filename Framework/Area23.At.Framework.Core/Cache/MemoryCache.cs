using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Cache
{

    /// <summary>
    /// CacheHashDict an application cache implemented with a <see cref="ConcurrentDictionary{string, CacheValue}"/> saved in memory only at runtime
    /// derive from <see cref="MemoryCache"/> and implement your own cache by implementing a new variant for property <see cref="AppDict"/>
    /// </summary>
    public class MemoryCache
    {

        public const string APP_CONCURRENT_DICT = "APP_CONCURRENT_DICT";
        public const int CACHE_READ_UPDATE_INTERVAL = 120;
        protected internal static readonly object _lock = new object(), _outerlock = new object();

        protected internal DateTime _lastCacheRW = DateTime.Now;

        protected internal TimeSpan _timePassedSinceLastRW = TimeSpan.Zero;

        protected internal static HashSet<string> _allKeys = new HashSet<string>();

        public static string CacheVariant = "MemoryCache";

        protected internal static bool _onceCreated = false;

        protected internal PersistType _persistType;
        public virtual string CacheType { get => _persistType.ToString(); }

        protected internal static Lazy<MemoryCache> _instance;
        public static MemoryCache CacheDict
        {
            get
            {
                if (_instance == null && !_onceCreated)
                {
                    PersistType cacheType = PersistInCache.CacheType;
                    CreateInstance(cacheType);
                }

                return _instance.Value;
            }
        }


        /// <summary>
        /// private <see cref="ConcurrentDictionary{string, CacheValue}"/> 
        /// </summary>
        protected ConcurrentDictionary<string, CacheValue> _appDict = new ConcurrentDictionary<string, CacheValue>();

        /// <summary>
        /// public property get accessor for <see cref="_appDict"/> stored in <see cref="AppDomain.CurrentDomain"/>
        /// </summary>
        protected virtual ConcurrentDictionary<string, CacheValue> AppDict
        {
            get => LoadDictionaryCache();           // get, where to get it (_appDict from cache)
            set => SaveDictionaryToCache(value);    // set it where to set it (value to _appDict to cache)
        }


        /// <summary>
        /// Indexer with string 
        /// </summary>
        /// <param name="ckey">key to lookup</param>
        /// <returns>object or null, thou must cast object</returns>
        public string this[string ckey]
        {
            get => GetString(ckey);
            set => SetString(ckey, value);
        }

        /// <summary>
        /// Get all keys from <see cref="AppDict"/> which is implemented as a <see cref="ConcurrentDictionary{string, CacheValue}"/>
        /// </summary>
        public virtual string[] AllKeys { get => GetAllKeys().ToArray(); }


        /// <summary>
        /// ctor
        /// </summary>
        static MemoryCache()
        {
            var persistType = PersistInCache.CacheType;
            CreateInstance(persistType);
        }


        public MemoryCache()
        {
            _persistType = PersistInCache.CacheType;
        }

        protected internal static void CreateInstance(PersistType cacheType)
        {
            lock (_lock)
            {
                switch (cacheType)
                {
                    case PersistType.JsonFile:
                        _instance = new Lazy<MemoryCache>(() => new JsonFileCache());
                        break;
                    case PersistType.RedisValkey:
                        _instance = new Lazy<MemoryCache>(() => new RedisValkeyCache());
                        break;
                    case PersistType.RedisMS:
                        _instance = new Lazy<MemoryCache>(() => new RedisMSCache());
                        break;
                    case PersistType.AppDomain:
                    case PersistType.ApplicationState:
                    default:
                        _instance = new Lazy<MemoryCache>(() => new AppDomainCache());
                        break;
                }

                _onceCreated = true;
                CacheVariant = cacheType.ToString();
            }

        }

        /// <summary>
        /// get, where to get it (_appDict from cache)
        /// </summary>
        /// <param name="repeatLoadingPeriodically">if true, _appDict will be repeatedly loaded from cache <see cref="CACHE_READ_UPDATE_INTERVAL" /> in seconds</param>
        /// <returns><see cref="ConcurrentDictionary{string, CacheValue}"/> _appDict</returns>
        public virtual ConcurrentDictionary<string, CacheValue> LoadDictionaryCache(bool repeatLoadingPeriodically = false)
        {

            if (_appDict == null || _appDict.Count == 0)
            {
                _timePassedSinceLastRW = DateTime.Now.Subtract(_lastCacheRW);
                _lastCacheRW = DateTime.Now;
                _appDict = new ConcurrentDictionary<string, CacheValue>();
                // where to set it _appDict
            }

            return _appDict;
        }

        /// <summary>
        /// set where to set <see cref="ConcurrentDictionary{string, CacheValue}">it</see>  
        /// (value to _appDict to cache)
        /// </summary>
        /// <param name="cacheDict"><see cref="ConcurrentDictionary{string, CacheValue}"/></param>
        public virtual void SaveDictionaryToCache(ConcurrentDictionary<string, CacheValue> cacheDict)
        {
            if (cacheDict != null) //  && value.Count > 0
                _appDict = cacheDict;
        }

        /// <summary>
        /// public ctor
        /// </summary>
        //public MemoryCache(PersistType cacheType = PersistType.AppDomain)
        //{

        //    switch (cacheType)
        //    {
        //        case PersistType.JsonFile:
        //            if (!_instance.IsValueCreated || _instance.Value == null || !_instance.Value.CacheType.Equals("JsonFileCache", StringComparison.CurrentCultureIgnoreCase))
        //                _instance = new Lazy<MemoryCache>(() => new JsonFileCache());
        //            break;
        //        case PersistType.Redis:
        //            // TODO: Redis                    
        //            if (!_instance.IsValueCreated || _instance.Value == null || !_instance.Value.CacheType.Equals("RedisCache", StringComparison.CurrentCultureIgnoreCase))
        //                _instance = new Lazy<MemoryCache>(() => new RedisCache());
        //            break;
        //        case PersistType.ApplicationState:
        //            if (!_instance.IsValueCreated || _instance.Value == null || !_instance.Value.CacheType.Equals("ApplicationStateCache", StringComparison.CurrentCultureIgnoreCase))
        //                _instance = new Lazy<MemoryCache>(() => new ApplicationStateCache());
        //            break;
        //        case PersistType.AppDomain:
        //        default:
        //            if (!_instance.IsValueCreated || _instance.Value == null || !_instance.Value.CacheType.Equals("AppDomainCache", StringComparison.CurrentCultureIgnoreCase))                        
        //                _instance = new Lazy<MemoryCache>(() => new AppDomainCache());
        //            break;
        //    }

        //}


        #region virtual cache operations on _appDict methods

        public virtual string GetString(string ckey)
        {
            string valString = "";

            if (!string.IsNullOrEmpty(ckey))
            {
                _appDict = AppDict;
                lock (_lock)
                {
                    if (_appDict.ContainsKey(ckey) && _appDict.TryGetValue(ckey, out var cvalue))
                    {
                        if (cvalue != null)
                            valString = cvalue.ToString();
                    }
                }
            }

            return valString;
        }

        /// <summary>
        /// Gets a value from <see cref="ConcurrentDictionary{string, CacheValue}"/> stored <see cref="System.AppDomain.CurrentDomain"/>
        /// </summary>
        /// <typeparam name="T">generic type of cached value</typeparam>
        /// <param name="ckey">cache key</param>
        /// <returns>generic cached value stored at key</returns>
        public virtual T GetValue<T>(string ckey)
        {
            lock (_outerlock)
            {
                T tvalue = default(T);

                if (!string.IsNullOrEmpty(ckey))
                {
                    _appDict = AppDict;
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

        }

        public virtual Nullable<T> GetNullableValue<T>(string ckey) where T : struct
        {
            Nullable<T> nullableT = null;

            if (!string.IsNullOrEmpty(ckey))
            {
                lock (_lock)
                {
                    if (_appDict.ContainsKey(ckey) && _appDict.TryGetValue(ckey, out var cvalue))
                    {
                        if (cvalue != null)
                            nullableT = cvalue.GetNullableValue<T>();
                    }
                }
            }

            return nullableT;
        }

        public virtual bool SetString(string ckey, string svalue)
        {
            lock (_outerlock)
            {
                bool addedOrUpdated = false;

                if (string.IsNullOrEmpty(ckey) || svalue == null)
                    return addedOrUpdated;

                CacheValue cvalue = new CacheValue();
                cvalue.SetValue<string>(svalue);

                _appDict = LoadDictionaryCache(true);

                lock (_lock)
                {
                    if (!_appDict.ContainsKey(ckey))
                        addedOrUpdated = _appDict.TryAdd(ckey, cvalue);
                    else if (_appDict.TryGetValue(ckey, out CacheValue oldValue))
                        addedOrUpdated = _appDict.TryUpdate(ckey, cvalue, oldValue);

                    // MAYBE SHORTER BUT NOBODY CAN QUICK READ AND UNDERSTAND THIS
                    // addedOrUpdated = (!AppCache.ContainsKey(ckey)) ? AppCache.TryAdd(ckey, cvalue) :
                    //    (AppCache.TryGetValue(ckey, out CacheValue oldValue)) ? _appCache.TryUpdate(ckey, cvalue, oldValue) : false;

                    if (addedOrUpdated) // saves the modified ConcurrentDictionary{string, CacheValue} back to AppDomain
                        SaveDictionaryToCache(_appDict);
                }

                return addedOrUpdated;
            }
        }

        /// <summary>
        /// Sets a generic value to <see cref="ConcurrentDictionary{string, CacheValue}"/> stored <see cref="System.AppDomain.CurrentDomain"/>
        /// </summary>
        /// <typeparam name="T">generic type of cached value</typeparam>
        /// <param name="ckey">cache key</param>
        /// <param name="cvalue">generic value to stored at key in cache</param>
        /// <returns>true, if add or updated succeeded, otherwise false</returns>
        public virtual bool SetValue<T>(string ckey, T tvalue)
        {
            lock (_outerlock)
            {
                bool addedOrUpdated = false;

                if (string.IsNullOrEmpty(ckey) || tvalue == null)
                    return addedOrUpdated;

                CacheValue cvalue = new CacheValue();
                cvalue.SetValue<T>(tvalue);

                _appDict = LoadDictionaryCache(true);

                lock (_lock)
                {
                    if (!_appDict.ContainsKey(ckey))
                        addedOrUpdated = _appDict.TryAdd(ckey, cvalue);
                    else if (_appDict.TryGetValue(ckey, out CacheValue oldValue))
                        addedOrUpdated = _appDict.TryUpdate(ckey, cvalue, oldValue);

                    // MAYBE SHORTER BUT NOBODY CAN QUICK READ AND UNDERSTAND THIS
                    // addedOrUpdated = (!AppCache.ContainsKey(ckey)) ? AppCache.TryAdd(ckey, cvalue) :
                    //    (AppCache.TryGetValue(ckey, out CacheValue oldValue)) ? _appCache.TryUpdate(ckey, cvalue, oldValue) : false;

                    if (addedOrUpdated) // saves the modified ConcurrentDictionary{string, CacheValue} back to AppDomain
                        SaveDictionaryToCache(_appDict);
                }

                return addedOrUpdated;
            }
        }

        /// <summary>
        /// Looks, if  <see cref="ConcurrentDictionary{string, CacheValue}"/>  contains the key
        /// </summary>
        /// <param name="ckey">lookup key</param>
        /// <returns>true, if ckey is not null or empty and <see cref="AppDict"/> contains ckey, otherwise false</returns>
        public virtual bool ContainsKey(string ckey)
        {
            return (!string.IsNullOrEmpty(ckey) && AppDict.ContainsKey(ckey));
        }

        /// <summary>
        /// RemoveKey removes a key value pair from <see cref="AppDict"/>
        /// </summary>
        /// <param name="ckey">key to remove</param>
        /// <returns>true, if key value pair was successfully removed or <see cref="AppDict"/> doesn't contain anymore ckey;
        /// false if ckey is <see cref="null"/> or <see cref="string.Empty"/> or removing ckey from <see cref="ConcurrentDictionary{string, CacheValue}"/> failed.</returns>
        public virtual bool RemoveKey(string ckey)
        {
            lock (_outerlock)
            {
                bool success = false;
                if (string.IsNullOrEmpty(ckey))
                    return success;

                _appDict = AppDict;

                lock (_lock)
                {
                    if ((success = !_appDict.ContainsKey(ckey)) == false)
                        if ((success = _appDict.TryRemove(ckey, out CacheValue cvalue)) == true)
                            SaveDictionaryToCache(_appDict);  // saves the modified ConcurrentDictionary{string, CacheValue} back to AppDomain
                }

                return success;
            }
        }

        public virtual HashSet<string> GetAllKeys()
        {
            _allKeys = new HashSet<string>(AppDict.Keys.ToArray());
            return _allKeys;
        }


        #endregion virtual cache operations on _appDict methods

    }

}
