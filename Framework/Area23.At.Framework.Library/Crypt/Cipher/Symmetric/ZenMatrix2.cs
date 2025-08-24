using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// In  <see cref="ZenMatrix2"/> each half byte in block of 16 bytes is shifted 
    /// least significant bit is mapped via  <see cref="MatrixPermutationKey"/> a 0x10 hex mapping <see cref="PermutationKeyHash"/>for encryption 
    ///     and <see cref="InverseMatrix" /> for decryption,
    /// while most significant bit is mapped via <see cref="InverseMatrix">_inverseMatrix for encryption</see>(!!!) 
    ///     and <see cref="MatrixPermutationKey">MatrixPermutationKey for decryption</see>(!!!)
    ///   
    /// positions of shifted bytes are swapped in a block of 32 hex = 16 <see cref="byte"/>  via a 2nd 0x20 long <see cref="MatrixPermutationKey2"/> (32 hex)
    /// and repositioned via <see cref="InverseMatrix2"/> in case of decryption
    /// 
    /// I have further the idea to use normal or inverse operation, depending on secret key bytes (dynamically.
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

        protected new internal static readonly int[] MagicOrder = {
            0x13, 0x16, 0x1f, 0x06, 0x14, 0x11, 0x1d, 0x0e,
            0x1c, 0x0f, 0x15, 0x1b, 0x1e, 0x02, 0x17, 0x19,
            0x12, 0x04, 0x07, 0x18, 0x0a, 0x1a, 0x05, 0x0c,
            0x03, 0x08, 0x01, 0x09, 0x0b, 0x10, 0x00, 0x0d
        };

        protected internal static readonly sbyte[] MatrixPermutationBase2 = {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
            0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
            0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
            0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f
        };

        protected internal byte[] privateBytes2 = new byte[0x20];

        public sbyte[] MatrixPermutationKey2 { get; protected internal set; }

        protected internal sbyte[] _inverseMatrix2 = new sbyte[0];
        /// <summary>
        /// Inverse ZenMatrix2 
        /// </summary>
        protected internal sbyte[] InverseMatrix2
        {
            get
            {
                if (_inverseMatrix2 == null ||
                    _inverseMatrix2.Length < 0x10 ||
                    (_inverseMatrix2[0] == (sbyte)0x0 && _inverseMatrix2[1] == (sbyte)0x0 && _inverseMatrix2[0xf] == (sbyte)0x0) ||
                    (_inverseMatrix2[0] == (sbyte)0x0 && _inverseMatrix2[1] == (sbyte)0x1 && _inverseMatrix2[0xf] == (sbyte)0xf))
                {
                    _inverseMatrix2 = BuildInverseMatrix2(MatrixPermutationKey2);
                }

                return _inverseMatrix2;
            }
        }

        public HashSet<sbyte> PermutationKeyHash2 { get; protected internal set; }

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
                for (bCnt = 0; bCnt < (2 * BLOCK_SIZE); bCnt++)
                {
                    char chA = shifted[bCnt];
                    int posA = (forEncryption) ? MatrixPermutationKey2[bCnt % 0x20] : InverseMatrix2[bCnt % 0x20];
                    swapped[posA] = chA;
                }
                for (aCnt = 0, bCnt = 0; bCnt < swapped.Length; aCnt++, bCnt += 2)
                {
                    string toByte = string.Concat(swapped[bCnt].ToString(), swapped[bCnt + 1].ToString());
                    byte forProcessed;
                    IFormatProvider provider = new NumberFormatInfo();
                    if (!Byte.TryParse(toByte, System.Globalization.NumberStyles.HexNumber, provider, out forProcessed))
                        forProcessed = (byte)0x0;
                    processed[aCnt] = forProcessed;
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
            for (bCnt = 0; bCnt < (2 * BLOCK_SIZE); bCnt++)
            {
                char chA = shifted[bCnt];
                int posA = (forEncryption) ? MatrixPermutationKey2[bCnt % 0x20] : InverseMatrix2[bCnt % 0x20];
                swapped[posA] = chA;
            }
            for (aCnt = 0, bCnt = 0; bCnt < swapped.Length; aCnt++, bCnt += 2)
            {
                string toByte = string.Concat(swapped[bCnt].ToString(), swapped[bCnt + 1].ToString());
                byte forProcessed;
                IFormatProvider provider = new NumberFormatInfo();
                if (!Byte.TryParse(toByte, System.Globalization.NumberStyles.HexNumber, provider, out forProcessed))
                    forProcessed = (byte)0x0;
                processed[aCnt] = forProcessed;
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
            sbyte sbcnt2 = 0x0;
            MatrixPermutationKey2 = new sbyte[0x20];
            foreach (sbyte s in MatrixPermutationBase2)
            {
                privateBytes2[sbcnt2 % 0x20] = (byte)0x0;
                MatrixPermutationKey2[sbcnt2++] = s;
            }
            PermutationKeyHash2 = new HashSet<sbyte>(MatrixPermutationBase2);
            _inverseMatrix2 = BuildInverseMatrix2(MatrixPermutationKey2);
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
            sbyte sbcnt = 0x0, sbcnt2 = 0x0;
            MatrixPermutationKey = new sbyte[0x10];
            MatrixPermutationKey2 = new sbyte[0x20];

            foreach (sbyte s1 in MatrixPermutationBase)
            {
                privateBytes[sbcnt % 0x10] = (byte)0x0;
                MatrixPermutationKey[sbcnt++ % 0x10] = s1;
            }

            foreach (sbyte s2 in MatrixPermutationBase2)
            {               
                privateBytes2[sbcnt2 % 0x20] = (byte)0x0;
                MatrixPermutationKey2[sbcnt2++ % 0x20] = s2;
            }

            PermutationKeyHash = new HashSet<sbyte>(MatrixPermutationBase);
            PermutationKeyHash2 = new HashSet<sbyte>(MatrixPermutationBase2);
            _inverseMatrix = BuildInverseMatrix(MatrixPermutationKey);
            _inverseMatrix2 = BuildInverseMatrix2(MatrixPermutationKey2);
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
        protected override void ZenMatrixGenWithBytes(byte[] keyBytes2, bool fullSymmetric = false)
        {
            
            base.ZenMatrixGenWithBytes(keyBytes2, fullSymmetric);
            int ba = 0, bb = 0;

            Array.Copy(keyBytes2, 0, privateBytes2, 0, Math.Min(keyBytes2.Length, 0x20));
            Dictionary<sbyte, sbyte> MatrixDict2 = new Dictionary<sbyte, sbyte>();
            PermutationKeyHash2 = new HashSet<sbyte>();

            /* Simplest method to fill deterministic up privateBytes from keyBytes with keyBytes.Length < 32
             * if (keyBytes2.Length < 0x20) {
             *     Array.Copy(keyBytes2, 0, privateBytes2, 0, Math.Min(keyBytes2.Length, 0x20));
             *     for (int i = keyBytes2.Length; i < 0x24; i++)  {
             *         if (i < 0x10)
             *             privateBytes2[i] = (byte)keyBytes2[i % keyBytes2.Length];
             *         else
             *             privateBytes2[i] = (byte)keyBytes2[0x10 - (i - 0x0f)];
             *     }
             * }
             * else {
             *     for (int l = 0, k = keyBytes2.Length - 1; (k >= 0 && l < 0x20); k--, l++) {
             *         privateBytes2[l] = (byte)keyBytes2[k];
             *    }
             * }
             */

            foreach (byte keyByte in new List<byte>(privateBytes2))
            {
                sbyte b = (sbyte)(keyByte % 0x20);
                for (int i = 0; i < 0x20; i++)
                {
                    if (PermutationKeyHash2.Contains(b) || ((int)b) == ba)
                    {
                        if (i < 0x20)
                            b = ((sbyte)((Convert.ToInt32(keyByte) + MagicOrder[i]) % 0x20));
                        if (i >= 0x20)
                            b = ((sbyte)((Convert.ToInt32(keyByte) + i) % 0x20));
                    }
                    else break;
                }

                if (!PermutationKeyHash2.Contains(b))
                {
                    bb = (int)b;
                    if (ba != bb)
                    {
                        if (fullSymmetric)
                        {
                            if (!MatrixDict2.Keys.Contains(b) && !MatrixDict2.Keys.Contains((sbyte)ba))
                            {
                                MatrixDict2.Add((sbyte)ba, (sbyte)bb);
                                MatrixDict2.Add((sbyte)bb, (sbyte)ba);
                            }
                        }

                        PermutationKeyHash2.Add(b);
                        MatrixPermutationKey2 = MatrixPermutationKey2.SwapTPositions<sbyte>(ba, bb);
                        ba++;
                    }
                }
            }

            if (fullSymmetric)
            {
                #region fullSymmetric => InverseMatrix2 = MatrixPermutationKey2;
                if (MatrixDict2.Count < 0x1f)
                {
                    for (int k = 0; k < 0x20; k++)
                    {
                        if (!MatrixDict2.Keys.Contains((sbyte)k))
                        {
                            for (int l = 0x1f; l >= 0; l--)
                            {
                                if (!MatrixDict2.Values.Contains((sbyte)l))
                                {
                                    MatrixDict2.Add((sbyte)k, (sbyte)l);
                                    if (!MatrixDict2.Keys.Contains((sbyte)l))
                                        MatrixDict2.Add((sbyte)l, (sbyte)k);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (MatrixDict2.Count == 0x20)
                {
                    sbyte bKey, bValue;
                    PermutationKeyHash2.Clear();
                    for (int n = 0; n < 0x10; n++)
                    {
                        bKey = (sbyte)n;
                        bValue = (sbyte)MatrixDict2[bKey];
                        PermutationKeyHash2.Add(bValue);
                        MatrixPermutationKey2[(int)bKey] = bValue;
                        MatrixPermutationKey2[(int)bValue] = bKey;
                    }
                }
                #endregion fullSymmetric => InverseMatrix2 = MatrixPermutationKey2;

                _inverseMatrix2 = MatrixPermutationKey2;
            }
            else
            {
                #region bugfix for missing permutations                
                sbyte[] strikeBytes2 = {  (sbyte)0x00, (sbyte)0x01, (sbyte)0x02, (sbyte)0x03, (sbyte)0x04, (sbyte)0x05, (sbyte)0x06, (sbyte)0x07,
                                        (sbyte)0x08, (sbyte)0x09, (sbyte)0x0a, (sbyte)0x0b, (sbyte)0x0c, (sbyte)0x0d, (sbyte)0x0e, (sbyte)0x0f,
                                        (sbyte)0x10, (sbyte)0x11, (sbyte)0x12, (sbyte)0x13, (sbyte)0x14, (sbyte)0x15, (sbyte)0x16, (sbyte)0x17,
                                        (sbyte)0x18, (sbyte)0x19, (sbyte)0x1a, (sbyte)0x1b, (sbyte)0x1c, (sbyte)0x1d, (sbyte)0x1e, (sbyte)0x1f };
                HashSet<sbyte> strikeList2 = new HashSet<sbyte>(strikeBytes2);

                for (int i = 0; i < 0x20; i++)
                {
                    if ((PermutationKeyHash2.Count <= i) && strikeList2.Count > 0)
                        PermutationKeyHash2.Add((sbyte)strikeList2.ElementAt(0));

                    sbyte inByte2 = (sbyte)i;
                    if ((int)PermutationKeyHash2.ElementAt(i) != i)
                    {
                        inByte2 = PermutationKeyHash2.ElementAt(i);
                        MatrixPermutationKey2[i] = inByte2;
                    }
                    if (strikeList2.Contains(inByte2))
                        strikeList2.Remove(inByte2);
                }

                _inverseMatrix2 = BuildInverseMatrix2(MatrixPermutationKey2);
                #endregion bugfix for missing permutations
            }

            string perm2 = string.Empty, kbs2 = string.Empty;

            for (int j = 0; j < 0x20; j++)
                perm2 += MatrixPermutationKey2[j].ToString("x2");
            for (int j = 0; j < keyBytes2.Length; j++)
                kbs2 += keyBytes2[j].ToString("x2");

            Area23Log.LogOriginMsg("ZenMatrix2", perm2 + " KeyBytes = " + kbs2);
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
                for (bCnt = 0; bCnt < (2 * len); bCnt++)
                {
                    char chA = shifted[bCnt];
                    int posA = (forEncryption) ? MatrixPermutationKey2[bCnt % 0x20] : InverseMatrix2[bCnt % 0x20];                    
                    swapped[posA] = chA;                    
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
        /// BuildInverseMatrix2, builds the determinant decryption matrix for sbyte[32] encryption matrix
        /// </summary>
        /// <param name="matrix">sbyte[16] encryption matrix</param>
        /// <returns><see cref="sbyte[]">sbyte[16]</see> decryption matrix (determinante)</returns>
        internal static sbyte[] BuildInverseMatrix2(sbyte[] matrix2, int size2 = 0x20)
        {
            if (matrix2 != null && matrix2.Length == size2)
            {
                sbyte[] inversed2 = new sbyte[size2];
                for (int m2 = 0; m2 < size2; m2++)
                {
                    sbyte sb2 = matrix2[m2];
                    inversed2[(int)sb2] = (sbyte)m2;
                }
                return inversed2;
            }
            throw new ApplicationException($"sbyte[] matrix2 is null or matrix2.Length != {size2}");
        }

        /// <summary>        
        /// In <see cref="ZenMatrix2"/> least significant bit is mapped via  <see cref="MatrixPermutationKey"/> for encryption 
        ///     and <see cref="_inverseMatrix" /> for decryption,
        /// while most significant bit is mapped via <see cref="_inverseMatrix">_inverseMatrix for encryption</see>(!!!) 
        ///     and <see cref="MatrixPermutationKey">MatrixPermutationKey for decryption</see>
        /// </summary>
        /// <param name="inByte"><see cref="byte"/> in byte to map</param>
        /// <param name="outByte"><see cref=byte"/> mapped out byte</param>
        /// <param name="encrypt">true for encryption, false for decryption</param>
        /// <returns>An <see cref="sbyte[]"/> array with 2  0x0 - 0xf segments (most significant & least significant) bit</returns>
        protected internal override sbyte[] MapByteValue(ref byte inByte, out byte outByte, bool encrypt = true)
        {
            List<sbyte> outSBytes = new List<sbyte>(2);
            sbyte lsbIn = (sbyte)((short)inByte % 0x10);
            sbyte msbIn = (sbyte)((short)((short)inByte / 0x10));
            sbyte lsbOut, msbOut;
            if (encrypt)
            {
                lsbOut = MatrixPermutationKey[(int)lsbIn];
                msbOut = _inverseMatrix[(int)msbIn];
                outSBytes.Add(lsbOut);
                outSBytes.Add(msbOut);
                outByte = (byte)((short)(((short)msbOut * 0x10) + ((short)lsbOut)));
            }
            else // if decrypt
            {
                lsbOut = _inverseMatrix[(int)lsbIn];
                msbOut = MatrixPermutationKey[(int)msbIn];
                outSBytes.Add(lsbOut);
                outSBytes.Add(msbOut);
                outByte = (byte)((short)(((short)msbOut * 0x10) + ((short)lsbOut)));
            }

            return outSBytes.ToArray();
        }

        #endregion static helpers swap byte and SwapT{T} generic


    }

}
