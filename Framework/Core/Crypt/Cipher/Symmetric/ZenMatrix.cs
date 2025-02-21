using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Crypt.Cipher.Symmetric
{

    /// <summary>
    /// Simple Matrix symmetric cipher maybe already invented, but created by zen@area23.at (Heinrich Elsigan)
    /// </summary>
    public static class ZenMatrix
    {

        #region fields

        private static readonly sbyte[] MatrixBasePerm = {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
            0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f
        };

        private static readonly int[] MagicOrder = {
            0x8, 0x3, 0x1, 0xe,
            0x9, 0xf, 0x5, 0xc,
            0x4, 0xd, 0xa, 0x7,
            0xb, 0x2, 0x0, 0x6
        };

        private static string privateKey = string.Empty;
        private static string userHash = string.Empty;
        private static byte[] privateBytes = new byte[16];

        #endregion fields

        #region Properties

        public static sbyte[] MatrixPermKey { get; internal set; }
        public static sbyte[] MatrixReverse { get; internal set; }

        public static HashSet<sbyte> PermKeyHash { get; internal set; }

        public static Dictionary<sbyte, sbyte> MatrixDict { get; internal set; }


        #endregion Properties

        #region ctor_init_gen_reverse

        /// <summary>
        /// Static constructor
        /// </summary>
        static ZenMatrix()
        {
            InitMatrixSymChiffer();
            // MatrixPermSalt = GenerateMatrixPermutationByKey(Constants.AUTHOR);
            // MatrixPermKey = GetMatrixPermutation(Constants.AUTHOR_EMAIL);
            // MatrixReverse = BuildReveseMatrix(MatrixPermKey);
        }


        /// <summary>
        /// InitMatrixSymChiffer - base initialization of variables, needed for matrix sym chiffer encryption
        /// </summary>
        internal static void InitMatrixSymChiffer()
        {
            sbyte cntSby = 0x0;
            MatrixPermKey = new sbyte[0x10];
            foreach (sbyte s in MatrixBasePerm)
            {
                privateBytes[cntSby % 16] = (byte)0;
                MatrixPermKey[cntSby++] = s;
            }
            PermKeyHash = new HashSet<sbyte>(MatrixBasePerm);
            MatrixReverse = BuildReveseMatrix(MatrixPermKey);
        }

        /// <summary>
        /// Generate Matrix sym chiffre permutation with a personal key string
        /// </summary>
        /// <param name="secretKey">string key to generate permutation <see cref="MatrixPermKey"/> 
        /// and <see cref="MatrixPermSalt"/> for encryption 
        /// and reverse matrix <see cref="MatrixReverse"/> for decryption</param>
        /// <param name="usrHash">user key hash</param>
        /// <param name="init">init three fish first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool ZenMatrixGenWithKey(string secretKey = "", string usrHash = "", bool init = true) // , byte[] textForEncryption = null)
        {
            if (!init)
            {
                if ((string.IsNullOrEmpty(privateKey) && !string.IsNullOrEmpty(secretKey)) ||
                    (!privateKey.Equals(secretKey, StringComparison.InvariantCultureIgnoreCase)))
                    return false;
            }

            if (init)
            {
                privateKey = string.IsNullOrEmpty(secretKey) ? Constants.AUTHOR_EMAIL : secretKey;
                userHash = string.IsNullOrEmpty(usrHash) ? Constants.AREA23_EMAIL : usrHash;
                byte[] keyBytes = CryptHelper.GetUserKeyBytes(privateKey, userHash, 16);

                return ZenMatrixGenWithBytes(keyBytes, init);
            }

            return true;
        }


        /// <summary>
        /// Generates ZenMatrix with key bytes
        /// </summary>
        /// <param name="keyBytes">must have at least 6 bytes</param>
        /// <param name="init">init three fish first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool ZenMatrixGenWithBytes(byte[] keyBytes, bool init = true) // , byte[] textForEncryption = null)
        {
            if ((keyBytes == null || keyBytes.Length < 6))
                return false;

            if (init)
            {
                int ba = 0, bb = 0;
                Array.Copy(keyBytes, 0, privateBytes, 0, Math.Min(keyBytes.Length, 16));

                InitMatrixSymChiffer();
                MatrixDict = new Dictionary<sbyte, sbyte>();
                PermKeyHash = new HashSet<sbyte>();
                List<byte> key2 = new List<byte>(keyBytes);

                //foreach (byte keyByte in keyBytes)
                //{
                //    byte lsb = (byte)(keyByte % 16);
                //    byte msb = (byte)((keyByte - (byte)lsb) / 16);
                //    key2.Add(msb);
                //    key2.Add(lsb);
                //}

                foreach (byte keyByte in key2)
                {
                    sbyte b = (sbyte)(keyByte % 16);
                    for (int i = 0; i < 32; i++)
                    {
                        if (PermKeyHash.Contains(b) || ((int)b) == ba)
                        {
                            if (i < 16)
                                b = ((sbyte)((Convert.ToInt32(keyByte) + MagicOrder[i]) % 16));
                            if (i >= 16)
                                b = ((sbyte)((Convert.ToInt32(keyByte) + i) % 16));
                        }
                        else break;
                    }

                    if (!PermKeyHash.Contains(b))
                    {
                        bb = (int)b;
                        if (ba != bb)
                        {
                            if (!MatrixDict.Keys.Contains(b) && !MatrixDict.Keys.Contains((sbyte)ba))
                            {
                                MatrixDict.Add((sbyte)ba, (sbyte)bb);
                                MatrixDict.Add((sbyte)bb, (sbyte)ba);
                            }

                            PermKeyHash.Add(b);
                            MatrixPermKey = MatrixPermKey.SwapTPositions<sbyte>(ba, bb);
                            ba++;
                        }
                    }
                }

                #region Constants.ZEN_MATRIX_SYMMETRIC only for full symmetric zen matrizes, where (key -> value => value -> key)
                if (Constants.ZEN_MATRIX_SYMMETRIC)
                {
                    if (MatrixDict.Count < 15)
                    {
                        for (int k = 0; k < 16; k++)
                            if (!MatrixDict.Keys.Contains((sbyte)k))
                                for (int l = 15; l >= 0; l--)
                                    if (!MatrixDict.Values.Contains((sbyte)l))
                                    {
                                        MatrixDict.Add((sbyte)k, (sbyte)l);
                                        if (!MatrixDict.Keys.Contains((sbyte)l))
                                            MatrixDict.Add((sbyte)l, (sbyte)k);
                                        break;
                                    }
                    }
                    if (MatrixDict.Count == 16)
                    {
                        sbyte bKey, bValue;
                        PermKeyHash.Clear();
                        for (int n = 0; n < 16; n++)
                        {
                            bKey = (sbyte)n;
                            bValue = (sbyte)MatrixDict[bKey];
                            PermKeyHash.Add(bValue);
                            MatrixPermKey[(int)bKey] = bValue;
                            MatrixPermKey[(int)bValue] = bKey;
                        }
                    }
                }
                #endregion Constants.ZEN_MATRIX_SYMMETRIC only for full symmetric zen matrizes, where (key -> value => value -> key)


                MatrixReverse = BuildReveseMatrix(MatrixPermKey);


            }

            return true;

        }



        /// <summary>
        /// BuildReveseMatrix, builds the determinant decryption matrix for byte{16] encryption matrix
        /// </summary>
        /// <param name="matrix">byte{16] encryption matrix</param>
        /// <returns>sbyte{16] decryption matrix</returns>
        internal static sbyte[] BuildReveseMatrix(sbyte[] matrix)
        {
            sbyte[] rmatrix = new sbyte[matrix.Length];
            if (matrix != null && matrix.Length >= 16)
            {
                for (int m = 0; m < matrix.Length; m++)
                {
                    sbyte sm = matrix[m];
                    rmatrix[(int)sm] = (sbyte)m;
                }
            }
            return rmatrix;
        }

        #endregion ctor_init_gen_reverse

        #region ProcessEncryptDecryptBytes

        /// <summary>
        /// ProcessEncryptBytes, processes the next len=16 bytes to encrypt, starting at offSet
        /// </summary>
        /// <param name="inBytesPadding">in bytes array to encrypt</param>
        /// <param name="offSet">starting offSet</param>
        /// <param name="len">len of byte block (default 16)</param>
        /// <returns>byte[len] (default: 16) segment of encrypted bytes</returns>
        public static byte[] ProcessEncryptBytes(byte[] inBytesPadding, int offSet = 0, int len = 16)
        {
            int aCnt = 0, bCnt = 0;
            byte[] processedEncrypted = null;
            if (offSet < inBytesPadding.Length && offSet + len <= inBytesPadding.Length)
            {
                processedEncrypted = new byte[len];
                for (aCnt = 0, bCnt = offSet; bCnt < offSet + len; aCnt++, bCnt++)
                {
                    byte b = inBytesPadding[bCnt];
                    MapByteValue(ref b, out byte mapEncryptB, true);
                    sbyte sm = MatrixPermKey[aCnt];
                    processedEncrypted[(int)sm] = mapEncryptB;
                }
            }
            return processedEncrypted;
        }

        /// <summary>
        /// ProcessDecryptBytes  processes the next len=16 bytes to decrypt, starting at offSet
        /// </summary>
        /// <param name="inBytesEncrypted">encrypted bytes array to deccrypt</param>
        /// <param name="offSet">starting offSet</param>
        /// <param name="len">len of byte block (default 16)</param>
        /// <returns>byte[len] (default: 16) segment of decrypted bytes</returns>
        public static byte[] ProcessDecryptBytes(byte[] inBytesEncrypted, int offSet = 0, int len = 16)
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
                    sbyte sm = MatrixReverse[aCnt];
                    processedDecrypted[(int)sm] = mapDecryptB;
                }
            }
            return processedDecrypted;
        }

        #endregion ProcessEncryptDecryptBytes

        #region EncryptDecryptBytes

        /// <summary>
        /// MatrixSymChiffer Encrypt member function
        /// </summary>
        /// <param name="plainData">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public static byte[] Encrypt(byte[] plainData)
        {
            // Check arguments.
            if (plainData == null || plainData.Length <= 0)
                throw new ArgumentNullException("ZenMatrix byte[] Encrypt(byte[] plainData): ArgumentNullException plainData = null or Lenght 0.");

            int bCnt = 0, cCnt = 0;
            long oSize = plainData.Length + (16 - (plainData.Length % 16));
            long outputSize = ((long)(oSize / 16)) * 16;
            byte[] inBytesPadding = new byte[outputSize];

            byte[] randBuffer = new byte[outputSize - plainData.Length];
            Random rand = new Random(plainData.Length);
            rand.NextBytes(randBuffer);

            for (bCnt = 0; bCnt < inBytesPadding.Length; bCnt++)
            {
                if (bCnt < plainData.Length)
                    inBytesPadding[bCnt] = plainData[bCnt];
                else if (bCnt == plainData.Length)
                    inBytesPadding[bCnt] = (byte)0x0;
                else if (bCnt == plainData.Length - 1)
                    inBytesPadding[bCnt] = (byte)0x0;
                else // random hash padding                    
                    inBytesPadding[bCnt] = randBuffer[cCnt++];
            }

            List<byte> outBytes = new List<byte>();
            for (int processCnt = 0; processCnt < inBytesPadding.Length; processCnt += 16)
            {
                byte[] retByte = ProcessEncryptBytes(inBytesPadding, processCnt, 16);
                foreach (byte rb in retByte)
                {
                    outBytes.Add(rb);
                }
            }

            byte[] outBytesEncrypted = outBytes.ToArray();
            return outBytesEncrypted;

        }

        /// <summary>
        /// MatrixSymChiffer Decrypt member function
        /// </summary>
        /// <param name="cipherData">encrypted <see cref="byte[]">bytes</see></param>
        /// <returns>decrypted plain byte[] data</returns>
        public static byte[] Decrypt(byte[] cipherData)
        {
            if (cipherData == null || cipherData.Length <= 0)
                throw new ArgumentNullException("ZenMatrix byte[] Encrypt(byte[] cipherData): ArgumentNullException cipherData = null or Lenght 0.");

            int bCnt = 0;
            long oSize = (cipherData.Length % 16 == 0) ? (long)cipherData.Length :
                (long)(cipherData.Length + (16 - (cipherData.Length % 16)));
            long outputSize = ((long)(oSize / 16)) * 16;
            byte[] inBytesEncrypted = new byte[outputSize];

            for (bCnt = 0; bCnt < inBytesEncrypted.Length; bCnt++)
            {
                if (bCnt < cipherData.Length)
                    inBytesEncrypted[bCnt] = cipherData[bCnt];
                else
                    inBytesEncrypted[bCnt] = (byte)0x0;
            }

            List<byte> outBytes = new List<byte>();
            for (int processCnt = 0; processCnt < inBytesEncrypted.Length; processCnt += 16)
            {
                byte[] retByte = ProcessDecryptBytes(inBytesEncrypted, processCnt, 16);
                foreach (byte rb in retByte)
                {
                    outBytes.Add(rb);
                }
            }

            byte[] outBytesPlainPadding = outBytes.ToArray();
            return outBytesPlainPadding;
        }

        #endregion EncryptDecryptBytes

        #region EnDecryptString

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="inPlainString">plain text string</param>
        /// <returns>Base64 encoded encrypted byte[]</returns>
        public static string EncryptString(string inPlainString)
        {
            byte[] plainTextData = EnDeCoder.GetBytes(inPlainString);
            byte[] encryptedData = Encrypt(plainTextData);
            string encryptedString = Convert.ToBase64String(encryptedData);
            // Encoding.ASCII.GetString(encryptedData).TrimEnd('\0');
            return encryptedString;
        }

        /// <summary>
        /// Decrypts a string, that is truely a base64 encoded encrypted byte[]
        /// </summary>
        /// <param name="inCryptString">base64 encoded string from encrypted byte[]</param>
        /// <returns>plain text string (decrypted)</returns>
        public static string DecryptString(string inCryptString)
        {
            byte[] cryptData = Convert.FromBase64String(inCryptString);
            //  EnDeCoder.GetBytes(inCryptString);
            byte[] plainTextData = Decrypt(cryptData);
            string plainTextString = EnDeCoder.GetString(plainTextData).TrimEnd('\0');

            return plainTextString;
        }

        #endregion EnDecryptString

        #region SwapHelpers

        /// <summary>
        /// MapByteValue splits a byte in 2 0x0 - 0xf segments and map both trough <see cref="MatrixPermKey"/> in case of encrypt,
        /// through <see cref="MatrixReverse"/> in case of decryption.
        /// </summary>
        /// <param name="inByte"><see cref="byte"/> in byte to map</param>
        /// <param name="outByte"><see cref=byte"/> mapped out byte</param>
        /// <param name="encrypt">true for encryption, false for decryption</param>
        /// <returns>An <see cref="sbyte[]"/> array with 2  0x0 - 0xf segments (most significant & least significant) bit</returns>
        internal static sbyte[] MapByteValue(ref byte inByte, out byte outByte, bool encrypt = true)
        {
            List<sbyte> outSBytes = new List<sbyte>(2);
            sbyte lsbIn = (sbyte)((short)inByte % 16);
            sbyte msbIn = (sbyte)((short)((short)inByte / 16));
            sbyte lsbOut, msbOut;
            if (encrypt)
            {
                lsbOut = MatrixPermKey[(int)lsbIn];
                msbOut = MatrixPermKey[(int)msbIn];
                outSBytes.Add(lsbOut);
                outSBytes.Add(msbOut);
                outByte = (byte)((short)(((short)msbOut * 16) + ((short)lsbOut)));
            }
            else // if decrypt
            {
                lsbOut = MatrixReverse[(int)lsbIn];
                msbOut = MatrixReverse[(int)msbIn];
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



        #endregion SwapHelpers


        #region ObsoleteDeprecated

        /// <summary>
        /// SwapBytes swaps two bytes
        /// </summary>
        /// <param name="ba"></param>
        /// <param name="bb"></param>
        /// <returns></returns>
        [Obsolete("SwapBytes(ref byte ba, ref byte bb) is deprecated obsolete. Use generic Method T[] Util.Ext.SwapT<T>(ref T t0, ref T t1).", true)]
        internal static byte[] SwapBytes(ref byte ba, ref byte bb)
        {
            byte[] tmp = new byte[2];
            tmp[0] = Convert.ToByte(ba.ToString());
            tmp[1] = Convert.ToByte(bb.ToString());
            ba = tmp[1];
            bb = tmp[0];
            return tmp;
        }

        /// <summary>
        /// SwapSBytes, swaps two sbyte
        /// </summary>
        /// <param name="sba">sbyte a0 to swap</param>
        /// <param name="sbb">sbyte b1 to swap</param>
        /// <returns>an array, where sbyte b1 is at position 0 and sbyte a0 is at position 1</returns>
        [Obsolete("SwapSBytes(ref byte ba, ref byte bb) is deprecated obsolete. Use generic Method T[] Util.Ext.SwapT<T>(ref T t0, ref T t1).", true)]
        internal static sbyte[] SwapSBytes(ref sbyte a0, ref sbyte b1)
        {
            sbyte[] tmp = new sbyte[2];
            tmp[0] = Convert.ToSByte(b1.ToString());
            tmp[1] = Convert.ToSByte(a0.ToString());
            a0 = tmp[0];
            b1 = tmp[1];
            return tmp;
        }


        [Obsolete("GetMatrixPermutation is obsolete, use GenerateMatrixPermutationByKey(string key) instead!", false)]
        internal static sbyte[] GetMatrixPermutation(string key)
        {
            InitMatrixSymChiffer();

            int aCnt = 0, bCnt = 0;

            InitMatrixSymChiffer();

            PermKeyHash = new HashSet<sbyte>();
            byte[] keyBytes = EnDeCoder.GetBytes(key);
            foreach (byte b in keyBytes)
            {
                sbyte sb = (sbyte)(((int)b) % 16);
                if (!PermKeyHash.Contains(sb))
                {
                    PermKeyHash.Add(sb);
                    aCnt = (int)sb;
                    if (aCnt != bCnt)
                    {
                        MatrixPermKey = MatrixPermKey.SwapTPositions<sbyte>(aCnt, bCnt);
                        // sbyte sba = MatrixPermKey[aCnt];
                        // sbyte sbb = MatrixPermKey[bCnt];
                        // SwapT<sbyte>(ref sba, ref sbb);
                        // MatrixPermKey[aCnt] = sba;
                        // MatrixPermKey[bCnt] = sbb;
                    }
                    bCnt++;
                }
            }

            MatrixReverse = BuildReveseMatrix(MatrixPermKey);

            /*
            HashSet<sbyte> takenSBytes = new HashSet<sbyte>();
            HashSet<int> dicedPos = new HashSet<int>();
            for (int randomizeCnt = 0; randomizeCnt <= 0x1f; randomizeCnt++)
            {
                Random rand = new Random(System.DateTime.UtcNow.Millisecond);
                int hpos = 0;
                int pos = (int)rand.Next(0x0, 0xf);
                while (dicedPos.Contains(pos))
                {
                    pos = (int)rand.Next(0x0, 0xf);
                    if (dicedPos.Contains(pos))
                    {
                        pos = hpos++;
                        if (hpos >= 16)
                            hpos = 0;
                    }
                }
                dicedPos.Add(pos);
                sbyte talenS = PermKeyHash.ElementAt(pos);
                takenSBytes.Add(talenS);
                if (takenSBytes.Count == 16)
                {
                    MatrixPermSalt = new sbyte[16];
                    takenSBytes.CopyTo(MatrixPermSalt);
                    PermKeyHash = new HashSet<sbyte>(MatrixPermSalt);
                    takenSBytes = new HashSet<sbyte>();
                    dicedPos = new HashSet<int>();
                }
            }
            */

            return MatrixPermKey;
        }

        #endregion ObsoleteDeprecated

    }

}
