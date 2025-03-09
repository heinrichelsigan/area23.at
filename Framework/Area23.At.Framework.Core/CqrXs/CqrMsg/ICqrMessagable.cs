using Area23.At.Framework.Core.CqrXs.CqrMsg;
using System;

namespace Area23.At.Framework.Core.CqrXs.CqrMsg
{
    public interface ICqrMessagable
    {
        public MsgEnum MsgType { get; }
        public string Message { get; }
        public string RawMessage { get; }       
        public string Hash { get; }
        public string Md5Hash { get; set; }

        abstract string ToJson();
        abstract T? FromJson<T>(string jsonText);
        abstract string ToXml();
        abstract T? FromXml<T>(string xmlText);
        
        static ICqrMessagable IsTo<T>(T cqrT) where T : ICqrMessagable
        {
            if (cqrT is ClientSrvMsg<FullSrvMsg<string>, FullSrvMsg<string>> csrvmsg)
                return (ICqrMessagable)csrvmsg;
            if (cqrT is ClientSrvMsg<FullSrvMsg<string>, FullSrvMsg<CqrFile>> csmsf)
                return (ICqrMessagable)csmsf;
            if (cqrT is ClientSrvMsg<FullSrvMsg<CqrFile>, FullSrvMsg<CqrFile>> csmff)
                return (ICqrMessagable)csmff;
            if (cqrT is ClientSrvMsg<FullSrvMsg<string>, FullSrvMsg<CqrImage>> csmsi)
                return (ICqrMessagable)csmsi;
            if (cqrT is ClientSrvMsg<FullSrvMsg<CqrImage>, FullSrvMsg<CqrImage>> csmii)
                return (ICqrMessagable)csmii;
            //TODO:
            if (cqrT is FullSrvMsg<string> fsmsg)
                return (ICqrMessagable)fsmsg;

            if (cqrT is CqrContact cntct)
                return (ICqrMessagable)cntct;

            if (cqrT is CqrImage cqrimg)
                return (ICqrMessagable)cqrimg;

            if (cqrT is CqrFile cqrf) // && cf.Data != null && !string.IsNullOrEmpty(cf.CqrFileName))
                return (ICqrMessagable)cqrf;

            if (cqrT is MsgContent msgc)
                return (ICqrMessagable)msgc;

            return (ICqrMessagable)cqrT;
        }


    }
}
