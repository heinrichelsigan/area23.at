using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Area23.At.Framework.Library.Core.Util;

namespace Area23.At.Framework.Library.Core.Net.IpSocket
{
    public class IPSockListener : IDisposable
    {
        private readonly object _lock = new object();
        private Thread t;

        public Socket? ServerSocket { get; protected internal set; }
        public IPAddress? ServerAddress { get; protected internal set; }
        public IPEndPoint? ServerEndPoint { get; protected internal set; }
        public Socket? ClientSocket { get; protected internal set; }

        public byte[] BufferedData { get; protected internal set; } = new byte[65535];


        public EventHandler<Area23EventArgs<byte[]>> EventHandlerClientRequest { get; protected internal set; }

        protected internal EventHandler AcceptClientConnection { get; set; }

        /// <summary>
        /// constructs a listening at <see cref="ServerAddress"/> via <see cref="ServerEndPoint"/> bound <see cref="ServerSocket"/>
        /// </summary>
        /// <param name="connectedIpIfAddr"><see cref="ServerAddress"/></param>
        /// <exception cref="InvalidOperationException"></exception>
        public IPSockListener(IPAddress connectedIpIfAddr)
        {
            if (connectedIpIfAddr.AddressFamily != AddressFamily.InterNetwork && connectedIpIfAddr.AddressFamily != AddressFamily.InterNetworkV6)
                throw new InvalidOperationException("We can only handle AddressFamily == AddressFamily.InterNetwork and AddressFamily.InterNetworkV6");
            
            ServerAddress = connectedIpIfAddr; 
            ServerEndPoint = new IPEndPoint(ServerAddress, Constants.CHAT_PORT);
            ServerSocket = new Socket(ServerAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(ServerEndPoint);
            ServerSocket.Listen(Constants.BACKLOG);

            Task task = new Task(() => OnAcceptClientConnection("ctor", new EventArgs()));
            task.Start();
        }

        public IPSockListener(IPAddress connectedIpIfAddr, EventHandler<Area23EventArgs<byte[]>> evClReq) : this(connectedIpIfAddr)
        {
            EventHandlerClientRequest = evClReq;
        }


        protected internal void OnAcceptClientConnection(object sender, EventArgs e)
        {
            if (ServerSocket != null && ServerSocket.IsBound)
            {
                Console.WriteLine(ListenToString());
                while (true)
                {
                    ClientSocket = ServerSocket.Accept();
                    IPEndPoint clientIEP = (IPEndPoint?)ClientSocket.RemoteEndPoint;
                    
                    string sstring = "Accept connection from " + clientIEP?.Address.ToString() + ":" + clientIEP?.Port.ToString() +
                        " => " + ServerAddress?.ToString() + ":" + ServerEndPoint?.ToString();
                    Area23Log.Logger.LogInfo(sstring);
                    // Task task = new Task(() => HandleClientRequest(sender, e));
                    // task.Start();
                    t = new Thread(new ThreadStart(() => HandleClientRequest(sender, e)));
                    t.Start();
                    Thread.Sleep(256);
                }
            }            
        }

        /// <summary>
        /// HandleClientRequest - handles client request
        /// </summary>
        protected internal void HandleClientRequest(object sender, EventArgs e)
        {
            if (ClientSocket != null)
            {
                lock (_lock)
                {
                    byte[] buffer = new byte[65536];
                    int rsize = ClientSocket.Receive(buffer, 0, 65536, 0);
                    Array.Copy(buffer, BufferedData, rsize);

                    byte[] sendData = new byte[8];
                    sendData = Encoding.Default.GetBytes("ACK\r\n\0");
                    ClientSocket.Send(sendData);
                    ClientSocket.Close();

                    if (EventHandlerClientRequest != null)
                    {
                        EventHandler<Area23EventArgs<byte[]>> handler = EventHandlerClientRequest;
                        Area23EventArgs<byte[]> area23EventArgs = new Area23EventArgs<byte[]>(BufferedData);
                        handler?.Invoke(this, area23EventArgs);
                    }
                }
            }
        }


        public virtual string ListenToString() => "Listening " +
            ((ServerEndPoint?.AddressFamily == AddressFamily.InterNetworkV6) ? "ip6 " : "ip4 ") +
            ServerAddress?.ToString() + ":" + ServerEndPoint?.Port + " " + ServerSocket?.SocketType.ToString();


        public virtual string AcceptToString() => "New connection from " + ClientSocket?.RemoteEndPoint?.ToString();

        public void Dispose()
        {
            if (ServerSocket != null && ServerSocket.IsBound)
            {
                if (ClientSocket != null && ClientSocket.Connected && ClientSocket.IsBound)
                {
                    ClientSocket.Disconnect(false);
                    ClientSocket.Close(Constants.CLOSING_TIMEOUT);
                }
                if (ServerSocket.Connected)
                    ServerSocket.Disconnect(false);

                ServerSocket.Close(Constants.CLOSING_TIMEOUT);
            }
            if (ClientSocket != null)
                ClientSocket.Dispose();
            if (ServerSocket != null)
                ServerSocket.Dispose();
        }

        ~IPSockListener()
        {
            this.Dispose();

            ClientSocket = null;
            ServerSocket = null;
            ServerEndPoint = null;
            ServerAddress = null;
        }            


    };
}
