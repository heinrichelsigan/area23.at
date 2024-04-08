using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

// https://github.com/DataDink/Bumpkit/blob/master/BumpKit/BumpKit/GifEncoder.cs

namespace Area23.At.Www.S.Util.BumpKit
{

    /// <summary>   
    /// This encoder is taken from <see cref="https://github.com/DataDink/Bumpkit" />
    /// <seealso cref="https://github.com/DataDink/Bumpkit/blob/master/BumpKit/BumpKit/GifEncoder.cs">GifEncoder.cs</seealso>
    /// <seealso cref="https://github.com/DataDink/Bumpkit?tab=Unlicense-1-ov-file">Unlicense license of DataDink/Bumpkit</seealso>
    /// Encodes multiple images as an animated gif to a stream. <br />
    /// ALWAYS ALWAYS ALWAYS wire this up   in a using block <br />
    /// Disposing the encoder will complete the file. <br />
    /// Uses default .net GIF encoding and adds animation headers.
    /// </summary>
    public class GifEncoder : IDisposable
    {
        #region Header Constants
        private const string FileType = "GIF";
        private const string FileVersion = "89a";
        private const byte FileTrailer = 0x3b;

        private const int ApplicationExtensionBlockIdentifier = 0xff21;
        private const byte FeByte = 0xfe;
        private const byte ApplicationBlockSize = 0x0b;
        private const string ApplicationIdentification = "NETSCAPE2.0";

        private const int GraphicControlExtensionBlockIdentifier = 0xf921;
        private const byte GraphicControlExtensionBlockSize = 0x04;

        private const long SourceGlobalColorInfoPosition = 10;
        private const long SourceGraphicControlExtensionPosition = 781;
        private const long SourceGraphicControlExtensionLength = 8;
        private const long SourceImageBlockPosition = 789;
        private const long SourceImageBlockHeaderLength = 11;
        private const long SourceColorBlockPosition = 13;
        private const long SourceColorBlockLength = 768;
        #endregion

        private bool _isFirstImage = true;
        private int? _width;
        private int? _height;
        private int? _repeatCount;
        private readonly Stream _stream;

        // Public Accessors
        public TimeSpan FrameDelay { get; set; }

        /// <summary>
        /// Encodes multiple images as an animated gif to a stream. <br />
        /// ALWAYS ALWAYS ALWAYS wire this in a using block <br />
        /// Disposing the encoder will complete the file. <br />
        /// Uses default .net GIF encoding and adds animation headers.
        /// </summary>
        /// <param name="stream">The stream that will be written to.</param>
        /// <param name="width">Sets the width for this gif or null to use the first frame's width.</param>
        /// <param name="height">Sets the height for this gif or null to use the first frame's height.</param>
        public GifEncoder(Stream stream, int? width = null, int? height = null, int? repeatCount = null)
        {
            _stream = stream;
            _width = width;
            _height = height;
            _repeatCount = repeatCount;
        }

        /// <summary>
        /// Adds a frame to this animation.
        /// </summary>
        /// <param name="img">The image to add</param>
        /// <param name="x">The positioning x offset this image should be displayed at.</param>
        /// <param name="y">The positioning y offset this image should be displayed at.</param>
        public void AddFrame(Image img, int x = 0, int y = 0, TimeSpan? frameDelay = null, string comment = null)
        {
            using (var gifStream = new MemoryStream())
            {
                img.Save(gifStream, ImageFormat.Gif);
                if (_isFirstImage) // Steal the global color table info
                {
                    comment = comment ?? ApplicationIdentification;
                    InitHeader(gifStream, img.Width, img.Height, comment);
                }
                WriteGraphicControlBlock(gifStream, frameDelay.GetValueOrDefault(FrameDelay));
                WriteImageBlock(gifStream, !_isFirstImage, x, y, img.Width, img.Height);
            }
            _isFirstImage = false;
        }

        private void InitHeader(Stream sourceGif, int w, int h, string comment = null)
        {            
            // File Header
            WriteString(FileType);
            WriteString(FileVersion);
            WriteShort(_width.GetValueOrDefault(w)); // Initial Logical Width
            WriteShort(_height.GetValueOrDefault(h)); // Initial Logical Height
            sourceGif.Position = SourceGlobalColorInfoPosition;
            WriteByte(sourceGif.ReadByte()); // Global Color Table Info
            WriteByte(0); // Background Color Index
            WriteByte(0); // Pixel aspect ratio
            WriteColorTable(sourceGif);

            // App Extension Header
            // WriteShort(ApplicationExtensionBlockIdentifier); // 0xff21
            // WriteByte(FeByte);
            // WriteByte(0x21);
            // WriteByte(0xfe);
            // byte[] bytesCommentLength = Encoding.ASCII.GetBytes(comment.Length.ToString("X"));
            // byte[] bytesComment = Encoding.ASCII.GetBytes(comment);
            // WriteString((byte)bytesComment.Length);
            // WriteString(comment);
            // WriteByte(ApplicationBlockSize);
            // WriteString(ApplicationIdentification);            
            // WriteByte(3); // Application block length
            // WriteByte(1);
            

            comment = comment ?? ApplicationIdentification;
            WriteComment(comment, _repeatCount.GetValueOrDefault(0));

            // WriteShort(_repeatCount.GetValueOrDefault(0)); // Repeat count for images.
            WriteByte(0); // terminator
        }

        private void WriteColorTable(Stream sourceGif)
        {
            sourceGif.Position = SourceColorBlockPosition; // Locating the image color table
            var colorTable = new byte[SourceColorBlockLength];
            sourceGif.Read(colorTable, 0, colorTable.Length);
            _stream.Write(colorTable, 0, colorTable.Length);
        }

        private void WriteGraphicControlBlock(Stream sourceGif, TimeSpan frameDelay)
        {
            sourceGif.Position = SourceGraphicControlExtensionPosition; // Locating the source GCE
            var blockhead = new byte[SourceGraphicControlExtensionLength];
            sourceGif.Read(blockhead, 0, blockhead.Length); // Reading source GCE

            WriteShort(GraphicControlExtensionBlockIdentifier); // Identifier
            WriteByte(GraphicControlExtensionBlockSize); // Block Size
            WriteByte(blockhead[3] & 0xf7 | 0x08); // Setting disposal flag
            WriteShort(Convert.ToInt32(frameDelay.TotalMilliseconds / 10)); // Setting frame delay
            WriteByte(blockhead[6]); // Transparent color index
            WriteByte(0); // Terminator
        }

        private void WriteImageBlock(Stream sourceGif, bool includeColorTable, int x, int y, int h, int w)
        {
            sourceGif.Position = SourceImageBlockPosition; // Locating the image block
            var header = new byte[SourceImageBlockHeaderLength];
            sourceGif.Read(header, 0, header.Length);
            WriteByte(header[0]); // Separator
            WriteShort(x); // Position X
            WriteShort(y); // Position Y
            WriteShort(h); // Height
            WriteShort(w); // Width

            if (includeColorTable) // If first frame, use global color table - else use local
            {
                sourceGif.Position = SourceGlobalColorInfoPosition;
                WriteByte(sourceGif.ReadByte() & 0x3f | 0x80); // Enabling local color table
                WriteColorTable(sourceGif);
            }
            else
            {
                WriteByte(header[9] & 0x07 | 0x07); // Disabling local color table
            }

            WriteByte(header[10]); // LZW Min Code Size

            // Read/Write image data
            sourceGif.Position = SourceImageBlockPosition + SourceImageBlockHeaderLength;

            var dataLength = sourceGif.ReadByte();
            while (dataLength > 0)
            {
                var imgData = new byte[dataLength];
                sourceGif.Read(imgData, 0, dataLength);

                _stream.WriteByte(Convert.ToByte(dataLength));
                _stream.Write(imgData, 0, dataLength);
                dataLength = sourceGif.ReadByte();
            }

            _stream.WriteByte(0); // Terminator

        }

        private void WriteByte(int value)
        {
            _stream.WriteByte(Convert.ToByte(value));
        }

        private void WriteShort(int value)
        {
            Byte b0 = Convert.ToByte(value & 0xff);
            _stream.WriteByte(b0);
            Byte b1 = Convert.ToByte((value >> 8) & 0xff);
            _stream.WriteByte(b1);
        }

        private void WriteString(string value)
        {
            byte[] stringBytes = value.ToArray().Select(c => (byte)c).ToArray();
            _stream.Write(stringBytes, 0, stringBytes.Length);
        }

        private void WriteComment(string comment, int repeatCount = 0)
        {
            int xfe21 = 0xfe21; // ApplicationExtensionBlockIdentifier; 
            Byte b0 = Convert.ToByte(xfe21 & 0xff);
            _stream.WriteByte(b0);
            Byte b1 = Convert.ToByte((xfe21 >> 8) & 0xff);
            _stream.WriteByte(b1);
                     
            byte[] bytesComment = Encoding.ASCII.GetBytes(comment);;
            b0 = Convert.ToByte(bytesComment.Length & 0xff);
            _stream.WriteByte(b0);
            b1 = Convert.ToByte((bytesComment.Length >> 8) & 0xff);
            _stream.WriteByte(b1);

            _stream.Write(bytesComment, 0, bytesComment.Length);

            // _stream.WriteByte(3); // Application block length
            // _stream.WriteByte(1);
            // TODO: fix it
            // repeatCount = 0;
            // b0 = Convert.ToByte(0x0 & 0xff);
            // _stream.WriteByte(b0);
            // b1 = Convert.ToByte((0x0 >> 8) & 0xff);
            // _stream.WriteByte(b1);

            // _stream.WriteByte(Convert.ToByte(0x0));

            // WriteByte(0); // terminator

        }

        public void Flush()
        {
            // Complete File
            WriteByte(FileTrailer);

            // Pushing data
            _stream.Flush();
        }

        public void Dispose()
        {
            // Complete File
            WriteByte(FileTrailer);

            // Pushing data
            _stream.Flush();
        }
    }
}