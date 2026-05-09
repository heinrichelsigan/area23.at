using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Media;

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
            int bufsze = 65536 * 2;
            try
            {
                if (!msg.EndsWith("\r\n"))
                    msg = msg.Replace("\n", "").Replace("\r", "") + "\r\n";
                IPEndPoint serverIep = new IPEndPoint(serverIp, serverPort);
                TcpClient tcpClient = new TcpClient();
                byte[] data = EnDeCodeHelper.GetBytes(msg);
                StreamWriter sw = null;
                StreamReader sr = null;
                NetworkStream netStream = null;
                tcpClient.SendBufferSize = Constants.MAX_BYTE_BUFFEER;
                byte[] outbuf = new byte[bufsze];
                tcpClient.Connect(serverIep);
                if (Constants.FortuneBool)
                {
                    tcpClient.Client.Send(data);
                    int read = tcpClient.Client.Receive(outbuf);
                }
                else
                {
                    netStream = tcpClient.GetStream();
                    sw = new StreamWriter(netStream);
                    sr = new StreamReader(netStream);
                    sw.Write(msg);
                    sw.Flush();
                    sr.BaseStream.Read(outbuf, 0, bufsze);
                }
                resp = System.Text.Encoding.Default.GetString(outbuf);
                //resp = tcpClient.Client.LocalEndPoint?.ToString();
                //if (resp != null && resp.Contains("::ffff:"))
                //{
                //    resp = resp?.Replace("::ffff:", "");
                //    if (resp != null && resp.Contains(':'))
                //    {
                //        int lastch = resp.LastIndexOf(":");
                //        resp = resp.Substring(0, lastch);
                //    }
                //    resp = resp?.Trim("[{()}]".ToCharArray());
                //}
                if (sw != null)
                    sw.Close();
                if (sr != null)
                    sr.Close();
                if (netStream != null)
                    netStream.Close();

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
