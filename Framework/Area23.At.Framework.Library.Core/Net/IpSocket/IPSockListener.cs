using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Net.IpSocket
{
    internal class IPSockListener
    {

        internal Socket? ServerSocket { get; private set; }
        internal IPAddress? ServerAddress { get; private set; }
        internal IPEndPoint? ServerEndPoint { get; private set; }
        internal Socket? ClientSocket { get; set; }
        
        internal EventHandler HandleClientRequest { get; set; }

        internal EventHandler AcceptClientConnection { get; set; }

        /// <summary>
        /// constructs a listening at <see cref="ServerAddress"/> via <see cref="ServerEndPoint"/> bound <see cref="ServerSocket"/>
        /// </summary>
        /// <param name="connectedIpIfAddr"><see cref="ServerAddress"/></param>
        /// <exception cref="InvalidOperationException"></exception>
        public IPSockListener(IPAddress connectedIpIfAddr)
        {
            if (connectedIpIfAddr.AddressFamily == AddressFamily.InterNetwork || connectedIpIfAddr.AddressFamily == AddressFamily.InterNetworkV6)
                throw new InvalidOperationException("We can only handle AddressFamily == AddressFamily.InterNetwork and AddressFamily.InterNetworkV6");
            
            ServerAddress = connectedIpIfAddr; 
            ServerEndPoint = new IPEndPoint(ServerAddress, Constants.CHAT_PORT);
            ServerSocket = new Socket(ServerAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(ServerEndPoint);
            ServerSocket.Listen(Constants.BACKLOG);
            EventArgs evArgs = new EventArgs();
            this.AcceptClientConnection += new EventHandler(OnAcceptClientConnection);
        }

        public void OnAcceptClientConnection(object sender, EventArgs e)
        {
            if (ServerSocket != null && ServerSocket.IsBound)
            {
                Console.WriteLine(ListenToString());
                while (true)
                {
                    ClientSocket = ServerSocket.Accept();
                    // Console.WriteLine(;
                }
            }            
        }

        public virtual string ListenToString() => "Listening " +
            ((ServerEndPoint?.AddressFamily == AddressFamily.InterNetworkV6) ? "ip6 " : "ip4 ") +
            ServerAddress?.ToString() + ":" + ServerEndPoint?.Port + " " + ServerSocket?.SocketType.ToString();


        public virtual string AcceptToString() => "New connection from " + ClientSocket?.RemoteEndPoint?.ToString();

        ~IPSockListener()
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
            ClientSocket = null;
            if (ServerSocket != null)
                ServerSocket.Dispose();
            ServerSocket = null;

            ServerEndPoint = null;
            ServerAddress = null;
        }            


    };
}
