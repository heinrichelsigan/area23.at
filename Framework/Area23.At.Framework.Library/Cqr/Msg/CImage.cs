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

        public override string EncryptToJson(
            string serverKey,
            EncodingType encoder = EncodingType.Base64,
            Zfx.ZipType zipType = Zfx.ZipType.None
        )
        {
            CFile cFile = CImage.ToFile(this);

            string serializedJson = CFile.ToJsonEncrypt(serverKey, ref cFile, encoder, zipType);

            return serializedJson;
        }

        public new CImage DecryptFromJson(string serverKey, string serialized = "",
            EncodingType decoder = EncodingType.Base64, Zfx.ZipType zipType = Zfx.ZipType.None)
        {
            if (string.IsNullOrEmpty(serialized))
                serialized = this.SerializedMsg;

            CFile cfile = CFile.FromJsonDecrypt(serverKey, serialized, decoder, zipType);
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

        public new static CImage CloneCopy(CFile source, CImage destination)
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

        public new static CFile ToFile(CImage source)
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
