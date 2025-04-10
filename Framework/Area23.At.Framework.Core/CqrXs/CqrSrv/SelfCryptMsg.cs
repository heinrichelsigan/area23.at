using Area23.At.Framework.Core.CqrXs.CqrMsg;
using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Net.IpSocket;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using Newtonsoft.Json;
using System.Formats.Tar;
using System.Net;


namespace Area23.At.Framework.Core.CqrXs.CqrSrv
{

    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    public class SelfCryptMsg : BaseMsg
    {

        /// <summary>
        /// SelfCryptMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public SelfCryptMsg(string srvKey = "") : base(srvKey) { }


        public SelfCryptMsg() : base(Constants.GIT_CQR_URL) { }
        

        /// <summary>
        /// CqrPeerMsg encrypts a msg 
        /// </summary>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="CipherPipe"/></returns>
        public string CqrSelfMsg(string msg, EncodingType encType = EncodingType.Base64)
        {
            return CqrBaseMsg(msg, encType);
        }

        public virtual string CqrSelfMsg(MsgContent msc, EncodingType encType = EncodingType.Base64)
        {
            return CqrBaseMsg(msc, encType);
        }


        /// <summary>
        /// NCqrPeerMsg decryptes an secure encrypted msg 
        /// </summary>
        /// <param name="cqrMessage">secure encrypted msg </param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns><see cref="string"/> Message plain text decrypted string</returns>
        /// <exception cref="InvalidOperationException">will be thrown, 
        /// if server and client or both side use a different secret key 4 encryption</exception>
        public string NCqrSelfMsg(string cqrMessage, out MsgEnum msgType, EncodingType encType = EncodingType.Base64)
        {
            string? selfDecryptedString = "";

            msgType = MsgEnum.RawWithHashAtEnd;
            try
            {
                selfDecryptedString = base.NCqrBaseMsg(cqrMessage, out msgType, encType);
            }
            catch (Exception exi)
            {
                CqrException.SetLastException(exi);
            }

            return selfDecryptedString ?? "";
        }


        public MsgContent NCqrSelfMsg(MsgContent msgInContent, EncodingType encType = EncodingType.Base64)
        {
            MsgContent msgOutContent = NCqrBaseMsg(msgInContent.RawMessage, encType);
            return msgOutContent;
        }


     
        public string SelfEnrypt(string msg, EncodingType encType = EncodingType.Base64)
        {
            string encrypted = CqrSelfMsg(msg, encType);
            
            return encrypted;
        }

        public string SelfDecrypt(string cryptMsg, out MsgEnum msgType, EncodingType encType = EncodingType.Base64)
        {
            string decrypted = NCqrSelfMsg(cryptMsg, out msgType, encType);
            
            return decrypted;
        }

        public static string Encrypt(string plain) => (new SelfCryptMsg()).SelfEnrypt(plain, EncodingType.Base64);            

        public static string Decrypt(string encryptedText) => (new SelfCryptMsg()).NCqrSelfMsg(encryptedText, out MsgEnum msgType, EncodingType.Base64);


    }

}
