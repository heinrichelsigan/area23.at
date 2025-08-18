

namespace Area23.At.Framework.Core.Crypt.EnDeCoding
{
    /// <summary>
    /// Base16 hexadecimal byte encoding / decoding
    /// </summary>
    public class RawString : IDecodable
    {
        public const string VALID_CHARS =
            " !\"#$%&'()*+,-./0123456789:;<=>?" +
            "@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_" +
            "`abcdefghijklmnopqrstuvwxyz{|}~Ç" +
            "üéâäàåçêëèïîìÄÅÉæÆôöòûùÿÖÜ¢£¥₧ƒá";
            // "íóúñÑªº¿½¼¡ßπΣσµτΦΘΩδ∞φε∩²³ⁿ©®≈"; 



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
            /*
            byte[] sixBytes = new byte[6];
            int len = inBytes.Length;
            int offset = inBytes.Length % 6;
            string rawString = "";
            for (int i = 0; i < inBytes.Length; i += 6)
            {
                string rawHex = "0x";
                offset = ((i + 6) <= len) ? 6 : len % 6; // (len - i);
                Array.Copy(inBytes, i, sixBytes, 0, offset);
                foreach (byte b in sixBytes)
                    rawHex += string.Format("{0:x2}", b);
                int intValue = Convert.ToInt32(rawHex, 16);
                // TODO: l8r
            }
            */


            return EnDeCodeHelper.GetString(inBytes);
        }

        public static byte[] FromRawString(string encodedString)
        {
            return EnDeCodeHelper.GetBytes(encodedString);
        }

    }

}