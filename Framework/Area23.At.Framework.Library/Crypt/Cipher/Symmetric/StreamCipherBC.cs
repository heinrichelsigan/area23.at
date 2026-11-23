using Area23.At.Framework.Library.Crypt.Cipher;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using System;

namespace EU.CqrXs.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// StreamCipherBC class to perform all StreamCiphers
    /// </summary>
    public class StreamCipherBC : IStreamCipher
    {

        #region fields and properties

        protected internal bool initialised = false, encryption;

        /// <summary>
        /// Base symmetric key block cipher interface, contains at runtime block cipher instance to constructor
        /// </summary>
        public IStreamCipher CryptoStreamCipher { get; private set; }

        public ICipherParameters StreamCipherParameters { get; private set; }

        #endregion fields and properties

        #region IStreamCipher interface imlementation

        public string AlgorithmName => CryptoStreamCipher.AlgorithmName;

        public void Reset() => CryptoStreamCipher.Reset();

        public void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is KeyParameter) && !(parameters is ParametersWithIV))
                throw new ArgumentException("only KeyParameter or ParametersWithIV expected.", "parameters");

            encryption = forEncryption;
            CryptoStreamCipher.Init(forEncryption, StreamCipherParameters);
            initialised = true;
        }

        public byte ReturnByte(byte input) => CryptoStreamCipher.ReturnByte(input);

        public void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
        {
            output = new byte[input.Length];
            int i = inOff, j = outOff;
            for (i = inOff; i < (inOff + length); i++)
            {
                output[j++] = CryptoStreamCipher.ReturnByte(input[i]);
            }
        }

        public void ProcessBytes(ReadOnlySpan<byte> input, Span<byte> output)
        {
            output = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                output[i] = ReturnByte(input[i]);
            }
        }

        #endregion IStreamCipher interface imlementation


        /// <summary>
        /// vtor og <see cref="StreamCipherBC"/>
        /// </summary>
        /// <param name="cparams">CryptParams</param>
        /// <param name="encrytion">true for encrytion, false for decrytion</param>
        public StreamCipherBC(CryptParams cparams, bool encrytion = true)
        {
            CryptoStreamCipher = cparams.StreamCipher == null ? new ChaChaEngine() : cparams.StreamCipher;
            StreamCipherParameters = cparams.CipherParameters;
            Init(encrytion, StreamCipherParameters);
        }

        /// <summary>
        /// generic Encryt for<see cref="StreamCipherBC"/>
        /// </summary>
        /// <param name="plainData">lain data bytes</param>
        /// <returns>ciher data bytes</returns>
        public byte[] Encrypt(byte[] plainData)
        {
            byte[] cipherData = new byte[plainData.Length];
            for (int j = 0; j < plainData.Length; j++)
            {
                cipherData[j] = ReturnByte(plainData[j]);
            }

            return cipherData;
        }

        /// <summary>
        /// generic Decryt for <see cref="StreamCipherBC"/>
        /// </summary>
        /// <param name="cipherData">encryted ciher data<see cref="T:byte[]"/></param>
        /// <returns>lain data  data <see cref="T:byte[]"/></returns>
        public byte[] Decrypt(byte[] cipherData)
        {
            byte[] plainData = new byte[cipherData.Length];
            for (int j = 0; j < cipherData.Length; j++)
            {
                plainData[j] = ReturnByte(cipherData[j]);
            }

            return plainData;
        }
    }
}