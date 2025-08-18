using Area23.At.Framework.Core.Cqr;
using Area23.At.Framework.Core.Static;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Configuration;

namespace Area23.At.Framework.Core.Cache
{

    /// <summary>
    /// RedisCache AWS elastic valkey cache singelton connector
    /// </summary>
    public class RedisValkeyCache : MemoryCache
    {

        #region const and static

        const string VALKEY_CACHE_HOST_PORT = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        const string VALKEY_CACHE_APP_KEY = "RedisValkeyCache";
        const string REDIS_VALKEY_SSL = "RedisValkeySsl";
        const string ALL_KEYS = "AllKeys";

        protected internal static object _redIsLock = new object();
        public static new string CacheVariant = "RedisValkey";
        private static bool _ssl = true;
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
        public static RedisValkeyCache ValKeyInstance => ((RedisValkeyCache)_instance.Value);

        public string Status { get => ConnMux.GetStatus(); }

        #endregion const and static


        ConfigurationOptions options;

        public override string[] AllKeys { get => GetAllKeys().ToArray(); }

        StackExchange.Redis.IDatabase _db;
        public StackExchange.Redis.IDatabase Db
        {
            get
            {
                if (_db == null)
                    _db = ConnMux.GetDatabase();

                return _db;
            }
        }

        ConnectionMultiplexer _connMux;
        public StackExchange.Redis.ConnectionMultiplexer ConnMux
        {
            get
            {
                if (_connMux == null)
                {
                    if (options == null)
                    {
                        if (!bool.TryParse((string)ConfigurationManager.AppSettings[REDIS_VALKEY_SSL], out _ssl))
                            _ssl = true;
                        options = new ConfigurationOptions
                        {
                            EndPoints = { EndPoint },
                            AbortOnConnectFail = false,
                            Ssl = _ssl
                        };
                    }
                    _connMux = ConnectionMultiplexer.Connect(options);
                }
                return _connMux;
            }
        }


        #region constructors

        public RedisValkeyCache() : this(PersistType.RedisValkey) { }

        /// <summary>
        /// default constructor for RedisCacheValKey cache singleton
        /// </summary>
        public RedisValkeyCache(PersistType cacheType)
        {
            _persistType = (cacheType == PersistType.RedisValkey) ? cacheType : PersistType.RedisValkey;
            _allKeys = new HashSet<string>();
            if (!bool.TryParse((string)ConfigurationManager.AppSettings[REDIS_VALKEY_SSL], out _ssl))
                _ssl = true;

            options = new ConfigurationOptions
            {
                EndPoints = { EndPoint },
                AbortOnConnectFail = false,
                Ssl = _ssl,
                ConnectTimeout = 6000,
                AsyncTimeout = 6000,
                SyncTimeout = 9000
            };

            _connMux = ConnectionMultiplexer.Connect(options);
            _db = _connMux.GetDatabase();
        }

        #endregion constructors

        #region GetString GetValue
        /// <summary>
        /// GetString gets a string value by RedisCache key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <returns>(<see cref="string"/>) value for key redIsKey</returns>
        public override string GetString(string redIsKey)
        {
            return GetStringWithParams(redIsKey, CommandFlags.None);
        }

        public virtual string GetStringWithParams(string ckey, CommandFlags flags = CommandFlags.None)
        {
            return Db.StringGet(ckey, flags);
        }

        /// <summary>
        /// gets a generic class type T from redis cache with key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ckey">rediskey</param>
        /// <returns>T value/returns>
        public override T GetValue<T>(string ckey)
        {
            string jsonVal = GetStringWithParams(ckey, CommandFlags.None); // Db.StringGet(ckey, flags);

            T tval = default(T);
            if (jsonVal != null)
            {
                tval = JsonConvert.DeserializeObject<T>(jsonVal);
            }

            return tval;
        }

        #endregion GetString GetValue

        #region SetString SetValue
        /// <summary>
        /// SetString set key with string value
        /// </summary>
        /// <param name="ckey">key for string/param>
        /// <param name="svalue">value to set</param>
        public override bool SetString(string ckey, string svalue)
        {
            return SetStringWithParams(ckey, svalue);
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
        public virtual bool SetStringWithParams(string redIsKey, string redIsString, TimeSpan? expiry = null, bool keepTtl = false, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            bool success = false;
            lock (_redIsLock)
            {
                _allKeys = GetAllKeys();
                success = Db.StringSet(redIsKey, redIsString, expiry, when, flags);

                if (success && !_allKeys.Contains(redIsKey))
                {
                    _allKeys.Add(redIsKey);
                    string jsonVal = JsonConvert.SerializeObject(_allKeys);
                    success = Db.StringSet(ALL_KEYS, jsonVal, null, keepTtl, When.Always, CommandFlags.None);
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
            TimeSpan? expiry = new TimeSpan(1, 1, 1, 1);
            bool keepTtl = false;
            When when = When.Always;
            CommandFlags flags = CommandFlags.None;
            string jsonVal = JsonConvert.SerializeObject(tvalue);
            bool success = SetStringWithParams(ckey, jsonVal, expiry, keepTtl, when, flags);

            return success;
        }

        #endregion SetString SetValue

        #region RemoveKey ContainsKey

        /// <summary>
        /// DeleteKey delete entry referenced at key
        /// </summary>
        /// <param name="redIsKey">key</param>
        /// <param name="flags"><see cref="CommandFlags.FireAndForget"/> as default</param>
        public override bool RemoveKey(string redIsKey)
        {
            CommandFlags flags = CommandFlags.FireAndForget;
            lock (_redIsLock)
            {
                if (ContainsKey(redIsKey) || _allKeys.Contains(redIsKey))
                {
                    _allKeys.Remove(redIsKey);
                    string jsonVal = JsonConvert.SerializeObject(_allKeys.ToArray());
                    Db.StringSet("AllKeys", jsonVal, null, false, When.Always, flags);
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
            _allKeys = GetAllKeys();
            if (_allKeys.Contains(ckey))
            {
                string redIsString = Db.StringGet(ckey, CommandFlags.None);
                if (redIsString != null)
                    return true;
            }
            return false;
        }

        #endregion RemoveKey ContainsKey

        /// <summary>
        /// GetAllKeys returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/>
        /// </summary>
        /// <returns>returns <see cref="HashSet{string}"/></string> <see cref="_allKeys"/></returns>
        public override HashSet<string> GetAllKeys()
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

}
