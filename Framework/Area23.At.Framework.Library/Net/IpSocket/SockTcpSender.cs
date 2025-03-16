using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;

namespace Area23.At.Framework.Library.Net.IpSocket
{

    /// <summary>
    /// IpSocket.SockTcpSender encapsulation of tcp ipv46 sender 
    /// When using <see cref="SockTcpListener"/> as server, you must use <see cref="SockTcpSender"/> as client,
    /// when using <see cref="Listener"/> as server, you should use <see cref="Sender"/> as client.
    /// </summary>
    public static class SockTcpSender
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
            byte[] outbuf = new byte[1024];
            char[] charbuf = new char[1024];
            try
            {
                IPEndPoint serverIep = new IPEndPoint(serverIp, serverPort);
                TcpClient tcpClient = new TcpClient();
                byte[] data = EnDeCodeHelper.GetBytes(msg);
                // byte[] data = Encoding.UTF8.GetBytes(msg);
                tcpClient.SendBufferSize = Constants.MAX_SOCKET_BYTE_BUFFEER;
                //tcpClient.ReceiveBufferSize = Constants.MAX_SOCKET_BYTE_BUFFEER;
                tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);           
                //tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, Constants.MAX_SOCKET_BYTE_BUFFEER);
                // tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, Constants.MAX_SOCKET_BYTE_BUFFEER);
                tcpClient.Client.SendBufferSize = Constants.MAX_SOCKET_BYTE_BUFFEER;
                tcpClient.Connect(serverIep);
                tcpClient.Client.SendTimeout = 16000;
                // int ssize = tcpClient.Client.Send(data, 0, data.Length, SocketFlags.None, out SocketError errorCode);
                // if (ssize < msg.Length) ;
                resp = tcpClient.Client.LocalEndPoint?.ToString();

                string tcpClientSettings = "address/port: " + resp + " , TcpClient.Client.SocketType = " + tcpClient.Client.SocketType + ",\n" +
                    " TcpClient SendBufferSize    = " + tcpClient.SendBufferSize + " TcpClient.SendTimeOut = " + tcpClient.SendTimeout + "\n" +
                    " TcpCLient.ReceiveBufferSize = " + tcpClient.ReceiveBufferSize + " TcpClient.ReceiveTimeout = " + tcpClient.ReceiveTimeout + "\n" +
                    " TcpClient.Client.SendBufferSize = " + tcpClient.Client.SendBufferSize + " TcpClient.Client.SendTimeOut = " + tcpClient.Client.SendTimeout + "\n" +
                    " TcpClient.Client.ReceiveBufferSize = " + tcpClient.Client.ReceiveBufferSize + " TcpClient.Client.ReceiveTimeout = " + tcpClient.Client.ReceiveTimeout + "\n" +
                    " TcpClient.Client.Ttl = " + tcpClient.Client.Ttl + " TcpClient.Client.NoDelay = " + tcpClient.Client.NoDelay + ",\n" +
                    " TcpClient.Client.Blocking = " + tcpClient.Client.Blocking + ";\n";
                Area23Log.LogStatic("Client: " + tcpClientSettings);
                                           

                using (NetworkStream netStream = tcpClient.GetStream())
                {
                    netStream.Write(data, 0, data.Length);
                    netStream.Read(outbuf, 0, outbuf.Length);                    
                    netStream.Flush();                    
                    //using (StreamReader sr = new StreamReader(netStream))
                    //{                        
                    //    sr.BaseStream.Read(outbuf, 0, outbuf.Length);
                    //}
                }
                // sw.Close();
                // sr.Close();
                // netStream.Close();

                string rs = EnDeCodeHelper.GetString(outbuf);
                // string rs = "";
                // foreach (char ch in charbuf) rs += ch;
                if (Int32.TryParse(rs, out int rsize))
                {
                    Area23Log.LogStatic($"msg.Length = {msg.Length}, rsize = {rsize}\n");
                }
                // sr.BaseStream.Read(outbuf, 0, 8192);
               
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

                // tcpClient.Client.Shutdown(SocketShutdown.Both);
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
