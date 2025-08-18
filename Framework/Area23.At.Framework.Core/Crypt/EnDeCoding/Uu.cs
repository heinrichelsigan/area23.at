using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using System.Text;

namespace Area23.At.Framework.Core.Crypt.EnDeCoding
{

    /// <summary>
    /// Uu is unix2unix uuencode uudecode
    /// Thanks to https://github.com/n3wt0n/Crypto/blob/master/DBTek.Crypto.Shared/UUEncoder.cs
    /// </summary>
    public class Uu : IDecodable
    {

        public static readonly object _lock = new object();
        public const string VALID_CHARS = "!\"#$%&\'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_` \r\n";
        
        //public static bool IsUnix { get => (Path.DirectorySeparatorChar == '/'); }
        //public static bool IsWindows { get => (Path.DirectorySeparatorChar == '\\'); }
        

        #region common interface, interfaces for static members appear in C# 7.3 or later

        public IDecodable Decodable => this;

        public static HashSet<char>? ValidCharList { get; private set; } = new HashSet<char>(VALID_CHARS.ToCharArray());

        public string Encode(byte[] data) => Uu.Encode(data, false, false);

        public byte[] Decode(string encodedString) => Uu.Decode(encodedString, false, false);


        public bool IsValidShowError(string encodedString, out string error) => Uu.IsValidUu(encodedString, out error);

        public bool IsValid(string encodedString) => Uu.IsValidUu(encodedString, out _);

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
            Area23Log.LogOriginMsg("Uu", $"{toUuFunCall} ... STARTED.");

            string bytStr = "", uu = "";

            if (originalUue && !fromFile)
            {
                bytStr = Encoding.UTF8.GetString(inBytes);
                uu = UuEncodeString(bytStr);
                uu = uu.Replace(" ", "`");
            }
            else
            {
                uu = UuEncodeBytesToString(inBytes);
            }

            Area23Log.LogOriginMsg("Uu", $"ToUu(byte[{inBytes.Length}] inBytes, bool originalUue = {originalUue}. bool fromFile = {fromFile}) ... FINISHED.");
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
            Area23Log.LogOriginMsg("Uu", fromUuFunCall + "... STARTED.");
            
            string plainStr = "";
            byte[] plainBytes = new byte[Math.Max(plainStr.Length, uuEncStr.Length)];

            if (originalUue && !fromFile)
            {
                lock (_lock)
                {
                    plainStr = UuDecodeString(uuEncStr.Replace(" ", "`"));
                    plainBytes = Encoding.UTF8.GetBytes(plainStr);
                }
            }
            else
            {
                Area23Log.LogOriginMsg("Uu", $"FromUu: Trying to get bytes from memory stream!");
                plainBytes = UuDecodeBytes(uuEncStr);
            }

            Area23Log.LogOriginMsg("Uu", $"byte[{plainBytes.Length}] plainBytes = FromUu(string uuEncStr, bool originalUue = {originalUue}, fromFile = {fromFile}) ... FINISHED.");
            return plainBytes;
        }

        /// <summary>
        /// UuEncode unix 2 unix encodes a string
        /// </summary>
        /// <param name="plainText">plain text string to encode</param>
        /// <returns>uuencoded string</returns>
        public static string UuEncode(string plainText)
        {
            return UuEncodeString(plainText).Replace(" ", "`");
        }

        /// <summary>
        /// UuDecode unix 2 unix decodes a string
        /// </summary>
        /// <param name="uuEncodedStr">uuencoded string</param>
        /// <returns>uudecoded plain text</returns>
        public static string UuDecode(string uuEncodedStr)
        {
            return UuDecodeString(uuEncodedStr.Replace(" ", "`"));
        }

        public static bool IsValidUu(string uuEncodedStr, out string error)
        {
            string encodedBody = uuEncodedStr;
            bool isValid = true;
            error = "";
            

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
                    {
                        error += ch.ToString();
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        #region helper

        private static byte[] UuEncodeBytes(byte[] src, int len)
        {
            if (len == 0) return new byte[] { 96, 13, 10 };

            List<byte> bytes = new List<byte>(src);            
            List<byte> cod = new List<byte>();
            cod.Add((byte)(len + 32));

            for (int i = 0; i < src.Length; i += 3)
            {
                cod.Add((byte)(32 + src[i] / 4));
                cod.Add((byte)(32 + (src[i] % 4) * 16 + src[i + 1] / 16));
                cod.Add((byte)(32 + (src[i + 1] % 16) * 4 + src[i + 2] / 64));
                cod.Add((byte)(32 + src[i + 2] % 64));
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
                Area23Log.LogOriginMsg("Uu", $"ToUu: Exception {exStream.Message}, when encoding to uu via MemoryStream in UuEncodeBytesToString(...)");
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
                Area23Log.LogOriginMsg("Uu", $"FromUu: read {plainBytes.Length} bytes from MemoryStream.");
                memStream.Close();
            }
            catch
            {
                Area23Log.LogOriginMsg("Uu", $"FromUu: uncaught com or unknown Exception, when reading bytes from MemoryStream!");
            }
            finally
            {
                memStream?.Close();
            }

            return plainBytes;
        }


        /// <summary>
        /// Encode a string using UUEncoder, thanks to
        /// Thanks to https://github.com/n3wt0n/Crypto/blob/master/DBTek.Crypto.Shared/UUEncoder.cs
        /// </summary>
        /// <param name="srcString">The source string to encode</param>
        /// <returns>The encoded string</returns>
        public static string UuEncodeString(string srcString)
        {
            if (!string.IsNullOrEmpty(srcString))
            {
                int length = srcString.Length;
                string text0 = "";
                if (length > 45)
                {
                    text0 = UuEncodeString(srcString.Substring(45));
                    srcString = srcString.Substring(0, 45);
                }

                string text1 = Convert.ToChar(srcString.Length + 32).ToString();
                while (srcString.Length % 3 != 0)
                {
                    srcString += '\0';
                }

                for (int i = 0; i < srcString.Length; i += 3)
                {
                    string text2 = ((char)(32 + (byte)srcString[i] / 4)).ToString();
                    text2 += (char)(32 + (byte)srcString[i] % 4 * 16 + (byte)srcString[i + 1] / 16);
                    text2 += (char)(32 + (byte)srcString[i + 1] % 16 * 4 + (byte)srcString[i + 2] / 64);
                    text2 += (char)(32 + (byte)srcString[i + 2] % 64);
                    text1 += text2;
                }

                return text1 + ((text0.Length == 0) ? "" : "\r\n") + text0;
            }

            return "`";
        }


        /// <summary>
        /// Decode a string encoded with UUEncoder
        /// Thanks to https://github.com/n3wt0n/Crypto/blob/master/DBTek.Crypto.Shared/UUEncoder.cs
        /// </summary>
        /// <param name="srcString">The encoded string to decode</param>
        /// <returns>The decoded string</returns>
        public static string UuDecodeString(string srcString)
        {
            if (!string.IsNullOrEmpty(srcString) && srcString[0] != '`')
            {
                string text0 = "";
                string[] array = srcString.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string text1 in array)
                {
                    int num = text1[0] - 32;
                    string text2 = "";
                    for (int j = 1; j < text2.Length; j += 4)
                    {
                        text2 += (char)((text1[j] - 32) * 4 + (text1[j + 1] - 32) / 16);
                        if (text2.Length == num)
                            break;

                        text2 += (char)((text1[j + 1] - 32) % 16 * 16 + (text1[j + 2] - 32) / 4);
                        if (text2.Length == num)
                            break;

                        text2 += (char)((text1[j + 2] - 32) % 4 * 64 + (text1[j + 3] - 32));
                        if (text2.Length == num)
                            break;
                    }

                    text0 += text2;
                }

                return text0;
            }

            return string.Empty;
        }


        #endregion helper

    }


}