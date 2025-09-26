using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

namespace Area23.At.Framework.Library.Zfx
{
    public static class WinZip
    {
        /// <summary>
        /// Zip zips a byte array and returns the zipped byte array.
        /// </summary>
        /// <param name="inBytes">input byte[] array</param>
        /// <returns>compressed byte[] array</returns>
        public static byte[] Zip(byte[] inBytes)
        {
            int buflen = (inBytes == null || inBytes.Length < 256) ? 256 : (inBytes.Length > 4096) ? 4096 : inBytes.Length;

            try
            {
                MemoryStream msIn = new MemoryStream(inBytes);
                MemoryStream msOut = new MemoryStream();
                byte[] outBytes = new byte[buflen];

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
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("WinZip", "Zip", ex);
            }
            return inBytes;
        }

        /// <summary>
        /// Unzip unzips a zip compressed byte array and returns the unzipped byte array.
        /// </summary>
        /// <param name="inBytes">compressed byte[] array</param>
        /// <returns>unzipped byte[] array</returns>
        public static byte[] UnZip(byte[] inBytes)
        {
            try
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
            catch (Exception ex)
            {
                Area23Log.LogOriginMsgEx("WinZip", "Zip", ex);
            }
            return inBytes;
        }

    }

}
