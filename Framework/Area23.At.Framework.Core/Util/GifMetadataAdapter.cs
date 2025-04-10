using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Area23.At.Framework.Core.Util
{

    public class GifMetadataAdapter
    {
        private readonly string? path;
        private readonly Stream stream;
        private BitmapFrame frame;
        public BitmapMetadata Metadata;

        public GifMetadataAdapter(string _path)
        {
            this.path = _path;
            frame = getBitmapFrame(path);
            stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            Metadata = (BitmapMetadata)frame.Metadata.Clone();
        }

        public GifMetadataAdapter(Stream _stream)
        {
            this.stream = _stream;
            frame = getBitmapFrame(stream);
            Metadata = (BitmapMetadata)getBitmapMetadata(stream).Clone();
        }

        public GifMetadataAdapter(string _path, Stream _stream) : this(_path) { this.stream = _stream; }

        public GifMetadataAdapter(Stream _stream, string _path) : this(_stream) { this.path = _path; }

        public void Save() { if (path != null) SaveAs(path); }

        public void SaveAs(string path)
        {
            GifBitmapEncoder encoder = new GifBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(frame, frame.Thumbnail, Metadata, frame.ColorContexts));
            using (Stream stream = File.Open(path, FileMode.Create, FileAccess.ReadWrite))
            {
                encoder?.Save(stream);
            }
        }

        private BitmapFrame getBitmapFrame(string path)
        {
            GifBitmapDecoder? decoder = null;
            using (Stream stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            }
            return decoder.Frames[0];
        }

        private BitmapFrame getBitmapFrame(Stream stream)
        {
            GifBitmapDecoder decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.None);
            return decoder.Frames.First();
        }

        private BitmapMetadata getBitmapMetadata(Stream stream)
        {
            GifBitmapDecoder decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
            return decoder.Metadata;
        }

    }

}
