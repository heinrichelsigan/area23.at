﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Area23.At.Framework.Library.Core.Net.WebHttp;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace Area23.At.Framework.Library.Core.Net
{

    /// <summary>
    /// NetworkAddresses provides several members to get all local network addresses (except loopback)
    /// </summary>
    public static class NetworkAddresses
    {


        /// <summary>
        /// GetConnectedIpAddresses gets connected IPAddress list.
        /// </summary>
        /// <param name="serverIps"><see cref="List{IPAddress}"/></param>
        /// <returns><see cref="List{IPAddress}"/></returns>
        public static List<IPAddress> GetConnectedIpAddresses(List<IPAddress>? serverIps = null)
        {
            List<IPAddress> validAddrs = new List<IPAddress>();
            if (serverIps == null || serverIps.Count == 0)
            {
                serverIps = new List<IPAddress>();
                foreach (IPAddress serverIp in GetIpAddrsByHostName("cqrxs.eu"))
                    serverIps.Add(serverIp);
                foreach (IPAddress serverIp in GetIpAddrsByHostName("paris.area23.at"))
                    serverIps.Add(serverIp);
                try
                {
                    foreach (IPAddress serverIp in GetIpAddrsByHostName("virginia.area23.at"))
                        serverIps.Add(serverIp);
                }
                catch (Exception exVirginia)
                {
                    Area23Log.LogStatic(exVirginia);
                }
                try
                {
                    foreach (IPAddress serverIp in GetIpAddrsByHostName("parisienne.area23.at"))
                        serverIps.Add(serverIp);
                }
                catch (Exception exParisienne)
                {
                    Area23Log.LogStatic(exParisienne);
                }
            }

            foreach (IPAddress serverIp in serverIps)
            {
                IPAddress clientIp;
                string resp = string.Empty;
                try
                {
                    resp = TcpClientWebRequest.MakeWebRequest(serverIp);

                    clientIp = IPAddress.Parse(resp);
                    if (!validAddrs.Contains(clientIp))
                        validAddrs.Add(clientIp);
                } 
                catch (Exception ex)
                {
                    Area23Log.LogStatic(ex);
                }
            }

            return validAddrs;
        }



        /// <summary>
        /// GetConnectedIpAddressesAsync
        /// </summary>
        /// <param name="serverIps">serverIPs List to connect and verify, if connection is possible through</param>
        /// <returns><see cref="Task{List{IPAddress}}"/></returns>
        public static async Task<List<IPAddress>> GetConnectedIpAddressesAsync(List<IPAddress>? serverIps = null)
        {
            return await Task<List<IPAddress>>.Run<List<IPAddress>>(() => (GetConnectedIpAddresses(serverIps)));
        }

        public static async Task<object> ConnectedIpAddressesAsync(List<IPAddress>? serverIps = null)
        {
            Task<object> valueTask = (Task<object>)await Task<object>.Run<object>(() =>
            {
                return (object)(GetConnectedIpAddresses(serverIps).ToArray());
            });

            return valueTask;
        }


        /// <summary>
        /// GetIpAddresses gets all IPAddresses except loopback adapter
        /// </summary>
        /// <returns><see cref="IEnumerable{IPAddressT}"/></returns>
        public static List<IPAddress> GetIpAddresses()
        {
            IEnumerable<IPAddress> ipAddrs =
                from address in NetworkInterface.GetAllNetworkInterfaces().Select(
                    x => x.GetIPProperties()).SelectMany(x => x.UnicastAddresses).Select(x => x.Address)
                where // !IPAddress.IsLoopback(address) &&
                        (address.AddressFamily == AddressFamily.InterNetwork ||
                         address.AddressFamily == AddressFamily.InterNetworkV6)
                // || address.AddressFamily == AddressFamily.Unix
                select address;

            return ipAddrs.ToList();
        }


        public static async Task<List<IPAddress>> GetIpAddressesAsync()
        {
            return await Task<List<IPAddress>>.Run<List<IPAddress>>(() => (GetIpAddresses()));
        }


        /// <summary>
        /// GetIpAddresses returns all IPAddresses for a certain <see cref="AddressFamily>AddressFamily</see>
        /// </summary>
        /// <param name="addressFamily">only <see cref="AddressFamily.InterNetwork"/>.
        /// <see cref="AddressFamily.InterNetworkV6">AddressFamily.InterNetworkV6</see> and 
        /// <seealso cref="AddressFamily.Unix"/> are supported.</param>
        /// <returns><see cref="IEnumerable{IPAddress}"/></returns>
        /// <exception cref="ProtocolViolationException"></exception>
        public static IEnumerable<IPAddress> GetIpAddresses(AddressFamily addressFamily)
        {
            switch (addressFamily)
            {
                case AddressFamily.Unix:
                case AddressFamily.InterNetwork:
                case AddressFamily.InterNetworkV6:
                    break;
                default:
                    string? addrFamily = Enum.GetName(typeof(AddressFamily), addressFamily);

                    Enum.Parse(typeof(AddressFamily), addressFamily.ToString(), true);
                    throw new ProtocolViolationException(
                        $"System.Net.Sockets.AddressFamily {addrFamily} value {Convert.ToUInt32((int)addressFamily)} is not supported! " +
                        $"Only AddressFamily Unix Internetwork InterNetworkV6 are supported.");
                    break;
            }

            IEnumerable<IPAddress> ipAddrs = from address in
                NetworkInterface.GetAllNetworkInterfaces().Select(x => x.GetIPProperties()).SelectMany(x => x.UnicastAddresses).Select(x => x.Address)
                                             where !IPAddress.IsLoopback(address) && address.AddressFamily == addressFamily
                                             select address;

            return ipAddrs;
        }


        /// <summary>
        /// GetMacAddress returns Mac Address
        /// </summary>
        /// <returns><see cref="IEnumerable{PhysicalAddress}"/></returns>
        public static IEnumerable<PhysicalAddress> GetMacAddress()
        {
            IEnumerable<PhysicalAddress> macAddrs =

                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress()
                ;

            return macAddrs;
        }

        #region dns

        /// <summary>
        /// GetIpAddrsByHostName get all ip addresses except loopback for a dns hostname
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <returns><see cref="IEnumerable{IPAddress}">IEnumerable{IPAddress}</see></returns>
        public static IEnumerable<IPAddress> GetIpAddrsByHostName(string hostname = "")
        {
            var host = GetHostEntryByHostName(hostname);
            IEnumerable<IPAddress> ipList = (from ip in host.AddressList where !IPAddress.IsLoopback(ip) select ip).ToList();
            return ipList;
        }

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
        /// GetDnsHostNamesByHostName gets official reverse lookup hostname for a hostname
        /// </summary>
        /// <param name="hostname"></param>
        /// <returns><see cref="IList{string}"/></returns>
        public static IList<string> GetDnsHostNamesByHostName(string hostname = "")
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
                }

                if (!string.IsNullOrEmpty(lastAdded) && !hostnames.Contains(lastAdded))
                    hostnames.Add(lastAdded);
            }

            return hostnames;
        }

        #endregion dns

    }

}
