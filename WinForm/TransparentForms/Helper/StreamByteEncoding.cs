using Area23.At.Framework.Library.Cqr.Msg;
using Area23.At.Framework.Library.Util;
using Area23.At.Framework.Library.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.WinForm.TransparentForms.Helper
{
    [Serializable]
    public class StreamByteEncoding
    {
        public string Name { get; set; }

        public string Base64Type { get; set; }

        public Image AnImage { get; set; }

        public byte[] Bytes { get; set; }

        public MemoryStream MemStream { get; set; }

        public string Base64Enc { get; set; }



        public StreamByteEncoding()
        {
            Name = string.Empty;
            Base64Type = string.Empty;
            AnImage = null;
            Bytes = new byte[0];
            MemStream = new MemoryStream(Bytes);
        }

        public StreamByteEncoding(string name, Bitmap bmp)
        {
            FromImage(name, bmp);
        }

        public StreamByteEncoding(string name, string base64)
        {
            FromBase64(name, base64);
        }

        public StreamByteEncoding(string name, byte[] bytes)
        {
            FromBytes(name, bytes);
        }
        public StreamByteEncoding(String name, Stream stream)
        {
            FromStream(name, stream);
        }

        public void FromImage(string name, Bitmap bmp)
        {
            int width = 64;
            int height = bmp.Height;
            double h = ((double)height / (double)(width / 64));
            height = (int)h;
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            AnImage = bmp.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);
            Name = name;
            MemStream = new MemoryStream();
            AnImage.Save(MemStream, bmp.RawFormat);
            MemStream.Position = 0;
            MemStream.Seek(0, SeekOrigin.Begin);
            Bytes = MemStream.ToByteArray();
            Base64Enc = Convert.ToBase64String(Bytes);
            Base64Type = MimeType.GetMimeType(Bytes, Name);
        }

        public void FromBase64(string name, string base64)
        {
            Name = name;
            Base64Enc = base64;
            Bytes = Convert.FromBase64String(Base64Enc);
            MemStream = new MemoryStream();
            MemStream.Write(Bytes, 0, Bytes.Length);
            MemStream.Position = 0;
            MemStream.Seek(0, SeekOrigin.Begin);
            AnImage = new Bitmap(MemStream);
            Base64Type = MimeType.GetMimeType(Bytes, Name);
        }

        public void FromBytes(string name, byte[] bytes)
        {            
            Name = name;
            Bytes = bytes;
            Base64Enc = Convert.ToBase64String(bytes);

            MemStream = new MemoryStream();
            MemStream.Write(Bytes, 0, Bytes.Length);
            MemStream.Position = 0;
            MemStream.Seek(0, SeekOrigin.Begin);
            
            AnImage = new Bitmap(MemStream);
            Base64Type = MimeType.GetMimeType(Bytes, Name);
        }

        public void FromStream(string name, Stream stream) 
        {
            Name = name;
            MemStream = new MemoryStream();            
            stream.CopyTo(MemStream);            
            
            MemStream.Position = 0;
            MemStream.Seek(0, SeekOrigin.Begin);
            Bytes = MemStream.ToByteArray();
            Base64Enc = Convert.ToBase64String(Bytes);
            AnImage = new Bitmap(MemStream);
            Base64Type = MimeType.GetMimeType(Bytes, Name);

            stream.Close();
            stream.Dispose();
        }

        public bool ThumbnailCallback() => false;


        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public StreamByteEncoding FromJson(string json)
        {
            StreamByteEncoding sbe = JsonConvert.DeserializeObject<StreamByteEncoding>(json);
            if (sbe != null)
            {
                this.Name = sbe.Name;
                this.AnImage = sbe.AnImage;
                this.Base64Enc = sbe.Base64Enc;
                this.Bytes = sbe.Bytes;
                this.MemStream = sbe.MemStream;
                this.Base64Type = sbe.Base64Type;
            }
            return this;
        }

    }

}
