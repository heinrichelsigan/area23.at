using System;
using System.Collections.Generic;
using System.Linq;

namespace Area23.At.Mono.Util.SymChiffer
{
    /// <summary>
    /// Simple Matrix SymChiffer maybe already invented, but created by zen@area23.at (Heinrich Elsigan)
    /// </summary>
    public static class MatrixSymChiffer
    {
        internal static readonly sbyte[] MatrixBasePerm = { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf };

        internal static sbyte[] MatrixPermKey { get; set; }

        internal static sbyte[] MatrixReverse { get; set; }

        internal static HashSet<sbyte> PermKeyHash { get; set; }

        /// <summary>
        /// Static constructor
        /// </summary>
        static MatrixSymChiffer()
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
            int iCnt = 0;
            MatrixPermKey = new sbyte[16];
            foreach (sbyte s in MatrixBasePerm)
            {
                MatrixPermKey[cntSby++] = s;
            }

            PermKeyHash = new HashSet<sbyte>(MatrixBasePerm);            

            MatrixReverse = BuildReveseMatrix(MatrixPermKey);
        }

        /// <summary>
        /// BuildReveseMatrix, builds the determinant decryption matrix for sbyte{16] encryption matrix
        /// </summary>
        /// <param name="matrix">sbyte{16] encryption matrix</param>
        /// <returns>sbyte{16] decryption matrix</returns>
        internal static sbyte[] BuildReveseMatrix(sbyte[] matrix)
        {
            sbyte[] rmatrix = new sbyte[matrix.Length];
            if (matrix != null && matrix.Length >= 12)
            {
                for (int m = 0; m < matrix.Length; m++)
                {
                    sbyte sm = matrix[m];
                    rmatrix[(int)sm] = (sbyte)m;
                }
            }
            return rmatrix;
        }

        /// <summary>
        /// Generate Matrix sym chiffre permutation by key string
        /// </summary>
        /// <param name="key">string key to generate permutation <see cref="MatrixPermKey"/> 
        /// and <see cref="MatrixPermSalt"/> for encryption 
        /// and reverse matrix <see cref="MatrixReverse"/> for decryption</param>
        /// <returns>sbyte[] permutation matrix for encryption</returns>
        public static sbyte[] GenerateMatrixPermutationByKey(string key) 
        {
            int aCnt = 0, bCnt = 0;           

            InitMatrixSymChiffer();

            PermKeyHash = new HashSet<sbyte>();
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
            foreach (byte b in keyBytes)
            {
                sbyte sb = (sbyte)(((int)b) % 16);
                if (!PermKeyHash.Contains(sb))
                {
                    PermKeyHash.Add(sb);
                    aCnt = (int)sb;
                    if (aCnt != bCnt)
                    {
                        sbyte ba = MatrixPermKey[aCnt];
                        sbyte bb = MatrixPermKey[bCnt];
                        SwapSByte(ref ba, ref bb);
                        MatrixPermKey[aCnt] = ba;
                        MatrixPermKey[bCnt] = bb;
                    }
                    bCnt++;
                }
            }

            MatrixReverse = BuildReveseMatrix(MatrixPermKey);

            return MatrixPermKey;
        }


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
                aCnt = 0;
                for (bCnt = offSet; bCnt < offSet + len; bCnt++)
                {
                    byte b = inBytesPadding[bCnt];
                    sbyte sm = MatrixPermKey[aCnt];
                    processedEncrypted[(int)sm] = b;
                    aCnt++;
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
                aCnt = 0;
                for (bCnt = offSet; bCnt < offSet + len; bCnt++)
                {
                    byte b = inBytesEncrypted[bCnt];
                    sbyte sm = MatrixReverse[aCnt];
                    processedDecrypted[(int)sm] = b;
                    aCnt++;
                }
            }
            return processedDecrypted;
        }

        /// <summary>
        /// MatrixSymChiffer Encrypt member function
        /// </summary>
        /// <param name="plainData">plain data as <see cref="byte[]"/></param>
        /// <returns>encrypted data <see cref="byte[]">bytes</see></returns>
        public static byte[] Encrypt(byte[] plainData)
        {
            int bCnt = 0, outCnt = 0;
            int oSize = plainData.Length + (16 - (plainData.Length % 16));
            int outputSize = ((int)(oSize / 16)) * 16;
            byte[] inBytesPadding = new byte[outputSize];
            for (bCnt = 0; bCnt < inBytesPadding.Length; bCnt++)
            {
                if (bCnt < plainData.Length)
                    inBytesPadding[bCnt] = plainData[bCnt];
                else
                    inBytesPadding[bCnt] = (byte)0x0;
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
            int bCnt = 0, outCnt = 0;
            int oSize = cipherData.Length + (16 - (cipherData.Length % 16));
            int outputSize = ((int)(oSize / 16)) * 16;
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


        #region EnDecryptString

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="inPlainString">plain text string</param>
        /// <returns>Base64 encoded encrypted byte[]</returns>
        public static string EncryptString(string inPlainString)
        {
            byte[] plainTextData = System.Text.Encoding.UTF8.GetBytes(inPlainString);
            byte[] encryptedData = Encrypt(plainTextData);
            string encryptedString = Convert.ToBase64String(encryptedData);
            // System.Text.Encoding.ASCII.GetString(encryptedData).TrimEnd('\0');
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
            //  System.Text.Encoding.UTF8.GetBytes(inCryptString);
            byte[] plainTextData = Decrypt(cryptData);
            string plainTextString = System.Text.Encoding.ASCII.GetString(plainTextData).TrimEnd('\0');
            return plainTextString;
        }

        #endregion EnDecryptString


        #region ObsoleteDeprecated

        // [Obsolete("SwapSBytes is obsolete", false)]
        internal static sbyte[] SwapSByte(ref sbyte sba, ref sbyte sbb)
        {
            sbyte[] tmp = new sbyte[2];
            tmp[0] = Convert.ToSByte(sba.ToString());
            tmp[1] = Convert.ToSByte(sbb.ToString());
            sba = tmp[1];
            sbb = tmp[0];
            return tmp;
        }

        [Obsolete("GetMatrixPermutation is obsolete, use GenerateMatrixPermutationByKey(string key) instead!", false)]
        internal static sbyte[] GetMatrixPermutation(string key)
        {            
            InitMatrixSymChiffer();

            int aCnt = 0, bCnt = 0;

            InitMatrixSymChiffer();

            PermKeyHash = new HashSet<sbyte>();
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
            foreach (byte b in keyBytes)
            {
                sbyte sb = (sbyte)(((int)b) % 16);
                if (!PermKeyHash.Contains(sb))
                {
                    PermKeyHash.Add(sb);
                    aCnt = (int)sb;
                    if (aCnt != bCnt)
                    {
                        sbyte ba = MatrixPermKey[aCnt];
                        sbyte bb = MatrixPermKey[bCnt];
                        SwapSByte(ref ba, ref bb);
                        MatrixPermKey[aCnt] = ba;
                        MatrixPermKey[bCnt] = bb;
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