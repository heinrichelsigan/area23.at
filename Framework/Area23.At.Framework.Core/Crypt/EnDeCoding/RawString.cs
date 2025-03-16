using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Crypt.EnDeCoding
{
    /// <summary>
    /// Base16 hexadecimal byte encoding / decoding
    /// </summary>
    public class RawString : IDecodable
    {
        public const string VALID_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz\"!'.?;,.;-_+*^%/=(){}[]ß´`:'#~<>§$&/|^°²³";
        

        public IDecodable Decodable => this;

        public static HashSet<char>? ValidCharList { get; private set; } = null;

        /// <summary>
        /// Encodes a byte[] 
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>hex16 encoded string</returns>
        public string Encode(byte[] inBytes)
        {
            return RawString.ToRawString(inBytes);
        }

        /// <summary>
        /// Decodes a hex string to byte[]
        /// </summary>
        /// <param name="hexString">hex16 encoded string</param>
        /// <returns></returns>
        public byte[] Decode(string encodedString)
        {
            return RawString.FromRawString(encodedString);
        }

        public bool IsValidShowError(string encodedString, out string error)
        {
            error = "";
            return true;
        }

        public bool IsValid(string encodedStr) => true;

        public static string ToRawString(byte[] inBytes)
        {
            return EnDeCodeHelper.GetString(inBytes);
        }

        public static byte[] FromRawString(string encodedString)
        {
            return EnDeCodeHelper.GetBytes(encodedString);
        }

    }

}