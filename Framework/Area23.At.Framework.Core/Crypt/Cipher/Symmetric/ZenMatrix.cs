using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace Area23.At.Framework.Core.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// <see cref="ZenMatrix"/>, a very simple symmetric block cipher
    /// hex shifting and position swapping reduced to 0x0 .. 0xf mapping matrix
    /// Implements <see cref="Org.BouncyCastle.Crypto.IBlockCipher">Org.BouncyCastle.Crypto.IBlockCipher</see>
    ///
    /// probably already invented, but created by zen@area23.at (Heinrich Elsigan)
    /// </summary>
    public class ZenMatrix : IBlockCipher
    {

        #region fields

        private const string SYMMCIPHERALGONAME = "ZenMatrix";
        protected internal const int ZEN_SIZE = 0x10;
        protected internal static int BLOCK_SIZE = 0x100;
        protected internal static readonly int[] BLOCK_SIZES = { 0x10, 0x20, 0x40, 0x80, 0xc0, 0x100, 0x400, 0x800, 0x1000 };
        protected internal bool initialised = false;
        protected internal bool forEncryption;

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
            if (!(parameters is KeyParameter) && !(parameters is ParametersWithIV))
                throw new ArgumentException("only KeyParameter or ParametersWithIV expected.", "parameters");

            if (parameters is KeyParameter)
            {
                this.privateBytes = ((KeyParameter)parameters).GetKey();
            }
            if (parameters is ParametersWithIV)
            {
                byte[] bKey = new byte[0], bIv = ((ParametersWithIV)parameters).GetIV();
                if (((ParametersWithIV)parameters).Parameters is KeyParameter)
                {
                    bKey = ((KeyParameter)(((ParametersWithIV)parameters).Parameters)).GetKey();
                }
                bKey = bKey ?? new byte[0];
                bIv = bIv ?? new byte[0];
                if (bKey.Length == 0 && bIv.Length == 0)
                    throw new ArgumentNullException("parameters", "KeyParameter and/or ParametersWithIV contain a null or empty key or iv.");

                this.privateBytes = bKey.TarBytes(bIv);
            }

            this.forEncryption = forEncryption;

            ZenMatrixGenWithBytes(privateBytes, false);
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
            if (privateBytes == null)
                throw new InvalidOperationException($"{SYMMCIPHERALGONAME} engine not initialised");

            // int len = BLOCK_SIZE;
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

                for (aCnt = 0, bCnt = 0; aCnt < BLOCK_SIZE; aCnt++)
                {
                    byte b = inOffBuf[aCnt];
                    MapByteValue(ref b, out byte mappedByte, forEncryption);
                    sbyte sm = forEncryption ? MatrixPermutationKey[aCnt % 0x10] : InverseMatrix[aCnt % 0x10];
                    int pos = bCnt + ((int)sm) % 0x10;
                    processed[pos] = mappedByte;
                    if (aCnt != 0 && aCnt % 0x10 == 0)
                        bCnt++;
                }

                // byte[] outBytes = processed;
                // if (!forEncryption)
                //    outBytes = PadBuffer(processed);
                // Array.Copy(outBytes, 0, outBuf, outOff, BLOCK_SIZE);

                Array.Copy(processed, 0, outBuf, outOff, BLOCK_SIZE);

                return BLOCK_SIZE;
            }

            return 0;
        }

        public int ProcessBlock(ReadOnlySpan<byte> input, Span<byte> output)
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

            for (aCnt = 0, bCnt = 0; aCnt < BLOCK_SIZE; aCnt++)
            {
                byte b = buffer[bCnt];
                MapByteValue(ref b, out byte mappedByte, forEncryption);
                sbyte sm = forEncryption ? MatrixPermutationKey[aCnt % 0x10] : InverseMatrix[aCnt % 0x10];
                int pos = bCnt + ((int)sm) % 0x10;
                processed[pos] = mappedByte;
                if (aCnt != 0 && aCnt % 0x10 == 0)
                    bCnt++;
            }

            // byte[] outBytes = processed;
            // if (!forEncryption)                             // trim padding buffer from decrypted output
            //     outBytes = PadBuffer(processed);
            // output = new Span<byte>(outBytes);

            output = new Span<byte>(processed);

            return BLOCK_SIZE;
        }

        #endregion IBlockCipher interface


        #region ctor_init_gen_reverse

        /// <summary>
        /// public constructor
        /// </summary>
        public ZenMatrix(int bs = 0x100)
        {
            for (int i = 0; i < BLOCK_SIZES.Length; i++)
            {
                if (bs == BLOCK_SIZES[i])
                    BLOCK_SIZE = i;
            }
            sbyte sbcnt = 0x0;
            MatrixPermutationKey = new sbyte[ZEN_SIZE];
            foreach (sbyte s in MatrixPermutationBase)
            {
                privateBytes[sbcnt % ZEN_SIZE] = (byte)0x0;
                MatrixPermutationKey[sbcnt++] = s;
            }
            PermutationKeyHash = new HashSet<sbyte>(MatrixPermutationBase);
            _inverseMatrix = BuildInverseMatrix(MatrixPermutationKey);
        }

        public ZenMatrix(string secretKey = "", KeyHash keyHash = KeyHash.Hex, bool fullSymmetric = false) : this(BLOCK_SIZE)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("secretKey");

            string hashIV = keyHash.Hash(secretKey);
            byte[] keyBytes = CryptHelper.GetUserKeyBytes(secretKey, hashIV, 0x10);

            ZenMatrixGenWithBytes(keyBytes, fullSymmetric);
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
        public ZenMatrix(string secretKey = "", string hashIV = "", bool fullSymmetric = false, KeyHash keyHash = KeyHash.Hex) : this(BLOCK_SIZE)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException("secretKey");

            hashIV = string.IsNullOrEmpty(hashIV) ? keyHash.Hash(secretKey) : hashIV;
            byte[] keyBytes = CryptHelper.GetUserKeyBytes(secretKey, hashIV, 0x10);

            ZenMatrixGenWithBytes(keyBytes, fullSymmetric);
        }


        /// <summary>
        /// initializes a <see cref="ZenMatrix"/> with an array of key bytes
        /// </summary>
        /// <param name="keyBytes">array of key bytes</param>
        /// <param name="fullSymmetric">
        /// fullSymmetric means that zen matrix is it's inverse element 
        /// and decrypts back to plain text, when encrypting twice or ²</param> 
        public ZenMatrix(byte[] keyBytes, bool fullSymmetric = false) : this(BLOCK_SIZE)
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
        protected virtual void ZenMatrixGenWithBytes(byte[] keyBytes, bool fullSymmetric = false)
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
                for (int i = 0; i < 0x20; i++)
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
                #region bugfix for missing permutations                
                sbyte[] strikeBytes = {  (sbyte)0x0, (sbyte)0x1, (sbyte)0x2, (sbyte)0x3, (sbyte)0x4, (sbyte)0x5, (sbyte)0x6, (sbyte)0x7,
                                        (sbyte)0x8, (sbyte)0x9, (sbyte)0xa, (sbyte)0xb, (sbyte)0xc, (sbyte)0xd, (sbyte)0xe, (sbyte)0xf  };
                HashSet<sbyte> strikeList = new HashSet<sbyte>(strikeBytes);

                for (int i = 0; i < 0x10; i++)
                {
                    if ((PermutationKeyHash.Count <= i) && strikeList.Count > 0)
                        PermutationKeyHash.Add((sbyte)strikeList.ElementAt(0));

                    sbyte inByte = (sbyte)i;
                    if ((int)PermutationKeyHash.ElementAt(i) != i)
                    {
                        inByte = PermutationKeyHash.ElementAt(i);
                        MatrixPermutationKey[i] = inByte;
                    }
                    if (strikeList.Contains(inByte))
                        strikeList.Remove(inByte);
                }

                _inverseMatrix = BuildInverseMatrix(MatrixPermutationKey);
                #endregion bugfix for missing permutations
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
        /// ProcessBytes processes bytes for encryption or decryption depending on <see cref="forEncryption"/>
        ///     processes the next len=16 bytes to encrypt, starting at offSet
        ///     or processes the next len=16 bytes to decrypt, starting at offSet
        /// </summary>
        /// <param name="inBytes">in bytes array to encrypt</param>
        /// <param name="offSet">starting offSet</param>
        /// <param name="len">len of byte block (default 16)</param>
        /// <returns>byte[len] (default: 16) segment of encrypted bytes</returns>
        protected internal virtual byte[] ProcessBytes(byte[] inBytes, int offSet = 0, int len = 0x10)
        {
            int aCnt = 0, bCnt = 0;
            if (offSet < inBytes.Length && offSet + len <= inBytes.Length)
            {
                byte[] processed = new byte[len];
                for (aCnt = 0, bCnt = offSet; bCnt < offSet + len; aCnt++, bCnt++)
                {
                    byte b = inBytes[bCnt];
                    MapByteValue(ref b, out byte mappedByte, forEncryption);
                    sbyte pos = (forEncryption) ? MatrixPermutationKey[aCnt % 0x10] : InverseMatrix[aCnt % 0x10];
                    processed[(int)pos] = mappedByte;
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
        protected internal virtual sbyte[] MapByteValue(ref byte inByte, out byte outByte, bool encrypt = true)
        {
            List<sbyte> outSBytes = new List<sbyte>(2);
            sbyte lsbIn = (sbyte)((short)inByte % 0x10);
            sbyte msbIn = (sbyte)((short)((short)inByte / 0x10));
            sbyte lsbOut, msbOut;
            if (encrypt)
            {
                lsbOut = MatrixPermutationKey[(int)lsbIn];
                msbOut = MatrixPermutationKey[(int)msbIn];
                outSBytes.Add(lsbOut);
                outSBytes.Add(msbOut);
                outByte = (byte)((short)(((short)msbOut * 0x10) + ((short)lsbOut)));
            }
            else // if decrypt
            {
                lsbOut = _inverseMatrix[(int)lsbIn];
                msbOut = _inverseMatrix[(int)msbIn];
                outSBytes.Add(lsbOut);
                outSBytes.Add(msbOut);
                outByte = (byte)((short)(((short)msbOut * 0x10) + ((short)lsbOut)));
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
