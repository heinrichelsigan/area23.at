using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Net.IpSocket
{

    /// <summary>
    /// IpSocket.Sender encapsulation of tcp ipv46 sender 
    /// </summary>
    public static class Sender
    {


        /// <summary>
        /// Send
        /// </summary>
        /// <param name="serverIp">server ip address</param>
        /// <param name="msg">msg to send</param>
        /// <param name="serverPort">server port (default 7777)</param>
        /// <returns>client address as string</returns>
        public static string Send(IPAddress serverIp, string msg, int serverPort = 7777)
        {
            string resp = string.Empty;
            try
            {
                IPEndPoint serverIep = new IPEndPoint(serverIp, serverPort);
                TcpClient tcpClient = new TcpClient();
                byte[] data = EnDeCodeHelper.GetBytes(msg);
                // byte[] data = Encoding.UTF8.GetBytes(msg);
                tcpClient.SendBufferSize = Constants.MAX_BYTE_BUFFEER;
                tcpClient.Connect(serverIep);
                tcpClient.Client.Send(data);
                // NetworkStream netStream = tcpClient.GetStream();
                // StreamWriter sw = new StreamWriter(netStream);
                // StreamReader sr = new StreamReader(netStream);
                // sw.Write(msg);
                // sw.Flush();
                // byte[] outbuf = new byte[8192];
                // int read = tcpClient.Client.Receive(outbuf);
                // sr.BaseStream.Read(outbuf, 0, 8192);
                resp = tcpClient.Client.LocalEndPoint?.ToString();
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
                // sw.Close();
                // sr.Close();
                // netStream.Close();
                tcpClient.Close();
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("Sender", "Send(...) throwed Exception " + ex.GetType(), ex);
                throw;
            }

            return resp ?? string.Empty;
        }


        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="serverIp">server ip address</param>
        /// <param name="msg">msg to send</param>
        /// <param name="serverPort">server port (default 7777)</param>
        /// <returns><see cref="Task{object}"/></returns>
        public static async Task<object> SendAsync(IPAddress serverIp, string msg, int serverPort = 7777)
        {
            Task<object> sendTaskAsync = (Task<object>)await Task<object>.Run<object>(() =>
            {
                string response = Send(serverIp, msg, serverPort);
                return response;
            });

            return sendTaskAsync;
        }


    }

}
