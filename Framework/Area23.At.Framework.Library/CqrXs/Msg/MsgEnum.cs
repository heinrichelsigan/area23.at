using System;

namespace Area23.At.Framework.Library.CqrXs.Msg
{

    [Serializable]
    public enum MsgEnum
    {
        None = 0,
        RawWithHashAtEnd = 1,
        Json = 2,
        Xml = 3,
        MimeAttachment = 4
    }

    public enum MsgKind
    {
        Server = 0,
        Client = 1
    }
}
