using DBTek.Crypto;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
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
        public static byte[] GZip(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Seek(0, SeekOrigin.Begin);
            MemoryStream msOut = new MemoryStream();            

            ICSharpCode.SharpZipLib.GZip.GZip.Compress(msIn, msOut, false, 1024, 6);

            byte[] zipBytes = msOut.ToByteArray();
            byte[] outBytes = new byte[msOut.Length];
            int pos = 0, read = (int)msOut.Length;
            ;
            while (read > 0)
            {
                read = msOut.Read(zipBytes, pos, count: outBytes.Length);
                pos += read;
            }

            msIn.Close();
            msIn.Dispose();
            msOut.Close();
            msOut.Dispose();

            return outBytes;
        }

        public static byte[] GUnZip(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Seek(0, SeekOrigin.Begin);
            GZipInputStream gin = new GZipInputStream(msIn);


            MemoryStream msOut = new MemoryStream();


            ICSharpCode.SharpZipLib.GZip.GZip.Decompress(gin, msOut, false);


            byte[] outBytes = msOut.ToByteArray();
            byte[] unzipBytes = msOut.ToByteArray();
            int pos = 0, read = (int)msOut.Length;
            ;
            while (read > 0)
            {
                read = msOut.Read(outBytes, pos, count: 1024);
                pos += read;
            }
            
            msIn.Close();
            msIn.Dispose();
            msOut.Close();

            return outBytes;
        }
    }
}
