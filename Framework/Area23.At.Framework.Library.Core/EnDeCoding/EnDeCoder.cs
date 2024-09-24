using DBTek.Crypto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.EnDeCoding
{
    public static class EnDeCoder
    {

        static EnDeCoder()
        {                

        }


        #region GetString 

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

    }
}
