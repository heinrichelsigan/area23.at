using Area23.At.Framework.Library.Core.Cipher.Symm;
using Area23.At.Framework.Library.Core.EnDeCoding;
using Area23.At.Framework.Library.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Net.CqrJd
{

    public class CqrSrvrMes
    {
        byte[] secKey = new byte[16];
        SymmCipherEnum[] symCiphers = new SymmCipherEnum[8];
        
        private string HashSymms
        {
            get
            {
                string sout = string.Empty;
                foreach (var cipher in symCiphers)
                {
                    ushort u = (ushort)cipher;
                    sout += ((char)((ushort)'y' - u)).ToString();
                }
                return sout;
            }
        }

        public CqrSrvrMes(string srvKey) 
        {
            byte[] bts = EnDeCoder.GetBytes(srvKey);
            Array.Copy(bts, secKey, Math.Min(bts.Length, 16));
            symCiphers = Cipher.Symm.Crypt.KeyBytesToSymmCipherPipeline(secKey, 8);
        }


        public string CqrMessage(string msg, EnDeCoding.EncodingType encType = EncodingType.Base64)
        {
            byte[] msgBytes = DeEnCoder.GetBytesFromString(msg, 64, true);
            byte[] nullBytes = new byte[8]; 
            for (ushort ib = 0; ib < 8; ib++) nullBytes[ib] = (byte)0;
            
            byte[] tarBytes = msgBytes.TarBytes(EnDeCoder.GetBytes(HashSymms));
            // HashSymms 
            byte[] cqrbytes = Cipher.Symm.Crypt.MerryGoRoundEncrpyt(tarBytes, EnDeCoder.GetString(secKey), DeEnCoder.KeyToHex(EnDeCoder.GetString(secKey)));

            EnDeCoding.DeEnCoder.EncodeBytes(cqrbytes.TarBytes(nullBytes), encType);
            return EnDeCoding.DeEnCoder.EncodeBytes(cqrbytes, encType);
        }


        public string NCqrMessage(string cqrMessage, EnDeCoding.EncodingType encType = EncodingType.Base64)
        {
            byte[] inBytes = EnDeCoding.DeEnCoder.DecodeText(cqrMessage, encType);
            byte[] cipherBytes = EnDeCoding.DeEnCoder.GetBytesTrimNulls(inBytes);
            byte[] unroundedMerryBytes = Cipher.Symm.Crypt.DecrpytRoundGoMerry(cipherBytes, EnDeCoder.GetString(secKey), DeEnCoder.KeyToHex(EnDeCoder.GetString(secKey)));
            byte[] hashSymBytes = new byte[8];
            Array.Copy(unroundedMerryBytes, unroundedMerryBytes.Length - 8, hashSymBytes, 0, 8);
            if (EnDeCoder.GetString(hashSymBytes).Equals(HashSymms, StringComparison.Ordinal))
            {
                string hashSymShow = HashSymms ?? "        ";
                hashSymShow = hashSymShow.Substring(0, 2) + "...." + hashSymShow.Substring(6);
      
                throw new InvalidOperationException($"SymmCiphers [{hashSymShow}] in crypt pipeline doesn't match serverside key !?$* byte length ={secKey.Length}");
            }
            byte[] outBytes = new byte[unroundedMerryBytes.Length - 8]; 
            Array.Copy(unroundedMerryBytes, 0, outBytes, 0, unroundedMerryBytes.Length - 8);
            return DeEnCoder.GetStringFromBytesTrimNulls(outBytes);
        }

    }

}
