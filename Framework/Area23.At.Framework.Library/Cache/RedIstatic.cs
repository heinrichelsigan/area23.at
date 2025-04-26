using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Area23.At.Framework.Library.Cache
{


    /// <summary>
    /// RedIstatic static AWS elastic valkey cache singelton connector
    /// </summary>
    public static class RedIstatic
    {


        static object _lock = new object();
        static string endpoint = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        


        private static HashSet<string> _allKeys = new HashSet<string>();
        public static string[] AllKeys { get => GetAllKeys(false).ToArray(); }

        


        /// <summary>
        /// static ctor for static class RedIstatic 
        /// </summary>
        static RedIstatic()
        {
            endpoint = Constants.VALKEY_CACHE_HOST_PORT; // "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
            if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY] != null)
                endpoint = (string)ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY];
            _allKeys = GetAllKeys();
        }



        /// <summary>
        /// GetString gets a string value by redis key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <returns>(<see cref="string"/>) value for key redIsKey</returns>
        public static string GetString(string redIsKey)
        {
            string redIsString = CacheHashDict.GetValue<string>(redIsKey);
            return redIsString;
        }

        /// <summary>
        /// SetString set key with string value
        /// </summary>
        /// <param name="redIsKey">key for string/param>
        /// <param name="redIsString"></param>
        public static void SetString(string redIsKey, string redIsString)
        {
            lock (_lock)
            {
                HashSet<string> allRedIsKeys = GetAllKeys(true);
                CacheHashDict.SetValue<string>(redIsKey, redIsString);

                if (!allRedIsKeys.Contains(redIsKey))
                {
                    allRedIsKeys.Add(redIsKey);
                    CacheHashDict.SetValue<string[]>(Constants.ALL_KEYS, allRedIsKeys.ToArray());
                    _allKeys = allRedIsKeys;
                }
            }
        }


        /// <summary>
        /// SetKey<typeparamref name="T"/> sets a genric type T with a referenced key
        /// </summary>
        /// <typeparam name="T">generic type or class</typeparam>
        /// <param name="redIsKey">key for cache</param>
        /// <param name="tValue">Generic value to set</param>
        public static void SetKey<T>(string redIsKey, T tValue)
        {
            lock (_lock)
            {
                HashSet<string> allRedIsKeys = GetAllKeys(true);
                CacheHashDict.SetValue<T>(redIsKey, tValue);

                if (!allRedIsKeys.Contains(redIsKey))
                {
                    allRedIsKeys.Add(redIsKey);
                    CacheHashDict.SetValue<string[]>(Constants.ALL_KEYS, allRedIsKeys.ToArray());
                    _allKeys = allRedIsKeys;
                }
            }
        }

        /// <summary>
        /// GetKey<typeparamref name="T"/> gets a generic class type T from redis cache with key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redIsKey">key</param>
        /// <returns></returns>
        public static T GetKey<T>(string redIsKey)
        {
            T tval = CacheHashDict.GetValue<T>(redIsKey);
            return tval;
        }

        /// <summary>
        /// DeleteKey delete entry referenced at key
        /// </summary>
        /// <param name="redIsKey">key</param>
        public static void DeleteKey(string redIsKey)
        {
            lock (_lock)
            {
                HashSet<string> allRedIsKeys = GetAllKeys();
                if (allRedIsKeys.Contains(redIsKey))
                {
                    allRedIsKeys.Remove(redIsKey);
                    CacheHashDict.SetValue<string[]>(Constants.ALL_KEYS, allRedIsKeys.ToArray());
                    _allKeys = allRedIsKeys;
                }
                try
                {
                    CacheHashDict.DeleteKeyValue(redIsKey);
                }
                catch (Exception ex)
                {
                    CqrException.SetLastException(ex);
                }
            }
        }



        /// <summary>
        /// ContainsKey check if <see cref="Constants.ALL_KEYS">AllKeys</see> key contains element redIsKey
        /// </summary>
        /// <param name="redIsKey">redIsKey to search</param>
        /// <returns>true, if cache contains key, otherwise false</returns>
        public static bool ContainsKey(string redIsKey)
        {
            if (GetAllKeys().Contains(redIsKey))
            {
                return CacheHashDict.ContainsKey(redIsKey);
            }

            return false;
        }



        /// <summary>
        /// GetAllKeys returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/>
        /// </summary>
        /// <returns>returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/></returns>
        public static HashSet<string> GetAllKeys(bool getThrough = false)
        {
            if (_allKeys == null || _allKeys.Count == 0 || getThrough)
            {
                string[] keys = CacheHashDict.GetValue<string[]>(Constants.ALL_KEYS);
                if (keys != null && keys.Length > 0)
                    _allKeys = new HashSet<string>(keys);
            }

            return _allKeys;
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