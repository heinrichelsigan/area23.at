using Area23.At.Framework.Library.CqrXs.CqrMsg;
using Area23.At.Framework.Library.Crypt.Hash;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Area23.At.Framework.Library.CqrMsg
{

    /// <summary>
    /// CqrImage is a image for a <see cref="CqrContact"/>
    /// </summary>
    [Serializable]
    public class CqrImage : MsgContent, ICqrMessagable
    {

        #region properties

        /// <summary>
        /// File Name with extension of Image
        /// </summary>
        public string ImageFileName { get; set; }

        /// <summary>
        /// Mime Type of Image
        /// </summary>
        public string ImageMimeType { get; set; }

        /// <summary>
        /// byte[] of Image Raw Data
        /// </summary>
        [JsonIgnore]
        internal byte[] ImageData { get; set; }

        /// <summary>
        /// Base64 mime encoded string of raw data
        /// </summary>
        public string ImageBase64 { get; set; }

        #endregion properties

        #region constructors

        /// <summary>
        /// Default empty constructor (needed for json serialize & deserialize)
        /// </summary>
        public CqrImage()
        {
            ImageFileName = string.Empty;
            ImageData = new byte[0];
            ImageMimeType = string.Empty;
            ImageBase64 = "";
            Md5Hash = "";
        }


        /// <summary>
        /// Ctor with filename and byte[] data
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public CqrImage(string fileName, byte[] data)
        {
            ImageFileName = fileName;
            ImageData = data;
            ImageMimeType = MimeType.GetMimeType(ImageData, ImageFileName);
            ImageBase64 = Convert.ToBase64String(ImageData, 0, ImageData.Length);
            Md5Hash = MD5Sum.Hash(ImageData, ImageFileName);
        }

        /// <summary>
        /// Ctor with filename and mime encoded base64 string
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="base64Image"></param>
        public CqrImage(string fileName, string base64Image)
        {
            ImageFileName = fileName;
            ImageBase64 = base64Image;
            ImageData = Convert.FromBase64String(base64Image);
            ImageMimeType = MimeType.GetMimeType(ImageData, ImageFileName);
            Md5Hash = MD5Sum.Hash(ImageData, ImageFileName);
        }

        /// <summary>
        /// Ctor with <see cref="Image"/> and fileName
        /// </summary>
        /// <param name="image"> <see cref="Image"/>  <see cref="Bitmap"/></param>
        /// <param name="fileName">fileName for the image,
        /// if fileName is null or empty
        /// then a name <see cref="Extensions.Area23DateTimeWithMillis(DateTime)"/></param> + "_image." + extension based mime type will be given
        public CqrImage(Image image, string fileName = "")
        {
            CqrImage cqrImage = FromDrawingImage(image, fileName);
            if (cqrImage != null)
            {
                ImageFileName = cqrImage.ImageFileName;
                ImageMimeType = cqrImage.ImageMimeType;
                ImageData = cqrImage.ImageData;
                ImageBase64 = cqrImage.ImageBase64;
                Md5Hash = MD5Sum.Hash(ImageData, ImageFileName);
            }
        }

        #endregion constructors


        #region members

        /// <summary>
        /// Serializes this <see cref="CqrImage"/> to <see cref="string">serialized json string</see>
        /// </summary>
        /// <returns><see cref="string">serialized json string</see></returns>
        public override string ToJson()
        {
            CqrImage image = new CqrImage(ImageFileName, ImageData);
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(image, Formatting.Indented);
            return jsonString;
        }

        /// <summary>
        /// FromJson deserializes a <see cref="CqrImage"/> from serialized json <see cref="string"/>
        /// </summary>
        /// <param name="jsonText">serialized json <see cref="string"/></param>
        /// <returns>deserialized  <see cref="CqrImage"/></returns>
        public virtual CqrImage FromJson(string jsonText)
        {
            CqrImage cqrJsonImage;
            try
            {
                cqrJsonImage = Newtonsoft.Json.JsonConvert.DeserializeObject<CqrImage>(jsonText);
                if (cqrJsonImage != null && !string.IsNullOrEmpty(cqrJsonImage?.ImageFileName) && !string.IsNullOrEmpty(cqrJsonImage?.ImageBase64))
                {
                    ImageFileName = cqrJsonImage.ImageFileName;
                    ImageBase64 = cqrJsonImage.ImageBase64;
                    ImageMimeType = cqrJsonImage.ImageMimeType;
                    ImageData = cqrJsonImage.ImageData;
                    return cqrJsonImage;
                }
            }
            catch (Exception exJson)
            {
                SLog.Log(exJson);
            }

            return null;
        }

        /// <summary>
        /// ToXml serializes this <see cref="CqrImage"/> to serialized xml <see cref="string"/>
        /// </summary>
        /// <returns></returns>
        public override string ToXml() => this.ToXml();

        /// <summary>
        /// FromXml deserializes <see cref="CqrImage"/> from <see cref="string">xml serialized string</see>
        /// </summary>
        /// <param name="xmlText"><see cref="string">xml serialized string</see></param>
        /// <returns>deserialized <see cref="CqrImage"/> /returns>
        public virtual CqrImage FromXml(string xmlText) 
        {
            CqrImage cqrImg = Utils.DeserializeFromXml<CqrImage>(xmlText ?? "");
            if (cqrImg != null && cqrImg is CqrImage cimg)
            {
                ImageFileName = cimg.ImageFileName;
                ImageBase64 = cimg.ImageBase64;
                ImageMimeType = cimg.ImageMimeType;
                ImageData = cimg.ImageData;
                _hash = cimg._hash ?? string.Empty;
                Md5Hash = cimg.Md5Hash;
                _message = cimg._message;
                MsgType = cimg.MsgType;
                RawMessage = cimg.RawMessage;
            }

            return cqrImg;
        }

        /// <summary>
        /// ToDrawingBitmap converts this <see cref="CqrImage"/> to <see cref="System.Drawing.Bitmap"/>
        /// </summary>
        /// <returns>transformed see cref="System.Drawing.Bitmap"/></returns>
        public virtual Bitmap ToDrawingBitmap()
        {
            Bitmap bmpImage;
            if (ImageData == null || ImageData.Length == 0)
            {
                byte[] data = Convert.FromBase64String(ImageBase64);
                using (MemoryStream ms = new MemoryStream(data))
                {
                    bmpImage = new Bitmap(ms, true);
                }
            }
            else
            {
                using (MemoryStream ms = new MemoryStream(ImageData))
                {
                    bmpImage = new Bitmap(ms, true);
                }
            }

            return bmpImage;
        }

        #endregion members

        #region static members

        /// <summary>
        /// Saves a <see cref="CqrImage"/> to a filepath
        /// </summary>
        /// <param name="image"><see cref="CqrImage"/></param>
        /// <param name="directoryPath">full directory and file path</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void SaveCqrImage(CqrImage image, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory {directoryPath} could not be found.");

            string saveFileName = Path.Combine(directoryPath, image.ImageFileName);
            File.WriteAllBytes(saveFileName, image.ImageData);

            return;
        }

        /// <summary>
        /// LoadCqrImage loads a <see cref="CqrImage"/> from filepath
        /// </summary>
        /// <param name="imageFilePath">full filepath</param>
        /// <returns><see cref="CqrImage"/></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static CqrImage LoadCqrImage(string imageFilePath)
        {
            if (!File.Exists(imageFilePath))
                throw new FileNotFoundException($"File {imageFilePath} could not be found.");


            string fileName = Path.GetFileName(imageFilePath);
            byte[] data = File.ReadAllBytes(imageFilePath);
            CqrImage image = new CqrImage(fileName, data);
            return image;

        }

        /// <summary>
        /// ToDrawingImage converts a <see cref="CqrImage"/> to a <see cref="System.Drawing.Image"/>
        /// </summary>
        /// <param name="cqrImage"><see cref="CqrImage"/> to convert</param>
        /// <returns>converted <see cref="System.Drawing.Image"/></returns>
        public static Image ToDrawingImage(CqrImage cqrImage)
        {
            Bitmap bmpImage;
            if (cqrImage.ImageData == null || cqrImage.ImageData.Length == 0)
            {
                byte[] data = Convert.FromBase64String(cqrImage.ImageBase64);
                using (MemoryStream ms = new MemoryStream(data))
                {
                    bmpImage = new Bitmap(ms, true);
                }
            }
            else
            {
                using (MemoryStream ms = new MemoryStream(cqrImage.ImageData))
                {
                    bmpImage = new Bitmap(ms, true);
                }
            }


            return (Image)bmpImage;
        }

        /// <summary>
        /// FromDrawingImage converts a <see cref="CqrImage"/> from a <see cref="System.Drawing.Image"/>
        /// </summary>
        /// <param name="image"><see cref="System.Drawing.Image"/></param>
        /// <param name="imgName"><see cref="string">string imgName</see></param>
        /// <returns><see cref="CqrImage">converted CqrImage</see></returns>
        public static CqrImage FromDrawingImage(Image image, string imgName = "")
        {
            if (image == null)
                return null;

            CqrImage cqrImage = null;
            // ImageFormat format = image.RawFormat;
            byte[] imageData;
            string fileName = string.IsNullOrEmpty(imgName) ? string.Empty : imgName;

            using (MemoryStream ms = new MemoryStream())
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName += DateTime.Now.Area23DateTimeWithMillis() + "_image.";
                    if (image.RawFormat == ImageFormat.Tiff)
                    {
                        fileName += "tif";
                        image.Save(ms, ImageFormat.Tiff);
                    }
                    else if (image.RawFormat == ImageFormat.Png)
                    {
                        fileName += "png";
                        image.Save(ms, ImageFormat.Png);
                    }
                    else if (image.RawFormat == ImageFormat.Jpeg)
                    {
                        fileName += "jpg";
                        image.Save(ms, ImageFormat.Jpeg);
                    }
                    else if (image.RawFormat == ImageFormat.Gif)
                    {
                        fileName += "gif";
                        image.Save(ms, ImageFormat.Gif);
                    }
                    else if (image.RawFormat == ImageFormat.Bmp || image.RawFormat == ImageFormat.MemoryBmp)
                    {
                        fileName += "bmp";
                        image.Save(ms, ImageFormat.Bmp);
                    }
                    else if (image.RawFormat == ImageFormat.Exif)
                    {
                        fileName += "exif";
                        image.Save(ms, ImageFormat.Exif);
                    }
                    else if (image.RawFormat == ImageFormat.Wmf)
                    {
                        fileName += "wmf";
                        image.Save(ms, ImageFormat.Wmf);
                    }
                    else if (image.RawFormat == ImageFormat.Emf)
                    {
                        fileName += "emf";
                        image.Save(ms, ImageFormat.Emf);
                    }
                    else if (image.RawFormat == ImageFormat.Icon)
                    {
                        fileName += "ico";
                        image.Save(ms, ImageFormat.Icon);
                    }
                    //else if (image.RawFormat == ImageFormat.Heif)
                    //{
                    //    fileName += "heif";
                    //    image.Save(ms, ImageFormat.Heif);
                    //}
                    //else if (image.RawFormat == ImageFormat.Webp)
                    //{
                    //    fileName += "webp";
                    //    image.Save(ms, ImageFormat.Webp);
                    //}
                    else
                    {
                        try
                        {
                            fileName += "bmp";
                            image.Save(ms, ImageFormat.Bmp);
                        }
                        catch (Exception exImg)
                        {
                            SLog.Log(exImg);
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
                cqrImage = new CqrImage(fileName, imageData);
            }

            return cqrImage;
        }

        #endregion static members

    }

}
