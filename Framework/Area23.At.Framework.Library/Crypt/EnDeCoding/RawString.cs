using System.Collections.Generic;

namespace Area23.At.Framework.Library.Crypt.EnDeCoding
{
    internal class RawString : IDecodable
    {
        public const string VALID_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz\"!'.?;,.;-_+*^%/=(){}[]ß´`:'#~<>§$&/|^°²³";

        #region common interface, interfaces for static members appear in C# 7.3 or later

        public IDecodable Decodable => this;
        
        public HashSet<char> ValidCharList => null;


        public string EnCode(byte[] inBytes) => RawString.Encode(inBytes);        

        public byte[] DeCode(string encodedString) => RawString.Decode(encodedString);

        public bool Validate(string encodedString) => true;

        public bool IsValidShowError(string encodedString, out string error)
        {
            error = "";
            return true;
        }

        #endregion common interface, interfaces for static members appear in C# 7.3 or later


        /// <summary>
        /// Encodes a byte[] 
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>hex16 encoded string</returns>
        public static string Encode(byte[] inBytes)
        {
            return RawString.ToRawString(inBytes);
        }

        /// <summary>
        /// Decodes a hex string to byte[]
        /// </summary>
        /// <param name="hexString">hex16 encoded string</param>
        /// <returns></returns>
        public static byte[] Decode(string encodedString)
        {
            return RawString.FromRawString(encodedString);
        }


        public static string ToRawString(byte[] inBytes)
        {
            return System.Text.Encoding.UTF8.GetString(inBytes);
        }

        public static byte[] FromRawString(string encodedString)
        {
            return System.Text.Encoding.UTF8.GetBytes(encodedString);
        }

    }

}
