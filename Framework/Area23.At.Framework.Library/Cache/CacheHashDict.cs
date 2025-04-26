using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Cache
{

    /// <summary>
    /// CacheHashDict an application cache over AppDomain
    /// </summary>
    public static class CacheHashDict
    {

        /// <summary>
        /// private <see cref="ConcurrentDictionary{string, CacheValue}"/> 
        /// </summary>
        private static ConcurrentDictionary<string, CacheValue> _appDict = new ConcurrentDictionary<string, CacheValue>();


        /// <summary>
        /// public property get accessor for <see cref="_appDict"/> stored in <see cref="AppDomain.CurrentDomain"/>
        /// </summary>
        public static ConcurrentDictionary<string, CacheValue> AppDict
        {
            get
            {
                _appDict = (ConcurrentDictionary<string, CacheValue>)AppDomain.CurrentDomain.GetData(Constants.APP_CONCURRENT_DICT);
                if (_appDict == null)
                {
                    _appDict = new ConcurrentDictionary<string, CacheValue>();
                    AppDomain.CurrentDomain.SetData(Constants.APP_CONCURRENT_DICT, _appDict);
                }

                return _appDict;
            }
            set
            {
                if (value != null && value.Count > 0)
                    _appDict = value;

                if (_appDict != null && _appDict.Count > 0)
                    AppDomain.CurrentDomain.SetData(Constants.APP_CONCURRENT_DICT, _appDict);
            }
        }


        /// <summary>
        /// Gets a value from <see cref="ConcurrentDictionary<string, CacheValue>"/> stored <see cref="System.AppDomain.CurrentDomain"/>
        /// </summary>
        /// <typeparam name="T">generic type of cached value</typeparam>
        /// <param name="ckey">cache key</param>
        /// <returns>generic cached value stored at key</returns>
        public static T GetValue<T>(string ckey)
        {
            if (AppDict.TryGetValue(ckey, out var cvalue))
                return cvalue.GetValue<T>();

            return default(T);
        }


        /// <summary>
        /// Sets a generic value to <see cref="ConcurrentDictionary<string, CacheValue>"/> stored <see cref="System.AppDomain.CurrentDomain"/>
        /// </summary>
        /// <typeparam name="T">generic type of cached value</typeparam>
        /// <param name="ckey">cache key</param>
        /// <param name="cvalue">generic value to stored with key in cache</param>
        public static void SetValue<T>(string ckey, T cvalue)
        {
            CacheValue cacheValue = new CacheValue();
            cacheValue.SetValue<T>(cvalue);

            if (AppDict.ContainsKey(ckey))
                _appDict.TryRemove(ckey, out var cacheOutValue);

            if (_appDict.TryAdd(ckey, cacheValue))
            {
                AppDict = _appDict;
                // AppDomain.CurrentDomain.SetData(Constants.APP_CONCURRENT_DICT, _appDict);
            }

        }

        public static bool ContainsKey(string ckey)
        {
            return AppDict.ContainsKey(ckey);
        }


        public static bool DeleteKeyValue(string ckey)
        {
            bool success = false;
            if ((success = AppDict.ContainsKey(ckey)))
                success = _appDict.TryRemove(ckey, out var cacheOutValue);

            return success;
        }

    }

}
