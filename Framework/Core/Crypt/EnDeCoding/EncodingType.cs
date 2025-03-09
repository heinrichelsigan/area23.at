using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Crypt.EnDeCoding
{

    [DefaultValue(EncodingType.Base64)]
    public enum EncodingType
    {
        Null = 0x00,
        None = 0x01,
        Base16 = 0x10,
        Hex16 = 0x11,
        Base32 = 0x20,
        Hex32 = 0x21,
        Uu = 0x33,
        Base64 = 0x40
    }

    public static class EncodingTypesExtensions
    {
        public static EncodingType[] GetEncodingTypes()
        {
            List<EncodingType> list = new List<EncodingType>();
            foreach (string encName in Enum.GetNames(typeof(EncodingType)))
            {
                list.Add((EncodingType)Enum.Parse(typeof(EncodingType), encName));
            }

            return list.ToArray();
        }

        public static IDecodable GetEnCoder(this EncodingType type)
        {
            switch (type)
            {
                case EncodingType.None:
                case EncodingType.Null: return ((IDecodable)new RawString());
                case EncodingType.Hex16: return ((IDecodable)new Hex16());
                case EncodingType.Base16: return ((IDecodable)new Base16());
                case EncodingType.Hex32: return ((IDecodable)new Hex32());
                case EncodingType.Base32: return ((IDecodable)new Base32());
                case EncodingType.Uu: return ((IDecodable)new Uu());
                case EncodingType.Base64:
                default: return ((IDecodable)new Base64());
            }
            
        }

    }

}
