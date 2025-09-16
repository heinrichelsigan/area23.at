using Area23.At.Framework.Core.Static;
using System.ComponentModel;
using System.Text;

namespace Area23.At.Framework.Core.Cqr.Msg
{

    [Serializable]
    [DefaultValue(SerType.Json)]
    public enum SerType : short
    {
        None = 0x0000,
        Json = 0x1000,
        Xml = 0x2000,
        Mime = 0x3000,
        Raw = 0x4000
    }


    public enum MsgKind
    {
        Server = 0,
        Client = 1
    }

    public static class SerializationTypeExtension
    {

        public static SerType[] GetSerTypes()
        {
            List<SerType> list = new List<SerType>();
            foreach (string encName in Enum.GetNames(typeof(SerType)))
            {
                list.Add((SerType)Enum.Parse(typeof(SerType), encName));
            }

            return list.ToArray();
        }

        public static SerType GetSerType(string typeString)
        {
            return (SerType)Enum.Parse(typeof(SerType), typeString);
        }

        public static SerType GetSerTypeFromValue(short serValue)
        {
            serValue = (short)((serValue % 0x10000) - (serValue % 0x1000));
            foreach (SerType serType in GetSerTypes())
            {
                if ((short)serType == serValue)
                    return serType;
            }
            return SerType.None;
        }


        public static string Cerialize<T>(this SerType serTyoe, T t)
        {
            switch (serTyoe)
            {
                case SerType.Json: return Newtonsoft.Json.JsonConvert.SerializeObject(t);
                case SerType.Xml: return Utils.SerializeToXml<T>(t);
                case SerType.Raw:
                    MemoryStream ms = new MemoryStream();
                    ProtoBuf.Serializer.Serialize<T>(ms, t);
                    ms.Seek(0, SeekOrigin.Begin);
                    return Encoding.UTF8.GetString(ms.ToByteArray());
                case SerType.Mime: // TODO implement it
                case SerType.None:
                default:
                    return null;
            }
        }

        public static T DeCerialize<T>(this SerType serType, string cerialsCornFlakes)
        {
            switch (serType)
            {
                case SerType.Json: return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(cerialsCornFlakes);
                case SerType.Xml: return Utils.DeserializeFromXml<T>(cerialsCornFlakes);
                case SerType.Raw:
                    MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(cerialsCornFlakes));
                    return ProtoBuf.Serializer.Deserialize<T>(ms);
                case SerType.Mime: // TODO implement it
                case SerType.None:
                default:
                    return default(T);
            }
        }
    }

}
