using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    [Serializable]
    [DefaultValue("ECB")]
    public enum CipherMode2 : byte
    {
        CBC = 0x0,
        CCM = 0x1,
        CFB = 0x2,
        CTS = 0x3,
        EAX = 0x4,
        ECB = 0x5,
        GOFB = 0x6
    }


    /// <summary>
    /// <see cref="System.Security.Cryptography.CipherMode"/>
    /// </summary>
    [Serializable]
    public struct CiffreMode
    {
        public static CipherMode CMode { get; internal set; }

        public static CipherMode2 CMode2 { get; internal set; }

        public CiffreMode(CipherMode2 cmod2)
        {
            CMode2 = cmod2;
            CMode = cmod2.ToCipherMode();
        }

        public CiffreMode(CipherMode cipherMode)
        {
            CMode = cipherMode;
            CMode2 = cipherMode.FromCipherMode();
        }

    }

    public static partial class CipherModeExtensions
    {

        //public static CipherMode ToCipherMode(this CipherMode2 cipherMode2)
        //{
        //    return cipherMode2 switch
        //    {
        //        CipherMode2.CBC => CipherMode.CBC,
        //        CipherMode2.CFB => CipherMode.CFB,
        //        CipherMode2.CTS => CipherMode.CTS,
        //        CipherMode2.ECB => CipherMode.ECB,
        //        _ => throw new NotSupportedException($"CipherMode2 '{cipherMode2}' is not supported in System.Security.Cryptography.CipherMode"),
        //    };
        //}

        public static CipherMode ToCipherMode(this CipherMode2 mode)
        {
            switch(mode) 
            {
                case CipherMode2.CBC: return CipherMode.CBC;
                case CipherMode2.CFB: return CipherMode.CFB;
                case CipherMode2.CTS: return CipherMode.CTS;
                case CipherMode2.ECB: return CipherMode.ECB;
                case CipherMode2.CCM: return CipherMode.CBC;
                case CipherMode2.EAX: return CipherMode.CBC;
                case CipherMode2.GOFB: return CipherMode.CBC;
                default: break;
            }
            return CipherMode.ECB;
        }

        public static CipherMode2 FromCipherMode(this CipherMode mode)
        {
            switch (mode)
            {
                case CipherMode.CBC: return CipherMode2.CBC;
                case CipherMode.CFB: return CipherMode2.CFB;
                case CipherMode.CTS: return CipherMode2.CTS;
                case CipherMode.ECB: return CipherMode2.ECB;
                default: break;
            }
            return CipherMode2.ECB;
        }

        public static CipherMode[] GetCipherModes()
        {
            List<CipherMode> list = new List<CipherMode>();
            foreach (string encName in Enum.GetNames(typeof(CipherMode)))
            {
                list.Add((CipherMode)Enum.Parse(typeof(CipherMode), encName));
            }

            return list.ToArray();
        }



        public static CipherMode2[] GetCipherModes2()
        {
            List<CipherMode2> list = new List<CipherMode2>();
            foreach (string encName in Enum.GetNames(typeof(CipherMode2)))
            {
                list.Add((CipherMode2)Enum.Parse(typeof(CipherMode2), encName));
            }

            return list.ToArray();
        }



        /// <summary>
        /// parses pipe semicolon separated pipe string to CipherList
        /// </summary>
        /// <param name="text">semicolon separated pipe string to CipherList </param>
        /// <returns><see cref="T:CipherMode"/> array of ciphers for the pipe</returns>
        public static CipherMode2 ParseText(string text)
        {
            CipherMode2 cipherMode = CipherMode2.ECB;
            List<CipherMode2> cipherList = new List<CipherMode2>();
            text = text ?? "";

            if (!Enum.TryParse<CipherMode2>(text, out cipherMode))
                cipherMode = CipherMode2.ECB;

            return cipherMode;
        }

    }

}
