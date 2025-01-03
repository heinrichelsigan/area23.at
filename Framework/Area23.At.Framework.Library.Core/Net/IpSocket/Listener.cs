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

        internal EventHandler HandleReceive { get; set; }

        public byte[] receiveData = new byte[65536];

        public Listener()
        {
            List<IPAddress> addrs = NetworkAddresses.GetConnectedIpAddresses();
            InitServers(addrs.ToArray());
        }

        public Listener(EventHandler handler)
        {
            List<IPAddress> addrs = NetworkAddresses.GetConnectedIpAddresses();
            HandleReceive = handler;
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
                ipSockListener.HandleClientRequest += new EventHandler(HandleClientRequest);
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
        public void HandleClientRequest(object sender, EventArgs e)
        {
            if (sender != null && sender is IPSockListener ipsl)
            {
                if (ipsl.ClientSocket != null)
                {
                    IPEndPoint clientIEP = (IPEndPoint?)ipsl.ClientSocket.RemoteEndPoint;
                    receiveData = new byte[65536];
                    int rsize = ipsl.ClientSocket.Receive(receiveData, 0, 65536, 0);
                    Array.Copy(receiveData, data, rsize);
                    string rstring = Encoding.Default.GetString(data, 0, rsize);
                    Console.WriteLine(rstring);
                    string sstring = ipsl.ServerAddress?.ToString() + " => " + clientIEP?.Address.ToString() + " : " + rstring;
                    byte[] sendData = new byte[65536];
                    sendData = Encoding.Default.GetBytes(sstring);
                    ipsl.ClientSocket.Send(sendData);
                    ipsl.ClientSocket.Close();
                    Console.WriteLine("Closing socket.");
                    EventHandler handler = HandleReceive;
                    EventArgs eventArgs = new EventArgs();
                    handler?.Invoke(this, eventArgs);
                }
            }
        }



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
