using Area23.At.Framework.Library.Static;
using System;
using System.Configuration;

namespace Area23.At.Framework.Library.Cache
{

    /// <summary>
    /// enum persist type
    /// </summary>
    public enum PersistType
    {
        None = 0,
        AppDomain = 1,        
        JsonFile = 2,
        ApplicationState = 3,
        SessionState = 4
        // RedisValkey = 5,
        // RedisMS = 6
    }

    /// <summary>
    /// PersistInCache is a static class, which loads persistence type from Web.Config or App.Config 
    /// </summary>
    public static class PersistInCache
    {
        private static readonly PersistType _cacheType = PersistType.AppDomain;


        /// <summary>
        /// returns where message is persisted
        /// </summary>
        public static PersistType CacheType { get => _cacheType; }

        /// <summary>
        /// static ctor
        /// </summary>
        static PersistInCache()
        {
            string persistWhere = "";
            if (ConfigurationManager.AppSettings[Constants.PERSIST_MSG_IN] != null)
                persistWhere = (string)ConfigurationManager.AppSettings[Constants.PERSIST_MSG_IN].ToString();

            if (!Enum.TryParse<PersistType>(persistWhere, out _cacheType))
                _cacheType = PersistType.AppDomain;
        }
    }

}
