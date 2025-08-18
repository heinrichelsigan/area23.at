using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Zfx;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;

namespace Area23.At.Framework.Library.Cqr.Msg
{
    [Serializable]
    public class CContent : IMsgAble
    {

        public CType MsgType { get; set; }

        // public bool IsMime { get => IsMimeAttachment(); 

        /// <summary>
        /// Message TODO:
        /// Obsolete("TODO: remove it with hash at end", false)]
        /// </summary>
        public string Message { get; set; }

        public string SerializedMsg { get; set; }

        public string Hash { get; set; }


        public string Md5Hash { get; set; }


        public byte[] CBytes { get; set; }

        #region ctor

        /// <summary>
        /// Parameterless constructor CContent
        /// </summary>
        public CContent()
        {
            MsgType = CType.Json;
            Message = string.Empty;
            SerializedMsg = string.Empty;
            Hash = string.Empty;
            Md5Hash = string.Empty;
            CBytes = new byte[0];
        }


        /// <summary>
        /// this constructor requires a serialized or rawstring in msg
        /// </summary>
        /// <param name="serializedString">serialized string</param>
        /// <param name="msgArt">Serialization type</param>
        public CContent(string serializedString, CType msgArt = CType.None)
        {
            Md5Hash = Crypt.Hash.MD5Sum.HashString(serializedString);
            Message = serializedString;
            SerializedMsg = serializedString;
            CBytes = new byte[0];

            string _message = Message;
            Hash = VerificationHash(out _message);
            Message = _message;

            switch (msgArt)
            {
                case CType.Json:
                    MsgType = CType.Json;
                    CContent cjson = GetMsgContentType(serializedString, out Type cqrType, CType.Json);
                    if (cjson != null)
                    {
                        cjson.MsgType = CType.Json;
                        CloneCopy(cjson, this);
                    }
                    break;
                case CType.Xml:
                    MsgType = CType.Xml;
                    CContent cXml = GetMsgContentType(serializedString, out Type cqType, msgArt);
                    if (cXml != null)
                    {
                        cXml.MsgType = CType.Xml;
                        CloneCopy(cXml, this);
                    }
                    break;
                case CType.None: //TODO
                    throw new NotImplementedException("TODO: implement reverse Reflection deserialization");

                case CType.Raw:
                default:
                    MsgType = CType.Raw;
                    Message = serializedString;
                    SerializedMsg = serializedString;
                    
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
        public CContent(string plainTextMsg, string hash, CType msgArt = CType.Raw, string md5Hash = "")
        {
            MsgType = msgArt;
            Hash = hash;
            Message = plainTextMsg;
            SerializedMsg = "";
            CBytes = new byte[0];
            Md5Hash = md5Hash;

            if (msgArt == CType.Json)
            {
                SerializedMsg = this.ToJson();
            }
            if (msgArt == CType.Xml)
            {
                SerializedMsg = this.ToXml();
            }
            if (msgArt == CType.Raw)
            {
                if (plainTextMsg.Contains(hash) && plainTextMsg.IndexOf(hash) > (plainTextMsg.Length - 10))
                {
                    Message = SerializedMsg.Substring(0, SerializedMsg.Length - Hash.Length);
                }
                else
                {
                    SerializedMsg = Message + "\n" + hash + "\0";
                }
            }
            if (msgArt == CType.None)
            {
                SerializedMsg = this.ToString();
            }
        }


        public CContent(CContent srcToClone)
        {
            CloneCopy(srcToClone, this);
        }

        #endregion ctor


        public virtual CContent CCopy(CContact leftDest, CChatRoom rightSrc)
        {
            return CloneCopy(rightSrc, leftDest);
        }

        #region EnDeCrypt+DeSerialize

       
        public virtual string EncryptToJson(string serverKey)
        {
            if (Encrypt(serverKey))
            {
                string serializedJson = ToJson();
                return serializedJson;
            }
            throw new CqrException($"EncryptToJson(string severKey failed");
        }

        public virtual bool Encrypt(string serverKey)
        {
            string pipeString = "";
            try
            {
                pipeString = (new SymmCipherPipe(serverKey)).PipeString;
                Hash = pipeString;
                Md5Hash = MD5Sum.HashString(String.Concat(serverKey, Hash, pipeString, Message), "");
                Message = SymmCipherPipe.EncrpytToString(Message, serverKey, out pipeString, EncodingType.Base64, ZipType.None);
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }
            return true;
        }


        public virtual CContent DecryptFromJson(string serverKey, string serialized = "")
        {
            if (string.IsNullOrEmpty(serialized))
                serialized = this.SerializedMsg;

            CContent cc = FromJson<CContent>(serialized);
            if (cc != null && cc.Decrypt(serverKey))
            {
                CloneCopy(cc, this);
                return cc;
            }
            throw new CqrException($"DecryptFromJson<T>(string severKey, string serialized) failed");
        }

        public virtual bool Decrypt(string serverKey)
        {
            string pipeString = "";
            try
            {
                string decrypted = SymmCipherPipe.DecrpytToString(Message, serverKey, out pipeString, EncodingType.Base64, ZipType.None);

                if (!Hash.Equals(pipeString))
                    throw new CqrException($"Hash: {Hash} doesn't match symmPipe.PipeString: {pipeString}");

                string md5Hash = MD5Sum.HashString(String.Concat(serverKey, Hash, pipeString, decrypted), "");
                if (!md5Hash.Equals(Md5Hash))
                    throw new CqrException($"md5Hash: {md5Hash} doesn't match property Md5Hash: {Md5Hash}");

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
        /// Serialize <see cref="CContent"/> to Json Stting
        /// </summary>
        /// <returns>json serialized string</returns>
        public virtual string ToJson()
        {
            this.SerializedMsg = "";
            string jsonText = JsonConvert.SerializeObject(this);
            this.SerializedMsg = jsonText;
            return jsonText;
        }

        public virtual T FromJson<T>(string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText))
                jsonText = SerializedMsg;

            T t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonText);
            if (t != null && t is CContent cc)
            {
                CloneCopy(cc, this);
            }
            return t;
        }

        public virtual string ToXml()
        {
            SerializedMsg = "";
            string xmlString = Utils.SerializeToXml<CContent>(this);
            SerializedMsg = xmlString;
            return xmlString;
        }

        public virtual T FromXml<T>(string xmlText)
        {
            T cqrT = Utils.DeserializeFromXml<T>(xmlText);
            if (cqrT is CContent cc)
            {
                CloneCopy(cc, this);
            }

            return cqrT;
        }

        public override string ToString()
        {
            string s = this.GetType().ToString() + "\n";
            var fields = Utils.GetAllFields(this.GetType());
            foreach (var field in fields)
                s += field.Name + " \t\"" + field.GetRawConstantValue().ToString() + "\"\n";
            var props = Utils.GetAllProperties(this.GetType());
            foreach (var prp in props)
                s += prp.Name + " \t\"" + prp.GetRawConstantValue().ToString() + "\"\n";

            return s;
        }

        #endregion serialization / deserialization


        #region members

        //public CContent SetMsgContent(string plainMsg)
        //{
        //    CContent msgContent = new CContent(plainMsg);
        //    Message = msgContent.Message;
        //    SerializedMsg = msgContent.SerializedMsg;
        //    Hash = msgContent.Hash;

        //    return (CContent)this;
        //}


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

            if ((SerializedMsg.IsValidJson() && SerializedMsg.Contains("FileName") && SerializedMsg.Contains("Base64Type")) ||
                (SerializedMsg.IsValidXml() && SerializedMsg.Contains("FileName") && SerializedMsg.Contains("Base64Type")))
                return true;

            return false;
        }


        public virtual CFile ToCFile()
        {
            if (this is CFile cf && string.IsNullOrEmpty(cf.FileName) && cf.Data != null)
                return cf;

            if (SerializedMsg.IsValidJson() && SerializedMsg.Contains("FileName") && SerializedMsg.Contains("Base64Type"))
                return (CFile)JsonConvert.DeserializeObject<CFile>(SerializedMsg);
            else if (SerializedMsg.IsValidXml() && SerializedMsg.Contains("CqrFileName") && SerializedMsg.Contains("Base64Type"))
                return (CFile)Static.Utils.DeserializeFromXml<CFile>(SerializedMsg);

            return null;
        }

        #endregion members

        #region static members

        public static CContent GetMsgContentType(string serString, out Type outType, CType msgType = CType.None)
        {
            outType = typeof(CContent);
            switch (msgType)
            {
                case CType.Json:
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
                            cFile.SerializedMsg = serString;
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
                case CType.Xml:
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
                case CType.Raw:
                case CType.None:
                default: throw new NotImplementedException("GetMsgContentType(...): case MsgEnum.RawWithHashAtEnd and MsgEnum.None not implemented");
            }

            return null;
        }

        public static string Encrypt(string serverKey, CContent cContent, EncodingType encType = EncodingType.Base64, ZipType zipType = ZipType.None)
        {
            cContent.SerializedMsg = "";
            cContent.Md5Hash = "";
            string pipeString = "";
            string encryptedMsg = "";

            try
            {
                cContent.Hash = (new SymmCipherPipe(serverKey)).PipeString;
                cContent.Md5Hash = MD5Sum.HashString(String.Concat(serverKey, cContent.Hash, cContent.Hash, cContent.Message), "");
                cContent.SerializedMsg = cContent.ToJson();

                encryptedMsg = SymmCipherPipe.EncrpytToString(cContent.SerializedMsg, serverKey, out pipeString, encType, zipType);
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }

            return encryptedMsg;
        }

        public static CContent Decrypt(string cryptedEncodedMsg, string serverKey, EncodingType encType = EncodingType.Base64)
        {
            CContent ccontent = null;
            string pipeString = "";
            try
            {
                string decrypted = SymmCipherPipe.DecrpytToString(cryptedEncodedMsg, serverKey, out pipeString, EncodingType.Base64, ZipType.None);
                if (string.IsNullOrEmpty(decrypted) || !decrypted.IsValidJson())
                    throw new CqrException($"md5Hash: {decrypted} isn't a valid json.");

                ccontent = JsonConvert.DeserializeObject<CContent>(decrypted);

                if (!ccontent.Hash.Equals(pipeString))
                    throw new CqrException($"Hash: {ccontent.Hash} doesn't match symmPipe.PipeString: {pipeString}");

                string md5Hash = MD5Sum.HashString(String.Concat(serverKey, pipeString, pipeString, decrypted), "");
                if (!md5Hash.Equals(ccontent.Md5Hash))
                    throw new CqrException($"md5Hash: {md5Hash} doesn't match property Md5Hash: {ccontent.Md5Hash}");

                ccontent.CBytes = new byte[0];
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }
            return ccontent;
        }


        public static CContent CloneCopy(CContent source, CContent destination)
        {
            if (source == null)
                return null;
            if (destination == null)
                destination = new CContent(source);

            destination.Hash = source.Hash;
            destination.Message = source.Message;
            destination.MsgType = source.MsgType;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;
            destination.SerializedMsg = "";
            destination.SerializedMsg = destination.ToJson();
            return destination;

        }


        #endregion static members

    }

}
