using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Area23.At.Framework.Library.Net.NameService
{


    /// <summary>
    /// Dns Domain Name Service Helper
    /// </summary>
    public static class DnsHelper
    {

        #region DnsHelper

        /// <summary>
        /// GetHostEntryByHostName gets an IPHostEntry for a dns hostname
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <returns><see cref="IPHostEntry"/></returns>
        public static IPHostEntry GetHostEntryByHostName(string hostname = "")
        {
            hostname = string.IsNullOrEmpty(hostname) ? Dns.GetHostName() : hostname;
            IPHostEntry host = Dns.GetHostEntry(hostname);

            return host;
        }

        /// <summary>
        /// GetIpAddrsByHostName get all ip addresses except loopback for a dns hostname
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <returns><see cref="List{IPAddress}">IEnumerable{IPAddress}</see></returns>
        public static List<IPAddress> GetIpAddrsByHostName(string hostname = "")
        {
            IPHostEntry ipHost = GetHostEntryByHostName(hostname);
            List<IPAddress> ipList = (from ip in ipHost.AddressList where !IPAddress.IsLoopback(ip) select ip).ToList();
            return ipList;
        }


        /// <summary>
        /// GetDnsHostNamesByHostName gets official reverse lookup hostname for a hostname
        /// </summary>
        /// <param name="hostname"></param>
        /// <returns><see cref="IList{string}"/></returns>
        public static IList<string> GetHostNamesByHostName(string hostname = "")
        {
            List<string> hostnames = new List<string>();
            string lastAdded = string.Empty;
            foreach (IPAddress ip in GetIpAddrsByHostName(hostname))
            {
                try
                {
                    lastAdded = Dns.GetHostEntry(ip).HostName;
                }
                catch (Exception ex)
                {
                    Area23Log.LogOriginMsgEx("DnsHelper", "GetHostNamesByHostName(hostname = " + hostname + ") Exception", ex);
                }

                if (!string.IsNullOrEmpty(lastAdded) && !hostnames.Contains(lastAdded))
                    hostnames.Add(lastAdded);
            }

            return hostnames;
        }



        #endregion dns

    }


}
