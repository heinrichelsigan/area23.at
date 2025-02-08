using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Crypt.CqrJd
{


    /// <summary>
    /// Represtents a MimeAttachment
    /// </summary>
    public class MimeAttachment
    {
        internal const string MIME_BASE64_FINISH = "\n\r\n";
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string Base64Mime { get; set; }
        public int ContentLength { get; set; }
        public string Verification { get; set; }

        public string Md5Hash { get; set; }
        public string Sha256Hash { get; set; }

        public string MimeMsg { get; set; }

        public MimeAttachment()
        {
            FileName = string.Empty;
            MimeType = string.Empty;
            Base64Mime = string.Empty;
            ContentLength = 0;
            Md5Hash = string.Empty;
            Sha256Hash = string.Empty;
            Verification = string.Empty;
            MimeMsg = string.Empty;
        }

        public MimeAttachment(string fileName, string mimeType, string base64Mime, string verification)
        {
            FileName = fileName;
            MimeType = mimeType;
            Base64Mime = base64Mime;
            ContentLength = base64Mime.Length;
            Verification = verification;
            MimeMsg = GetMimeMessage();
        }

        public MimeAttachment(string fileName, string mimeType, string base64Mime, string verification, string sMd5 = "", string sSha256 = "")
        {
            FileName = fileName;
            MimeType = mimeType;
            Base64Mime = base64Mime;
            ContentLength = base64Mime.Length;
            Verification = verification;
            Md5Hash = sMd5;
            Sha256Hash = sSha256;
            MimeMsg = GetMimeMessage();
        }


        public MimeAttachment(string plainText)
        {
            MimeAttachment mimeAttachment = MimeAttachment.GetBase64Attachment(plainText);
            MimeType = mimeAttachment.MimeType;
            FileName = mimeAttachment.FileName;
            ContentLength = mimeAttachment.ContentLength;
            Verification = mimeAttachment.Verification;
            Md5Hash = mimeAttachment.Md5Hash;
            Sha256Hash = mimeAttachment.Sha256Hash;
            Base64Mime = mimeAttachment.Base64Mime;
            MimeMsg = mimeAttachment.MimeMsg;
        }

        public string GetMimeMessage()
        {
            string mimeMsg = $"Content-Type: {MimeType}; name=\"{FileName}\";\n";
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

        public MimeAttachment GetMimeAttachment(string plainAttachment)
        {
            MimeAttachment mimeAttachment = MimeAttachment.GetBase64Attachment(plainAttachment);

            MimeType = mimeAttachment.MimeType;
            FileName = mimeAttachment.FileName;
            ContentLength = mimeAttachment.ContentLength;
            Verification = mimeAttachment.Verification;
            Md5Hash = mimeAttachment.Md5Hash;
            Sha256Hash = mimeAttachment.Sha256Hash;
            Base64Mime = mimeAttachment.Base64Mime;
            MimeMsg = mimeAttachment.MimeMsg;

            return (MimeAttachment)this;

        }

        public string GetFileNameContentLength()
        {
            string fileCLen = FileName + "[" + ContentLength + "]";
            return fileCLen;
        }

        public static MimeAttachment GetBase64Attachment(string plainAttachment)
        {
            string restString = plainAttachment;

            string mimeType = restString.Substring(restString.IndexOf("Content-Type: ") + "Content-Type: ".Length);
            // restString = mimeType.Substring(mimeType.IndexOf("; name=\"") + "; name=\"".Length);
            mimeType = mimeType.Substring(0, mimeType.IndexOf(";"));

            string fileName = restString.Substring(restString.IndexOf("; name=\"") + "; name=\"".Length);
            fileName = fileName.Substring(0, fileName.IndexOf("\";"));

            string contentLengthString = restString.Substring(restString.IndexOf("Content-Length: ") + "Content-Length: ".Length);
            contentLengthString = contentLengthString.Substring(0, contentLengthString.IndexOf(";\n"));
            string contentLenString = string.Empty;
            foreach (char ch in contentLengthString.ToCharArray())
            {
                if (Char.IsDigit(ch) || Char.IsNumber(ch) || ch == '.')
                    contentLenString += ch.ToString();
            }
            int contentLen = Int32.Parse(contentLenString);

            restString = restString.Substring(restString.IndexOf("Content-Verification: ") + "Content-Verification: ".Length);
            string verification = restString.Substring(0, restString.IndexOf(";"));

            string md5 = restString.Substring(restString.IndexOf("md5=\"") + "md5 =\"".Length);
            md5 = md5.Substring(0, md5.IndexOf("\";"));

            string sha256 = restString.Substring(restString.IndexOf("sha256=\"") + "sha256=\"".Length);
            sha256 = sha256.Substring(0, sha256.IndexOf("\";"));


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

    }

}
