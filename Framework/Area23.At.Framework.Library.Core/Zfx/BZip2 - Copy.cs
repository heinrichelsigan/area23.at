using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Zfx
{
    public static class WinZip
    {
        public static byte[] Zip(byte[] inByte)
        {
            byte[] outByte = new byte[inByte.Length * 2];
            /*
            MemoryStream msin = new MemoryStream(inByte);
            MemoryStream msout = new MemoryStream(outByte);
            ICSharpCode.SharpZipLib.Zip.ZipInputStream zIn = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(msin);
            ICSharpCode.SharpZipLib.Zip.ZipOutputStream zOut = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(msout);
            // ICSharpCode.SharpZipLib.Zip..BZip2.Compress(bzIn, bzOut, true, 6);

            byte[] zipBytes = msout.ToByteArray();
            */
            return outByte;
        }

        public static byte[] UnZip(byte[] inByte)
        {
            byte[] outByte = new byte[inByte.Length * 2];
            /*
            MemoryStream msin = new MemoryStream(inByte);
            MemoryStream msout = new MemoryStream(outByte);
            ICSharpCode.SharpZipLib.BZip2.BZip2InputStream bzIn = new ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(msin);
            ICSharpCode.SharpZipLib.BZip2.BZip2OutputStream bzOut = new ICSharpCode.SharpZipLib.BZip2.BZip2OutputStream(msout);
            ICSharpCode.SharpZipLib.BZip2.BZip2.Decompress(bzIn, bzOut, true);

            byte[] unzipBytes = msout.ToByteArray();
            */
            return outByte;
        }
    }
}
