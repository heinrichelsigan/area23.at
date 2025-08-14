using Area23.At.Framework.Library.Crypt.EnDeCoding;
using Area23.At.Framework.Library.Static;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Core;
using System;
using System.IO;


namespace Area23.At.Framework.Library.Zfx
{

    /// <summary>
    /// static class BZip2 provides bzip2 and bunzip2 functionality
    /// </summary>
    public static class BZip2
    {

        #region bzip2 compression

        /// <summary>
        /// BZip transfdorms uncompressed <see cref="byte[]">byte[] inBytes</see> to <see cref="byte[]">bzip2 compressed (byte[])bytes</see>
        /// </summary>
        /// <param name="inBytes"> <see cref="byte[]">bytes</see> ready to compress</param>
        /// <param name="compressionLevel">level of compression: 
        ///  1  ... for at least no compression, 
        /// 4,5 ... for average compression
        ///  9  ... for strongest bzip2 compression, generating smallest most compact output 
        /// </param>
        /// <returns><see cref="byte[]?">byte[] bzip2 compressed out bytes</see></returns>
        public static byte[] BZip(byte[] inBytes, int compressionLevel = 9)
        {
            byte[] zipBytes = null;
            MemoryStream msIn = new MemoryStream(inBytes, 0, inBytes.Length);
            using (MemoryStream msOut = new MemoryStream())
            {
                ICSharpCode.SharpZipLib.BZip2.BZip2.Compress(msIn, msOut, true, compressionLevel);
                msOut.Flush();
                zipBytes = msOut.ToByteArray();
            }

            return zipBytes;
        }

        [Obsolete("BZipViaStream is deprecated, please use BZip2Bytes or BZip2Stream instead", false)]
        public static byte[] BZipViaStream(byte[] inBytes) => BZip2Bytes(inBytes);

        /// <summary>
        /// BZip2Stream bzip2 data on a  a <see cref="MemoryStream">Memorystream memIn</see>
        /// and write bzip2 compressed data on <see cref="MemoryStream">MemoryStream memOut</see>
        /// </summary>
        /// <param name="memIn"><see cref="MemoryStream">Memorystream containg data to compress</see><</param>
        /// <param name="closeDisposeMemIn">if true, parameter passed <see cref="MemoryStream">memIn</see> will be closed and disppsed 
        /// after compressed data are written to <see cref="MemoryStream">MemoryStream memOut</see>, which will be returned</param>
        /// <param name="compressionLevel">level of compression: 
        ///  1  ... for at least no compression, 
        /// 4,5 ... for average compression
        ///  9  ... for strongest bzip2 compression, generating smallest most compact output 
        /// </param>
        /// <returns><see cref="MemoryStream">MemoryStream memOut containing bzip2 compressed data</see></returns>
        public static MemoryStream BZip2Stream(MemoryStream memIn, bool closeDisposeMemIn = false, int compressionLevel = 9)
        {
            long buflen = Math.Max(memIn.Length, 4096);
            MemoryStream memOut = new MemoryStream();
            using (BZip2OutputStream bzOut = new BZip2OutputStream(memOut, compressionLevel))
            {
                StreamUtils.Copy(memIn, bzOut, new byte[buflen]);
            }
            memOut.Flush();
            if (closeDisposeMemIn)
            {
                memIn.Close();
                memIn.Dispose();
            }

            return memOut;
        }

        /// <summary>
        /// BZip2Bytes bzip2 <see cref="byte[]">byte[] inBytes</see> 
        /// and write bzip2 compressed data <see cref="byte[]">byte[] outBytes</see> 
        /// </summary>
        /// <param name="inBytes"><see cref="byte[]">byte[] inBytes, containing data to compress with bzip2</see></param>
        /// <param name="compressionLevel">level of compression: 
        ///  1  ... for at least no compression, 
        /// 4,5 ... for average compression
        ///  9  ... for strongest bzip2 compression, generating smallest most compact output 
        /// </param>
        /// <returns><see cref="byte[]">byte[] outBytes</see> containing bzip2 compressed data of / from <see cref="byte[]">byte[] outBytes</see></returns>
        public static byte[] BZip2Bytes(byte[] inBytes, int compressionLevel = 9)
        {
            byte[] zipBytes = null;

            using (MemoryStream msIn = new MemoryStream(inBytes, 0, inBytes.Length))
            {
                msIn.Seek(0, SeekOrigin.Begin);
                using (MemoryStream msOut = BZip2Stream(msIn, false, compressionLevel))
                {
                    zipBytes = msOut.ToArray();
                }
            }

            return zipBytes;
        }


        /// <summary>
        /// BZips a string and returns it as hex16, hex32, base32, base64 (default) or uuencoded string
        /// </summary>
        /// <param name="inText"></param>
        /// <param name="compressionLevel"></param>
        /// <param name="encType"></param>
        /// <returns></returns>
        public static string BZip2TextAndEncode(string inText, int compressionLevel = 9, EncodingType encType = EncodingType.Base64)
        {
            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(inText);
            byte[] zipBytes = BZip2Bytes(inBytes, compressionLevel);
            string bz2TextEncoded = EnDeCodeHelper.Encode(zipBytes, encType);

            return bz2TextEncoded;
        }

        #endregion bzip2 compression


        #region bzip2 decompression

        /// <summary>
        /// BUnZips compressed <see cref="byte[]">byte[] inBytes</see> 
        /// and returns the decompressed bunzipped <see cref="byte[]?" />.
        /// </summary>
        /// <param name="inBytes">compressed / bzipped <see cref="byte[]">byte[] inBytes</see></param>
        /// <returns>decompressed bunzipped <see cref="byte[]?">byte[]?</see></returns>
        public static byte[] BUnZip(byte[] inBytes)
        {
            byte[] unZipBytes = null;

            MemoryStream msIn = new MemoryStream(inBytes, 0, inBytes.Length);
            msIn.Seek(0, SeekOrigin.Begin);
            using (MemoryStream msOut = new MemoryStream())
            {
                ICSharpCode.SharpZipLib.BZip2.BZip2.Decompress(msIn, msOut, true);
                msOut.Flush();
                unZipBytes = msOut.ToByteArray();
            }

            return unZipBytes;
        }


        [Obsolete("BUnZipViaStream is deprecated, please use BUnZip2Stream or BUnZip2Bytes instead", false)]
        public static byte[] BUnZipViaStream(byte[] inBytes) => BUnZip2Bytes(inBytes);


        /// <summary>
        /// BUnZip2Stream bunzip2 (= decompresses) data on a <see cref="MemoryStream">Memorystream memIn</see>
        /// and writes decompressed unzipped data to <see cref="MemoryStream">MemoryStream memOut</see>
        /// </summary>
        /// <param name="memIn"><see cref="MemoryStream">Memorystream containg bzip2 compressed / zipped data to decompress</see><</param>        
        /// <param name="closeDisposeMemIn">if true, parameter passed <see cref="MemoryStream">memIn</see> will be closed and disppsed 
        /// after compressed data are written to <see cref="MemoryStream">MemoryStream memOut</see>, which will be returned</param>
        /// <returns><see cref="MemoryStream">MemoryStream memOut containing bunzip2 decompressed / unzipped data</see></returns>
        public static MemoryStream BUnZip2Stream(MemoryStream memIn, bool closeDisposeMemIn = false)
        {
            long buflen = Math.Max(memIn.Length, 4096);
            MemoryStream memOut = new MemoryStream();

            ICSharpCode.SharpZipLib.BZip2.BZip2InputStream memIs = new BZip2InputStream(memIn);
            using (BZip2InputStream bzIn = new BZip2InputStream(memIn))
            {
                StreamUtils.Copy(bzIn, memOut, new byte[buflen]);
            }
            memOut.Flush();
            memOut.Seek(0, SeekOrigin.Begin);

            if (closeDisposeMemIn)
            {
                memIn.Close();
                memIn.Dispose();
            }

            return memOut;
        }


        /// <summary>
        /// BUnZip2Bytes bunzips <see cref="byte[]">byte[] inBytes</see> and writes
        /// decompressed unzipped data to <see cref="byte[]">byte[] outBytes</see> 
        /// </summary>
        /// <param name="inBytes"><see cref="byte[]">byte[] inBytes, containing bzip2 compressed data</see></param>
        /// <returns><see cref="byte[]">byte[] outBytes</see> containing bunzipped / decompressed data from <see cref="byte[]">byte[] inBytes</see></returns>
        public static byte[] BUnZip2Bytes(byte[] inBytes)
        {
            byte[] zipBytes = null;

            using (MemoryStream msIn = new MemoryStream(inBytes, 0, inBytes.Length))
            {
                msIn.Seek(0, SeekOrigin.Begin);
                using (MemoryStream msOut = BUnZip2Stream(msIn, true))
                {
                    zipBytes = msOut.ToArray();
                }
            }

            return zipBytes;
        }

        #endregion bzip2 decompression


        /// <summary>
        /// BzFile bzips or bunzips a file
        /// </summary>
        /// <param name="inFile"><see cref="string">full (unc) filepath to file</see></param>
        /// <param name="outMessage">string with information, what happend</param>
        /// <param name="outFile"><see cref="string"/>full (unc) filepath to outfile,
        /// if keept empty, .bz2 will be added after compression and .bz|.bz2 will be removed after decompressing bzip'd file.</param>
        /// <param name="zip"><see cref="bool">(bool)true for bzip2, (bool)false for bunzip2 (bzip2 -d)</see></param>
        /// <param name="compressionLevel">level of compression: 
        ///  1  ... for at least no compression, 
        /// 4,5 ... for average compression
        ///  9  ... for strongest bzip2 compression, generating smallest most compact output 
        /// </param>
        /// <returns><see cref="string">string name of processed (bzipped/bunzipped) file</see></returns>
        public static bool BzFile(string inFile, out string outMessage, string outFile = "", bool zip = true, int compressionLevel = 9)
        {
            if (string.IsNullOrEmpty(inFile) || !File.Exists(inFile))
                throw new ArgumentNullException("string BzFile(string inFile, bool zip = true) => inFile is either null or empty or doesn't exist!");

            outMessage = String.Format("{0} {1} {2} ... ",
                DateTime.Now.Area23DateTimeWithSeconds(),
                (zip ? $"gzip " : $"gunzip "),
                Path.GetFileName(inFile));

            byte[] inBytes = File.ReadAllBytes(inFile);
            byte[] outBytes = zip ? BZip2Bytes(inBytes, 9) : BUnZip2Bytes(inBytes);

            if (string.IsNullOrEmpty(outFile))
                outFile = zip ? inFile + ".bz2" : inFile.EndsWith(".bz2") ?
                    inFile.Replace(".bz2", "").Replace(".bz", "") : DateTime.Now.ToString("yy-MM-dd_") + inFile;
            File.WriteAllBytes(outFile, outBytes);

            FileInfo fi = new FileInfo(outFile);
            if (fi.Exists)
            {
                outMessage += $"created file {fi.Name} length={fi.Length} at {fi.CreationTime.ToShortDateString()} {fi.CreationTime.ToShortTimeString()}\nin directory {fi.DirectoryName}";
                return true;
            }

            else outMessage += $"=> file {outFile} NOT created; something went wrong!";

            return false;
        }

    }

}
