using Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Core.Crypt.Cipher;
using Area23.At.Framework.Library.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Core.Net.WebHttp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Area23.At.Framework.Library.Core.Net.IpSocket;
using System.Net;

namespace Area23.At.Framework.Library.Core.Crypt.CqrJd
{

    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    public class CqrPeer2PeerMsg
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
        public CqrPeer2PeerMsg(string srvKey = "")
        {
            if (string.IsNullOrEmpty(srvKey))
            {
                throw new ArgumentNullException("public CqrPeer2PeerMsg(string srvKey = \"\")");
            }
            key = srvKey;
            hash = DeEnCoder.KeyToHex(srvKey);
            keyBytes = CryptHelper.GetUserKeyBytes(key, hash, 16);
            symmPipe = new SymmCipherPipe(keyBytes, 8);
        }


        /// <summary>
        /// CqrPeerMsg encrypts a msg 
        /// </summary>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        public string CqrPeerMsg(string msg, EncodingType encType = EncodingType.Base64)
        {
            msg = msg + "\n" + symmPipe.PipeString;
            byte[] msgBytes = DeEnCoder.GetBytesFromString(msg);

            byte[] cqrbytes = symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash);
            CqrMsg = DeEnCoder.EncodeBytes(cqrbytes, encType);

            return CqrMsg;
        }


        /// <summary>
        /// NCqrPeerMsg decryptes an secure encrypted msg 
        /// </summary>
        /// <param name="cqrMessage">secure encrypted msg </param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>plain text decrypted string</returns>
        /// <exception cref="InvalidOperationException">will be thrown, 
        /// if server and client or both side use a different secret key 4 encryption</exception>
        public string NCqrPeerMsg(string cqrMessage, EncodingType encType = EncodingType.Base64)
        {
            CqrMsg = cqrMessage.TrimEnd("\0".ToCharArray());
            byte[] cipherBytes = DeEnCoder.DecodeText(CqrMsg, encType);

            byte[] unroundedMerryBytes = symmPipe.DecrpytRoundGoMerry(cipherBytes, key, hash);
            string decrypted = DeEnCoder.GetStringFromBytesTrimNulls(unroundedMerryBytes);

            string hashVerification = decrypted.Substring(decrypted.Length - 8);

            int failureCnt = 0, ic = 0;
            int minLen = Math.Min(8, symmPipe.PipeString.Length);
            int maxLen = Math.Min(minLen, hashVerification.Length);
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


        public string SendCqrPeerMsg(string msg, IPAddress peerIp, EncodingType encodingType = EncodingType.Base64, int serverPort = 7777)
        {
            string encrypted = CqrPeerMsg(msg, encodingType);
            string response = IPSocketSender.Send(peerIp, encrypted, Constants.CHAT_PORT);
            return response;
        }

    }

}
