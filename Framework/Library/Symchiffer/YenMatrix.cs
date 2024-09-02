using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Symchiffer
{

    /// <summary>
    /// Simple Matrix SymChiffer maybe already invented, but created by zen@area23.at (Heinrich Elsigan)
    /// </summary>
    public static class YenMatrix
    {

        #region fields

        private static string privateKey = string.Empty, userHostAddress = string.Empty;

        private static readonly sbyte[] MatrixBasePerm = {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
            0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f
        };

        internal static string PrivateUserKey { get => string.Concat(privateKey, privateKey); }

        internal static string PrivateUserHostKey { get => string.Concat(privateKey, userHostAddress, privateKey, userHostAddress); }

        internal static sbyte[] MatrixPermKey { get; set; }
        private static sbyte[] MatrixReverse { get; set; }

        private static HashSet<sbyte> PermKeyHash { get; set; }

        #endregion fields

        #region ctor_init_gen_reverse

        /// <summary>
        /// Static constructor
        /// </summary>
        static YenMatrix()
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
                MatrixPermKey[cntSby++] = s;
            }

            PermKeyHash = new HashSet<sbyte>(MatrixBasePerm);
            MatrixReverse = BuildReveseMatrix(MatrixPermKey);
        }

        /// <summary>
        /// GetUserKeyBytes gets symetric chiffer private byte[KeyLen] encryption / decryption key
        /// </summary>
        /// <param name="usrHostAddr">user host ip address</param>
        /// <param name="secretKey">user secret key, default email address</param>
        /// <returns>Array of byte with length KeyLen</returns>
        internal static byte[] GetUserKeyBytes(string secretKey = "postmaster@localhost", string usrHostAddr = "127.0.0.1", int keyLen = 16)
        {
            privateKey = secretKey;
            userHostAddress = usrHostAddr;

            int keyByteCnt = -1;
            string keyByteHashString = privateKey;
            byte[] tmpKey = new byte[keyLen];

            if ((keyByteCnt = Encoding.UTF8.GetByteCount(keyByteHashString)) < keyLen)
            {
                keyByteHashString = PrivateUserKey;
                keyByteCnt = Encoding.UTF8.GetByteCount(keyByteHashString);
            }
            if (keyByteCnt < keyLen)
            {
                keyByteHashString = PrivateUserHostKey;
                keyByteCnt = Encoding.UTF8.GetByteCount(keyByteHashString);
            }
            if (keyByteCnt < keyLen)
            {
                RandomNumberGenerator randomNumGen = RandomNumberGenerator.Create();
                randomNumGen.GetBytes(tmpKey, 0, keyLen);

                byte[] tinyKeyBytes = new byte[keyByteCnt];
                tinyKeyBytes = Encoding.UTF8.GetBytes(keyByteHashString);
                int tinyLength = tinyKeyBytes.Length;

                for (int bytCnt = 0; bytCnt < keyLen; bytCnt++)
                {
                    tmpKey[bytCnt] = tinyKeyBytes[bytCnt % tinyLength];
                }
            }
            else
            {
                byte[] ssSmallNotTinyKeyBytes = new byte[keyByteCnt];
                ssSmallNotTinyKeyBytes = Encoding.UTF8.GetBytes(keyByteHashString);
                int ssSmallByteCnt = ssSmallNotTinyKeyBytes.Length;

                for (int bytIdx = 0; bytIdx < keyLen; bytIdx++)
                {
                    tmpKey[bytIdx] = ssSmallNotTinyKeyBytes[bytIdx];
                }
            }

            return tmpKey;

        }

        /// <summary>
        /// Generate Matrix sym chiffre permutation with a personal key string
        /// </summary>
        /// <param name="secretKey">string key to generate permutation <see cref="MatrixPermKey"/> 
        /// and <see cref="MatrixPermSalt"/> for encryption 
        /// and reverse matrix <see cref="MatrixReverse"/> for decryption</param>
        /// <param name="init">init three fish first time with a new key</param>
        /// <returns>true, if init was with same key successfull</returns>
        public static bool YenMatrixGenWithKey(string secretKey = "", string userHostAddr = "", bool init = true) // , byte[] textForEncryption = null)
        {
            int aCnt = 0, bCnt = 0;

            if (!init)
            {
                if ((string.IsNullOrEmpty(privateKey) && !string.IsNullOrEmpty(secretKey)) ||
                    (!privateKey.Equals(secretKey, StringComparison.InvariantCultureIgnoreCase)))
                    return false;
            }

            if (init)
            {
                if (string.IsNullOrEmpty(secretKey))
                    privateKey = string.Empty;
                else
                    privateKey = secretKey;

                InitMatrixSymChiffer();

                byte[] keyBytes = (!string.IsNullOrEmpty(secretKey)) ?
                    GetUserKeyBytes(privateKey, userHostAddr, 16) :
                    GetUserKeyBytes(string.Concat(Constants.DES3_KEY, Constants.AES_KEY, Constants.BOUNCEK),
                        string.Concat(Constants.DES3_IV, Constants.AES_IV, Constants.BOUNCE4), 16);

                foreach (byte keyByte in keyBytes)
                {
                    sbyte b = (sbyte)(((int)keyByte) % 16);
                    if (!PermKeyHash.Contains(b))
                    {
                        PermKeyHash.Add(b);
                        aCnt = (int)b;
                        if (aCnt != bCnt)
                        {
                            sbyte sba = MatrixPermKey[aCnt];
                            sbyte sbb = MatrixPermKey[bCnt];
                            SwapSBytes(ref sba, ref sbb);
                            MatrixPermKey[aCnt] = sba;
                            MatrixPermKey[bCnt] = sbb;
                        }
                        bCnt++;
                    }
                }

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
                aCnt = 0;
                for (bCnt = offSet; bCnt < offSet + len; bCnt++)
                {
                    byte b = inBytesPadding[bCnt];
                    MapByteValue(ref b, out byte mapEncryptB, true);
                    sbyte sm = MatrixPermKey[aCnt];
                    processedEncrypted[(int)sm] = mapEncryptB;
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
                    MapByteValue(ref b, out byte mapDecryptB, false);
                    sbyte sm = MatrixReverse[aCnt];
                    processedDecrypted[(int)sm] = mapDecryptB;
                    aCnt++;
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

            int bCnt = 0;
            long oSize = plainData.Length + (16 - (plainData.Length % 16));
            long outputSize = ((long)(oSize / 16)) * 16;
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
            if (cipherData == null || cipherData.Length <= 0)
                throw new ArgumentNullException("ZenMatrix byte[] Encrypt(byte[] cipherData): ArgumentNullException cipherData = null or Lenght 0.");

            int bCnt = 0;
            long oSize = cipherData.Length + (16 - (cipherData.Length % 16));
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

        /// <summary>
        /// SwapBytes swaps two bytes
        /// </summary>
        /// <param name="ba"></param>
        /// <param name="bb"></param>
        /// <returns></returns>
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
        internal static sbyte[] SwapSBytes(ref sbyte a0, ref sbyte b1)
        {
            sbyte[] tmp = new sbyte[2];
            tmp[0] = Convert.ToSByte(b1.ToString());
            tmp[1] = Convert.ToSByte(a0.ToString());
            a0 = tmp[0];
            b1 = tmp[1];
            return tmp;
        }

        #endregion SwapHelpers


        #region ObsoleteDeprecated

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
                        sbyte sba = MatrixPermKey[aCnt];
                        sbyte sbb = MatrixPermKey[bCnt];
                        SwapSBytes(ref sba, ref sbb);
                        MatrixPermKey[aCnt] = sba;
                        MatrixPermKey[bCnt] = sbb;
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
