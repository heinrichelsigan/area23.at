using Area23.At.Framework;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using DBTek.Crypto;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Text.RegularExpressions;

namespace Area23.At.Framework.Core.Crypt.EnDeCoding
{

    /// <summary>
    /// Uu is unix2unix uuencode uudecode
    /// </summary>
    public class Uu : IDecodable
    {

        public static readonly object _lock = new object();
        public const string VALID_CHARS = "!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_` \r\n";
        
        public static bool IsUnix { get => (Path.DirectorySeparatorChar == '/'); }
        public static bool IsWindows { get => (Path.DirectorySeparatorChar == '\\'); }
        

        #region common interface, interfaces for static members appear in C# 7.3 or later

        public IDecodable Decodable => this;

        public static HashSet<char>? ValidCharList { get; private set; } = new HashSet<char>(VALID_CHARS.ToCharArray());

        public string Encode(byte[] data) 
        {
            return Uu.Encode(data, false, false);
        }

        public byte[] Decode(string encodedString)
        {
            return Uu.Decode(encodedString, false, false);
        }

        public bool IsValid(string encodedString)
        {
            return Uu.IsValidUu(encodedString);
        }

        #endregion common interface, interfaces for static members appear in C# 7.3 or later


        /// <summary>
        /// Encodes byte[] to valid encode formatted string
        /// </summary>
        /// <param name="inBytes">byte array to encode</param>
        /// <returns>encoded string</returns>
        public static string Encode(byte[] inBytes, bool fromPlain = false, bool fromFile = false)
        {
            return Uu.ToUu(inBytes, fromPlain, fromFile);
        }

        /// <summary>
        /// Decodes an encoded string to byte[]
        /// </summary>
        /// <param name="encodedString">encoded string</param>
        /// <returns>byte array</returns>
        public static byte[] Decode(string encodedString, bool fromPlain = false, bool fromFile = false)
        {
            return Uu.FromUu(encodedString, fromPlain, fromFile);
        }


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
            SLog.Log($"{toUuFunCall} ... STARTED.");

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

                SLog.Log($"ToUu: uuOutFile={uuOutFile}, hexOutFile = {hexOutPath}.");

                System.IO.File.WriteAllBytes(hexOutPath, inBytes);
                SLog.Log("ToUu: Wrote inBytes to " + hexOutPath);

                try
                {
                    (new UUEncoder()).EncodeFile(hexOutPath, uuOutPath);
                }
                catch (Exception exDbTekUu)
                {
                    SLog.Log($"ToUu: Exception {exDbTekUu.Message} in {toUuFunCall},\n \twhen encoding to uu via DBTek.Crypto.");
                }
                Thread.Sleep(32);


                if (File.Exists(uuOutPath))
                {
                    uu = System.IO.File.ReadAllText(uuOutPath);
                    SLog.Log($"ToUu: Read uuencoded text (length = {uu.Length} from file {uuOutPath}.");
                    
                    SLog.Log($"ToUu(byte[{inBytes.Length}] inBytes, bool originalUue = {originalUue}. bool fromFile = {fromFile}) ... FINISHED.");
                    // uu = uu.Replace(" ", "`");
                    return uu;
                }
                                
            }


            uu = UuEncodeBytesToString(inBytes);
            SLog.Log($"ToUu(byte[{inBytes.Length}] inBytes, bool originalUue = {originalUue}. bool fromFile = {fromFile}) ... FINISHED.");
            // uu = uu.Replace(" ", "`");
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
            SLog.Log(fromUuFunCall + "... STARTED.");

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
                SLog.Log($"byte[{plainBytes.Length}] plainBytes = FromUu(string uuEncStr, bool originalUue = {originalUue}, fromFile = {fromFile}) ... FINISHED.");
                return plainBytes;
            }
            else
            {
                string uuOutFile = DateTime.Now.Area23DateTimeWithMillis() + ".uu";
                string uuOutPath = LibPaths.SytemDirUuPath + uuOutFile;
                string hexOutPath = uuOutPath.Replace(".uu", ".oct");

                SLog.Log($"FromUu: start wrting uuEncStr to file {uuOutPath}");
                System.IO.File.WriteAllText(uuOutPath, uuEncStr);
                SLog.Log($"{fromUuFunCall} ... wrote uuEncstr (length = {uuEncStr.Length}) to {uuOutPath}.");
                string[] uuEncodedLines = uuEncStr.Replace("\r\n", "\n").Split("\n".ToCharArray());

                try
                {
                    DBTek.Crypto.UUEncoder uudf = new UUEncoder();
                    uudf.DecodeFile(uuOutPath, hexOutPath);

                    Thread.Sleep(32);
                    if (File.Exists(hexOutPath))
                    {
                        plainBytes = System.IO.File.ReadAllBytes(hexOutPath);
                        SLog.Log($"ToUu: Read uuencoded bytes (length = {plainBytes.Length} from file {hexOutPath}.");
                        return plainBytes;
                    }
                }
                catch (Exception exFileUuDecode)
                {
                    SLog.Log($"FromUu: Exception {exFileUuDecode.Message},\n \twhen writing bytes to {hexOutPath}!");
                }               

            }

            SLog.Log($"FromUu: Trying to get bytes from memory stream!");
            plainBytes = UuDecodeBytes(uuEncStr);

            SLog.Log($"byte[{plainBytes.Length}] plainBytes = FromUu(string uuEncStr, bool originalUue = {originalUue}, fromFile = {fromFile}) ... FINISHED.");
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
            string encodedBody = uuEncodedStr;

            if (ValidCharList != null)
            {
                if (uuEncodedStr.StartsWith("begin"))
                {
                    encodedBody = uuEncodedStr.GetSubStringByPattern("begin", true, "", "\n", false, StringComparison.CurrentCultureIgnoreCase);
                    if (encodedBody.Contains("\nend") || encodedBody.Contains("end"))
                    {
                        encodedBody = encodedBody.GetSubStringByPattern("end", false, "", "", true, StringComparison.CurrentCultureIgnoreCase);
                    }
                }

                foreach (char ch in uuEncodedStr)
                {
                    if (!ValidCharList.Contains(ch))
                        return false;
                }
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

        private static string UuEncodeBytesToString(byte[] inBytes)
        {
            string uu = string.Empty;
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
                uu = EnDeCodeHelper.GetString(ms.ToByteArray());
                ms.Close();
            }
            catch (Exception exStream)
            {
                SLog.Log($"ToUu: Exception {exStream.Message}, when encoding to uu via MemoryStream in UuEncodeBytesToString(...)");
            }
            finally
            {
                ms?.Close();
            }

            return uu;
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
                SLog.Log($"FromUu: read {plainBytes.Length} bytes from MemoryStream.");
                memStream.Close();
            }
            catch
            {
                SLog.Log($"FromUu: uncaught com or unknown Exception, when reading bytes from MemoryStream!");
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