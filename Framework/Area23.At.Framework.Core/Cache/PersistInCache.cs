using Area23.At.Framework.Core.Static;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Cache
{
    /// <summary>
    /// enum persist type
    /// </summary>
    public enum PersistType
    {
        Memory = 0,
        AppDomain = 1,
        RedisValkey = 2,
        JsonFile = 3,
        ApplicationState = 4,
        RedisMS = 5
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
            string persistWhere = "AppDomain";
            if (ConfigurationManager.AppSettings[Constants.PERSIST_MSG_IN] != null)
                persistWhere = (string)ConfigurationManager.AppSettings[Constants.PERSIST_MSG_IN].ToString();

            if (!Enum.TryParse<PersistType>(persistWhere, out _cacheType))
                _cacheType = PersistType.AppDomain;
        }
    }

}
