using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// More complex but still lightweight byte mapping from 0x0 .. to 0xf as symmetric cipher matrix
    /// position swaps and byte mappings are seperated in 2 matrizes 
    /// and maybe I will add ZenMatrix3 l8r, to multiply and divide byte values with a 3rd matrix 
    /// for mapping byte[1] => byte[1] 0xf => 0xab and generate 
    /// 
    /// I would never introduce such a cipher in real world applications, 
    /// only for students how the simplest blockcipher works
    /// 
    /// maybe this encryption is already invented, but created at Git by zen@area23.at (Heinrich Elsigan)
    /// </summary>
    public class ZenMatrix2 : ZenMatrix
    {

        #region fields

        protected internal static readonly byte[] MatrixPermutationBase2 = {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
            0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
            0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
            0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
            0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
            0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
            0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
            0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
            0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f,
            0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
            0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
            0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xcb, 0xcc, 0xcd, 0xce, 0xcf,
            0xd0, 0xd1, 0xd2, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 0xdc, 0xdd, 0xde, 0xdf,
            0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 0xec, 0xed, 0xee, 0xef,
            0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff
        };
        // protected internal new byte[] privateBytes = new byte[0x10];
        protected internal byte[] privateBytes2 = new byte[0x100];

        #endregion fields

        #region Properties

        /// <summary>
        /// abstraction of a 0x10 => 0x10 matrix, for example
        /// <see cref="MatrixPermutationKey2"/> 
        /// </summary>
        public byte[] MatrixPermutationKey2 { get; protected internal set; }

        protected internal byte[] _inverseMatrix2 = new byte[0];
        /// <summary>
        /// Inverse Matrix 2 
        /// </summary>
        protected internal byte[] InverseMatrix2
        {
            get
            {
                if (_inverseMatrix2 == null ||
                    _inverseMatrix2.Length < 0x100 ||
                    (_inverseMatrix2[0] == (byte)0x0 && _inverseMatrix2[1] == (byte)0x0 && _inverseMatrix2[0xf] == (byte)0x0) ||
                    (_inverseMatrix2[0] == (byte)0x0 && _inverseMatrix2[1] == (byte)0x1 && _inverseMatrix2[0xf] == (byte)0xf))
                {
                    _inverseMatrix2 = BuildInverseMatrix(MatrixPermutationKey2);
                }

                return _inverseMatrix2;
            }
        }


        /// <summary>
        /// PermutationKeyHash is same as <see cref="MatrixPermutationKey2"/>
        /// Advantage of <see cref="T:HashSet{byte}"/> is, that no duplicated values can be inside
        /// </summary>
        public HashSet<byte> PermutationKeyHash2 { get; protected internal set; }


        #endregion Properties

        #region IBlockCipher interface

        private const string SYMMCIPHERALGONAME2 = "ZenMatrix2";
        public new string AlgorithmName => SYMMCIPHERALGONAME2;


        protected internal static new int BLOCK_SIZE = 256;
        public new int GetBlockSize() => BLOCK_SIZE;


        #endregion IBlockCipher interface


        #region ctor_init_gen_reverse

        public ZenMatrix2(int bs) : base(bs)
        {
            byte sbcnt = 0x0;
            MatrixPermutationKey2 = new byte[0x100];
            foreach (byte s in MatrixPermutationBase)
            {
                privateBytes2[sbcnt % 0x100] = (byte)0x0;
                MatrixPermutationKey2[sbcnt++] = s;
            }
            PermutationKeyHash2 = new HashSet<byte>(MatrixPermutationBase);
            _inverseMatrix2 = BuildInverseMatrix2(MatrixPermutationKey2);
        }

        /// <summary>
        /// public constructor
        /// </summary>
        public ZenMatrix2() : this(256)
        {
            byte sbcnt = 0x0;
            MatrixPermutationKey2 = new byte[0x100];
            foreach (byte s in MatrixPermutationBase2)
            {
                privateBytes2[sbcnt % 0x100] = (byte)0x0;
                MatrixPermutationKey2[sbcnt++] = s;
            }
            PermutationKeyHash2 = new HashSet<byte>(MatrixPermutationBase2);
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
            secretKey = string.IsNullOrEmpty(secretKey) ? Constants.AUTHOR_EMAIL : secretKey;
            hashIV = string.IsNullOrEmpty(hashIV) ? Constants.AREA23_EMAIL : hashIV;
            byte[] keyBytes2 = CryptHelper.GetUserKeyBytes(secretKey, hashIV, 0x100);

            ZenMatrixGenWithBytes2(keyBytes2, true);
        }

        /// <summary>
        /// initializes a <see cref="ZenMatrix"/> with an array of key bytes
        /// </summary>
        /// <param name="keyBytes2"><see cref="T:byte[]">array of key bytes</see></param>
        /// <param name="fullSymmetric">
        /// fullSymmetric means that zen matrix is it's inverse element 
        /// and decrypts back to plain text, when encrypting twice or ²</param> 
        public ZenMatrix2(byte[] keyBytes2, bool fullSymmetric = false) : this()
        {
            ZenMatrixGenWithBytes2(keyBytes2, fullSymmetric);
        }

        /// <summary>
        /// InitMatrixSymChiffer - base initialization of variables, needed for matrix sym chiffer encryption
        /// </summary>
        private void InitMatrixSymChiffer2()
        {
            byte sbcnt = 0x0;
            MatrixPermutationKey = new byte[0x10];
            MatrixPermutationKey2 = new byte[0x100];
            foreach (byte s in MatrixPermutationBase2)
            {
                privateBytes[sbcnt % 0x10] = (byte)0x0;
                privateBytes2[sbcnt % 0x100] = (byte)0x0;
                MatrixPermutationKey[sbcnt % 0x10] = (byte)((int)s % 0x10);
                MatrixPermutationKey2[sbcnt++] = s;
            }
            PermutationKeyHash = new HashSet<byte>(MatrixPermutationBase);
            PermutationKeyHash2 = new HashSet<byte>(MatrixPermutationBase2);
            _inverseMatrix = BuildInverseMatrix(MatrixPermutationKey);
            _inverseMatrix2 = BuildInverseMatrix(MatrixPermutationKey2);
        }


        /// <summary>
        /// Generates ZenMatrix with key bytes
        /// </summary>
        /// <param name="keyBytes2">must have at least 4 bytes and will be truncated after 16 bytes
        /// only the first 16 bytes will be taken from keyBytes for <see cref="ZenMatrix"/>
        /// </param>
        /// <returns>true, if init was with same key successfull</returns>
        /// <param name="fullSymmetric">
        /// fullSymmetric means that zen matrix is it's inverse element 
        /// and decrypts back to plain text, when encrypting twice or ²</param>       
        /// <exception cref="ApplicationException"></exception>
        protected internal virtual void ZenMatrixGenWithBytes2(byte[] keyBytes2, bool fullSymmetric = false)
        {
            if ((keyBytes2 == null || keyBytes2.Length < 4))
                throw new ApplicationException("byte[] keyBytes is null or keyBytes.Length < 4");

            base.ZenMatrixGenWithBytes(keyBytes2, fullSymmetric);

            int ba = 0, bb = 0;

            Dictionary<byte, byte> MatrixDict2 = new Dictionary<byte, byte>();
            PermutationKeyHash2 = new HashSet<byte>();

            if (keyBytes2.Length < 0x100)
            {
                privateBytes2 = CryptHelper.GetKeyHashBytes(keyBytes2, privateBytes, 0x100);
            }
            else
            {
                for (int l = 0, k = keyBytes2.Length - 1; (k >= 0 && l < 0x100); k--, l++)
                {
                    privateBytes2[l] = (byte)keyBytes2[k];
                }
            }


            foreach (byte keyByte in new List<byte>(privateBytes2))
            {
                byte b = (byte)(keyByte % 0x100);
                for (int i = 0; i < 0x100; i++)
                {
                    if (PermutationKeyHash2.Contains(b) || ((int)b) == ba)
                    {
                        if (i < 0x100)
                            b = ((byte)((Convert.ToInt32(keyByte) + (MagicOrder[i % 10] * MagicOrder[i % 10]) % 0x100)));
                        if (i >= 0x100)
                            b = ((byte)((Convert.ToInt32(keyByte) + i) % 0x100));
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
                            if (!MatrixDict2.Keys.Contains(b) && !MatrixDict2.Keys.Contains((byte)ba))
                            {
                                MatrixDict2.Add((byte)ba, (byte)bb);
                                MatrixDict2.Add((byte)bb, (byte)ba);
                            }
                        }

                        PermutationKeyHash2.Add(b);
                        MatrixPermutationKey2 = MatrixPermutationKey2.SwapTPositions<byte>(ba, bb);
                        ba++;
                    }
                }
            }

            if (fullSymmetric)
            {
                #region fullSymmetric => InverseMatrix2 = MatrixPermutationKey2;
                if (MatrixDict2.Count < 0xff)
                {
                    for (int k = 0; k < 0x100; k++)
                    {
                        if (!MatrixDict2.Keys.Contains((byte)k))
                        {
                            for (int l = 0xff; l >= 0; l--)
                            {
                                if (!MatrixDict2.Values.Contains((byte)l))
                                {
                                    MatrixDict2.Add((byte)k, (byte)l);
                                    if (!MatrixDict2.Keys.Contains((byte)l))
                                        MatrixDict2.Add((byte)l, (byte)k);
                                    break;
                                }
                            }
                        }
                    }
                }
                if (MatrixDict2.Count == 0x100)
                {
                    byte bKey, bValue;
                    PermutationKeyHash2.Clear();
                    for (int n = 0; n < 0x100; n++)
                    {
                        bKey = (byte)n;
                        bValue = (byte)MatrixDict2[bKey];
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
                byte[] strikeBytes = {  0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
                                        0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
                                        0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
                                        0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
                                        0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
                                        0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
                                        0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
                                        0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
                                        0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
                                        0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f,
                                        0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
                                        0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
                                        0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xcb, 0xcc, 0xcd, 0xce, 0xcf,
                                        0xd0, 0xd1, 0xd2, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 0xdc, 0xdd, 0xde, 0xdf,
                                        0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 0xec, 0xed, 0xee, 0xef,
                                        0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };
                HashSet<byte> strikeList = new HashSet<byte>(strikeBytes);
                int cancelationCounter = 0;
                if (PermutationKeyHash.Count < 0x100)
                {
                    while (strikeList.Count > 0 && cancelationCounter++ < 0x100)
                    {
                        for (int k = 0; k < 0x100; k++)
                        {
                            try
                            {
                                byte inByte = PermutationKeyHash.ElementAt(k);
                                if (strikeList.Contains(inByte))
                                    strikeList.Remove(inByte);
                            }
                            catch (Exception exByte)
                            {
                                Area23Log.LogOriginMsgEx("ZenMatrix", $"Error when loading PermutationKeyHash.ElementAt({k});", exByte);
                                if (strikeList.Count > 0)
                                {
                                    byte addedFromStrikeList = (byte)strikeList.ElementAt(0);
                                    strikeList.Remove(addedFromStrikeList);
                                    PermutationKeyHash.Add(addedFromStrikeList);
                                }
                            }
                        }
                    }
                }
                #endregion bugfix for missing permutations
                for (int i = 0; i < 0x100; i++)
                {
                    if ((int)PermutationKeyHash2.ElementAt(i) != i)
                    {
                        MatrixPermutationKey2[i] = PermutationKeyHash2.ElementAt(i);
                    }
                }

                _inverseMatrix2 = BuildInverseMatrix(MatrixPermutationKey);
            }

            string perm2 = string.Empty, kbs2 = string.Empty;

            for (int j = 0; j < 0x100; j++)
                perm2 += MatrixPermutationKey2[j].ToString("x2");
            for (int j = 0; j < keyBytes2.Length; j++)
                kbs2 += keyBytes2[j].ToString("x2");

            Area23Log.LogOriginMsg("ZenMatrix2", perm2 + " KeyBytes = " + kbs2);
        }

        #endregion ctor_init_gen_reverse

        #region ProcessEncryptDecryptBytes

        /// <summary>
        /// ProcessBytes2  processes the next len=16 bytes to encrypt or decrypt, starting at offSet
        /// </summary>
        /// <param name="inBytes">incoming bytes array to en-/deccrypt</param>
        /// <param name="offSet">starting offSet</param>
        /// <param name="len">len of byte block (default 16)</param>
        /// <returns>byte[len] (default: 16) segment of decrypted bytes</returns>
        protected internal virtual byte[] ProcessBytes2(byte[] inBytes, int offSet = 0, int len = 0x100)
        {
            int aCnt = 0, bCnt = 0;
            byte[] processed = null;
            if (offSet < inBytes.Length && offSet + len <= inBytes.Length)
            {
                processed = new byte[len];
                for (aCnt = 0, bCnt = offSet; bCnt < offSet + len; aCnt++, bCnt++)
                {
                    byte b = inBytes[bCnt];
                    MapByteValue2(ref b, out byte mappedByte, forEncryption);
                    byte sm = (forEncryption) ? MatrixPermutationKey2[aCnt] : InverseMatrix2[aCnt];
                    processed[(int)sm] = mappedByte;
                }
            }

            return processed ?? new byte[0];
        }

        protected internal virtual byte[] ProcessBlocks2(byte[] inBytes)
        {
            int aCnt = 0, bCnt = 0;
            byte[] processed = new byte[(int)inBytes.Length];
            Array.Copy(inBytes, processed, inBytes.Length);
            for (int bs = 0; bs < inBytes.Length; bs += 0x100)
            {
                for (int cs = 0, ds = 0; cs < 0x100 && (bs + cs) < inBytes.Length; cs += 0x10)
                {

                    int sm = (int)(0x10 * (int)((forEncryption) ? MatrixPermutationKey[ds] : InverseMatrix[ds]));
                    Array.Copy(inBytes, bs + cs, processed, bs + sm, 0x10);
                    ds++;
                }
            }


            return processed ?? new byte[0];
        }


        /// <summary>
        /// MatrixSymChiffer Encrypt member function
        /// </summary>
        /// <param name="pdata">plain data as <see cref="T:byte[]"/></param>
        /// <returns>encrypted data <see cref="T:byte[]">bytes</see></returns>
        public override byte[] Encrypt(byte[] pdata, bool randomBuf = false)
        {
            // Check arguments.
            if (pdata == null || pdata.Length <= 0)
                throw new ArgumentNullException("ZenMatrix byte[] Encrypt(byte[] pdata): ArgumentNullException pdata = null or Lenght 0.");

            forEncryption = true;

            int dlen = pdata.Length;                        // length of data bytes
            int reverseset = 256 - (dlen % 256);
            int oSize = dlen + reverseset;                  // oSize is rounded up to next number % 16 == 0
            long olen = ((long)(oSize / 0x100)) * 0x100;    // olen is (long)oSize
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
                else if (i == dlen + 1)
                    obytes[i] = (byte)0xff;                  // write 0xff as stop byte beginning padding buffer
                else if (i == (olen - 1))
                    obytes[i] = (byte)0x0;                  // terminate end of obytes with 0x0                                    
                else if (i > dlen)
                    obytes[i] = rndbuf[j++];                // fill rest with random hash padding 
            }


            List<byte> encryptedBytes = new List<byte>();
            int b = 0;
            for (int i = 0; i < obytes.Length; i += 0x100)
            {
                foreach (byte pb in ProcessBytes2(obytes, i, 0x100))
                {
                    encryptedBytes.Add(pb);
                }
                b++;
            }
            byte[] processed2 = ProcessBlocks2(encryptedBytes.ToArray());

            return processed2.ToArray();

        }

        /// <summary>
        /// MatrixSymChiffer Decrypt member function
        /// </summary>
        /// <param name="ecdata">encrypted cipher <see cref="T:byte[]">bytes</see></param>
        /// <returns>decrypted plain byte[] data</returns>
        public override byte[] Decrypt(byte[] ecdata)
        {
            if (ecdata == null || ecdata.Length <= 0)
                throw new ArgumentNullException("ZenMatrix byte[] Encrypt(byte[] ecdata): ArgumentNullException ecdata = null or Lenght 0.");

            forEncryption = false;

            int eclen = ecdata.Length;
            int ecSize = (eclen % 0x10 == 0) ? eclen : (eclen + (0x10 - (eclen % 0x10)));
            if (ecSize > eclen) {; } // something went wrong                            

            byte[] preProcessed = ProcessBlocks2(ecdata);


            List<byte> outBytes = new List<byte>();
            for (int pc = 0; pc < preProcessed.Length; pc += 256)
            {
                foreach (byte rb in ProcessBytes2(preProcessed, pc, 256))
                {
                    outBytes.Add(rb);
                }
            }

            int olen = outBytes.Count;
            int dlen = olen;
            bool first0 = false, last0 = (outBytes.ElementAt(olen - 1) == (byte)0x0);

            for (dlen = olen; dlen > 0 && !first0; dlen--)
            {
                if (dlen <= olen &&
                    (outBytes.ElementAt(dlen - 1) == (byte)0xff) &&
                    // || outBytes.ElementAt(dlen - 1) == (byte)0x00) &&
                    outBytes.ElementAt(dlen - 2) == (byte)0x0)
                {
                    first0 = true;
                    break;
                }
            }
            byte[] obytes = (dlen > 1) ? new byte[dlen] : new byte[olen];
            Array.Copy(outBytes.ToArray(), 0, obytes, 0, obytes.Length);

            return obytes;
        }


        /// <summary>
        /// BuildInverseMatrix, builds the determinant decryption matrix for byte[16] encryption matrix
        /// </summary>
        /// <param name="matrix">byte[16] encryption matrix</param>
        /// <returns><see cref="T:byte[]">byte[16]</see> decryption matrix (determinante)</returns>
        internal static byte[] BuildInverseMatrix2(byte[] matrix, int size = 0x100)
        {
            return BuildInverseMatrix(matrix, 0x100);
        }

        /// <summary>
        /// MapByteValue splits a byte in 2 0x0 - 0xf segments and map both trough <see cref="MatrixPermutationKey2"/> in case of encrypt,
        /// through <see cref="InverseMatrix2"/> in case of decryption.
        /// </summary>
        /// <param name="inByte"><see cref="byte"/> in byte to map</param>
        /// <param name="outByte"><see cref="byte"/> mapped out byte</param>
        /// <param name="encrypt">true for encryption, false for decryption</param>
        /// <returns>An <see cref="T:byte[]"/> array with 2  0x0 - 0xf segments (most significant + least significant) bit</returns>
        private byte[] MapByteValue2(ref byte inByte, out byte outByte, bool encrypt = true)
        {
            List<byte> outBytes = new List<byte>(2);
            if (encrypt)
                outByte = MatrixPermutationKey2[inByte];
            else // if decrypt
                outByte = _inverseMatrix2[(int)inByte];

            byte lsbOut = (byte)(outByte & 0x0F);
            byte msbOut = (byte)((outByte & 0xF0) / 0x10);
            outBytes.Add(lsbOut);
            outBytes.Add(msbOut);

            return outBytes.ToArray();
        }

        #endregion ProcessEncryptDecryptBytes

    }

}

