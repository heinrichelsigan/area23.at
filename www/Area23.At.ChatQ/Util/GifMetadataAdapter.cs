using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Area23.At.ChatQ.Util
{
    public class GifMetadataAdapter 
    {
        private readonly string path;
        private readonly Stream stream;
        private BitmapFrame frame;
        public BitmapMetadata Metadata;

        public GifMetadataAdapter(string _path)
        {
            this.path = _path;
            frame = GetBitmapFrame(path);
            Metadata = (BitmapMetadata)frame.Metadata.Clone();            
        }

        public GifMetadataAdapter(Stream _stream)
        {
            this.stream = _stream;
            frame = GetBitmapFrame(stream);
            Metadata = (BitmapMetadata)GetBitmapMetadata(stream).Clone();
        }

        public GifMetadataAdapter(string _path, Stream _stream) : this(_path)  { this.stream = _stream; }

        public GifMetadataAdapter(Stream _stream, string _path) : this(_stream) { this.path= _path; }

        public void Save() { SaveAs(path); }

        public void SaveAs(string path)
        {
            GifBitmapEncoder encoder = new GifBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(frame, frame.Thumbnail, Metadata, frame.ColorContexts));
            using (Stream stream = File.Open(path, FileMode.Create, FileAccess.ReadWrite))
            {
                encoder.Save(stream);
            }
        }

        private BitmapFrame GetBitmapFrame(string path)
        {
            GifBitmapDecoder decoder = null;
            using (Stream stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            }
            return decoder.Frames[0];
        }

        private BitmapFrame GetBitmapFrame(Stream stream)
        {
            GifBitmapDecoder decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.None);
            return decoder.Frames.First();
        }

        private BitmapMetadata GetBitmapMetadata(Stream stream)
        {
            GifBitmapDecoder decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
            return decoder.Metadata;
        }

    }
}