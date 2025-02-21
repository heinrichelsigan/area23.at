using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Documents;

// https://github.com/DataDink/Bumpkit/blob/master/BumpKit/BumpKit/GifEncoder.cs

namespace Area23.At.Framework.Core.Util
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
        private bool _isFinished = false;
        private int? _repeatCount;
        TimeSpan _frameDelay;
        private List<byte> _byteList;
        
        // Public Accessors
        public TimeSpan FrameDelay { get => _frameDelay; }
        public byte[] GifData
        {
            get
            {
                if (!_isFinished) Finish();
                return _byteList.ToArray();
            }
        }



        /// <summary>
        /// Encodes multiple images as an animated gif to a stream. <br />
        /// ALWAYS ALWAYS ALWAYS wire this in a using block <br />
        /// Disposing the encoder will complete the file. <br />
        /// Uses default .net GIF encoding and adds animation headers.
        /// </summary>
        /// <param name="stream">The stream that will be written to.</param>
        /// <param name="width">Sets the width for this gif or null to use the first frame's width.</param>
        /// <param name="height">Sets the height for this gif or null to use the first frame's height.</param>
        public GifEncoder(Image img, int? repeatCount = null, TimeSpan? frameDelay = null)
        {
            _byteList = new List<byte>();            
            _repeatCount = repeatCount;
            _frameDelay = frameDelay.GetValueOrDefault();
            _isFinished = false;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Gif);
                InitHeader(ms, img.Width, img.Height);
                WriteGraphicControlBlock(ms, FrameDelay);
                WriteImageBlock(ms, !_isFirstImage, 0, 0, img.Width, img.Height);
            }
            _isFirstImage = false;
        }

        /// <summary>
        /// Adds a frame to this animation.
        /// </summary>
        /// <param name="img">The image to add</param>
        /// <param name="frameDelay">TimeSpan for delay</param>
        public void AddFrame(Image img, TimeSpan? frameDelay = null)
        {
            if (_isFinished)
                return;

            _frameDelay = frameDelay.GetValueOrDefault();
            using (var gifStream = new MemoryStream())
            {
                img.Save(gifStream, ImageFormat.Gif);
                if (_isFirstImage) // Steal the global color table info
                {
                    InitHeader(gifStream, img.Width, img.Height);
                }
                WriteGraphicControlBlock(gifStream, FrameDelay);
                WriteImageBlock(gifStream, !_isFirstImage, 0, 0, img.Width, img.Height);
            }
            _isFirstImage = false;
        }

        private void InitHeader(Stream sourceGif, int w, int h)
        {
            if (_isFinished)
                return;

            // File Header
            WriteString(FileType);
            WriteString(FileVersion);
            WriteShort(w); // Initial Logical Width
            WriteShort(h); // Initial Logical Height
            sourceGif.Position = SourceGlobalColorInfoPosition;
            WriteByte(sourceGif.ReadByte()); // Global Color Table Info
            WriteByte(0); // Background Color Index
            WriteByte(0); // Pixel aspect ratio
            WriteColorTable(sourceGif);

            // App Extension Header
            WriteShort(ApplicationExtensionBlockIdentifier); // 0xff21            
            WriteByte(ApplicationBlockSize);
            WriteString(ApplicationIdentification);            
            WriteByte(3); // Application block length
            WriteByte(1);            
            WriteShort(_repeatCount.GetValueOrDefault(0)); // Repeat count for images.
            WriteByte(0); // terminator
        }

        private void WriteColorTable(Stream sourceGif)
        {
            if (_isFinished)
                return;

            sourceGif.Position = SourceColorBlockPosition; // Locating the image color table
            byte[] colorTable = new byte[SourceColorBlockLength];
            sourceGif.Read(colorTable, 0, colorTable.Length);
            WriteBytes(colorTable, colorTable.Length);
        }

        private void WriteGraphicControlBlock(Stream sourceGif, TimeSpan frameDelay)
        {
            if (_isFinished)
                return;

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
            if (_isFinished)
                return;

            sourceGif.Position = SourceImageBlockPosition; // Locating the image block
            byte[] header = new byte[SourceImageBlockHeaderLength];
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
                byte[] imgData = new byte[dataLength];
                sourceGif.Read(imgData, 0, dataLength);

                WriteByte(Convert.ToByte(dataLength));
                WriteBytes(imgData, dataLength);
                dataLength = sourceGif.ReadByte();
            }

            WriteByte(0); // Terminator

        }

        private void WriteByte(int value)
        {
            if (_isFinished)
                return;

            _byteList.Add(Convert.ToByte(value));
        }

        private void WriteShort(int value)
        {
            if (_isFinished)
                return;
            Byte b0 = Convert.ToByte(value & 0xff);
            _byteList.Add(b0);
            Byte b1 = Convert.ToByte((value >> 8) & 0xff);
            _byteList.Add(b1);
        }

        private void WriteString(string value)
        {
            if (_isFinished)
                return;

            byte[] stringBytes = value.ToArray().Select(c => (byte)c).ToArray();
            foreach (byte b in stringBytes)
                _byteList.Add(b);
        }

        private void WriteBytes(byte[] bytes, int length)
        {
            if (_isFinished)
                return;

            if (bytes != null && bytes.Length > 0)
            {
                for (int bc = 0; bc < Math.Min(length, bytes.Length); bc++)
                {
                    _byteList.Add((byte)bytes[bc]);
                }
            }
        }

        private void WriteBytes(byte[] bytes)
        {
            if (_isFinished)
                return;

            if (bytes != null && bytes.Length > 0)
            {
                foreach (byte b in bytes)
                    _byteList.Add(b);
            }
        }
  
        public void Finish()
        {
            // Complete File
            WriteByte(FileTrailer);
            _isFinished = true;
        }

        public void Dispose()
        {
            
        }
    }
}
