using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Static;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.Core;

namespace Area23.At.Framework.Core.Zfx
{

    /// <summary>
    /// abstraction of gnu zip gzip compression & decompression
    /// </summary>
    public static class Z7
    {
        const int BUFSZE = 1024;

        #region 7zip compression

        /// <summary>
        /// Zip7 directly
        /// </summary>
        /// <param name="inBytes"><see cref="byte[]"/> inBytes</param>
        /// <param name="compressionLevel">level of compression: 
        ///  1  ... for at least no compression, 
        /// 4,5 ... for average compression
        ///  9  ... for strongest bzip2 compression, generating smallest most compact output 
        /// </param>
        /// <returns><see cref="byte[]"/> outbytes</returns>
        public static byte[] Zip7(byte[] inBytes, int compressionLevel = 6)
        {
            byte[] zipBytes = new byte[0];
            string inFile = Path.Combine(LibPaths.SystemDirTmpPath, DateTime.Now.ToString("yyMMdd_hhmmss") + ".hex");
            string outFile = inFile.Replace(".hex", ".7z");
            File.WriteAllBytes(inFile, inBytes);
            ProcessCmd.Execute("7z", string.Format(" -t7z -spf -ssc a {0} {1}", outFile, inFile));
            Thread.Sleep(32);
            if (File.Exists(outFile))
                zipBytes = File.ReadAllBytes(outFile);

            //try
            //{
            //    File.Delete(inFile);
            //    if (File.Exists(outFile))
            //        File.Delete(outFile);
            //}
            //catch (Exception ex)
            //{
            //    Area23Log.LogOriginEx("Z7", ex);
            //}

            return (zipBytes.Length > 0) ? zipBytes : inBytes;
        }

        #endregion 7zip compression

        #region 7zip decompression

        /// <summary>
        /// <see cref="UnZip7"/>
        /// </summary>
        /// <param name="inBytes"><see cref="byte[]"/> inBytes</param>
        /// <returns><see cref="byte[]"/> outbytes</returns>
        public static byte[] UnZip7(byte[] zippedBytes)
        {
            byte[] outBytes = new byte[0];
            string inFile = Path.Combine(LibPaths.SystemDirTmpPath, DateTime.Now.ToString("yyMMdd_hhmmss") + ".7z");
            string outFile = inFile.Replace(".7z", ".hex");
            File.WriteAllBytes(inFile, zippedBytes);
            ProcessCmd.Execute("7z", string.Format(" -t7z -so x {0} > {1}", inFile, outFile));
            Thread.Sleep(64);
            if (File.Exists(outFile))
                outBytes = File.ReadAllBytes(outFile);

            //try
            //{
            //    if (File.Exists(inFile))
            //        File.Delete(inFile);
            //    if (File.Exists(outFile))
            //        File.Delete(outFile);
            //}
            //catch (Exception ex)
            //{
            //    Area23Log.LogOriginMsgEx("Z7", $"Exception deleting {inFile} or {outFile}", ex);
            //}

            return (outBytes.Length > 0) ? outBytes : zippedBytes;
        }


        #endregion 7zip decompression

    }

}
