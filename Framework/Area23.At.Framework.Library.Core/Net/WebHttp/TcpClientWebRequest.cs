﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Area23.At.Framework.Library.Core.Net.WebHttp
{
    public static class IPSocketSender
    {

        /// <summary>
        /// send
        /// </summary>
        /// <param name="serverIp">server ip address</param>
        /// <param name="serverPort">server port (default 80)</param>
        /// <param name="msg"></param>
        /// <returns>client address as string</returns>
        public static string Send(IPAddress serverIp, int serverPort = 7777, string msg)
        {
            string? resp = string.Empty;
            try
            {
                IPEndPoint serverIep = new IPEndPoint(serverIp, serverPort);
                TcpClient tcpClient = new TcpClient();
                byte[] data = Encoding.UTF8.GetBytes(msg);
                tcpClient.Connect(serverIep);
                // tcpClient.Client.Send(data);
                NetworkStream netStream = tcpClient.GetStream();
                StreamWriter sw = new StreamWriter(netStream);
                // StreamReader sr = new StreamReader(netStream);
                sw.Write(msg);
                sw.Flush();
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
                sw.Close();
                // sr.Close();
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
