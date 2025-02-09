using DBTek.Crypto;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.EnDeCoding
{
    /// <summary>
    /// interface IEnDeCoder provides basic functionality, that each EnDeCoder must offer
    /// </summary>
    public interface IEnDeCoder
    {
        /// <summary>
        /// Generic Encode static method
        /// </summary>
        /// <param name="inBytes">plain in byte[]</param>
        /// <param name="encodingType"><see cref="EncodingType"</param>
        /// <returns>encoded string</returns>
        public static string Encode(byte[] inBytes, EncodingType encodingType = EncodingType.Base64)
        {
            switch (encodingType)
            {
                case EncodingType.Null: return System.Text.Encoding.UTF8.GetString(inBytes);
                case EncodingType.Hex16: return Hex16.Encode(inBytes);
                case EncodingType.Base16: return Base16.Encode(inBytes);
                case EncodingType.Hex32: return Hex32.Encode(inBytes);
                case EncodingType.Base32: return Base32.Encode(inBytes);
                case EncodingType.Uu: return Uu.Encode(inBytes);
                case EncodingType.Base64:
                default: return Base64.Encode(inBytes);
            }
        }


        /// <summary>
        /// Generic Decocde static Method
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <returns>Decoded array of byte</returns>
        public static byte[] Decode(string encodedString, EncodingType encodingType = EncodingType.Base64)
        {
            switch (encodingType)
            {
                case EncodingType.Null: return System.Text.Encoding.UTF8.GetBytes(encodedString);
                case EncodingType.Hex16: return Hex16.Decode(encodedString);
                case EncodingType.Base16: return Base16.Decode(encodedString);
                case EncodingType.Hex32: return Hex32.Decode(encodedString);
                case EncodingType.Base32: return Base32.Decode(encodedString);
                case EncodingType.Uu: return Uu.Decode(encodedString);
                case EncodingType.Base64:
                default: return Base64.Decode(encodedString);
            }
        }

        public static bool IsValid(string encodedString, EncodingType encodingType = EncodingType.Base64)
        {
            switch (encodingType)
            {
                case EncodingType.Null: return true;
                case EncodingType.Hex16: return Hex16.IsValid(encodedString);
                case EncodingType.Base16: return Base16.IsValid(encodedString);
                case EncodingType.Hex32: return Hex32.IsValid(encodedString);
                case EncodingType.Base32: return Base32.IsValid(encodedString);
                case EncodingType.Uu: return Uu.IsValid(encodedString);
                case EncodingType.Base64:
                default: return Base64.IsValid(encodedString);
            }
        }

    }

}