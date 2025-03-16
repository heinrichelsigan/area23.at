using Newtonsoft.Json;

namespace Area23.At.Framework.Core.CqrXs.Msg
{

    public class ClientSrvMsg<TS, TC> : CqrMsg, ICqrMessagable
        where TS : class
        where TC : class
    {

        public FullSrvMsg<TS>? ServerMsg { get; set; }
        public FullSrvMsg<TC>? ClientMsg { get; set; }

        string ServerMsgString { get; set; }

        string ClientMsgString { get; set; }
        
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

        public override string ToJson() => JsonConvert.SerializeObject(this);

        public virtual T? FromJson<T>(string jsonText)
        {
            T? t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonText);
            if (t != null && t is ClientSrvMsg<TS, TC> csrvmsg)
            {
                this._hash = csrvmsg._hash;
                this._message = csrvmsg._message;
                this.RawMessage = csrvmsg.RawMessage;
                this.ClientMsgString = csrvmsg.ClientMsgString;
                this.ServerMsgString = csrvmsg.ServerMsgString;
                this.ServerMsg = csrvmsg.ServerMsg;
                this.ClientMsg = csrvmsg.ClientMsg;
            }
            return t;
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }

        public T? FromXml<T>(string xmlText)
        {
            throw new NotImplementedException();
        }
    }


}
