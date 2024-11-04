using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Net
{
    public static class MyAddr
    {

        public static IEnumerable<IPAddress> GetIpAddresses()
        {
            IEnumerable<IPAddress> ipAddrs = (from address in
                NetworkInterface.GetAllNetworkInterfaces().Select(x => x.GetIPProperties()).SelectMany(x => x.UnicastAddresses).Select(x => x.Address)
                                              where !IPAddress.IsLoopback(address) && address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                                              select address);

            return ipAddrs;
        }

        public static IEnumerable<IPAddress> GetIpAddrsByHostName()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            IEnumerable<IPAddress> ipList = (from ip in host.AddressList where !IPAddress.IsLoopback(ip) select ip).ToList();
            return ipList;
        }


        public static IEnumerable<PhysicalAddress> GetMacAddress()
        {
            IEnumerable<PhysicalAddress> macAddrs =
                (
                    from nic in NetworkInterface.GetAllNetworkInterfaces()
                    where nic.OperationalStatus == OperationalStatus.Up
                    select nic.GetPhysicalAddress()
                );

            return macAddrs;
        }
    }

}
