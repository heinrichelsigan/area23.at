using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.EnDeCoding
{

    [DefaultValue(EncodingType.Base64)]
    public enum EncodingType
    {
        Null    = 0x00,
        None    = 0x01,
        Base16  = 0x10,
        Hex16   = 0x11,
        Base32  = 0x20,
        Hex32   = 0x21,
        Uu      = 0x33,
        Base64  = 0x40,
        Hex64   = 0x41
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
    }
}
