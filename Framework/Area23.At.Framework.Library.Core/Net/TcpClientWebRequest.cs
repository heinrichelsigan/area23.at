using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Net
{
    public static class TcpClientWebRequest
    {
        const string TEST_HTTP_REQUEST_HEADER = @"GET / HTTP/1.1
Host: heinrichelsigan.area23.at
User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:133.0) Gecko/20100101 Firefox/133.0
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8
Accept-Language: en-US,en;q=0.5
Accept-Encoding: gzip, deflate, br, zstd
Connection: keep-alive
Upgrade-Insecure-Requests: 1
Sec-Fetch-Dest: document
Sec-Fetch-Mode: navigate
Sec-Fetch-Site: same-origin
If-Modified-Since: Mon, 02 Dec 2024 04:17:07 GMT
If-None-Match: ""1c3e-62841d2582843-gzip""
Priority: u=0, i
Pragma: no-cache
Cache-Control: no-cache";

        public static string MakeWebRequest(IPAddress serverIp, int serverPort = 80)
        {
            string? resp = string.Empty;
            try
            {
                IPEndPoint serverIep = new IPEndPoint(serverIp, serverPort);
                TcpClient tcpClient = new TcpClient();                
                byte[] data = Encoding.ASCII.GetBytes(TEST_HTTP_REQUEST_HEADER);
                tcpClient.Connect(serverIep);
                // tcpClient.Client.Send(data);
                NetworkStream netStream = tcpClient.GetStream();
                StreamWriter sw = new StreamWriter(netStream);
                StreamReader sr = new StreamReader(netStream);
                sw.Write(TEST_HTTP_REQUEST_HEADER);
                sw.Flush();
                byte[] outbuf = new byte[8192];
                // int read = tcpClient.Client.Receive(outbuf);
                sr.BaseStream.Read(outbuf, 0, 8192);                
                resp = tcpClient.Client.LocalEndPoint?.ToString();
                // if (resp.Contains("[::ffff:"))
                //     resp = resp?.Replace("[::ffff:", "[");
                sw.Close();
                sr.Close();
                netStream.Close();
                tcpClient.Close();                
            }
            catch (Exception ex)
            {
                Area23Log.Logger.Log(ex);
                throw;
            }

            return resp ?? string.Empty;
        }
    }
    
}
