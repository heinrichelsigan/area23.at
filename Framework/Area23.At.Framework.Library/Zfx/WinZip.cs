using Area23.At.Framework.Library.Static;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace Area23.At.Framework.Library.Zfx
{
    public static class WinZip
    {
        public static byte[] Zip(byte[] inBytes, string entryName = "")
        {
            int buflen = (inBytes == null || inBytes.Length < 256) ? 256 : (inBytes.Length > 4096) ? 4096 : inBytes.Length;

            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Flush();
            msIn.Seek(0, SeekOrigin.Begin);
            MemoryStream msOut = new MemoryStream();

            string zipEntryName = (string.IsNullOrEmpty(entryName) ? DateTime.Now.Area23DateTimeWithMillis() + "CoolCrypt.txt" : entryName);
            ZipOutputStream zipOut = new ZipOutputStream(msOut);
            zipOut.UseZip64 = UseZip64.Off;
            ZipEntry newEntry = new ZipEntry(zipEntryName);
            newEntry.DateTime = DateTime.Now;
            zipOut.PutNextEntry(newEntry);
            StreamUtils.Copy(msIn, zipOut, new byte[buflen]);
            zipOut.CloseEntry();
            zipOut.IsStreamOwner = false;
            zipOut.Close();
            
            msOut.Seek(0, SeekOrigin.Begin);
            byte[] zipBytes = msOut.ToByteArray();

            msOut.Close();
            msOut.Dispose();
            msIn.Close();
            msIn.Dispose();

            return zipBytes;
        }

        public static byte[] UnZip(byte[] inBytes)
        {
            int buflen = (inBytes == null || inBytes.Length < 256) ? 256 : (inBytes.Length > 4096) ? 4096 : inBytes.Length;

            MemoryStream msIn = new MemoryStream(inBytes);
            msIn.Seek(0, SeekOrigin.Begin);
            MemoryStream msOut = new MemoryStream();

            ZipEntry entry = null;
            using (ZipInputStream zipIn = new ZipInputStream(msIn))                 
            {
                if (entry == null)
                    entry = zipIn.GetNextEntry();
                StreamUtils.Copy(zipIn, msOut, new byte[buflen]);
            }
            msOut.Seek(0, SeekOrigin.Begin);
            byte[] unZipBytes = msOut.ToByteArray();

            msOut.Close();
            msOut.Dispose();
            msIn.Close();
            msIn.Dispose();

            return unZipBytes;           
        }

    }

}
