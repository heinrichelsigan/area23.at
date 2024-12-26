using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Util;
using System;

namespace Area23.At.Framework.Library.Net.CqrJd
{

    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    public class CqrServerMsg
    {
        private readonly byte[] keyBytes = new byte[16];
        private readonly SymmCipherPipe symmPipe;

        public string CqrMsg { get; protected internal set; }


        public CqrServerMsg(byte[] userKeyBytes)
        {
            Array.Copy(userKeyBytes, keyBytes, Math.Min(userKeyBytes.Length, 16));
            symmPipe = new SymmCipherPipe(keyBytes, 8);
        }

        public CqrServerMsg(string srvKey = "") : this(EnDeCoder.GetBytes(srvKey)) { }


        public string CqrMessage(string msg, EncodingType encType = EncodingType.Base64)
        {
            byte[] msgBytes = DeEnCoder.GetBytesFromString(msg.Replace("\n", " \r\n"), 64, true);
            byte[] nullBytes = new byte[8];
            for (ushort ib = 0; ib < 8; ib++) nullBytes[ib] = 0;

            byte[] tarBytes = msgBytes.TarBytes(EnDeCoder.GetBytes(symmPipe.PipeString));
            // HashSymms 
            byte[] cqrbytes = SymmCrypt.MerryGoRoundEncrpyt(tarBytes, EnDeCoder.GetString(keyBytes), 
                DeEnCoder.KeyToHex(EnDeCoder.GetString(keyBytes)));

            DeEnCoder.EncodeBytes(cqrbytes.TarBytes(nullBytes), encType);
            CqrMsg = DeEnCoder.EncodeBytes(cqrbytes, encType);
            return CqrMsg;
        }


        public string NCqrMessage(string cqrMessage, EncodingType encType = EncodingType.Base64)
        {
            CqrMsg = cqrMessage;
            byte[] inBytes = DeEnCoder.DecodeText(cqrMessage, encType);
            byte[] cipherBytes = DeEnCoder.GetBytesTrimNulls(inBytes);

            byte[] unroundedMerryBytes = SymmCrypt.DecrpytRoundGoMerry(cipherBytes, 
                EnDeCoder.GetString(keyBytes), 
                DeEnCoder.KeyToHex(EnDeCoder.GetString(keyBytes)));

            byte[] hashSymBytes = new byte[8];
            Array.Copy(unroundedMerryBytes, unroundedMerryBytes.Length - 8, hashSymBytes, 0, 8);
            string hashVerification = EnDeCoder.GetString(hashSymBytes);
            int failureCnt = 0, ic = 0;
            for (ic = 0; ic < 8; ic++)
            {
                if (hashVerification[ic] != symmPipe.PipeString[ic])
                    failureCnt += ic;
            }

            if (failureCnt > 0)
            {
                string hashSymShow = symmPipe.PipeString ?? "        ";
                hashSymShow = hashSymShow.Substring(0, 2) + "...." + hashSymShow.Substring(6);

                throw new InvalidOperationException(
                    $"SymmCiphers [{hashSymShow}] in crypt pipeline doesn't match serverside key !?$* byte length ={keyBytes.Length}");
            }
            byte[] outBytes = new byte[unroundedMerryBytes.Length - 8];
            Array.Copy(unroundedMerryBytes, 0, outBytes, 0, unroundedMerryBytes.Length - 8);
            return DeEnCoder.GetStringFromBytesTrimNulls(outBytes);
        }

    }

}
