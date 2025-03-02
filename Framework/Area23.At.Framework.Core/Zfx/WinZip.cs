using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Zfx
{
    public static class WinZip
    {
        public static byte[] Zip(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Flush();
            msIn.Seek(0, SeekOrigin.Begin);
            MemoryStream msOut = new MemoryStream();

            using (ZipOutputStream zipOut = new ZipOutputStream(msOut))
            {
                StreamUtils.Copy(msIn, zipOut, new byte[inBytes.Length]);
            }
            msOut.Flush();
            byte[] zipBytes = msOut.ToByteArray();

            msOut.Close();
            msOut.Dispose();
            msIn.Close();
            msIn.Dispose();

            return zipBytes;
        }

        public static byte[] UnZip(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream(inBytes);
            msIn.Seek(0, SeekOrigin.Begin);
            MemoryStream msOut = new MemoryStream();

            using (ZipInputStream zipIn = new ZipInputStream(msIn))
            {
                StreamUtils.Copy(zipIn, msOut, new byte[inBytes.Length]);
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
