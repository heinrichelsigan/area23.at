using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Area23.At.Framework.Core.Util;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Area23.At.Framework.Core.CqrXs.CqrMsg
{

    /// <summary>
    /// CqrImage is a image for a <see cref="CqrContact"/>
    /// </summary>
    [JsonObject]
    [Serializable]
    public class CqrImage
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
        }

        /// <summary>
        /// Ctor with <see cref="Image"/> and fileName
        /// </summary>
        /// <param name="image"> <see cref="Image"/>  <see cref="Bitmap"/></param>
        /// <param name="fileName">fileName for the image,
        /// if fileName is null or empty
        /// then a name <see cref="Extensions.Area23DateTimeWithMillis(DateTime)"/></param> + "_image." + extension based mime type will be given
        public CqrImage(Image image, string? fileName = "")
        {
            CqrImage? cqrImage = FromDrawingImage(image, fileName);
            if (cqrImage != null)
            {
                ImageFileName = cqrImage.ImageFileName;
                ImageMimeType = cqrImage.ImageMimeType;
                ImageData = cqrImage.ImageData;
                ImageBase64 = cqrImage.ImageBase64;
            }
        }

        #endregion constructors

        #region members

        public virtual string ToJson()
        {
            CqrImage image = new CqrImage(ImageFileName, ImageData);
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(image, Formatting.Indented);
            return jsonString;
        }

        public virtual CqrImage? FromJson(string jsonText)
        {
            CqrImage? cqrJsonImage;
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
                Area23Log.LogStatic(exJson);
            }

            return null;
        }

        public virtual Bitmap? ToDrawingBitmap()
        {
            Bitmap? bmpImage;
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

        public static void SaveCqrImage(CqrImage image, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory {directoryPath} could not be found.");

            string saveFileName = Path.Combine(directoryPath, image.ImageFileName);
            File.WriteAllBytes(saveFileName, image.ImageData);

            return;
        }


        public static CqrImage LoadCqrImage(string imageFilePath)
        {
            if (!File.Exists(imageFilePath))
                throw new FileNotFoundException($"File {imageFilePath} could not be found.");


            string fileName = Path.GetFileName(imageFilePath);
            byte[] data = File.ReadAllBytes(imageFilePath);
            CqrImage image = new CqrImage(fileName, data);
            return image;

        }

        public static Image? ToDrawingImage(CqrImage cqrImage)
        {
            Bitmap? bmpImage;
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
            

            return (Image?)bmpImage;
        }

        public static CqrImage? FromDrawingImage(Image? image, string? imgName = "")
        {
            if (image == null)
                return null;

            CqrImage? cqrImage = null;
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
                    else if (image.RawFormat == ImageFormat.Heif)
                    {
                        fileName += "heif";
                        image.Save(ms, ImageFormat.Heif);
                    }
                    else if (image.RawFormat == ImageFormat.Webp)
                    {
                        fileName += "webp";
                        image.Save(ms, ImageFormat.Webp);
                    }
                    else
                    {
                        try
                        {
                            fileName += "bmp";
                            image.Save(ms, ImageFormat.Bmp);
                        }
                        catch (Exception exImg)
                        {
                            Area23Log.LogStatic(exImg);
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
