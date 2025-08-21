using System;

namespace Area23.At.Framework.Library.Cqr.Msg
{

    [Serializable]
    public enum SerType
    {
        None = 0,
        Json = 1,
        Xml = 2,
        Mime = 3,
        Raw = 4
    }

    public enum MsgKind
    {
        Server = 0,
        Client = 1
    }
}
