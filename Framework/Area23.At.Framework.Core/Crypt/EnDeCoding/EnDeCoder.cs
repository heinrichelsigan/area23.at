using System.Text;

namespace Area23.At.Framework.Core.Crypt.EnDeCoding
{
    /*
    /// <summary>
    /// static class EnDeCoder provides serveral static methods for ASCII, UTF7, UTF8, Unicode, UTF32 encoding.
    /// </summary>
    public class EnDeCoder<TCD> : IDecodable where TCD : IDecodable
    {

        public TCD TCodeable { get; private set; }

        public IDecodable Decodable => ((IDecodable)TCodeable);

        public Encoding EnCodIng { get; internal set; }

       

        public EnDeCoder(TCD encoder, EncodingType encodingType = EncodingType.None)
        {
            TCodeable = encoder;
            switch (encodingType)
            {
                case EncodingType.None: TCodeable = new RawString();
                case EncodingType.Hex16: return Hex16.Encode(inBytes);
                case EncodingType.Base16: return Base16.Encode(inBytes);
                case EncodingType.Hex32: return Hex32.Encode(inBytes);
                case EncodingType.Base32: return Base32.Encode(inBytes);
                case EncodingType.Uu: return Uu.Encode(inBytes, fromPlain, fromFile);
                case EncodingType.Base64:
                default: return Base64.Encode(inBytes);
            }
        }

        public string Encode(byte[] inBytes)
        {
            return (Encode(inBytes));
        }

        public byte[] Decode(string encodedString)
        {
            throw new NotImplementedException();
        }
    }
    */
}
