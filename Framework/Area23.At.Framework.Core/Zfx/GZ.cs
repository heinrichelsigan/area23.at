using Area23.At.Framework.Core.Util;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Zfx
{
    public static class GZ
    {
        public static byte[] GZip(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Flush();
            msIn.Seek(0, SeekOrigin.Begin);

            MemoryStream msOut = new MemoryStream();

            ICSharpCode.SharpZipLib.GZip.GZip.Compress(msIn, msOut, false);

            msOut.Flush();
            byte[] zipBytes = msOut.ToByteArray();

            msOut.Close();
            msOut.Dispose();
            msIn.Close();
            msIn.Dispose();

            return zipBytes;
        }

        public static byte[] GZipViaStream(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Flush();
            msIn.Seek(0, SeekOrigin.Begin);

            MemoryStream msOut = new MemoryStream();
            int buflen = Math.Max(inBytes.Length, 4096);

            using (GZipOutputStream gzOut = new GZipOutputStream(msOut))
            {
                StreamUtils.Copy(msIn, gzOut, new byte[buflen]);
            }

            msOut.Flush();
            byte[] zipBytes = msOut.ToByteArray();

            msOut.Close();
            msOut.Dispose();
            msIn.Close();
            msIn.Dispose();

            return zipBytes;
        }

        public static byte[] GUnZip(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Flush();
            msIn.Seek(0, SeekOrigin.Begin);

            MemoryStream msOut = new MemoryStream();

            ICSharpCode.SharpZipLib.GZip.GZip.Decompress(msIn, msOut, false);

            msOut.Flush();
            byte[] unZipBytes = msOut.ToByteArray();

            msOut.Close();
            msOut.Dispose();
            msIn.Close();
            msIn.Dispose();

            return unZipBytes;
        }

        public static byte[] GUnZipViaStream(byte[] inBytes)
        {
            int buflen = Math.Max(inBytes.Length * 2, 4096);
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Flush();
            msIn.Seek(0, SeekOrigin.Begin);

            MemoryStream msOut = new MemoryStream();
            using (GZipInputStream gzIn = new GZipInputStream(msIn))
            {
                StreamUtils.Copy(gzIn, msOut, new byte[buflen]);
            }

            msOut.Flush();
            byte[] unZipBytes = msOut.ToByteArray();

            msOut.Close();
            msOut.Dispose();
            msIn.Close();
            msIn.Dispose();

            return unZipBytes;
        }

    }

}
