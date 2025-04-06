using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;

namespace Area23.At.Framework.Core.Net.IpSocket
{

    /// <summary>
    /// IpSocket.Sender encapsulation of tcp ipv46 sender 
    /// When using <see cref="SockTcpListener"/> as server, you must use <see cref="SockTcpSender"/> as client,
    /// when using <see cref="Listener"/> as server, you should use <see cref="Sender"/> as client.
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
            string? resp = string.Empty;
            TcpClient? tcpClient = null;
            try
            {
                IPEndPoint serverIep = new IPEndPoint(serverIp, serverPort);
                tcpClient = new TcpClient();
                byte[] data = EnDeCodeHelper.GetBytes(msg);
                //Span<byte> spanBuffer = new Span<byte>(data);
                // byte[] data = Encoding.UTF8.GetBytes(msg);
                tcpClient.SendBufferSize = Constants.MAX_SOCKET_BYTE_BUFFEER;
                tcpClient.ReceiveBufferSize = Constants.MAX_SOCKET_BYTE_BUFFEER;
                // tcpClient.NoDelay = true;
                // tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontFragment, true);
                // tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
                // tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontFragment, true);
                tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, Constants.MAX_SOCKET_BYTE_BUFFEER);
                tcpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, Constants.MAX_SOCKET_BYTE_BUFFEER);
                tcpClient.Client.SendBufferSize = Constants.MAX_SOCKET_BYTE_BUFFEER;
                tcpClient.Connect(serverIp, serverPort);

                // tcpClient.Client.NoDelay = true;
                tcpClient.Client.SendTimeout = 16000;
                
                // if (ssize < msg.Length) ;
                byte[] outbuf = new byte[32];
                //using (NetworkStream netStream = tcpClient.GetStream())
                //{
                //    using (StreamWriter sw = new StreamWriter(netStream))
                //    {
                //        sw.Write(msg);
                //        sw.Flush();
                //    }
                //    using (StreamReader sr = new StreamReader(netStream))
                //    {                        
                //        sr.Read(charbuf, 0, charbuf.Length);
                //    }
                //}
                int ssize = 0, fsize = 0;

                // Send full data length before sending 
                byte[] sendData = Encoding.Default.GetBytes(data.Length.ToString());
                tcpClient.Client.Send(sendData, SocketFlags.None);
                
                // We must receive full size + " " + ACK
                int read = tcpClient.Client.Receive(outbuf, SocketFlags.None);
                string resp1 = EnDeCodeHelper.GetString(outbuf);
                if (!resp1.Equals(data.Length.ToString() +  " " + Constants.ACK))
                    ; // rtodo i+nvli+d prorocoll

                // repeat until send all data
                while (fsize < data.Length)
                {
                    ssize = tcpClient.Client.Send(data, fsize, data.Length, SocketFlags.None, out SocketError errorCode);
                    Area23Log.LogStatic($"Socket send: data.len = {data.Length}, offset = {fsize} SocketError = {errorCode.ToString()} \n");

                    

                    outbuf = new byte[32];
                    read = tcpClient.Client.Receive(outbuf, SocketFlags.None);
                    string rs = EnDeCodeHelper.GetString(outbuf);
                    if (Int32.TryParse(rs, out int rsize))
                    {
                        if (ssize != rsize)
                        {
                            fsize += rsize;
                            Area23Log.LogStatic($"msg.Length = {msg.Length}, fsize = {fsize}, rsize = {rsize}\n");                            
                        }
                        else
                            fsize += ssize;
                    }
                    Thread.Sleep(5);
                }
                              
                // compare bytes total read / send with initial length
                //read = tcpClient.Client.Receive(outbuf, SocketFlags.None);
                //string rc = EnDeCodeHelper.GetString(outbuf);
                //if (Int32.TryParse(rc, out int rsize))
                //{
                //    if (fsize != rsize)
                //    {
                //        Area23Log.LogStatic($"msg.Length = {msg.Length}, ssize = {ssize}, rsize = {rsize}\n");
                //        throw new IndexOutOfRangeException($"msg.Length = {msg.Length}, ssize = {ssize}, rsize = {rsize}");
                //    }
                //}
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
                Thread.Sleep(125);
                tcpClient.Client.Shutdown(SocketShutdown.Both);
                Thread.Sleep(125);
                // tcpClient.Close();
            }
            catch (Exception ex)
            {
                Area23Log.Logger.Log(ex);
                throw;
            }
            finally
            {
                if (tcpClient != null)
                {
                    try
                    {
                        tcpClient.Close();
                    }
                    catch (Exception ex)
                    {
                        Area23Log.Logger.Log(ex);
                    }
                    try
                    {
                        tcpClient.Dispose();
                    }
                    catch { }
                }
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
