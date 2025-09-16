using Area23.At.Framework.Core.Util;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Area23.At.Framework.Core.Net.WebHttp
{

    /// <summary>
    /// <see cref="TcpClientWebRequest"/> performs web request raw on tcp socket 
    /// </summary>
    public static class TcpClientWebRequest
    {


        const string TEST_HTTP_REQUEST_HEADER = @"GET /net/R.aspx HTTP/1.1
Host: cqrxs.eu
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
Accept-Language: en-US,en;q=0.5
Accept-Encoding: deflate, br
Connection: keep-alive
User-Agent: cqrxs.eu test agent [.Net Core C# 9.0]
Priority: u=0, i
Pragma: no-cache
Cache-Control: no-cache";

        /// <summary>
        /// MakeWebRequest
        /// </summary>
        /// <param name="serverIp">server ip address</param>
        /// <param name="serverPort">server port (default 80)</param>
        /// <returns>client address as string</returns>
        public static string MakeWebRequest(IPAddress serverIp, out List<IPAddress> clientIPs, int serverPort = 80)
        {
            string? resp = string.Empty, respIpClient = string.Empty, respIpSrv = string.Empty;
            IPAddress? clientIp = null, realClientIp = null, clientIpFromServer = null;
            clientIPs = new List<IPAddress>();
            IPEndPoint serverIep;
            TcpClient tcpClient = new TcpClient();
            try
            {
                serverIep = new IPEndPoint(serverIp, serverPort);
                byte[] data = Encoding.UTF8.GetBytes(TEST_HTTP_REQUEST_HEADER);
                tcpClient.SendTimeout = 1000;
                tcpClient.ReceiveTimeout = 1000;
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
                clientIPs.Add(realClientIp);
                resp = FormatResponseFromTcp(respIpClient);
                clientIp = IPAddress.Parse(resp);
                clientIPs.Add(clientIp);
                // string resonseFromWebTcp = System.Text.Encoding.UTF8.GetString(outbuf);
                // string responeExtIp = resonseFromWebTcp.GetSubStringByPattern("<body>", true, "", "</body>", false, StringComparison.InvariantCultureIgnoreCase);
                // Console.Error.WriteLine("external ip from response: " + responeExtIp);
                // clientIpFromServer = IPAddress.Parse(resonseFromWebTcp);
                // clientIPs.Add(clientIpFromServer);
                // sw.Close();
                // sr.Close();
                // netStream.Close();
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Area23Log.Log(ex);
                throw;
            }
            finally
            {
                if (tcpClient != null)
                {
                    tcpClient.Close();
                    tcpClient.Dispose();
                }
                tcpClient = null;
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
                IPAddress? clientIp = null, realClientIp = null, clientIpFromServer = null;
                List<IPAddress> clientIPs = new List<IPAddress>();
                string clientIpStr = MakeWebRequest(serverIp, out clientIPs, serverPort);
                
                realClientIp = (clientIPs != null && clientIPs.Count > 0) ? clientIPs.ElementAt(0) : null;
                clientIp = (clientIPs != null && clientIPs.Count > 1) ? clientIPs.ElementAt(1) : null;
                clientIpFromServer = (clientIPs != null && clientIPs.Count > 2) ? clientIPs.ElementAt(2) : null;
                
                string cips = string.Empty;
                if (realClientIp != null)
                    cips += realClientIp;
                if (clientIp != null)
                    cips += (string.IsNullOrEmpty(cips) ? "" : ";")  + realClientIp;
                if (clientIpFromServer != null)
                    cips += (string.IsNullOrEmpty(cips) ? "" : ";") + clientIpFromServer;
                
                return cips;
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
