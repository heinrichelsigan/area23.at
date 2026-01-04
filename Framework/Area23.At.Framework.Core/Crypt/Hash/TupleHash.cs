using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Utilities.Encoders;

namespace Area23.At.Framework.Core.Crypt.Hash
{

    /// <summary>
    /// <see cref="Org.BouncyCastle.Crypto.Digests.TupleHash" />
    /// </summary>
    public static class TupleHash
    {
        /// <summary>
        /// <see cref="Org.BouncyCastle.Crypto.Digests.TupleHash" />
        /// </summary>
        /// <param name="stringToHash">string to be hased</param>
        /// <returns>hashed hex string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string HashString(string stringToHash)
        {
            if (string.IsNullOrEmpty(stringToHash))
                throw new ArgumentNullException("stringToHash");

            string resStr = string.Empty;
            byte[] bytes = EnDeCodeHelper.GetBytes(stringToHash);
            IDigest digest = new Org.BouncyCastle.Crypto.Digests.TupleHash(256, bytes, 32);
            byte[] resBuf = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(bytes, 0, bytes.Length);
            digest.DoFinal(resBuf, 0);
            resStr = Hex.ToHexString(resBuf);

            return resStr;
        }

    }
}
