using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Area23.At.Framework.Core.Zfx
{
    public static class WinZip
    {
        public static byte[] Zip(byte[] inBytes, string fileName = "bogus.zip", int zipLevel = 6)
        {
            try
            {
                MemoryStream msIn = new MemoryStream(inBytes);
                MemoryStream msOut = new MemoryStream();
                byte[] outBytes = new byte[65536];

                ZipOutputStream zipStream = new ZipOutputStream(msOut);
                zipStream.SetLevel(zipLevel);
                ZipEntry newEntry = new ZipEntry(fileName);
                newEntry.DateTime = DateTime.Now;
                zipStream.PutNextEntry(newEntry);
                StreamUtils.Copy(msIn, zipStream, new byte[4096]);
                zipStream.CloseEntry();
                zipStream.IsStreamOwner = false;
                zipStream.Close();

                msOut.Flush();
                byte[] zipBytes = msOut.ToByteArray();

                msOut.Close();
                msOut.Dispose();

                msIn.Close();
                msIn.Dispose();

                return zipBytes;
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("WinZip", "Zip", ex);
            }
            return inBytes;
        }

        public static byte[] UnZip(byte[] inBytes)
        {
            try
            {
                MemoryStream msIn = new MemoryStream(inBytes);
                msIn.Seek(0, SeekOrigin.Begin);
                MemoryStream msOut = new MemoryStream();
                byte[] outBytes = new byte[65536];

                using (ZipInputStream zipIn = new ZipInputStream(msIn, 4096))
                {
                    StreamUtils.Copy(zipIn, msOut, outBytes);
                }
                msOut.Flush();
                byte[] unZipBytes = msOut.ToByteArray();

                msOut.Close();
                msOut.Dispose();
                msIn.Close();
                msIn.Dispose();

                return unZipBytes;
            }
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("WinZip", "Zip", ex);
            }
            return inBytes;
        }

    }


}
