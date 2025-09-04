using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web;
using System.Windows.Input;

namespace Area23.At.Framework.Library.Cache
{

    /// <summary>
    /// SessionStateCache is implemented directly via <see cref="HttpContext.Current.Session" />
    /// 
    /// </summary>
    public class SessionStateCache : MemoryCache
    {

        protected internal static readonly object _smartLock = new object();

        public static new string CacheVariant = "SessionStateCache";


        public SessionStateCache() : this(PersistType.SessionState) { }

        public SessionStateCache(PersistType cacheType)
        {
            _persistType = (cacheType == PersistType.SessionState) ? cacheType : PersistType.SessionState;
            _allKeys = GetAllKeys();
            LoadDictionaryCache(false);
        }
        
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

            if (HttpContext.Current != null && HttpContext.Current.Session != null &&
                HttpContext.Current.Session[Constants.APP_CONCURRENT_DICT] != null)
            {
                lock (_smartLock)
                {
                    try
                    {
                        _appDict = (ConcurrentDictionary<string, CacheValue>)HttpContext.Current.Session[Constants.APP_CONCURRENT_DICT];
                        _lastCacheRW = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        Area23.At.Framework.Library.Util.Area23Log.LogOriginMsgEx("SessionStateCache",
                            "LoadDictionaryCache(repeatLoadingPeriodically=" + repeatLoadingPeriodically + ") throwed Exception " + ex.GetType(), ex);
                    }
                }
            }

            if (_appDict == null)
            {
                lock (_smartLock)
                {
                    _appDict = new ConcurrentDictionary<string, CacheValue>();
                    HttpContext.Current.Session[Constants.APP_CONCURRENT_DICT] = _appDict;
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
                    if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session[Constants.APP_CONCURRENT_DICT] = _appDict;
                        _lastCacheRW = DateTime.Now;
                    }
                }
            }
        }

        /*

        
        public override string GetString(string ckey)
        {
            if (!string.IsNullOrEmpty(ckey))
            {
                lock (_lock)
                {
                    if (HttpContext.Current != null && HttpContext.Current.Session != null)
                    {
                        foreach (string skey in HttpContext.Current.Session.Keys)
                        {
                            if (ckey.Equals(skey))
                                return (string)HttpContext.Current.Session[ckey];
                        }
                    }
                }
            }

            return null;
        }

        public override T GetValue<T>(string ckey)
        {
            lock (_outerlock)
            {
                if (!string.IsNullOrEmpty(ckey) && HttpContext.Current != null && HttpContext.Current.Session != null)
                {                        
                    lock (_lock)
                    {
                        foreach (string skey in HttpContext.Current.Session.Keys)
                        {
                            if (ckey.Equals(skey))
                                return (T)HttpContext.Current.Session[ckey];
                        }
                    }
                }               
            }

            return default(T);
        }

        public override Nullable<T> GetNullableValue<T>(string ckey) 
        {
            lock (_outerlock)
            {
                Nullable<T> nullableT = null;

                if (!string.IsNullOrEmpty(ckey) && HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    lock (_lock)
                    {
                        foreach (string skey in HttpContext.Current.Session.Keys)
                        {
                            if (ckey.Equals(skey))
                                return new Nullable<T>((T)HttpContext.Current.Session[ckey]);
                        }
                    }
                }

                return nullableT;
            }            
        }

        public override bool SetString(string ckey, string svalue)
        {
            lock (_outerlock)
            {
                if (!string.IsNullOrEmpty(ckey) && HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    lock (_lock)
                    {
                        HttpContext.Current.Session[ckey] = svalue;
                        return true;
                    }                    
                }
                return false;
            }
        }

        public override bool SetValue<T>(string ckey, T tvalue)
        {
            lock (_outerlock)
            {
                if (!string.IsNullOrEmpty(ckey) && HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    lock (_lock)
                    {
                        HttpContext.Current.Session[ckey] = tvalue;
                        return true;
                    }
                }                
            }
            return false;
        }


        public override bool ContainsKey(string ckey)
        {
            lock (_outerlock)
            {
                if (!string.IsNullOrEmpty(ckey) && HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    lock (_lock)
                    {
                        foreach (string skey in HttpContext.Current.Session.Keys)
                        {
                            if (ckey.Equals(skey))
                                return true;
                        }
                    }
                }                
            }
            return false;
        }

        public override bool RemoveKey(string ckey)
        {
            lock (_outerlock)
            {
                if (!string.IsNullOrEmpty(ckey) && HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    lock (_lock)
                    {
                        foreach (string skey in HttpContext.Current.Session.Keys)
                        {
                            if (ckey.Equals(skey))
                            {
                                HttpContext.Current.Session.Remove(ckey);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public override HashSet<string> GetAllKeys()
        {
            lock (_outerlock)
            {
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    lock (_lock)
                    {
                        _allKeys = new HashSet<string>();
                        foreach (string skey in HttpContext.Current.Session.Keys)
                            _allKeys.Add(skey);
                    }
                }
                return _allKeys;
            }
        }
        */
    }

}
