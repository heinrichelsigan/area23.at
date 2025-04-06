using Area23.At.Framework.Library.CqrXs.CqrMsg;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Net.IpSocket;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using System;
using System.Net;

namespace Area23.At.Framework.Library.CqrXs.CqrSrv
{

    /// <summary>
    /// Provides a secure encrypted message to send to the server or receive from server
    /// </summary>
    public class Peer2PeerMsg : BaseMsg
    {

        /// <summary>
        /// CqrServerMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public Peer2PeerMsg(string srvKey = "") : base(srvKey) { }


        /// <summary>
        /// CqrPeerMsg encrypts a msg 
        /// </summary>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="CipherPipe"/></returns>
        public string CqrPeerMsg(string msg, EncodingType encType = EncodingType.Base64)
        {
            return CqrBaseMsg(msg, encType);
        }

        public virtual string CqrPeerMsg(MsgContent msc, EncodingType encType = EncodingType.Base64)
        {
            return CqrBaseMsg(msc, encType);
        }



        /// <summary>
        /// CqrFile, encrypts a attached file persisted in <see cref="CqrFile(CqrMsg.CqrFile, MsgEnum, EncodingType)"/>
        /// </summary>
        /// <param name="cqrFile">file to encrypt</param>
        /// <param name="msgType"></param>
        /// <param name="encType"></param>
        /// <returns></returns>
        public string CqrFile(CqrFile cqrFile, MsgEnum msgType = MsgEnum.Json, EncodingType encType = EncodingType.Base64)
        {
            if (msgType == MsgEnum.None || msgType == MsgEnum.RawWithHashAtEnd)
            {
                cqrFile.RawMessage += "\n" + symmPipe.PipeString + "\0";
            }
            else if (msgType == MsgEnum.Json)
            {
                cqrFile.RawMessage = JsonConvert.SerializeObject(cqrFile);
            }
            else if (msgType == MsgEnum.Xml)
            {
                cqrFile.RawMessage = Utils.SerializeToXml(cqrFile);
            }
            byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(cqrFile.RawMessage);

            // Crypt cipherbytes from message
            byte[] cqrbytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;
            // Encode crypted bytes by (in most cases Base64, but other encodings like Uu, Base32 or Hex16 are valid too
            CqrMessage = EnDeCodeHelper.EncodeBytes(cqrbytes, encType);

            return CqrMessage;
        }

        /// <summary>
        /// NCqrPeerMsg decryptes an secure encrypted msg 
        /// </summary>
        /// <param name="cqrMessage">secure encrypted msg </param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns><see cref="MsgContent"/> Message plain text decrypted string</returns>
        /// <exception cref="InvalidOperationException">will be thrown, 
        /// if server and client or both side use a different secret key 4 encryption</exception>
        public MsgContent NCqrPeerMsg(string cqrMessage, EncodingType encType = EncodingType.Base64)
        {
            MsgContent msgContent = base.NCqrBaseMsg(cqrMessage, encType);
            return msgContent;
        }


        /// <summary>
        /// NCqrFile decrypts a encrypted file
        /// </summary>
        /// <param name="cqrMessage">encrypted message</param>
        /// <param name="msgType"></param>
        /// <param name="encType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public CqrFile NCqrFile(string cqrMessage, MsgEnum msgType = MsgEnum.Json, EncodingType encType = EncodingType.Base64)
        {
            CqrMessage = cqrMessage.TrimEnd("\0".ToCharArray());

            byte[] cipherBytes = EnDeCodeHelper.DecodeText(CqrMessage, encType);
            byte[] unroundedMerryBytes = LibPaths.CqrEncrypt ? symmPipe.DecrpytRoundGoMerry(cipherBytes, key, hash) : cipherBytes;
            string decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes); //DeEnCoder.GetStringFromBytesTrimNulls(unroundedMerryBytes);
            while (decrypted[decrypted.Length - 1] == '\0')
                decrypted = decrypted.Substring(0, decrypted.Length - 1);

            if (decrypted.IsValidJson())
                msgType = MsgEnum.Json;
            else if (decrypted.StartsWith("{\"") && decrypted.Contains("\"_hash\":") && decrypted.Contains("\"_message\":"))
            {
                if (Char.IsLetter(decrypted[decrypted.Length - 1]) || Char.IsDigit(decrypted[decrypted.Length - 1]))
                {
                    decrypted += "\" }";
                    msgType = MsgEnum.Json;
                }
            }
            else if (decrypted.IsValidXml())
                msgType = MsgEnum.Xml;
            else msgType = MsgEnum.RawWithHashAtEnd;

            CqrFile cqrFile = new CqrFile(decrypted, msgType);
            string hashVerification = cqrFile.Hash;
            bool verified = VerifyHash(hashVerification, symmPipe.PipeString);
            if (!verified)
            {
                string hashSymShow = symmPipe.PipeString ?? "        ";
                throw new InvalidOperationException(
                    string.Format("SymmCiphers [{0}] in crypt pipeline doesn't match serverside key !?$* byte length={1}.",
                        hashSymShow.Substring(0, 2) + "...." + hashSymShow.Substring(6), keyBytes.Length));
            }

            return cqrFile;
        }

        public MsgContent NCqrSrvMsg(MsgContent msgInContent, EncodingType encType = EncodingType.Base64)
        {
            MsgContent msgOutContent = NCqrBaseMsg(msgInContent.RawMessage, encType);
            return msgOutContent;
        }


        /// <summary>
        /// Send_CqrPeerMsg, sends a plain-text message to peer 2 peer partner
        /// </summary>
        /// <param name="msg">message to send</param>
        /// <param name="peerIp">peer partner ip address</param>
        /// <param name="serverPort">tcp server port</param>
        /// <param name="encodingType"><see cref="EncodingType"/></param>
        /// <returns>response string</returns>
        public string Send_CqrPeerMsg(string msg, IPAddress peerIp, int serverPort = 7777, EncodingType encodingType = EncodingType.Base64)
        {
            string encrypted = CqrPeerMsg(msg, encodingType);
            string response = Sender.Send(peerIp, encrypted, Constants.CHAT_PORT);
            return response;
        }



        /// <summary>
        /// Send_CqrFile, sends an attached file to peer 2 peer partner
        /// </summary>
        /// <param name="cqrFile">Entity, that represents the file, data are stored as byte[]</param>
        /// <param name="peerIp"><see cref="IPAddress"/ for the partner></param>
        /// <param name="serverPort">server port, default <see cref="Constants.CHAT_PORT"/></param>
        /// <param name="msgType"><see cref="MsgEnum">msgType</see> default with <see cref="MsgEnum.Json"/></param>
        /// <param name="encType"><see cref="EncodingType"/> default to <see cref="EncodingType.Base64"/></param>
        /// <returns></returns>
        public string Send_CqrFile(CqrFile cqrFile, IPAddress peerIp, int serverPort = 7777, MsgEnum msgType = MsgEnum.Json, EncodingType encType = EncodingType.Base64)
        {
            cqrFile._hash = PipeString;
            cqrFile.MsgType = msgType;
            string encrypted = CqrFile(cqrFile, msgType, encType);
            string response = Sender.Send(peerIp, encrypted, Constants.CHAT_PORT);
            return response;
        }

    }

}
