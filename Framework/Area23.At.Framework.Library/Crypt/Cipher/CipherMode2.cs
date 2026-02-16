using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    /// <summary>
    /// CipherMode2 enumeration type for more cipher mode as found in standard <see cref="System.Security.Cryptography.CipherMode"/>
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>code changes</listheader>
    /// <item>
    /// 2026-02-11 alert-fix-13 changed mode from "ECB" to "CFB"     
    /// Reason: Git security scans
    /// consequences: no more fully deterministic math bijective proper symmertric cipher en-/decryption in pipe
    /// fixed attacks: not so easy REPLY attacks with binary format header and heuristic key collection
    /// </item>
    /// <item>
    /// 2026-mm-dd [enter pull request name here] [enter what you did here]
    /// Reason: [enter a senseful reason]
    /// consequences: [describe most impactful consequences of bugfix or code change request]
    /// fixed [vulnerability, code smell]: [Describe understandable precise in 1-2 setences]
    /// </item>
    /// </list>
    /// </remarks>
    [Serializable]
    [DefaultValue("CFB")]
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
        public static CipherMode CMode { get; internal set; } = CipherMode.CFB;

        public static CipherMode2 CMode2 { get; internal set; } = CipherMode2.CFB;

        public CiffreMode(CipherMode2 cipherMode2)
        {
            CMode2 = cipherMode2;
            CMode = cipherMode2.ToCipherMode();
        }

    }

    /// <summary>
    /// Extension methods for <see cref="CipherMode2">enum CipherMode2</see>
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <listheader>code changes</listheader>
    /// <item>
    /// 2026-02-11 alert-fix-13 changed mode from "ECB" to "CFB"     
    /// Reason: Git security scans
    /// consequences: no more fully deterministic math bijective proper symmertric cipher en-/decryption in pipe
    /// fixed attacks: not so easy REPLY attacks with binary format header and heuristic key collection
    /// </item>
    /// <item>
    /// 2026-mm-dd [enter pull request name here] [enter what you did here]
    /// Reason: [enter a senseful reason]
    /// consequences: [describe most impactful consequences of bugfix or code change request]
    /// fixed [vulnerability, code smell]: [Describe understandable precise in 1-2 setences]
    /// </item>
    /// </list>
    /// </remarks>
    public static partial class CipherModeExtensions
    {

        //public static CipherMode ToCipherMode(this CipherMode2 cipherMode2)
        //{
        //    switch (cipherMode2)
        //    {
        //        case CipherMode2.CBC: return CipherMode.CBC;
        //        case CipherMode2.CFB: return CipherMode.CFB;
        //        case CipherMode2.CTS: return CipherMode.CTS;
        //        case CipherMode2.ECB: return CipherMode.ECB;
        //        default: throw new NotSupportedException($"CipherMode2 '{cipherMode2}' is not supported in System.Security.Cryptography.CipherMode"),
        //    };
        //}

        public static CipherMode ToCipherMode(this CipherMode2 mode)
        {
            switch(mode)
            {
                case CipherMode2.CBC: return CipherMode.CBC;
                case CipherMode2.CCM: return CipherMode.CBC;
                case CipherMode2.CFB: return CipherMode.CFB;
                case CipherMode2.CTS: return CipherMode.CTS;
                case CipherMode2.EAX: return CipherMode.CBC;
                case CipherMode2.ECB: return CipherMode.ECB;
                case CipherMode2.GOFB: return CipherMode.CBC;
                default: return CipherMode.CFB;
            };
        }

        public static CipherMode2 FromCipherMode(this CipherMode mode)
        {
            switch(mode)
            {
                case CipherMode.CBC: return CipherMode2.CBC;
                case CipherMode.CFB: return CipherMode2.CFB;
                case CipherMode.CTS: return CipherMode2.CTS;
                case CipherMode.ECB: return CipherMode2.ECB;
                default: return CipherMode2.CFB;
            };
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
            CipherMode2 cipherMode = CipherMode2.CFB;
            List<CipherMode2> cipherList = new List<CipherMode2>();
            text = text ?? "";

            if (!Enum.TryParse<CipherMode2>(text, out cipherMode))
                cipherMode = CipherMode2.CFB;

            return cipherMode;
        }

    }

}
