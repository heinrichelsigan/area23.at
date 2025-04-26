using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Cqr.Msg;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.CryptoPro;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Cqr.Msg
{

    [Serializable]
    public class CFile : CContent, IMsgAble
    {

        #region properties 

        public string FileName { get; set; }

        public string Base64Type { get; set; }

        public byte[] Data { get; set; }

        public string Sha256Hash { get; protected internal set; }

        public long FileByteLen { get; protected internal set; }

        public EncodingType EnCodingType { get; internal set; }


        #endregion properties 

        #region ctors

        public CFile() : base()
        {
            FileName = string.Empty;
            Base64Type = string.Empty;
            Md5Hash = string.Empty;
            Sha256Hash = string.Empty;
            Data = new byte[0];
            MsgType = CType.None;
        }

        public CFile(string fileName, byte[] data, string hash = "")
        {
            FileName = fileName;
            Data = data;
            Base64Type = MimeType.GetMimeType(Data, FileName);
            _hash = hash;
            MsgType = CType.Json;
            Md5Hash = "";
            Sha256Hash = Sha256Sum.Hash(Data, "");
            FileByteLen = Data.LongLength;
            EnCodingType = EncodingType.Base64;
        }

        public CFile(string fileName, string mimeType, byte[] data, string hash) : this(fileName, data, hash)
        {
            Base64Type = mimeType;
        }

        public CFile(string fileName, string base64, string hash = "")
        {
            FileName = fileName;
            Data = Convert.FromBase64String(base64);
            Base64Type = MimeType.GetMimeType(Data, FileName);
            _hash = hash;
            Md5Hash = "";
            FileByteLen = Data.LongLength;
            Sha256Hash = Sha256Sum.Hash(Data, "");
            MsgType = CType.Json;
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

        public CFile(string fileName, string mimeType, byte[] data, string hash, string sMd5 = "", string sSha256 = "", CType msgType = CType.Json) :
                this(fileName, mimeType, data, hash, sMd5, sSha256)
        {
            MsgType = msgType;
        }

        public CFile(string fileName, string mimeType, byte[] data, string hash, string sMd5 = "", string sSha256 = "", CType msgType = CType.Json, EncodingType enCodeType = EncodingType.Base64) :
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
            FileByteLen = Data.LongLength;
            Sha256Hash = Sha256Sum.Hash(Data, "");
            MsgType = CType.Json;
            EnCodingType = EncodingType.Base64;
            this._hash = hash;
        }


        public CFile(string filePath, string hash = "", CType msgArt = CType.Json)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"Didn't find a file at filePath = {filePath}");

            FileName = Path.GetFileName(filePath);
            Data = System.IO.File.ReadAllBytes(filePath);
            Base64Type = MimeType.GetMimeType(Data, FileName);
            Md5Hash = "";
            Sha256Hash = Sha256Sum.Hash(Data, "");
            MsgType = msgArt;
            EnCodingType = EncodingType.Base64;
            this._hash = hash;

        }


        /// <summary>
        /// Constructor CqrFile from an json, xml or raw serialized plaintext
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="msgArt"></param>
        public CFile(string plainText, CType msgArt = CType.Json)
        {
            CFile cf = GetCFile(plainText, msgArt);
            CCopy(this, cf);
        }


        /// <summary>
        /// CFile constructor with antoher <see cref="CFile"/> to clone
        /// </summary>
        /// <param name="cFile"><see cref="CFile"/> to clone</param>
        public CFile(CFile cFile) : this()
        {
            if (cFile != null)
                CCopy(this, cFile);
        }

        #endregion ctors


        public new CFile CCopy(CFile leftDest, CFile rightSrc)
        {
            if (rightSrc == null)
                return null;
            if (leftDest == null)
                leftDest = new CFile(rightSrc);

            leftDest._message = rightSrc._message;
            leftDest._hash = rightSrc._hash;
            leftDest.MsgType = rightSrc.MsgType;
            leftDest.CBytes = rightSrc.CBytes;
            leftDest.Md5Hash = rightSrc.Md5Hash;

            leftDest.FileName = rightSrc.FileName;
            leftDest.Base64Type = rightSrc.Base64Type;
            leftDest.Data = rightSrc.Data;
            leftDest.Sha256Hash = rightSrc.Sha256Hash;
            leftDest.FileByteLen = rightSrc.FileByteLen;
            leftDest.EnCodingType = rightSrc.EnCodingType;
            leftDest.Base64Type = rightSrc.Base64Type;
            leftDest.SerializedMsg = "";
            leftDest.SerializedMsg = leftDest.ToJson();

            return leftDest;
        }


        #region EnDeCrypt+DeSerialize


        public override byte[] EncryptToJsonToBytes(string serverKey)
        {
            string serialized = EncryptToJson(serverKey);
            return Encoding.UTF8.GetBytes(serialized);
        }

        public override string EncryptToJson(string serverKey)
        {
            CFile cFile = new CFile(this);
            string serializedJson = ToJsonEncrypt(serverKey, ref cFile);
            if (string.IsNullOrEmpty(serializedJson))
                throw new CqrException($"override string EncryptToJson(string serverKey) failed");

            return serializedJson;
        }


        public new CFile DecryptFromJsonFromBytes(string serverKey, byte[] serializedBytes)
        {
            string serialized = Encoding.UTF8.GetString(serializedBytes);
            CFile cFile = DecryptFromJson(serverKey, serialized);
            return cFile;
        }


        public new CFile DecryptFromJson(string serverKey, string serialized = "")
        {
            CFile cfile = FromJsonDecrypt(serverKey, serialized);
            if (cfile == null)
                throw new CqrException($"override File DecryptFromJson(string serverKey, string serialized) failed");

            return CCopy(this, cfile);
        }


        #endregion EnDeCrypt+DeSerialize


        #region members


        public virtual string ToBase64() => Convert.ToBase64String(Data);

        /// <summary>
        /// Serialize <see cref="CFile"/> to Json Stting
        /// </summary>
        /// <returns>json serialized string</returns>
        public override string ToJson()
        {
            this.SerializedMsg = "";
            string jsonText = JsonConvert.SerializeObject(this);
            this.SerializedMsg = jsonText;
            return jsonText;
        }

        /// <summary>
        /// Generic method to convert back from json string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonText"></param>
        /// <returns></returns>
        public override T FromJson<T>(string jsonText)
        {
            T t = JsonConvert.DeserializeObject<T>(jsonText);
            if (t != null)
            {
                if (t is CFile cf)
                {
                    CCopy(this, cf);
                }

            }
            return t;
        }

        public override string ToXml()
        {
            SerializedMsg = "";
            SerializedMsg = Utils.SerializeToXml<CContent>(this);
            return SerializedMsg;
        }


        public override T FromXml<T>(string xmlText)
        {
            T cqrT = default(T);
            cqrT = base.FromXml<T>(xmlText);
            if (cqrT is CFile cf)
            {
                CCopy(this, cf);
            }

            return cqrT;

        }


        public CFile GetCFile(string encodedSerilizedOrRawText, CType msgArt = CType.Json)
        {
            if (msgArt == CType.None || msgArt == CType.Raw)
            {
                throw new NotImplementedException("CqrFile GetCqrFile(string encodedSerilizedOrRawText, MsgEnum msgArt = MsgEnum.Json)" +
                    " is not implemented for MsgEnum.None and MsgEnum.RawWithHashAtEnd");
            }
            if (msgArt == CType.Mime)
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
                    _hash = hash;
                }
            }
            else if (msgArt == CType.Json)
            {
                this.FromJson<CFile>(encodedSerilizedOrRawText);
            }
            else if (msgArt == CType.Xml)
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

        #region static members SaveCqrFile LoadCqrFile GetByBase64Attachment

        public static void SaveCqrFile(CFile file, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory {directoryPath} could not be found.");

            string saveFileName = Path.Combine(directoryPath, file.FileName);
            System.IO.File.WriteAllBytes(saveFileName, file.Data);

            return;
        }

        public static CFile LoadCqrFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"File {filePath} could not be found.");

            string fileName = Path.GetFileName(filePath);
            byte[] data = System.IO.File.ReadAllBytes(fileName);
            string mimeType = MimeType.GetMimeType(data, fileName);
            CFile cfile = new CFile(fileName, mimeType, data, "");
            return cfile;
        }


        internal static bool GetByBase64Attachment(string plainAttachment,
            out string cqrFileName, out string base64Type,
            out string md5Hash, out string sha256Hash,
            out string mimeBase64, out string _hash)
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

            _hash = restString.GetSubStringByPattern("Content-Verification: ", true, "", ";", false);
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

        #endregion static members SaveCqrFile LoadCqrFile GetByBase64Attachment


        #region static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg

        /// <summary>
        /// ToJsonEncrypt
        /// </summary>
        /// <param name="serverKey">server key to encrypt</param>
        /// <param name="ccntct"><see cref="CContact"/> to encrypt and serialize</param>
        /// <returns>a serialized <see cref="string" /> of encrypted <see cref="CContact"/></returns>
        /// <exception cref="CqrException"></exception>
        public static string ToJsonEncrypt(string serverKey, ref CFile cfile)
        {
            if (string.IsNullOrEmpty(serverKey) || cfile == null)
                throw new CqrException($"static string ToJsonEncrypt(string serverKey, ref CFile cfile) failed: NULL reference!");

            if (!EncryptSrvMsg(serverKey, ref cfile))
                throw new CqrException($"static string ToJsonEncrypt(string serverKey, ref CFile cfile) failed.");

            string serializedJson = cfile.ToJson();
            return serializedJson;
        }

        public static bool EncryptSrvMsg(string serverKey, ref CFile cfile)
        {
            try
            {
                string hash = EnDeCodeHelper.KeyToHex(serverKey);
                SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, hash);
                cfile._hash = hash;
                cfile.Md5Hash = MD5Sum.HashString(String.Concat(serverKey, hash, symmPipe.PipeString, cfile._message), "");
                cfile.Sha256Hash = Sha256Sum.Hash(cfile.Data, "");

                byte[] msgBytes = cfile.Data;
                byte[] cqrbytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, serverKey, hash) : msgBytes;

                cfile.CBytes = cqrbytes;
                cfile.Data = new byte[0];
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
        public static CFile FromJsonDecrypt(string serverKey, string serialized)
        {
            if (string.IsNullOrEmpty(serialized))
                throw new CqrException("static CFile FromJsonDecrypt(string serverKey, string serialized): serialized is null or empty.");

            CFile cfile = new CFile(serialized, CType.Json);
            cfile = DecryptSrvMsg(serverKey, ref cfile);
            if (cfile == null)
                throw new CqrException($"static CFile FromJsonDecrypt(string serverKey, string serialized) failed.");

            return cfile;
        }

        public static CFile DecryptSrvMsg(string serverKey, ref CFile cfile)
        {
            try
            {
                string hash = EnDeCodeHelper.KeyToHex(serverKey);
                SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, hash);

                byte[] cipherBytes = cfile.CBytes;
                byte[] unroundedMerryBytes = LibPaths.CqrEncrypt ? symmPipe.DecrpytRoundGoMerry(cipherBytes, serverKey, hash) : cipherBytes;

                if (!cfile._hash.Equals(symmPipe.PipeString))
                    throw new CqrException($"Hash: {cfile._hash} doesn't match symmPipe.PipeString: {symmPipe.PipeString}");

                string md5Hash = MD5Sum.HashString(String.Concat(serverKey, cfile._hash, symmPipe.PipeString, cfile._message), "");
                if (!md5Hash.Equals(cfile.Md5Hash))
                {
                    string md5ErrMsg = $"md5Hash: {md5Hash} doesn't match property Md5Hash: {cfile.Md5Hash}";
                    Area23Log.LogStatic(md5ErrMsg);
                    // throw new CqrException(md5ErrMsg);
                }
                string sha256Hash = Sha256Sum.Hash(unroundedMerryBytes, "");
                if (!sha256Hash.Equals(cfile.Sha256Hash))
                {
                    string sha256ErrMsg = $"Sha256 from decrypted = {sha256Hash}, while this.Sha256Hash = {cfile.Sha256Hash}.";
                    Area23Log.LogStatic(sha256ErrMsg);
                    // throw new CqrException(sha256ErrMsg);
                }

                cfile.Data = unroundedMerryBytes;
                cfile.CBytes = new byte[0];

            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }

            return cfile;
        }

        #endregion static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg


        #endregion static members 

    }

}
