using Area23.At.Framework.Core.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace Area23.At.Framework.Core.CqrXs.CqrMsg
{

    /// <summary>
    /// Represtents a MimeAttachment
    /// </summary>
    [JsonObject]
    [Serializable]
    public class MimeAttachment : MsgContent
    {

        internal const string MIME_BASE64_FINISH = "\n\r\n";

        #region properties 

        public string FileName { get; set; }
        public string Base64Type { get; set; }
        public string Base64Mime { get; set; }
        public int ContentLength { get; set; }
        public string Verification { get; set; }

        public string Md5Hash { get; set; }
        public string Sha256Hash { get; set; }

        public string MimeMsg { get => this.GetMimeMessage(); }

        #endregion properties 

        #region ctors

        public MimeAttachment() : base()
        {
            FileName = string.Empty;
            Base64Type = string.Empty;
            Base64Mime = string.Empty;
            ContentLength = 0;
            Md5Hash = string.Empty;
            Sha256Hash = string.Empty;
            Verification = string.Empty;
        }

        public MimeAttachment(string fileName, string mimeType, string base64Mime, string verification)
        {
            FileName = fileName;
            Base64Type = mimeType;
            Base64Mime = base64Mime;
            ContentLength = base64Mime.Length;
            Verification = verification;
            _hash = verification;
            _isMime = true;
        }

        public MimeAttachment(string fileName, string mimeType, string base64Mime, string verification, string sMd5 = "", string sSha256 = "")
        {
            FileName = fileName;
            Base64Type = mimeType;
            Base64Mime = base64Mime;
            ContentLength = base64Mime.Length;
            Verification = verification;
            Md5Hash = sMd5;
            Sha256Hash = sSha256;
            _hash = verification;
            _isMime = true;
        }

        public MimeAttachment(string plainText, MsgEnum msgArt = MsgEnum.None)
        {
            if (msgArt == MsgEnum.None || msgArt == MsgEnum.RawWithHashAtEnd)
            {
                MimeAttachment mimeAttachment = MimeAttachment.GetBase64Attachment(plainText);
                Base64Type = mimeAttachment.Base64Type;
                FileName = mimeAttachment.FileName;
                ContentLength = mimeAttachment.ContentLength;
                Verification = mimeAttachment.Verification;
                Md5Hash = mimeAttachment.Md5Hash;
                Sha256Hash = mimeAttachment.Sha256Hash;
                Base64Mime = mimeAttachment.Base64Mime;
                _hash = Verification;
                _isMime = true;
            }
            else if (msgArt == MsgEnum.JsonSerialized || msgArt == MsgEnum.JsonDeserialized)
            {
                this.FromJson<MimeAttachment>(plainText);
            }
        }

        #endregion ctors

        #region members


        public override string ToJson()
        {
            string jsonText = JsonConvert.SerializeObject(this);
            return jsonText;
        }

        public override T? FromJson<T>(string jsonText) where T : default
        {
            T? t = JsonConvert.DeserializeObject<T>(jsonText);
            if (t != null)
            {
                if (t is MsgContent mc)
                {
                    this._hash = mc.Hash;
                    this._message = mc.Message;
                    this._rawMessage = mc.RawMessage;
                    _isMime = false;
                }
                if (t is MimeAttachment ma)
                {
                    this.ContentLength = ma.ContentLength;
                    this.Base64Mime = ma.Base64Mime;
                    this.Base64Type = ma.Base64Type;
                    this.FileName = ma.FileName;
                    this.Md5Hash = ma.Md5Hash;
                    this.Sha256Hash = ma.Sha256Hash;
                    this.Verification = ma.Verification;
                    _isMime = true;
                }
            }
            return t;
        }


        public string GetMimeMessage()
        {
            string mimeMsg = $"Content-Type: {Base64Type}; name=\"{FileName}\";\n";
            mimeMsg += $"Content-Transfer-Encoding: base64;\n";
            mimeMsg += $"Content-Length: {Base64Mime.Length};\n";
            mimeMsg += $"Content-Verification: {Verification};";
            if (!string.IsNullOrEmpty(Md5Hash))
                mimeMsg += $" md5=\"{Md5Hash}\";";
            if (!string.IsNullOrEmpty(Sha256Hash))
                mimeMsg += $" sha256=\"{Sha256Hash}\";";
            mimeMsg += "\n";
            mimeMsg += Base64Mime + MIME_BASE64_FINISH;
            return mimeMsg;
        }

        public string GetWebPage()
        {
            string html = $"<html>\n\t<head>\n\t\t<title>{FileName} {Base64Type}</title>\n\t</head>";
            html += $"\n\t<body>\n\t\t";
            if (MimeType.IsMimeTypeImage(Base64Type))
                html += $"\n\t\t<img src=\"data:{Base64Type};base64,{Base64Mime}\" alt=\"{Base64Type} {FileName}\" />";
            if (MimeType.IsMimeTypeDocument(Base64Type))
            {
                html += $"\n\t\t<object data=\"data:{Base64Type};base64,{Base64Mime}\" type=\"{Base64Type}\" width=\"640px\" height=\"480px\" >";
                html += $"\n\t\t\t<p>Unable to display {Base64Type} <b>{FileName}</b></p>";
                html += $"\n\t\t</object>";
            }
            if (MimeType.IsMimeTypeAudio(Base64Type))
            {
                html += $"\n\t\t<audio controls>";
                html += $"\n\t\t\t<source src=\"data:{Base64Type};base64,{Base64Mime}\" type=\"{Base64Type}\">";
                html += $"\n\t\tYour browser does not support the audio element.";
                html += $"\n\t\t</audio>";
            }
            if (MimeType.IsMimeTypeVideo(Base64Type))
            {
                html += $"\n\t\t<video width=\"320\" height=\"240\" controls>";
                html += $"\n\t\t\t<source src=\"data:{Base64Type};base64,{Base64Mime}\" type=\"{Base64Type}\">";
                html += $"\n\t\tYour browser does not support the video tag.";
                html += $"\n\t\t</video>";
            }
            html += $"\n\t</body>\n\t\t";
            html += $"\n</html>\n";


            return html;
        }

        public MimeAttachment GetMimeAttachment(string plainAttachment, MsgEnum msgArt = MsgEnum.None)
        {
            if (msgArt == MsgEnum.None)
            {
                MimeAttachment mimeAttachment = MimeAttachment.GetBase64Attachment(plainAttachment);

                Base64Type = mimeAttachment.Base64Type;
                FileName = mimeAttachment.FileName;
                ContentLength = mimeAttachment.ContentLength;
                Verification = mimeAttachment.Verification;
                Md5Hash = mimeAttachment.Md5Hash;
                Sha256Hash = mimeAttachment.Sha256Hash;
                Base64Mime = mimeAttachment.Base64Mime;
                _isMime = true;
                _hash = mimeAttachment.Verification;
            }
            else if (msgArt == MsgEnum.JsonSerialized)
            {
                this.FromJson<MimeAttachment>(plainAttachment);
            }

            return (MimeAttachment)this;

        }

        public string GetFileNameContentLength()
        {
            string fileCLen = FileName + " [" + ContentLength + "]";
            return fileCLen;
        }


        public override MimeAttachment ToMimeAttachment()
        {
            if (!IsMimeAttachment())
                throw new InvalidCastException($"MsgContent Message={_message} isn't a mime attachment!");

            MimeAttachment mAttach = MimeAttachment.GetBase64Attachment(_message);
            this.Base64Mime = mAttach.Base64Mime;
            this.FileName = mAttach.FileName;
            this.ContentLength = mAttach.ContentLength;
            this.Verification = mAttach.Verification;
            this.Base64Type = mAttach.Base64Type;
            this.Md5Hash = mAttach.Md5Hash;
            this.Sha256Hash = mAttach.Sha256Hash;
            this._hash = mAttach._hash;
            this._message = mAttach._message;
            this._isMime = true;


            return mAttach;
        }


        #endregion members

        #region static members

        public static MimeAttachment GetBase64Attachment(string plainAttachment)
        {
            string restString = plainAttachment;

            string mimeType = restString.GetSubStringByPattern("Content-Type: ", true, "", ";", false, StringComparison.CurrentCulture);
            string fileName = restString.GetSubStringByPattern("; name=\"", true, "", "\";", false, StringComparison.CurrentCulture);
            string contentLengthString = restString.GetSubStringByPattern("Content-Length: ", true, "", ";\n", false, StringComparison.CurrentCulture);            
            string contentLenString = string.Empty;
            foreach (char ch in contentLengthString.ToCharArray())
                if (Char.IsDigit(ch) || Char.IsNumber(ch) || ch == '.')
                    contentLenString += ch.ToString();
            int contentLen = Int32.Parse(contentLenString);

            string verification = restString.GetSubStringByPattern("Content-Verification: ", true, "", ";", false, StringComparison.CurrentCulture);
            string md5 = restString.GetSubStringByPattern("md5=\"", true, "", "\";", false, StringComparison.CurrentCultureIgnoreCase);
            string sha256 = restString.GetSubStringByPattern("sha256=\"", true, "", "\";", false, StringComparison.CurrentCultureIgnoreCase);

            restString = restString.Substring(restString.IndexOf("Content-Verification: ") + "Content-Verification: ".Length);

            string mimeBase64 = restString.Substring(restString.IndexOf(";\n") + ";\n".Length);
            restString = restString.Substring(restString.IndexOf(";\n") + ";\n".Length);
            try
            {
                int len1 = mimeBase64.LastIndexOf("\r");
                if (len1 > 0)
                {
                    mimeBase64 = mimeBase64.Substring(0, len1);
                }
                else
                {
                    len1 = mimeBase64.Length;
                    int len2 = contentLen;
                    int len0 = Math.Min(len1, len2);
                    mimeBase64 = mimeBase64.Substring(0, len0);
                }
            }
            catch (Exception exMime)
            {
                Area23Log.LogStatic(exMime);
            }

            MimeAttachment mimeAttach = new MimeAttachment(fileName, mimeType, mimeBase64, verification, md5, sha256);
            return mimeAttach;
        }

        public static string GetMimeMessage(string fileName, string mimeType, string base64Mime, string verification, string md5 = "", string sha256 = "")
        {
            string mimeMsg = $"Content-Type: {mimeType}; name=\"{fileName}\";\n";
            mimeMsg += $"Content-Transfer-Encoding: base64;\n";
            mimeMsg += $"Content-Length: {base64Mime.Length};\n";
            mimeMsg += $"Content-Verification: {verification};";
            if (!string.IsNullOrEmpty(md5))
                mimeMsg += $" md5=\"{md5}\";";
            if (!string.IsNullOrEmpty(sha256))
                mimeMsg += $" sha256=\"{sha256}\";";
            mimeMsg += "\n";
            mimeMsg += base64Mime + MIME_BASE64_FINISH;
            return mimeMsg;
        }


        #endregion static members

    }


}
