using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Net.IpSocket
{


    /// <summary>
    /// Net.IpSocket.Listener creates a server socket and listen and accept multi threaded connections
    /// </summary>
    public class Listener : IDisposable
    {
        private readonly object _lock = new object();
        private Thread t;

        public Socket ServerSocket { get; protected internal set; }
        public IPAddress ServerAddress { get; protected internal set; }
        public IPEndPoint ServerEndPoint { get; protected internal set; }
        public Socket ClientSocket { get; protected internal set; }

        public byte[] BufferedData { get; protected internal set; } = new byte[Constants.MAX_BYTE_BUFFEER];



        public EventHandler<Area23EventArgs<ReceiveData>> EventHandlerClientRequest { get; protected internal set; }

        protected internal EventHandler AcceptClientConnection { get; set; }

        /// <summary>
        /// constructs a listening at <see cref="ServerAddress"/> via <see cref="ServerEndPoint"/> bound <see cref="ServerSocket"/>
        /// </summary>
        /// <param name="connectedIpIfAddr"><see cref="ServerAddress"/></param>
        /// <exception cref="InvalidOperationException"></exception>
        public Listener(IPAddress connectedIpIfAddr)
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

        public Listener(IPAddress connectedIpIfAddr, EventHandler<Area23EventArgs<ReceiveData>> evClReq) : this(connectedIpIfAddr)
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
                    byte[] buffer = new byte[Constants.MAX_BYTE_BUFFEER];
                    
                    IPEndPoint clientIEP = (IPEndPoint)ClientSocket.RemoteEndPoint;
                    string sstring = "Accept connection from " + clientIEP.Address.ToString() + ":" + clientIEP.Port.ToString() +
                        " => " + ServerAddress.ToString() + ":" + ServerEndPoint.ToString();
                    Area23Log.Log(sstring);

                    ClientSocket.ReceiveBufferSize = Constants.MAX_BYTE_BUFFEER;
                    SocketFlags flags = SocketFlags.None;
                    SocketError errorCode;
                    // int rsize = ClientSocket.Receive(buffer, flags, out errorCode);
                    int rsize = ClientSocket.Receive(buffer, 0, Constants.MAX_BYTE_BUFFEER, flags, out errorCode);
                    BufferedData = new byte[rsize];

                    Array.Copy(buffer, BufferedData, rsize);

                    ReceiveData receiveData = new ReceiveData(buffer, rsize, clientIEP.Address.ToString(), clientIEP.Port);

                    // byte[] sendData = new byte[8];
                    // sendData = Encoding.Default.GetBytes("ACK\r\n\0");
                    // ClientSocket.Send(sendData, SocketFlags.None);

                    ClientSocket.Close();

                    if (EventHandlerClientRequest != null)
                    {
                        EventHandler<Area23EventArgs<ReceiveData>> handler = EventHandlerClientRequest;
                        Area23EventArgs<ReceiveData> area23EventArgs = new Area23EventArgs<ReceiveData>(receiveData);
                        handler?.Invoke(this, area23EventArgs);
                    }


                }
            }
        }


        public virtual string ListenToString() => "Listening " +
            ServerEndPoint?.AddressFamily.ShortInfo() + 
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

        ~Listener()
        {
            this.Dispose();

            ClientSocket = null;
            ServerSocket = null;
            ServerEndPoint = null;
            ServerAddress = null;
        }


    }

}
