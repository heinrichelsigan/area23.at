using Area23.At.Framework.Core.Static;
using Newtonsoft.Json;

namespace Area23.At.Framework.Core.CqrXs.CqrMsg
{

    public class ClientSrvMsg<TS, TC> : MsgContent, ICqrMessagable
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


        public override T? FromJson<T>(string jsonText) where T : default
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

        public override string ToXml() => Utils.SerializeToXml<ClientSrvMsg<TS, TC>>(this);

        public override T? FromXml<T>(string xmlText) where T : default
        {
            T? cqrT = Utils.DeserializeFromXml<T>(xmlText);
            if (cqrT is ClientSrvMsg<TS, TC> csmsgmc)
            {
                this.ServerMsg = csmsgmc.ServerMsg;
                this.ClientMsg = csmsgmc.ClientMsg;
                this._hash = csmsgmc._hash;
                this.RawMessage = csmsgmc.RawMessage;
                this.ServerMsg = csmsgmc.ServerMsg;
                this._message = csmsgmc._message;
                this.Md5Hash = csmsgmc.Md5Hash;
                this.ClientMsgString = csmsgmc.ClientMsgString;
                this.ServerMsg = csmsgmc.ServerMsg;
            }

            return cqrT;
            return base.FromXml<T>(xmlText);
        }

    }


}
