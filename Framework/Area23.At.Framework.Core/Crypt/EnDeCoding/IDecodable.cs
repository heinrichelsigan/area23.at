namespace Area23.At.Framework.Core.Crypt.EnDeCoding
{

    /// <summary>
    /// IDecodable is an common interface for <see cref="Base16"/>, <see cref="Base32"/>, <see cref="Base64"/>,
    /// <see cref="Hex16"/>, <see cref="Hex32"/> and <seealso cref="Uu"/> Encoding / Decoding
    /// </summary>
    public interface IDecodable
    {
        IDecodable Decodable { get; }

        public static HashSet<char>? ValidCharList { get; private set; }

        public static string EnCode(byte[] inBytes, EncodingType encodeType = EncodingType.Base64)
        {
            IDecodable enc = encodeType.GetEnCoder();
            return enc.Encode(inBytes);
        }

        public static byte[] DeCode(string encodedString, EncodingType encodeType = EncodingType.Base64)
        {
            IDecodable dec = encodeType.GetEnCoder();
            return dec.Decode(encodedString);
        }

        public static bool Validate(string encodedString, out string error, EncodingType encodeType = EncodingType.Base64)
        {
            IDecodable dec = encodeType.GetEnCoder();
            bool isValide = dec.IsValidShowError(encodedString, out error);
            return isValide;
        }


        public abstract byte[] Decode(string encodedString);

        public abstract string Encode(byte[] inBytes);

        public abstract bool IsValid(string encodedString);

        public abstract bool IsValidShowError(string encodedString, out string error);


    }

}
