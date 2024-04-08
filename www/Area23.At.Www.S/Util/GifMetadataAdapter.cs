using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Area23.At.Www.S.Util
{
    public class GifMetadataAdapter 
    {
        private readonly string path;
        private BitmapFrame frame;
        public BitmapMetadata Metadata;

        public GifMetadataAdapter(string path)
        {
            this.path = path;
            frame = getBitmapFrame(path);
            Metadata = (BitmapMetadata)frame.Metadata.Clone();            
        }

        public GifMetadataAdapter(string path, Stream stream)
        {
            this.path = path;
            frame = getBitmapFrame(stream);
            Metadata = (BitmapMetadata)getBitmapMetadata(stream).Clone();
            // System.Windows.Freezable fr = (Freezable)getBitmapMetadata(stream).Clone().GetAsFrozen();
        }

        public void Save()
        {
            SaveAs(path);
        }

        public void SaveAs(string path)
        {
            GifBitmapEncoder encoder = new GifBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(frame, frame.Thumbnail, Metadata, frame.ColorContexts));
            using (Stream stream = File.Open(path, FileMode.Create, FileAccess.ReadWrite))
            {
                encoder.Save(stream);
            }
        }

        private BitmapFrame getBitmapFrame(string path)
        {
            GifBitmapDecoder decoder = null;
            using (Stream stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
            }
            return decoder.Frames[0];
        }

        private BitmapFrame getBitmapFrame(Stream stream)
        {
            GifBitmapDecoder decoder = null;
            decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);            
            return decoder.Frames.First();
        }

        private BitmapMetadata getBitmapMetadata(Stream stream)
        {
            GifBitmapDecoder decoder = null;
            decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
            return decoder.Metadata;
        }

    }
}