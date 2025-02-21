using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.CqrXs.CqrMsg
{

    [Serializable]
    public enum MsgEnum
    {
        None = 0,
        RawWithHashAtEnd = 1,
        JsonSerialized = 2,
        JsonDeserialized = 3
    }

    public enum MsgKind
    {
        Server = 0,
        Client = 1
    }
}
