using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Windows.Input;

namespace Area23.At.Mono.Util.SymChiffer
{
    /// <summary>
    /// Simple Matrix SymChiffer maybe already invented, but created by zen@area23.at (Heinrich Elsigan)
    /// </summary>
    public static class MatrixSymChiffer
    {
        internal static readonly sbyte[] MatrixBasePerm = { 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf };

        internal static sbyte[] MatrixPermKey { get; private set; }

        internal static sbyte[] MatrixReverse { get; private set; }

        internal static sbyte[] PermKeySalt { get; private set; }

        internal static HashSet<sbyte> PermKeyHash { get; private set; }


        static MatrixSymChiffer()
        {
            InitMatrixPermutations();
            sbyte[] NatrixPermSalt = GetMatrixPermutation(null);
            MatrixReverse = BuildReveseMatrix(NatrixPermSalt);
        }


        internal static void InitMatrixPermutations()
        {
            sbyte cntSby = 0x0;
            MatrixPermKey = new sbyte[16];
            PermKeySalt = new sbyte[0x10];
            foreach (sbyte s in MatrixBasePerm)
            {
                MatrixPermKey[cntSby++] = s;
            }

            PermKeyHash = new HashSet<sbyte>(MatrixBasePerm);
        }

        internal static sbyte[] SwapSByte(sbyte[] sbArray, int i, int j)
        {
            sbyte[] tmp = null;
            if (sbArray != null && sbArray.Length >= 16)
            {
                tmp = new sbyte[2];
                tmp[0] = sbArray[i];
                tmp[1] = sbArray[j];
                sbArray[i] = tmp[1];
                sbArray[j] = tmp[0];
            }
            return tmp;
        }

        internal static sbyte[] GetMatrixPermutation(sbyte[] salt)
        {
            InitMatrixPermutations();
            for (int i = 0x0; i < MatrixBasePerm.Length; i += 3)
            {
                for (int j = MatrixBasePerm.Length - 1; j >= i; j -= 2)
                {
                    SwapSByte(MatrixPermKey, i, j);
                }
            }

            HashSet<sbyte> takenSBytes = new HashSet<sbyte>();
            HashSet<int> dicedPos = new HashSet<int>();
            for (int randomizeCnt = 0; randomizeCnt <= 0x3f; randomizeCnt++)
            {
                Random rand = new Random(System.DateTime.UtcNow.Millisecond);
                int pos = (int)rand.Next(0x0, 0xf);
                while (dicedPos.Contains(pos))
                {
                    pos = (int)rand.Next(0x0, 0xf);
                }
                dicedPos.Add(pos);
                sbyte talenS = PermKeyHash.ElementAt(pos);
                takenSBytes.Add(talenS);
                if (takenSBytes.Count > 0xf)
                {
                    PermKeySalt = new sbyte[16];
                    takenSBytes.CopyTo(PermKeySalt, 16);
                    PermKeyHash = new HashSet<sbyte>(PermKeySalt);
                    takenSBytes = new HashSet<sbyte>();
                    dicedPos = new HashSet<int>();
                }
            }

            return MatrixPermKey;
        }

        internal static sbyte[] BuildReveseMatrix(sbyte[] matrix)
        {
            sbyte[] rmatrix = null;
            if (matrix != null && matrix.Length > 7)
            {
                rmatrix = new sbyte[matrix.Length];
                for (int m = 0; m < matrix.Length; m++)
                {
                    sbyte sm = matrix[m];
                    rmatrix[(int)sm] = (sbyte)m;
                }
            }
            return rmatrix;
        }


        public static byte[] ProcessEncryptBytes(byte[] inBytesPadding, int offSet = 0, int len = 16)
        {
            int aCnt = 0, bCnt = 0;
            byte[] processedEncrypted = null;
            if (offSet < inBytesPadding.Length && offSet + len <= inBytesPadding.Length)
            {
                processedEncrypted = new byte[len];
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

        public static byte[] ProcessDecryptBytes(byte[] inBytesEncrypted, int offSet = 0, int len = 16)
        {
            int aCnt = 0, bCnt = 0;
            byte[] processedDecrypted = null;
            if (offSet < inBytesEncrypted.Length && offSet + len <= inBytesEncrypted.Length)
            {
                processedDecrypted = new byte[len];
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
            int outputSize = plainData.Length + (16 - plainData.Length % 16);
            byte[] inBytesPadding = new byte[outputSize];
            byte[] outBytesEncrypted = new byte[outputSize];
            for (bCnt = 0; bCnt < inBytesPadding.Length; bCnt++)
            {
                inBytesPadding[bCnt] = (byte)0x0;
                outBytesEncrypted[bCnt] = (byte)0x0;
            }
            plainData.CopyTo(inBytesPadding, plainData.Length);
            for (int processCnt = 0; processCnt < inBytesPadding.Length; processCnt += 16)
            {
                byte[] retByte = ProcessEncryptBytes(inBytesPadding, processCnt, 16);
                foreach (byte rb in retByte)
                {
                    outBytesEncrypted[outCnt++] = rb;
                }
            }

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
            int outputSize = cipherData.Length + (16 - cipherData.Length % 16);
            byte[] inBytesEncrypted = new byte[outputSize];
            byte[] outBytesPlainPadding = new byte[outputSize];
            for (bCnt = 0; bCnt < inBytesEncrypted.Length; bCnt++)
            {
                inBytesEncrypted[bCnt] = (byte)0x0;
                outBytesPlainPadding[bCnt] = (byte)0x0;
            }
            cipherData.CopyTo(inBytesEncrypted, cipherData.Length);
            for (int processCnt = 0; processCnt < inBytesEncrypted.Length; processCnt += 16)
            {
                byte[] retByte = ProcessDecryptBytes(inBytesEncrypted, processCnt, 16);
                foreach (byte rb in retByte)
                {
                    outBytesPlainPadding[outCnt++] = rb;
                }
            }

            return outBytesPlainPadding;

        }

    }
}