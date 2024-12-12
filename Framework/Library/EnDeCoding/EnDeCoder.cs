using DBTek.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.EnDeCoding
{
    /// <summary>
    /// static class EnDeCoder provides serveral static methods for ASCII, UTF7, UTF8, Unicode, UTF32 encoding.
    /// </summary>
    public static class EnDeCoder
    {

        private static Encoding encoding = Encoding.UTF8;
        public static Encoding EnCodIng { get => encoding; internal set => encoding = value; }

        static EnDeCoder() { }


        public static string GetString(byte[] data)
        {
            return EnCodIng.GetString(data, 0, data.Length);
        }

        public static byte[] GetBytes(string str2encode)
        {
            return EnCodIng.GetBytes(str2encode);
        }


        public static int GetByteCount(string str)
        {
            return EnCodIng.GetByteCount(str);
        }


        #region GetString 


        public static string GetStringDefault(byte[] data)
        {
            return Encoding.Default.GetString(data, 0, data.Length);
        }

        public static string GetStringASCII(byte[] data)
        {
            return Encoding.ASCII.GetString(data, 0, data.Length);
        }

        public static string GetString7(byte[] data)
        {
            return Encoding.UTF7.GetString(data, 0, data.Length);
        }

        public static string GetString8(byte[] data)
        {
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public static string GetString16(byte[] data)
        {
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }

        public static string GetString32(byte[] data)
        {
            return Encoding.UTF32.GetString(data, 0, data.Length);
        }
        #endregion GetString 

        #region GetBytes

        public static byte[] GetBytesDefault(string str2encode)
        {
            return Encoding.Default.GetBytes(str2encode);
        }

        public static byte[] GetBytesASCII(string str2encode)
        {
            return Encoding.ASCII.GetBytes(str2encode);
        }

        public static byte[] GetBytes7(string str2encode)
        {
            return Encoding.UTF7.GetBytes(str2encode);
        }

        public static byte[] GetBytes8(string str2encode)
        {
            return Encoding.UTF8.GetBytes(str2encode);
        }

        public static byte[] GetBytes16(string str2encode)
        {
            return Encoding.Unicode.GetBytes(str2encode);
        }

        public static byte[] GetBytes32(string str2encode)
        {
            return Encoding.UTF32.GetBytes(str2encode);
        }

        #endregion GetBytes

        #region GetByteCount

        public static int GetByteCountDefault(string str)
        {
            return Encoding.Default.GetByteCount(str);
        }

        public static int GetByteCountASCII(string str)
        {
            return Encoding.ASCII.GetByteCount(str);
        }

        public static int GetByteCount7(string str)
        {
            return Encoding.UTF7.GetByteCount(str);
        }

        public static int GetByteCount8(string str)
        {
            return Encoding.UTF8.GetByteCount(str);
        }

        public static int GetByteCount16(string str)
        {
            return Encoding.Unicode.GetByteCount(str);
        }

        public static int GetByteCount32(string str)
        {
            return Encoding.UTF32.GetByteCount(str);
        }

        #endregion GetByteCount

    }
}
