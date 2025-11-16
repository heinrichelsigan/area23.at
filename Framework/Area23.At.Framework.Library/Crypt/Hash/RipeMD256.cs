using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using System;

namespace Area23.At.Framework.Library.Crypt.Hash
{
    /// <summary>
    /// <see cref="Org.BouncyCastle.Crypto.Digests.RipeMD256Digest" />
    /// Thanx to the legion of <see href="https://bouncycastle.org/"" />
    /// </summary>
    public static class RipeMD256
    {
        /// <summary>
        /// <see cref="Org.BouncyCastle.Crypto.Digests.RipeMD256Digest" />
        /// </summary>
        /// <param name="stringToHash"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string HashString(string stringToHash)
        {
            if (string.IsNullOrEmpty(stringToHash))
                throw new ArgumentNullException("stringToHash");
            
            string resStr = string.Empty;
            byte[] bytes = EnDeCodeHelper.GetBytes(stringToHash);
            IDigest digest = new Org.BouncyCastle.Crypto.Digests.RipeMD256Digest();
            byte[] resBuf = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(bytes, 0, bytes.Length);
            digest.DoFinal(resBuf, 0);
            resStr = Hex.ToHexString(resBuf);

            return resStr;
        }
    }
}
