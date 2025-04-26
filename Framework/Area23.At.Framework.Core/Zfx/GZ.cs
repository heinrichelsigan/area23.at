using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Zfx
{

    /// <summary>
    /// abstraction of gnu zip gzip compression & decompression
    /// </summary>
    public static class GZ
    {
        const int BUFSZE = 1024;

        #region gzip compression

        /// <summary>
        /// GZip directly, please use <see cref="GZipViaStream(byte[])"/>
        /// </summary>
        /// <param name="inBytes"><see cref="byte[]"/> inBytes</param>
        /// <param name="compressionLevel">level of compression: 
        ///  1  ... for at least no compression, 
        /// 4,5 ... for average compression
        ///  9  ... for strongest bzip2 compression, generating smallest most compact output 
        /// </param>
        /// <returns><see cref="byte[]"/> outbytes</returns>
        public static byte[] GZip(byte[] inBytes, int compressionLevel = 6)
        {
            byte[]? zipBytes = null;
            MemoryStream msIn = new MemoryStream(inBytes, 0, inBytes.Length);
            msIn.Seek(0, SeekOrigin.Begin);

            using (MemoryStream msOut = new MemoryStream())
            {
                ICSharpCode.SharpZipLib.GZip.GZip.Compress(msIn, msOut, true, BUFSZE, compressionLevel);
                msOut.Flush();
                zipBytes = msOut.ToByteArray();
            }

            return zipBytes;
        }

        public static byte[] GZipBytes(byte[] inBytes)
        {
            byte[]? zipBytes = null;
            using (MemoryStream memIn = new MemoryStream(inBytes, 0, inBytes.Length))
            {
                MemoryStream memOut = GZipStream(memIn);
                zipBytes = memOut.ToByteArray();
            }

            return zipBytes;
        }

        /// <summary>
        /// GZipStream 
        /// </summary>
        /// <param name="inMem"><see cref="MemoryStream"/> inMem</param>
        /// <returns><see cref="MemoryStream"/> outMem</returns>
        public static MemoryStream GZipStream(MemoryStream inMem)
        {
            MemoryStream memOut = new MemoryStream();
            // using (GZipOutputStream gzOut = new GZipOutputStream(msOut, buflen))
            using (GZipStream gzOut = new GZipStream(memOut, CompressionMode.Compress, false))
            {
                StreamUtils.Copy(inMem, gzOut, new byte[BUFSZE]);
            }
            memOut.Flush();

            return memOut;
        }

        #endregion gzip compression


        #region gunzip decompression

        /// <summary>
        /// Please use <see cref="GUnZipViaStream(byte[])"/>
        /// </summary>
        /// <param name="inBytes"><see cref="byte[]"/> inBytes</param>
        /// <returns><see cref="byte[]"/> outbytes</returns>
        public static byte[] GUnZip(byte[] inBytes)
        {
            byte[]? unZipBytes = null;

            using (MemoryStream msIn = new MemoryStream(inBytes, 0, inBytes.Length))
            {
                msIn.Seek(0, SeekOrigin.Begin);
                using (MemoryStream msOut = new MemoryStream())
                {
                    ICSharpCode.SharpZipLib.GZip.GZip.Decompress(msIn, msOut, false);
                    // msOut.Flush();
                    msOut.Seek(0, SeekOrigin.Begin);

                    unZipBytes = msOut.ToByteArray();
                }
            }

            return unZipBytes;
        }


        /// <summary>
        /// GUnZipViaStream
        /// </summary>
        /// <param name="inBytes"><see cref="byte[]"/> inBytes</param>
        /// <returns><see cref="byte[]"/> outbytes</returns>
        public static byte[] GUnZipBytes(byte[] inBytes)
        {
            byte[]? unZipBytes = null;

            using (MemoryStream msIn = new MemoryStream(inBytes, 0, inBytes.Length))
            {
                using (MemoryStream msOut = new MemoryStream())
                {
                    // using (GZipInputStream gzIn = new GZipInputStream(msIn))
                    using (GZipStream gzIn = new GZipStream(msIn, CompressionMode.Decompress, false))
                    {
                        StreamUtils.Copy(gzIn, msOut, new byte[BUFSZE]);
                    }

                    // msOut.Flush();
                    unZipBytes = msOut.ToByteArray();
                }
            }

            return unZipBytes;
        }


        public static MemoryStream GUnZipStream(MemoryStream memIn)
        {
            MemoryStream memOut = new MemoryStream();
            // using (GZipInputStream gzIn = new GZipInputStream(msIn))
            using (GZipStream gzIn = new GZipStream(memOut, CompressionMode.Decompress, false))
            {
                StreamUtils.Copy(gzIn, memOut, new byte[BUFSZE]);
            }
            memOut.Flush();

            return memOut;
        }

        #endregion gunzip decompression


        /// <summary>
        /// 
        /// </summary>
        /// <param name="inFile"></param>

        /// <summary>
        /// GzFile gzips or gunzips a file
        /// </summary>
        /// <param name="inFile"><see cref="string">full (unc) filepath to file</see></param>
        /// <param name="outMessage">string with information, what happend</param>
        /// <param name="outFile"><see cref="string"/>full (unc) filepath to outfile,
        /// if keept empty, .gz will be added after compression and .gz will be removed after decompressing gzip'd file.</param>
        /// <param name="zip"><see cref="bool">(bool)true for gzip, (bool)false for gunzip</see></param>
        /// <param name="compressionLevel">level of compression: 
        ///  1  ... for at least no compression, 
        /// 4,5 ... for average compression
        ///  9  ... for strongest bzip2 compression, generating smallest most compact output 
        /// </param>
        /// <returns><see cref="string">string name of processed (gzipped/gunzipped) file</see></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool GzFile(string inFile, out string outMessage, string outFile = "", bool zip = true, int compressionLevel = 6)
        {
            if (string.IsNullOrEmpty(inFile) || !File.Exists(inFile))
                throw new ArgumentNullException("string GzFile(string inFile, bool zip = true) => inFile is either null or empty or doesn't exist!");

            outMessage = String.Format("{0}: {1} {2} ... ",
                DateTime.Now.Area23DateTimeWithSeconds(),
                (zip ? $"gzip " : $"gunzip "),
                Path.GetFileName(inFile));

            byte[] inBytes = File.ReadAllBytes(inFile);
            byte[] outBytes = zip ? GZipBytes(inBytes) : GUnZipBytes(inBytes);
            if (string.IsNullOrEmpty(outFile))
                outFile = zip ? inFile + ".gz" : inFile.EndsWith(".gz") ?
                    inFile.Replace(".gz", "") : DateTime.Now.ToString("yy-MM-dd_") + inFile;
            File.WriteAllBytes(outFile, outBytes);

            FileInfo fi = new FileInfo(outFile);
            if (fi.Exists)
            {
                outMessage += $"created file {fi.Name} length={fi.Length} at {fi.CreationTime.ToShortDateString()} {fi.CreationTime.ToShortTimeString()}\nin directory {fi.DirectoryName}";
                return true;
            }
            else
                outMessage += $"=> file {outFile} NOT created; something went wrong!";

            return false;
        }

    }



}
