using Area23.At.Framework.Library.CqrXs.CqrMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.CqrXs.CqrMsg
{
    public interface ICqrMessagable
    {
        MsgEnum MsgType { get; }        
        string Message { get; }
        string RawMessage { get; }

        string Hash { get; }
        string Md5Hash { get; }

        string ToJson();
        T FromJson<T>(string jsonText);
        string ToXml();
        T FromXml<T>(string xmlText);
      
    }
}
