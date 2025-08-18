using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Zfx
{
    [DefaultValue(None)]
    public enum ZipType
    {
        None = 0x0,
        Zip = 0x1,
        GZip = 0x2,
        BZip2 = 0x3,
        Z7 = 0x4
    }


    public static class ZipTypeExtensions
    {
        /// <summary>
        /// Generic zip extension method for <see cref="ZipType"/>
        /// </summary>
        /// <param name="inBytes"></param>
        /// <returns>zipped bytes</returns>
        public static byte[] Zip(this ZipType zipType, byte[] inBytes)
        {
            if (inBytes == null || inBytes.Length == 0)
                throw new InvalidOperationException("byte[] Zip(this ZipType zipType, byte[] inBytes = NULL)");

            switch (zipType)
            {
                case ZipType.BZip2:
                    return BZip2.BZip(inBytes);
                case ZipType.GZip:
                    return GZ.GZipBytes(inBytes);
                case ZipType.Zip:
                    return WinZip.Zip(inBytes);
                case ZipType.Z7: // TODO
                case ZipType.None:
                    return inBytes;
                default: // Asset(0)
                    break;
            }

            return new byte[0];
        }

        /// <summary>
        /// Generic unzip extension method for <see cref="ZipType"/>
        /// </summary>
        /// <param name="zipType">this the <see cref="ZipType"/>/param>
        /// <param name="compressedBytes"></param>
        /// <returns>decompressed bytes</returns>
        public static byte[] Unzip(this ZipType zipType, byte[] compressedBytes)
        {
            if (compressedBytes == null || compressedBytes.Length == 0)
                throw new InvalidOperationException("byte[] Unzip(this ZipType zipType, byte[] compressedBytes = NULL)");

            switch (zipType)
            {
                case ZipType.BZip2:
                    return BZip2.BUnZip(compressedBytes);
                case ZipType.GZip:
                    return GZ.GUnZipBytes(compressedBytes);
                case ZipType.Zip:
                    return WinZip.UnZip(compressedBytes);
                case ZipType.Z7: // TODO
                case ZipType.None:
                    return compressedBytes;
                default: // Asset(0)
                    break;
            }

            return new byte[0];
        }
    }
    
}
