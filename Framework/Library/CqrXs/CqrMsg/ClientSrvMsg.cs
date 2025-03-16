using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.CqrXs.CqrMsg
{
    public class ClientSrvMsg<TS, TC>
        where TS : class
        where TC : class
    {

        public ClientSrvMsg()
        {
            ServerMsgString = string.Empty;
            ClientMsgString = string.Empty;
        }

        public ClientSrvMsg(FullSrvMsg<TS> serverMsg, FullSrvMsg<TC> clientMsg, string serverMsgString, string clientMsgString)
        {
            ServerMsg = serverMsg;
            ClientMsg = clientMsg;
            ServerMsgString = serverMsgString;
            ClientMsgString = clientMsgString;
        }

        public ClientSrvMsg(string serverMsgString, string clientMsgString)
        {
            ServerMsgString = serverMsgString;
            ClientMsgString = clientMsgString;
        }


        public FullSrvMsg<TS> ServerMsg { get; set; }
        public FullSrvMsg<TC> ClientMsg { get; set; }

        string ServerMsgString { get; set; }

        string ClientMsgString { get; set; }


    }

}
