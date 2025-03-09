using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
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


        // protected internal new byte[] privateBytes = new byte[0x10];
        protected internal byte[] privateBytes2 = new byte[0x10];

        #endregion fields

        #region Properties

        /// <summary>
        /// abstraction of a 0x10 => 0x10 matrix, for example
        /// <see cref="MatrixPermutationKey"/> 
        /// </summary>
        public sbyte[] MatrixPermutationKey2 { get; protected internal set; }

        protected internal sbyte[] _inverseMatrix2 = new sbyte[0];
        /// <summary>
        /// Inverse Matrix 2 
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
                    _inverseMatrix2 = BuildInverseMatrix(MatrixPermutationKey2);
                }

                return _inverseMatrix2;
            }
        }


        /// <summary>
        /// PermutationKeyHash is same as <see cref="MatrixPermutationKey2"/>
        /// Advantage of <see cref="HashSet{sbyte}"/> is, that no duplicated values can be inside
        /// </summary>
        public HashSet<sbyte> PermutationKeyHash2 { get; protected internal set; }


        #endregion Properties

        #region ctor_init_gen_reverse

        /// <summary>
        /// public constructor
        /// </summary>
        public ZenMatrix2() : base()
        {
            sbyte sbcnt = 0x0;
            MatrixPermutationKey2 = new sbyte[0x10];
            foreach (sbyte s in MatrixPermutationBase)
            {
                privateBytes2[sbcnt % 0x10] = (byte)0x0;
                MatrixPermutationKey2[sbcnt++] = s;
            }
            PermutationKeyHash2 = new HashSet<sbyte>(MatrixPermutationBase);
            _inverseMatrix2 = BuildInverseMatrix(MatrixPermutationKey2);
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
            byte[] keyBytes2 = CryptHelper.GetUserKeyBytes(secretKey, hashIV, 0x10);

            ZenMatrixGenWithBytes2(keyBytes2, true);
        }


        /// <summary>
        /// initializes a <see cref="ZenMatrix"/> with an array of key bytes
        /// </summary>
        /// <param name="keyBytes">array of key bytes</param>
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
            sbyte sbcnt = 0x0;
            MatrixPermutationKey = new sbyte[0x10];
            MatrixPermutationKey2 = new sbyte[0x10];
            foreach (sbyte s in MatrixPermutationBase)
            {
                privateBytes[sbcnt % 0x10] = (byte)0x0;
                privateBytes2[sbcnt % 0x10] = (byte)0x0;
                MatrixPermutationKey[sbcnt] = s;
                MatrixPermutationKey2[sbcnt++] = s;
            }
            PermutationKeyHash = new HashSet<sbyte>(MatrixPermutationBase);
            PermutationKeyHash2 = new HashSet<sbyte>(MatrixPermutationBase);
            _inverseMatrix = BuildInverseMatrix(MatrixPermutationKey);
            _inverseMatrix2 = BuildInverseMatrix(MatrixPermutationKey2);
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
        protected internal virtual void ZenMatrixGenWithBytes2(byte[] keyBytes2, bool fullSymmetric = false)
        {
            if ((keyBytes2 == null || keyBytes2.Length < 4))
                throw new ApplicationException("byte[] keyBytes is null or keyBytes.Length < 4");

            base.ZenMatrixGenWithBytes(keyBytes2, fullSymmetric);

            int ba = 0, bb = 0;

            Dictionary<sbyte, sbyte> MatrixDict2 = new Dictionary<sbyte, sbyte>();
            PermutationKeyHash2 = new HashSet<sbyte>();

            if (keyBytes2.Length < 0x10)
            {
                Array.Copy(keyBytes2, 0, privateBytes2, 0, Math.Min(keyBytes2.Length, 0x10));
                for (int i = keyBytes2.Length; i < 0x10; i++)
                {
                    if (i < 0x08)
                        privateBytes2[i] = (byte)keyBytes2[i % keyBytes2.Length];
                    else
                        privateBytes2[i] = (byte)keyBytes2[0x08 - (i - 0x07)];
                }
            }
            else
            {
                for (int l = 0, k = keyBytes2.Length - 1; (k >= 0 && l < 0x10); k--, l++)
                {
                    privateBytes2[l] = (byte)keyBytes2[k];
                }
            }


            foreach (byte keyByte in new List<byte>(privateBytes2))
            {
                sbyte b = (sbyte)(keyByte % 0x10);
                for (int i = 0; i < 32; i++)
                {
                    if (PermutationKeyHash2.Contains(b) || ((int)b) == ba)
                    {
                        if (i < 0x10)
                            b = ((sbyte)((Convert.ToInt32(keyByte) + MagicOrder[i]) % 0x10));
                        if (i >= 0x10)
                            b = ((sbyte)((Convert.ToInt32(keyByte) + i) % 0x10));
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
                if (MatrixDict2.Count < 0x0f)
                {
                    for (int k = 0; k < 0x10; k++)
                    {
                        if (!MatrixDict2.Keys.Contains((sbyte)k))
                        {
                            for (int l = 0x0f; l >= 0; l--)
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
                if (MatrixDict2.Count == 0x10)
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
                for (int i = 0; i < 0x10; i++)
                {
                    if ((int)PermutationKeyHash2.ElementAt(i) != i)
                    {
                        MatrixPermutationKey2[i] = PermutationKeyHash2.ElementAt(i);
                    }
                }

                _inverseMatrix2 = BuildInverseMatrix(MatrixPermutationKey);
            }

            string perm2 = string.Empty, kbs2 = string.Empty;

            for (int j = 0; j < 0x10; j++)
                perm2 += MatrixPermutationKey2[j].ToString("x1");
            for (int j = 0; j < keyBytes2.Length; j++)
                kbs2 += keyBytes2[j].ToString("x2");

            Area23Log.LogStatic("ZenMatrix2 " + perm2 + " KeyBytes = " + kbs2);
        }


        #endregion ctor_init_gen_reverse

        #region ProcessEncryptDecryptBytes

        /// <summary>
        /// ProcessEncryptBytes2, processes the next len=16 bytes to encrypt, starting at offSet
        /// </summary>
        /// <param name="inBytesPadding">in bytes array to encrypt</param>
        /// <param name="offSet">starting offSet</param>
        /// <param name="len">len of byte block (default 16)</param>
        /// <returns>byte[len] (default: 16) segment of encrypted bytes</returns>
        protected internal virtual byte[] ProcessEncryptBytes2(byte[] inBytesPadding, int offSet = 0, int len = 0x10)
        {
            int aCnt = 0, bCnt = 0;
            byte[] processedEncrypted = null;
            if (offSet < inBytesPadding.Length && offSet + len <= inBytesPadding.Length)
            {
                processedEncrypted = new byte[len];
                for (aCnt = 0, bCnt = offSet; bCnt < offSet + len; aCnt++, bCnt++)
                {
                    byte b = inBytesPadding[bCnt];
                    MapByteValue2(ref b, out byte mapEncryptB, true);
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
        protected internal virtual byte[] ProcessDecryptBytes2(byte[] inBytesEncrypted, int offSet = 0, int len = 0x10)
        {
            int aCnt = 0, bCnt = 0;
            byte[] processedDecrypted = null;
            if (offSet < inBytesEncrypted.Length && offSet + len <= inBytesEncrypted.Length)
            {
                processedDecrypted = new byte[len];
                for (aCnt = 0, bCnt = offSet; bCnt < offSet + len; aCnt++, bCnt++)
                {
                    byte b = inBytesEncrypted[bCnt];
                    MapByteValue2(ref b, out byte mapDecryptB, false);
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
        public override byte[] Encrypt(byte[] pdata)
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
                foreach (byte pb in ProcessEncryptBytes2(obytes, i, 0x10))
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
        public override byte[] Decrypt(byte[] ecdata)
        {
            if (ecdata == null || ecdata.Length <= 0)
                throw new ArgumentNullException("ZenMatrix byte[] Encrypt(byte[] ecdata): ArgumentNullException ecdata = null or Lenght 0.");

            int eclen = ecdata.Length;
            int ecSize = (eclen % 0x10 == 0) ? eclen : (eclen + (0x10 - (eclen % 0x10)));
            if (ecSize > eclen) {; } // something went wrong                

            List<byte> outBytes = new List<byte>();
            for (int pc = 0; pc < ecdata.Length; pc += 16)
            {
                foreach (byte rb in ProcessDecryptBytes2(ecdata, pc, 16))
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
                lsbOut = MatrixPermutationKey2[(int)lsbIn];
                msbOut = MatrixPermutationKey2[(int)msbIn];
                outSBytes.Add(lsbOut);
                outSBytes.Add(msbOut);
                outByte = (byte)((short)(((short)msbOut * 16) + ((short)lsbOut)));
            }
            else // if decrypt
            {
                lsbOut = _inverseMatrix2[(int)lsbIn];
                msbOut = _inverseMatrix2[(int)msbIn];
                outSBytes.Add(lsbOut);
                outSBytes.Add(msbOut);
                outByte = (byte)((short)(((short)msbOut * 16) + ((short)lsbOut)));
            }

            return outSBytes.ToArray();
        }

        #endregion static helpers swap byte and SwapT{T} generic


    }

}
