﻿using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using Area23.At.Framework.Library.Static;

namespace Area23.At.Framework.Library.CqrXs.CqrMsg
{

    /// <summary>
    /// Provides abstract base class for secure encrypted message to send to the server or receive from server
    /// </summary>
    public abstract class BaseMsg
    {
        protected internal readonly string key;
        protected internal readonly string hash;
        protected internal readonly byte[] keyBytes;
        protected internal bool _isMimeAttachment = false;

        protected internal readonly SymmCipherPipe symmPipe;


        public string PipeString { get; internal set; }


        public string CqrMessage { get; protected internal set; }

        /// <summary>
        /// CqrBaseMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public BaseMsg(string srvKey = "")
        {
            if (string.IsNullOrEmpty(srvKey))
            {
                throw new ArgumentNullException("public BaseMsg(string srvKey = \"\")");
            }
            key = srvKey;
            hash = EnDeCodeHelper.KeyToHex(srvKey);
            keyBytes = CryptHelper.GetUserKeyBytes(key, hash, 16);
            symmPipe = new SymmCipherPipe(keyBytes, 8);
            PipeString = symmPipe.PipeString;
        }


        /// <summary>
        /// CqrBaseMsg encrypts a msg 
        /// </summary>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        public virtual string CqrBaseMsg(string msg, EncodingType encType = EncodingType.Base64)
        {
            MsgContent msc;
            if (msg.Contains(PipeString) && msg.IndexOf(PipeString) > 10)
                msc = new MsgContent(msg, MsgEnum.None);
            else
                msc = new MsgContent(msg, PipeString);

            byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(msc.RawMessage);

            byte[] cqrbytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;
            CqrMessage = EnDeCodeHelper.EncodeBytes(cqrbytes, encType);

            return CqrMessage;
        }

        /// <summary>
        /// CqrBaseMsg encrypts a msg 
        /// </summary>
        /// <param name="msc">plain MsgContent</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        public virtual string CqrBaseMsg(MsgContent msc, EncodingType encType = EncodingType.Base64)
        {
            byte[] msgBytes = new byte[msc.RawMessage.Length];
            msc._hash = PipeString;

            if (msc.MsgType == MsgEnum.None || msc.MsgType == MsgEnum.RawWithHashAtEnd)
            {
                msc._hash = PipeString;
                if (!msc.RawMessage.EndsWith("\n" + PipeString + "\0") &&
                    !msc.RawMessage.EndsWith("\n" + PipeString) &&
                    (msc.RawMessage.LastIndexOf("\n" + PipeString) < msc.RawMessage.Length - 10))
                {
                    msc.RawMessage = msc.Message + "\n" + PipeString + "\0";
                }                
            }
            else if (msc.MsgType == MsgEnum.Json)
            {
                bool shouldSerialize = true;
                if (msc.RawMessage.IsValidJson())
                {
                    MsgContent c = JsonConvert.DeserializeObject<MsgContent>(msc.RawMessage);
                    if (c != null && string.IsNullOrEmpty(c._hash) && c._hash.Equals(PipeString) && !string.IsNullOrEmpty(c._message))
                        shouldSerialize = false;
                }                
                if (shouldSerialize) 
                    msc.RawMessage = JsonConvert.SerializeObject(msc);
            }

            msgBytes = EnDeCodeHelper.GetBytesFromString(msc.RawMessage);
            byte[] cqrbytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;
            CqrMessage = EnDeCodeHelper.EncodeBytes(cqrbytes, encType);

            return CqrMessage;
        }



        /// <summary>
        /// NCqrBaseMsg decryptes an secure encrypted msg 
        /// </summary>
        /// <param name="cqrMessage">secure encrypted msg </param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>MsgContent Message plain text decrypted string</returns>
        /// <exception cref="InvalidOperationException">will be thrown, 
        /// if server and client or both side use a different secret key 4 encryption</exception>
        public virtual MsgContent NCqrBaseMsg(string cqrMessage, EncodingType encType = EncodingType.Base64)
        {
            CqrMessage = cqrMessage.TrimEnd("\0".ToCharArray());

            byte[] cipherBytes = EnDeCodeHelper.DecodeText(CqrMessage, encType);
            byte[] unroundedMerryBytes = LibPaths.CqrEncrypt ? symmPipe.DecrpytRoundGoMerry(cipherBytes, key, hash) : cipherBytes;
            string decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes); //DeEnCoder.GetStringFromBytesTrimNulls(unroundedMerryBytes);
            while (decrypted[decrypted.Length - 1] == '\0')
                decrypted = decrypted.Substring(0, decrypted.Length - 1);

            MsgEnum msgEnum = (decrypted.IsValidJson()) ? MsgEnum.Json : MsgEnum.RawWithHashAtEnd;
            MsgContent msgContent = new MsgContent(decrypted, msgEnum);
            string hashVerification = msgContent.Hash;
            bool verified = VerifyHash(hashVerification, symmPipe.PipeString);
            if (!verified)
            {
                string hashSymShow = symmPipe.PipeString ?? "        ";
                throw new InvalidOperationException(
                    string.Format("SymmCiphers [{0}] in crypt pipeline doesn't match serverside key !?$* byte length={1}.",
                        hashSymShow.Substring(0, 2) + "...." + hashSymShow.Substring(6), keyBytes.Length));
            }

            return msgContent;
        }




        /// <summary>
        /// GetVerificationHash gets verification hash
        /// </summary>
        /// <param name="decrypted">decrypted text</param>
        /// <returns>verification hash</returns>
        protected internal virtual string VerificationHash(ref MsgContent msgContent)
        {
            string hash = msgContent.Message.Substring(msgContent.Message.Length - 8);
            hash = !string.IsNullOrEmpty(msgContent._hash) ? msgContent._hash : hash;
            if (msgContent.IsCqrFile())
            {
                CqrFile cqFile = msgContent.ToCqrFile();
                hash = cqFile._hash;
                // hash = cfile._hash;
            }

            return hash;
        }

        /// <summary>
        /// VerifyHash verifies hash against <see cref="SymmCipherPipe.PipeString"/>
        /// </summary>
        /// <param name="hash">verification hash parsed out of msg</param>
        /// <returns>true, if msg could be verified, otherwise false</returns>
        protected internal virtual bool VerifyHash(string hash, string pipeClientOrServer)
        {
            int failureCnt = 0;
            int minLen = Math.Min(hash.Length, pipeClientOrServer.Length);
            for (int ic = 0; ic < minLen; ic++)
            {
                if (hash[ic] != pipeClientOrServer[ic])
                    failureCnt += ic;
            }

            return failureCnt == 0;
        }

    }


}

/*
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using Area23.At.Framework.Library.CqrXs.CqrMsg;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;

namespace Area23.At.Framework.Library.CqrXs.CqrSrv
{

    /// <summary>
    /// Provides abstract base class for secure encrypted message to send to the server or receive from server
    /// </summary>
    public abstract class BaseMsg
    {
        protected internal readonly string key;
        protected internal readonly string hash;
        protected internal readonly byte[] keyBytes;
        protected internal bool _isMimeAttachment = false;

        protected internal readonly SymmCipherPipe symmPipe;
        protected internal static bool fishOnAesEngine = false;

        public string PipeString { get; internal set; }


        public string CqrMessage { get; protected internal set; }

        /// <summary>
        /// CqrBaseMsg constructor with srvKey
        /// </summary>
        /// <param name="srvKey">server key (normally client ip + secret)</param>
        /// <exception cref="ArgumentNullException">thrown, when srvKey is null or <see cref="string.Empty"/></exception>
        public BaseMsg(string srvKey = "")
        {
            if (string.IsNullOrEmpty(srvKey))
            {
                throw new ArgumentNullException("public BaseMsg(string srvKey = \"\")");
            }
            fishOnAesEngine = (AppDomain.CurrentDomain.GetData(Constants.FISH_ON_AES_ENGINE) == null) ? false
               : fishOnAesEngine = Convert.ToBoolean(AppDomain.CurrentDomain.GetData(Constants.FISH_ON_AES_ENGINE));

            key = srvKey;
            hash = EnDeCodeHelper.KeyToHex(srvKey);
            keyBytes = CryptHelper.GetUserKeyBytes(key, hash, 16);
            symmPipe = new SymmCipherPipe(keyBytes, 8);
            PipeString = symmPipe.PipeString;
        }


        /// <summary>
        /// CqrBaseMsg encrypts a msg 
        /// </summary>
        /// <param name="msg">plain text string</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        public virtual string CqrBaseMsg(string msg, EncodingType encType = EncodingType.Base64)
        {
            MsgContent msc;
            if (msg.Contains(PipeString) && msg.IndexOf(PipeString) > msg.Length - 10)
                msc = new MsgContent(msg, MsgEnum.None);
            else
                msc = new MsgContent(msg, PipeString);

            byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(msc.RawMessage);

            byte[] cqrbytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;
            CqrMessage = EnDeCodeHelper.EncodeBytes(cqrbytes, encType);

            return CqrMessage;
        }

        /// <summary>
        /// CqrBaseMsg encrypts a msg 
        /// </summary>
        /// <param name="msc">plain MsgContent</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted msg via <see cref="SymmCipherPipe"/></returns>
        public virtual string CqrBaseMsg(MsgContent msc, EncodingType encType = EncodingType.Base64)
        {
            byte[] msgBytes = new byte[msc.RawMessage.Length];
            if (msc.MsgType == MsgEnum.None || msc.MsgType == MsgEnum.RawWithHashAtEnd)
            {
                msc._hash = PipeString;
                if (msc.RawMessage.EndsWith("\n" + PipeString + "\0") ||
                    msc.RawMessage.EndsWith("\n" + PipeString + "\0"))
                    msgBytes = EnDeCodeHelper.GetBytesFromString(msc.RawMessage);
                else
                {
                    msc._rawMessage = msc.Message + "\n" + PipeString + "\0";
                    msgBytes = EnDeCodeHelper.GetBytesFromString(msc.RawMessage);
                }
            }
            else if (msc.MsgType == MsgEnum.JsonSerialized || msc.MsgType == MsgEnum.JsonDeserialized)
            {
                msc._hash = PipeString;
                if (!msc.RawMessage.IsValidJson())
                    msgBytes = EnDeCodeHelper.GetBytesFromString(JsonConvert.SerializeObject(msc));
                else
                    msgBytes = EnDeCodeHelper.GetBytesFromString(msc.RawMessage);
            }
            byte[] cqrbytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;

            CqrMessage = EnDeCodeHelper.EncodeBytes(cqrbytes, encType);

            return CqrMessage;
        }


        /// <summary>
        /// CqrBaseAttachment encrypts a file attchment message
        /// </summary>
        /// <param name="fileName">file name of attached file</param>
        /// <param name="mimeType"><see cref="Util.MimeType"/></param>
        /// <param name="base64Mime">base64 encoded mime block</param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>encrypted attachment msg via <see cref="SymmCipherPipe"/></returns>
        public virtual string CqrBaseAttachment(string fileName, string mimeType, string base64Mime, out MimeAttachment attachment,
            EncodingType encType = EncodingType.Base64, string sMd5 = "", string sSha256 = "")
        {
            attachment = new MimeAttachment(fileName, mimeType, base64Mime, symmPipe.PipeString, sMd5, sSha256);
            string mimeMsg = attachment.MimeMsg;
            mimeMsg += "\n" + symmPipe.PipeString + "\0";
            byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(mimeMsg);

            byte[] cqrbytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, key, hash) : msgBytes;

            CqrMessage = EnDeCodeHelper.EncodeBytes(cqrbytes, encType);

            return CqrMessage;
        }

        /// <summary>
        /// NCqrBaseMsg decryptes an secure encrypted msg 
        /// </summary>
        /// <param name="cqrMessage">secure encrypted msg </param>
        /// <param name="encType"><see cref="EncodingType"/></param>
        /// <returns>MsgContent Message plain text decrypted string</returns>
        /// <exception cref="InvalidOperationException">will be thrown, 
        /// if server and client or both side use a different secret key 4 encryption</exception>
        public virtual MsgContent NCqrBaseMsg(string cqrMessage, EncodingType encType = EncodingType.Base64, bool fishOnAes = false)
        {
            if (fishOnAes) fishOnAesEngine = true;
            
            CqrMessage = cqrMessage.TrimEnd("\0".ToCharArray());
            byte[] cipherBytes = EnDeCodeHelper.DecodeText(CqrMessage, encType);            
            byte[] unroundedMerryBytes = (!LibPaths.CqrEncrypt) ? cipherBytes :
                symmPipe.DecrpytRoundGoMerry(cipherBytes, key, hash, fishOnAesEngine);

            string decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes); //EnDeCodeHelper.GetStringFromBytesTrimNulls(unroundedMerryBytes);
            while (decrypted[decrypted.Length - 1] == '\0') decrypted = decrypted.Substring(0, decrypted.Length - 1);
            
            MsgEnum msgEnum = (decrypted.IsValidJson()) ? MsgEnum.JsonSerialized : MsgEnum.RawWithHashAtEnd;

            MsgContent msgContent = new MsgContent(decrypted, msgEnum);            
            string hashVerification = msgContent.Hash;
            bool verified = VerifyHash(hashVerification, symmPipe.PipeString);
            if (!verified)
            {
                string hashSymShow = symmPipe.PipeString ?? "        ";
                throw new InvalidOperationException(
                    string.Format("SymmCiphers [{0}] in crypt pipeline doesn't match serverside key !?$* byte length={1}.",
                        hashSymShow.Substring(0, 2) + "...." + hashSymShow.Substring(6), keyBytes.Length));
            }

            return msgContent;
        }



        /// <summary>
        /// IsMimeAttachmentMsg checks if decrypted text contains a mime attachmen
        /// </summary>
        /// <param name="decrypted">decrypted text</param>
        /// <returns>true, if a mime attachment is inside, otherwise false</returns>
        protected internal virtual bool IsMimeAttachmentMsg(MsgContent msgContent)
        {
            _isMimeAttachment = msgContent.IsMimeAttachment();
            return _isMimeAttachment;
        }


        /// <summary>
        /// GetVerificationHash gets verification hash
        /// </summary>
        /// <param name="decrypted">decrypted text</param>
        /// <returns>verification hash</returns>
        protected internal virtual string VerificationHash(ref MsgContent msgContent)
        {
            string hash = msgContent.Message.Substring(msgContent.Message.Length - 8);
            _isMimeAttachment = msgContent.IsMimeAttachment();
            if (_isMimeAttachment)
            {
                MimeAttachment mime = msgContent.ToMimeAttachment();
                hash = mime.Verification;
            }

            return hash;
        }

        /// <summary>
        /// VerifyHash verifies hash against <see cref="SymmCipherPipe.PipeString"/>
        /// </summary>
        /// <param name="hash">verification hash parsed out of msg</param>
        /// <returns>true, if msg could be verified, otherwise false</returns>
        protected internal virtual bool VerifyHash(string hash, string pipeClientOrServer)
        {
            int failureCnt = 0;
            int minLen = Math.Min(hash.Length, pipeClientOrServer.Length);
            for (int ic = 0; ic < minLen; ic++)
            {
                if (hash[ic] != pipeClientOrServer[ic])
                    failureCnt += ic;
            }

            return failureCnt == 0;
        }

    }



}
*/
