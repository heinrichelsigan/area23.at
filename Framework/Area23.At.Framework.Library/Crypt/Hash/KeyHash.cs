using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;

namespace Area23.At.Framework.Library.Crypt.Hash
{

    /// <summary>
    /// KeyHash 
    /// </summary>
    [Serializable]
    [DefaultValue(KeyHash.Hex)]
    public enum KeyHash : short
    {
        Empty =         0x00,
        Hex =           0x01,
        Sha1 =          0x02,
        OpenBSDCrypt =  0x03,
        BCrypt =        0x04,
        SCrypt =        0x05,
        MD5 =           0x06,
        Sha256 =        0x07,
        Sha384 =        0x08,
        Oct =           0x09,
        Sha512 =        0x0a,
        Whirlpool =     0x0b,
        Blake2xs =      0x0c,
        CShake =        0x0d,
        Dstu7564 =      0x0e,
        RipeMD256 =     0x0f,
        TupleHash =     0x11
    }

    public static class KeyHash_Extensions
    {

        private static readonly KeyHash[] keyHashes = { KeyHash.Empty,
                KeyHash.BCrypt, KeyHash.Blake2xs, KeyHash.CShake, KeyHash.Dstu7564,
                KeyHash.MD5, KeyHash.Hex, KeyHash.Oct, KeyHash.OpenBSDCrypt,
                KeyHash.SCrypt, KeyHash.Sha1, KeyHash.Sha256, KeyHash.Sha384, KeyHash.Sha512,
                KeyHash.RipeMD256, KeyHash.TupleHash, KeyHash.Whirlpool };

        private static readonly KeyHash[] secureHashes = {
                KeyHash.BCrypt, KeyHash.Blake2xs, KeyHash.CShake, KeyHash.Dstu7564,
                KeyHash.OpenBSDCrypt, KeyHash.SCrypt, KeyHash.RipeMD256, KeyHash.Whirlpool };


        public static ListItem[] KeyHashListItems
        {
            get
            {
                List<ListItem> list = new List<ListItem>();
                foreach (KeyHash keyHash in GetHashes())
                {
                    list.Add(new KeyHashData(keyHash.ToString(), false, true).WebUIListItem);
                }

                return list.ToArray();
            }
        }

        public static KeyHash[] GetHashes() => keyHashes;

        public static KeyHash[] GetSecureHashes() => secureHashes;

        public static KeyHash[] GetHashTypes()
        {
            List<KeyHash> list = new List<KeyHash>();
            foreach (string hashName in Enum.GetNames(typeof(KeyHash)))
            {
                list.Add((KeyHash)Enum.Parse(typeof(KeyHash), hashName));
            }

            return list.ToArray();
        }

        public static KeyHash GetHashType(string typeString)
        {
            return (KeyHash)Enum.Parse(typeof(KeyHash), typeString);
        }

        public static KeyHash GetKeyHashFromValue(short kValue)
        {
            kValue = (short)((kValue % 0x10));
            foreach (KeyHash kHash in GetHashTypes())
            {
                if ((short)kHash == kValue)
                    return kHash;
            }
            return KeyHash.Hex;
        }

        public static KeyHash GetKeyHashFromString(string stringToHash)
        {
            if (!string.IsNullOrEmpty(stringToHash))
            {
                switch (stringToHash.ToLower())
                {
                    case "empty":
                    case "null":
                    case "none":
                    case "scrypt": return KeyHash.SCrypt;
                    case "bcrypt": return KeyHash.BCrypt;
                    case "openbsd":
                    case "bsdcrypt":
                    case "openbsdcrypt": return KeyHash.OpenBSDCrypt;
                    case "md5": return KeyHash.MD5;
                    case "sha1": return KeyHash.Sha1;
                    case "sha256": return KeyHash.Sha256;
                    case "sha384": return KeyHash.Sha384;
                    case "sha512": return KeyHash.Sha512;
                    case "whirlwind":
                    case "whirlpool": return KeyHash.Whirlpool;
                    case "blake2":
                    case "blake2xs": return KeyHash.Blake2xs;
                    case "shake":
                    case "cshake": return KeyHash.CShake;
                    case "dstu7564": return KeyHash.Dstu7564;
                    case "ripe":
                    case "ripe256":
                    case "ripemd256": return KeyHash.RipeMD256;
                    case "2hash":
                    case "hash2":
                    case "tuplehash":
                    case "TupleHash": return KeyHash.TupleHash;
                    case "Oct":
                    case "Octal":
                    case "octal":
                    case "oct8":
                    case "oct": return KeyHash.Oct;
                    case "hex16":
                    case "hex": return KeyHash.Hex;
                    default:
                        if (Enum.TryParse<KeyHash>(stringToHash, out KeyHash khash))
                            return khash;
                        break;
                }
            }
            return KeyHash.Hex;
        }

        /// <summary>
        /// GetKeyFromBaseBytes Extension method for byte[] baseBytes
        /// </summary>
        /// <param name="baseBytes">baseBytes from which keyBytes are extracted</param>
        /// <param name="keyLen">length of keyBytes array</param>
        /// <returns><see cref="T:byte[]">byte[] bytesKey</see></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Obsolete("byte[] GetKeyFromBaseBytes(this byte[] baseBytes, int keyLen = 32) is obsolete", true)]
        public static byte[] GetKeyFromBaseBytes(this byte[] baseBytes, int keyLen = 32)
        {
            if (baseBytes == null || baseBytes.Length == 0)
                throw new ArgumentNullException("baseBytes");

            byte[] hashBytes = EnDeCodeHelper.GetBytes(Hex16.ToHex16(baseBytes));

            int keyByteCnt = -1;
            keyLen = (keyLen > Constants.MAX_KEY_LEN) ? Constants.MAX_KEY_LEN : keyLen;
            byte[] bytesKey = new byte[keyLen];

            byte[] keyHashBytes = CryptHelper.KeyHashBytes(baseBytes, hashBytes);
            keyByteCnt = keyHashBytes.Length;
            byte[] keyHashTarBytes = new byte[keyByteCnt * 2 + 1];

            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(CryptHelper.KeyHashBytes(hashBytes, baseBytes));
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }
            if (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(
                    CryptHelper.KeyHashBytes(hashBytes, baseBytes),
                    CryptHelper.KeyHashBytes(baseBytes, hashBytes)
                );
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }

            while (keyByteCnt < keyLen)
            {
                keyHashTarBytes = keyHashBytes.TarBytes(keyHashBytes);
                keyByteCnt = keyHashTarBytes.Length;
                keyHashBytes = new byte[keyByteCnt];
                Array.Copy(keyHashTarBytes, 0, keyHashBytes, 0, keyByteCnt);
            }

            if (keyLen <= keyByteCnt)
            {
                // Array.Copy(keyHashBytes, 0, tmpKey, 0, keyLen);
                for (int bytIdx = 0; bytIdx < keyLen; bytIdx++)
                    bytesKey[bytIdx] = keyHashBytes[bytIdx];
            }

            return bytesKey;

        }

        public static string GetExtension(this KeyHash khash)
        {
            int xval = (int)khash;
            switch (khash)
            {
                case KeyHash.Empty: return "";
                case KeyHash.Hex: return ".hex";
                case KeyHash.Sha1: return ".sha1";
                case KeyHash.OpenBSDCrypt: return ".openbsdcrypt";
                case KeyHash.BCrypt: return ".bcrypt";
                case KeyHash.SCrypt: return ".scrypt";
                case KeyHash.MD5: return ".md5";
                case KeyHash.Sha256: return ".sha256";
                case KeyHash.Sha384: return ".sha384";
                case KeyHash.Oct: return ".oct";
                case KeyHash.Sha512: return ".sha512";
                case KeyHash.Whirlpool: return ".whirlpool";
                case KeyHash.Blake2xs: return ".blake2xs";
                case KeyHash.CShake: return ".cshake";
                case KeyHash.Dstu7564: return ".dstu7564";
                case KeyHash.RipeMD256: return ".ripemd256";
                case KeyHash.TupleHash: return ".tuplehash";
                default:
                    break;
            }
            return ".hex";
        }

        public static string Hash(this KeyHash hash, string stringToHash)
        {
            if (string.IsNullOrEmpty(stringToHash))
                throw new ArgumentNullException("stringToHash");
            byte[] inBytes = Encoding.UTF8.GetBytes(stringToHash);
            byte[] resBuf = new byte[0];
            IDigest digest = new Org.BouncyCastle.Crypto.Digests.NullDigest();
            switch (hash)
            {
                case KeyHash.Empty:
                    return "";

                //case KeyHash.Ascon:
                //    digest = new Org.BouncyCastle.Crypto.Digests.AsconHash256();
                //    resBuf = new byte[digest.GetDigestSize()];
                //    digest.BlockUpdate(inBytes, 0, inBytes.Length);
                //    digest.DoFinal(resBuf, 0);
                //    return Hex.ToHexString(resBuf);

                case KeyHash.BCrypt:
                    return BCrypt.HashString(stringToHash);

                case KeyHash.Blake2xs:
                    digest = new Org.BouncyCastle.Crypto.Digests.Blake2xsDigest(32, inBytes);
                    resBuf = new byte[digest.GetDigestSize()];
                    digest.BlockUpdate(inBytes, 0, inBytes.Length);
                    digest.DoFinal(resBuf, 0);
                    return Hex.ToHexString(resBuf);

                case KeyHash.CShake:
                    // digest = new Org.BouncyCastle.Crypto.Digests.CShakeDigest(256, null, null);
                    digest = new Org.BouncyCastle.Crypto.Digests.CShakeDigest(256, inBytes, CryptHelper.GetKeyBytesFromBytes(inBytes, 32));
                    resBuf = new byte[digest.GetDigestSize()];
                    digest.BlockUpdate(inBytes, 0, inBytes.Length);
                    digest.DoFinal(resBuf, 0);
                    return Hex.ToHexString(resBuf);

                case KeyHash.Dstu7564:
                    digest = new Org.BouncyCastle.Crypto.Digests.Dstu7564Digest(256);
                    resBuf = new byte[digest.GetDigestSize()];
                    digest.BlockUpdate(inBytes, 0, inBytes.Length);
                    digest.DoFinal(resBuf, 0);
                    return Hex.ToHexString(resBuf);

                case KeyHash.MD5:
                    return MD5Sum.HashString(stringToHash, "");

                case KeyHash.Oct:
                    string octString = string.Empty;
                    for (int wc = 0; wc < inBytes.Length; wc++)
                        octString += Convert.ToString(((inBytes[wc] - 32) % 64), 8);
                    return octString;

                case KeyHash.OpenBSDCrypt:
                    return OpenBSDCrypt.HashString(stringToHash);

                case KeyHash.RipeMD256:
                    digest = new Org.BouncyCastle.Crypto.Digests.RipeMD256Digest();
                    resBuf = new byte[digest.GetDigestSize()];
                    digest.BlockUpdate(inBytes, 0, inBytes.Length);
                    digest.DoFinal(resBuf, 0);
                    return Hex.ToHexString(resBuf);

                case KeyHash.SCrypt:
                    return SCrypt.HashString(stringToHash);

                case KeyHash.Sha1:
                    return Hex.ToHexString(SHA1.Create().ComputeHash(inBytes));

                case KeyHash.Sha256:
                    return Sha256Sum.HashString(stringToHash, "");

                case KeyHash.Sha384:
                    return Hex.ToHexString(SHA384.Create().ComputeHash(inBytes));

                case KeyHash.Sha512:
                    /*  Classical bouncy castle implementation of Sha512tDigest
                       digest = new Org.BouncyCastle.Crypto.Digests.Sha512tDigest(inBytes.Length);
                       resBuf = new byte[digest.GetDigestSize()];
                       digest.BlockUpdate(inBytes, 0, inBytes.Length);
                       digest.DoFinal(resBuf, 0);
                       return Hex.ToHexString(resBuf);
                   */
                    return Hex.ToHexString(SHA512.Create().ComputeHash(inBytes)); // Sha512Sum.HashString(stringToHash);


                case KeyHash.TupleHash:
                    digest = new Org.BouncyCastle.Crypto.Digests.TupleHash(256, inBytes, 32);
                    resBuf = new byte[digest.GetDigestSize()];
                    digest.BlockUpdate(inBytes, 0, inBytes.Length);
                    digest.DoFinal(resBuf, 0);
                    return Hex.ToHexString(resBuf);

                case KeyHash.Whirlpool:
                    digest = new Org.BouncyCastle.Crypto.Digests.WhirlpoolDigest();
                    resBuf = new byte[digest.GetDigestSize()];
                    digest.BlockUpdate(inBytes, 0, inBytes.Length);
                    digest.DoFinal(resBuf, 0);
                    return Hex.ToHexString(resBuf);

                //case KeyHash.Xodyak:
                //    bytes = EnDeCodeHelper.GetBytes(stringToHash);
                //    digest = new Org.BouncyCastle.Crypto.Digests.XoodyakDigest();
                //    resBuf = new byte[digest.GetDigestSize()];
                //    digest.BlockUpdate(inBytes, 0, inBytes.Length);
                //    digest.DoFinal(resBuf, 0);
                //    return = Hex.ToHexString(resBuf);                

                case KeyHash.Hex:
                default:
                    return Hex16.ToHex16(inBytes);
            }
        }

    }

    public class KeyHashData
    {
        public bool Selected { get; set; }

        public string Text { get; set; } 

        public string Value { get; set; }

        public bool Enabled { get; set; }


        public ListItem WebUIListItem  
        { 
            get => 
                new ListItem(this.Text, this.Value, this.Enabled) { Selected = this.Selected };            
        }
    
        public KeyHashData()
        {
            Enabled = true;
            Selected = false;
            Text = "";
            Value = "";
        }

        public KeyHashData(string textValue, bool selected = false, bool enabled = true) : this()
        {
            Text = textValue;
            Value = textValue;
            Enabled = enabled;
            Selected = selected;
        }

        public KeyHashData(ListItem listItem) : this()
        {
            Text = listItem.Text.ToLower();
            Value = listItem.Value;
            Enabled = listItem.Enabled;
            Selected = listItem.Selected;
        }


    }

}


