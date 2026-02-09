using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Zfx;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Windows.Input;
using static QRCoder.PayloadGenerator.SwissQrCode;

namespace Area23.At.Framework.Library.Cqr.Msg
{

    [Serializable]
    public class CContent : IMsgAble
    {
        
        #region properties

        public SerType MsgType { get; set; }
        
        public string Message { get; set; }

        [JsonIgnore]
        public virtual string SerializedMsg
        {
            get => (MsgType == SerType.Xml) ?
                        ToXml() :
                        Newtonsoft.Json.JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public string Hash { get; set; }

        public string Md5Hash { get; set; }

        public EncodingType EnCodingType { get; set; }

        [JsonIgnore]
        protected internal byte[] CBytes { get; set; }

        #endregion properties

        #region ctor

        /// <summary>
        /// Parameterless constructor CContent
        /// </summary>
        public CContent()
        {
            MsgType = SerType.Json;
            Message = string.Empty;
            // SerializedMsg = string.Empty;
            Hash = string.Empty;
            Md5Hash = string.Empty;
            CBytes = new byte[0];
            EnCodingType = EncodingType.Base64;
        }


        /// <summary>
        /// this constructor requires a serialized or rawstring in msg
        /// </summary>
        /// <param name="serializedString">serialized string</param>
        /// <param name="msgArt">Serialization type</param>
        public CContent(string serializedString, SerType msgArt = SerType.None)
        {
            Md5Hash = Crypt.Hash.MD5Sum.HashString(serializedString);
            Message = serializedString;
            // SerializedMsg = serializedString;
            CBytes = new byte[0];

            string _message = Message;
            Hash = VerificationHash(out _message);
            Message = _message;

            switch (msgArt)
            {
                case SerType.Json:
                    MsgType = SerType.Json;
                    CContent cjson = GetMsgContentType(serializedString, out Type cqrType, SerType.Json);
                    if (cjson != null)
                    {
                        cjson.MsgType = SerType.Json;
                        CloneCopy(cjson, this);
                    }
                    break;
                case SerType.Xml:
                    MsgType = SerType.Xml;
                    CContent cXml = GetMsgContentType(serializedString, out Type cqType, msgArt);
                    if (cXml != null)
                    {
                        cXml.MsgType = SerType.Xml;
                        CloneCopy(cXml, this);
                    }
                    break;
                case SerType.None: //TODO
                    throw new NotImplementedException("TODO: implement reverse Reflection deserialization");

                case SerType.Raw:
                default:
                    MsgType = SerType.Raw;
                    Message = serializedString;
                    // SerializedMsg = serializedString;

                    _message = Message;
                    Hash = VerificationHash(out _message);
                    Message = _message;

                    Md5Hash = Crypt.Hash.MD5Sum.HashString(SerializedMsg);
                    break;
            }

        }

        /// <summary>
        /// this ctor requires a plainstring and serialize it in _SerializedMsg
        /// </summary>
        /// <param name="plainTextMsg">plain text message</param>
        /// <param name="hash"></param>
        /// <param name="msgArt"></param>
        public CContent(string plainTextMsg, string hash, SerType msgArt = SerType.Raw, string md5Hash = "")
        {
            MsgType = msgArt;
            Hash = hash;
            Message = plainTextMsg;
            // SerializedMsg = "";
            CBytes = new byte[0];
            Md5Hash = md5Hash;

            //if (msgArt == CType.Json)
            //{
            //	SerializedMsg = this.ToJson();
            //}
            //if (msgArt == CType.Xml)
            //{
            //	SerializedMsg = this.ToXml();
            //}
            if (msgArt == SerType.Raw)
            {
                if (plainTextMsg.Contains(hash) && plainTextMsg.IndexOf(hash) > (plainTextMsg.Length - 10))
                {
                    Message = SerializedMsg.Substring(0, SerializedMsg.Length - Hash.Length);
                }
                //else
                //{
                // SerializedMsg = Message + "\n" + hash + "\0";
                // }
            }
            // if (msgArt == CType.None)
            // {
            //	SerializedMsg = this.ToString();
            // }
        }


        public CContent(CContent srcToClone)
        {
            CloneCopy(srcToClone, this);
        }

        #endregion ctor

        #region members

        #region EnDeCrypt+DeSerialize


        public virtual string EncryptToJson(string serverKey, EncodingType encoder = EncodingType.Base64, 
            Zfx.ZipType zipType = Zfx.ZipType.None, KeyHash kHash = KeyHash.Hex)
        {
            if (Encrypt(serverKey, encoder, zipType))
            {
                string serializedJson = ToJson();
                return serializedJson;
            }
            throw new CqrException($"EncryptToJson(string severKey failed");
        }

        public virtual bool Encrypt(string serverKey, EncodingType encoder = EncodingType.Base64, 
            Zfx.ZipType zipType = Zfx.ZipType.None, KeyHash kHash = KeyHash.Hex)
        {
            string encrypted = "", keyHash = kHash.Hash(serverKey);
            CipherPipe pipe = new CipherPipe(serverKey, keyHash);
            string pipeString = pipe.PipeString;

            Hash = pipeString;
            try
            {
                encrypted = Encoding.UTF8.GetString(
                    pipe.EncryptEncodeBytes(Encoding.UTF8.GetBytes(Message),
                        serverKey, keyHash, encoder, zipType, kHash, CipherMode2.ECB));
                Hash = pipeString;
                Md5Hash = MD5Sum.HashString(String.Concat(serverKey, keyHash, pipeString, Message), "");

                Message = encrypted;
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }
            return true;
        }


        public virtual CContent DecryptFromJson(string serverKey, string serialized = "",
            EncodingType decoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            if (string.IsNullOrEmpty(serialized))
                serialized = this.SerializedMsg;

            CContent cc = FromJson<CContent>(serialized);
            if (cc != null && cc.Decrypt(serverKey, decoder, zipType))
            {
                CloneCopy(cc, this);
                return cc;
            }
            throw new CqrException($"DecryptFromJson<T>(string severKey, string serialized) failed");
        }

        public virtual bool Decrypt(string serverKey, EncodingType decoder = EncodingType.Base64, 
            Zfx.ZipType zipType = Zfx.ZipType.None, KeyHash kHash = KeyHash.Hex)
        {

            string keyHash = kHash.Hash(serverKey);
            CipherPipe symmPipe = new CipherPipe(serverKey, keyHash);
            try
            {
                string pipeString = symmPipe.PipeString; 
                string decrypted = CipherPipe.EncrpytT<string,string>(Message, serverKey, keyHash, 
                    EncodingType.Base64, ZipType.None, kHash, CipherMode2.ECB); 

                if (!Hash.Equals(pipeString))
                    throw new CqrException($"CContent.Hash={Hash} doesn't match PipeString={pipeString}");

                string md5Hash = MD5Sum.HashString(String.Concat(serverKey, keyHash, pipeString, decrypted), "");
                if (!md5Hash.Equals(Md5Hash))
                    throw new CqrException($"CContent.Md5Hash={Md5Hash} doesn't match md5Hash={md5Hash}.");

                Message = decrypted;
                CBytes = new byte[0];
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }
            return true;
        }


        #endregion EnDeCrypt+DeSerialize


        #region serialization / deserialization

        /// <summary>
        /// Serialize all CC classes to json
        /// </summary>
        /// <returns>json serialized string</returns>
        public virtual string ToJson() => JsonConvert.SerializeObject(this, Formatting.Indented);

        public virtual T FromJson<T>(string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText))
                jsonText = SerializedMsg;

            T t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonText);
            if (t != null)
            {
                if (t is CContact cct)
                    cct.CCopy(this, cct);
                if (t is CFile cfile)
                    cfile.CCopy(this, cfile);
                else if (t is CImage cimg)
                    cimg.CCopy(this, cimg);
                else if (t is CChatRoom cchatr)
                    cchatr.CCopy(this, cchatr);
                else if (t is CSrvMsg<string> cmsg)
                    cmsg.CCopy(this, cmsg);
                else if (t is CSrvMsg<List<string>> clmsg)
                    clmsg.CCopy(this, clmsg);
                if (t is CContent cc)
                    cc.CCopy(this, cc);
            }

            return t;
        }

        public virtual string ToXml() => Utils.SerializeToXml<CContent>(this);

        public virtual T FromXml<T>(string xmlText)
        {
            T t = Utils.DeserializeFromXml<T>(xmlText);
            if (t != null)
            {
                if (t is CContact cct)
                    cct.CCopy(this, cct);
                else if (t is CFile cfile)
                    cfile.CCopy(this, cfile);
                else if (t is CImage cimg)
                    cimg.CCopy(this, cimg);
                else if (t is CChatRoom cchatr)
                    cchatr.CCopy(this, cchatr);
                else if (t is CSrvMsg<string> cmsg)
                    cmsg.CCopy(this, cmsg);
                else if (t is CSrvMsg<List<string>> clmsg)
                    clmsg.CCopy(this, clmsg);
                if (t is CContent cc)
                    cc.CCopy(this, cc);
            }

            return t;
        }


        #endregion serialization / deserialization


        public virtual CContent CCopy(CContent leftDest, CContent rightSrc)
        {
            return CloneCopy(rightSrc, leftDest);
        }

        public virtual string VerificationHash(out string msg)
        {
            msg = Message;
            if (!string.IsNullOrEmpty(Hash))
            {
                return Hash;
            }

            if (IsCFile())
            {
                CFile cqFile = ToCFile();
                if (cqFile != null && !string.IsNullOrEmpty(cqFile.Hash))
                {
                    Hash = cqFile.Hash;
                    if (!string.IsNullOrEmpty(cqFile.Message))
                        msg = cqFile.Message;

                    return Hash;
                }
            }

            if (SerializedMsg.Length > 9)
            {
                // if (Message.Contains('\n') && Message.LastIndexOf('\n') < Message.Length)
                string tmp = SerializedMsg.Substring(SerializedMsg.Length - 10);
                if (tmp.Contains('\n') && tmp.IndexOf('\n') < 9)
                {
                    Hash = tmp.Substring(tmp.LastIndexOf('\n') + 1);
                    if (Hash.Contains("\0"))
                        Hash = Hash.Substring(0, Hash.LastIndexOf("\0"));
                }
            }
            else
            {
                Hash = SerializedMsg;
            }

            if (string.IsNullOrEmpty(Hash))
            {
                string hsh = "";
                if (SerializedMsg.Contains("\"Hash\":\""))
                {
                    int hshlen = "\"Hash\":\"".Length;
                    int hidx = SerializedMsg.IndexOf("\"Hash\":\"");
                    if (hidx > 0)
                    {
                        hsh = SerializedMsg.Substring((int)(hidx + hshlen));
                        if ((hidx = hsh.IndexOf("\"")) > 0)
                        {
                            Hash = hsh.Substring(0, hidx);
                            return Hash;
                        }
                    }
                }
            }


            if (Hash != null && Hash.Length > 4 && SerializedMsg.Substring(SerializedMsg.Length - Hash.Length).Equals(Hash, StringComparison.InvariantCulture))
                msg = SerializedMsg.Substring(0, SerializedMsg.Length - Hash.Length);

            return Hash ?? string.Empty;
        }

        public virtual bool IsCFile()
        {
            if (this is CFile cf && string.IsNullOrEmpty(cf.FileName) && cf.Data != null)
                return true;

            if (string.IsNullOrEmpty(SerializedMsg))
            {
                // if (MsgType == null || MsgType == CType.Json || MsgType == CType.Json)
                // SerializedMsg = this.ToJson();
                // else if (MsgType == CType.Xml)
                // SerializedMsg = this.ToXml();
            }
            if ((SerializedMsg.IsValidJson() && SerializedMsg.Contains("FileName") && SerializedMsg.Contains("Base64Type")) ||
                (SerializedMsg.IsValidXml() && SerializedMsg.Contains("FileName") && SerializedMsg.Contains("Base64Type")))
                return true;

            return false;
        }

        public virtual CFile ToCFile()
        {
            if (this is CFile cf && string.IsNullOrEmpty(cf.FileName) && cf.Data != null)
                return cf;

            if (string.IsNullOrEmpty(SerializedMsg))
            {
                //if (MsgType == null || MsgType == CType.Json || MsgType == CType.Json)
                //	SerializedMsg = this.ToJson();
                //else if (MsgType == CType.Xml)
                //	SerializedMsg = this.ToXml();
            }
            if (SerializedMsg.IsValidJson() && SerializedMsg.Contains("FileName") && SerializedMsg.Contains("Base64Type"))
                return (CFile)JsonConvert.DeserializeObject<CFile>(SerializedMsg);
            else if (SerializedMsg.IsValidXml() && SerializedMsg.Contains("CqrFileName") && SerializedMsg.Contains("Base64Type"))
                return (CFile)Static.Utils.DeserializeFromXml<CFile>(SerializedMsg);

            return null;
        }

        #endregion members

        #region static members

        public static CContent GetMsgContentType(string serString, out Type outType, SerType msgType = SerType.None)
        {
            outType = typeof(CContent);
            switch (msgType)
            {
                case SerType.Json:
                    if (serString.IsValidJson())
                    {
                        if (serString.Contains("Sender") && serString.Contains("Recipients") && serString.Contains("TContent"))
                        {
                            try
                            {
                                outType = typeof(CSrvMsg<List<string>>);
                                CSrvMsg<List<string>> clmsg = (CSrvMsg<List<string>>)JsonConvert.DeserializeObject<CSrvMsg<List<string>>>(serString);
                                return clmsg;
                            }
                            catch (Exception ex)
                            {
                                CqrException.SetLastException(ex);
                                outType = typeof(CSrvMsg<string>);
                                CSrvMsg<string> cmsg = (CSrvMsg<string>)JsonConvert.DeserializeObject<CSrvMsg<string>>(serString);
                                return cmsg;
                            }
                        }

                        if (serString.Contains("FileName") && serString.Contains("Base64Type"))
                        {
                            outType = typeof(CFile);
                            CFile cFile = (CFile)JsonConvert.DeserializeObject<CFile>(serString);
                            // cFile.SerializedMsg = serString;
                            return cFile;
                        }
                        if (serString.Contains("ImageFileName") && serString.Contains("ImageMimeType"))
                        {
                            outType = typeof(CImage);
                            return (CImage)JsonConvert.DeserializeObject<CImage>(serString);
                        }
                        if (serString.Contains("ContactId") && serString.Contains("Cuid") && serString.Contains("Email"))
                        {
                            outType = typeof(CContact);
                            return (CContact)JsonConvert.DeserializeObject<CContact>(serString);
                        }

                        outType = typeof(CContent);
                        return (CContent)JsonConvert.DeserializeObject<CContent>(serString);
                    }
                    break;
                case SerType.Xml:
                    if (serString.IsValidXml())
                    {
                        if (serString.Contains("Sender") && serString.Contains("Recipients") && serString.Contains("TContent"))
                        {
                            try
                            {
                                outType = typeof(CSrvMsg<List<string>>);
                                CSrvMsg<List<string>> clmsg = (CSrvMsg<List<string>>)Utils.DeserializeFromXml<CSrvMsg<List<string>>>(serString);
                                return clmsg;
                            }
                            catch (Exception ex)
                            {
                                CqrException.SetLastException(ex);
                                outType = typeof(CSrvMsg<string>);
                                CSrvMsg<string> cmsg = (CSrvMsg<string>)Utils.DeserializeFromXml<CSrvMsg<string>>(serString);
                                return cmsg;
                            }
                        }
                        if (serString.Contains("FileName") && serString.Contains("Base64Type"))
                        {
                            outType = typeof(CFile);
                            return (CFile)Utils.DeserializeFromXml<CFile>(serString);
                        }
                        if (serString.Contains("ImageFileName") && serString.Contains("ImageMimeType"))
                        {
                            outType = typeof(CImage);
                            return (CImage)Utils.DeserializeFromXml<CImage>(serString);
                        }
                        if (serString.Contains("ContactId") && serString.Contains("Cuid") && serString.Contains("Email"))
                        {
                            outType = typeof(CContact);
                            return (CContact)Utils.DeserializeFromXml<CContact>(serString);
                        }

                        outType = typeof(CContent);
                        return (CContent)Utils.DeserializeFromXml<CContent>(serString);
                    }
                    break;
                case SerType.Raw:
                case SerType.None:
                default: throw new NotImplementedException("GetMsgContentType(...): case MsgEnum.RawWithHashAtEnd and MsgEnum.None not implemented");
            }

            return null;
        }

        #region static members DeSeralizeDeCrypt<T> EncryptSerialize<T> Encrypt2Json Json2Decrypt

        public static T DeSeralizeDeCrypt<T>(string serverKey, string serialized = "", EncodingType decoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            if (string.IsNullOrEmpty(serialized))
                new ArgumentNullException("serialized");

            string pipeString = "";
            T t = (serialized.IsValidJson() && !serialized.IsValidXml()) ?
                        Newtonsoft.Json.JsonConvert.DeserializeObject<T>(serialized) :
                        Utils.DeserializeFromXml<T>(serialized);

            Area23Log.LogOriginMsg("CContent", $"DecryptSerialized<T = {t?.GetType()}>(...) => {JsonConvert.SerializeObject(t)}.");
            String keyHash = KeyHash.Hex.Hash(serverKey); 
            if (t != null)
            {
                if (t is CSrvMsg<string> cmsg)
                    if (cmsg.Decrypt(serverKey, decoder, zipType))
                        return (T)t;

                if (t is CSrvMsg<List<string>> clmsg)
                    if (clmsg.Decrypt(serverKey, decoder, zipType))
                        return (T)t;

                if (t is CFile cfile)
                    if (cfile.Decrypt(serverKey, decoder, zipType))
                        return (T)t;

                if (t is CContact ctnct)
                    if (ctnct.Decrypt(serverKey, decoder, zipType))
                        return (T)t;

                if (t is CChatRoom cchatr)
                    if (cchatr.Decrypt(serverKey, decoder, zipType))
                        return (T)t;

                if (t is CImage cimg)
                    if (cimg.Decrypt(serverKey, decoder, zipType))
                        return (T)t;

                if (t is CContent cc)
                {
                    CipherPipe ccPipe = new CipherPipe(serverKey, keyHash, EncodingType.Base64, ZipType.None, KeyHash.Hex, CipherMode2.ECB);
                    try
                    {
                        pipeString = ccPipe.PipeString;
                        string decrypted = CipherPipe.EncrpytT<string, string>(cc.Message,
                            serverKey, keyHash, EncodingType.Base64, ZipType.None, KeyHash.Hex, CipherMode2.ECB);

                        if (!cc.Hash.Equals(pipeString))
                            throw new CqrException($"cContent.Hash={cc.Hash} doesn't match PipeString={pipeString}");

                        string md5Hash = MD5Sum.HashString(String.Concat(serverKey, EnDeCodeHelper.KeyToHex(serverKey), pipeString, decrypted), "");
                        if (!md5Hash.Equals(cc.Md5Hash))
                            throw new CqrException($"cContent.Md5Hash={cc.Md5Hash} doesn't match md5Hash={md5Hash}.");

                        cc.Message = decrypted;
                        cc.CBytes = new byte[0];
                    }
                    catch (Exception exCrypt)
                    {
                        CqrException.SetLastException(exCrypt);
                        throw;
                    }

                    return (T)t;
                }

            }

            throw new CqrException($"DecryptSeralized<T>((string severKey, string serialized) failed");
        }

        public static string EncryptSerialize<T>(string serverKey, ref T t, EncodingType encType = EncodingType.Base64, ZipType zipType = ZipType.None)
        {
            if (t != null)
            {
                if (t is CSrvMsg<string> cmsg)
                    return cmsg.EncryptToJson(serverKey, encType, zipType);
                if (t is CSrvMsg<List<string>> clmsg)
                    return clmsg.EncryptToJson(serverKey, encType, zipType);
                if (t is CFile cfile)
                    return cfile.EncryptToJson(serverKey, encType, zipType);
                if (t is CContact ctnct)
                    return ctnct.EncryptToJson(serverKey, encType, zipType);
                if (t is CImage cimg)
                    return cimg.EncryptToJson(serverKey, encType, zipType);
                if (t is CChatRoom cchatr)
                    return cchatr.EncryptToJson(serverKey, encType, zipType);
                if (t is CContent cc)
                    return cc.EncryptToJson(serverKey, encType, zipType);
            }

            return null;
        }

        /// <summary>
        /// Encrypt2Json
        /// </summary>
        /// <param name="key">server key to encrypt</param>
        /// <param name="cmsg"><see cref="CContent"/> to encrypt and serialize</param>
        /// <returns>a serialized <see cref="string" /> of encrypted <see cref="CContent"/></returns>
        /// <exception cref="CqrException"></exception>
        public static string Encrypt2Json(string key, CContent cmsg, EncodingType encoder = EncodingType.Base64, 
            Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (cmsg == null)
                throw new ArgumentNullException("cmsg");

            string keyHash = EnDeCodeHelper.KeyToHex(key);
            string encrypted = "";
            CipherPipe pipe = new CipherPipe(key, keyHash);
            string pipeString = pipe.PipeString;
            try
            {
                encrypted = Encoding.UTF8.GetString(
                        pipe.EncryptEncodeBytes(Encoding.UTF8.GetBytes(cmsg.Message),
                            key, keyHash, encoder, zipType, KeyHash.Hex, CipherMode2.ECB));             
                cmsg.Hash = pipeString;
                cmsg.Md5Hash = MD5Sum.HashString(String.Concat(key, keyHash, pipeString, cmsg.Message), "");

                cmsg.Message = encrypted;
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }

            return JsonConvert.SerializeObject(cmsg);
        }

        /// <summary>
        /// Json2Decrypt
        /// </summary>
        /// <param name="key">server key to decrypt</param>
        /// <param name="serialized">serialized string of <see cref="CContent"/></param>
        /// <returns>deserialized and decrypted <see cref="CContent"/></returns>
        /// <exception cref="CqrException">thrown, 
        /// when serialized string to decrypt and deserialize is either null or empty 
        /// or <see cref="CContent"/> can't be decrypted and deserialized.
        /// </exception>
        public static CContent Json2Decrypt(string key, string serialized, EncodingType decoder = EncodingType.Base64, ZipType zipType = ZipType.None)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (string.IsNullOrEmpty(serialized))
                throw new CqrException("static CContent Json2Decrypt(string serverKey, string serialized): serialized is null or empty.");

            CContent cmsg = Newtonsoft.Json.JsonConvert.DeserializeObject<CContent>(serialized);

            CipherPipe cmsgPipe = new CipherPipe(key, EnDeCodeHelper.KeyToHex(key), EncodingType.Base64, ZipType.None, KeyHash.Hex, CipherMode2.ECB);
            string pipeString = cmsgPipe.PipeString;
            try
            {
                string decrypted = CipherPipe.DecrpytT<string, string>(cmsg.Message, key, KeyHash.Hex.Hash(key), 
                    EncodingType.Base64, ZipType.None, KeyHash.Hex, CipherMode2.ECB);   

                if (!cmsg.Hash.Equals(pipeString))
                    throw new CqrException($"cContent.Hash={cmsg.Hash} doesn't match PipeString={pipeString}");

                string md5Hash = MD5Sum.HashString(String.Concat(key, EnDeCodeHelper.KeyToHex(key), pipeString, decrypted), "");
                if (!md5Hash.Equals(cmsg.Md5Hash))
                    throw new CqrException($"cContent.Md5Hash={cmsg.Md5Hash} doesn't match md5Hash={md5Hash}.");

                cmsg.Message = decrypted;
                cmsg.CBytes = new byte[0];
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }
            return cmsg;
        }

        #endregion static members DeSeralizeDeCrypt<T> EncryptSerialize<T> Encrypt2Json Json2Decrypt



        public static CContent CloneCopy(CContent source, CContent destination)
        {
            if (source == null)
                return null;
            if (destination == null)
                destination = new CContent();

            destination.Hash = source.Hash;
            destination.Message = source.Message;
            destination.MsgType = source.MsgType;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;
            destination.EnCodingType = source.EnCodingType;

            return destination;

        }


        #endregion static members

    }

}
