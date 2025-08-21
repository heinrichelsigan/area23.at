using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Crypt.Hash;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using Newtonsoft.Json;

namespace Area23.At.Framework.Core.Cqr.Msg
{

    [Serializable]
    public class CFile : CContent, IMsgAble
    {

        #region properties 

        public string FileName { get; set; }

        public string Base64Type { get; set; }

        public byte[] Data { get; set; }

        public string Sha256Hash { get; protected internal set; }

        [JsonIgnore]
        protected internal long FileByteLen { get => Data.LongLength; }

        #endregion properties 

        #region ctors

        public CFile() : base()
        {
            FileName = string.Empty;
            Base64Type = string.Empty;
            Sha256Hash = string.Empty;
            Data = new byte[0];
			EnCodingType = EncodingType.Base64;
		}

        public CFile(string fileName, byte[] data, string hash = "") : this()
        {
            FileName = fileName;            
            Data = data;
            Base64Type = MimeType.GetMimeType(Data, FileName);
            Hash = hash;
            MsgType = SerType.Json;
            Sha256Hash = Sha256Sum.Hash(Data, "");
            EnCodingType = EncodingType.Base64;
        }

        public CFile(string fileName, string mimeType, byte[] data, string hash) : this(fileName, data, hash) 
        {            
            Base64Type = mimeType;         
        }

        public CFile(string fileName, string base64, string hash = "") : this()
        {
            FileName = fileName;
            Data = Convert.FromBase64String(base64);
            Base64Type = MimeType.GetMimeType(Data, FileName);
            Hash = hash;            
            Sha256Hash = Sha256Sum.Hash(Data, "");
            MsgType = SerType.Json;
            EnCodingType = EncodingType.Base64;
        }

        public CFile(string fileName, string mimeType, string base64, string hash) : this(fileName, base64, hash) 
        {            
            Base64Type = mimeType;            
        }

        public CFile(string fileName, string mimeType, byte[] data, string hash, string sMd5 = "", string sSha256 = "")
             : this(fileName, mimeType, data, hash)
        {
            Md5Hash = sMd5;
            Sha256Hash = sSha256;
        }

        public CFile(string fileName, string mimeType, byte[] data, string hash, string sMd5 = "", string sSha256 = "", SerType msgType = SerType.Json) :
                this(fileName, mimeType, data, hash, sMd5, sSha256)
        {
            MsgType = msgType;
        }

        public CFile(string fileName, string mimeType, byte[] data, string hash, string sMd5 = "", string sSha256 = "", SerType msgType = SerType.Json, EncodingType enCodeType = EncodingType.Base64) :
                this(fileName, mimeType, data, hash, sMd5, sSha256, msgType)
        {
            this.EnCodingType = enCodeType;
        }

        public CFile(FileInfo fi, string hash = "") : this()
        {
            FileName = fi.Name;            
            Data = System.IO.File.ReadAllBytes(fi.FullName);
            Base64Type = MimeType.GetMimeType(Data, FileName);
            Md5Hash = "";
            Sha256Hash = Sha256Sum.Hash(Data, "");
            MsgType = SerType.Json;
            EnCodingType = EncodingType.Base64;
            Hash = hash;
        }

        public CFile(string filePath, string hash = "", SerType msgType = SerType.Json) : this()
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"Didn't find a file at filePath = {filePath}");

            FileName = Path.GetFileName(filePath);
            Data = System.IO.File.ReadAllBytes(filePath);
            Base64Type = MimeType.GetMimeType(Data, FileName);
            Md5Hash = "";
            Sha256Hash = Sha256Sum.Hash(Data, "");
            MsgType = msgType;
            EnCodingType = EncodingType.Base64;
            Hash = hash;
        }

		/// <summary>
		/// Constructor CqrFile from an json, xml or raw serialized plaintext
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="msgType"></param>
		public CFile(string plainText, SerType msgType = SerType.Json)
        {
            CFile cf = GetCFile(plainText, msgType);
            CloneCopy(cf, this);
        }

        /// <summary>
        /// CFile constructor with antoher <see cref="CFile"/> to clone
        /// </summary>
        /// <param name="cFile"><see cref="CFile"/> to clone</param>
        public CFile(CFile cFile) : this()
        {
            if (cFile != null)
				CloneCopy(cFile, this);
		}

        #endregion ctors


        #region EnDeCrypt+DeSerialize

        public override string EncryptToJson(string serverKey, EncodingType encoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            CFile cFile = new CFile(this);
            string serializedJson = ToJsonEncrypt(serverKey, ref cFile, encoder, zipType);
            if (string.IsNullOrEmpty(serializedJson))
                throw new CqrException($"override string EncryptToJson(string serverKey) failed");

            return serializedJson;
        }

        public new CFile? DecryptFromJson(string serverKey, string serialized = "",
            EncodingType decoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            CFile? cfile = FromJsonDecrypt(serverKey, serialized, decoder, zipType);
            if (cfile == null)
                throw new CqrException($"override File? DecryptFromJson(string serverKey, string serialized) failed");                

            return CFile.CloneCopy(cfile, this);            
        }

        #endregion EnDeCrypt+DeSerialize


        #region members

        public override CContent CCopy(CContent leftDest, CContent rightSrc)
        {
            if (leftDest is CFile && rightSrc is CFile)
                return CFile.CloneCopy(rightSrc, leftDest);

            return base.CCopy(leftDest, rightSrc);
        }

        public virtual string ToBase64() => Convert.ToBase64String(Data);
    
        public override string ToXml() => Utils.SerializeToXml<CFile>(this);
        
        public CFile GetCFile(string encodedSerilizedOrRawText, SerType msgArt = SerType.Json)
        {
            if (msgArt == SerType.None || msgArt == SerType.Raw)
            {
                throw new NotImplementedException("CqrFile GetCqrFile(string encodedSerilizedOrRawText, MsgEnum msgArt = MsgEnum.Json)" +
                    " is not implemented for MsgEnum.None and MsgEnum.RawWithHashAtEnd");
            }
            if (msgArt == SerType.Mime)
            {
                Data = new byte[0];

                bool mimeGot = GetByBase64Attachment(encodedSerilizedOrRawText,
                    out string cqrFileName, out string base64Type,
                    out string md5Hash, out string sha256Hash,
                    out string mimeBase64, out string hash);
                if (mimeGot && !string.IsNullOrEmpty(mimeBase64))
                {
                    FileName = cqrFileName;
                    Base64Type = base64Type;
                    Md5Hash = md5Hash;
                    Sha256Hash = sha256Hash;
                    Data = Convert.FromBase64String(mimeBase64);
                    Hash = hash;
                }
            }
            else if (msgArt == SerType.Json)
            {
                this.FromJson<CFile>(encodedSerilizedOrRawText);
            }
            else if (msgArt == SerType.Xml)
            {
                this.FromXml<CFile>(encodedSerilizedOrRawText);
            }
            return this;
        }

        /// <summary>
        /// Get Html Page embedding CqrFile
        /// </summary>
        /// <returns>html as string rendered</returns>
        public string GetWebPage()
        {
            string base64Mime = Convert.ToBase64String(Data);
            string html = $"<html>\n\t<head>\n\t\t<title>{FileName} {Base64Type}</title>\n\t</head>";
            html += $"\n\t<body>\n\t\t";
            if (MimeType.IsMimeTypeImage(Base64Type))
                html += $"\n\t\t<img src=\"data:{Base64Type};base64,{base64Mime}\" alt=\"{Base64Type} {FileName}\" />";
            if (MimeType.IsMimeTypeDocument(Base64Type))
            {
                html += $"\n\t\t<object data=\"data:{Base64Type};base64,{base64Mime}\" type=\"{Base64Type}\" width=\"640px\" height=\"480px\" >";
                html += $"\n\t\t\t<p>Unable to display {Base64Type} <b>{FileName}</b></p>";
                html += $"\n\t\t</object>";
            }
            if (MimeType.IsMimeTypeAudio(Base64Type))
            {
                html += $"\n\t\t<audio controls>";
                html += $"\n\t\t\t<source src=\"data:{Base64Type};base64,{base64Mime}\" type=\"{Base64Type}\">";
                html += $"\n\t\tYour browser does not support the audio element.";
                html += $"\n\t\t</audio>";
            }
            if (MimeType.IsMimeTypeVideo(Base64Type))
            {
                html += $"\n\t\t<video width=\"320\" height=\"240\" controls>";
                html += $"\n\t\t\t<source src=\"data:{Base64Type};base64,{base64Mime}\" type=\"{Base64Type}\">";
                html += $"\n\t\tYour browser does not support the video tag.";
                html += $"\n\t\t</video>";
            }
            html += $"\n\t</body>\n\t\t";
            html += $"\n</html>\n";


            return html;
        }

        /// <summary>
        /// GetFileNameContentLength write <see cref="CqrFileName"/> and <see cref="Data.Length"/>
        /// </summary>
        /// <returns>CqrFileName + " [" + Data.Length + "]";</returns>
        public string GetFileNameContentLength()
        {
            string fileCLen = FileName + " [" + Data.Length + "]";
            return fileCLen;
        }

        #endregion members


        #region static members

        #region static members SaveCFile LoadCFile GetByBase64Attachment

        public static void SaveCFile(CFile file, string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
                throw new NullReferenceException("SaveCFile((CFile file, string directoryPath = null");

            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory {directoryPath} could not be found.");

            string saveFileName = Path.Combine(directoryPath, file.FileName);
            System.IO.File.WriteAllBytes(saveFileName, file.Data);

            return;
        }

        public static CFile LoadCFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new NullReferenceException("LoadCFile(string filePath = null");

            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"File {filePath} could not be found.");

            string fileName = Path.GetFileName(filePath);
            byte[] data = System.IO.File.ReadAllBytes(filePath);
            string mimeType = MimeType.GetMimeType(data, fileName);

            CFile cfile = new CFile(fileName, mimeType, data, "");

            return cfile;
        }


        internal static bool GetByBase64Attachment(string plainAttachment,
            out string cqrFileName, out string base64Type,
            out string md5Hash, out string sha256Hash,
            out string mimeBase64, out string Hash)
        {
            string restString = plainAttachment;

            base64Type = restString.GetSubStringByPattern("Content-Type: ", true, "", ";", false);
            cqrFileName = restString.GetSubStringByPattern("; name=\"", true, "", "\";", false);
            string contentLengthString = restString.GetSubStringByPattern("Content-Length: ", true, "", ";\n", false);
            string contentLenString = string.Empty;
            foreach (char ch in contentLengthString.ToCharArray())
                if (Char.IsDigit(ch) || Char.IsNumber(ch) || ch == '.')
                    contentLenString += ch.ToString();
            int contentLen = Int32.Parse(contentLenString);

            Hash = restString.GetSubStringByPattern("Content-Verification: ", true, "", ";", false);
            md5Hash = restString.GetSubStringByPattern("md5=\"", true, "", "\";", false);
            sha256Hash = restString.GetSubStringByPattern("sha256=\"", true, "", "\";", false);

            restString = restString.Substring(restString.IndexOf("Content-Verification: ") + "Content-Verification: ".Length);

            mimeBase64 = restString.Substring(restString.IndexOf(";\n") + ";\n".Length);
            restString = restString.Substring(restString.IndexOf(";\n") + ";\n".Length);

            bool isMimeAttachment = false;
            if (!string.IsNullOrEmpty(mimeBase64) && mimeBase64.Length >= 4)
            {
                try
                {
                    if (restString.EndsWith($"\n{Hash}\0"))
                        mimeBase64 = restString.Substring(0, restString.LastIndexOf($"\n{Hash}\0"));
                    if (restString.EndsWith($"\n{Hash}"))
                        mimeBase64 = restString.Substring(0, restString.LastIndexOf($"\n{Hash}"));
                    if (restString.LastIndexOf("\n") >= (restString.Length - 11))
                        mimeBase64 = restString.Substring(0, restString.LastIndexOf($"\n"));

                    if (mimeBase64.Length < (contentLen + 10) && mimeBase64.Length > (contentLen - 10))
                        isMimeAttachment = true;
                }
                catch (Exception exMime)
                {
                    Area23Log.LogOriginMsgEx("CFile", "GetByBase64Attachment", exMime);
                }
            }
            return isMimeAttachment;

        }

        #endregion static members SaveCqrFile LoadCqrFile GetByBase64Attachment


        #region static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg

        /// <summary>
        /// ToJsonEncrypt
        /// </summary>
        /// <param name="serverKey">server key to encrypt</param>
        /// <param name="ccntct"><see cref="CContact"/> to encrypt and serialize</param>
        /// <returns>a serialized <see cref="string" /> of encrypted <see cref="CContact"/></returns>
        /// <exception cref="CqrException"></exception>
        public static string ToJsonEncrypt(string serverKey, ref CFile cfile, 
            EncodingType encoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            if (string.IsNullOrEmpty(serverKey) || cfile == null)
                throw new CqrException($"static string ToJsonEncrypt(string serverKey, ref CFile cfile) failed: NULL reference!");

            if (!EncryptSrvMsg(serverKey, ref cfile, encoder, zipType))
                throw new CqrException($"static string ToJsonEncrypt(string serverKey, ref CFile cfile) failed.");

            string serializedJson = cfile.ToJson();
            return serializedJson;
        }

        public static bool EncryptSrvMsg(string serverKey, ref CFile cfile,
            EncodingType encoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            string pipeString = "", keyHash = "", encrypted = "";
            try
            {
                keyHash = EnDeCodeHelper.KeyToHex(serverKey);
                pipeString = (new SymmCipherPipe(serverKey, keyHash)).PipeString;
                cfile.Hash = pipeString;
                cfile.Md5Hash = MD5Sum.HashString(String.Concat(serverKey, keyHash, pipeString, cfile.FileName), "");
                cfile.Sha256Hash = Sha256Sum.Hash(cfile.Data, "");

                encrypted = SymmCipherPipe.EncrpytBytesToString(cfile.Data, serverKey, out pipeString, encoder, zipType);                
                cfile.Data = new byte[0];
                cfile.Message = encrypted;
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }

            return true;
        }

        /// <summary>
        /// FromJsonDecrypt
        /// </summary>
        /// <param name="serverKey">server key to decrypt</param>
        /// <param name="serialized">serialized string of <see cref="CFile"/></param>
        /// <returns>deserialized and decrypted <see cref="CFile"/></returns>
        /// <exception cref="CqrException">thrown, 
        /// when serialized string to decrypt and deserialize is either null or empty 
        /// or <see cref="CFile"/> can't be decrypted and deserialized.
        /// </exception>
        public static CFile FromJsonDecrypt(
            string serverKey, 
            string serialized,
            EncodingType decoder = EncodingType.Base64, 
            Zfx.ZipType zipType = Zfx.ZipType.None
        )
        {
            if (string.IsNullOrEmpty(serialized))
                throw new CqrException("static CFile FromJsonDecrypt(string serverKey, string serialized): serialized is null or empty.");

            CFile deserializedFile = Newtonsoft.Json.JsonConvert.DeserializeObject<CFile>(serialized); 
            CFile decryptedFile = DecryptSrvMsg(serverKey, ref deserializedFile, decoder, zipType);
            if (decryptedFile == null)
            {
                throw new CqrException($"static CFile FromJsonDecrypt(string serverKey, string serialized) failed.");
            }

            decryptedFile.Base64Type = deserializedFile.Base64Type;
            decryptedFile.Sha256Hash = deserializedFile.Base64Type;                

            return decryptedFile;
        }

        public static CFile DecryptSrvMsg(
            string serverKey, 
            ref CFile cfile,
            EncodingType decoder = EncodingType.Base64, 
            Zfx.ZipType zipType = Zfx.ZipType.None
        )
        {
            string decrypted = "", pipeString = "", keyHash = EnDeCodeHelper.KeyToHex(serverKey);
            try
            {
                pipeString = (new SymmCipherPipe(serverKey, keyHash)).PipeString;

                byte[] fileBytes = SymmCipherPipe.DecrpytStringToBytes(cfile.Message, serverKey, out pipeString, decoder, zipType);
                
                string md5Hash = MD5Sum.HashString(String.Concat(serverKey, keyHash, pipeString, cfile.FileName), "");
                if (!cfile.Hash.Equals(pipeString))
                {
                    throw new CqrException($"CFile.Hash={cfile.Hash} doesn't match PipeString={pipeString}");
                }                    
                if (!md5Hash.Equals(cfile.Md5Hash))
                {
                    string md5ErrMsg = $"cfile.Md5Hash={cfile.Md5Hash} doesn't match md5Hash={md5Hash}.";
                    Area23Log.LogOriginMsg("CFile", md5ErrMsg);
                    // throw new CqrException(md5ErrMsg);
                }
                string sha256Hash = Sha256Sum.Hash(fileBytes, "");
                if (!sha256Hash.Equals(cfile.Sha256Hash))
                {
                    string sha256ErrMsg = $"cfile.Sha256Hash={cfile.Sha256Hash} doesn't match sha256Hash={sha256Hash}.";
                    Area23Log.LogOriginMsg("CFile", sha256ErrMsg);
                    // throw new CqrException(sha256ErrMsg);
                }

                cfile.Data = fileBytes;
                cfile.Message = "";

            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }

            return cfile;
        }

        #endregion static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg

        public new static CFile? CloneCopy(CFile? source, CFile? destination)
        {
            if (source == null)
                return null;
            if (destination == null)
                destination = new CFile(source);

            destination.Message = source.Message;
            destination.Hash = source.Hash;
            destination.MsgType = source.MsgType;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;

            destination.FileName = source.FileName;
            destination.Base64Type = source.Base64Type;
            destination.Data = source.Data;
            destination.Sha256Hash = source.Sha256Hash;
            destination.EnCodingType = source.EnCodingType;
            destination.Base64Type = source.Base64Type;

            return destination;
        }

        #endregion static members 

    }

}
