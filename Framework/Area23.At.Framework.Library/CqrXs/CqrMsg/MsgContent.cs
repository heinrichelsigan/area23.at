using Area23.At.Framework.Library;
using Area23.At.Framework.Library.CqrMsg;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.Serialization;


namespace Area23.At.Framework.Library.CqrXs.CqrMsg
{


    /// <summary>
    /// Represtents a MsgContent
    /// </summary>
    [Serializable]
    public class MsgContent : ICqrMessagable
    {
        public string _hash;
        public string _message;

        public MsgEnum MsgType { get; set; }

        // public bool IsMime { get => IsMimeAttachment(); 

        /// <summary>
        /// Message TODO:
        /// Obsolete("TODO: remove it with hash at end", false)]
        /// </summary>
        public string Message
        {
            get
            {
                if (_message.EndsWith("\n" + _hash + "\0"))
                    _message = _message.Substring(0, _message.LastIndexOf("\n" + _hash + "\0"));
                else if (_message.EndsWith("\n" + _hash))
                    _message = _message.Substring(0, _message.LastIndexOf("\n" + _hash));
                else if (_message.EndsWith(_hash + "\0"))
                    _message = _message.Substring(0, _message.LastIndexOf(_hash + "\0"));

                return _message;
            }
        }


        public string RawMessage { get; set; }

        public string Hash { get => _hash; }


        public string Md5Hash { get; set; }


        #region ctor

        /// <summary>
        /// Parameterless constructor MsgContent
        /// </summary>
        public MsgContent()
        {
            MsgType = MsgEnum.None;
            _message = string.Empty;
            RawMessage = string.Empty;
            _hash = string.Empty;
            Md5Hash = string.Empty;
        }


        /// <summary>
        /// this constructor requires a serialized or rawstring in msg
        /// </summary>
        /// <param name="serializedString">serialized string</param>
        /// <param name="msgArt">Serialization type</param>
        public MsgContent(string serializedString, MsgEnum msgArt = MsgEnum.None)
        {
            Md5Hash = Crypt.Hash.MD5Sum.HashString(serializedString);
            _message = serializedString;
            RawMessage = serializedString;
            _hash = VerificationHash(out _message);

            switch (msgArt)
            {
                case MsgEnum.Json:                  
                    MsgType = MsgEnum.Json;
                    MsgContent c = GetMsgContentType(serializedString, out Type cqrType, MsgEnum.Json);
                    if (c != null)
                    {
                        RawMessage = c.RawMessage;
                        _hash = c._hash;
                        _message = c._message;
                        Md5Hash = Crypt.Hash.MD5Sum.HashString(_message);
                    }
                    break;
                case MsgEnum.Xml:
                    MsgType = MsgEnum.Xml;
                    MsgContent cXml = GetMsgContentType(serializedString, out Type cqType, msgArt);
                    if (cXml != null)
                    {
                        RawMessage = cXml.RawMessage;
                        _hash = cXml._hash;
                        _message = cXml._message;
                        Md5Hash = Crypt.Hash.MD5Sum.HashString(_message);
                    }
                    break;
                case MsgEnum.None: //TODO
                    throw new NotImplementedException("TODO: implement reverse Reflection deserialization");

                case MsgEnum.RawWithHashAtEnd:
                default:
                    MsgType = MsgEnum.RawWithHashAtEnd;
                    _message = serializedString;
                    RawMessage = serializedString;
                    _hash = VerificationHash(out _message);
                    Md5Hash = Crypt.Hash.MD5Sum.HashString(RawMessage);
                    break;                
            }
            
        }

        /// <summary>
        /// this ctor requires a plainstring and serialize it in _rawMessage
        /// </summary>
        /// <param name="plainTextMsg">plain text message</param>
        /// <param name="hash"></param>
        /// <param name="msgArt"></param>
        public MsgContent(string plainTextMsg, string hash, MsgEnum msgArt = MsgEnum.RawWithHashAtEnd, string md5Hash = "")
        {
            MsgType = msgArt;
            _hash = hash;
            _message = plainTextMsg;
            RawMessage = plainTextMsg;
            Md5Hash = md5Hash;

            if (msgArt == MsgEnum.Json)
            {
                RawMessage = this.ToJson();
            }
            if (msgArt == MsgEnum.Xml)
            {
                RawMessage = Utils.SerializeToXml<MsgContent>(this);
            }
            if (msgArt == MsgEnum.RawWithHashAtEnd)
            {
                if (plainTextMsg.Contains(hash) && plainTextMsg.IndexOf(hash) > (plainTextMsg.Length - 10))
                {
                    _message = RawMessage.Substring(0, RawMessage.Length - _hash.Length);
                }
                else
                {
                    RawMessage = _message + "\n" + hash + "\0";
                }
            }
            if (msgArt == MsgEnum.None)
            {
                RawMessage = this.ToString();
            }
        }

        #endregion ctor

        public MsgContent SetMsgContent(string plainMsg)
        {
            MsgContent msgContent = new MsgContent(plainMsg);
            _message = msgContent.Message;
            RawMessage = msgContent.RawMessage;
            _hash = msgContent._hash;

            return (MsgContent)this;
        }

        #region serialization / deserialization

        public virtual string ToJson() => Newtonsoft.Json.JsonConvert.SerializeObject(this);

        public virtual T FromJson<T>(string jsonText)
        {
            T t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonText);
            if (t != null && t is MsgContent mc)
            {
                this._hash = mc.Hash;
                this._message = mc._message;
                this.RawMessage = mc.RawMessage;
            }
            return t;
        }

        public virtual string ToXml() => Utils.SerializeToXml<MsgContent>(this);
       
        public virtual T FromXml<T>(string xmlText)
        {
            T cqrT = Utils.DeserializeFromXml<T>(xmlText);
            if (cqrT is MsgContent mc)
            {
                this._hash = mc._hash;
                this.RawMessage = mc.RawMessage;
                this._message = mc._message;
            }
            
            return cqrT;
        }

        public override string ToString()
        {
            string s = this.GetType().ToString() + "\n";
            var fields = Utils.GetAllFields(this.GetType());
            foreach (var field in fields)           
                s += field.Name + " \t\"" + field.GetRawConstantValue()?.ToString() + "\"\n";            
            var props = Utils.GetAllProperties(this.GetType());
            foreach (var prp in props)
                s += prp.Name + " \t\"" + prp.GetRawConstantValue()?.ToString() + "\"\n";

            return s;
        }

        #endregion serialization / deserialization

        public virtual string VerificationHash(out string msg)
        {
            msg = _message;
            if (!string.IsNullOrEmpty(_hash))
            {
                return _hash;
            }

            if (IsCqrFile())
            {
                CqrFile cqFile = ToCqrFile();
                if (cqFile != null && !string.IsNullOrEmpty(cqFile.Hash))
                {
                    _hash = cqFile._hash;
                    if (!string.IsNullOrEmpty(cqFile._message))
                        msg = cqFile._message;

                    return _hash;
                }

            }

            if (RawMessage.Length > 9)
            {
                // if (_message.Contains('\n') && _message.LastIndexOf('\n') < _message.Length)
                string tmp = RawMessage.Substring(RawMessage.Length - 10);
                if (tmp.Contains('\n') && tmp.IndexOf('\n') < 9)
                {
                    _hash = tmp.Substring(tmp.LastIndexOf('\n') + 1);
                    if (_hash.Contains("\0"))
                        _hash = _hash.Substring(0, _hash.LastIndexOf("\0"));
                }
            }
            else
            {
                _hash = RawMessage;
            }

            if (string.IsNullOrEmpty(_hash))
            {
                string hsh = "";
                if (RawMessage.Contains("\"_hash\":\""))
                {
                    int hshlen = "\"_hash\":\"".Length;
                    int hidx = RawMessage.IndexOf("\"_hash\":\"");
                    if (hidx > 0)
                    {
                        hsh = RawMessage.Substring((int)(hidx + hshlen));
                        if ((hidx = hsh.IndexOf("\"")) > 0)
                        {
                            _hash = hsh.Substring(0, hidx);
                            return _hash;
                        }
                    }
                }
            }


            if (_hash != null && _hash.Length > 4 && RawMessage.Substring(RawMessage.Length - _hash.Length).Equals(_hash, StringComparison.InvariantCulture))
                msg = RawMessage.Substring(0, RawMessage.Length - _hash.Length);

            return _hash ?? string.Empty;
        }


        public virtual bool IsCqrFile()
        {
            if (this is CqrFile cf && string.IsNullOrEmpty(cf.CqrFileName) && cf.Data != null)
                return true;

            if ((RawMessage.IsValidJson() && RawMessage.Contains("CqrFileName") && RawMessage.Contains("Base64Type")) ||
                (RawMessage.IsValidXml() && RawMessage.Contains("CqrFileName") && RawMessage.Contains("Base64Type")))
                return true;

            return false;
        }


        public virtual CqrFile ToCqrFile()
        {
            if (this is CqrFile cf && string.IsNullOrEmpty(cf.CqrFileName) && cf.Data != null)
                return cf;

            if (RawMessage.IsValidJson() && RawMessage.Contains("CqrFileName") && RawMessage.Contains("Base64Type"))
                return (CqrFile)JsonConvert.DeserializeObject<CqrFile>(RawMessage);
            else if (RawMessage.IsValidXml() && RawMessage.Contains("CqrFileName") && RawMessage.Contains("Base64Type"))
                return (CqrFile)Static.Utils.DeserializeFromXml<CqrFile>(RawMessage);

            return null;
        }


        #region static members

        public static MsgContent GetMsgContentType(string serString, out Type outType, MsgEnum msgType = MsgEnum.None)
        {
            outType = typeof(MsgContent);
            switch (msgType)
            {
                case MsgEnum.Json:
                    if (serString.IsValidJson())
                    {
                        //if (serString.Contains("ServerMsg") && serString.Contains("ClientMsg") && serString.Contains("ServerMsgString") && serString.Contains("ClientMsgString"))
                        //{
                        //    outType = typeof(ClientSrvMsg<FullSrvMsg<string>, FullSrvMsg<string>>);
                        //    return (ClientSrvMsg<FullSrvMsg<string>, FullSrvMsg<string>>)
                        //        JsonConvert.DeserializeObject<ClientSrvMsg<FullSrvMsg<string>, FullSrvMsg<string>>>(serString);
                        //}
                        if (serString.Contains("Sender") && serString.Contains("Recipients") && serString.Contains("TContent"))
                        {
                            outType = typeof(FullSrvMsg<string>);
                            return (FullSrvMsg<string>)JsonConvert.DeserializeObject<FullSrvMsg<string>>(serString);
                        }

                        if (serString.Contains("CqrFileName") && serString.Contains("Base64Type"))
                        {
                            outType = typeof(CqrFile);
                            CqrFile cqrFile = (CqrFile)JsonConvert.DeserializeObject<CqrFile>(serString);
                            cqrFile.RawMessage = serString;
                            return cqrFile;
                        }
                        if (serString.Contains("ImageFileName") && serString.Contains("ImageMimeType"))
                        {
                            outType = typeof(CqrImage);
                            return (CqrImage)JsonConvert.DeserializeObject<CqrImage>(serString);
                        }
                        if (serString.Contains("ContactId") && serString.Contains("Cuid") && serString.Contains("Email"))
                        {
                            outType = typeof(CqrContact);
                            return (CqrContact)JsonConvert.DeserializeObject<CqrContact>(serString);
                        }

                        outType = typeof(MsgContent);
                        return (MsgContent)JsonConvert.DeserializeObject<MsgContent>(serString);
                    }
                    break;
                case MsgEnum.Xml:
                    if (serString.IsValidXml())
                    {
                        //if (serString.Contains("ServerMsg") && serString.Contains("ClientMsg") && serString.Contains("ServerMsgString") && serString.Contains("ClientMsgString"))
                        //{
                        //    outType = typeof(ClientSrvMsg<FullSrvMsg<string>, FullSrvMsg<string>>);
                        //    return (ClientSrvMsg<FullSrvMsg<string>, FullSrvMsg<string>>)
                        //        Utils.DeserializeFromXml<ClientSrvMsg<FullSrvMsg<string>, FullSrvMsg<string>>>(serString);                            
                        //}
                        if (serString.Contains("Sender") && serString.Contains("Recipients") && serString.Contains("TContent"))
                        {
                            outType = typeof(FullSrvMsg<string>);
                            return (FullSrvMsg<string>)Utils.DeserializeFromXml<FullSrvMsg<string>>(serString);
                        }
                        if (serString.Contains("CqrFileName") && serString.Contains("Base64Type"))
                        {
                            outType = typeof(CqrFile);
                            return (CqrFile)Utils.DeserializeFromXml<CqrFile>(serString);
                        }
                        if (serString.Contains("ImageFileName") && serString.Contains("ImageMimeType"))
                        {
                            outType = typeof(CqrImage);
                            return (CqrImage)Utils.DeserializeFromXml<CqrImage>(serString);
                        }
                        if (serString.Contains("ContactId") && serString.Contains("Cuid") && serString.Contains("Email"))
                        {
                            outType = typeof(CqrContact);
                            return (CqrContact)Utils.DeserializeFromXml<CqrContact>(serString);
                        }

                        outType = typeof(MsgContent);
                        return (MsgContent)Utils.DeserializeFromXml<MsgContent>(serString);
                    }
                    break;
                case MsgEnum.RawWithHashAtEnd:
                case MsgEnum.None:
                default: throw new NotImplementedException("GetMsgContentType(...): case MsgEnum.RawWithHashAtEnd and MsgEnum.None not implemented");
            }

            return null;
        }

        #endregion static members

    }


}
