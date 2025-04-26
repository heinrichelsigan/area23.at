using Area23.At.Framework.Core.Static;
using Newtonsoft.Json;
using StackExchange.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;

namespace Area23.At.Framework.Core.Cache
{

    /// <summary>
    /// Redis AWS elastic valkey cache singelton connector
    /// </summary>
    public class RedIs
    {
        private static readonly Lazy<RedIs> _instance = new Lazy<RedIs>(() => new RedIs());

        ConnectionMultiplexer connMux;
        ConfigurationOptions options;
        string endpoint = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        StackExchange.Redis.IDatabase db;

        public static RedIs ValKey => _instance.Value;

        private static HashSet<string> _allKeys = new HashSet<string>();
        public static string[] AllKeys {  get => _allKeys.ToArray(); }

        public static StackExchange.Redis.IDatabase Db
        {
            get
            {
                if (_instance.Value.db == null)
                    _instance.Value.db = ConnMux.GetDatabase();
                return _instance.Value.db;
            }
        }

        public static StackExchange.Redis.ConnectionMultiplexer ConnMux
        {
            get
            {
                if (_instance.Value.connMux == null)
                {
                    if (_instance.Value.options == null)
                        _instance.Value.options = new ConfigurationOptions
                        {
                            EndPoints = { _instance.Value.endpoint },
                            Ssl = true
                        };
                    _instance.Value.connMux = ConnectionMultiplexer.Connect(_instance.Value.options);
                }
                return _instance.Value.connMux;
            }
        }


        /// <summary>
        /// default parameterless constructor for RedIsValKey cache singleton 
        /// </summary>
        public RedIs()
        {
            endpoint = Constants.VALKEY_CACHE_HOST_PORT; // "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
            if (ConfigurationManager.AppSettings != null && ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY] != null)
                endpoint = (string)ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT_KEY];
            options = new ConfigurationOptions
            {
                EndPoints = { endpoint },
                Ssl = true
            };
            if (connMux == null)
                connMux = ConnectionMultiplexer.Connect(options);
            if (db == null)
                db = connMux.GetDatabase();

            var keys = GetKey<string[]>("AllKeys");
            if (keys != null && keys.Length > 0) 
                _allKeys = new HashSet<string>(keys);
        }



        /// <summary>
        /// GetString gets a string value by redis key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"><see cref="CommandFlags"/></param>
        /// <returns>(<see cref="string"/>) value for key redIsKey</returns>
        public string GetString(string redIsKey, CommandFlags flags = CommandFlags.None)
        {
            string redIsString = Db.StringGet(redIsKey, flags);
            return redIsString;
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
        public void SetString(string redIsKey, string redIsString, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            Db.StringSet(redIsKey, redIsString, expiry, keepTtl, when, flags);
            if (!_allKeys.Contains(redIsKey)) 
            {
                _allKeys.Add(redIsKey);
                SetKey<string[]>("AllKeys", AllKeys);
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
        public void SetKey<T>(string redIsKey, T tValue, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            string jsonVal = JsonConvert.SerializeObject(tValue);
            SetString(redIsKey, jsonVal, expiry, keepTtl, when, flags);
        }

        /// <summary>
        /// GetKey<typeparamref name="T"/> gets a generic class type T from redis cache with key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public T GetKey<T>(string redIsKey, CommandFlags flags = CommandFlags.None)
        {
            string jsonVal = GetString(redIsKey, flags);

            var tValue = JsonConvert.DeserializeObject<T>(jsonVal);

            return tValue;
        }

        /// <summary>
        /// DeleteKey delete entry referenced at key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"><see cref="CommandFlags.FireAndForget"/> as default</param>
        public void DeleteKey(string redIsKey, CommandFlags flags = CommandFlags.FireAndForget)
        {            
            Db.StringGetDelete(redIsKey, flags);
            if (!_allKeys.Contains(redIsKey))
            {
                _allKeys.Remove(redIsKey);
                SetKey<string[]>("AllKeys", AllKeys);
            }
        }


    }

}
