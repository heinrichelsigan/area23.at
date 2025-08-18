using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using System.Net;
using System.Net.Sockets;

namespace Area23.At.Framework.Core.Net.IpSocket
{


    /// <summary>
    /// Net.IpSocket.SockTcpListener creates a server socket and listen and accept multi threaded connections
    /// When using <see cref="SockTcpListener"/> as server, you must use <see cref="SockTcpSender"/> as client,
    /// when using <see cref="Listener"/> as server, you should use <see cref="Sender"/> as client.
    /// </summary>
    public class SockTcpListener : IDisposable
    {
        private readonly Lock _lock = new Lock();
        private Thread t;
        internal static bool disposed = false;

        public static string ListenerName { get; protected internal set; }

        public TcpListener? ServerTcpListener { get; protected internal set; }        
        public Socket? ServerSocket  { get; protected internal set; }   
        public IPAddress? ServerAddress { get; protected internal set; }
        public IPEndPoint? ServerEndPoint { get; protected internal set; }
        public Socket? ClientSocket { get; protected internal set; }
        public TcpClient? ClientTcpClient {  get; protected internal set; }  

        public byte[] BufferedData { get; protected internal set; } = new byte[Constants.MAX_SOCKET_BYTE_BUFFEER];



        public EventHandler<Area23EventArgs<ReceiveData>> EventHandlerClientRequest { get; internal set; }

        protected internal EventHandler AcceptClientConnection { get; set; }

        /// <summary>
        /// constructs a listening at <see cref="ServerAddress"/> via <see cref="ServerEndPoint"/> bound <see cref="ServerSocket"/>
        /// </summary>
        /// <param name="connectedIpIfAddr"><see cref="ServerAddress"/></param>
        /// <exception cref="InvalidOperationException"></exception>
        public SockTcpListener(IPAddress connectedIpIfAddr)
        {
            if (connectedIpIfAddr.AddressFamily != AddressFamily.InterNetwork && connectedIpIfAddr.AddressFamily != AddressFamily.InterNetworkV6)
                throw new InvalidOperationException("We can only handle AddressFamily == AddressFamily.InterNetwork and AddressFamily.InterNetworkV6");

            // ClientSocket = null;
            // ServerSocket = null;            

            disposed = false;
            ServerAddress = connectedIpIfAddr;
            ServerEndPoint = new IPEndPoint(ServerAddress, Constants.CHAT_PORT);
            ServerTcpListener = new TcpListener(ServerEndPoint);
            ListenerName = ServerEndPoint.ToString();            
            ServerTcpListener.Start();

            Area23Log.LogOriginMsg("SockTcpListener", "new Socket created at " + ListenerName);            
        }

        public SockTcpListener(IPAddress connectedIpIfAddr, EventHandler<Area23EventArgs<ReceiveData>> evClReq) : this(connectedIpIfAddr)
        {
            EventHandlerClientRequest = evClReq;
            Thread.Sleep(100);
            Task task = new Task(() => OnAcceptClientConnection("ctor", new EventArgs()));
            task.Start();
        }


        protected internal void OnAcceptClientConnection(object sender, EventArgs e)
        {
            while (ServerTcpListener != null  && !disposed)
            {
                ServerSocket = ServerTcpListener.Server;
                ServerTcpListener.Server.ReceiveTimeout = 16000;
                ServerTcpListener.Server.ReceiveBufferSize = Constants.MAX_SOCKET_BYTE_BUFFEER;
                Console.WriteLine(ListenToString());
                string tcpServerSettings = "address/port: " + ServerEndPoint.ToString() + " , ServerTcpListener.Server.SocketType = " + ServerTcpListener.Server.SocketType + ",\n" +
                    " ServerTcpListener.Server.SendBufferSize = " + ServerTcpListener.Server.SendBufferSize + ", ServerTcpListener.Server.SendTimeOut = " + ServerTcpListener.Server.SendTimeout + ",\n" +
                    " ServerTcpListener.Server.ReceiveBufferSize = " + ServerTcpListener.Server.ReceiveBufferSize + ", ServerTcpListener.Server.ReceiveTimeout = " + ServerTcpListener.Server.ReceiveTimeout + ",\n" +
                    " ServerTcpListener.Server.Ttl = " + ServerTcpListener.Server.Ttl + ", ServerTcpListener.Server.NoDelay = " + ServerTcpListener.Server.NoDelay + ",\n" +
                    " ServerTcpListener.Server.Blocking = " + ServerTcpListener.Server.Blocking + ";\n";
                Area23Log.LogOriginMsg("SockTcpListener", "Server: " + tcpServerSettings);

                if (ServerTcpListener.Pending())
                {
                    lock (_lock)
                    {
                        try
                        {
                            ClientTcpClient = ServerTcpListener.AcceptTcpClient();
                            ClientTcpClient.ReceiveTimeout = 16000;
                            ClientTcpClient.ReceiveBufferSize = Constants.MAX_SOCKET_BYTE_BUFFEER;
                            ClientSocket = ClientTcpClient.Client;                            
                        }
                        catch (Exception exSock)
                        {
                            Area23Log.LogOriginMsgEx("SockTcpListener", "OnAcceptClientConnection", exSock);
                            continue;
                        }
                    }
                    if (ClientTcpClient != null && ClientSocket != null && ClientSocket.Connected && ServerSocket.IsBound)
                    {
                        // Task task = new Task(() => HandleClientRequest(sender, e));
                        // task.Start();       
                        t = new Thread(new ThreadStart(() => HandleClientRequest(sender, e)));
                        t.Start();
                    }
                }
                Thread.Sleep(125);
            }
        }

        /// <summary>
        /// HandleClientRequest - handles client request
        /// </summary>
        protected internal void HandleClientRequest(object sender, EventArgs e)
        {
            if (ClientTcpClient != null && ClientSocket != null)
            {
                byte[] buffer = new byte[Constants.MAX_SOCKET_BYTE_BUFFEER];
                // char[] charbuf = new char[Constants.MAX_SOCKET_BYTE_BUFFEER];
                IPEndPoint clientIEP = (IPEndPoint?)ClientSocket.RemoteEndPoint;

                string tcpClientSettings = "address/port: " + clientIEP.ToString() + " , ClientTcpClient.Client.SocketType = " + ClientTcpClient.Client.SocketType + ",\n" +
                    ", ClientTcpClient SendBufferSize    = " + ClientTcpClient.SendBufferSize + ", ClientTcpClient.SendTimeOut = " + ClientTcpClient.SendTimeout + ",\n" +
                    " ClientTcpClient.ReceiveBufferSize = " + ClientTcpClient.ReceiveBufferSize + ", ClientTcpClient.ReceiveTimeout = " + ClientTcpClient.ReceiveTimeout + ",\n" +
                    " ClientTcpClient.Client.SendBufferSize = " + ClientTcpClient.Client.SendBufferSize + ", ClientTcpClient.Client.SendTimeOut = " + ClientTcpClient.Client.SendTimeout + ",\n" +
                    " ClientTcpClient.Client.ReceiveBufferSize = " + ClientTcpClient.Client.ReceiveBufferSize + ", ClientTcpClient.Client.ReceiveTimeout = " + ClientTcpClient.Client.ReceiveTimeout + ",\n" +
                    " ClientTcpClient.Client.Ttl = " + ClientTcpClient.Client.Ttl + ", ClientTcpClient.Client.NoDelay = " + ClientTcpClient.Client.NoDelay + ",\n" +
                    " ClientTcpClient.Client.Blocking = " + ClientTcpClient.Client.Blocking + ";\n";    
                string sstring = "Accept connection from " + clientIEP?.Address.ToString() + ":" + clientIEP?.Port.ToString() + " => " + ServerAddress?.ToString() + ":" + ServerEndPoint?.ToString();
                Area23Log.LogOriginMsg("SockTcpListener", sstring);
                Area23Log.LogOriginMsg("SockTcpListener", "Client: " + tcpClientSettings);

                lock (_lock)
                {
                    
                    int rsize = -1;
                    using (NetworkStream netStream = ClientTcpClient.GetStream())
                    {
                        netStream.ReadTimeout = 6000;
                        netStream.WriteTimeout = 6000;
                        rsize = netStream.Read(buffer, 0, buffer.Length);
                        if (netStream.CanWrite)
                        {
                            byte[] outbytes = EnDeCodeHelper.GetBytes(rsize.ToString());
                            netStream.Write(outbytes, 0, outbytes.Length);
                            netStream.Flush();
                        }
                     }                       
                                        
                    
                    BufferedData = new byte[rsize];
                    Array.Copy(buffer, 0, BufferedData, 0, rsize);
                    ReceiveData receiveData = new ReceiveData(buffer, (int)rsize, clientIEP?.Address.ToString(), clientIEP?.Port);


                    ClientTcpClient.Close();

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

            if (ServerTcpListener != null)
            {
                if (ClientTcpClient != null)
                {
                    try
                    {
                        ClientTcpClient.Close();
                    }
                    catch (Exception exClientClose)
                    {
                        Area23Log.LogOriginMsgEx("SockTcpListener", "Dispose", exClientClose);
                    }
                    try
                    {
                        ClientTcpClient.Dispose();
                    }
                    catch (Exception exClientDispose)
                    {
                        Area23Log.LogOriginMsgEx("SockTcpListener", "Dispose", exClientDispose);
                    }
                }

                try
                {
                    ServerTcpListener.Stop();
                }
                catch (Exception exSrvListenStop)
                {
                    Area23Log.LogOriginMsgEx("SockTcpListener", "Dispose", exSrvListenStop);
                }
                try
                {
                    ServerTcpListener.Dispose();
                }
                catch (Exception exSrvListenerDispose)
                {
                    Area23Log.LogOriginMsgEx("SockTcpListener", "Dispose", exSrvListenerDispose);
                }
            }
 
            disposed = true;

            try
            {
                ClientTcpClient = null;
            }
            catch (Exception exTcpClientNull)
            {
                Area23Log.LogOriginMsgEx("SockTcpListener", "Dispose", exTcpClientNull);                
            }
            try
            {
                ServerTcpListener = null;
            }
            catch (Exception exSrvListenerNull)
            {
                Area23Log.LogOriginMsgEx("SockTcpListener", "Dispose", exSrvListenerNull);
            }

            try
            {
                EventHandlerClientRequest = null;
            }
            catch (Exception exEventHandlerNull)
            {
                Area23Log.LogOriginMsgEx("SockTcpListener", "Dispose", exEventHandlerNull);
            }
            try { ListenerName = ""; ServerEndPoint = null; }
            catch (Exception exSockNull) { Area23Log.LogOriginMsgEx("SockTcpListener", "Dispose", exSockNull); }
            try { ClientSocket = null; }
            catch (Exception exSockNull) { Area23Log.LogOriginMsgEx("SockTcpListener", "Dispose", exSockNull); }
            try { ServerSocket = null; } catch (Exception exSockNull) { Area23Log.LogOriginMsgEx("SockTcpListener", "Dispose", exSockNull); }
            try { ServerAddress = null; }
            catch (Exception exSrvAddr) { Area23Log.LogOriginMsgEx("SockTcpListener", "Dispose", exSrvAddr); }

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
