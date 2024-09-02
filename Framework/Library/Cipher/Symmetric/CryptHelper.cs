using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Area23.At.Framework.Library.Cipher.Symmetric
{

    /// <summary>
    /// static class CryptHelper provides static helper methods for encryption / decryption
    /// </summary>
    public static class CryptHelper
    {

        /// <summary>
        /// BytesBytes, a static method inspired by strstr 
        /// </summary>
        /// <param name="needle">byte[] of needle to find</param>
        /// <param name="hayStack">byte[] of haystack to search through</param>
        /// <param name="matchBytes">match the only first matchBytes of needle, -1 for all bytes</param>
        /// <returns>index of first byte of matching needle in haystack</returns>
        public static int BytesBytes(byte[] needle, byte[] hayStack, int matchBytes = -1)
        {
            if (needle == null || needle.Length == 0 || hayStack == null || hayStack.Length == 0 || needle.Length > hayStack.Length)
                return -1;

            int needleIt = 0;
            for (int fFwdIt = 0; fFwdIt < hayStack.Length - needle.Length; fFwdIt++)
            {
                if (hayStack[fFwdIt] == needle[needleIt])
                {
                    if (needle.Length == 1)
                        return fFwdIt;

                    for (needleIt = 1; needleIt < needle.Length; needleIt++)
                    {
                        if (hayStack[fFwdIt + needleIt] != needle[needleIt])
                        {
                            needleIt = 0;
                            break;
                        }
                        if (matchBytes > 0 && needleIt == matchBytes)
                            return fFwdIt;

                        if (needleIt >= (needle.Length - 1))
                            return fFwdIt;
                    }
                }
            }

            return -1;
        }

        public static byte[] TarBytes(byte[] baseBytes, params byte[][] bytesToAdd)
        {
            List<byte> largeBytesList = new List<byte>(baseBytes);

            foreach (byte[] bs in bytesToAdd)
            {
                largeBytesList.AddRange(bs);
            }

            return largeBytesList.ToArray();
        }

        /// <summary>
        /// KeyHexString transforms a private secret key to hex string
        /// </summary>
        /// <param name="key">private secret key</param>
        /// <returns>hex string of bytes</returns>
        public static string KeyHexString(string key)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
            string ivStr = keyBytes.ToHexString();
            return ivStr;
        }

        /// <summary>
        /// GetBytesFromString gets byte[] array representing binary transformation of a string
        /// </summary>
        /// <param name="inString">string to transfer to binary byte[] data</param>
        /// <param name="blockSize">current block size, default: 256</param>
        /// <param name="upStretchToCorrectBlockSize">fills at the end of byte[] padding zero 0 bytes, default: false</param>
        /// <returns>byte[] array of binary byte</returns>
        public static byte[] GetBytesFromString(string inString, int blockSize = 256, bool upStretchToCorrectBlockSize = false)
        {
            string sourceString = (string.IsNullOrEmpty(inString)) ? string.Empty : inString;
            byte[] sourceBytes = System.Text.Encoding.UTF8.GetBytes(sourceString);
            int inBytesLen = sourceBytes.Length;
            if (blockSize == 0)
                blockSize = 256;
            else if (blockSize < 0)
                blockSize = Math.Abs(blockSize);

            if (upStretchToCorrectBlockSize)
            {
                int mul = ((int)(sourceBytes.Length / blockSize));
                double dDiv = (double)(sourceBytes.Length / blockSize);
                int iFactor = (int)Math.Min(Math.Truncate(dDiv), Math.Round(dDiv));
                inBytesLen = (iFactor + 1) * blockSize;
            }

            byte[] inBytes = new byte[inBytesLen];
            for (int bytCnt = 0; bytCnt < inBytesLen; bytCnt++)
            {
                inBytes[bytCnt] = (bytCnt < sourceBytes.Length) ? sourceBytes[bytCnt] : (byte)0;
            }

            return inBytes;
        }

        /// <summary>
        /// GetStringFromBytesTrimNulls gets a plain text string from binary byte[] data and truncate all 0 byte at the end.
        /// </summary>
        /// <param name="decryptedBytes">decrypted byte[]</param>
        /// <returns>truncated string without a lot of \0 (null) characters</returns>
        public static string GetStringFromBytesTrimNulls(byte[] decryptedBytes)
        {
            int ig = -1;
            string decryptedText = string.Empty;

            if ((ig = decryptedBytes.ArrayIndexOf((byte)0)) > 0)
            {
                byte[] decryptedNonNullBytes = new byte[ig];
                Array.Copy(decryptedBytes, decryptedNonNullBytes, ig);
                decryptedText = System.Text.Encoding.UTF8.GetString(decryptedNonNullBytes);
            }
            else
                decryptedText = System.Text.Encoding.UTF8.GetString(decryptedBytes);

            return decryptedText;
        }

        /// <summary>
        /// GetBytesTrimNulls gets a byte[] from binary byte[] data and truncate all 0 byte at the end.
        /// </summary>
        /// <param name="decryptedBytes">decrypted byte[]</param>
        /// <returns>truncated byte[] without a lot of \0 (null) characters</returns>
        public static byte[] GetBytesTrimNulls(byte[] decryptedBytes)
        {
            int ig = -1;
            byte[] decryptedNonNullBytes = null;

            if ((ig = decryptedBytes.ArrayIndexOf((byte)0)) > 0)
            {
                decryptedNonNullBytes = new byte[ig];
                Array.Copy(decryptedBytes, decryptedNonNullBytes, ig);
            }
            else
                decryptedNonNullBytes = decryptedBytes;

            return decryptedNonNullBytes;
        }

        /// <summary>
        /// Trim_Decrypted_Text removes all special control characters from a text string
        /// </summary>
        /// <param name="decryptedText">string to trim and strip from special control characters.</param>
        /// <returns>text only string with at least text formation special characters.</returns>
        public static string Trim_Decrypted_Text(string decryptedText)
        {
            int ig = 0;
            List<char> charList = new List<char>();
            for (int i = 1; i < 32; i++)
            {
                char ch = (char)i;
                if (ch != '\v' && ch != '\f' && ch != '\t' && ch != '\r' && ch != '\n')
                    charList.Add(ch);
            }
            char[] chars = charList.ToArray();
            decryptedText = decryptedText.TrimEnd(chars);
            decryptedText = decryptedText.TrimStart(chars);
            decryptedText = decryptedText.Replace("\0", "");
            foreach (char ch in chars)
            {
                while ((ig = decryptedText.IndexOf(ch)) > 0)
                {
                    decryptedText = decryptedText.Substring(0, ig) + decryptedText.Substring(ig + 1);
                }
            }

            return decryptedText;
        }

    
        public static IBlockCipher GetBlockCipher(string requestedAlgorithm, ref string mode, ref int blockSize, ref int keyLen)
        {
            IBlockCipher blockCipher = null;
            if (string.IsNullOrEmpty(mode))
                mode = "ECB";
            if (blockSize < 64)
                blockSize = 256;
            if (keyLen < 8) 
                keyLen = 32;

            switch (requestedAlgorithm)
            {
                case "Camellia":
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.CamelliaEngine();
                    break;
                case "Cast5":
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.Cast5Engine();
                    break;
                case "Cast6":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.Cast6Engine();
                    break;
                case "Gost28147":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.Gost28147Engine();
                    break;
                case "Idea":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.IdeaEngine();
                    break;
                case "Noekeon":
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.NoekeonEngine();
                    break;
                case "RC2":
                    blockSize = 128;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC2Engine();
                    break;
                case "RC532":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC532Engine();
                    break;
                //case "RC564":
                //    blockSize = 256;
                //    keyLen = 32;
                //    mode = "ECB";
                //    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC564Engine();
                //    break;
                case "RC6":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.RC6Engine();
                    break;
                case "Seed":
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.SeedEngine();
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    break;
                //case "Serpent":
                //    blockCipher = new Org.BouncyCastle.Crypto.Engines.SerpentEngine();
                //    blockSize = 256;
                //    keyLen = 16;
                //    mode = "ECB";
                //    break;
                case "Skipjack":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.SkipjackEngine();
                    break;
                case "Tea":
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.TeaEngine();
                    break;
                case "Tnepres":
                    blockSize = 128;
                    keyLen = 16;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.TnepresEngine();
                    break;
                case "XTea":
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.XteaEngine();
                    break;
                case "Rijndael":
                default:
                    blockSize = 256;
                    keyLen = 32;
                    mode = "ECB";
                    blockCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();
                    break;
            }


            return blockCipher;
        }


        public static CryptParams GetBlockCipher(string requestAlgorithm)
        {
            CryptParams cryptParams = new CryptParams(requestAlgorithm);
            return cryptParams;
        }


        public static IBlockCipher GetBlockCipher(CryptParams cryptParams)
        {
            CryptParams cParams = new CryptParams(cryptParams.AlgorithmName);
            return cryptParams.BlockChipher;
        }


        #region GetUserKeyBytes

        /// <summary>
        /// PrivateUserKey, helper to double private secret key to get a longer byte[]
        /// </summary>
        /// <param name="secretKey">users private secret key</param>
        /// <returns>doubled concatendated string of secretKey</returns>
        internal static string PrivateUserKey(string secretKey)
        {
            string secKey = string.IsNullOrEmpty(secretKey) ? Constants.AUTHOR_EMAIL : secretKey;
            return string.Concat(secKey, secKey);
        }

        /// <summary>
        /// PrivateKeyWithUserHash, helper to double private secret key with hash
        /// </summary>
        /// <param name="secretKey">users private secret key</param>
        /// <param name="userHash">users private secret key hash</param>
        /// <returns>doubled concatendated string of (secretKey + hash)</returns>
        internal static string PrivateKeyWithUserHash(string secretKey, string userHash)
        {
            string secKey = string.IsNullOrEmpty(secretKey) ? Constants.AUTHOR_EMAIL : secretKey;
            string usrHash = string.IsNullOrEmpty(userHash) ? Constants.AREA23_EMAIL : userHash;

            return string.Concat(secKey, usrHash, secKey, usrHash);
        }


        /// <summary>
        /// GetUserKeyBytes gets symetric chiffer private byte[KeyLen] encryption / decryption key
        /// </summary>
        /// <param name="secretKey">user secret key, default email address</param>
        /// <param name="usrHostAddr">user host ip address</param>
        /// <returns>Array of byte with length KeyLen</returns>
        public static byte[] GetUserKeyBytes(string secretKey = "postmaster@localhost", string usrHostAddr = "127.0.0.1", int keyLen = 32)
        {
            
            int keyByteCnt = -1;
            string keyByteHashString = secretKey;
            byte[] tmpKey = new byte[keyLen];

            if ((keyByteCnt = Encoding.UTF8.GetByteCount(keyByteHashString)) < keyLen)
            {
                keyByteHashString = PrivateUserKey(secretKey);
                keyByteCnt = Encoding.UTF8.GetByteCount(keyByteHashString);
            }
            if (keyByteCnt < keyLen)
            {
                keyByteHashString = PrivateKeyWithUserHash(secretKey, usrHostAddr);
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

        #endregion GetUserKeyBytes


    }

}
