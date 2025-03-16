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

        public static EncodingType GetEnum(string enCodingString) 
        {
            switch (enCodingString.ToLower())
            {
                case "raw":
                case "none":
                case "null":
                case "0":
                    return EncodingType.None;

                case "hex16":
                case "hex":
                case "h16":
                case "16":
                    return EncodingType.Hex16;

                case "base16":
                case "b16":
                    return EncodingType.Base16;

                case "base32":
                case "b32":
                    return EncodingType.Base32; 

                case "hex32":
                case "h32":
                case "32":
                    return EncodingType.Hex32; 

                case "uu":
                case "uue":
                case "uud":
                case "uuencode":
                case "uudecode":
                    return EncodingType.Uu;

                case "base64":
                case "mime":
                case "b64":
                case "64":
                default:
                    return EncodingType.Base64;
            }

        }        

    }

}
