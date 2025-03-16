using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Static;
using Newtonsoft.Json;

namespace Area23.At.Framework.Core.CqrXs.Msg
{

    /// <summary>
    /// Represtents a MimeAttachment
    /// </summary>
    [Serializable]
    public class CqrFile : CqrMsg, ICqrMessagable
    {       

        #region properties 

        public string CqrFileName { get; set; }

        public string Base64Type { get; set; }
        
        public byte[] Data { get; set; }
        

        public string Sha256Hash { get; set; }

        public EncodingType EnCodingType { get; internal set; }

        
        #endregion properties 

        #region ctors

        public CqrFile() : base()
        {
            CqrFileName = string.Empty;
            Base64Type = string.Empty;
            Md5Hash = string.Empty;
            Sha256Hash = string.Empty;
            Data = new byte[0];
        }

        public CqrFile(string fileName, string mimeType, byte[] data, string hash)
        {
            CqrFileName = fileName;
            Base64Type = mimeType;
            Data = data;
            _hash = hash;
            MsgType = MsgEnum.Json;
            Md5Hash = string.Empty;
            Sha256Hash = string.Empty;
            EnCodingType = EncodingType.Base64;
        }

        public CqrFile(string fileName, string mimeType, string base64, string hash)
        {
            CqrFileName = fileName;
            Base64Type = mimeType;
            Data = Convert.FromBase64String(base64);
            _hash = hash;
            Md5Hash = string.Empty;
            Sha256Hash = string.Empty;
            MsgType = MsgEnum.Json;
            EnCodingType = EncodingType.Base64;
        }


        public CqrFile(string fileName, string mimeType, byte[] data, string hash, string sMd5 = "", string sSha256 = "")
             : this(fileName, mimeType, data, hash)
        {
            Md5Hash = sMd5;
            Sha256Hash = sSha256;
        }

        public CqrFile(string fileName, string mimeType, byte[] data, string hash, string sMd5 = "", string sSha256 = "",
            MsgEnum msgType = MsgEnum.Json) :
                this(fileName, mimeType, data, hash, sMd5, sSha256) 
        {
            this.MsgType = msgType;
        }

        public CqrFile(string fileName, string mimeType, byte[] data, string hash, string sMd5 = "", string sSha256 = "", 
            MsgEnum msgType = MsgEnum.Json, EncodingType enCodeType = EncodingType.Base64) :
                this(fileName, mimeType, data, hash, sMd5, sSha256, msgType)
        {
            this.EnCodingType = enCodeType;
        }


        /// <summary>
        /// Constructor CqrFile from an json, xml or raw serialized plaintext
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="msgArt"></param>
        public CqrFile(string plainText, MsgEnum msgArt = MsgEnum.Json)
        {
            CqrFile? cf = GetCqrFile(plainText, msgArt);
            CqrFileName = cf.CqrFileName;
            Base64Type = cf.Base64Type;
            Data = cf.Data;
            Md5Hash = cf.Md5Hash;                
            Sha256Hash = cf.Sha256Hash;            
            _hash = cf._hash;
            _message = cf._message;
            RawMessage = cf.RawMessage;            
            EnCodingType = cf.EnCodingType;
            MsgType = cf.MsgType;
        }
   
        #endregion ctors

        #region members

        public virtual string ToBase64() => Convert.ToBase64String(Data);       

        /// <summary>
        /// Serialize <see cref="CqrFile"/> to Json Stting
        /// </summary>
        /// <returns></returns>
        public override string ToJson() => JsonConvert.SerializeObject(this);
        

        /// <summary>
        /// Generic method to convert back from json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public override T? FromJson<T>(string jsonText) where T : default
        {
            T? t = JsonConvert.DeserializeObject<T>(jsonText);
            if (t != null)
            {                
                if (t is CqrMsg mc)
                {
                    this._hash = mc.Hash;
                    this._message = mc.Message;
                    this.RawMessage = mc.RawMessage;
                    this.MsgType = mc.MsgType;
                }
                if (t is CqrFile cf)
                {
                    CqrFileName = cf.CqrFileName;
                    Base64Type = cf.Base64Type;
                    Data = cf.Data;
                    Md5Hash = cf.Md5Hash;
                    Sha256Hash = cf.Sha256Hash;
                    EnCodingType = cf.EnCodingType;
                    MsgType = MsgEnum.Json;
                    _message = cf.Message;
                    RawMessage = cf.RawMessage;
                    _hash = cf._hash ?? string.Empty;
                }

            }
            return t;
        }

        public override string ToXml() => this.ToXml();
        

        public override T? FromXml<T>(string xmlText) where T : default
        {
            T? cqrT = default(T);
            cqrT = base.FromXml<T>(xmlText);
            if (cqrT is CqrFile cf)
            {
                this.Base64Type = cf.Base64Type;
                this.Data = cf.Data;
                this.Base64Type = cf.Base64Type;
                this.EnCodingType = cf.EnCodingType;
                this.Data = cf.Data;
                this._hash = cf._hash ?? string.Empty;
                this.Md5Hash = cf.Md5Hash;
                this._message = cf._message;
                this.MsgType = cf.MsgType;
                this.RawMessage = cf.RawMessage;
                this.Sha256Hash = cf.Sha256Hash;
            }

            return cqrT;

        }


        public CqrFile GetCqrFile(string encodedSerilizedOrRawText, MsgEnum msgArt = MsgEnum.Json)
        {
            if (msgArt == MsgEnum.None || msgArt == MsgEnum.RawWithHashAtEnd)
            {
                throw new NotImplementedException("CqrFile GetCqrFile(string encodedSerilizedOrRawText, MsgEnum msgArt = MsgEnum.Json)" +
                    " is not implemented for MsgEnum.None and MsgEnum.RawWithHashAtEnd");
            }
            if (msgArt == MsgEnum.MimeAttachment)
            {               
                Data = new byte[0];

                bool mimeGot = GetByBase64Attachment(encodedSerilizedOrRawText,
                    out string cqrFileName, out string base64Type,
                    out string md5Hash, out string sha256Hash,
                    out string mimeBase64, out string hash);
                if (mimeGot && !string.IsNullOrEmpty(mimeBase64))
                {
                    CqrFileName = cqrFileName;
                    Base64Type = base64Type;
                    Md5Hash = md5Hash;
                    Sha256Hash = sha256Hash;
                    Data = Convert.FromBase64String(mimeBase64);
                    _hash = hash;
                }
            }
            else if (msgArt == MsgEnum.Json)
            {
                this.FromJson<CqrFile>(encodedSerilizedOrRawText);
            }
            else if (msgArt == MsgEnum.Xml)
            {                
                this.FromXml<CqrFile>(encodedSerilizedOrRawText);
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
            string html = $"<html>\n\t<head>\n\t\t<title>{CqrFileName} {Base64Type}</title>\n\t</head>";
            html += $"\n\t<body>\n\t\t";
            if (MimeType.IsMimeTypeImage(Base64Type))
                html += $"\n\t\t<img src=\"data:{Base64Type};base64,{base64Mime}\" alt=\"{Base64Type} {CqrFileName}\" />";
            if (MimeType.IsMimeTypeDocument(Base64Type))
            {
                html += $"\n\t\t<object data=\"data:{Base64Type};base64,{base64Mime}\" type=\"{Base64Type}\" width=\"640px\" height=\"480px\" >";
                html += $"\n\t\t\t<p>Unable to display {Base64Type} <b>{CqrFileName}</b></p>";
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
            string fileCLen = CqrFileName + " [" + Data.Length + "]";
            return fileCLen;
        }

        #endregion members

        #region static members



        public static void SaveCqrFile(CqrFile file, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory {directoryPath} could not be found.");

            string saveFileName = Path.Combine(directoryPath, file.CqrFileName);
            File.WriteAllBytes(saveFileName, file.Data);

            return;
        }


        public static CqrFile LoadCqrFile(string cqrFileName)
        {
            if (!File.Exists(cqrFileName))
                throw new FileNotFoundException($"File {cqrFileName} could not be found.");

            string fileName = Path.GetFileName(cqrFileName);
            byte[] data = File.ReadAllBytes(cqrFileName);
            string mimeType = MimeType.GetMimeType(data, fileName);
            CqrFile cfile = new CqrFile(fileName, mimeType, data, "");
            return cfile;
        }



        internal static bool GetByBase64Attachment(string plainAttachment,
            out string cqrFileName, out string base64Type,
            out string md5Hash, out string sha256Hash,
            out string mimeBase64, out string _hash)
        {
            string restString = plainAttachment;

            base64Type = restString.GetSubStringByPattern("Content-Type: ", true, "", ";", false, StringComparison.CurrentCulture);
            cqrFileName = restString.GetSubStringByPattern("; name=\"", true, "", "\";", false, StringComparison.CurrentCulture);
            string contentLengthString = restString.GetSubStringByPattern("Content-Length: ", true, "", ";\n", false, StringComparison.CurrentCulture);
            string contentLenString = string.Empty;
            foreach (char ch in contentLengthString.ToCharArray())
                if (Char.IsDigit(ch) || Char.IsNumber(ch) || ch == '.')
                    contentLenString += ch.ToString();
            int contentLen = Int32.Parse(contentLenString);

            _hash = restString.GetSubStringByPattern("Content-Verification: ", true, "", ";", false, StringComparison.CurrentCulture);
            md5Hash = restString.GetSubStringByPattern("md5=\"", true, "", "\";", false, StringComparison.CurrentCultureIgnoreCase);
            sha256Hash = restString.GetSubStringByPattern("sha256=\"", true, "", "\";", false, StringComparison.CurrentCultureIgnoreCase);

            restString = restString.Substring(restString.IndexOf("Content-Verification: ") + "Content-Verification: ".Length);

            mimeBase64 = restString.Substring(restString.IndexOf(";\n") + ";\n".Length);
            restString = restString.Substring(restString.IndexOf(";\n") + ";\n".Length);

            bool isMimeAttachment = false;
            if (!string.IsNullOrEmpty(mimeBase64) && mimeBase64.Length >= 4)
            {
                try
                {
                    if (restString.EndsWith($"\n{_hash}\0"))
                        mimeBase64 = restString.Substring(0, restString.LastIndexOf($"\n{_hash}\0"));
                    if (restString.EndsWith($"\n{_hash}"))
                        mimeBase64 = restString.Substring(0, restString.LastIndexOf($"\n{_hash}"));
                    if (restString.LastIndexOf("\n") >= (restString.Length - 11))
                        mimeBase64 = restString.Substring(0, restString.LastIndexOf($"\n"));

                    if (mimeBase64.Length < (contentLen + 10) && mimeBase64.Length > (contentLen - 10))
                        isMimeAttachment = true;
                }
                catch (Exception exMime)
                {
                    SLog.Log(exMime);                    
                }
            }
            return isMimeAttachment;
            
        }
        #endregion static members

    }


}
