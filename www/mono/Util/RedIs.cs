using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.Util
{

    public class RedIs
    {
        private static readonly Lazy<RedIs> _instance = new Lazy<RedIs>(() => new RedIs());

        // ConnectionMultiplexer connMux;
        // ConfigurationOptions options;
        string endpoint = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
        // StackExchange.Redis.IDatabase db;

        public static RedIs Singleton => _instance.Value;

        //public static StackExchange.Redis.IDatabase Db
        //{
        //    get
        //    {
        //        if (_instance.Value.db == null)
        //            _instance.Value.db = ConnMux.GetDatabase();
        //        return _instance.Value.db;
        //    }
        //}

        //public static StackExchange.Redis.ConnectionMultiplexer ConnMux
        //{
        //    get
        //    {
        //        if (_instance.Value.connMux == null)
        //        {
        //            if (_instance.Value.options == null)
        //                _instance.Value.options = new ConfigurationOptions
        //                {
        //                    EndPoints = { _instance.Value.endpoint },
        //                    Ssl = true
        //                };
        //            _instance.Value.connMux = ConnectionMultiplexer.Connect(_instance.Value.options);
        //        }
        //        return _instance.Value.connMux;
        //    }
        //}



        public RedIs()
        {
            //endpoint = "cqrcachecqrxseu-53g0xw.serverless.eus2.cache.amazonaws.com:6379";
            //if (ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT] != null)
            //    endpoint = ConfigurationManager.AppSettings[Constants.VALKEY_CACHE_HOST_PORT].ToString();
            //options = new ConfigurationOptions
            //{
            //    EndPoints = { endpoint },
            //    Ssl = true
            //};
            //if (connMux == null)
            //    connMux = ConnectionMultiplexer.Connect(options);
            //if (db == null)
            //    db = connMux.GetDatabase();
        }


    }
}