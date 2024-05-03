using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Area23.At.Mono.Util.SymChiffer
{
    /// <summary>
    /// <see cref="https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.tripledes.-ctor?view=net-8.0" />
    /// <seealso cref="https://www.c-sharpcorner.com/article/tripledes-encryption-and-decryption-in-c-sharp/ "/>
    /// </summary>
    public class TripleDes
    {
        const string secretKey = "My Secret Key";
        public static byte[] toDecryptArray;

        static public byte[] DesKey { get; set; }

        static public byte[] DesIv { get; set; }


        static TripleDes()
        {
            // Generate a key using SHA256 hash function
            byte[] key = new byte[16];
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(secretKey));
                Array.Copy(hash, key, 16);
            }

            // Generate a IV using SHA256 hash function
            byte[] iv = new byte[8];
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(secretKey));
                Array.Copy(hash, iv, 8);
            }
            DesIv = iv;
            DesKey = key;
        }

        public static byte[] Encrypt(byte[] inBytes, byte[] tdesKey, byte[] tdesIV)
        {
            //Create the file streams to handle the input and output files.
            MemoryStream mOut = new MemoryStream();
            mOut.SetLength(0);

            int totlen = inBytes.Length;    //This is the total length of the input file.

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(mOut, tdes.CreateEncryptor(tdesKey, tdesIV), CryptoStreamMode.Write);

            Console.WriteLine("Encrypting...");

            encStream.Write(inBytes, 0, totlen);
            encStream.Flush();
            mOut.Flush();
            byte[] outBytes = mOut.ToByteArray();
            encStream.Close();

            return outBytes;
        }


        public static string EncryptString(string toEncrypt)
        {
            byte[] keyArray = TripleDes.DesKey;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);


            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string EncryptString(string inString, byte[] tdesKey, byte[] tdesIV)
        {
            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(inString);
            byte[] encryptedBytes = Encrypt(inBytes, tdesKey, tdesIV);
            string encryptedText = Convert.ToBase64String(encryptedBytes);
            // System.Text.Encoding.ASCII.GetString(encryptedBytes).TrimEnd('\0');
            return encryptedText;
        }

        public static byte[] Decrypt(byte[] cipherBytes, byte[] key, byte[] iv)
        {
            // Check arguments. 
            if (cipherBytes == null || cipherBytes.Length <= 0)
                throw new ArgumentNullException("cipherBytes");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("IV");

            // Decypted byte list
            List<byte> decrypted = new List<byte>();

            using (TripleDES tdes = TripleDES.Create())
            {
                // Create a decryptor
                ICryptoTransform decryptor = tdes.CreateDecryptor(key, iv);

                // Create a MemoryStream
                using (MemoryStream ms = new MemoryStream())
                {
                    // Create a CryptoStream
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        // Write the bytes to the CryptoStream
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.FlushFinalBlock();
                    }

                    // Get the decrypted data from the MemoryStream
                    decrypted = ms.ToArray().ToList();
                    ms.Dispose();
                }
            }

            return decrypted.ToArray();
        }


        public static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = Decrypt(cipherBytes, key, iv);
            string plaintext = System.Text.Encoding.ASCII.GetString(decryptedBytes).TrimEnd('\0');
            return plaintext;
        }


        public static string DecryptString(string cipherString)
        {
            byte[] keyArray = TripleDes.DesKey;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;
            toDecryptArray = new byte[toEncryptArray.Length * 2];
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
