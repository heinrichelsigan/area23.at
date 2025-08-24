using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// More complex sbyte mapping from 0x0 .. to 0xf as symmetric cipher matrix
    /// position swaps and byte mappings are seperated in 2 matrizes 
    /// and maybe I will add ZenMatrix3 l8r, to multiply and divide byte values with a 3rd matrix 
    /// for mapping sbyte[1] => byte[1] 0xf => 0xab and generate 
    /// 
    /// maybe this encryption is already invented, but created at Git by zen@area23.at (Heinrich Elsigan)
    /// </summary>
    public class ZenMatrix2 : ZenMatrix
    {

        #region fields

        private const string SYMM_CIPHER_ALGO_NAME = "ZenMatrix2";
        // protected internal new byte[] privateBytes = new byte[0x10];
        //protected internal byte[] privateBytes2 = new byte[0x10];

        #endregion fields

        #region Properties
        

        /// <summary>
        /// Inverse Matrix 2 
        /// </summary>
        protected new internal sbyte[] InverseMatrix
        {
            get
            {
                if (_inverseMatrix == null ||
                    _inverseMatrix.Length < 0x10 ||
                    (_inverseMatrix[0] == (sbyte)0x0 && _inverseMatrix[1] == (sbyte)0x0 && _inverseMatrix[0xf] == (sbyte)0x0) ||
                    (_inverseMatrix[0] == (sbyte)0x0 && _inverseMatrix[1] == (sbyte)0x1 && _inverseMatrix[0xf] == (sbyte)0xf))
                {
                    _inverseMatrix = BuildInverseMatrix(MatrixPermutationKey);
                }

                return _inverseMatrix;
            }
        }


        #endregion Properties

        #region IBlockCipher interface

        public new string AlgorithmName => SYMM_CIPHER_ALGO_NAME;

        /// <summary>
        /// Processes one BLOCK with BLOCK_SIZE <see cref="BLOCK_SIZE"/>
        /// </summary>
        /// <param name="inBuf"></param>
        /// <param name="inOff"></param>
        /// <param name="outBuf"></param>
        /// <param name="outOff"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        public new int ProcessBlock(byte[] inBuf, int inOff, byte[] outBuf, int outOff)
        {
            if (privateBytes == null)
                throw new InvalidOperationException($"{SYMM_CIPHER_ALGO_NAME} engine not initialised");

            int len = BLOCK_SIZE;
            int aCnt = 0, bCnt = 0;

            if (inOff >= inBuf.Length || inOff + BLOCK_SIZE > inBuf.Length)
                throw new InvalidDataException($"Cannot process next {BLOCK_SIZE} bytes, because inOff ({inOff}) + BLOCK_SIZE ({BLOCK_SIZE}) > inBuf.Length ({inBuf.Length})");
            if (outOff >= outBuf.Length || outOff + BLOCK_SIZE > outBuf.Length)
                throw new InvalidDataException($"Cannot process next {BLOCK_SIZE} bytes, because inOff ({outOff}) + BLOCK_SIZE ({BLOCK_SIZE}) > outBuf.Length ({outBuf.Length})");

            if (inOff < inBuf.Length && inOff + BLOCK_SIZE <= inBuf.Length && outOff < outBuf.Length && outOff + BLOCK_SIZE <= outBuf.Length)
            {
                byte[] inOffBuf = new byte[inBuf.Length - inOff];
                Array.Copy(inBuf, inOff, inOffBuf, 0, inOffBuf.Length);

                if (forEncryption)
                {
                    byte[] padBytes = PadBuffer(inOffBuf);
                    inOffBuf = padBytes;
                }

                if (BLOCK_SIZE > inOffBuf.Length)
                    throw new InvalidOperationException($"{BLOCK_SIZE} > inOffBuf.Length = {inOffBuf.Length}");

                byte[] processed = new byte[BLOCK_SIZE];
                string shifted = "";

                for (bCnt = 0; bCnt < BLOCK_SIZE; bCnt++)
                {
                    byte b = inOffBuf[bCnt];
                    MapByteValue(ref b, out byte mappedByte, forEncryption);
                    processed[bCnt] = mappedByte;
                    shifted += mappedByte.ToString("x2");                 
                }
                char[] swapped = shifted.ToCharArray();

                for (aCnt = 0, bCnt = len; aCnt < len; aCnt++, bCnt++)
                {
                    char chA = shifted[aCnt];
                    char chB = shifted[bCnt];
                    int posA = (forEncryption) ? MatrixPermutationKey[aCnt] : InverseMatrix[aCnt];
                    int posB = ((forEncryption) ? MatrixPermutationKey[bCnt % 0x10] : InverseMatrix[bCnt % 0x10]) + 0x10;
                    swapped[posA] = chA;
                    swapped[posB] = chB;
                }
                for (aCnt = 0, bCnt = 0; bCnt < swapped.Length; aCnt++, bCnt += 2)
                {
                    string toByte = string.Concat(swapped[bCnt].ToString(), swapped[bCnt + 1].ToString());
                    byte forProcessed;
                    IFormatProvider provider = new NumberFormatInfo();
                    if (!Byte.TryParse(toByte, System.Globalization.NumberStyles.HexNumber, provider, out forProcessed))
                        forProcessed = (byte)0x0;
                    processed[aCnt] = Convert.ToByte(toByte);
                }

                // byte[] outBytes = processed;
                //if (!forEncryption)
                //{
                //    outBytes = PadBuffer(processed);
                //}

                Array.Copy(processed, 0, outBuf, outOff, BLOCK_SIZE);

                return BLOCK_SIZE;
            }

            return 0;
        }

        public new int ProcessBlock(ReadOnlySpan<byte> input, Span<byte> output)
        {
            int aCnt = 0, bCnt = 0;
            byte[] buffer = input.ToArray();
            if (forEncryption)                                  // add padding buffer to match BLOCK_SIZE
            {
                byte[] padBytes = PadBuffer(input.ToArray());
                buffer = padBytes;
            }

            if (BLOCK_SIZE > buffer.Length)
                throw new InvalidOperationException($"{BLOCK_SIZE} > buffer.Length = {buffer.Length}");

            byte[] processed = new byte[BLOCK_SIZE];
            string shifted = "";

            for (bCnt = 0; bCnt < BLOCK_SIZE; bCnt++)
            {
                byte b = buffer[bCnt];
                MapByteValue(ref b, out byte mappedByte, forEncryption);
                processed[bCnt] = mappedByte;
                shifted += mappedByte.ToString("x2");
            }
            char[] swapped = shifted.ToCharArray();

            for (aCnt = 0, bCnt = BLOCK_SIZE; aCnt < BLOCK_SIZE; aCnt++, bCnt++)
            {
                char chA = shifted[aCnt];
                char chB = shifted[bCnt];
                int posA = (forEncryption) ? MatrixPermutationKey[aCnt] : InverseMatrix[aCnt];
                int posB = ((forEncryption) ? MatrixPermutationKey[bCnt % 0x10] : InverseMatrix[bCnt % 0x10]) + 0x10;
                swapped[posA] = chA;
                swapped[posB] = chB;
            }
            for (aCnt = 0, bCnt = 0; bCnt < swapped.Length; aCnt++, bCnt += 2)
            {
                string toByte = string.Concat(swapped[bCnt].ToString() + swapped[bCnt + 1].ToString());
                byte forProcessed;
                IFormatProvider provider = new NumberFormatInfo();
                if (!Byte.TryParse(toByte, System.Globalization.NumberStyles.HexNumber, provider, out forProcessed))
                    forProcessed = (byte)0x0;
                processed[aCnt] = Convert.ToByte(toByte);
            }

            // byte[] outBytes = processed;
            //if (!forEncryption)                             // trim padding buffer from decrypted output
            //{
            //    outBytes = PadBuffer(processed);
            //}

            output = new Span<byte>(processed);

            return BLOCK_SIZE;
        }

        #endregion IBlockCipher interface

        #region ctor_init_gen_reverse

        /// <summary>
        /// public constructor
        /// </summary>
        public ZenMatrix2() : base()
        {
            sbyte sbcnt = 0x0;
            MatrixPermutationKey = new sbyte[0x10];
            foreach (sbyte s in MatrixPermutationBase)
            {
                privateBytes[sbcnt % 0x10] = (byte)0x0;
                MatrixPermutationKey[sbcnt++] = s;
            }
            PermutationKeyHash = new HashSet<sbyte>(MatrixPermutationBase);
            _inverseMatrix = BuildInverseMatrix(MatrixPermutationKey);
        }

        /// <summary>
        /// initializes a <see cref="ZenMatrix"/> with secret user key string and hash iv
        /// </summary>
        /// <param name="secretKey">user's secret key</param>
        /// <param name="hashIV">private key hash iv string</param>
        /// <param name="fullSymmetric">
        /// fullSymmetric means that zen matrix is it's inverse element 
        /// and decrypts back to plain text, when encrypting twice or ²</param>       
        /// <exception cref="ApplicationException"></exception>
        public ZenMatrix2(string secretKey = "", string hashIV = "", bool fullSymmetric = false) : this()
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("secretKey");

            hashIV = string.IsNullOrEmpty(hashIV) ? EnDeCodeHelper.KeyToHex(secretKey) : hashIV;
            byte[] keyBytes = CryptHelper.GetUserKeyBytes(secretKey, hashIV, 0x10);
            byte[] keyBytes2 = CryptHelper.GetUserKeyBytes(secretKey, hashIV, 0x20);

            ZenMatrixGenWithBytes(keyBytes2, true);
        }


        /// <summary>
        /// initializes a <see cref="ZenMatrix"/> with an array of key bytes
        /// </summary>
        /// <param name="keyBytes">array of key bytes</param>
        /// <param name="fullSymmetric">
        /// fullSymmetric means that zen matrix is it's inverse element 
        /// and decrypts back to plain text, when encrypting twice or ²</param> 
        public ZenMatrix2(byte[] keyBytes, bool fullSymmetric = false) : this()
        {
            ZenMatrixGenWithBytes(keyBytes, fullSymmetric);
        }

        /// <summary>
        /// InitMatrixSymChiffer - base initialization of variables, needed for matrix sym chiffer encryption
        /// </summary>
        private void InitMatrixSymChiffer()
        {
            sbyte sbcnt = 0x0;
            MatrixPermutationKey = new sbyte[0x10];
            
            foreach (sbyte s in MatrixPermutationBase)
            {
                privateBytes[sbcnt % 0x10] = (byte)0x0;
                MatrixPermutationKey[sbcnt] = s;
            }
            PermutationKeyHash = new HashSet<sbyte>(MatrixPermutationBase);
            _inverseMatrix = BuildInverseMatrix(MatrixPermutationKey);
        }


        /// <summary>
        /// Generates ZenMatrix with key bytes
        /// </summary>
        /// <param name="keyBytes">must have at least 4 bytes and will be truncated after 16 bytes
        /// only the first 16 bytes will be taken from keyBytes for <see cref="ZenMatrix"/>
        /// </param>
        /// <returns>true, if init was with same key successfull</returns>
        /// <param name="fullSymmetric">
        /// fullSymmetric means that zen matrix is it's inverse element 
        /// and decrypts back to plain text, when encrypting twice or ²</param>       
        /// <exception cref="ApplicationException"></exception>
        protected override void ZenMatrixGenWithBytes(byte[] keyBytes, bool fullSymmetric = false)
        {
            if ((keyBytes == null || keyBytes.Length < 4))
                throw new ApplicationException("byte[] keyBytes is null or keyBytes.Length < 4");

            base.ZenMatrixGenWithBytes(keyBytes, fullSymmetric);
        }


        #endregion ctor_init_gen_reverse

        #region ProcessEncryptDecryptBytes

        /// <summary>
        /// ProcessBytes processes bytes for encryption or decryption depending on <see cref="forEncryption"/>
        ///     processes the next len=16 bytes to encrypt, starting at offSet
        ///     or processes the next len=16 bytes to decrypt, starting at offSet
        /// </summary>
        /// <param name="inBytes">in bytes array to encrypt</param>
        /// <param name="offSet">starting offSet</param>
        /// <param name="len">len of byte block (default 16)</param>
        /// <returns>byte[len] (default: 16) segment of encrypted bytes</returns>
        protected internal override byte[] ProcessBytes(byte[] inBytes, int offSet = 0, int len = 0x10)
        {
            int aCnt = 0, bCnt = 0;
            if (offSet < inBytes.Length && offSet + len <= inBytes.Length)
            {
                byte[] processed = new byte[len];
                string shifted = "";
                for (aCnt = 0, bCnt = offSet; bCnt < offSet + len; aCnt++, bCnt++)
                {
                    byte b = inBytes[bCnt];
                    MapByteValue(ref b, out byte mappedByte, forEncryption);
                    processed[aCnt] = mappedByte;
                    shifted += mappedByte.ToString("x2");
                }

                char[] swapped = shifted.ToCharArray();
                for (aCnt = 0, bCnt = len; aCnt < len; aCnt++, bCnt++)
                {
                    char chA = shifted[aCnt];
                    char chB = shifted[bCnt];
                    int posA = (forEncryption) ? MatrixPermutationKey[aCnt] : InverseMatrix[aCnt];
                    int posB = ((forEncryption) ? MatrixPermutationKey[bCnt % 0x10] : InverseMatrix[bCnt % 0x10]) + 0x10;
                    swapped[posA] = chA;
                    swapped[posB] = chB;
                }
                for (aCnt = 0, bCnt = 0; bCnt < swapped.Length; aCnt++, bCnt+=2)
                {
                    string toByte = string.Concat(swapped[bCnt].ToString(), swapped[bCnt + 1].ToString());
                    byte forProcessed;
                    IFormatProvider provider = new NumberFormatInfo();
                    if (!Byte.TryParse(toByte, System.Globalization.NumberStyles.HexNumber, provider, out forProcessed))
                        forProcessed = (byte)0x0;
                    processed[aCnt] = forProcessed;
                }

                return processed;
            }

            return new byte[0];
        }

        #endregion ProcessEncryptDecryptBytes

        #region encrypt decrypt

        /// <summary>
        /// in case of encryption, 
        ///     pads 0 or random buffer at end of inBytes,
        ///     so that inBytes % BLOCK_SIZE == 0 
        /// in case of decryption,
        ///     trims remaining padding buffer from inBytes
        /// encryption or decryption are triggered via <see cref="forEncryption"/>
        /// </summary>
        /// <param name="inBytes">input bytes to pad </param>
        /// <param name="useRandom">use random padding</param>
        /// <returns>padded or unpadded out bytes</returns>
        public virtual byte[] PadBuffer(byte[] inBytes, bool useRandom = false)
        {
            int ilen = inBytes.Length;                          // length of data bytes
            int oSize = (BLOCK_SIZE - (ilen % BLOCK_SIZE));     // oSize is rounded up to next number % BLOCK_SIZE == 0
            byte[] outBytes;

            if (forEncryption)                                  // add buffer for encryption to inbytes
            {
                long olen = ((long)(ilen + oSize));             // olen is (long)(ilen + oSize)
                byte[] padbuf = new byte[oSize];                // padding buffer 
                outBytes = new byte[olen];                      // out bytes with random padding bytes at end            

                if (!useRandom)
                    for (int ic = 0; ic < padbuf.Length; padbuf[ic++] = (byte)0) ;
                else
                {
                    Random rnd = new Random(ilen);
                    rnd.NextBytes(padbuf);
                }

                for (int i = 0, j = 0; i < olen; i++)
                {
                    // outBytes[i] = (i < ilen) ? inBytes[i] : ((i == ilen || i == (olen - 1)) ? (byte)0x0 : buf[j++]);
                    if (i < ilen)
                        outBytes[i] = inBytes[i];               // copy full inBytes to outBytes
                    else if (i == ilen)
                        outBytes[i] = (byte)0x0;                // write 0x0 at end of inBytes
                    else if (i > ilen)
                        outBytes[i] = padbuf[j++];              // fill rest with padding buffer
                    else if (i == (olen - 1))
                        outBytes[i] = (byte)0x0;                // terminate outBytes with NULL
                }
            }
            else                                                // truncate padding buffer to get trimmed decrypted output
            {
                int olen = inBytes.Length;
                bool last0 = false;

                for (olen = ilen; (olen > 0 && !last0); olen--)
                {
                    if (olen < (ilen - 2))
                    {
                        if ((inBytes[olen - 1] == (byte)0x0) && inBytes[olen - 2] != (byte)0x0)
                        {
                            last0 = true;
                            break;
                        }
                    }
                }

                outBytes = (olen > 1) ? new byte[olen] : new byte[ilen];
                Array.Copy(inBytes, 0, outBytes, 0, outBytes.Length);
            }

            return outBytes;

        }
    
        /// <summary>
        /// MatrixSymChiffer Encrypt member function
        /// </summary>
        /// <param name="pdata">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public virtual byte[] Encrypt(byte[] pdata)
        {
            // Check arguments.
            if (pdata == null || pdata.Length <= 0)
                throw new ArgumentNullException("ZenMatrix byte[] Encrypt(byte[] pdata): ArgumentNullException pdata = null or Lenght 0.");

            forEncryption = true;
            byte[] obytes = PadBuffer(pdata, true);

            List<byte> encryptedBytes = new List<byte>();
            for (int i = 0; i < obytes.Length; i += 0x10)
            {
                foreach (byte pb in ProcessBytes(obytes, i, 0x10))
                {
                    encryptedBytes.Add(pb);
                }
            }

            return encryptedBytes.ToArray();
        }

        /// <summary>
        /// MatrixSymChiffer Decrypt member function
        /// </summary>
        /// <param name="cdata">encrypted cipher <see cref="byte[]">bytes</see></param>
        /// <returns>decrypted plain byte[] data</returns>
        public virtual byte[] Decrypt(byte[] ecdata)
        {
            if (ecdata == null || ecdata.Length <= 0)
                throw new ArgumentNullException("ZenMatrix byte[] Encrypt(byte[] ecdata): ArgumentNullException ecdata = null or Lenght 0.");

            forEncryption = false;
            int eclen = ecdata.Length;

            List<byte> decBytes = new List<byte>();
            for (int pc = 0; pc < ecdata.Length; pc += 16)
            {
                foreach (byte rb in ProcessBytes(ecdata, pc, 16))
                {
                    decBytes.Add(rb);
                }
            }

            byte[] outBytes = PadBuffer(decBytes.ToArray(), true);

            return outBytes;
        }

        #endregion encrypt decrypt



        #region static helpers swap byte and SwapT{T} generic 

        /// <summary>
        /// BuildInverseMatrix, builds the determinant decryption matrix for sbyte[16] encryption matrix
        /// </summary>
        /// <param name="matrix">sbyte[16] encryption matrix</param>
        /// <returns><see cref="sbyte[]">sbyte[16]</see> decryption matrix (determinante)</returns>
        internal static sbyte[] BuildInverseMatrix2(sbyte[] matrix, int size = 0x10)
        {
            return BuildInverseMatrix(matrix, 0x10);
        }

        /// <summary>
        /// MapByteValue splits a byte in 2 0x0 - 0xf segments and map both trough <see cref="MatrixPermutationKey"/> in case of encrypt,
        /// through <see cref="InverseMatrix2"/> in case of decryption.
        /// </summary>
        /// <param name="inByte"><see cref="byte"/> in byte to map</param>
        /// <param name="outByte"><see cref=byte"/> mapped out byte</param>
        /// <param name="encrypt">true for encryption, false for decryption</param>
        /// <returns>An <see cref="sbyte[]"/> array with 2  0x0 - 0xf segments (most significant & least significant) bit</returns>
        private sbyte[] MapByteValue2(ref byte inByte, out byte outByte, bool encrypt = true)
        {
            List<sbyte> outSBytes = new List<sbyte>(2);
            sbyte lsbIn = (sbyte)((short)inByte % 16);
            sbyte msbIn = (sbyte)((short)((short)inByte / 16));
            sbyte lsbOut, msbOut;
            if (encrypt)
            {
                lsbOut = MatrixPermutationKey[(int)lsbIn];
                msbOut = MatrixPermutationKey[(int)msbIn];
                outSBytes.Add(lsbOut);
                outSBytes.Add(msbOut);
                outByte = (byte)((short)(((short)msbOut * 16) + ((short)lsbOut)));
            }
            else // if decrypt
            {
                lsbOut = _inverseMatrix[(int)lsbIn];
                msbOut = _inverseMatrix[(int)msbIn];
                outSBytes.Add(lsbOut);
                outSBytes.Add(msbOut);
                outByte = (byte)((short)(((short)msbOut * 16) + ((short)lsbOut)));
            }

            return outSBytes.ToArray();
        }

        #endregion static helpers swap byte and SwapT{T} generic


    }

}
