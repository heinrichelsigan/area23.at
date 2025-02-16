using Area23.At.Framework.Library.Util;
using DBTek.Crypto;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using ICSharpCode.SharpZipLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Core;
using System.Runtime.InteropServices.ComTypes;
using System.IO.Compression;

namespace Area23.At.Framework.Library.Zfx
{

    /// <summary>
    /// abstraction of gnu zip gzip compression & decompression
    /// </summary>
    public static class GZ
    {

        /// <summary>
        /// GZip directly, please use <see cref="GZipViaStream(byte[])"/>
        /// </summary>
        /// <param name="inBytes"><see cref="byte[]"/> inBytes</param>
        /// <returns><see cref="byte[]"/> outbytes</returns>
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


        /// <summary>
        /// GZipViaStream 
        /// </summary>
        /// <param name="inBytes"><see cref="byte[]"/> inBytes</param>
        /// <returns><see cref="byte[]"/> outbytes</returns>
        public static byte[] GZipViaStream(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);            
            msIn.Flush();
            msIn.Seek(0, SeekOrigin.Begin);

            MemoryStream msOut = new MemoryStream();
            int buflen = Math.Max(inBytes.Length, 4096);


            // using (GZipOutputStream gzOut = new GZipOutputStream(msOut, buflen))
            using (GZipStream gzOut = new GZipStream(msOut, CompressionMode.Compress, false))
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

        /// <summary>
        /// Please use <see cref="GUnZipViaStream(byte[])"/>
        /// </summary>
        /// <param name="inBytes"><see cref="byte[]"/> inBytes</param>
        /// <returns><see cref="byte[]"/> outbytes</returns>
        public static byte[] GUnZip(byte[] inBytes)
        {
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Flush();
            msIn.Seek(0, SeekOrigin.Begin);

            MemoryStream msOut = new MemoryStream();

            ICSharpCode.SharpZipLib.GZip.GZip.Decompress(msIn, msOut, false);

            // msOut.Flush();
            // msOut.Seek(0, SeekOrigin.Begin);
            // msOut.Flush();

            byte[] unZipBytes = msOut.ToByteArray();

            msOut.Close();
            msOut.Dispose();
            msIn.Close();
            msIn.Dispose();

            return unZipBytes;
        }


        /// <summary>
        /// GUnZipViaStream
        /// </summary>
        /// <param name="inBytes"><see cref="byte[]"/> inBytes</param>
        /// <returns><see cref="byte[]"/> outbytes</returns>
        public static byte[] GUnZipViaStream(byte[] inBytes)
        {
            int buflen = Math.Max(inBytes.Length * 2, 4096);
            MemoryStream msIn = new MemoryStream();
            msIn.Write(inBytes, 0, inBytes.Length);
            msIn.Flush();
            msIn.Seek(0, SeekOrigin.Begin);
            
            MemoryStream msOut = new MemoryStream();

            // using (GZipInputStream gzIn = new GZipInputStream(msIn))
            using (GZipStream gzIn = new GZipStream(msIn, CompressionMode.Decompress, false))
            {
                StreamUtils.Copy(gzIn, msOut, new byte[buflen]);
            }
            
            // msOut.Flush();
            byte[] unZipBytes = msOut.ToByteArray();

            msOut.Close();
            msOut.Dispose();
            msIn.Close();
            msIn.Dispose();

            return unZipBytes;
        }

    }

}
