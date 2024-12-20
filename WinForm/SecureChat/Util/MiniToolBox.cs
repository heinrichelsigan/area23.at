using Area23.At.Framework.Library.Core.Cipher.Symm.Algo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.WinForm.SecureChat.Util
{
    internal static class MiniToolBox
    {

        internal static byte[] ToExternalBytes(this IPAddress? ip)
        {
            List<byte> bytes = new List<byte>();
            if (ip == null)
                return bytes.ToArray();

            string tmps = ip.ToString();

            if (!string.IsNullOrEmpty(tmps))
            {
                switch (ip.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        foreach (string ipv4Segment in tmps.Trim("{}".ToCharArray()).Split('.'))
                            bytes.Add(Convert.ToByte(ipv4Segment));
                        break;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        foreach (string ipv6Segment in tmps.Trim("[{}]".ToCharArray()).Split(':'))
                            bytes.Add(Convert.ToByte(ipv6Segment));
                        break;
                    default:
                        bytes.AddRange(ip.GetAddressBytes());
                        break;
                }
            }

            return bytes.ToArray();
        }
    
    
        internal static byte[] ToVersionBytes(this System.Version? version)
        {
            List<byte> bytes = new List<byte>();
            if (version == null)
                return bytes.ToArray();

            bytes.Add(Convert.ToByte(version.Major));
            bytes.Add(Convert.ToByte(version.Minor));
            bytes.Add(Convert.ToByte(version.Build % 256));
            bytes.Add(Convert.ToByte(version.Revision));

            return bytes.ToArray();
        }

        //internal static byte[] ScrambleBytes(params byte[] bytes)
        //{
        //    foreach ()
        //}
        
    }
}
