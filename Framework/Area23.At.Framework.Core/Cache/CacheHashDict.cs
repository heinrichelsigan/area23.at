using Area23.At.Framework.Core.Cqr;
using Area23.At.Framework.Core.Static;
using System.Collections.Concurrent;
using System.Windows.Input;

namespace Area23.At.Framework.Core.Cache
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
        public static T? GetValue<T>(string ckey)
        {
            if (AppDict.ContainsKey(ckey))
            {
                if (!AppDict.TryGetValue(ckey, out var cvalue))                    
                    throw new CqrException($"{typeof(T)}? GetValue<{typeof(T)}>(string ckey = {ckey}) failed.",
                        new InvalidOperationException($"ConcurrentDictionary<string, CacheValue>.TryGetValue(string = {ckey}, out {typeof(T)} cvalue failed."));               
                return cvalue.GetValue<T>();
            }

            return default(T);
        }

        /// <summary>
        /// ContainsKey checks if application cache hash dict contains key
        /// </summary>
        /// <param name="ckey">key to check</param>
        /// <returns>true, if <see cref="AppDict" /> <see cref="ConcurrentDictionary{string, CacheValue}"/> contains key ckey, otherwise false</returns>
        public static bool ContainsKey(string ckey)
        {
            return (AppDict.ContainsKey(ckey));
        }



        /// <summary>
        /// Sets a generic value to <see cref="ConcurrentDictionary<string, CacheValue>"/> stored <see cref="System.AppDomain.CurrentDomain"/>
        /// </summary>
        /// <typeparam name="T">generic type of cached value</typeparam>
        /// <param name="ckey">cache key</param>
        /// <param name="cvalue">generic value to stored with key in cache</param>
        /// <returns>true on succesfull add or succesfull update, false if add or update operation on <see cref="AppDict"/> failed</returns>
        public static bool SetValue<T>(string ckey, T cvalue)
        {
            bool success = false, addNotUpdate = true;
            CacheValue cacheValue = new CacheValue();
            cacheValue.SetValue<T>(cvalue);

            if (AppDict.ContainsKey(ckey))
            {
                if (_appDict.TryGetValue(ckey, out var oldValue))
                {
                    success = _appDict.TryUpdate(ckey, cacheValue, oldValue);
                    addNotUpdate = false;
                }
                else
                    _appDict.TryRemove(ckey, out var cacheOutValue);
            }

            if (addNotUpdate)
                success = _appDict.TryAdd(ckey, cacheValue);
                        
            if (success)
                AppDict = _appDict;                                                  

            return success;                       
        }


        /// <summary>
        /// DeleteKey delete key with corresponding <see cref="CacheValue"/> from <see cref="AppDict"/>
        /// </summary>
        /// <param name="ckey">corresponding key</param>
        /// <returns>true, if <see cref="AppDict"/> contained key and delete key operation was successful, otherwise false</returns>
        public static bool DeleteKey(string ckey)
        {
            bool success = false;

            if (AppDict.ContainsKey(ckey))
                success = _appDict.TryRemove(ckey, out var cacheOutValue);

            if (success)
                AppDict = _appDict;

            return success;
        }

    }

}
