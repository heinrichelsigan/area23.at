using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Zfx;
using Newtonsoft.Json;
using System;
using System.Linq;

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


        public virtual string EncryptToJson(string serverKey, EncodingType encoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            if (Encrypt(serverKey, encoder, zipType))
            {
                string serializedJson = ToJson();
                return serializedJson;
            }
            throw new CqrException($"EncryptToJson(string severKey failed");
        }

        public virtual bool Encrypt(string serverKey, EncodingType encoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            string pipeString = "", encrypted = "", keyHash = EnDeCodeHelper.KeyToHex(serverKey);
            try
            {
                encrypted = SymmCipherPipe.EncrpytToString(Message, serverKey, out pipeString, encoder, zipType);
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

        public virtual bool Decrypt(string serverKey, EncodingType decoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            string pipeString = "", keyHash = EnDeCodeHelper.KeyToHex(serverKey);
            try
            {
                string decrypted = SymmCipherPipe.DecrpytToString(Message, serverKey, out pipeString, EncodingType.Base64, ZipType.None);

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
                if (t is CContent cc)
                    cc.CCopy(this, cc);
                if (t is CContact cct)
                    cct.CCopy(this, cct);
                if (t is CFile cfile)
                    cfile.CCopy(this, cfile);
                else if (t is CImage cimg)
                    cimg.CCopy(this, cimg);
                else if (t is CChatRoom cchatr)
                    cchatr.CCopy(this, cchatr);
            }

            return t;
        }

        public virtual string ToXml() => Utils.SerializeToXml<CContent>(this);

        public virtual T FromXml<T>(string xmlText)
        {
            T t = Utils.DeserializeFromXml<T>(xmlText);
            if (t != null)
            {
                if (t is CContent cc)
                    cc.CCopy(this, cc);
                if (t is CContact cct)
                    cct.CCopy(this, cct);
                else if (t is CFile cfile)
                    cfile.CCopy(this, cfile);
                else if (t is CImage cimg)
                    cimg.CCopy(this, cimg);
                else if (t is CChatRoom cchatr)
                    cchatr.CCopy(this, cchatr);
                //else if (t is CSrvMsg<TC> csrvmsg)
                //                csrvmsg.CCopy(this, csrvmsg);
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
                        //if (serString.Contains("ServerMsg") && serString.Contains("ClientMsg") && serString.Contains("ServerMsgString") && serString.Contains("ClientMsgString"))
                        //{
                        //    outType = typeof(ClientSrvMsg<CSrvMsg<string>, CSrvMsg<string>>);
                        //    return (ClientSrvMsg<CSrvMsg<string>, CSrvMsg<string>>)
                        //        JsonConvert.DeserializeObject<CSrvMsg<CSrvMsg<string>, CSrvMsg<string>>>(serString);
                        //}
                        if (serString.Contains("Sender") && serString.Contains("Recipients") && serString.Contains("TContent"))
                        {
                            outType = typeof(CSrvMsg<string>);
                            return (CSrvMsg<string>)JsonConvert.DeserializeObject<CSrvMsg<string>>(serString);
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
                        //if (serString.Contains("ServerMsg") && serString.Contains("ClientMsg") && serString.Contains("ServerMsgString") && serString.Contains("ClientMsgString"))
                        //{
                        //    outType = typeof(ClientSrvMsg<CSrvMsg<string>, CSrvMsg<string>>);
                        //    return (ClientSrvMsg<CSrvMsg<string>, CSrvMsg<string>>)
                        //        Utils.DeserializeFromXml<ClientSrvMsg<CSrvMsg<string>, CSrvMsg<string>>>(serString);                            
                        //}
                        if (serString.Contains("Sender") && serString.Contains("Recipients") && serString.Contains("TContent"))
                        {
                            outType = typeof(CSrvMsg<string>);
                            return (CSrvMsg<string>)Utils.DeserializeFromXml<CSrvMsg<string>>(serString);
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


        public static string Encrypt(string serverKey, ref CContent cContent, EncodingType encType = EncodingType.Base64, ZipType zipType = ZipType.None)
        {
            // cContent.SerializedMsg = "";
            cContent.Md5Hash = "";
            string pipeString = "";
            string encryptedMsg = "";

            try
            {
                pipeString = (new SymmCipherPipe(serverKey)).PipeString;
                cContent.Hash = pipeString;
                cContent.Md5Hash = MD5Sum.HashString(String.Concat(serverKey, EnDeCodeHelper.KeyToHex(serverKey), pipeString, cContent.Message), "");

                encryptedMsg = SymmCipherPipe.EncrpytToString(cContent.Message, serverKey, out pipeString, encType, zipType);
                cContent.Message = encryptedMsg;

            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }

            return encryptedMsg;
        }

        public static CContent Decrypt(ref CContent cContent, string serverKey, EncodingType encType = EncodingType.Base64)
        {
            string pipeString = "";
            try
            {
                string decrypted = SymmCipherPipe.DecrpytToString(cContent.Message, serverKey, out pipeString, EncodingType.Base64, ZipType.None);

                if (!cContent.Hash.Equals(pipeString))
                    throw new CqrException($"cContent.Hash={cContent.Hash} doesn't match PipeString={pipeString}");

                string md5Hash = MD5Sum.HashString(String.Concat(serverKey, EnDeCodeHelper.KeyToHex(serverKey), pipeString, decrypted), "");
                if (!md5Hash.Equals(cContent.Md5Hash))
                    throw new CqrException($"cContent.Md5Hash={cContent.Md5Hash} doesn't match md5Hash={md5Hash}.");

                cContent.Message = decrypted;
                cContent.CBytes = new byte[0];
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }
            return cContent;
        }



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
