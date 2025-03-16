using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Area23.At.Framework.Core.Util;
using System.Diagnostics.Eventing.Reader;
using Area23.At.Framework.Core.Static;
using System.Windows.Interop;

namespace Area23.At.Framework.Core.Net.IpSocket
{


    /// <summary>
    /// Net.IpSocket.Listener creates a server socket and listen and accept multi threaded connections
    /// When using <see cref="SockTcpListener"/> as server, you must use <see cref="SockTcpSender"/> as client,
    /// when using <see cref="Listener"/> as server, you should use <see cref="Sender"/> as client.
    /// </summary>
    public class Listener : IDisposable
    {
        private readonly Lock _lock = new Lock();
        private Thread t;
        internal static bool disposed = false;

        public static string ListenerName { get; protected internal set; }
        public Socket? ServerSocket { get; protected internal set; }
        // public TcpListener ServerTcpListener { get; protected internal set; }   
        public IPAddress? ServerAddress { get; protected internal set; }
        public IPEndPoint? ServerEndPoint { get; protected internal set; }
        public Socket? ClientSocket { get; protected internal set; }
        // public TcpClient TcpClientSocket {  get; protected internal set; }  

        public byte[] BufferedData { get; protected internal set; } = new byte[Constants.MAX_SOCKET_BYTE_BUFFEER];



        public EventHandler<Area23EventArgs<ReceiveData>> EventHandlerClientRequest { get; internal set; }

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

            // ServerTcpListener = new TcpListener(ServerEndPoint);
            // ServerTcpListener.Start();

            disposed = false;
            ServerAddress = connectedIpIfAddr;
            ServerEndPoint = new IPEndPoint(ServerAddress, Constants.CHAT_PORT);
            
            ServerSocket = new Socket(ServerAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.ReceiveBufferSize = Constants.MAX_SOCKET_BYTE_BUFFEER;
            // ServerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
            ServerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            ServerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, Constants.MAX_SOCKET_BYTE_BUFFEER);
            // ServerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, Constants.MAX_SOCKET_BYTE_BUFFEER);
            // ServerSocket.NoDelay = true;
            ServerSocket.Bind(ServerEndPoint);
            ServerSocket.Listen(Constants.BACKLOG);
            ListenerName = ServerEndPoint.ToString();

            SLog.Log("new Socket created at " + ListenerName);            
        }

        public Listener(IPAddress connectedIpIfAddr, EventHandler<Area23EventArgs<ReceiveData>> evClReq) : this(connectedIpIfAddr)
        {            
            Thread.Sleep(100);
            EventHandlerClientRequest = evClReq;
            Thread.Sleep(100);
            Task task = new Task(() => OnAcceptClientConnection("ctor", new EventArgs()));
            task.Start();
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
                                SLog.Log(exSock, ListenerName);
                            }

                            // Task task = new Task(() => HandleClientRequest(sender, e));
                            // task.Start();       
                            t = new Thread(new ThreadStart(() => HandleClientRequest(sender, e)));
                            t.Start();
                            Thread.Sleep(256);
                        }                        
                    }                    
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
                    byte[] buffer = new byte[Constants.MAX_SOCKET_BYTE_BUFFEER];
                    // char[] charbuf = new char[Constants.MAX_SOCKET_BYTE_BUFFEER];
                    Span<byte> buf = new Span<byte>(buffer, 0, Constants.MAX_SOCKET_BYTE_BUFFEER);
                    IPEndPoint clientIEP = (IPEndPoint?)ClientSocket.RemoteEndPoint;
                    string sstring = "Accept connection from " + clientIEP?.Address.ToString() + ":" + clientIEP?.Port.ToString() +
                        " => " + ServerAddress?.ToString() + ":" + ServerEndPoint?.ToString();
                    Area23Log.Logger.LogInfo(sstring);

                    ClientSocket.ReceiveBufferSize = Constants.MAX_SOCKET_BYTE_BUFFEER;
                    ClientSocket.SendBufferSize = Constants.MAX_SOCKET_BYTE_BUFFEER;
                    ClientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    // ClientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
                    ClientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, Constants.MAX_SOCKET_BYTE_BUFFEER);
                    ClientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, Constants.MAX_SOCKET_BYTE_BUFFEER);
                    SocketFlags flags = SocketFlags.None;
                    SocketError errorCode;
                    ClientSocket.ReceiveTimeout = 16000;
                    // ClientSocket.NoDelay = true;
                    //int rsize = -1;
                    //using (NetworkStream netStream = new NetworkStream(ClientSocket))
                    //{
                    //    using (StreamReader sr = new StreamReader(netStream))
                    //    {
                    //        rsize = sr.BaseStream.Read(buffer, 0, buffer.Length);
                    //    }
                    //    using (StreamWriter sw = new StreamWriter(netStream))
                    //    {
                    //        sw.Write($"ACK {clientIEP?.Address.ToString()}:{clientIEP?.Port} => {ServerAddress.ToString()}:{Constants.CHAT_PORT}!");
                    //        sw.Flush();
                    //    }
                    //}
                    // long rsize = (long)ClientSocket.Receive(buf, flags, out errorCode);
                    int rsize = ClientSocket.Receive(buffer, 0, Constants.MAX_SOCKET_BYTE_BUFFEER, flags, out errorCode);

                    // int rsize = ClientSocket.Receive(buffer, 0, Constants.MAX_BYTE_BUFFEER, flags, out errorCode);
                    BufferedData = new byte[rsize];

                    Array.Copy(buffer, 0, BufferedData, 0, rsize);

                    // ReceiveData receiveData = new ReceiveData(buf.ToArray(), (int)rsize, clientIEP?.Address.ToString(), clientIEP?.Port);
                    ReceiveData receiveData = new ReceiveData(buffer, (int)rsize, clientIEP?.Address.ToString(), clientIEP?.Port);

                    byte[] sendData = new byte[32];
                    sendData = Encoding.Default.GetBytes(rsize.ToString());
                    ClientSocket.Send(sendData, SocketFlags.None); 

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

                if (ServerSocket != null)
                {
                    if (ClientSocket != null)
                    {
                        try
                        {
                            if (ClientSocket.Connected)
                                ClientSocket.Disconnect(true);
                        }
                        catch (Exception exSockDisconnect)
                        {
                            SLog.Log(exSockDisconnect);
                        }
                        try
                        {
                            if (ClientSocket.IsBound)
                                ClientSocket.Close(Constants.CLOSING_TIMEOUT);
                        }
                        catch (Exception exSockClose)
                        {
                            SLog.Log(exSockClose);
                        }
                    }
                    try
                    {
                        if (ServerSocket.Connected)
                            ServerSocket.Disconnect(true);
                    }
                    catch (Exception exSrvSockDisconnect)
                    {
                        SLog.Log(exSrvSockDisconnect);
                    }
                    try
                    {
                        if (ServerSocket.IsBound)
                            ServerSocket.Close(Constants.CLOSING_TIMEOUT);
                    }
                    catch (Exception exSrvSockClose)
                    {
                        SLog.Log(exSrvSockClose);
                    }
                }

 
            disposed = true;

            try
            {
                if (ServerSocket != null)
                    ServerSocket.Dispose();
            }
            catch (Exception exClientSockDispose)
            {
                SLog.Log(exClientSockDispose);
            }
            try
            {
                if (ClientSocket != null)
                    ClientSocket.Dispose();
            }
            catch (Exception exSrvSockDispose)
            {
                SLog.Log(exSrvSockDispose);
            }

            try { EventHandlerClientRequest = null; }
            catch (Exception exEventHandlerNull) { SLog.Log(exEventHandlerNull); }
            try { ListenerName = ""; ServerEndPoint = null; }
            catch (Exception exSockNull) { SLog.Log(exSockNull); }

            try { ClientSocket = null; }
            catch (Exception exSockNull) { SLog.Log(exSockNull); }

            try { ServerSocket = null; } catch (Exception exSockNull) { SLog.Log(exSockNull); }

            try { ServerAddress = null; }
            catch (Exception exSrvAddr) { SLog.Log(exSrvAddr); }

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
