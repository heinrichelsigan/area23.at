using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Area23.At.Framework.Library.Crypt.CqrJd
{

    /// <summary>
    /// CqrImage is a image for a <see cref="CqrContact"/>
    /// </summary>
    [DataContract(Name = "CqrImage")]
    [Description("cqrxs.eu image")]
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
        /// Ctor with <see cref="System.Drawing.Image"/> and fileName
        /// </summary>
        /// <param name="image"> <see cref="System.Drawing.Image"/>  <see cref="System.Drawing.Bitmap"/></param>
        /// <param name="fileName">fileName for the image,
        /// if fileName is null or empty
        /// then a name <see cref="Util.Extensions.Area23DateTimeWithMillis(DateTime)"/></param> + "_image." + extension based mime type will be given
        public CqrImage(Image image, string fileName = "")
        {
            CqrImage cqrImage = CqrImage.FromDrawingImage(image, fileName);
            if (cqrImage != null)
            {
                this.ImageFileName = cqrImage.ImageFileName;
                this.ImageMimeType = cqrImage.ImageMimeType;
                this.ImageData = cqrImage.ImageData;
                this.ImageBase64 = cqrImage.ImageBase64;
            }
        }

        #endregion constructors

        #region members

        public virtual string ToJson()
        {
            CqrImage image = new CqrImage(ImageFileName, ImageData);
            string jsonString = JsonConvert.SerializeObject(image, Formatting.Indented);
            return jsonString;
        }

        public virtual CqrImage FromJson(string jsonText)
        {
            CqrImage cqrJsonImage;
            try
            {
                cqrJsonImage = JsonConvert.DeserializeObject<CqrImage>(jsonText);
                if (cqrJsonImage != null && !string.IsNullOrEmpty(cqrJsonImage?.ImageFileName) && !string.IsNullOrEmpty(cqrJsonImage?.ImageBase64))
                {
                    this.ImageFileName = cqrJsonImage.ImageFileName;
                    this.ImageBase64 = cqrJsonImage.ImageBase64;
                    this.ImageMimeType = cqrJsonImage.ImageMimeType;
                    this.ImageData = cqrJsonImage.ImageData;
                    return cqrJsonImage;
                }
            }
            catch (Exception exJson)
            {
                Area23Log.LogStatic(exJson);
            }

            return null;
        }

        public virtual Bitmap ToDrawingBitmap()
        {
            Bitmap bmpImage;

            using (MemoryStream ms = new MemoryStream(ImageData))
            {
                bmpImage = new Bitmap(ms, true);
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

        public static Image ToDrawingImage(CqrImage cqrImage)
        {
            Bitmap bmpImage;

            using (MemoryStream ms = new MemoryStream(cqrImage.ImageData))
            {
                bmpImage = new Bitmap(ms, true);
            }

            return (Image)bmpImage;
        }

        public static CqrImage FromDrawingImage(System.Drawing.Image image, string imgName = "")
        {
            if (image == null)
                return null;

            CqrImage cqrImage;
            byte[] imageData;
            string fileName = imgName ?? string.Empty;
            if (string.IsNullOrEmpty(imgName))
            {
                fileName = DateTime.Now.Area23DateTimeWithMillis() + "_image.";
                if (image.RawFormat == ImageFormat.Tiff)
                    fileName += "tif";
                else if (image.RawFormat == ImageFormat.Png)
                    fileName += "png";
                else if (image.RawFormat == ImageFormat.Jpeg)
                    fileName += "jpg";
                else if (image.RawFormat == ImageFormat.Gif)
                    fileName += "gif";
                else if (image.RawFormat == ImageFormat.Bmp || image.RawFormat == ImageFormat.MemoryBmp)
                    fileName += "bmp";
                else if (image.RawFormat == ImageFormat.Exif)
                    fileName += "exif";
                else if (image.RawFormat == ImageFormat.Wmf)
                    fileName += "wmf";
                else if (image.RawFormat == ImageFormat.Emf)
                    fileName += "emf";
                else if (image.RawFormat == ImageFormat.Icon)
                    fileName += "ico";
                else
                    fileName += "raw";
            }
            using (MemoryStream ms = new MemoryStream())
            {
                image?.Save(ms, image.RawFormat);
                imageData = ms.ToByteArray();
                cqrImage = new CqrImage(fileName, imageData);
            }

            return cqrImage;
        }

        #endregion static members

    }


}
