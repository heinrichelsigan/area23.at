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

namespace Area23.At.Framework.Library.Cqr.Msg
{

    /// <summary>
    /// CImage is a image for a <see cref="CqrContact"/>
    /// </summary>
    [Serializable]
    public class CImage : CContent, IMsgAble
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
        [JsonIgnore]
        public string ImageBase64 { get; set; }

        public string Sha256Hash { get; set; }

        #endregion properties

        #region constructors

        /// <summary>
        /// Default empty constructor (needed for json serialize & deserialize)
        /// </summary>
        public CImage()
        {
            ImageFileName = string.Empty;
            ImageData = new byte[0];
            ImageMimeType = string.Empty;
            ImageBase64 = "";
            Md5Hash = "";
            Sha256Hash = "";
            CBytes = new byte[0];
        }


        /// <summary>
        /// Ctor with filename and byte[] data
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public CImage(string fileName, byte[] data)
        {
            ImageFileName = fileName;
            ImageData = data;
            ImageMimeType = MimeType.GetMimeType(ImageData, ImageFileName);
            ImageBase64 = Convert.ToBase64String(ImageData, 0, ImageData.Length);
            Sha256Hash = Sha256Sum.Hash(ImageData, "");
        }

        /// <summary>
        /// Ctor with filename and mime encoded base64 string
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="base64Image"></param>
        public CImage(string fileName, string base64Image)
        {
            ImageFileName = fileName;
            ImageBase64 = base64Image;
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
                CCopy(this, cImage);
            }
        }

        public CImage(CImage cImage)
        {
            if (cImage != null)
            {
                CCopy(this, cImage);
            }
        }

        public CImage(string serializedImgage, CType msgType = CType.Json)
        {
            CImage cImage = (msgType == CType.Xml) ? FromXml<CImage>(serializedImgage) : FromJson(serializedImgage);
            if (cImage != null)
            {
                CCopy(this, cImage);
            }
        }

        #endregion constructors



        public new CImage CCopy(CImage destination, CImage source)
        {
            return CloneCopy(source, destination);
        }

        #region EnDeCrypt+DeSerialize


        public override string EncryptToJson(string serverKey)
        {
            CImage cimg = new CImage(this);

            string serializedJson = ToJsonEncrypt(serverKey, ref cimg);

            return serializedJson;
        }

        public new CImage DecryptFromJson(string serverKey, string serialized = "")
        {
            if (string.IsNullOrEmpty(serialized))
                serialized = this.SerializedMsg;

            CImage cimg = FromJsonDecrypt(serverKey, serialized);
            if (cimg == null)
                throw new CqrException($"CImage DecryptFromJson(string serverKey, string serialized) failed.");

            return CCopy(cimg, this);
        }


        #endregion EnDeCrypt+DeSerialize


        #region members


        /// <summary>
        /// Serializes this <see cref="CImage"/> to <see cref="string">serialized json string</see>
        /// </summary>
        /// <returns><see cref="string">serialized json string</see></returns>
        public override string ToJson()
        {
            this.SerializedMsg = "";
            string jsonText = JsonConvert.SerializeObject(this);
            this.SerializedMsg = jsonText;
            return jsonText;
        }

        /// <summary>
        /// FromJson deserializes a <see cref="CImage"/> from serialized json <see cref="string"/>
        /// </summary>
        /// <param name="jsonText">serialized json <see cref="string"/></param>
        /// <returns>deserialized  <see cref="CImage"/></returns>
        public virtual CImage FromJson(string jsonText)
        {
            CImage cJsonImage;
            try
            {
                cJsonImage = Newtonsoft.Json.JsonConvert.DeserializeObject<CImage>(jsonText);
                if (cJsonImage != null && !string.IsNullOrEmpty(cJsonImage.ImageFileName) && !string.IsNullOrEmpty(cJsonImage.ImageBase64))
                {
                    return CCopy(this, cJsonImage);
                }
            }
            catch (Exception exJson)
            {
                SLog.Log(exJson);
            }

            return null;
        }

        /// <summary>
        /// ToXml serializes this <see cref="CImage"/> to serialized xml <see cref="string"/>
        /// </summary>
        /// <returns></returns>
        public override string ToXml() => this.ToXml();

        /// <summary>
        /// FromXml deserializes <see cref="CImage"/> from <see cref="string">xml serialized string</see>
        /// </summary>
        /// <param name="xmlText"><see cref="string">xml serialized string</see></param>
        /// <returns>deserialized <see cref="CImage"/> /returns>
        public virtual CImage FromXml(string xmlText)
        {
            CImage cXmlImg = Utils.DeserializeFromXml<CImage>(xmlText ?? "");
            if (cXmlImg != null && cXmlImg is CImage cimg)
            {
                return CCopy(this, cimg);
            }

            return cXmlImg;
        }

        /// <summary>
        /// ToDrawingBitmap converts this <see cref="CImage"/> to <see cref="System.Drawing.Bitmap"/>
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

        #region static members SaveCqrImage LoadCqrImage ToDrawingImage FromDrawingImage

        /// <summary>
        /// Saves a <see cref="CImage"/> to a filepath
        /// </summary>
        /// <param name="image"><see cref="CImage"/></param>
        /// <param name="directoryPath">full directory and file path</param>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static void SaveCqrImage(CImage cImage, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory {directoryPath} could not be found.");

            string saveFileName = Path.Combine(directoryPath, cImage.ImageFileName);
            File.WriteAllBytes(saveFileName, cImage.ImageData);

            return;
        }

        /// <summary>
        /// LoadCqrImage loads a <see cref="CImage"/> from filepath
        /// </summary>
        /// <param name="imageFilePath">full filepath</param>
        /// <returns><see cref="CImage"/></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static CImage LoadCqrImage(string imageFilePath)
        {
            if (!File.Exists(imageFilePath))
                throw new FileNotFoundException($"File {imageFilePath} could not be found.");


            string fileName = Path.GetFileName(imageFilePath);
            byte[] data = File.ReadAllBytes(imageFilePath);
            CImage image = new CImage(fileName, data);
            return image;

        }

        /// <summary>
        /// ToDrawingImage converts a <see cref="CImage"/> to a <see cref="System.Drawing.Image"/>
        /// </summary>
        /// <param name="cqrImage"><see cref="CImage"/> to convert</param>
        /// <returns>converted <see cref="System.Drawing.Image"/></returns>
        public static Image ToDrawingImage(CImage cImage)
        {
            Bitmap bmpImage;
            if (cImage.ImageData == null || cImage.ImageData.Length == 0)
            {
                byte[] data = Convert.FromBase64String(cImage.ImageBase64);
                using (MemoryStream ms = new MemoryStream(data))
                {
                    bmpImage = new Bitmap(ms, true);
                }
            }
            else
            {
                using (MemoryStream ms = new MemoryStream(cImage.ImageData))
                {
                    bmpImage = new Bitmap(ms, true);
                }
            }


            return (Image)bmpImage;
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
                cImage = new CImage(fileName, imageData);
            }

            return cImage;
        }

        #endregion static members SaveCqrImage LoadCqrImage ToDrawingImage FromDrawingImage

        #region static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg

        /// <summary>
        /// ToJsonEncrypt
        /// </summary>
        /// <param name="serverKey">server key to encrypt</param>
        /// <param name="ccntct"><see cref="CContact"/> to encrypt and serialize</param>
        /// <returns>a serialized <see cref="string" /> of encrypted <see cref="CContact"/></returns>
        /// <exception cref="CqrException"></exception>
        public static string ToJsonEncrypt(string serverKey, ref CImage cimg)
        {
            if (string.IsNullOrEmpty(serverKey) || cimg == null)
                throw new CqrException($"static stringToJsonEncrypt(string serverKey, CImage cimg) failed: NULL reference!");

            if (!EncryptSrvMsg(serverKey, ref cimg))
                throw new CqrException($"static string ToJsonEncrypt(string serverKey, CImage cimg) failed.");

            string serializedJson = cimg.ToJson();
            return serializedJson;
        }

        public static bool EncryptSrvMsg(string serverKey, ref CImage cimg)
        {
            try
            {
                string hash = EnDeCodeHelper.KeyToHex(serverKey);
                SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, hash);
                cimg.Hash = hash;
                cimg.Md5Hash = MD5Sum.HashString(String.Concat(serverKey, hash, symmPipe.PipeString, cimg.Message), "");
                cimg.Sha256Hash = Sha256Sum.Hash(cimg.ImageData, "");

                byte[] msgBytes = cimg.ImageData;
                byte[] cqrbytes = LibPaths.CqrEncrypt ? symmPipe.MerryGoRoundEncrpyt(msgBytes, serverKey, hash) : msgBytes;

                cimg.CBytes = cqrbytes;
                cimg.ImageData = new byte[0];
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
        /// <param name="serialized">serialized string of <see cref="CImage"/></param>
        /// <returns>deserialized and decrypted <see cref="CImage"/></returns>
        /// <exception cref="CqrException">thrown, 
        /// when serialized string to decrypt and deserialize is either null or empty 
        /// or <see cref="CImage"/> can't be decrypted and deserialized.
        /// </exception>
        public static CImage FromJsonDecrypt(string serverKey, string serialized)
        {
            if (string.IsNullOrEmpty(serialized))
                throw new CqrException("static CContact FromJsonDecrypt(string serverKey, string serialized): serialized is null or empty.");

            CImage cimg = new CImage(serialized, CType.Json);
            cimg = DecryptSrvMsg(serverKey, ref cimg);
            if (cimg == null)
                throw new CqrException($"static CImage FromJsonDecrypt(string serverKey, string serialized).");

            return cimg;
        }

        public static CImage DecryptSrvMsg(string serverKey, ref CImage cimg)
        {
            try
            {
                string hash = EnDeCodeHelper.KeyToHex(serverKey);
                SymmCipherPipe symmPipe = new SymmCipherPipe(serverKey, hash);

                byte[] cipherBytes = cimg.CBytes;
                byte[] unroundedMerryBytes = LibPaths.CqrEncrypt ? symmPipe.DecrpytRoundGoMerry(cipherBytes, serverKey, hash) : cipherBytes;

                if (!cimg.Hash.Equals(symmPipe.PipeString))
                    throw new CqrException($"Hash: {cimg.Hash} doesn't match symmPipe.PipeString: {symmPipe.PipeString}");

                string md5Hash = MD5Sum.HashString(String.Concat(serverKey, cimg.Hash, symmPipe.PipeString, cimg.Message), "");
                if (!md5Hash.Equals(cimg.Md5Hash))
                {
                    string md5ErrMsg = $"md5Hash: {md5Hash} doesn't match property Md5Hash: {cimg.Md5Hash}";
                    Area23Log.Logger.LogOriginMsg("CImage", md5ErrMsg);
                    // throw new CqrException(md5ErrMsg);
                }
                string sha256Hash = Sha256Sum.Hash(unroundedMerryBytes, "");
                if (!sha256Hash.Equals(cimg.Sha256Hash))
                {
                    string sha256ErrMsg = $"Sha256 from decrypted = {sha256Hash}, while this.Sha256Hash = {cimg.Sha256Hash}.";
                    Area23Log.Logger.LogOriginMsg("CImage", sha256ErrMsg);
                    // throw new CqrException(sha256ErrMsg);
                }

                cimg.ImageData = unroundedMerryBytes;
                cimg.CBytes = new byte[0];

            }
            catch (Exception exCrypt)
            {
                CqrException.SetLastException(exCrypt);
                throw;
            }

            return cimg;
        }

        #endregion static members ToJsonEncrypt EncryptSrvMsg FromJsonDecrypt DecryptSrvMsg


        public new static CImage CloneCopy(CImage source, CImage destination)
        {
            if (source == null)
                return null;
            if (destination == null)
                destination = new CImage(source);

            destination.Message = source.Message;
            destination.Hash = source.Hash;
            destination.MsgType = source.MsgType;
            destination.CBytes = source.CBytes;
            destination.Md5Hash = source.Md5Hash;

            destination.ImageFileName = source.ImageFileName;
            destination.ImageMimeType = source.ImageMimeType;
            destination.ImageData = source.ImageData;
            destination.ImageBase64 = source.ImageBase64;
            destination.Sha256Hash = source.Sha256Hash;
            destination.ImageBase64 = source.ImageBase64;
            destination.SerializedMsg = "";
            destination.SerializedMsg = destination.ToJson();

            return destination;
        }



        #endregion static members

    }


}
