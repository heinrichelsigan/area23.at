using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Area23.At.Framework.Core.Util;
using System.Diagnostics.Eventing.Reader;

namespace Area23.At.Framework.Core.Net.IpSocket
{


    /// <summary>
    /// Net.IpSocket.Listener creates a server socket and listen and accept multi threaded connections
    /// </summary>
    public class Listener : IDisposable
    {
        private readonly Lock _lock = new Lock();
        private Thread t;
        internal static bool disposed = false;

        public static string ListenerName { get; protected internal set; }
        public Socket? ServerSocket { get; protected internal set; }
        public IPAddress? ServerAddress { get; protected internal set; }
        public IPEndPoint? ServerEndPoint { get; protected internal set; }
        public Socket? ClientSocket { get; protected internal set; }

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

            //if (ClientSocket != null)
            //{
            //    if (ClientSocket.Connected)
            //        ClientSocket.Disconnect(true);
            //    if (ClientSocket.IsBound)
            //        ClientSocket.Close();
            //    ClientSocket.Dispose();
            //}
            //ClientSocket = null;
            //if (ServerSocket != null)
            //{
            //    if (ServerSocket.Connected)
            //        ServerSocket.Disconnect(true);
            //    if (ServerSocket.IsBound)
            //        ServerSocket.Close();
            //    ServerSocket.Dispose();
            //}
            //ServerSocket = null;            
            Thread.Sleep(200);
            disposed = false;
            ServerAddress = connectedIpIfAddr;
            ServerEndPoint = new IPEndPoint(ServerAddress, Constants.CHAT_PORT);
            ServerSocket = new Socket(ServerAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(ServerEndPoint);
            ServerSocket.Listen(Constants.BACKLOG);
            ListenerName = ServerEndPoint.ToString();

            Area23Log.LogStatic("new Socket created at " + ListenerName);
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
                while (!disposed)
                {
                    lock (_lock)
                    {
                        if (ServerSocket != null && ServerSocket.IsBound)
                        {
                            try
                            {
                                ClientSocket = ServerSocket.Accept();
                            }
                            catch (Exception exSock)
                            {
                                Area23Log.LogStatic(exSock, ListenerName);
                            }
                        }                        
                    }

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
                    Span<byte> buf = new Span<byte>(buffer, 0, Constants.MAX_BYTE_BUFFEER);
                    IPEndPoint clientIEP = (IPEndPoint?)ClientSocket.RemoteEndPoint;
                    string sstring = "Accept connection from " + clientIEP?.Address.ToString() + ":" + clientIEP?.Port.ToString() +
                        " => " + ServerAddress?.ToString() + ":" + ServerEndPoint?.ToString();
                    Area23Log.Logger.LogInfo(sstring);

                    ClientSocket.ReceiveBufferSize = Constants.MAX_BYTE_BUFFEER;
                    SocketFlags flags = SocketFlags.None;
                    SocketError errorCode;
                    int rsize = ClientSocket.Receive(buf, flags, out errorCode);
                    // int rsize = ClientSocket.Receive(buffer, 0, Constants.MAX_BYTE_BUFFEER, flags, out errorCode);
                    BufferedData = new byte[rsize];

                    Array.Copy(buf.ToArray(), BufferedData, rsize);

                    ReceiveData receiveData = new ReceiveData(buf.ToArray(), rsize, clientIEP?.Address.ToString(), clientIEP?.Port);

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
            ((ServerEndPoint?.AddressFamily == AddressFamily.InterNetworkV6) ? "ip6 " : "ip4 ") +
            ServerAddress?.ToString() + ":" + ServerEndPoint?.Port + " " + ServerSocket?.SocketType.ToString();


        public virtual string AcceptToString() => "New connection from " + ClientSocket?.RemoteEndPoint?.ToString();

        public void Dispose()
        {
            disposed = true;
            if (ServerSocket != null)
            {
                if (ClientSocket != null)
                {
                    if (ClientSocket.Connected)
                        ClientSocket.Disconnect(true);
                    if (ClientSocket.IsBound)
                        ClientSocket.Close(Constants.CLOSING_TIMEOUT);
                }
                if (ServerSocket.Connected)
                    ServerSocket.Disconnect(true);

                if (ServerSocket.IsBound)
                    ServerSocket.Close(Constants.CLOSING_TIMEOUT);

            }
            if (ClientSocket != null)
                ClientSocket.Dispose();
            if (ServerSocket != null)
                ServerSocket.Dispose();
            
            ListenerName = "";
            ServerEndPoint = null; ClientSocket = null; ServerSocket = null; ServerAddress = null;

        }

        //~Listener()
        //{
        //    this.Dispose();

        //    ClientSocket = null;
        //    ServerSocket = null;
        //    ServerEndPoint = null;
        //    ServerAddress = null;
        //}


    }


}
