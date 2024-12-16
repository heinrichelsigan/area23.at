using DBTek.Crypto;
using ICSharpCode.SharpZipLib.BZip2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Zfx
{
    public static class BZip2
    {
        public static byte[] BZip(byte[] inBytes)
        {           
            MemoryStream msIn = new MemoryStream(inBytes);
            MemoryStream msOut = new MemoryStream();

            ICSharpCode.SharpZipLib.BZip2.BZip2.Compress(msIn, msOut, false, 6);


            byte[] outBytes = new byte[msOut.Length];
            byte[] zipBytes = msOut.ToByteArray();
            int pos = 0, read = (int)msOut.Length;
            ;
            while (read > 0)
            {
                read = msOut.Read(outBytes, pos, count: outBytes.Length);                
                pos += read;
            }

            msOut.Close();
            msOut.Dispose();
            msIn.Close();            

            return outBytes;
        }

        public static byte[] BUnZip(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Seek(0, SeekOrigin.Begin);            

            MemoryStream msOut = new MemoryStream();

            ICSharpCode.SharpZipLib.BZip2.BZip2.Decompress(msIn, msOut, false);

            byte[] outBytes = new byte[msOut.Length];
            byte[] unzipBytes = msOut.ToByteArray();
            int pos = 0, read = (int)msOut.Length;
            ;
            while (read > 0)
            {
                read = msOut.Read(outBytes, pos, count: outBytes.Length);
                pos += read;
            }
            
            msIn.Close();
            msOut.Close();

            return outBytes;
        }
    }
}
