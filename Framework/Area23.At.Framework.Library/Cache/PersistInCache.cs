using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Cache
{

    /// <summary>
    /// enum persist type
    /// </summary>
    public enum PersistType
    {
        None = 0,
        AppDomain = 1,
        Redis = 2,
        JsonFile = 3,
        ApplicationState = 4
        // ReddisCache = 3,
    }

    /// <summary>
    /// PersistMsgIn 
    /// </summary>
    public static class PersistInCache
    {
        private static readonly PersistType _cacheType = PersistType.AppDomain;


        /// <summary>
        /// returns where message is persisted
        /// </summary>
        public static PersistType CacheType
        {
            get => _cacheType;
        }

        static PersistInCache()
        {
            string persistSet = "";
            if (ConfigurationManager.AppSettings[Constants.PERSIST_MSG_IN] != null)
                persistSet = (string)ConfigurationManager.AppSettings[Constants.PERSIST_MSG_IN].ToString();

            if (!Enum.TryParse<PersistType>(persistSet, out _cacheType))
                _cacheType = PersistType.AppDomain;
        }
    }

}
