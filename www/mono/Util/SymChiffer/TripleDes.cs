using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

        public static byte[] EncryptBytes(byte[] inBytes, byte[] tdesKey, byte[] tdesIV)
        {
            //Create the file streams to handle the input and output files.
            MemoryStream mOut = new MemoryStream();
            mOut.SetLength(0);

            int totlen = inBytes.Length;    //This is the total length of the input file.

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            CryptoStream encStream = new CryptoStream(mOut, tdes.CreateEncryptor(tdesKey, tdesIV), CryptoStreamMode.Write);

            Console.WriteLine("Encrypting...");

            encStream.Write(inBytes, 0, totlen);
            encStream.Close();

            return encStream.ToByteArray();
        }


        public static string EncryptString(string inString, byte[] tdesKey, byte[] tdesIV)
        {
            byte[] inByzes = Convert.FromBase64String(inString);
            byte[] encryptedBytes = EncryptBytes(inByzes, tdesKey, tdesIV);
            string encryptedText = Convert.ToBase64String(encryptedBytes);
            return encryptedText;
        }

        public static byte[] DecryptBytes(byte[] cipherBytes, byte[] key, byte[] iv)
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
                    }

                    // Get the decrypted data from the MemoryStream
                    decrypted = ms.ToArray().ToList();
                }
            }

            return decrypted.ToArray();
        }


        static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            byte[] decryptedBytes = DecryptBytes(cipherBytes, key, iv);
            string plaintext = Convert.ToBase64String(decryptedBytes);
            return plaintext;
        }

    }
}
