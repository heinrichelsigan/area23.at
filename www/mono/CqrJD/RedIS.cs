using Area23.At.Framework.Library.Cache;
using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.CqrJD
{


    /// <summary>
    /// Redis AWS elastic valkey cache singelton connector
    /// </summary>
    public class RedIS : MemoryCache
    {
        private static readonly Lazy<RedIS> _instance = new Lazy<RedIS>(() => new RedIS());

        private static readonly object _lock = new object();

        // ConnectionMultiplexer connMux;
        // ConfigurationOptions options;
        string endpoint = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        // StackExchange.Redis.IDatabase db;

        public static RedIS ValKey => _instance.Value;

        // private static HashSet<string> _allKeys = new HashSet<string>();
        // public static string[] AllKeys { get => GetAllKeys().ToArray(); }

        public static string EndPoint
        {
            get
            {
                _instance.Value.endpoint = Constants.VALKEY_CACHE_HOST_PORT; // "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";                
                if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY] != null)
                    _instance.Value.endpoint = (string)ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY];
                return _instance.Value.endpoint;
            }
        }

        //public static StackExchange.Redis.IDatabase Db
        //{
        //    get
        //    {
        //        if (_instance.Value.db == null)
        //            _instance.Value.db = ConnMux.GetDatabase();

        //        return _instance.Value.db;
        //    }
        //}

        //public static StackExchange.Redis.ConnectionMultiplexer ConnMux
        //{
        //    get
        //    {
        //        if (_instance.Value.connMux == null)
        //        {
        //            if (_instance.Value.options == null)
        //                _instance.Value.options = new ConfigurationOptions
        //                {
        //                    EndPoints = { EndPoint },
        //                    Ssl = true
        //                };
        //            _instance.Value.connMux = ConnectionMultiplexer.Connect(_instance.Value.options);
        //        }
        //        return _instance.Value.connMux;
        //    }
        //}


        /// <summary>
        /// default parameterless constructor for RedIsValKey cache singleton 
        /// </summary>
        public RedIS()
        {
            endpoint = Constants.VALKEY_CACHE_HOST_PORT; // "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
            if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY] != null)
                endpoint = (string)ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY];
            //options = new ConfigurationOptions
            //{
            //    EndPoints = { endpoint },
            //    Ssl = true
            //};
            //if (connMux == null)
            //    connMux = ConnectionMultiplexer.Connect(options);
            //if (db == null)
            //    db = connMux.GetDatabase();

        }



        /// <summary>
        /// GetString gets a string value by redis key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <returns>(<see cref="string"/>) value for key redIsKey</returns>
        public string GetString(string redIsKey) // CommandFlags flags = CommandFlags.None)
        {
            if (!AppDict.TryGetValue(redIsKey, out CacheValue cacheValue))
                return null;
            return cacheValue.GetValue<string>();
        }

        /// <summary>
        /// SetString set key with string value
        /// </summary>
        /// <param name="redIsKey">key for string/param>
        /// <param name="redIsString"></param>
        public void SetString(string redIsKey, string redIsString)
        {
            lock (_lock)
            {
                CacheValue newCache = new CacheValue(redIsString, typeof(string));
                newCache.SetValue<string>(redIsString);
                HashSet<string> allRedIsKeys = GetAllKeys();
                if (AppDict.ContainsKey(redIsKey))
                {
                    AppDict.TryGetValue(redIsKey, out CacheValue oldCache);
                    AppDict.TryUpdate(redIsKey, oldCache, newCache);
                }
                else
                    AppDict.TryAdd(redIsKey, newCache);


                if (!allRedIsKeys.Contains(redIsKey))
                {
                    allRedIsKeys.Add(redIsKey);
                    AppDict.TryGetValue(Constants.ALL_KEYS, out CacheValue oldAllKeys);
                    CacheValue newAllKeys = new CacheValue(allRedIsKeys.ToArray(), typeof(String[]));
                    newAllKeys.SetValue<string[]>(allRedIsKeys.ToArray());
                    AppDict.TryUpdate(Constants.ALL_KEYS, oldAllKeys, newAllKeys);
                    // _allKeys = allRedIsKeys;
                }
            }
        }


        /// <summary>
        /// SetKey<typeparamref name="T"/> sets a genric type T with a referenced key
        /// </summary>
        /// <typeparam name="T">generic type or class</typeparam>
        /// <param name="redIsKey">key for cache</param>
        /// <param name="tValue">Generic value to set</param>
        /// <param name="expiry"></param>
        /// <param name="keepTtl"></param>
        /// <param name="when"></param>
        /// <param name="flags"></param>
        public void SetKey<T>(string redIsKey, T tValue)
        {
            lock (_lock)
            {
                HashSet<string> allRedIsKeys = GetAllKeys();
                CacheValue newCache = new CacheValue(redIsKey, typeof(T));
                newCache.SetValue<T>(tValue);
                if (AppDict.ContainsKey(redIsKey))
                {
                    AppDict.TryGetValue(redIsKey, out CacheValue oldCache);
                    AppDict.TryUpdate(redIsKey, oldCache, newCache);
                }
                else
                    AppDict.TryAdd(redIsKey, newCache);


                if (!allRedIsKeys.Contains(redIsKey))
                {
                    allRedIsKeys.Add(redIsKey);
                    AppDict.TryGetValue(Constants.ALL_KEYS, out CacheValue oldAllKeys);
                    CacheValue newAllKeys = new CacheValue(allRedIsKeys.ToArray(), typeof(String[]));
                    newAllKeys.SetValue<string[]>(allRedIsKeys.ToArray());
                    AppDict.TryUpdate(Constants.ALL_KEYS, oldAllKeys, newAllKeys);
                    // _allKeys = allRedIsKeys;
                }
            }
            
        }

        /// <summary>
        /// GetKey<typeparamref name="T"/> gets a generic class type T from redis cache with key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T GetKey<T>(string redIsKey) // , CommandFlags flags = CommandFlags.None)
        {
            AppDict.TryGetValue(redIsKey, out CacheValue getCacheValue);
            T tvalue = getCacheValue.GetValue<T>();    
            return tvalue;
        }

        /// <summary>
        /// DeleteKey delete entry referenced at key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"><see cref="CommandFlags.FireAndForget"/> as default</param>
        public void DeleteKey(string redIsKey) //, CommandFlags flags = CommandFlags.FireAndForget)
        {

            lock (_lock)
            {
                HashSet<string> allRedIsKeys = GetAllKeys();
                if (AppDict.TryRemove(redIsKey, out CacheValue removeCacheValue) && allRedIsKeys.Contains(redIsKey))
                {
                    allRedIsKeys.Add(redIsKey);
                    AppDict.TryGetValue(Constants.ALL_KEYS, out CacheValue oldAllKeys);
                    CacheValue newAllKeys = new CacheValue(allRedIsKeys.ToArray(), typeof(String[]));
                    newAllKeys.SetValue<string[]>(allRedIsKeys.ToArray());
                    AppDict.TryUpdate(Constants.ALL_KEYS, oldAllKeys, newAllKeys);
                    // _allKeys = allRedIsKeys;
                }
            }
        }



        /// <summary>
        /// ContainsKey check if <see cref="Constants.ALL_KEYS">AllKeys</see> key contains element redIsKey
        /// </summary>
        /// <param name="redIsKey">redIsKey to search</param>
        /// <returns>true, if cache contains key, otherwise false</returns>
        public bool ContainsKey(string redIsKey)
        {
            if (GetAllKeys().Contains(redIsKey))
            {
                return AppDict.ContainsKey(redIsKey);
            }

            return false;
        }



        /// <summary>
        /// GetAllKeys returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/>
        /// </summary>
        /// <returns>returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/></returns>
        protected static internal HashSet<string> GetAllKeys()
        {
            if (MemoryCache.CacheDict.AllKeys == null || MemoryCache.CacheDict.AllKeys.Length == 0)
            {                
                string[] keys = MemoryCache.CacheDict.GetValue<string[]>(Constants.ALL_KEYS);
                if (keys != null && keys.Length > 0)
                    return new HashSet<string>(keys);                    
            }

            return new HashSet<string>(MemoryCache.CacheDict.AllKeys);
        }


        public static string GetStatus()
        {
            var allCacheKeys = GetAllKeys();
            if (allCacheKeys == null || allCacheKeys.Count == 0)
                return "cmpty cache";

            return allCacheKeys.Count + " keys in cache";
        }

    }
    

}