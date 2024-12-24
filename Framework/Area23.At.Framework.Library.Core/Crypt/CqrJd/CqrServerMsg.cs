using Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Core.Crypt.Cipher;
using Area23.At.Framework.Library.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Crypt.CqrJd
{

    public class CqrServerMsg
    {
        byte[] secKey = new byte[16];
        SymmCipherEnum[] symmCiphers = new SymmCipherEnum[8];

        private string hashSymms = string.Empty;

        private string HashSymms { get => SymmCrypt.SymmCipherPipeString(symmCiphers); }

        public CqrServerMsg(string srvKey)
        {
            byte[] bts = EnDeCoder.GetBytes(srvKey);
            Array.Copy(bts, secKey, Math.Min(bts.Length, 16));
            symmCiphers = SymmCrypt.KeyBytesToSymmCipherPipeline(secKey, 8);
        }


        public string CqrMessage(string msg, EncodingType encType = EncodingType.Base64)
        {
            byte[] msgBytes = DeEnCoder.GetBytesFromString(msg.Replace("\n", " \r\n"), 64, true);
            byte[] nullBytes = new byte[8];
            for (ushort ib = 0; ib < 8; ib++) nullBytes[ib] = 0;

            byte[] tarBytes = msgBytes.TarBytes(EnDeCoder.GetBytes(HashSymms));
            // HashSymms 
            byte[] cqrbytes = SymmCrypt.MerryGoRoundEncrpyt(tarBytes, EnDeCoder.GetString(secKey), DeEnCoder.KeyToHex(EnDeCoder.GetString(secKey)));

            DeEnCoder.EncodeBytes(cqrbytes.TarBytes(nullBytes), encType);
            return DeEnCoder.EncodeBytes(cqrbytes, encType);
        }


        public string NCqrMessage(string cqrMessage, EncodingType encType = EncodingType.Base64)
        {
            byte[] inBytes = DeEnCoder.DecodeText(cqrMessage, encType);
            byte[] cipherBytes = DeEnCoder.GetBytesTrimNulls(inBytes);
            byte[] unroundedMerryBytes = SymmCrypt.DecrpytRoundGoMerry(cipherBytes, EnDeCoder.GetString(secKey), DeEnCoder.KeyToHex(EnDeCoder.GetString(secKey)));
            byte[] hashSymBytes = new byte[8];
            Array.Copy(unroundedMerryBytes, unroundedMerryBytes.Length - 8, hashSymBytes, 0, 8);
            string hashVerification = EnDeCoder.GetString(hashSymBytes);
            int failureCnt = 0, ic = 0;
            for (ic = 0; ic < 8; ic++)
            {
                if (hashVerification[ic] != HashSymms[ic])
                    failureCnt += ic;
            }

            if (failureCnt > 0)
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
