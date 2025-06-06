﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Area23.At.Framework.Core.Net.WebHttp;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using Area23.At.Framework.Core.Net.NameService;
using Area23.At.Framework.Core.Util;
using Area23.At.Framework.Core.Static;

namespace Area23.At.Framework.Core.Net
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
            }

            foreach (IPAddress serverIp in DnsHelper.GetIpAddrsByHostName(Constants.CQRXS_EU))
                if (!serverIps.Contains(serverIp))
                    serverIps.Add(serverIp);
            // foreach (IPAddress serverIp in DnsHelper.GetIpAddrsByHostName(Constants.PARIS_CQRXS_EU))
            //     serverIps.Add(serverIp);

            try
            {
                foreach (IPAddress serverIp in DnsHelper.GetIpAddrsByHostName(Constants.IPV6_CQRXS_EU))
                    if (!serverIps.Contains(serverIp))
                        serverIps.Add(serverIp);
            }
            catch (Exception exV6)
            {
                SLog.Log(exV6);
            }               
            //try
            //{
            //    foreach (IPAddress serverIp in DnsHelper.GetIpAddrsByHostName(Constants.PARISIENNE_CQRXS_EU))
            //      serverIps.Add(serverIp);
            //}
            //catch (Exception exParisienne)
            //{
            //    SLog.Log(exParisienne);
            //}

            foreach (IPAddress serverIp in serverIps)
            {
                List<IPAddress> clientIPs = new List<IPAddress>();
                string resp = string.Empty;
                try
                {
                    resp = TcpClientWebRequest.MakeWebRequest(serverIp, out clientIPs);
                    foreach (IPAddress cIp in clientIPs)
                    {
                        if (cIp.IsIPv4MappedToIPv6)
                        {
                            IPAddress tmpIp = cIp.MapToIPv4();
                            if (!validAddrs.Contains(tmpIp))
                                validAddrs.Add(tmpIp);
                            continue;
                        }
                        if (!validAddrs.Contains(cIp))
                            validAddrs.Add(cIp);
                    }
                }
                catch (Exception ex)
                {
                    SLog.Log(ex);
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
                    (address.AddressFamily == AddressFamily.InterNetworkV6 &&
                        (!address.IsIPv6LinkLocal || address.IsIPv6Multicast ||
                            address.IsIPv6SiteLocal || address.IsIPv6Teredo)))                            
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
        /// <returns><see cref="IList{IPAddress}"/></returns>
        /// <exception cref="ProtocolViolationException"></exception>
        public static IList<IPAddress> GetIpAddresses(AddressFamily addressFamily)
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

            return ipAddrs.ToList();
        }


        /// <summary>
        /// GetMacAddress returns Mac Address
        /// </summary>
        /// <returns><see cref="IList{PhysicalAddress}"/></returns>
        public static IList<PhysicalAddress> GetMacAddress()
        {
            IEnumerable<PhysicalAddress> macAddrs =

                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress()
                ;

            return macAddrs.ToList();
        }


    }

}
