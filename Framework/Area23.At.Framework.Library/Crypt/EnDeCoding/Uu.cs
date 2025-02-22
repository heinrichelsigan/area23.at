using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Util;
using DBTek.Crypto;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Area23.At.Framework.Library.Crypt.EnDeCoding
{

    /// <summary>
    /// Uu is unix2unix uuencode uudecode
    /// </summary>
    public static class Uu
    {

        public static readonly object _lock = new object();
        public static readonly char[] ValidChars = "!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_` \r\n".ToCharArray();
        public static List<char> ValidCharList = new List<char>(ValidChars);

        public static bool IsUnix { get => (Path.DirectorySeparatorChar == '/'); }
        public static bool IsWindows { get => (Path.DirectorySeparatorChar == '\\'); }

        #region common interface, interfaces for static members appear in C# 7.3 or later

        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public static string Encode(byte[] inBytes, bool fromPlain = false, bool fromFile = false)
        {
            return ToUu(inBytes, fromPlain, fromFile);
        }

        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public static byte[] Decode(string encodedString, bool fromPlain = false, bool fromFile = false)
        {
            return FromUu(encodedString, fromPlain, fromFile);
        }

        /// <summary>
        /// Checks if a string is a valid encoded string
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>true, when encoding is OK, otherwise false, if encoding contains illegal characters</returns>
        public static bool IsValid(string encodedString)
        {
            return IsValidUu(encodedString);
        }

        #endregion common interface, interfaces for static members appear in C# 7.3 or later


        /// <summary>
        /// ToUu
        /// </summary>
        /// <param name="inBytes">binary byte array</param>
        /// <param name="originalUue">true, if only uuencode without encrpytion</param>
        /// <param name="fromFile">Only for uu: true, if file and not textbox will be encrypted, default (false)</param>
        /// <returns>uuencoded string</returns>
        public static string ToUu(byte[] inBytes, bool originalUue = true, bool fromFile = false)
        {
            string hexOutPath = LibPaths.SytemDirUuPath + DateTime.Now.Area23DateTimeWithMillis() + ".hex";
            string toUuFunCall = $"ToUu(byte[{inBytes.Length}] inBytes, bool originalUue = {originalUue}, bool fromFile = {fromFile})";
            Area23Log.LogStatic($"{toUuFunCall} ... STARTED.");

            string bytStr = "", uu = "";

            if (originalUue && !fromFile)
            {
                bytStr = Encoding.UTF8.GetString(inBytes);
                uu = (new UUEncoder()).EncodeString(bytStr);
                uu = uu.Replace(" ", "`");
            }
            else
            {
                string uuOutFile = DateTime.Now.Area23DateTimeWithMillis() + ".uu";
                string uuOutPath = LibPaths.SytemDirUuPath + uuOutFile;

                Area23Log.LogStatic($"ToUu: uuOutFile={uuOutFile}, hexOutFile = {hexOutPath}.");

                System.IO.File.WriteAllBytes(hexOutPath, inBytes);
                Area23Log.LogStatic("ToUu: Wrote inBytes to " + hexOutPath);

                if (IsUnix)
                {
                    try
                    {
                        string exeCmd = "/usr/bin/uuencode";
                        if (System.IO.File.Exists("/usr/local/bin/uuencrypt.sh"))
                            exeCmd = "/usr/local/bin/uuencrypt.sh";
                        else if (System.IO.File.Exists(LibPaths.AdditionalBinDir + "uuencrypt.sh"))
                            exeCmd = LibPaths.AdditionalBinDir + "uuencrypt.sh";

                        Area23Log.LogStatic("ToUu: exeCmd = " + exeCmd);

                        try
                        {
                            ProcessCmd.Execute(exeCmd, " " + hexOutPath + " " + uuOutFile + " " + uuOutPath + " ", false);
                        }
                        catch (Exception exExe1)
                        {
                            Area23Log.LogStatic(exExe1);
                            try
                            {
                                ProcessCmd.Execute("/usr/bin/uuencode", " " + hexOutPath + " " + uuOutFile + " > " + uuOutPath + " ", false);
                            }
                            catch (Exception exExe2)
                            {
                                Area23Log.LogStatic(exExe2);
                            }
                        }

                        Thread.Sleep(64);
                        if (!File.Exists(uuOutPath))
                            uu = "";
                    }
                    catch (Exception ex)
                    {
                        uu = "";
                        Area23Log.LogStatic($"ToUu: Exception in {toUuFunCall} ...");
                        Area23Log.LogStatic(ex);
                    }
                }

                if (IsWindows || string.IsNullOrEmpty(uu))
                {
                    try
                    {
                        (new UUEncoder()).EncodeFile(hexOutPath, uuOutPath);
                    }
                    catch (Exception exDbTekUu)
                    {
                        Area23Log.LogStatic($"ToUu: Exception {exDbTekUu.Message} in {toUuFunCall},\n \twhen encoding to uu via DBTek.Crypto.");
                    }
                    Thread.Sleep(32);
                }

                if (File.Exists(uuOutPath))
                {
                    uu = System.IO.File.ReadAllText(uuOutPath);
                    Area23Log.LogStatic($"ToUu: Read uuencoded text (length = {uu.Length} from file {uuOutPath}.");
                }
                if (!File.Exists(uuOutPath))
                {
                    MemoryStream ms = new MemoryStream();
                    try
                    {
                        TextWriter textWriter = new StreamWriter(ms, Encoding.UTF8);
                        for (int i = 0; i <= inBytes.Length; i += 45)
                        {
                            int num = ((inBytes.Length - i > 45) ? 45 : (inBytes.Length - i));
                            byte[] array2 = new byte[(num % 3 == 0) ? num : (num + 3 - num % 3)];
                            Array.ConstrainedCopy(inBytes, i, array2, 0, num);
                            textWriter.WriteLine(Array.ConvertAll(UuEncodeBytes(array2, num), Convert.ToChar));
                        }
                        textWriter.Flush();
                        textWriter.Close();
                        ms.Flush();
                        uu = EnDeCoder.GetString(ms.ToByteArray());
                        ms.Close();
                    }
                    catch (Exception exStream)
                    {
                        Area23Log.LogStatic($"ToUu: Exception {exStream.Message}, when encoding to uu via MemoryStream in {toUuFunCall}");
                    }
                    finally
                    {
                        ms?.Close();
                    }
                }

                Area23Log.LogStatic($"ToUu(byte[{inBytes.Length}] inBytes, bool originalUue = {originalUue}. bool fromFile = {fromFile}) ... FINISHED.");
                return uu;
            }

            

            return uu;
        }

        /// <summary>
        /// FromUu
        /// </summary>
        /// <param name="uuEncStr">uuencoded string</param>
        /// <param name="originalUue">Only for uu: true, if <see cref="uuEncStr"/> represent a binary without encryption</param>
        /// <param name="fromFile">Only for uu: true, if file and not textbox will be encrypted, default (false)</param>
        /// <returns>binary byte array</returns>
        public static byte[] FromUu(string uuEncStr, bool originalUue = true, bool fromFile = false)
        {
            
            string fromUuFunCall = "FromUu(string uuEncStr[.Length=" + uuEncStr.Length + "], bool originalUue = " + originalUue + ", bool fromFile = " + fromFile + ")";
            Area23Log.LogStatic(fromUuFunCall + "... STARTED.");

            bool errInWin = false, errInUnix = false;
            string plainStr = "";
            byte[] plainBytes = new byte[Math.Max(plainStr.Length, uuEncStr.Length)];

            if (originalUue && !fromFile)
            {
                lock (_lock)
                {
                    plainStr = (new UUEncoder()).DecodeString(uuEncStr.Replace(" ", "`"));
                    plainBytes = Encoding.UTF8.GetBytes(plainStr);                 
                }
                Area23Log.LogStatic($"byte[{plainBytes.Length}] plainBytes = FromUu(string uuEncStr, bool originalUue = {originalUue}, fromFile = {fromFile}) ... FINISHED.");
                return plainBytes;
            }
            else
            {
                string uuOutFile = DateTime.Now.Area23DateTimeWithMillis() + ".uu";
                string uuOutPath = LibPaths.SytemDirUuPath + uuOutFile;
                string hexOutPath = uuOutPath.Replace(".uu", ".oct");

                Area23Log.LogStatic($"FromUu: start wrting uuEncStr to file {uuOutPath}");
                System.IO.File.WriteAllText(uuOutPath, uuEncStr);
                Area23Log.LogStatic($"{fromUuFunCall} ... wrote uuEncstr (length = {uuEncStr.Length}) to {uuOutPath}.");
                string[] uuEncodedLines = uuEncStr.Replace("\r\n", "\n").Split("\n".ToCharArray());

                if (IsWindows || !errInUnix)
                {
                    try
                    {
                        DBTek.Crypto.UUEncoder uudf = new UUEncoder();
                        uudf.DecodeFile(uuOutPath, hexOutPath);

                        Thread.Sleep(32);
                        if (!File.Exists(hexOutPath))
                            errInWin = true;
                    }
                    catch (Exception exFileUuDecode)
                    {
                        Area23Log.LogStatic($"FromUu: Exception {exFileUuDecode.Message},\n \twhen writing bytes to {hexOutPath}!");
                        errInWin = true;
                    }
                }

                if (IsUnix && errInWin)
                {
                    string exeCmd = "/usr/bin/uudecode";
                    if (System.IO.File.Exists("/usr/local/bin/uudecrypt.sh"))
                        exeCmd = "/usr/local/bin/uudecrypt.sh";
                    else if (System.IO.File.Exists(LibPaths.AdditionalBinDir + "uudecrypt.sh"))
                        exeCmd = LibPaths.AdditionalBinDir + "uudecrypt.sh";

                    Area23Log.LogStatic($"FromUu: exeCmd = {exeCmd}.");
                    try
                    {
                        ProcessCmd.Execute(exeCmd, " " + uuOutPath + "  " + hexOutPath + " ", false);
                    }
                    catch (Exception exExe1)
                    {
                        Area23Log.LogStatic(exExe1);
                        try
                        {
                            ProcessCmd.Execute(exeCmd, uuOutPath + " -o " + hexOutPath + " ", false);
                        }
                        catch (Exception exExe2)
                        {
                            Area23Log.LogStatic(exExe2);
                            errInUnix = true;
                        }
                    }
                    if (!errInUnix)
                    {
                        Thread.Sleep(64);
                        if (!File.Exists(hexOutPath))
                            errInUnix = true;
                    }
                }


                if (File.Exists(hexOutPath))
                {
                    lock (_lock)
                    {
                        Area23Log.LogStatic($"FromUu: start reading uudecoded bytes from {hexOutPath}");
                        plainBytes = System.IO.File.ReadAllBytes(hexOutPath);
                        Area23Log.LogStatic($"FromUu: read {plainBytes.Length} bytes from {hexOutPath}.");
                    }
                    Area23Log.LogStatic($"byte[{plainBytes.Length}] plainBytes = FromUu(string uuEncStr, bool originalUue = {originalUue}, fromFile = {fromFile}) ... FINISHED.");
                    if (plainBytes != null && plainBytes.Length != 0 && plainBytes.Length != 45)
                        return plainBytes;
                }
                                
                Area23Log.LogStatic($"FromUu: Trying to get bytes from memory stream, instead {hexOutPath}");
                plainBytes = UuDecodeBytes(uuEncStr);

            }

            Area23Log.LogStatic($"byte[{plainBytes.Length}] plainBytes = FromUu(string uuEncStr, bool originalUue = {originalUue}, fromFile = {fromFile}) ... FINISHED.");
            return plainBytes;
        }

        /// <summary>
        /// UuEncode unix 2 unix encodes a string
        /// </summary>
        /// <param name="plainText">plain text string to encode</param>
        /// <returns>uuencoded string</returns>
        public static string UuEncode(string plainText)
        {
            string uue = (new UUEncoder()).EncodeString(plainText);
            uue = uue.Replace(" ", "`");
            return uue;
        }

        /// <summary>
        /// UuDecode unix 2 unix decodes a string
        /// </summary>
        /// <param name="uuEncodedStr">uuencoded string</param>
        /// <returns>uudecoded plain text</returns>
        public static string UuDecode(string uuEncodedStr)
        {
            uuEncodedStr = uuEncodedStr.Replace(" ", "`");
            string plainStr = (new UUEncoder()).DecodeString(uuEncodedStr);
            return plainStr;
        }

        public static bool IsValidUu(string uuEncodedStr)
        {
            if (uuEncodedStr.StartsWith("begin") && uuEncodedStr.Contains("end"))
            {
                return true;
            }
            if (uuEncodedStr.EndsWith("\nend") || uuEncodedStr.EndsWith("\nend\n") || uuEncodedStr.EndsWith("\nend\r\n"))
            {
                uuEncodedStr = uuEncodedStr.Replace("\nend\r\n", "\n");
                uuEncodedStr = uuEncodedStr.Replace("\nend\n", "\n");
                uuEncodedStr = uuEncodedStr.Replace("\nend", "\n");
            }

            foreach (char ch in uuEncodedStr)
            {
                if (!ValidCharList.Contains(ch))
                    return false;
            }
            return true;
        }

        #region helper

        private static byte[] UuEncodeBytes(byte[] src, int len)
        {
            if (len == 0) return new byte[] { 96, 13, 10 };

            List<byte> bytes = new List<byte>(src);
            //switch ((src.Length % 3))
            //{
            //    case 1: bytes.Add((byte)0); bytes.Add((byte)0); src = bytes.ToArray(); break;
            //    case 2: bytes.Add((byte)0); src = bytes.ToArray(); break;
            //    case 0:
            //    default: break;
            //}
            List<byte> cod = new List<byte>();
            cod.Add((byte)(len + 32));

            for (int i = 0; i < src.Length; i += 3)
            {
                cod.Add((byte)(32 + src[i] / 4));
                cod.Add((byte)(32 + (src[i] % 4) * 16 + src[i + 1] / 16));
                cod.Add((byte)(32 + (src[i + 1] % 16) * 4 + src[i + 2] / 64));
                cod.Add((byte)(32 + src[i + 2] % 64));
                // cod.Add((char)((src[i] >> 2) + 33));
                // cod.Add((char)(((char)((src[i] & 0x3) << 4) | (char)(src[i + 1] >> 4)) + 33));
                // cod.Add((char)(((char)((src[i + 1] & 0xf) << 2) | (char)(src[i + 2] >> 6)) + 33));
                // cod.Add((char)((char)(src[i + 2] & 0x3f) + 33));
            }

            return cod.ToArray();
        }

        private static byte[] UuDecodeBytes(string uuEnc)
        {
            byte[] plainBytes = new byte[uuEnc.Length];
            MemoryStream memStream = null;
            try
            {
                memStream = new MemoryStream();
                string[] array = uuEnc.Replace("\r\n", "\n").Split("\n".ToCharArray());
                string[] array2 = array;
                foreach (string sourceString in array2)
                {
                    byte[] array3 = Array.ConvertAll(UuDecode(sourceString).ToCharArray(), Convert.ToByte);
                    memStream.Write(array3, 0, array3.Length);
                    memStream.Flush();
                }

                memStream.Position = 0;
                plainBytes = memStream.ToByteArray();
                Area23Log.LogStatic($"FromUu: read {plainBytes.Length} bytes from MemoryStream.");
                memStream.Close();
            }
            catch
            {
                Area23Log.LogStatic($"FromUu: uncaught com or unknown Exception, when reading bytes from MemoryStream!");
            }
            finally
            {
                memStream?.Close();
            }

            return plainBytes;
        }

        [Obsolete("UuDecodeBytes is obsolete, use byte[] UuDecodeBytes(string uuEnc), because uuencoded is always an ascii string and never a binary byte[]", false)]
        private static byte[] UuDecodeBytes(byte[] uuEnc, int len)
        {
            List<byte> bytes = new List<byte>(uuEnc);
            switch ((uuEnc.Length % 4))
            {
                case 1: bytes.Add((byte)0); bytes.Add((byte)0); bytes.Add((byte)0); uuEnc = bytes.ToArray(); break;
                case 2: bytes.Add((byte)0); bytes.Add((byte)0); uuEnc = bytes.ToArray(); break;
                case 3: bytes.Add((byte)0); uuEnc = bytes.ToArray(); break;
                case 0:
                default: break;
            }

            List<byte> cod = new List<byte>();
            for (int i = 0; i < uuEnc.Length; i += 4)
            {
                cod.Add((byte)((byte)((uuEnc[i] - 33) << 2) | (byte)((uuEnc[i + 1] - 33) >> 4)));
                cod.Add((byte)(((byte)((uuEnc[i] & 0x3) << 4) | (uuEnc[i + 1] >> 4)) + 33));
                cod.Add((byte)(((byte)((uuEnc[i + 1] & 0xf) << 2) | (byte)(uuEnc[i + 2] >> 6)) + 33));
                cod.Add((byte)(((byte)(uuEnc[i + 2] - 33) << 6) | (byte)(uuEnc[i + 3] - 33)));
            }

            return cod.ToArray();
        }

        #endregion helper

    }

}