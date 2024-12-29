using Area23.At.Framework.Library.Core.Crypt.Cipher;
using Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Core.Net.WebHttp;
using Area23.At.Framework.Library.Core.Util;
using System.Configuration;
using System.Net;
using System.Security.Policy;

namespace Area23.At.Framework.Library.Core.Crypt.CqrJd
{

    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    public class CqrServerMsg
    {
        private readonly string key;
        private readonly string hash;
        private readonly byte[] keyBytes;
#if DEBUG
        public readonly SymmCipherPipe symmPipe;
#else
        private readonly SymmCipherPipe symmPipe;
#endif
        public string CqrMsg { get; protected internal set; }

        /// <summary>
        /// CqrServerMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public CqrServerMsg(string srvKey = "")
        {
            if (string.IsNullOrEmpty(srvKey))
            {
                throw new ArgumentNullException("public CqrServerMsg(string srvKey = \"\")");
            }
            key = srvKey;
            hash = DeEnCoder.KeyToHex(srvKey);
            keyBytes = CryptHelper.GetUserKeyBytes(key, hash, 16);
            symmPipe = new SymmCipherPipe(keyBytes, 8);
        }


        /// <summary>
        /// CqrMessage encrypts a msg 
        /// </summary>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        public string CqrMessage(string msg, EncodingType encType = EncodingType.Base64)
        {
            msg = msg + "\n" + symmPipe.PipeString;
            byte[] msgBytes = DeEnCoder.GetBytesFromString(msg);

            byte[] cqrbytes = symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash);
            CqrMsg = DeEnCoder.EncodeBytes(cqrbytes, encType);

            return CqrMsg;
        }


        /// <summary>
        /// NCqrMessage decryptes an secure encrypted msg 
        /// </summary>
        /// <param name="cqrMessage">secure encrypted msg </param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>plain text decrypted string</returns>
        /// <exception cref="InvalidOperationException">will be thrown, 
        /// if server and client or both side use a different secret key 4 encryption</exception>
        public string NCqrMessage(string cqrMessage, EncodingType encType = EncodingType.Base64)
        {
            CqrMsg = cqrMessage;
            byte[] cipherBytes = DeEnCoder.DecodeText(cqrMessage, encType);

            byte[] unroundedMerryBytes = symmPipe.DecrpytRoundGoMerry(cipherBytes, key, hash);
            string decrypted = DeEnCoder.GetStringFromBytesTrimNulls(unroundedMerryBytes);

            string hashVerification = decrypted.Substring(decrypted.Length - 8);

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

            string decryptedFinally = decrypted.Substring(0, decrypted.Length - 8);
            return decryptedFinally;
        }


        public string SendCqrSrvMsg(string msg, IPAddress srvIp, EncodingType encodingType = EncodingType.Base64)
        {
            string encrypted = String.Format("TextBoxEncrypted={0}\r\nTextBoxDecrypted=\r\nTextBoxLastMsg=\r\nButtonSubmit=Submit",
                CqrMessage(msg));

            string posturl = ConfigurationManager.AppSettings["ServerUrlToPost"].ToString();
            string hostheader = ConfigurationManager.AppSettings["SendHostHeader"].ToString();

            string response = WebClientRequest.PostMessage(encrypted, posturl, hostheader, srvIp.ToString());

            return response;
        }

    }

}
