using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Concurrent;
using System.Web;

namespace Area23.At.Framework.Library.Cache
{

    /// <summary>
    /// CacheHashDict an application cache implemented with a <see cref="ConcurrentDictionary{string, CacheValue}"/> saved in <see cref="HttpApplicationState"/>
    /// </summary>
    public class ApplicationStateCache : MemoryCache
    {

        protected internal static readonly object _smartLock = new object();

        public static new string CacheVariant = "ApplicationStateCache";

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

                _timePassedSinceLastRW = DateTime.Now.Subtract(_lastCacheRW);

                if (HttpContext.Current != null && HttpContext.Current.Application != null &&
                    HttpContext.Current.Application[Constants.APP_CONCURRENT_DICT] != null)
                {
                    lock (_smartLock)
                    {
                        try
                        {
                            _appDict = (ConcurrentDictionary<string, CacheValue>)HttpContext.Current.Application[Constants.APP_CONCURRENT_DICT];
                            _lastCacheRW = DateTime.Now;                        
                        }
                        catch (Exception ex)
                        {
                            Area23.At.Framework.Library.Util.Area23Log.LogStatic(ex);
                        }
                    }
                }


                if (_appDict == null)
                {
                    lock (_smartLock)
                    {
                        _appDict = new ConcurrentDictionary<string, CacheValue>();
                        HttpContext.Current.Application[Constants.APP_CONCURRENT_DICT] = _appDict;
                        _lastCacheRW = DateTime.Now;
                    }
                }

                return _appDict;            
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
                if (cacheDict != null && cacheDict.Count > 0)
                {
                    _appDict = cacheDict;
                    if (HttpContext.Current != null && HttpContext.Current.Application != null)
                    {
                        HttpContext.Current.Application[Constants.APP_CONCURRENT_DICT] = _appDict;
                        _lastCacheRW = DateTime.Now;
                    }
                }
            }
        }


        public ApplicationStateCache() : this(PersistType.ApplicationState) { }

        public ApplicationStateCache(PersistType cacheType)
        {
            _persistType = (cacheType == PersistType.ApplicationState) ? cacheType : PersistType.ApplicationState;
        }

    }

}
