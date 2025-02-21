using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Area23.At.Framework.Core.Net.WebHttp
{

    public static class TcpClientWebRequest
    {
        const string TEST_HTTP_REQUEST_HEADER = @"GET /cqrsrv/tcpweb HTTP/1.1
Host: cqrxs.eu
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
Accept-Language: en-US,en;q=0.5
Accept-Encoding: deflate, br
Connection: keep-alive
Upgrade-Insecure-Requests: 1
Sec-Fetch-Dest: document
Sec-Fetch-Mode: navigate
Sec-Fetch-Site: same-origin
Priority: u=0, i
Pragma: no-cache
Cache-Control: no-cache";

        /// <summary>
        /// MakeWebRequest
        /// </summary>
        /// <param name="serverIp">server ip address</param>
        /// <param name="serverPort">server port (default 80)</param>
        /// <returns>client address as string</returns>
        public static string MakeWebRequest(IPAddress serverIp, out IPAddress clientIp, out IPAddress realClientIp, int serverPort = 80)
        {
            string? resp = string.Empty, respIpClient = string.Empty, respIpSrv = string.Empty;  
            try
            {
                IPEndPoint serverIep = new IPEndPoint(serverIp, serverPort);
                TcpClient tcpClient = new TcpClient();
                byte[] data = Encoding.UTF8.GetBytes(TEST_HTTP_REQUEST_HEADER);
                tcpClient.Connect(serverIep);
                tcpClient.Client.Send(data);
                // NetworkStream netStream = tcpClient.GetStream();
                // StreamWriter sw = new StreamWriter(netStream);
                // StreamReader sr = new StreamReader(netStream);
                // sw.Write(TEST_HTTP_REQUEST_HEADER);
                // sw.Flush();
                // byte[] outbuf = new byte[8192];
                // int read = tcpClient.Client.Receive(outbuf);
                // sr.BaseStream.Read(outbuf, 0, 8192);
                // tcpClient.Client.Poll(10, SelectMode.SelectRead);
                
                respIpClient = tcpClient.Client.LocalEndPoint?.ToString();
                realClientIp = IPAddress.Parse(respIpClient);
                resp = FormatResponseFromTcp(respIpClient);
                clientIp = IPAddress.Parse(resp);
                // sw.Close();
                // sr.Close();
                // netStream.Close();
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Area23Log.Logger.Log(ex);
                throw;
            }

            return resp ?? string.Empty;
        }


        /// <summary>
        /// MakeWebRequestAsync
        /// </summary>
        /// <param name="serverIp">server ip address</param>
        /// <param name="serverPort">server port (default 80)</param>
        /// <returns><see cref="Task{object}"/></returns>
        public static async Task<object> MakeWebRequestAsync(IPAddress serverIp, int serverPort = 80)
        {
            Task<object> makeTcpRequestTask = (Task<object>)await Task<object>.Run<object>(() =>
            {
                IPAddress clientIp, realClientIp;
                string clientIpStr = MakeWebRequest(serverIp, out clientIp, out realClientIp, serverPort);
                if (clientIp != null && !string.IsNullOrEmpty(clientIp.ToString()))
                {
                    byte[] data = clientIp.GetAddressBytes();
                    if (data != null && data.Length > 0)
                        return clientIp.ToString();
                }
                if (realClientIp != null && !string.IsNullOrEmpty(realClientIp.ToString()))
                {
                    byte[] data = realClientIp.GetAddressBytes();
                    if (data != null && data.Length > 0)
                        return realClientIp.ToString();
                }
                return clientIpStr;
            });

            return makeTcpRequestTask;
        }



        public static string? FormatResponseFromTcp(string? resp)
        {
            if (resp != null && resp.Contains("::ffff:"))
            {
                resp = resp?.Replace("::ffff:", "");
                if (resp != null && resp.Contains(':'))
                {
                    int lastch = resp.LastIndexOf(":");
                    resp = resp.Substring(0, lastch);
                }
                resp = resp?.Trim("[{()}]".ToCharArray());
            }

            return resp;
        }
    }

}
