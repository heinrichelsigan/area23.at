using Ionic.Zlib;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Zfx
{
    public static class GZ
    {
        public static byte[] GZip(byte[] inByte)
        {            
            byte[] outByte = new byte[inByte.Length * 2];
            MemoryStream msin = new MemoryStream(inByte);
            MemoryStream msout = new MemoryStream(outByte);
            ICSharpCode.SharpZipLib.GZip.GZipInputStream gzIn = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(msin, 1024);
            ICSharpCode.SharpZipLib.GZip.GZipOutputStream gzOut = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(msout, 1024);
            ICSharpCode.SharpZipLib.GZip.GZip.Compress(gzIn, gzOut, true, 6);

            byte[] zipBytes = msout.ToByteArray();

            return zipBytes;
        }

        public static byte[] GUnZip(byte[] inByte)
        {
            byte[] outByte = new byte[inByte.Length * 2];
            MemoryStream msin = new MemoryStream(inByte);
            MemoryStream msout = new MemoryStream(outByte);
            ICSharpCode.SharpZipLib.GZip.GZipInputStream gzIn = new ICSharpCode.SharpZipLib.GZip.GZipInputStream(msin, 1024);
            ICSharpCode.SharpZipLib.GZip.GZipOutputStream gzOut = new ICSharpCode.SharpZipLib.GZip.GZipOutputStream(msout, 1024);
            ICSharpCode.SharpZipLib.BZip2.BZip2.Decompress(gzIn, gzOut, true);

            byte[] unzipBytes = msout.ToByteArray();

            return unzipBytes;
        }
    }
}
