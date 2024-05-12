using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Area23.At.Mono.Util.SymChiffer
{
    /// <summary>
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-8.0" />
    /// </summary>
    public static class Aes
    {
        static public byte[] AesKey { get; set; }

        static public byte[] AesIv { get; set; }

        public static RijndaelManaged AesAlgo { get; private set; }

        static Aes()
        {
            AesKey = Convert.FromBase64String(ResReader.GetValue(Constants.AES_KEY));
            AesIv = Convert.FromBase64String(ResReader.GetValue(Constants.AES_IV));
            AesAlgo = new RijndaelManaged();
            AesAlgo.Mode = CipherMode.CBC;
            AesAlgo.KeySize = 256;
            AesAlgo.Padding = PaddingMode.PKCS7;
            // AesAlgo.GenerateIV();
            // AesAlgo.GenerateKey();
            AesAlgo.Key = AesKey;
            AesAlgo.IV = AesIv;
        }


        public static byte[] Encrypt(byte[] inBytes)
        {
            // Check arguments. 
            if (inBytes == null || inBytes.Length <= 0)
                throw new ArgumentNullException("inBytes");
            byte[] encrypted;
            // Create an RijndaelManaged object 
            // with the specified key and IV.                       
            // Create a decryptor to perform the stream transform.
            ICryptoTransform encryptor = AesAlgo.CreateEncryptor(AesAlgo.Key, AesAlgo.IV);

            // Create the streams used for encryption. 
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(inBytes, 0, inBytes.Length);
                    csEncrypt.Flush();
                    encrypted = msEncrypt.ToArray();
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return encrypted;
        }

        public static string EncryptString(string plainText)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            byte[] encrypted;
             
            // Create a decryptor to perform the stream transform.
            ICryptoTransform encryptor = AesAlgo.CreateEncryptor(AesAlgo.Key, AesAlgo.IV);

            // Create the streams used for encryption. 
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                    // only to avoid compiler warnings, not needed really because it's surrounded with using(..)
                    csEncrypt.Close();
                    csEncrypt.Dispose();
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return Convert.ToBase64String(encrypted);

        }


        public static byte[] Decrypt(byte[] cipherBytes)
        {
            // Check arguments. 
            if (cipherBytes == null || cipherBytes.Length <= 0)
                throw new ArgumentNullException("cipherBytes");            

            byte[] outBytes = null;
            ICryptoTransform decryptor = AesAlgo.CreateDecryptor(AesAlgo.Key, AesAlgo.IV);

            using (MemoryStream msDecryptStr = new MemoryStream(cipherBytes))
            {
                using (CryptoStream csDecryptStr = new CryptoStream(msDecryptStr, decryptor, CryptoStreamMode.Read))
                {
                    // csDecryptStr.Read(outBytes, 0, (int)csDecryptStr.Length);
                    using (var msPlain = new System.IO.MemoryStream())
                    {
                        csDecryptStr.CopyTo(msPlain);
                        outBytes = msPlain.ToArray();
                    }
                }
            }

            return outBytes;
        }


        public static string DecryptString(string cipherText)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            string plaintext = null;

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = AesAlgo.CreateDecryptor(AesAlgo.Key, AesAlgo.IV);

            byte[] chipherBytes = Convert.FromBase64String(cipherText);
            // Create the streams used for decryption. 
            using (MemoryStream msDecryptStr = new MemoryStream(chipherBytes))
            {
                using (CryptoStream csDecryptStr = new CryptoStream(msDecryptStr, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecryptStr))
                    {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                    csDecryptStr.Close();
                    csDecryptStr.Dispose();
                }
            }

            return plaintext;
        }

    }
}