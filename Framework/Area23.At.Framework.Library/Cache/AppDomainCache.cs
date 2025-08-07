using Area23.At.Framework.Library.Cqr;
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
    /// AppDomainCache an application cache implemented with a <see cref="ConcurrentDictionary{string, CacheValue}"/>
    /// </summary>
    public class AppDomainCache : MemoryCache
    {
        protected internal static readonly object _smartLock = new object();

        public static new string CacheVariant = "AppDomainCache";
        public override string CacheType => "AppDomainCache";

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

                if (_appDict == null || _appDict.Count == 0 || (repeatLoadingPeriodically && _timePassedSinceLastRW.TotalSeconds > CACHE_READ_UPDATE_INTERVAL))
                {
                    lock (_lock)
                    {
                        try
                        {
                            _appDict = (ConcurrentDictionary<string, CacheValue>)AppDomain.CurrentDomain.GetData(APP_CONCURRENT_DICT);
                            _lastCacheRW = DateTime.Now;
                        }
                        catch (Exception appDomDictEx)
                        {
                            CqrException.SetLastException(appDomDictEx);
                        }
                    }
                }

                if (_appDict == null || _appDict.Count == 0)
                {
                    lock (_lock)
                    {
                        _appDict = new ConcurrentDictionary<string, CacheValue>();
                        AppDomain.CurrentDomain.SetData(APP_CONCURRENT_DICT, _appDict);
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

            lock (_smartLock)
            {
                if (cacheDict != null) //  && value.Count > 0
                {
                    lock (_lock)
                    {
                        _appDict = cacheDict;
                        AppDomain.CurrentDomain.SetData(APP_CONCURRENT_DICT, _appDict);
                        _lastCacheRW = DateTime.Now;
                    }
                }
            }
        }


        public AppDomainCache(PersistType cacheType = PersistType.AppDomain)
        {
            _persistType = cacheType;
        }

    }

}
