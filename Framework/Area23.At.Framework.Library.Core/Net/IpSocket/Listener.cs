using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Bcpg;

namespace Area23.At.Framework.Library.Core.Net.IpSocket
{
    /// <summary>
    /// Listener is ipv4 or ipv6 server socket IpSocket
    /// </summary>
    public class Listener
    {
         
        private static HashSet<IPSockListener> _listeners = new HashSet<IPSockListener>();
        // public static IPSockListener[] IpListeners { get => _listeners.ToArray(); }

        private byte[] data = new byte[8192];
        Thread t;

        public Listener()
        {
            List<IPAddress> addrs = NetworkAddresses.GetConnectedIpAddresses();
            InitServers(addrs.ToArray());
        }


        /// <summary>
        /// Constructor for Listeneer6
        /// </summary>
        /// <param name="address">ip address to listen on</param>
        public Listener(IPAddress[] address)
        {
            InitServers(address); 
        }


        public void InitServers(IPAddress[] address)
        {
            foreach (IPAddress addr in address)
            {
                IPSockListener ipSockListener = new IPSockListener(addr);
                // ipSockListener.AcceptClientConnection += new EventHandler(On)
                _listeners.Add(ipSockListener);
            }
        }


        /// <summary>
        /// RunServer - runs server oo serverSocket
        /// </summary>
        public void RunServers()
        {
            if (_listeners != null && _listeners.Count > 0) 
            {
                foreach (IPSockListener ipsl in _listeners)
                {
                    Console.WriteLine(ipsl.ToString());
                    
                    while (true)
                    {
                        ipsl.ClientSocket = ipsl.ServerSocket.Accept();
                        Console.WriteLine("New connection from " + ipsl.ClientSocket.RemoteEndPoint?.ToString());
                        // t = new Thread(new ThreadStart(HandleClientRequest));
                        // t.Start();
                        // Thread.Sleep(500);
                    }
                }
            }
        }


        /// <summary>
        /// HandleClientRequest - handles client request
        /// </summary>
        //public void HandleClientRequest()
        //{

        //    if (clientSocket != null)
        //    {
        //        clientIEP = (IPEndPoint?)clientSocket.RemoteEndPoint;
        //        byte[] receiveData = new byte[8192];
        //        int rsize = clientSocket.Receive(receiveData, 0, 8192, 0);
        //        Array.Copy(receiveData, data, rsize);
        //        string rstring = Encoding.Default.GetString(data, 0, rsize);
        //        Console.WriteLine(rstring);
        //        string sstring = serverAddress?.ToString() + " => " + clientIEP?.Address.ToString() + " : " + rstring;
        //        byte[] sendData = new byte[8192];
        //        sendData = Encoding.Default.GetBytes(sstring);
        //        clientSocket.Send(sendData);
        //        clientSocket.Close();
        //        Console.WriteLine("Closing socket.");
        //    }
        //}



        /// <summary>
        /// GetTcpServer gets a server enpoint and a server socket
        /// </summary>
        /// <param name="address">ip address to listen on</param>
        /// <returns><see cref="Socket">socket</see> where server listens</returns>
        public static Socket GetTcpServer(IPAddress address)
        {
            IPEndPoint ipEo = new IPEndPoint(address, Constants.CHAT_PORT);
            Socket socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            socket.Bind(ipEo);
            socket.Listen(Constants.BACKLOG);

            return socket;

        }

    }

}
