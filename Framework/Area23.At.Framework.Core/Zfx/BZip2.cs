using Area23.At.Framework.Core.Util;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Zfx
{

    public static class BZip2
    {
        public static byte[] BZip(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Flush();
            msIn.Seek(0, SeekOrigin.Begin);

            MemoryStream msOut = new MemoryStream();

            ICSharpCode.SharpZipLib.BZip2.BZip2.Compress(msIn, msOut, false, 4);

            msOut.Flush();
            byte[] zipBytes = msOut.ToByteArray();

            msOut.Close();
            msOut.Dispose();
            msIn.Close();
            msIn.Dispose();

            return zipBytes;
        }

        public static byte[] BZipViaStream(byte[] inBytes)
        {
            int buflen = Math.Max(inBytes.Length, 4096);
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Seek(0, SeekOrigin.Begin);

            MemoryStream msOut = new MemoryStream();
            using (BZip2OutputStream bzOut = new BZip2OutputStream(msOut))
            {
                StreamUtils.Copy(msIn, bzOut, new byte[buflen]);
            }

            msIn.Close();
            msIn.Dispose();

            msOut.Flush();
            byte[] zipBytes = msOut.ToArray();
            msOut.Close();
            msOut.Dispose();

            return zipBytes;
        }

        public static byte[] BUnZip(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Flush();
            msIn.Seek(0, SeekOrigin.Begin);

            MemoryStream msOut = new MemoryStream();

            ICSharpCode.SharpZipLib.BZip2.BZip2.Decompress(msIn, msOut, false);

            msOut.Flush();
            byte[] unZipBytes = msOut.ToByteArray();

            msOut.Close();
            msOut.Dispose();
            msIn.Close();
            msIn.Dispose();

            return unZipBytes;
        }


        public static byte[] BUnZipViaStream(byte[] inBytes)
        {
            int buflen = Math.Max(inBytes.Length, 4096);
            MemoryStream msIn = new MemoryStream(inBytes);
            msIn.Seek(0, SeekOrigin.Begin);

            MemoryStream msOut = new MemoryStream();
            using (BZip2InputStream bzIn = new BZip2InputStream(msIn))
            {
                StreamUtils.Copy(bzIn, msOut, new byte[buflen]);
            }


            msOut.Flush();
            byte[] unZipBytes = msOut.ToArray();

            msIn.Close();
            msIn.Dispose();

            msOut.Close();
            msOut.Dispose();

            return unZipBytes;
        }

    }

}
