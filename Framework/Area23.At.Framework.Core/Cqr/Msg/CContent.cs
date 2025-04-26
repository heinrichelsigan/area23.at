using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Static;
using Newtonsoft.Json;
using System.Text;

namespace Area23.At.Framework.Core.Cqr.Msg
{
    [Serializable]
    public class CContent : IMsgAble
	{
		public string _hash;
		public string _message;

		public CType MsgType { get; set; }

		// public bool IsMime { get => IsMimeAttachment(); 

		/// <summary>
		/// Message TODO:
		/// Obsolete("TODO: remove it with hash at end", false)]
		/// </summary>
		public string Message { get => _message; }
		
		public string SerializedMsg { get; set; }

		public string Hash { get => _hash; }


		public string Md5Hash { get; set; }

		
		public byte[] CBytes { get; set; }			

        #region ctor

        /// <summary>
        /// Parameterless constructor CContent
        /// </summary>
        public CContent()
		{
			MsgType = CType.None;
			_message = string.Empty;
			SerializedMsg = string.Empty;
			_hash = string.Empty;
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
			_message = serializedString;
			SerializedMsg = serializedString;
            CBytes = new byte[0];
            _hash = VerificationHash(out _message);

			switch (msgArt)
			{
				case CType.Json:
					MsgType = CType.Json;
					CContent cjson = GetMsgContentType(serializedString, out Type cqrType, CType.Json);
					if (cjson != null)
					{
                        cjson.MsgType = CType.Json;
                        CCopy(this, cjson);
                    }
					break;
				case CType.Xml:
                    MsgType = CType.Xml;
                    CContent cXml = GetMsgContentType(serializedString, out Type cqType, msgArt);
                    if (cXml != null)
                    {
                        cXml.MsgType = CType.Xml;
                        CCopy(this, cXml);
                    }
                    break;
				case CType.None: //TODO
					throw new NotImplementedException("TODO: implement reverse Reflection deserialization");

				case CType.Raw:
				default:
					MsgType = CType.Raw;
					_message = serializedString;
					SerializedMsg = serializedString;
					_hash = VerificationHash(out _message);
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
			_hash = hash;
			_message = plainTextMsg;
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
					_message = SerializedMsg.Substring(0, SerializedMsg.Length - _hash.Length);
				}
				else
				{
					SerializedMsg = _message + "\n" + hash + "\0";
				}
			}
			if (msgArt == CType.None)
			{
				SerializedMsg = this.ToString();
            }
        }


        public CContent(CContent srcToClone)
        {
            CCopy(this, srcToClone);
        }

        #endregion ctor

        public virtual CContent CCopy(CContent leftDest, CContent rightSrc)
        {
            if (rightSrc == null)
                return null;
            if (leftDest == null)
                leftDest = new CContent(rightSrc);

            leftDest._hash = rightSrc._hash;
            leftDest._message = rightSrc._message;
            leftDest.MsgType = rightSrc.MsgType;
            leftDest.CBytes = rightSrc.CBytes;
            leftDest.Md5Hash = rightSrc.Md5Hash;
            leftDest.SerializedMsg = "";
            leftDest.SerializedMsg = leftDest.ToJson();
            return leftDest;

        }

        #region EnDeCrypt+DeSerialize


        public virtual byte[] EncryptToJsonToBytes(string serverKey)
		{
			string serialized = EncryptToJson(serverKey);
			return Encoding.UTF8.GetBytes(serialized);
		}

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
			try
			{
				string hash = EnDeCodeHelper.KeyToHex(serverKey);
				SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, hash);				
				_hash = symmPipe.PipeString;                
                Md5Hash = MD5Sum.HashString(String.Concat(serverKey, _hash, symmPipe.PipeString, _message), "");

                byte[] msgBytes = EnDeCodeHelper.GetBytesFromString(Message);
				byte[] cqrbytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, serverKey, hash) : msgBytes;

				CBytes = cqrbytes;
				_message = Base64.ToBase64(CBytes);
			} 
			catch (Exception exCrypt)
			{
				CqrException.SetLastException(exCrypt);
				throw;
			}
			return true;
        }

        public virtual CContent? DecryptFromJsonFromBytes(string serverKey, byte[] serializedBytes)
		{
			string serialized = Encoding.UTF8.GetString(serializedBytes);
			return DecryptFromJson(serverKey, serialized);
		}


        public virtual CContent? DecryptFromJson(string serverKey, string serialized = "")
		{
			if (string.IsNullOrEmpty(serialized))
				serialized = this.SerializedMsg;

            CContent? cc = FromJson<CContent>(serialized);
			if (cc != null && cc.Decrypt(serverKey))
			{
				CCopy(this, cc);				
				return cc;
			}
            throw new CqrException($"DecryptFromJson<T>(string severKey, string serialized) failed");
        }
		
		public virtual bool Decrypt(string serverKey)
        {
            try
            {
                string hash = EnDeCodeHelper.KeyToHex(serverKey);
                SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, hash);                
                
				byte[] cipherBytes = CBytes;
                byte[] unroundedMerryBytes = LibPaths.CqrEncrypt ? symmPipe.DecrpytRoundGoMerry(cipherBytes, serverKey, hash) : cipherBytes;
                string decrypted = EnDeCodeHelper.GetString(unroundedMerryBytes); //DeEnCoder.GetStringFromBytesTrimNulls(unroundedMerryBytes);
                while (decrypted[decrypted.Length - 1] == '\0')
                    decrypted = decrypted.Substring(0, decrypted.Length - 1);
                              
				
				if (!_hash.Equals(symmPipe.PipeString))
					throw new CqrException($"Hash: {_hash} doesn't match symmPipe.PipeString: {symmPipe.PipeString}");
                string md5Hash = MD5Sum.HashString(String.Concat(serverKey, _hash, symmPipe.PipeString, decrypted), "");
                if (!md5Hash.Equals(Md5Hash))
                    throw new CqrException($"md5Hash: {md5Hash} doesn't match property Md5Hash: {Md5Hash}");
                
				_message = decrypted;
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

        public virtual T? FromJson<T>(string jsonText)
        {
            if (string.IsNullOrEmpty(jsonText))
                jsonText = SerializedMsg;

            T? t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonText);
            if (t != null && t is CContent cc)
            {
				CCopy(this, cc);
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
			T? cqrT = Utils.DeserializeFromXml<T>(xmlText);
			if (cqrT is CContent cc)
			{
				CCopy(this, cc);
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


        #region members

        public CContent SetMsgContent(string plainMsg)
        {
            CContent msgContent = new CContent(plainMsg);
            _message = msgContent.Message;
            SerializedMsg = msgContent.SerializedMsg;
            _hash = msgContent._hash;

            return (CContent)this;
        }


        public virtual string VerificationHash(out string msg)
		{
			msg = _message;
			if (!string.IsNullOrEmpty(_hash))
			{
				return _hash;
			}

			if (IsCFile())
			{
				CFile cqFile = ToCFile();
				if (cqFile != null && !string.IsNullOrEmpty(cqFile.Hash))
				{
					_hash = cqFile._hash;
					if (!string.IsNullOrEmpty(cqFile._message))
						msg = cqFile._message;

					return _hash;
				}

			}

			if (SerializedMsg.Length > 9)
			{
				// if (_message.Contains('\n') && _message.LastIndexOf('\n') < _message.Length)
				string tmp = SerializedMsg.Substring(SerializedMsg.Length - 10);
				if (tmp.Contains('\n') && tmp.IndexOf('\n') < 9)
				{
					_hash = tmp.Substring(tmp.LastIndexOf('\n') + 1);
					if (_hash.Contains("\0"))
						_hash = _hash.Substring(0, _hash.LastIndexOf("\0"));
				}
			}
			else
			{
				_hash = SerializedMsg;
			}

			if (string.IsNullOrEmpty(_hash))
			{
				string hsh = "";
				if (SerializedMsg.Contains("\"_hash\":\""))
				{
					int hshlen = "\"_hash\":\"".Length;
					int hidx = SerializedMsg.IndexOf("\"_hash\":\"");
					if (hidx > 0)
					{
						hsh = SerializedMsg.Substring((int)(hidx + hshlen));
						if ((hidx = hsh.IndexOf("\"")) > 0)
						{
							_hash = hsh.Substring(0, hidx);
							return _hash;
						}
					}
				}
			}


			if (_hash != null && _hash.Length > 4 && SerializedMsg.Substring(SerializedMsg.Length - _hash.Length).Equals(_hash, StringComparison.InvariantCulture))
				msg = SerializedMsg.Substring(0, SerializedMsg.Length - _hash.Length);

			return _hash ?? string.Empty;
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

		#endregion static members

	}


}
