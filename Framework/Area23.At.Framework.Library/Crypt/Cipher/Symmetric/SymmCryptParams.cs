using Area23.At.Framework.Library.Crypt.Hash;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Security.Cryptography;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// SymmCryptParams prefered params for symmetric block cipher
    /// </summary>
    public class SymmCryptParams : CryptParams
    {
        SymmCipherEnum symmCipher;
        public SymmCipherEnum SymmCipher
        {
            get => symmCipher;
            set
            {
                symmCipher = value;
                Cipher = symmCipher.ToCipherEnum();
            }
        }

        #region ctor

        /// <summary>
        /// standard ctor with <see cref="SymmCipherEnum.Aes"/> default
        /// </summary>
        public SymmCryptParams() : base()
        {
            SymmCipher = SymmCipherEnum.Aes;
            Size = 256;
            KeyLen = 32;
            Mode = "ECB";
            CMode = CipherMode.ECB;
            BlockCipher = new AesEngine();
            KeyHashing = KeyHash.Hex;
        }

        /// <summary>
        /// constructs a object with correct <see cref="Mode"/>, <see cref="Size"/>, <see cref="CryptParams.KeyLen"/>
        /// for parameter <see cref="Cipher"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/></param>
        public SymmCryptParams(SymmCipherEnum cipherAlgo) : base(cipherAlgo.ToCipherEnum())
        {
            SymmCipher = cipherAlgo;
        }

        /// <summary>
        /// constructs a <see cref="SymmCryptParams"/> object 
        /// with correct <see cref="Mode"/>, <see cref="Size"/>, 
        /// with additional Key and <see cref="KeyHash" />
        /// for parameter <see cref="Cipher"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/></param>
        /// <param name="key"></param>
        /// <param name="hash"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SymmCryptParams(SymmCipherEnum cipherAlgo, string key, string hash) :
            this(cipherAlgo)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");
            SymmCipher = cipherAlgo;
            Key = key;
            Hash = (string.IsNullOrEmpty(hash)) ? KeyHashing.Hash(key) : hash;
        }


        /// <summary>
        /// constructs a <see cref="SymmCryptParams"/> object by <see cref="SymmCipherEnum"/>
        /// with additional key <see cref="KeyHash"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/></param>
        /// <param name="key">secret key</param>
        /// <param name="keyHash">key hashing</param>
        public SymmCryptParams(SymmCipherEnum cipherAlgo, string key, KeyHash keyHash) :
            this(cipherAlgo)
        {
            Key = key;
            KeyHashing = keyHash;
            Hash = KeyHashing.Hash(key);
        }

        /// <summary>
        /// constructs a <see cref="CryptParams"/> object by <see cref="CipherEnum"/>
        /// with additional kay and hash, <see cref="KeyHash"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="CipherEnum"/></param>
        /// <param name="key">secret key</param>
        /// <param name="hash">corresponding key hash</param>
        /// <param name="keyHash">key hashing</param>
        public SymmCryptParams(SymmCipherEnum cipherAlgo, string key, string hash, KeyHash keyHash) :
            this(cipherAlgo)
        {
            Key = key;
            KeyHashing = keyHash;
            Hash = (string.IsNullOrEmpty(hash)) ? KeyHashing.Hash(key) : hash;
        }

        /// <summary>
        /// Constructs instance via another object instance
        /// </summary>
        /// <param name="cryptParams">another instance</param>
        public SymmCryptParams(SymmCryptParams cryptParams) :
            this(cryptParams.SymmCipher, cryptParams.Key, cryptParams.Hash, cryptParams.KeyHashing)
        { CMode = cryptParams.CMode; }

        #endregion ctor

        /// <summary>
        /// static way to get valid <see cref="SymmCryptParams"/> for a requested <see cref="SymmCipherEnum"/>
        /// </summary>
        /// <param name="cipherAlgo"><see cref="SymmCipherEnum"/></param>
        /// <returns><see cref="SymmCryptParams"/></returns>
        [Obsolete("RequestPreferedAlgorithm no mote used", true)]
        public static SymmCryptParams RequestPreferedAlgorithm(SymmCipherEnum cipherAlgo)
        {
            return new SymmCryptParams(cipherAlgo);
        }

        [Obsolete("GetCryptParams is not used anymore.", true)]
        public static IBlockCipher GetCryptParams(SymmCipherEnum cipherAlgo)
        {
            return new SymmCryptParams(cipherAlgo).BlockCipher;
        }

    }

}
