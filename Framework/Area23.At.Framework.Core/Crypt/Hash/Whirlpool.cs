using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;

namespace Area23.At.Framework.Core.Crypt.Hash
{
    /// <summary>
    /// <see cref="Org.BouncyCastle.Crypto.Digests.WhirlpoolDigest" />
    /// </summary>
    public static class Whirlpool
    {
        public static string HashString(string stringToHash)
        {
            string resStr = string.Empty;

            if (string.IsNullOrEmpty(stringToHash))
                throw new ArgumentNullException("stringToHash");
                        
            byte[] bytes = EnDeCodeHelper.GetBytes(stringToHash);
            IDigest digest = new Org.BouncyCastle.Crypto.Digests.WhirlpoolDigest();
            byte[] resBuf = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(bytes, 0, bytes.Length);
            digest.DoFinal(resBuf, 0);
            resStr = Hex.ToHexString(resBuf);
            return resStr;
        }
    }
}
