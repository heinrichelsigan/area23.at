using Area23.At.Framework.Library.Crypt.Cipher;
using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Policy;

namespace Area23.At.Framework.Library.Cqr.Msg
{

    /// <summary>
    /// CImage is a image derived drom <see cref="CFile"/>
    /// </summary>
    [Serializable]
    public class CImage : CFile, IMsgAble
    {

        #region properties

        /// <summary>
        /// File Name with extension of Image wraps <see cref="CFile.FileName"/>
        /// </summary>
        [JsonIgnore]
        public string ImageFileName { get => base.FileName; set => FileName = value; }

        /// <summary>
        /// Mime Type of Image
        /// </summary>
        [JsonIgnore]
        public string ImageMimeType { get => base.Base64Type; set => Base64Type = value; }

        /// <summary>
        /// byte[] of Image Raw Data wraps <see cref="CFile.Data"/>
        /// </summary>
        [JsonIgnore]
        public byte[] ImageData { get => base.Data; set => Data = value; }

        /// <summary>
        /// Base64 mime encoded string of raw data
        /// </summary>
        [JsonIgnore]
        public string ImageBase64 { get => Convert.ToBase64String(Data, 0, Data.Length); }


        #endregion properties

        #region constructors

        /// <summary>
        /// Default empty constructor (needed for json serialize & deserialize)
        /// </summary>
        public CImage() : base()
        {
            ImageFileName = "";
            ImageData = new byte[0];
            ImageMimeType = "";
            Sha256Hash = "";
        }


        /// <summary>
        /// Ctor with filename and byte[] data
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public CImage(string fileName, byte[] data) : this()
        {
            ImageFileName = fileName;
            ImageData = data;
            ImageMimeType = MimeType.GetMimeType(ImageData, ImageFileName);
            Sha256Hash = Sha256Sum.Hash(ImageData, "");
        }

        /// <summary>
        /// Ctor with filename and mime encoded base64 string
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="base64Image"></param>
        public CImage(string fileName, string base64Image) : this()
        {
            ImageFileName = fileName;
            ImageData = Convert.FromBase64String(base64Image);
            ImageMimeType = MimeType.GetMimeType(ImageData, ImageFileName);
            Sha256Hash = Sha256Sum.Hash(ImageData, "");
        }

        /// <summary>
        /// Ctor with <see cref="Image"/> and fileName
        /// </summary>
        /// <param name="image"> <see cref="Image"/>  <see cref="Bitmap"/></param>
        /// <param name="fileName">fileName for the image,
        /// if fileName is null or empty
        /// then a name <see cref="Extensions.Area23DateTimeWithMillis(DateTime)"/></param> + "_image." + extension based mime type will be given
        public CImage(Image image, string fileName = "")
        {
            CImage cImage = FromDrawingImage(image, fileName);
            if (cImage != null)
            {
                CloneCopy(cImage, this);
            }
        }

        public CImage(CImage cImage) : this()
        {
            if (cImage != null)
            {
                CloneCopy(cImage, this);
            }
        }

        public CImage(string serializedImgage, SerType msgType = SerType.Json)
        {
            CImage cImage = (msgType == SerType.Xml) ? FromXml<CImage>(serializedImgage) : FromJson<CImage>(serializedImgage);
            if (cImage != null)
            {
                CloneCopy(cImage, this);
            }
        }

        #endregion constructors


        #region members

        #region EnDeCrypt+DeSerialize

        public override string EncryptToJson(string serverKey, EncodingType encoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None, KeyHash kHash = KeyHash.Hex)
        {
            CFile cfile = (CFile)this.ToCFile();
            string serializedJson = CImage.Encrypt2Json(serverKey, ref cfile, encoder, zipType, kHash);

            return serializedJson;
        }

        public new CImage DecryptFromJson(string serverKey, string serialized = "",
            EncodingType decoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None, KeyHash kHash = KeyHash.Hex)
        {
            if (string.IsNullOrEmpty(serialized))
                serialized = this.SerializedMsg;

            CFile cfile = CImage.Json2Decrypt(serverKey, serialized, decoder, zipType, kHash);
            if (cfile == null)
                throw new CqrException($"CImage DecryptFromJson(string serverKey, string serialized) failed.");

            return CloneCopy(cfile, this);
        }

        #endregion EnDeCrypt+DeSerialize

        public override CContent CCopy(CContent leftDest, CContent rightSrc)
        {
            if (leftDest is CImage && rightSrc is CImage)
                return CImage.CloneCopy(rightSrc, leftDest);

            return base.CCopy(leftDest, rightSrc);
        }

        /// <summary>
        /// ToXml serializes this <see cref="CImage"/> to serialized xml <see cref="string"/>
        /// </summary>
        /// <returns>xml serialized string</returns>
        public override string ToXml() => Utils.SerializeToXml<CImage>(this);

        /// <summary>
        /// ToDrawingBitmap converts this <see cref="CImage"/> to <see cref="System.Drawing.Bitmap"/>
        /// </summary>
        /// <returns>transformed see cref="System.Drawing.Bitmap"/></returns>
        public virtual Bitmap ToDrawingBitmap() => CImage.ToDrawingImage(this);

        #endregion members


        #region static members

        /// <summary>
        /// ToDrawingImage converts a <see cref="CImage"/> to a <see cref="System.Drawing.Image"/>
        /// </summary>
        /// <param name="cqrImage"><see cref="CImage"/> to convert</param>
        /// <returns>converted <see cref="System.Drawing.Image"/></returns>
        public static Bitmap ToDrawingImage(CImage cImage)
        {
            Bitmap bmpImage;
            if (cImage != null && cImage.ImageData != null && cImage.ImageData.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(cImage.Data))
                {
                    bmpImage = new Bitmap(ms, true);
                }

                return bmpImage;
            }

            throw new CqrException("ToDrawingBitmap() CImage.Data is null",
                            new NullReferenceException("CImage or CImage.Data is null"));
        }

        /// <summary>
        /// FromDrawingImage converts a <see cref="CImage"/> from a <see cref="System.Drawing.Image"/>
        /// </summary>
        /// <param name="image"><see cref="System.Drawing.Image"/></param>
        /// <param name="imgName"><see cref="string">string imgName</see></param>
        /// <returns><see cref="CImage">converted CqrImage</see></returns>
        public static CImage FromDrawingImage(Image image, string imgName = "")
        {
            if (image == null)
                return null;

            CImage cImage = null;
            // ImageFormat format = image.RawFormat;
            byte[] imageData;
            string fileName = string.IsNullOrEmpty(imgName) ? string.Empty : imgName;

            using (MemoryStream ms = new MemoryStream())
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName += DateTime.Now.Area23DateTimeWithMillis() + "_image";
                    if (image.RawFormat == ImageFormat.Tiff)
                    {
                        fileName += ".tif";
                        image.Save(ms, ImageFormat.Tiff);
                    }
                    else if (image.RawFormat == ImageFormat.Png)
                    {
                        fileName += ".png";
                        image.Save(ms, ImageFormat.Png);
                    }
                    else if (image.RawFormat == ImageFormat.Jpeg)
                    {
                        fileName += ".jpg";
                        image.Save(ms, ImageFormat.Jpeg);
                    }
                    else if (image.RawFormat == ImageFormat.Gif)
                    {
                        fileName += ".gif";
                        image.Save(ms, ImageFormat.Gif);
                    }
                    else if (image.RawFormat == ImageFormat.Bmp || image.RawFormat == ImageFormat.MemoryBmp)
                    {
                        fileName += ".bmp";
                        image.Save(ms, ImageFormat.Bmp);
                    }
                    else if (image.RawFormat == ImageFormat.Exif)
                    {
                        fileName += ".exif";
                        image.Save(ms, ImageFormat.Exif);
                    }
                    else if (image.RawFormat == ImageFormat.Wmf)
                    {
                        fileName += ".wmf";
                        image.Save(ms, ImageFormat.Wmf);
                    }
                    else if (image.RawFormat == ImageFormat.Emf)
                    {
                        fileName += ".emf";
                        image.Save(ms, ImageFormat.Emf);
                    }
                    else if (image.RawFormat == ImageFormat.Icon)
                    {
                        fileName += ".ico";
                        image.Save(ms, ImageFormat.Icon);
                    }
                    //else if (image.RawFormat == ImageFormat.Heif)
                    //{
                    //    fileName += ".heif";
                    //    image.Save(ms, ImageFormat.Heif);
                    //}
                    //else if (image.RawFormat == ImageFormat.Webp)
                    //{
                    //    fileName += ".webp";
                    //    image.Save(ms, ImageFormat.Webp);
                    //}
                    else
                    {
                        try
                        {
                            fileName += ".bmp";
                            image.Save(ms, ImageFormat.Bmp);
                        }
                        catch (Exception exImg)
                        {
                            Area23Log.LogOriginMsgEx("CImage", "FromDrawingImage", exImg);
                            return null;
                        }
                    }
                }
                else
                {
                    switch (Path.GetExtension(fileName).ToLower())
                    {
                        case "tif":
                        case "tiff":
                        case ".tif":
                            image.Save(ms, ImageFormat.Tiff);
                            break;
                        case "png":
                        case ".png":
                            image.Save(ms, ImageFormat.Png);
                            break;
                        case "jpg":
                        case "jpeg":
                        case ".jpg":
                        case ".jpeg":
                            image.Save(ms, ImageFormat.Jpeg);
                            break;
                        case "gif":
                        case ".gif":
                            image.Save(ms, ImageFormat.Gif);
                            break;
                        case "bmp":
                        case ".bmp":
                            image.Save(ms, ImageFormat.Bmp);
                            break;
                        case "exif":
                        case ".exif":
                            image.Save(ms, ImageFormat.Exif);
                            break;
                        case "emf":
                        case ".emf":
                            image.Save(ms, ImageFormat.Emf);
                            break;
                        case "wmf":
                        case ".wmf":
                            image.Save(ms, ImageFormat.Wmf);
                            break;
                        case "ico":
                        case ".ico":
                            image.Save(ms, ImageFormat.Icon);
                            break;
                        default:
                            image.Save(ms, image.RawFormat);
                            break;
                    }
                }


                imageData = ms.ToByteArray();
                cImage = new CImage(fileName, imageData);
            }

            return cImage;
        }

        #region static members Encrypt2Json Json2Decrypt

        /// <summary>
        /// Encrypt2Json
        /// </summary>
        /// <param name="key">server key to encrypt</param>
        /// <param name="cimg"><see cref="CImage"/> to encrypt and serialize</param>
        /// <returns>a serialized <see cref="string" /> of encrypted <see cref="CImage"/></returns>
        /// <exception cref="CqrException"></exception>
        public static string Encrypt2Json(string key, CImage cimg, EncodingType encoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            if (string.IsNullOrEmpty(key) || cimg == null)
                throw new CqrException($"static string ToJsonEncrypt(string key, ref CImage cimg) failed: NULL reference!");

            string keyHash = EnDeCodeHelper.KeyToHex(key);
            CipherPipe symmPipe = new CipherPipe(key, keyHash);
            try
            {                
                string pipeString = symmPipe.PipeString;
                cimg.Hash = pipeString;
                cimg.Md5Hash = MD5Sum.HashString(String.Concat(key, keyHash, pipeString, cimg.FileName), "");
                cimg.Sha256Hash = Sha256Sum.Hash(cimg.Data, "");

                string encrypted = System.Text.Encoding.UTF8.GetString(
                    symmPipe.EncryptEncodeBytes(cimg.Data, key, keyHash, encoder, zipType, KeyHash.Hex, Crypt.Cipher.CipherMode2.ECB));

                cimg.Data = new byte[0];
                cimg.Message = encrypted;
            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }

            return JsonConvert.SerializeObject(cimg);
        }

        /// <summary>
        /// Json2Decrypt
        /// </summary>
        /// <param name="key">server key to decrypt</param>
        /// <param name="serialized">serialized string of <see cref="CImage"/></param>
        /// <returns>deserialized and decrypted <see cref="CImage"/></returns>
        /// <exception cref="CqrException">thrown, 
        /// when serialized string to decrypt and deserialize is either null or empty 
        /// or <see cref="CImage"/> can't be decrypted and deserialized.
        /// </exception>
        public static new CImage Json2Decrypt(string key, string serialized, EncodingType decoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            if (string.IsNullOrEmpty(serialized))
                throw new CqrException("static CFile FromJsonDecrypt(string serverKey, string serialized): serialized is null or empty.");

            CImage cimg = Newtonsoft.Json.JsonConvert.DeserializeObject<CImage>(serialized);

            string keyHash = EnDeCodeHelper.KeyToHex(key);
            try
            {
                string pipeString = (new CipherPipe(key, keyHash)).PipeString;

                byte[] fileBytes = CipherPipe.DecrpytT<byte[], string>(cimg.Message, key, keyHash,
                        decoder, zipType, KeyHash.Hex, CipherMode2.ECB);

                string md5Hash = MD5Sum.HashString(String.Concat(key, keyHash, pipeString, cimg.FileName), "");
                if (!cimg.Hash.Equals(pipeString))
                {
                    throw new CqrException($"CImage.Hash={cimg.Hash} doesn't match PipeString={pipeString}");
                }
                if (!md5Hash.Equals(cimg.Md5Hash))
                {
                    string md5ErrMsg = $"cimg.Md5Hash={cimg.Md5Hash} doesn't match md5Hash={md5Hash}.";
                    Area23Log.LogOriginMsg("CImage", md5ErrMsg);
                    // throw new CqrException(md5ErrMsg);
                }
                string sha256Hash = Sha256Sum.Hash(fileBytes, "");
                if (!sha256Hash.Equals(cimg.Sha256Hash))
                {
                    string sha256ErrMsg = $"cimg.Sha256Hash={cimg.Sha256Hash} doesn't match sha256Hash={sha256Hash}.";
                    Area23Log.LogOriginMsg("CImage", sha256ErrMsg);
                    // throw new CqrException(sha256ErrMsg);
                }

                cimg.Data = fileBytes;
                cimg.Message = "";

            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }

            return cimg;
        }

        #endregion static members Encrypt2Json Json2Decrypt


        public static CImage CloneCopy(CImage source, CImage destination)
        {
            if (source == null)
                return null;
            if (destination == null)
                destination = new CImage();

            destination.Message = source.Message;
            destination.Hash = source.Hash;
            destination.MsgType = source.MsgType;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;

            destination.FileName = source.FileName;
            destination.Base64Type = source.Base64Type;
            destination.Data = source.Data;
            destination.Sha256Hash = source.Sha256Hash;

            return destination;
        }

        public static CImage CloneCopy(CFile source, CImage destination)
        {
            if (source == null)
                return null;
            if (destination == null)
                destination = new CImage();

            destination.Message = source.Message;
            destination.Hash = source.Hash;
            destination.MsgType = source.MsgType;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;

            destination.FileName = source.FileName;
            destination.Base64Type = source.Base64Type;
            destination.Data = source.Data;
            destination.Sha256Hash = source.Sha256Hash;

            return destination;
        }

        public static CFile ToFile(CImage source)
        {
            if (source == null)
                return null;

            CFile destination = new CFile();

            destination.Message = source.Message;
            destination.Hash = source.Hash;
            destination.MsgType = source.MsgType;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;

            destination.FileName = source.FileName;
            destination.Base64Type = source.Base64Type;
            destination.Data = source.Data;
            destination.Sha256Hash = source.Sha256Hash;

            return destination;
        }

        #endregion static members

    }

}
