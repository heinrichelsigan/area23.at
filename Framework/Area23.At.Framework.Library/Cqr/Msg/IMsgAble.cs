namespace Area23.At.Framework.Library.Cqr.Msg
{
    public interface IMsgAble
    {
        CType MsgType { get; }        
        string Message { get; }
        string SerializedMsg { get; }

        string Hash { get; }
        string Md5Hash { get; }

        string ToJson();
        T FromJson<T>(string jsonText);
        string ToXml();
        T FromXml<T>(string xmlText);
      
    }
}
