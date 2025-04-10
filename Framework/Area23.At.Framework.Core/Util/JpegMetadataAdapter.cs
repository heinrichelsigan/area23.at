using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Windows.Media.Imaging;

namespace Area23.At.Framework.Core.Util
{
    public class JpegMetadataAdapter
    {
        private readonly string path;
        private BitmapFrame frame;        
        public BitmapMetadata Metadata;

        public JpegMetadataAdapter(string path)
        {
            this.path = path;
            frame = GetBitmapFrame(path);
            Metadata = (BitmapMetadata)frame.Metadata.Clone();
        }

        public JpegMetadataAdapter(string path, Stream stream)
        {
            this.path = path;
            frame = GetBitmapFrame(stream);
            Metadata = (BitmapMetadata)frame.Metadata.Clone();
        }

        public void Save()
        {
            SaveAs(path);
        }

        public void SaveAs(string path)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(frame, frame.Thumbnail, Metadata, frame.ColorContexts));
            using (Stream stream = File.Open(path, FileMode.Create, FileAccess.ReadWrite))
            {
                encoder.Save(stream);
            }
        }

        private BitmapFrame GetBitmapFrame(string path)
        {
            BitmapDecoder decoder = null;
            using (Stream stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            }
            return decoder.Frames[0];
        }

        private BitmapFrame GetBitmapFrame(Stream stream)
        {
            BitmapDecoder? decoder = null;
            decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.None);
            return decoder.Frames[0];
        }
    }
}