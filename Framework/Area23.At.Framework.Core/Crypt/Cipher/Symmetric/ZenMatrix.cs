using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Area23.At.Framework.Core.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// Simple sbyte reduced to 0x0 .. 0xf symmetric cipher mapping matrix,
    /// maybe already invented, but created by zen@area23.at (Heinrich Elsigan)
    /// </summary>
    public class ZenMatrix : IBlockCipher
    {

        #region fields

        private const string SYMMCIPHERALGONAME = "ZenMatrix";
        private const int BLOCK_SIZE = 0x10;
        private bool initialised = false;

        /// <summary>
        /// MatrixPermutationBase is a Permutation Matrix where every value will mapped to itself
        /// ( 
        ///     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
        ///     0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
        ///     0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
        ///     0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
        ///     0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
        ///     0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
        ///     0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 
        ///     0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 
        ///     0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 
        /// </summary>
        protected internal static readonly sbyte[] MatrixPermutationBase = {
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7,
            0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
        };

        /// <summary>
        /// MagicOrder is a byte[], that helps filling up keybytes up 16 bytes deterministic, when keybytes < 16
        /// </summary>
        protected internal static readonly int[] MagicOrder = {
            0x8,    0x3,    0x1,    0xe,
            0x9,    0xf,    0x5,    0xc,
            0x4,    0xd,    0xa,    0x7,
            0xb,    0x2,    0x0,    0x6
        };

        protected internal byte[] privateBytes = new byte[0x10];

        #endregion fields

        #region Properties

        /// <summary>
        /// abstraction of a 0x10 => 0x10 matrix, for example
        /// if MatrixPermutationKey = { 0x8, 0x3, 0x1, 0xe, 0x9, 0xf, 0x5, 0xc, 0x4, 0xd, 0xa, 0x7, 0xb, 0x2, 0x0, 0x6 }
        ///            
        ///     0  1  2  3  4  5  6  7  8  9  a  b  c  d  e  f
        ///     
        /// (   0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0,             0
        ///     0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,             1
        ///     0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,             2
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0,             3
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,             4
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,             5
        ///     0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,             6
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0,             7
        ///     0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,             8
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0,             9
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0,             a
        ///     0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,             b
        ///     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0,             c
        ///     0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,             d
        ///     1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,             e
        ///     0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0,             f
        ///     
        /// then 
        /// value 0x0 will be mapped to 0x8      0 =>  8
        /// value 0x1 will be mapped to 0x3      1 =>  3
        /// value 0x2 will be mapped to 0x1      2 =>  1
        /// value 0x3 will be mapped to 0xe      3 => 14
        /// ... and         
        /// value 0xe will be mapped to 0x0     14 =>  0
        /// value 0xf will be mapped to 0x6     15 =>  6         
        /// 
        /// a full symmetric <see cref="MatrixPermutationKey"/> would like:
        ///     0    1    2    3    4    5    6    7    8    9    a    b    c    d    e    f
        ///  { 0x8, 0x3, 0xe, 0x1, 0x9, 0xf, 0xb, 0xd, 0x0, 0x4, 0xc, 0x6, 0xa, 0x7, 0x2, 0x5 }
        /// 
        ///  that means, that
        ///  value 0x0 will be mapped to 0x8 <=> 0x8 will be mapped back to 0x0      0 => 8 &&  8 => 0
        ///  value 0x1 will be mapped to 0x3 <=> 0x3 will be mapped back to 0x1      1 => 3 &&  3 => 1
        ///  value 0x2 will be mapped to 0xe <=> 0xe will be mapped back to 0x2      2 => e &&  e => 2
        ///  value 0x3 is already mapped back to 0x1 !                               3 => 1
        ///  ...
        ///  value 0xf is   already mapped back to 0x5 !                             f => 5
        /// </summary>
        public sbyte[] MatrixPermutationKey { get; protected internal set; }

        protected internal sbyte[] _inverseMatrix = new sbyte[0];
        /// <summary>
        /// Inverse Matrix 
        /// </summary>
        protected internal sbyte[] InverseMatrix
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


        /// <summary>
        /// PermutationKeyHash is same as <see cref="MatrixPermutationKey"/>
        /// Advantage of <see cref="HashSet{sbyte}"/> is, that no duplicated values can be inside
        /// </summary>
        public HashSet<sbyte> PermutationKeyHash { get; protected internal set; }


        #endregion Properties
        #region IBlockCipher interface

        public string AlgorithmName => SYMMCIPHERALGONAME;

        public int GetBlockSize() => BLOCK_SIZE;


        public void Init(bool forEncryption, ICipherParameters parameters)
        {
            if (!(parameters is KeyParameter))
                throw new ArgumentException("only simple KeyParameter expected.");

            ZenMatrixGenWithBytes(privateBytes, !forEncryption);
            initialised = true;
        }


        /// <summary>
        /// Processes one BLOCK with BLOCK_SIZE <see cref="BLOCK_SIZE"/>
        /// </summary>
        /// <param name="inBuf"></param>
        /// <param name="inOff"></param>
        /// <param name="outBuf"></param>
        /// <param name="outOff"></param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException"></exception>
        public int ProcessBlock(byte[] inBuf, int inOff, byte[] outBuf, int outOff)
        {
            int len = BLOCK_SIZE;
            int aCnt = 0, bCnt = 0;
            byte[] processedEncrypted = null;
            if (inOff >= inBuf.Length || inOff + len > inBuf.Length)
                throw new InvalidDataException($"Cannot process next {BLOCK_SIZE} bytes, because inOff ({inOff}) + BLOCK_SIZE ({BLOCK_SIZE}) > inBuf.Length ({inBuf.Length})");
            if (outOff >= outBuf.Length || outOff + len > outBuf.Length)
                throw new InvalidDataException($"Cannot process next {BLOCK_SIZE} bytes, because inOff ({outOff}) + BLOCK_SIZE ({BLOCK_SIZE}) > outBuf.Length ({outBuf.Length})");

            if (inOff < inBuf.Length && inOff + len <= inBuf.Length && outOff < outBuf.Length && outOff + len <= outBuf.Length)
            {
                processedEncrypted = new byte[len];
                for (aCnt = 0, bCnt = inOff; bCnt < inOff + len; aCnt++, bCnt++)
                {
                    byte b = inBuf[bCnt];
                    MapByteValue(ref b, out byte mapEncryptB, true);
                    sbyte sm = MatrixPermutationKey[aCnt];
                    outBuf[outOff + (int)sm] = mapEncryptB;
                }
            }

            return BLOCK_SIZE;
        }

        public int ProcessBlock(ReadOnlySpan<byte> input, Span<byte> output)
        {
            throw new NotImplementedException();
        }

        #endregion IBlockCipher interface


        #region ctor_init_gen_reverse

        /// <summary>
        /// public constructor
        /// </summary>
        public ZenMatrix()
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
        public ZenMatrix(string secretKey = "", string hashIV = "", bool fullSymmetric = false) : this()
        {
            secretKey = string.IsNullOrEmpty(secretKey) ? Constants.AUTHOR_EMAIL : secretKey;
            hashIV = string.IsNullOrEmpty(hashIV) ? Constants.AREA23_EMAIL : hashIV;
            byte[] keyBytes = CryptHelper.GetUserKeyBytes(secretKey, hashIV, 16);

            ZenMatrixGenWithBytes(keyBytes, fullSymmetric);
        }


        /// <summary>
        /// initializes a <see cref="ZenMatrix"/> with an array of key bytes
        /// </summary>
        /// <param name="keyBytes">array of key bytes</param>
        /// <param name="fullSymmetric">
        /// fullSymmetric means that zen matrix is it's inverse element 
        /// and decrypts back to plain text, when encrypting twice or ²</param> 
        public ZenMatrix(byte[] keyBytes, bool fullSymmetric = false) : this()
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
                MatrixPermutationKey[sbcnt++] = s;
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
        protected internal void ZenMatrixGenWithBytes(byte[] keyBytes, bool fullSymmetric = false)
        {
            if ((keyBytes == null || keyBytes.Length < 4))
                throw new ApplicationException("byte[] keyBytes is null or keyBytes.Length < 4");

            // InitMatrixSymChiffer();

            int ba = 0, bb = 0;
            Array.Copy(keyBytes, 0, privateBytes, 0, Math.Min(keyBytes.Length, 0x10));

            PermutationKeyHash = new HashSet<sbyte>();

            // MatrixDict is only needed, when (fullSymmetric == true)
            Dictionary<sbyte, sbyte> MatrixDict = new Dictionary<sbyte, sbyte>();

            /* Simplest method to fill deterministic up privateBytes from keyBytes with keyBytes.Length < 16
             * for (int i = keyBytes.Length; i < 0x10; i++)
             * {
             *      if (i < 0x08)
             *          privateBytes[i] = (byte)keyBytes[i % keyBytes.Length];
             *      else
             *          privateBytes[i] = (byte)keyBytes[0x08 - (i - 0x07)];
             * }
             */

            foreach (byte keyByte in new List<byte>(privateBytes))
            {
                sbyte b = (sbyte)(keyByte % 0x10);
                for (int i = 0; i < 32; i++)
                {
                    if (PermutationKeyHash.Contains(b) || ((int)b) == ba)
                    {
                        if (i < 0x10)
                            b = ((sbyte)((Convert.ToInt32(keyByte) + MagicOrder[i]) % 0x10));
                        if (i >= 0x10)
                            b = ((sbyte)((Convert.ToInt32(keyByte) + i) % 0x10));
                    }
                    else break;
                }

                if (!PermutationKeyHash.Contains(b))
                {
                    bb = (int)b;
                    if (ba != bb)
                    {
                        if (fullSymmetric)
                        {
                            if (!MatrixDict.Keys.Contains(b) && !MatrixDict.Keys.Contains((sbyte)ba))
                            {
                                MatrixDict.Add((sbyte)ba, (sbyte)bb);
                                MatrixDict.Add((sbyte)bb, (sbyte)ba);
                            }
                        }

                        PermutationKeyHash.Add(b);
                        MatrixPermutationKey = MatrixPermutationKey.SwapTPositions<sbyte>(ba, bb);
                        ba++;
                    }
                }
            }

            if (fullSymmetric)
            {
                #region fullSymmetric => InverseMatrix = MatrixPermutationKey;
                if (MatrixDict.Count < 0x0f)
                {
                    for (int k = 0; k < 0x10; k++)
                    {
                        if (!MatrixDict.Keys.Contains((sbyte)k))
                        {
                            for (int l = 0x0f; l >= 0; l--)
                            {
                                if (!MatrixDict.Values.Contains((sbyte)l))
                                {
                                    MatrixDict.Add((sbyte)k, (sbyte)l);
                                    if (!MatrixDict.Keys.Contains((sbyte)l))
                                        MatrixDict.Add((sbyte)l, (sbyte)k);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (MatrixDict.Count == 0x10)
                {
                    sbyte bKey, bValue;
                    PermutationKeyHash.Clear();
                    for (int n = 0; n < 0x10; n++)
                    {
                        bKey = (sbyte)n;
                        bValue = (sbyte)MatrixDict[bKey];
                        PermutationKeyHash.Add(bValue);
                        MatrixPermutationKey[(int)bKey] = bValue;
                        MatrixPermutationKey[(int)bValue] = bKey;
                    }
                }
                #endregion fullSymmetric => InverseMatrix = MatrixPermutationKey;

                _inverseMatrix = MatrixPermutationKey;
            }
            else
            {
                for (int i = 0; i < 0x10; i++)
                {
                    if ((int)PermutationKeyHash.ElementAt(i) != i)
                    {
                        MatrixPermutationKey[i] = PermutationKeyHash.ElementAt(i);
                    }
                }

                _inverseMatrix = BuildInverseMatrix(MatrixPermutationKey);
            }

            string perm = string.Empty, kbs = string.Empty;

            for (int j = 0; j < 0x10; j++)
                perm += MatrixPermutationKey[j].ToString("x1");
            for (int j = 0; j < keyBytes.Length; j++)
                kbs += keyBytes[j].ToString("x2");


            initialised = true;
            Area23Log.LogOriginMsg("ZenMatrix", perm + " KeyBytes = " + kbs);
        }


        #endregion ctor_init_gen_reverse

        #region ProcessEncryptDecryptBytes

        /// <summary>
        /// ProcessEncryptBytes, processes the next len=16 bytes to encrypt, starting at offSet
        /// </summary>
        /// <param name="inBytes">in bytes array to encrypt</param>
        /// <param name="offSet">starting offSet</param>
        /// <param name="len">len of byte block (default 16)</param>
        /// <returns>byte[len] (default: 16) segment of encrypted bytes</returns>
        protected internal virtual byte[] ProcessEncryptBytes(byte[] inBytes, int offSet = 0, int len = 0x10)
        {
            int aCnt = 0, bCnt = 0;
            byte[] processedEncrypted = null;
            if (offSet < inBytes.Length && offSet + len <= inBytes.Length)
            {
                processedEncrypted = new byte[len];
                for (aCnt = 0, bCnt = offSet; bCnt < offSet + len; aCnt++, bCnt++)
                {
                    byte b = inBytes[bCnt];
                    MapByteValue(ref b, out byte mapEncryptB, true);
                    sbyte sm = MatrixPermutationKey[aCnt];
                    processedEncrypted[(int)sm] = mapEncryptB;
                }
            }
            return processedEncrypted ?? new byte[0];
        }

        /// <summary>
        /// ProcessDecryptBytes  processes the next len=16 bytes to decrypt, starting at offSet
        /// </summary>
        /// <param name="inBytesEncrypted">encrypted bytes array to deccrypt</param>
        /// <param name="offSet">starting offSet</param>
        /// <param name="len">len of byte block (default 16)</param>
        /// <returns>byte[len] (default: 16) segment of decrypted bytes</returns>
        protected internal virtual byte[] ProcessDecryptBytes(byte[] inBytesEncrypted, int offSet = 0, int len = 0x10)
        {
            int aCnt = 0, bCnt = 0;
            byte[] processedDecrypted = null;
            if (offSet < inBytesEncrypted.Length && offSet + len <= inBytesEncrypted.Length)
            {
                processedDecrypted = new byte[len];
                for (aCnt = 0, bCnt = offSet; bCnt < offSet + len; aCnt++, bCnt++)
                {
                    byte b = inBytesEncrypted[bCnt];
                    MapByteValue(ref b, out byte mapDecryptB, false);
                    sbyte sm = InverseMatrix[aCnt];
                    processedDecrypted[(int)sm] = mapDecryptB;
                }
            }
            return processedDecrypted ?? new byte[0];
        }

        #endregion ProcessEncryptDecryptBytes

        #region encrypt decrypt

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

            int dlen = pdata.Length;                        // length of data bytes
            int oSize = dlen + (0x10 - (dlen % 0x10));      // oSize is rounded up to next number % 16 == 0
            long olen = ((long)(oSize / 0x10)) * 0x10;      // olen is (long)oSize
            byte[] rndbuf = new byte[olen - dlen];          // random padding buffer 
            byte[] obytes = new byte[olen];                 // out bytes with random padding bytes at end            

            Random rnd = new Random(dlen);
            rnd.NextBytes(rndbuf);

            for (int i = 0, j = 0; i < olen; i++)
            {
                // obytes[i] = (i < dlen) ? data[i] : (i == dlen || i == (olen - 1)) ? obytes[i] = (byte)0x0 : rndbuf[j++];
                if (i < dlen)
                    obytes[i] = pdata[i];                    // copy full data to obytes
                else if (i == dlen)
                    obytes[i] = (byte)0x0;                  // write 0x0 at end of data bytes
                else if (i > dlen)
                    obytes[i] = rndbuf[j++];                // fill rest with random hash padding 

                if (i == (olen - 1))
                    obytes[i] = (byte)0x0;                  // terminate end of obytes with 0x0                                    
            }


            List<byte> encryptedBytes = new List<byte>();
            for (int i = 0; i < obytes.Length; i += 0x10)
            {
                foreach (byte pb in ProcessEncryptBytes(obytes, i, 0x10))
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

            int eclen = ecdata.Length;
            int ecSize = (eclen % 0x10 == 0) ? eclen : (eclen + (0x10 - (eclen % 0x10)));
            if (ecSize > eclen) {; } // something went wrong                

            List<byte> outBytes = new List<byte>();
            for (int pc = 0; pc < ecdata.Length; pc += 16)
            {
                foreach (byte rb in ProcessDecryptBytes(ecdata, pc, 16))
                {
                    outBytes.Add(rb);
                }
            }

            int olen = outBytes.Count;
            int dlen = olen;
            bool first0 = false, last0 = (outBytes.ElementAt(olen - 1) == (byte)0x0);

            for (dlen = olen; dlen > 0 && !first0; dlen--)
            {
                if (dlen < (olen - 2) && (outBytes.ElementAt(dlen - 1) == (byte)0x0))
                {
                    first0 = true;
                    break;
                }
            }
            byte[] obytes = (dlen > 1) ? new byte[dlen] : new byte[olen];
            Array.Copy(outBytes.ToArray(), 0, obytes, 0, obytes.Length);

            return obytes;
        }

        #region encrypt decrypt variants

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="inPlainString">plain text string</param>
        /// <returns>Base64 encoded encrypted byte[]</returns>
        public virtual byte[] EncryptTextToBytes(string plaintext)
        {
            byte[] ecdata = Encrypt(EnDeCodeHelper.GetBytes(plaintext));
            return ecdata;
        }

        /// <summary>
        /// EncryptTextToEncoded Encrypts a string to a raw data string or 
        /// hex16, base32, hex32, uu or base64 encoded string
        /// </summary>
        /// <param name="plaintext">string to encrypt</param>        
        /// <param name="encType"><see cref="EncodingType"/>, default: <see cref="EncodingType.Base64/></param>
        /// <returns>a raw string or a hex16, base32, hex32, uu or base64 encoded string</returns>
        public virtual string EncryptTextToEncoded(string plaintext, EncodingType encType = EncodingType.Base64)
        {
            byte[] ecdata = EncryptTextToBytes(plaintext);
            return EncryptBytesToEncoded(ecdata, encType);
        }

        /// <summary>
        /// EncryptBytesToEncoded Encrypts plain data bytes to a raw data string or 
        /// a hex16, base32, hex32, uu or base64 encoded string
        /// </summary>
        /// <param name="pdata">plain data bytes</param>
        /// <param name="encType"><see cref="EncodingType"/>, default: <see cref="EncodingType.Base64/></param>
        /// <returns>a raw string or a hex16, base32, hex32, uu or base64 encoded string</returns>
        public virtual string EncryptBytesToEncoded(byte[] pdata, EncodingType encType = EncodingType.Base64)
        {
            byte[] ecdata = Encrypt(pdata);
            switch (encType)
            {
                case EncodingType.Null:
                case EncodingType.None:
                    return RawString.ToRawString(ecdata);
                case EncodingType.Hex16:
                    return Hex16.ToHex16(ecdata);
                case EncodingType.Base32:
                    return Base32.ToBase32(ecdata);
                case EncodingType.Hex32:
                    return Hex32.ToHex32(ecdata);
                case EncodingType.Uu:
                    return Uu.Encode(ecdata);
                case EncodingType.Base64:
                default:
                    return Base64.ToBase64(ecdata);
            }

            // return new byte[0];
        }

        /// <summary>
        /// Decrypts a string, that is truely a base64 encoded encrypted byte[]
        /// </summary>
        /// <param name="inCryptString">base64 encoded string from encrypted byte[]</param>
        /// <returns>plain text string (decrypted)</returns>
        public virtual string DecryptTextFromBytes(byte[] ecdata)
        {
            byte[] plaindata = Decrypt(ecdata);
            string plaintext = EnDeCodeHelper.GetString(plaindata);
            return plaintext;
        }

        /// <summary>
        /// DecryptTextFromEncoded dectypts a encoded and encrypted string to a plain string
        /// </summary>
        /// <param name="encodedStr">a encrypted and encoded string</param>
        /// <param name="encType"><see cref="EncodingType"/>, default: <see cref="EncodingType.Base64/></param>
        /// <returns>plain text string</returns>
        public virtual string DecryptTextFromEncoded(string encodedStr, EncodingType encType = EncodingType.Base64)
        {
            byte[] ecdata = DecryptBytesFromEncoded(encodedStr, encType);
            string plaintext = DecryptTextFromBytes(ecdata);
            return plaintext;
        }

        /// <summary>
        /// DecryptBytesFromEncoded
        /// </summary>
        /// <param name="encodedStr"></param>
        /// <param name="encType"><see cref="EncodingType"/>, default: <see cref="EncodingType.Base64/></param>
        /// <returns>plain data bytes</returns>
        public virtual byte[] DecryptBytesFromEncoded(string encodedStr, EncodingType encType = EncodingType.Base64)
        {
            byte[] ecdata = new byte[0];
            switch (encType)
            {
                case EncodingType.Null:
                case EncodingType.None:
                    ecdata = RawString.FromRawString(encodedStr);
                    break;
                case EncodingType.Hex16:
                    ecdata = Hex16.FromHex16(encodedStr);
                    break;
                case EncodingType.Base32:
                    ecdata = Base32.FromBase32(encodedStr);
                    break;
                case EncodingType.Hex32:
                    ecdata = Hex32.FromHex32(encodedStr);
                    break;
                case EncodingType.Uu:
                    ecdata = Uu.Decode(encodedStr);
                    break;
                case EncodingType.Base64:
                default:
                    ecdata = Base64.FromBase64(encodedStr);
                    break;
            }

            return ecdata;
        }

        #endregion encrypt decrypt variants

        #endregion encrypt decrypt


        #region static helpers swap byte and SwapT{T} generic 

        /// <summary>
        /// BuildInverseMatrix, builds the determinant decryption matrix for sbyte[16] encryption matrix
        /// </summary>
        /// <param name="matrix">sbyte[16] encryption matrix</param>
        /// <returns><see cref="sbyte[]">sbyte[16]</see> decryption matrix (determinante)</returns>
        internal static sbyte[] BuildInverseMatrix(sbyte[] matrix, int size = 0x10)
        {
            if (matrix != null && matrix.Length == size)
            {
                sbyte[] inverseM = new sbyte[size];
                for (int m = 0; m < size; m++)
                {
                    sbyte sm = matrix[m];
                    inverseM[(int)sm] = (sbyte)m;
                }
                return inverseM;
            }
            throw new ApplicationException($"sbyte[] matrix is null or matrix.Length != {size}");
        }

        /// <summary>
        /// MapByteValue splits a byte in 2 0x0 - 0xf segments and map both trough <see cref="MatrixPermutationKey"/> in case of encrypt,
        /// through <see cref="InverseMatrix"/> in case of decryption.
        /// </summary>
        /// <param name="inByte"><see cref="byte"/> in byte to map</param>
        /// <param name="outByte"><see cref=byte"/> mapped out byte</param>
        /// <param name="encrypt">true for encryption, false for decryption</param>
        /// <returns>An <see cref="sbyte[]"/> array with 2  0x0 - 0xf segments (most significant & least significant) bit</returns>
        protected internal sbyte[] MapByteValue(ref byte inByte, out byte outByte, bool encrypt = true)
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

        internal static T[] SwapT<T>(ref T t0, ref T t1)
        {
            T[] tt = new T[2];
            tt[0] = t0;
            tt[1] = t1;
            t0 = tt[1];
            t1 = tt[0];

            return tt;
        }


        #endregion static helpers swap byte and SwapT{T} generic

    }

}
