using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.CqrJD
{
    /// <summary>
    /// enum persist type
    /// </summary>
    public enum PersistType
    {
        None = 0,
        ApplicationState = 1,
        AmazonElasticCache = 2,
        FileSystem = 3
        // ReddisCache = 3,
    }

    /// <summary>
    /// PersistMsgIn 
    /// </summary>
    public static class PersistMsgIn
    {
        private static PersistType _persistMsgIn = PersistType.None;


        /// <summary>
        /// returns where message is persisted
        /// </summary>
        public static PersistType PersistMsg
        {
            get
            {
                string persistSet = "";
                if (ConfigurationManager.AppSettings[Constants.PERSIST_MSG_IN] != null)
                    persistSet = (string)ConfigurationManager.AppSettings[Constants.PERSIST_MSG_IN].ToString();

                if (!Enum.TryParse<PersistType>(persistSet, out _persistMsgIn))
                    _persistMsgIn = PersistType.ApplicationState;

                return _persistMsgIn;
            }
        }
    }

}