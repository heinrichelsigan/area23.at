using Area23.At.Framework.Core.Static;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using Area23.At.Framework.Core.Cqr;

namespace Area23.At.Framework.Core.Cache
{

    /// <summary>
    /// RedIstatic static AWS elastic valkey cache singelton connector
    /// </summary>
    public class RedisMSCache : MemoryCache
    {

        const string VALKEY_CACHE_HOST_PORT = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        const string VALKEY_CACHE_APP_KEY = "ValkeyCacheHostPort";
        const string ALL_KEYS = "AllKeys";
        protected internal static object _redIsLock = new object();

        public static new string CacheVariant = "RedisMSCache";

        public static MemoryCache ValKey => _instance.Value;

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

        public static RedisCache RedisMS { get; protected internal set; }

        public static RedisCacheOptions RedisMSOptions { get; protected internal set; }


        public RedisMSCache() : this(PersistType.RedisMS) { }

        /// <summary>
        /// default constructor for RedisCacheValKey cache singleton
        /// </summary>
        public RedisMSCache(PersistType cacheType)
        {
            _persistType = (cacheType == PersistType.RedisMS) ? cacheType : PersistType.RedisMS;
            _allKeys = new HashSet<string>();

            string endpoint = EndPoint;

            RedisMSOptions = new RedisCacheOptions() { Configuration = endpoint };
            RedisMS = new RedisCache(RedisMSOptions);

        }


        /// <summary>
        /// GetString gets a string value by redis key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <returns>(<see cref="string"/>) value for key redIsKey</returns>
        public override string GetString(string redIsKey)
        {
            string redIsString = RedisMS.GetString(redIsKey);
            return redIsString;
        }

        /// <summary>
        /// GetKey<typeparamref name="T"/> gets a generic class type T from redis cache with key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ckey">key</param>
        /// <returns>generic value of key</returns>
        public override T GetValue<T>(string ckey)
        {
            string jsonVal = RedisMS.GetString(ckey);
            var tValue = JsonConvert.DeserializeObject<T>(jsonVal);

            return tValue;
        }


        /// <summary>
        /// SetString set key with string value
        /// </summary>
        /// <param name="ckey">key for string/param>
        /// <param name="svalue">value to set</param>
        public override bool SetString(string ckey, string svalue)
        {
            try
            {
                RedisMS.SetString(ckey, svalue);
            }
            catch (Exception ex)
            {
                CqrException.SetLastException(ex);
                return false;
            }

            if (!ContainsKey(ckey))
            {
                _allKeys.Add(ckey);
                string jsonVal = JsonConvert.SerializeObject(AllKeys);
                RedisMS.SetString("AllKeys", jsonVal);
            }

            return true;
        }

        /// <summary>
        /// SetKey<typeparamref name="T"/> sets a genric type T with a referenced key
        /// </summary>
        /// <typeparam name="T">generic type or class</typeparam>
        /// <param name="redIsKey">key for cache</param>
        /// <param name="tValue">Generic value to set</param>        
        public override bool SetValue<T>(string ckey, T tValue)
        {
            string jsonVal = JsonConvert.SerializeObject(tValue);
            bool success = SetString(ckey, jsonVal);
            return success;
        }


        /// <summary>
        /// DeleteKey delete entry referenced at key
        /// </summary>
        /// <param name="redIsKey">key</param>
        public override bool RemoveKey(string redIsKey)
        {
            RedisMS.Remove(redIsKey);

            if (ContainsKey(redIsKey))
            {
                _allKeys.Remove(redIsKey);
                string jsonVal = JsonConvert.SerializeObject(AllKeys);
                RedisMS.SetString("AllKeys", jsonVal);
                return true;
            }
            return false;
        }

        /// <summary>
        /// ContainsKey check if <see cref="Constants.ALL_KEYS">AllKeys</see> key contains element redIsKey
        /// </summary>
        /// <param name="ckey">redIsKey to search</param>
        /// <returns>true, if cache contains key, otherwise false</returns>
        public override bool ContainsKey(string ckey)
        {
            _allKeys = GetAllKeys();
            if (_allKeys.Contains(ckey))
            {
                string redIsString = RedisMS.GetString(ckey);
                if (redIsString != null)
                    return true;
            }

            return false;
        }


        /// <summary>
        /// GetAllKeys returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/>
        /// </summary>
        /// <returns>returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/></returns>
        public override HashSet<string> GetAllKeys()
        {
            if (_allKeys == null || _allKeys.Count == 0)
            {
                string jsonVal = RedisMS.GetString(ALL_KEYS);
                string[] keys = (jsonVal != null) ? JsonConvert.DeserializeObject<string[]>(jsonVal) : new string[0];
                if (keys != null && keys.Length > 0)
                    _allKeys = new HashSet<string>(keys);
            }

            return _allKeys;
        }

    }

}
