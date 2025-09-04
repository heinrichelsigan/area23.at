using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Static;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Area23.At.Framework.Library.Util
{

    public struct MimeTypeExtSig
    {

        /// <summary>
        /// File Extension
        /// </summary>
        public string FileExt { get; set; }
        /// <summary>
        /// Mime type name
        /// </summary>
        public string MimeTyp { get; set; }
        /// <summary>
        /// detection list of bytes signature
        /// </summary>
        public List<byte[]> SigBytesList;

        public int SigBytesLen { get => SigBytesList[0].Length; }


        public MimeTypeExtSig(string fileExt, string mimeType, params byte[][] signatureBytesArray)
        {
            this.FileExt = fileExt;
            this.MimeTyp = mimeType;
            SigBytesList = new List<byte[]>();
            foreach (byte[] bs in signatureBytesArray)
            {
                SigBytesList.Add(bs);
            }
        }

        public MimeTypeExtSig(string fileExt, string mimeType, byte[,] signatureBytesArray)
        {
            this.FileExt = fileExt;
            this.MimeTyp = mimeType;
            SigBytesList = new List<byte[]>();
            byte b;
            List<byte> byteList = new List<byte>();
            for (int i = 0; i < signatureBytesArray.Rank; i++)
            {
                for (int j = 0; j < signatureBytesArray.Length; j++)
                {
                    b = signatureBytesArray[i, j];
                    byteList.Add(b);
                }
                byte[] bytesSig = byteList.ToArray();
                SigBytesList.Add(bytesSig);
            }
        }

        public MimeTypeExtSig(string fileExt, string mimeType, byte[] sigBytes)
        {
            this.FileExt = fileExt;
            this.MimeTyp = mimeType;
            SigBytesList = new List<byte[]>();
            SigBytesList.Add(sigBytes);
        }

    }


    /// <summary>
    /// MimeType gets mime type out of content byte[] (and filename) by MIT magick cookie
    /// </summary>
    public class MimeSignature
    {

        #region static internal readonly

        internal static readonly byte[] _EVA = { 0x4D, 0x5A };
        internal static readonly byte[] _BMP = { 0x42, 0x4D };
        internal static readonly byte[] _DOC = { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
        internal static readonly byte[] _EXE_DLL = { 0x4D, 0x5A };
        internal static readonly byte[] _EXE = { 0x4D, 0x5A };

        internal static readonly byte[] _GIF = { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }; // { 71, 73, 70, 56 };
        internal static readonly byte[] _ICO = { 0x00, 0x00, 0x01, 0x00 };
        internal static readonly byte[] _JPG = { 0xff, 0xd8, 0xff };
        internal static readonly byte[] _MP3 = { 0xff, 0xfb, 0x30 };

        internal static readonly byte[] _OGG = { 0x4F, 0x67, 0x67, 0x53, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        internal static readonly byte[] _PDF = { 0x25, 0x50, 0x44, 0x46, 0x2d, 0x31, 0x2e };
        internal static readonly byte[] _PNG = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0d, 0x49, 0x48, 0x44, 0x52 };
        internal static readonly byte[,] _RAR = { { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00 }, { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01 } };
        internal static readonly byte[,] _SWF = { { 0x43, 0x57, 0x53 }, { 0x46, 0x57, 0x53 } };
        internal static readonly byte[,] _TIF = { { 0x49, 0x49, 0x2A, 0x00 }, { 0x4D, 0x4D, 0x00, 0x2A } };
        internal static readonly byte[] _TORRENT = { 100, 56, 58, 97, 110, 110, 111, 117, 110, 99, 101 };
        internal static readonly byte[] _TTF = { 0x00, 0x01, 0x00, 0x00, 0x00 };
        internal static readonly byte[] _WAV_AVI = { 0x52, 0x49, 0x46, 0x46 };
        internal static readonly byte[] _WAV = { 0x52, 0x49, 0x46, 0x46 };
        internal static readonly byte[] _WMV_WMA = { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C };
        internal static readonly byte[] _WMA = { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C };
        internal static readonly byte[] _WMV = { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C };
        internal static readonly byte[,] _DOCX = { { 0x50, 0x4B, 0x03, 0x04 }, { 0x50, 0x4B, 0x05, 0x06 }, { 0x50, 0x4B, 0x07, 0x08 } };

        internal static readonly byte[] _WEBP = { 0x52, 0x49, 0x46, 0x46 };
        internal static readonly byte[,] _HEIF = { { 0x66, 0x74, 0x79, 0x70, 0x6D, 0x69, 0x66, 0x31 }, { 0x66, 0x74, 0x79, 0x70, 0x68, 0x65, 0x69, 0x63 } };
        internal static readonly byte[] _RPM = { 0xed, 0xab, 0xee, 0xdb };
        internal static readonly byte[] _BIN = { 0x53, 0x50, 0x30, 0x31 };
        internal static readonly byte[] _PIC = { 0x00 };
        internal static readonly byte[] _MP4 = { 0x66, 0x74, 0x79, 0x70 };

        internal static readonly byte[] _OTF = { 0x4F, 0x54, 0x54, 0x4F };
        internal static readonly byte[] _SOT = { 0x50, 0x4C };
        internal static readonly byte[] _WOFF = { 0x77, 0x4F, 0x46, 0x46 };
        internal static readonly byte[] _WOFF2 = { 0x77, 0x4F, 0x46, 0x32 };

        internal static readonly byte[] _PDB = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        internal static readonly byte[] _DBA = { 0xBE, 0xBA, 0xFE, 0xCA };
        internal static readonly byte[] _DBA2 = { 0x00, 0x01, 0x42, 0x44 };
        internal static readonly byte[] _TDA = { 0x00, 0x01, 0x44, 0x54 };
        internal static readonly byte[] _TSA2 = { 0x00, 0x01, 0x00, 0x00 };

        internal static readonly byte[] _3PG = { 0x66, 0x74, 0x79, 0x70, 0x33, 0x67 };
        internal static readonly byte[] _Z = { 0x1F, 0x9D };
        internal static readonly byte[] _TARZ = { 0x1F, 0xA0 };
        internal static readonly byte[] _BAC = { 0x42, 0x41, 0x43, 0x4B, 0x4D, 0x49, 0x4B, 0x45, 0x44, 0x49, 0x53, 0x4B };
        internal static readonly byte[] _BZ2 = { 0x42, 0x5A, 0x68 };
        internal static readonly byte[] _ZIP = { 0x50, 0x4B, 0x03, 0x04 };

        internal static readonly byte[] _CR2 = { 0x49, 0x49, 0x2A, 0x00, 0x10, 0x00, 0x00, 0x00, 0x43, 0x52 };
        internal static readonly byte[,] _CIN = { { 0x80, 0x2A, 0x5F, 0xD7 }, { 0x52, 0x4E, 0x43, 0x01 }, { 0x52, 0x4E, 0x43, 0x02 } };
        internal static readonly byte[,] _DPX = { { 0x53, 0x44, 0x50, 0x58 }, { 0x58, 0x50, 0x44, 0x53 } };
        internal static readonly byte[] _EXR = { 0x76, 0x2F, 0x31, 0x01 };
        internal static readonly byte[] _BPG = { 0x42, 0x50, 0x47, 0xFB };

        internal static readonly byte[] _IDX = { 0x49, 0x4E, 0x44, 0x58 };
        internal static readonly byte[] _LZ = { 0x4C, 0x5A, 0x49, 0x50 };

        internal static readonly byte[,] _JAR = { { 0x50, 0x4B, 0x03, 0x04 }, { 0x50, 0x4B, 0x05, 0x06 }, { 0x50, 0x4B, 0x07, 0x08 } };
        internal static readonly byte[,] _ODT = { { 0x50, 0x4B, 0x03, 0x04 }, { 0x50, 0x4B, 0x05, 0x06 }, { 0x50, 0x4B, 0x07, 0x08 } };
        internal static readonly byte[,] _ODS = { { 0x50, 0x4B, 0x03, 0x04 }, { 0x50, 0x4B, 0x05, 0x06 }, { 0x50, 0x4B, 0x07, 0x08 } };
        internal static readonly byte[,] _ODP = { { 0x50, 0x4B, 0x03, 0x04 }, { 0x50, 0x4B, 0x05, 0x06 }, { 0x50, 0x4B, 0x07, 0x08 } };
        internal static readonly byte[,] _XLSX = { { 0x50, 0x4B, 0x03, 0x04 }, { 0x50, 0x4B, 0x05, 0x06 }, { 0x50, 0x4B, 0x07, 0x08 } };
        internal static readonly byte[,] _PPTX = { { 0x50, 0x4B, 0x03, 0x04 }, { 0x50, 0x4B, 0x05, 0x06 }, { 0x50, 0x4B, 0x07, 0x08 } };
        internal static readonly byte[,] _VSDX = { { 0x50, 0x4B, 0x03, 0x04 }, { 0x50, 0x4B, 0x05, 0x06 }, { 0x50, 0x4B, 0x07, 0x08 } };

        internal static readonly byte[,] _APK = { { 0x50, 0x4B, 0x03, 0x04 }, { 0x50, 0x4B, 0x05, 0x06 }, { 0x50, 0x4B, 0x07, 0x08 } };

        internal static readonly byte[,] _AAR = { { 0x50, 0x4B, 0x03, 0x04 }, { 0x50, 0x4B, 0x05, 0x06 }, { 0x50, 0x4B, 0x07, 0x08 } };


        internal static readonly byte[] _APNG = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        internal static readonly byte[] _CLASS = { 0xCA, 0xFE, 0xBA, 0xBE };
        internal static readonly byte[] _PS = { 0x25, 0x21, 0x50, 0x53 };

        internal static readonly byte[] _ASF = { 0x30, 0x26, 0xB2, 0x75, 0x8E, 0x66, 0xCF, 0x11, 0xA6, 0xD9, 0x00, 0xAA, 0x00, 0x62, 0xCE, 0x6C };

        internal static readonly byte[] _DEPLOYMENTIMAGE = { 0x24, 0x53, 0x44, 0x49, 0x30, 0x30, 0x30, 0x31 };

        internal static readonly byte[] _OGX = { 0x4F, 0x67, 0x67, 0x53 };
        internal static readonly byte[] _PSD = { 0x38, 0x42, 0x50, 0x53 };
        internal static readonly byte[] _CLIP = { 0x43, 0x53, 0x46, 0x43, 0x48, 0x55, 0x4e, 0x4b };

        internal static readonly byte[,] __MP3 = { { 0xFF, 0xFB }, { 0xFF, 0xF3 }, { 0xFF, 0xF2 } };

        internal static readonly byte[] _ISO = { 0x43, 0x44, 0x30, 0x30, 0x31 };
        internal static readonly byte[] _FLAC = { 0x66, 0x4C, 0x61, 0x43 };
        internal static readonly byte[] _MID = { 0x4D, 0x54, 0x68, 0x64 };
        internal static readonly byte[] _MIDI = { 0x4D, 0x54, 0x68, 0x64 };
        internal static readonly byte[] _XLS = { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
        internal static readonly byte[] _PPT = { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
        internal static readonly byte[] _MSG = { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
        internal static readonly byte[] _DEX = { 0x64, 0x65, 0x78, 0x0A, 0x30, 0x33, 0x35, 0x00 };
        internal static readonly byte[] _VMDK = { 0x4B, 0x44, 0x4D };
        internal static readonly byte[] _CRX = { 0x43, 0x72, 0x32, 0x34 };
        internal static readonly byte[] _FH8 = { 0x41, 0x47, 0x44, 0x33 };
        internal static readonly byte[] _CWK = { 0x05, 0x07, 0x00, 0x00, 0x42, 0x4F, 0x42, 0x4F, 0x05, 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
        internal static readonly byte[] _CWK_0 = { 0x06, 0x07, 0xE1, 0x00, 0x42, 0x4F, 0x42, 0x4F, 0x06, 0x07, 0xE1, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 };
        internal static readonly byte[] _TOAST_0 = { 0x45, 0x52, 0x02, 0x00, 0x00, 0x00 };
        internal static readonly byte[] _TOAST_1 = { 0x8B, 0x45, 0x52, 0x02, 0x00, 0x00, 0x00 };
        internal static readonly byte[] _DMG = { 0x78, 0x01, 0x73, 0x0D, 0x62, 0x62, 0x60 };
        internal static readonly byte[] _XAR = { 0x78, 0x61, 0x72, 0x21 };
        internal static readonly byte[] _DAT = { 0x50, 0x4D, 0x4F, 0x43, 0x43, 0x4D, 0x4F, 0x43 };
        internal static readonly byte[] _NES = { 0x4E, 0x45, 0x53, 0x1A };
        internal static readonly byte[] _TAR = { 0x75, 0x73, 0x74, 0x61, 0x72, 0x20, 0x20, 0x00 };
        internal static readonly byte[] _TOX = { 0x74, 0x6F, 0x78, 0x33 };
        internal static readonly byte[] _MIV = { 0x4D, 0x4C, 0x56, 0x49 };
        internal static readonly byte[] _WINDOWSUPDATE = { 0x44, 0x43, 0x4D, 0x01, 0x50, 0x41, 0x33, 0x30 };
        internal static readonly byte[] _7Z = { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C };
        internal static readonly byte[] _GZ = { 0x1F, 0x8B };
        internal static readonly byte[] _TARGZ = { 0x1F, 0x8B };
        internal static readonly byte[] _XZ = { 0xFD, 0x37, 0x7A, 0x58, 0x5A, 0x00, 0x00 };
        internal static readonly byte[] _TARXZ = { 0xFD, 0x37, 0x7A, 0x58, 0x5A, 0x00, 0x00 };
        internal static readonly byte[] _LZ2 = { 0x04, 0x22, 0x4D, 0x18 };
        internal static readonly byte[] _CAB = { 0x4D, 0x53, 0x43, 0x46 };
        internal static readonly byte[] _MKV = { 0x1A, 0x45, 0xDF, 0xA3 };
        internal static readonly byte[] _MKA = { 0x1A, 0x45, 0xDF, 0xA3 };
        internal static readonly byte[] _MLS = { 0x1A, 0x45, 0xDF, 0xA3 };
        internal static readonly byte[] _MK3D = { 0x1A, 0x45, 0xDF, 0xA3 };
        internal static readonly byte[] _WEBM = { 0x1A, 0x45, 0xDF, 0xA3 };
        internal static readonly byte[] _DCM = { 0x44, 0x49, 0x43, 0x4D };
        internal static readonly byte[] _XML = { 0x3C, 0x3f, 0x78, 0x6d, 0x6C, 0x20 };
        internal static readonly byte[] _WASM = { 0x00, 0x61, 0x73, 0x6d };
        internal static readonly byte[] _LEP = { 0xCF, 0x84, 0x01 };

        internal static readonly byte[] _DEB = { 0x21, 0x3C, 0x61, 0x72, 0x63, 0x68, 0x3E };
        internal static readonly byte[] _RTF = { 0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31 };
        internal static readonly byte[] _M2P = { 0x00, 0x00, 0x01, 0xBA };
        internal static readonly byte[] _VOB = { 0x00, 0x00, 0x01, 0xBA };
        internal static readonly byte[] _MPG = { 0x00, 0x00, 0x01, 0xBA };
        internal static readonly byte[,] _MPEG = { { 0x00, 0x00, 0x01, 0xBA }, { 0x00, 0x00, 0x01, 0xB3 } }; // { 0x47 },

        internal static readonly byte[,] _MOV = {   { 0x66, 0x72, 0x65, 0x65 }, { 0x6D, 0x64, 0x61, 0x74 },
                                                    { 0x6D, 0x6F, 0x6F, 0x76 }, { 0x77, 0x69, 0x64, 0x65 } };
        internal static readonly byte[] _MOVIE = { 0x66, 0x74, 0x79, 0x70, 0x71, 0x74 };

        internal static readonly byte[] _Hl2DEMO = { 0x48, 0x4C, 0x32, 0x44, 0x45, 0x4D, 0x4F };
        internal static readonly byte[,] TXT = { { 0xFF, 0xFE }, { 0xFE, 0xFF } };
        internal static readonly byte[,] TEXT = { { 0xFF, 0xFE, 0x00, 0x00 }, { 0x00, 0x00, 0xFE, 0xFF } };

        internal static readonly byte[] _SUBRIP = { 0x31, 0x0D, 0x0A, 0x30, 0x30, 0x3A };
        internal static readonly byte[,] _WEBVTT = {    { 0xEF, 0xBB, 0xBF, 0x57, 0x45, 0x42, 0x56, 0x54, 0x54, 0x0A },
                                                        { 0xEF, 0xBB, 0xBF, 0x57, 0x45, 0x42, 0x56, 0x54, 0x54, 0x0D },
                                                        { 0xEF, 0xBB, 0xBF, 0x57, 0x45, 0x42, 0x56, 0x54, 0x54, 0x20 },
                                                        { 0xEF, 0xBB, 0xBF, 0x57, 0x45, 0x42, 0x56, 0x54, 0x54, 0x09 } };
        internal static readonly byte[,] _WEB_VTT = {   { 0x57, 0x45, 0x42, 0x56, 0x54, 0x54, 0x0A },
                                                        { 0x57, 0x45, 0x42, 0x56, 0x54, 0x54, 0x0D },
                                                        { 0x57, 0x45, 0x42, 0x56, 0x54, 0x54, 0x20 },
                                                        { 0x57, 0x45, 0x42, 0x56, 0x54, 0x54, 0x09 } };


        internal static readonly byte[,] _JSON = { { 0x7B }, { 0x5B } };
        internal static readonly byte[] _ELF = { 0x7F, 0x45, 0x4C, 0x46 };
        internal static readonly byte[,] _MACH = {      { 0xFE, 0xED, 0xFA, 0xCE }, { 0xFE, 0xED, 0xFA, 0xCF },
                                                        { 0xCE, 0xFA, 0xED, 0xFE }, { 0xCF, 0xFA, 0xED, 0xFE },
                                                        { 0xFE, 0xED, 0xFA, 0xCE }, { 0xFE, 0xED, 0xFA, 0xCF } };

        internal static readonly byte[] _EML = { 0x52, 0x65, 0x63, 0x65, 0x69, 0x76, 0x65, 0x64, 0x3A };
        internal static readonly byte[] _SVG = { 0x3c, 0x73, 0x76, 0x67 };
        internal static readonly byte[] _AVIF = { 0x66, 0x74, 0x79, 0x70, 0x61, 0x76, 0x69, 0x66 };

        internal static readonly byte[,] _TEXT = {  { 0x09 }, { 0x0a }, { 0x0d }, 
                                                    { 0x20 }, { 0x21 }, { 0x22 }, { 0x23 }, { 0x24 }, { 0x25 }, { 0x26 }, { 0x27 },
                                                    { 0x28 }, { 0x29 }, { 0x2A }, { 0x2B }, { 0x2C }, { 0x2D }, { 0x2E }, { 0x2F },
                                                    { 0x30 }, { 0x31 }, { 0x32 }, { 0x33 }, { 0x34 }, { 0x35 }, { 0x36 }, { 0x37 },
                                                    { 0x38 }, { 0x39 }, { 0x3A }, { 0x3B }, { 0x3C }, { 0x3D }, { 0x3E }, { 0x3F },
                                                    { 0x40 }, { 0x41 }, { 0x42 }, { 0x43 }, { 0x44 }, { 0x45 }, { 0x46 }, { 0x47 },
                                                    { 0x48 }, { 0x49 }, { 0x4A }, { 0x4B }, { 0x4C }, { 0x4D }, { 0x4E }, { 0x4F },
                                                    { 0x50 }, { 0x51 }, { 0x52 }, { 0x53 }, { 0x54 }, { 0x55 }, { 0x56 }, { 0x57 },
                                                    { 0x58 }, { 0x59 }, { 0x5A }, { 0x5B }, { 0x5C }, { 0x5D }, { 0x5E }, { 0x5F },
                                                    { 0x60 }, { 0x61 }, { 0x62 }, { 0x63 }, { 0x64 }, { 0x65 }, { 0x66 }, { 0x67 },
                                                    { 0x68 }, { 0x69 }, { 0x6A }, { 0x6B }, { 0x6C }, { 0x6D }, { 0x6E }, { 0x6F },
                                                    { 0x70 }, { 0x71 }, { 0x72 }, { 0x73 }, { 0x74 }, { 0x75 }, { 0x76 }, { 0x77 },
                                                    { 0x78 }, { 0x79 }, { 0x7A }, { 0x7B }, { 0x7C }, { 0x7D }, { 0x7E }, { 0x7F }   };
        #endregion static internal readonly

            #region public static MimeTypeExtSig[] MimeSignatureMap

        public static MimeTypeExtSig[] MimeSignatureMap =
        {
            new MimeTypeExtSig(".eva", "application/x-eva", MimeSignature._EVA ),
            new MimeTypeExtSig(".gif", "image/gif", MimeSignature._GIF),
            new MimeTypeExtSig(".ico", "image/x-icon", MimeSignature._ICO),
            new MimeTypeExtSig(".jpg", "image/jpg", MimeSignature._JPG),
            new MimeTypeExtSig(".jpeg","image/jpeg", MimeSignature._JPG),
            new MimeTypeExtSig(".bmp",  "image/bmp", MimeSignature._BMP),
            new MimeTypeExtSig(".webp", "image/webp", MimeSignature._WEBP),
            new MimeTypeExtSig(".heif", "image/heif", MimeSignature._HEIF),
            new MimeTypeExtSig(".heifs", "image/heif-sequence", MimeSignature._HEIF),
            new MimeTypeExtSig(".tif", "image/tiff", MimeSignature._TIF),
            new MimeTypeExtSig(".tiff", "image/tiff", MimeSignature._TIF),
            new MimeTypeExtSig(".wav", "audio/wav", MimeSignature._WAV),
            new MimeTypeExtSig(".mp3", "audio/mpeg", MimeSignature._MP3),
            new MimeTypeExtSig(".wave", "audio/wav", MimeSignature._WAV),
            new MimeTypeExtSig(".wma", "audio/x-ms-wma", MimeSignature._WMA),
            new MimeTypeExtSig(".mid", "audio/mid", MimeSignature._MID),
            new MimeTypeExtSig(".midi", "audio/mid", MimeSignature._MID),
            new MimeTypeExtSig(".weba", "audio/webm", MimeSignature._WEBM),
            new MimeTypeExtSig(".3ga", "audio/mp4", MimeSignature._MP4),
            new MimeTypeExtSig(".ogg", "audio/ogg", MimeSignature._OGG),
            new MimeTypeExtSig(".oga", "audio/ogg", MimeSignature._OGG),
            new MimeTypeExtSig(".ogx", "application/ogg", MimeSignature._OGG),
            new MimeTypeExtSig(".ogv", "video/ogg", MimeSignature._OGG),
            new MimeTypeExtSig(".wmv", "video/x-ms-wmv", MimeSignature._WMV),
            new MimeTypeExtSig(".f4v", "video/mp4", MimeSignature._MP4),
            new MimeTypeExtSig(".webm", "video/webm", MimeSignature._WEBM),
            new MimeTypeExtSig(".3gpa", "audio/mp4", MimeSignature._MP4),
            new MimeTypeExtSig(".mp4", "video/mp4", MimeSignature._MP4),
            new MimeTypeExtSig(".mp4a", "audio/mp4", MimeSignature._MP4),
            new MimeTypeExtSig(".mp4s", "application/mp4", MimeSignature._MP4),
            new MimeTypeExtSig(".mp4v", "video/mp4", MimeSignature._MP4),
            new MimeTypeExtSig(".mpg4", "video/mp4", MimeSignature._MP4),
            new MimeTypeExtSig(".3gp", "video/3gpp", MimeSignature._3PG),
            new MimeTypeExtSig(".3gp2", "video/3gpp2", MimeSignature._3PG),
            new MimeTypeExtSig(".3gpp", "video/3gpp", MimeSignature._3PG),

            new MimeTypeExtSig(".swf", "application/x-shockwave-flash", MimeSignature._SWF),

            new MimeTypeExtSig(".ttc", "application/x-font-ttf", MimeSignature._TTF),
            new MimeTypeExtSig(".ttf", "application/x-font-ttf", MimeSignature._TTF),

            new MimeTypeExtSig(".doc", "application/msword", MimeSignature._DOC),
            new MimeTypeExtSig(".docm", "application/vnd.ms-word.document.macroEnabled.12", MimeSignature._DOCX),
            new MimeTypeExtSig(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", MimeSignature._DOCX),
            new MimeTypeExtSig(".dot", "application/msword", MimeSignature._DOCX),
            new MimeTypeExtSig(".dotm", "application/vnd.ms-word.template.macroEnabled.12", MimeSignature._DOCX),
            new MimeTypeExtSig(".dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template", MimeSignature._DOCX),
            new MimeTypeExtSig(".xls", "application/vnd.ms-excel",  MimeSignature._XLS),
            new MimeTypeExtSig(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", MimeSignature._XLSX),
            new MimeTypeExtSig(".xl", "application/excel", MimeSignature._XLS),
            new MimeTypeExtSig(".xla", "application/vnd.ms-excel", MimeSignature._XLSX),
            new MimeTypeExtSig(".xlam", "application/vnd.ms-excel.addin.macroEnabled.12",_XLSX),
            new MimeTypeExtSig(".xlb", "application/excel", MimeSignature._XLSX),
            new MimeTypeExtSig(".xlc", "application/vnd.ms-excel", MimeSignature._XLSX),
            new MimeTypeExtSig(".xld", "application/vnd.ms-excel", MimeSignature._XLSX),
            new MimeTypeExtSig(".xlk", "application/vnd.ms-excel", MimeSignature._XLSX),
            new MimeTypeExtSig(".xll", "application/vnd.ms-excel", MimeSignature._XLSX),
            new MimeTypeExtSig(".xlm", "application/vnd.ms-excel", MimeSignature._XLSX),
            new MimeTypeExtSig(".xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12", MimeSignature._XLSX),
            new MimeTypeExtSig(".xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12", MimeSignature._XLSX),
            new MimeTypeExtSig(".xlt", "application/vnd.ms-excel", MimeSignature._XLSX),
            new MimeTypeExtSig(".xltm", "application/vnd.ms-excel.template.macroEnabled.12", MimeSignature._XLSX),
            new MimeTypeExtSig(".xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template", MimeSignature._XLSX),
            new MimeTypeExtSig(".xlv", "application/excel", MimeSignature._XLSX),
            new MimeTypeExtSig(".xlw", "application/vnd.ms-excel", MimeSignature._XLSX),
            new MimeTypeExtSig(".ppt", "application/vnd.ms-powerpoint", MimeSignature._PPT),
            new MimeTypeExtSig(".ppt", "application/vnd.ms-powerpoint.presentation.macroEnabled.12", MimeSignature._PPT),
            new MimeTypeExtSig(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", MimeSignature._PPTX),
            new MimeTypeExtSig(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", MimeSignature._PPTX),
            new MimeTypeExtSig(".vsd", "application/vnd.visio", MimeSignature._VSDX),
            new MimeTypeExtSig(".vsw", "application/vnd.visio", MimeSignature._VSDX),
            new MimeTypeExtSig(".vsd", "application/vnd.visio", MimeSignature._VSDX),
            new MimeTypeExtSig(".vsx", "application/vnd.visio", MimeSignature._VSDX),
            new MimeTypeExtSig(".vtx", "application/vnd.visio", MimeSignature._VSDX),
            new MimeTypeExtSig(".vsdx", "application/vnd.ms-visio.viewer",  _VSDX),
            new MimeTypeExtSig(".vds", "model/vnd.sap.vds", MimeSignature._VSDX),
            new MimeTypeExtSig(".vdx", "application/vnd.ms-visio.viewer", MimeSignature._VSDX),
            new MimeTypeExtSig(".vsto", "application/x-ms-vsto", MimeSignature._VSDX),
            new MimeTypeExtSig(".vss", "application/vnd.visio", MimeSignature._VSDX),
            new MimeTypeExtSig(".vst", "application/vnd.visio", MimeSignature._VSDX),

            new MimeTypeExtSig(".pdf", "application/pdf", MimeSignature._PDF),
            new MimeTypeExtSig(".xrm-ms", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".xsc", "application/xml", MimeSignature._XML),
            new MimeTypeExtSig(".xsd", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".xsf", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".xsl", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".xslt", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".vml", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".disco", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".mno", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".SSISDeploymentManifest", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".vsixlangpack", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".vsixmanifest", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".vssettings", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".vstemplate", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".AddIn", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".exe.config",   "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".dll.config", "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".dtd", "text/xml" , MimeSignature._XML),
            new MimeTypeExtSig(".dtsConfig",    "text/xml", MimeSignature._XML),
            new MimeTypeExtSig(".wsdl",  "text/xml" , MimeSignature._XML),
            new MimeTypeExtSig(".vsct",  "text/xml" , MimeSignature._XML),



            new MimeTypeExtSig(".gz", "application/x-gzip", MimeSignature._GZ),
            new MimeTypeExtSig(".tar", "application/x-tar", MimeSignature._TAR),
            new MimeTypeExtSig(".tar.gz", "application/gzip", MimeSignature._GZ),
            new MimeTypeExtSig(".gzip", "application/x-gzip", MimeSignature._GZ),
            new MimeTypeExtSig(".bz", "application/x-bzip", MimeSignature._BZ2),
            new MimeTypeExtSig(".bz2", "application/x-bzip2", MimeSignature._BZ2),
            new MimeTypeExtSig(".xz", "application/x-xz", MimeSignature._XZ),
            new MimeTypeExtSig(".7z", "application/x-7z-compressed", MimeSignature._7Z),
            new MimeTypeExtSig(".7zip", "application/x-7z-compressed", MimeSignature._7Z),
            new MimeTypeExtSig(".s7z", "application/x-7z-compressed", MimeSignature._7Z),
            new MimeTypeExtSig(".zip", "application/zip", MimeSignature._ZIP),
            new MimeTypeExtSig(".rar", "application/x-rar-compressed", MimeSignature._RAR),
            new MimeTypeExtSig(".jar", "application/java-archive", MimeSignature._JAR),

            new MimeTypeExtSig(".iso", "application/x-iso9660-image", MimeSignature._ISO),

            new MimeTypeExtSig(".hxx", "text/plain", MimeSignature._TEXT),
            new MimeTypeExtSig(".i", "text/plain", MimeSignature._TEXT),
            new MimeTypeExtSig(".idc", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".idl", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".in",  "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".inc", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".ini", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".inl", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".ipproj", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".po",  "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".py",  "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".rc",  "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".rc2", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".rct", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".vb",  "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".vbdproj",   "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".vcs", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".vddproj",   "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".vdp", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".vdproj", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".vspscc", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".vsscc", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".vssscc", "text/plain" , MimeSignature._TEXT),

            new MimeTypeExtSig(".torrent", "application/x-bittorrent", MimeSignature._TORRENT)
        };


        /*
            new MimeTypeExtSig(".3ds", "image/x-3ds" },
            new MimeTypeExtSig(".3g2", "video/3gpp2" },

            
            new MimeTypeExtSig(".a",   "application/octet-stream" },
            new MimeTypeExtSig(".aa",  "audio/audible" },
            new MimeTypeExtSig(".aab", "application/x-authorware-bin" },
            new MimeTypeExtSig(".aac", "audio/aac" },
            new MimeTypeExtSig(".aaf", "application/octet-stream" },
            new MimeTypeExtSig(".aam", "application/x-authorware-map" },
            new MimeTypeExtSig(".aas", "application/x-authorware-seg" },
            new MimeTypeExtSig(".aax", "audio/vnd.audible.aax" },
            new MimeTypeExtSig(".abc", "text/vndabc" },
            new MimeTypeExtSig(".abw", "application/x-abiword" },
            new MimeTypeExtSig(".ac",  "application/pkix-attr-cert" },
            new MimeTypeExtSig(".ac3", "audio/ac3" },
            new MimeTypeExtSig(".aca", "application/octet-stream" },
            new MimeTypeExtSig(".acc", "application/vnd.americandynamics.acc" },
            new MimeTypeExtSig(".accda", "application/msaccess.addin" },
            new MimeTypeExtSig(".accdb", "application/msaccess" },
            new MimeTypeExtSig(".accdc", "application/msaccess.cab" },
            new MimeTypeExtSig(".accde", "application/msaccess" },
            new MimeTypeExtSig(".accdr", "application/msaccess.runtime" },
            new MimeTypeExtSig(".accdt", "application/msaccess" },
            new MimeTypeExtSig(".accdw", "application/msaccess.webapplication" },
            new MimeTypeExtSig(".accft", "application/msaccess.ftemplate" },
            new MimeTypeExtSig(".ace", "application/x-ace-compressed" },
            new MimeTypeExtSig(".acgi", "text/html" },
            new MimeTypeExtSig(".acu", "application/vnd.acucobol" },
            new MimeTypeExtSig(".acutc", "application/vnd.acucorp" },
            new MimeTypeExtSig(".acx", "application/internet-property-stream" },

            new MimeTypeExtSig(".ade", "application/msaccess" },
            new MimeTypeExtSig(".adobebridge",   "application/x-bridge-url" },
            new MimeTypeExtSig(".adp", "application/msaccess" },
            new MimeTypeExtSig(".ADT", "audio/vnd.dlna.adts" },
            new MimeTypeExtSig(".ADTS", "audio/aac" },
            new MimeTypeExtSig(".aep", "application/vnd.audiograph" },
            new MimeTypeExtSig(".aff", "audio/aiff" },
            new MimeTypeExtSig(".afl", "video/animaflex" },
            new MimeTypeExtSig(".afm", "application/octet-stream" },
            new MimeTypeExtSig(".afp", "application/vnd.ibm.modcap" },
            new MimeTypeExtSig(".age", "application/vnd.age" },
            new MimeTypeExtSig(".ahead", "application/vnd.ahead.space" },
            new MimeTypeExtSig(".ai",  "application/postscript" },
            new MimeTypeExtSig(".aif", "audio/aiff" },
            new MimeTypeExtSig(".aifc", "audio/aiff" },
            new MimeTypeExtSig(".aiff", "audio/aiff" },
            new MimeTypeExtSig(".aim", "application/x-aim" },
            new MimeTypeExtSig(".aip", "text/x-audiosoft-intra" },
            new MimeTypeExtSig(".air", "application/vnd.adobe.air-application-installer-package+zip" },
            new MimeTypeExtSig(".ait", "application/vnd.dvb.ait" },
            new MimeTypeExtSig(".alz", "application/x-alz-compressed" },
            new MimeTypeExtSig(".amc", "application/mpeg" },
            new MimeTypeExtSig(".ami", "application/vnd.amiga.ami" },
            new MimeTypeExtSig(".aml", "application/automationml-aml+xml" },
            new MimeTypeExtSig(".amlx", "application/automationml-amlx+zip" },
            new MimeTypeExtSig(".amr", "audio/amr" },
            new MimeTypeExtSig(".ani", "application/x-navi-animation" },
            new MimeTypeExtSig(".anx", "application/annodex" },
            new MimeTypeExtSig(".aos", "application/x-nokia-9000-communicator-add-on-software" },
            new MimeTypeExtSig(".apk", "application/vnd.android.package-archive" },
            new MimeTypeExtSig(".apng", "image/apng" },
            new MimeTypeExtSig(".appcache",  "text/cache-manifest" },
            new MimeTypeExtSig(".appinstaller",  "application/appinstaller" },
            new MimeTypeExtSig(".application",   "application/x-ms-application" },
            new MimeTypeExtSig(".appx", "application/appx" },
            new MimeTypeExtSig(".appxbundle",    "application/appxbundle" },
            new MimeTypeExtSig(".apr", "application/vnd.lotus-approach" },
            new MimeTypeExtSig(".aps", "application/mime" },
            new MimeTypeExtSig(".arc", "application/x-freearc" },
            new MimeTypeExtSig(".arj", "application/x-arj" },
            new MimeTypeExtSig(".art", "image/x-jg" },
            new MimeTypeExtSig(".arw", "image/x-sony-arw" },
            new MimeTypeExtSig(".asa", "application/xml" },
            new MimeTypeExtSig(".asax", "application/xml" },
            new MimeTypeExtSig(".asc", "application/pgp-signature" },
            new MimeTypeExtSig(".ascx", "application/xml" },
            new MimeTypeExtSig(".asd", "application/octet-stream" },
            new MimeTypeExtSig(".asf", "video/x-ms-asf" },
            new MimeTypeExtSig(".ashx", "application/xml" },
            new MimeTypeExtSig(".asi", "application/octet-stream" },
            new MimeTypeExtSig(".asm", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".asmx", "application/xml" },
            new MimeTypeExtSig(".aso", "application/vnd.accpac.simply.aso" },
            new MimeTypeExtSig(".asp", "text/asp" },
            new MimeTypeExtSig(".aspx", "application/xml" },
            new MimeTypeExtSig(".asr", "video/x-ms-asf" },
            new MimeTypeExtSig(".asx", "video/x-ms-asf" },
            new MimeTypeExtSig(".atc", "application/vnd.acucorp" },
            new MimeTypeExtSig(".atom", "application/atom+xml" },
            new MimeTypeExtSig(".atomcat",   "application/atomcat+xml" },
            new MimeTypeExtSig(".atomdeleted",   "application/atomdeleted+xml" },
            new MimeTypeExtSig(".atomsvc",   "application/atomsvc+xml" },
            new MimeTypeExtSig(".atx", "application/vnd.antix.game-component" },
            new MimeTypeExtSig(".au",  "audio/basic" },
            new MimeTypeExtSig(".avci", "image/avci" },
            new MimeTypeExtSig(".avcs", "image/avcs" },
            new MimeTypeExtSig(".avi", "video/x-msvideo" },
            new MimeTypeExtSig(".avif", "image/avif" },
            new MimeTypeExtSig(".avifs", "image/avif-sequence" },
            new MimeTypeExtSig(".avs", "video/avs-video" },
            new MimeTypeExtSig(".aw",  "application/applixware" },
            new MimeTypeExtSig(".axa", "audio/annodex" },
            new MimeTypeExtSig(".axs", "application/olescript" },
            new MimeTypeExtSig(".axv", "video/annodex" },
            new MimeTypeExtSig(".azf", "application/vnd.airzip.filesecure.azf" },
            new MimeTypeExtSig(".azs", "application/vnd.airzip.filesecure.azs" },
            new MimeTypeExtSig(".azv", "image/vnd.airzip.accelerator.azv" },
            new MimeTypeExtSig(".azw", "application/vnd.amazon.ebook" },
            new MimeTypeExtSig(".b16", "image/vnd.pco.b16" },
            new MimeTypeExtSig(".bas", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".bat", "application/x-msdownload" },
            new MimeTypeExtSig(".bcpio", "application/x-bcpio" },
            new MimeTypeExtSig(".bdf", "application/x-font-bdf" },
            new MimeTypeExtSig(".bdm", "application/vnd.syncml.dm+wbxml" },
            new MimeTypeExtSig(".bdoc", "application/x-bdoc" },
            new MimeTypeExtSig(".bed", "application/vnd.realvnc.bed" },
            new MimeTypeExtSig(".bh2", "application/vnd.fujitsu.oasysprs" },
            new MimeTypeExtSig(".bib", "text/x-bibtex" },
            new MimeTypeExtSig(".bin", "application/octet-stream" },
            new MimeTypeExtSig(".blb", "application/x-blorb" },
            new MimeTypeExtSig(".blorb", "application/x-blorb" },
            new MimeTypeExtSig(".bmi", "application/vnd.bmi" },
            new MimeTypeExtSig(".bmml", "application/vnd.balsamiq.bmml+xml" },

            new MimeTypeExtSig(".boo", "text/x-boo" },
            new MimeTypeExtSig(".book", "application/vnd.framemaker" },
            new MimeTypeExtSig(".box", "application/vnd.previewsystems.box" },
            new MimeTypeExtSig(".boz", "application/x-bzip2" },
            new MimeTypeExtSig(".bpk", "application/octet-stream" },
            new MimeTypeExtSig(".bsh", "application/x-bsh" },
            new MimeTypeExtSig(".bsp", "model/vnd.valve.source.compiled-map" },
            new MimeTypeExtSig(".btf", "image/prs.btif" },
            new MimeTypeExtSig(".btif", "image/prs.btif" },
            new MimeTypeExtSig(".buffer", "application/octet-stream" },

            new MimeTypeExtSig(".c",   "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".c++", "text/x-c++src" },
            new MimeTypeExtSig(".c11amc", "application/vnd.cluetrust.cartomobile-config" },
            new MimeTypeExtSig(".c11amz", "application/vnd.cluetrust.cartomobile-config-pkg" },
            new MimeTypeExtSig(".c4d", "application/vnd.clonk.c4group" },
            new MimeTypeExtSig(".c4f", "application/vnd.clonk.c4group" },
            new MimeTypeExtSig(".c4g", "application/vnd.clonk.c4group" },
            new MimeTypeExtSig(".c4p", "application/vnd.clonk.c4group" },
            new MimeTypeExtSig(".c4u", "application/vnd.clonk.c4group" },
            new MimeTypeExtSig(".cab", "application/octet-stream" },
            new MimeTypeExtSig(".caf", "audio/x-caf" },
            new MimeTypeExtSig(".calx", "application/vnd.ms-office.calx" },
            new MimeTypeExtSig(".cap", "application/vnd.tcpdump.pcap" },
            new MimeTypeExtSig(".car", "application/vnd.curl.car" },
            new MimeTypeExtSig(".cat", "application/vnd.ms-pki.seccat" },
            new MimeTypeExtSig(".cb7", "application/x-cb7" },
            new MimeTypeExtSig(".cba", "application/x-cbr" },
            new MimeTypeExtSig(".cbr", "application/x-cbr" },
            new MimeTypeExtSig(".cbt", "application/x-cbt" },
            new MimeTypeExtSig(".cbz", "application/x-cbz" },
            new MimeTypeExtSig(".cc",  "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".ccad", "application/clariscad" },
            new MimeTypeExtSig(".cco", "application/x-cocoa" },
            new MimeTypeExtSig(".cct", "application/x-director" },
            new MimeTypeExtSig(".ccxml", "application/ccxml+xml" },
            new MimeTypeExtSig(".cd",  "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".cdbcmsg",   "application/vnd.contact.cmsg" },
            new MimeTypeExtSig(".cdda", "audio/aiff" },
            new MimeTypeExtSig(".cdf", "application/x-cdf" },
            new MimeTypeExtSig(".cdfx", "application/cdfx+xml" },
            new MimeTypeExtSig(".cdkey", "application/vnd.mediastation.cdkey" },
            new MimeTypeExtSig(".cdmia", "application/cdmi-capability" },
            new MimeTypeExtSig(".cdmic", "application/cdmi-container" },
            new MimeTypeExtSig(".cdmid", "application/cdmi-domain" },
            new MimeTypeExtSig(".cdmio", "application/cdmi-object" },
            new MimeTypeExtSig(".cdmiq", "application/cdmi-queue" },
            new MimeTypeExtSig(".cdr", "image/x-coreldraw" },
            new MimeTypeExtSig(".cdt", "image/x-coreldrawtemplate" },
            new MimeTypeExtSig(".cdx", "chemical/x-cdx" },
            new MimeTypeExtSig(".cdxml", "application/vnd.chemdraw+xml" },
            new MimeTypeExtSig(".cdy", "application/vnd.cinderella" },
            new MimeTypeExtSig(".cer", "application/x-x509-ca-cert" },
            new MimeTypeExtSig(".cfg", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".cfs", "application/x-cfs-compressed" },
            new MimeTypeExtSig(".cgm", "image/cgm" },
            new MimeTypeExtSig(".cha", "application/x-chat" },
            new MimeTypeExtSig(".chat", "application/x-chat" },
            new MimeTypeExtSig(".chm", "application/octet-stream" },
            new MimeTypeExtSig(".chrt", "application/vnd.kde.kchart" },
            new MimeTypeExtSig(".cif", "chemical/x-cif" },
            new MimeTypeExtSig(".cii", "application/vnd.anser-web-certificate-issue-initiation" },
            new MimeTypeExtSig(".cil", "application/vnd.ms-artgalry" },
            new MimeTypeExtSig(".cjs", "application/node" },
            new MimeTypeExtSig(".cla", "application/vnd.claymore" },
            new MimeTypeExtSig(".class", "application/x-java-applet" },
            new MimeTypeExtSig(".cld", "model/vnd.cld" },
            new MimeTypeExtSig(".clkk", "application/vnd.crick.clicker.keyboard" },
            new MimeTypeExtSig(".clkp", "application/vnd.crick.clicker.palette" },
            new MimeTypeExtSig(".clkt", "application/vnd.crick.clicker.template" },
            new MimeTypeExtSig(".clkw", "application/vnd.crick.clicker.wordbank" },
            new MimeTypeExtSig(".clkx", "application/vnd.crick.clicker" },
            new MimeTypeExtSig(".clp", "application/x-msclip" },
            new MimeTypeExtSig(".cls", "text/x-tex" },
            new MimeTypeExtSig(".cmc", "application/vnd.cosmocaller" },
            new MimeTypeExtSig(".cmd", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".cmdf", "chemical/x-cmdf" },
            new MimeTypeExtSig(".cml", "chemical/x-cml" },
            new MimeTypeExtSig(".cmp", "application/vnd.yellowriver-custom-menu" },
            new MimeTypeExtSig(".cmx", "image/x-cmx" },
            new MimeTypeExtSig(".cnf", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".cod", "image/cis-cod" },
            new MimeTypeExtSig(".coffee", "text/coffeescript" },
            new MimeTypeExtSig(".com", "application/x-msdownload" },
            new MimeTypeExtSig(".conf", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".config", "application/xml" },
            new MimeTypeExtSig(".contact",   "text/x-ms-contact" },
            new MimeTypeExtSig(".coverage",  "application/xml" },
            new MimeTypeExtSig(".cpio", "application/x-cpio" },
            new MimeTypeExtSig(".cpl", "application/cpl+xml" },
            new MimeTypeExtSig(".cpp", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".cpt", "application/mac-compactpro" },
            new MimeTypeExtSig(".cr2", "image/x-canon-cr2" },
            new MimeTypeExtSig(".cr3", "image/x-canon-cr3" },
            new MimeTypeExtSig(".crd", "application/x-mscardfile" },
            new MimeTypeExtSig(".crl", "application/pkix-crl" },
            new MimeTypeExtSig(".crt", "application/x-x509-ca-cert" },
            new MimeTypeExtSig(".crw", "image/x-canon-crw" },
            new MimeTypeExtSig(".crx", "application/x-chrome-extension" },
            new MimeTypeExtSig(".cryptonote",    "application/vnd.rig.cryptonote" },
            new MimeTypeExtSig(".cs",  "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".csdproj",   "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".csh", "application/x-csh" },
            new MimeTypeExtSig(".csl", "application/vnd.citationstyles.style+xml" },
            new MimeTypeExtSig(".csml", "chemical/x-csml" },
            new MimeTypeExtSig(".csp", "application/vnd.commonspace" },
            new MimeTypeExtSig(".csproj", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".css", "text/css" },
            new MimeTypeExtSig(".cst", "application/x-director" },
            new MimeTypeExtSig(".csv", "text/csv" },
            new MimeTypeExtSig(".cu",  "application/cu-seeme" },
            new MimeTypeExtSig(".cur", "application/octet-stream" },
            new MimeTypeExtSig(".curl", "text/vnd.curl" },
            new MimeTypeExtSig(".cwl", "application/cwl" },
            new MimeTypeExtSig(".cww", "application/prs.cww" },
            new MimeTypeExtSig(".cxt", "application/x-director" },
            new MimeTypeExtSig(".cxx", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".czx", "application/x-czx" },
            new MimeTypeExtSig(".d",   "text/x-dsrc" },
            new MimeTypeExtSig(".dae", "model/vnd.collada+xml" },
            new MimeTypeExtSig(".daf", "application/vnd.mobius.daf" },
            new MimeTypeExtSig(".dar", "application/x-dar" },
            new MimeTypeExtSig(".dart", "application/vnd.dart" },
            new MimeTypeExtSig(".dat", "application/octet-stream" },
            new MimeTypeExtSig(".dataless",  "application/vnd.fdsn.seed" },
            new MimeTypeExtSig(".datasource",    "application/xml" },
            new MimeTypeExtSig(".davmount",  "application/davmount+xml" },
            new MimeTypeExtSig(".db",  "application/vnd.sqlite3" },
            new MimeTypeExtSig(".db-shm", "application/vnd.sqlite3" },
            new MimeTypeExtSig(".db-wal", "application/vnd.sqlite3" },
            new MimeTypeExtSig(".dbf", "application/vnd.dbf" },
            new MimeTypeExtSig(".dbk", "application/docbook+xml" },
            new MimeTypeExtSig(".dbproj", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".dcr", "application/x-director" },
            new MimeTypeExtSig(".dcurl", "text/vnd.curl.dcurl" },
            new MimeTypeExtSig(".dd2", "application/vnd.oma.dd2+xml" },
            new MimeTypeExtSig(".ddd", "application/vnd.fujixerox.ddd" },
            new MimeTypeExtSig(".ddf", "application/vnd.syncml.dmddf+xml" },
            new MimeTypeExtSig(".dds", "image/vnd.ms-dds" },
            new MimeTypeExtSig(".deb", "application/x-debian-package" },
            new MimeTypeExtSig(".deepv", "application/x-deepv" },
            new MimeTypeExtSig(".def", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".deploy", "application/octet-stream" },
            new MimeTypeExtSig(".der", "application/x-x509-ca-cert" },
            new MimeTypeExtSig(".dfac", "application/vnd.dreamfactory" },
            new MimeTypeExtSig(".dgc", "application/x-dgc-compressed" },
            new MimeTypeExtSig(".dgml", "application/xml" },
            new MimeTypeExtSig(".dib", "image/bmp" },
            new MimeTypeExtSig(".dic", "text/x-c" },
            new MimeTypeExtSig(".dif", "video/x-dv" },
            new MimeTypeExtSig(".diff", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".dir", "application/x-director" },
            new MimeTypeExtSig(".dis", "application/vnd.mobius.dis" },

            new MimeTypeExtSig(".disposition-notification", "message/disposition-notification" },
            new MimeTypeExtSig(".dist", "application/octet-stream" },
            new MimeTypeExtSig(".distz", "application/octet-stream" },
            new MimeTypeExtSig(".divx", "video/divx" },
            new MimeTypeExtSig(".djv", "image/vnd.djvu" },
            new MimeTypeExtSig(".djvu", "image/vnd.djvu" },
            new MimeTypeExtSig(".dl",  "video/dl" },
            new MimeTypeExtSig(".dll", "application/x-msdownload" },

            new MimeTypeExtSig(".dlm", "text/dlm" },
            new MimeTypeExtSig(".dmg", "application/octet-stream" },
            new MimeTypeExtSig(".dmp", "application/vnd.tcpdump.pcap" },
            new MimeTypeExtSig(".dms", "application/octet-stream" },
            new MimeTypeExtSig(".dna", "application/vnd.dna" },
            new MimeTypeExtSig(".dng", "image/x-adobe-dng" },


            new MimeTypeExtSig(".dp",  "application/vnd.osgi.dp" },
            new MimeTypeExtSig(".dpg", "application/vnd.dpgraph" },
            new MimeTypeExtSig(".dpx", "image/dpx" },
            new MimeTypeExtSig(".dra", "audio/vnd.dra" },
            new MimeTypeExtSig(".drle", "image/dicom-rle" },
            new MimeTypeExtSig(".drw", "application/drafting" },
            new MimeTypeExtSig(".dsc", "text/prs.lines.tag" },
            new MimeTypeExtSig(".dsp", "application/octet-stream" },
            new MimeTypeExtSig(".dssc", "application/dssc+der" },
            new MimeTypeExtSig(".dsw", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".dtb", "application/x-dtbook+xml" },

            new MimeTypeExtSig(".dts", "audio/vnd.dts" },

            new MimeTypeExtSig(".dtshd", "audio/vnd.dts.hd" },
            new MimeTypeExtSig(".dump", "application/octet-stream" },
            new MimeTypeExtSig(".dv",  "video/x-dv" },
            new MimeTypeExtSig(".dvb", "video/vnd.dvb.file" },
            new MimeTypeExtSig(".dvi", "application/x-dvi" },
            new MimeTypeExtSig(".dwd", "application/atsc-dwd+xml" },
            new MimeTypeExtSig(".dwf", "drawing/x-dwf" },
            new MimeTypeExtSig(".dwg", "application/acad" },
            new MimeTypeExtSig(".dwp", "application/octet-stream" },
            new MimeTypeExtSig(".dxf", "application/x-dxf" },
            new MimeTypeExtSig(".dxp", "application/vnd.spotfire.dxp" },
            new MimeTypeExtSig(".dxr", "application/x-director" },
            new MimeTypeExtSig(".ear", "application/java-archive" },
            new MimeTypeExtSig(".ecelp4800", "audio/vnd.nuera.ecelp4800" },
            new MimeTypeExtSig(".ecelp7470", "audio/vnd.nuera.ecelp7470" },
            new MimeTypeExtSig(".ecelp9600", "audio/vnd.nuera.ecelp9600" },
            new MimeTypeExtSig(".ecma", "application/ecmascript" },
            new MimeTypeExtSig(".edm", "application/vnd.novadigm.edm" },
            new MimeTypeExtSig(".edx", "application/vnd.novadigm.edx" },
            new MimeTypeExtSig(".efif", "application/vnd.picsel" },
            new MimeTypeExtSig(".ei6", "application/vnd.pg.osasli" },
            new MimeTypeExtSig(".el",  "text/x-scriptelisp" },
            new MimeTypeExtSig(".elc", "application/octet-stream" },
            new MimeTypeExtSig(".emf", "image/emf" },
            new MimeTypeExtSig(".eml", "message/rfc822" },
            new MimeTypeExtSig(".emma", "application/emma+xml" },
            new MimeTypeExtSig(".emotionml", "application/emotionml+xml" },
            new MimeTypeExtSig(".emz", "application/octet-stream" },
            new MimeTypeExtSig(".env", "application/x-envoy" },
            new MimeTypeExtSig(".eol", "audio/vnd.digital-winds" },
            new MimeTypeExtSig(".eot", "application/vnd.ms-fontobject" },
            new MimeTypeExtSig(".eps", "application/postscript" },
            new MimeTypeExtSig(".epub", "application/epub+zip" },
            new MimeTypeExtSig(".erf", "application/x-endace-erf" },
            new MimeTypeExtSig(".es",  "application/ecmascript" },
            new MimeTypeExtSig(".es3", "application/vnd.eszigno3+xml" },
            new MimeTypeExtSig(".esa", "application/vnd.osgi.subsystem" },
            new MimeTypeExtSig(".esf", "application/vnd.epson.esf" },
            new MimeTypeExtSig(".et3", "application/vnd.eszigno3+xml" },
            new MimeTypeExtSig(".etl", "application/etl" },
            new MimeTypeExtSig(".etx", "text/x-setext" },

            new MimeTypeExtSig(".evy", "application/envoy" },
            new MimeTypeExtSig(".exe", "application/vnd.microsoft.portable-executable" },

            new MimeTypeExtSig(".exi", "application/exi" },
            new MimeTypeExtSig(".exp", "application/express" },
            new MimeTypeExtSig(".exr", "image/aces" },
            new MimeTypeExtSig(".ext", "application/vnd.novadigm.ext" },
            new MimeTypeExtSig(".ez",  "application/andrew-inset" },
            new MimeTypeExtSig(".ez2", "application/vnd.ezpix-album" },
            new MimeTypeExtSig(".ez3", "application/vnd.ezpix-package" },
            new MimeTypeExtSig(".f",   "text/x-fortran" },

            new MimeTypeExtSig(".f77", "text/x-fortran" },
            new MimeTypeExtSig(".f90", "text/x-fortran" },
            new MimeTypeExtSig(".fb",  "application/x-maker" },
            new MimeTypeExtSig(".fbdoc", "application/x-maker" },
            new MimeTypeExtSig(".fbs", "image/vnd.fastbidsheet" },
            new MimeTypeExtSig(".fcdt", "application/vnd.adobe.formscentral.fcdt" },
            new MimeTypeExtSig(".fcs", "application/vnd.isac.fcs" },
            new MimeTypeExtSig(".fdf", "application/vnd.fdf" },
            new MimeTypeExtSig(".fdt", "application/fdt+xml" },
            new MimeTypeExtSig(".feature",   "text/x-gherkin" },
            new MimeTypeExtSig(".fe_launch", "application/vnd.denovo.fcselayout-link" },
            new MimeTypeExtSig(".fg5", "application/vnd.fujitsu.oasysgp" },
            new MimeTypeExtSig(".fgd", "application/x-director" },
            new MimeTypeExtSig(".fh",  "image/x-freehand" },
            new MimeTypeExtSig(".fh4", "image/x-freehand" },
            new MimeTypeExtSig(".fh5", "image/x-freehand" },
            new MimeTypeExtSig(".fh7", "image/x-freehand" },
            new MimeTypeExtSig(".fhc", "image/x-freehand" },
            new MimeTypeExtSig(".fif", "application/fractals" },
            new MimeTypeExtSig(".fig", "application/x-xfig" },
            new MimeTypeExtSig(".filters",   "application/xml" },
            new MimeTypeExtSig(".fits", "image/fits" },
            new MimeTypeExtSig(".fla", "application/octet-stream" },
            new MimeTypeExtSig(".flac", "audio/flac" },
            new MimeTypeExtSig(".fli", "video/x-fli" },
            new MimeTypeExtSig(".flo", "application/vnd.micrografx.flo" },
            new MimeTypeExtSig(".flr", "x-world/x-vrml" },
            new MimeTypeExtSig(".flv", "video/x-flv" },
            new MimeTypeExtSig(".flw", "application/vnd.kde.kivio" },
            new MimeTypeExtSig(".flx", "text/vnd.fmi.flexstor" },
            new MimeTypeExtSig(".fly", "text/vnd.fly" },
            new MimeTypeExtSig(".fm",  "application/vnd.framemaker" },
            new MimeTypeExtSig(".fmf", "video/x-atomic3d-feature" },
            new MimeTypeExtSig(".fnc", "application/vnd.frogans.fnc" },
            new MimeTypeExtSig(".fo",  "application/vnd.software602.filler.form+xml" },
            new MimeTypeExtSig(".for", "text/x-fortran" },
            new MimeTypeExtSig(".fpx", "image/vnd.fpx" },
            new MimeTypeExtSig(".frame", "application/vnd.framemaker" },
            new MimeTypeExtSig(".frl", "application/freeloader" },
            new MimeTypeExtSig(".frm", "application/x-maker" },
            new MimeTypeExtSig(".fsc", "application/vnd.fsc.weblaunch" },
            new MimeTypeExtSig(".fsscript",  "application/fsharp-script" },
            new MimeTypeExtSig(".fst", "image/vnd.fst" },
            new MimeTypeExtSig(".fsx", "application/fsharp-script" },
            new MimeTypeExtSig(".ftc", "application/vnd.fluxtime.clip" },
            new MimeTypeExtSig(".fti", "application/vnd.anser-web-funds-transfer-initiation" },
            new MimeTypeExtSig(".funk", "audio/make" },
            new MimeTypeExtSig(".fvt", "video/vnd.fvt" },
            new MimeTypeExtSig(".fxp", "application/vnd.adobe.fxp" },
            new MimeTypeExtSig(".fxpl", "application/vnd.adobe.fxp" },
            new MimeTypeExtSig(".fzs", "application/vnd.fuzzysheet" },
            new MimeTypeExtSig(".g",   "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".g2w", "application/vnd.geoplan" },
            new MimeTypeExtSig(".g3",  "image/g3fax" },
            new MimeTypeExtSig(".g3w", "application/vnd.geospace" },
            new MimeTypeExtSig(".gac", "application/vnd.groove-account" },
            new MimeTypeExtSig(".gam", "application/x-tads" },
            new MimeTypeExtSig(".gbr", "application/vnd.gerber" },
            new MimeTypeExtSig(".gca", "application/x-gca-compressed" },
            new MimeTypeExtSig(".gcd", "text/x-pcs-gcd" },
            new MimeTypeExtSig(".gcf", "application/x-graphing-calculator" },
            new MimeTypeExtSig(".gdl", "model/vnd.gdl" },
            new MimeTypeExtSig(".gdoc", "application/vnd.google-apps.document" },
            new MimeTypeExtSig(".ged", "text/vnd.familysearch.gedcom" },
            new MimeTypeExtSig(".gemini", "text/gemini" },
            new MimeTypeExtSig(".generictest",   "application/xml" },
            new MimeTypeExtSig(".geo", "application/vnd.dynageo" },
            new MimeTypeExtSig(".geojson",   "application/geo+json" },
            new MimeTypeExtSig(".gex", "application/vnd.geometry-explorer" },
            new MimeTypeExtSig(".ggb", "application/vnd.geogebra.file" },
            new MimeTypeExtSig(".ggt", "application/vnd.geogebra.tool" },
            new MimeTypeExtSig(".ghf", "application/vnd.groove-help" },

            new MimeTypeExtSig(".gim", "application/vnd.groove-identity-message" },
            new MimeTypeExtSig(".gitattributes", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".gitignore", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".gl",  "video/gl" },
            new MimeTypeExtSig(".glb", "model/gltf-binary" },
            new MimeTypeExtSig(".gltf", "model/gltf+json" },
            new MimeTypeExtSig(".gmi", "text/gemini" },
            new MimeTypeExtSig(".gml", "application/gml+xml" },
            new MimeTypeExtSig(".gmx", "application/vnd.gmx" },
            new MimeTypeExtSig(".gnumeric",  "application/x-gnumeric" },
            new MimeTypeExtSig(".gph", "application/vnd.flographit" },
            new MimeTypeExtSig(".gpx", "application/gpx+xml" },
            new MimeTypeExtSig(".gqf", "application/vnd.grafeq" },
            new MimeTypeExtSig(".gqs", "application/vnd.grafeq" },
            new MimeTypeExtSig(".gram", "application/srgs" },
            new MimeTypeExtSig(".gramps", "application/x-gramps-xml" },
            new MimeTypeExtSig(".gre", "application/vnd.geometry-explorer" },
            new MimeTypeExtSig(".group", "text/x-ms-group" },
            new MimeTypeExtSig(".grv", "application/vnd.groove-injector" },
            new MimeTypeExtSig(".grxml", "application/srgs+xml" },
            new MimeTypeExtSig(".gsd", "audio/x-gsm" },
            new MimeTypeExtSig(".gsf", "application/x-font-ghostscript" },
            new MimeTypeExtSig(".gsheet", "application/vnd.google-apps.spreadsheet" },
            new MimeTypeExtSig(".gslides",   "application/vnd.google-apps.presentation" },
            new MimeTypeExtSig(".gsm", "audio/x-gsm" },
            new MimeTypeExtSig(".gsp", "application/x-gsp" },
            new MimeTypeExtSig(".gss", "application/x-gss" },
            new MimeTypeExtSig(".gtar", "application/x-gtar" },
            new MimeTypeExtSig(".gtm", "application/vnd.groove-tool-message" },
            new MimeTypeExtSig(".gtw", "model/vnd.gtw" },
            new MimeTypeExtSig(".gv",  "text/vnd.graphviz" },
            new MimeTypeExtSig(".gxf", "application/gxf" },
            new MimeTypeExtSig(".gxt", "application/vnd.geonext" },


        const byte[] TARGZ = { 0x1F, 0x8B };
            new MimeTypeExtSig(".h",   "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".h++", "text/x-c++hdr" },
            new MimeTypeExtSig(".h261", "video/h261" },
            new MimeTypeExtSig(".h263", "video/h263" },
            new MimeTypeExtSig(".h264", "video/h264" },
            new MimeTypeExtSig(".hal", "application/vnd.hal+xml" },
            new MimeTypeExtSig(".hbci", "application/vnd.hbci" },
            new MimeTypeExtSig(".hbs", "text/x-handlebars-template" },
            new MimeTypeExtSig(".hdd", "application/x-virtualbox-hdd" },
            new MimeTypeExtSig(".hdf", "application/x-hdf" },
            new MimeTypeExtSig(".hdml", "text/x-hdml" },
            new MimeTypeExtSig(".hdr", "image/vnd.radiance" },
            new MimeTypeExtSig(".heic", "image/heic" },
            new MimeTypeExtSig(".heics", "image/heic-sequence" },

            new MimeTypeExtSig(".hej2", "image/hej2k" },
            new MimeTypeExtSig(".held", "application/atsc-held+xml" },
            new MimeTypeExtSig(".help", "application/x-helpfile" },
            new MimeTypeExtSig(".hgl", "application/vndhp-hpgl" },
            new MimeTypeExtSig(".hh",  "text/x-c" },
            new MimeTypeExtSig(".hhc", "application/x-oleobject" },
            new MimeTypeExtSig(".hhk", "application/octet-stream" },
            new MimeTypeExtSig(".hhp", "application/octet-stream" },
            new MimeTypeExtSig(".hjson", "application/hjson" },
            new MimeTypeExtSig(".hlb", "text/x-script" },
            new MimeTypeExtSig(".hlp", "application/winhlp" },
            new MimeTypeExtSig(".hpg", "application/vndhp-hpgl" },
            new MimeTypeExtSig(".hpgl", "application/vnd.hp-hpgl" },
            new MimeTypeExtSig(".hpid", "application/vnd.hp-hpid" },
            new MimeTypeExtSig(".hpp", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".hps", "application/vnd.hp-hps" },
            new MimeTypeExtSig(".hqx", "application/mac-binhex40" },
            new MimeTypeExtSig(".hs",  "text/x-haskell" },
            new MimeTypeExtSig(".hsj2", "image/hsj2" },
            new MimeTypeExtSig(".hta", "application/hta" },
            new MimeTypeExtSig(".htc", "text/x-component" },
            new MimeTypeExtSig(".htke", "application/vnd.kenameaapp" },
            new MimeTypeExtSig(".htm", "text/html" },
            new MimeTypeExtSig(".html", "text/html" },
            new MimeTypeExtSig(".htmls", "text/html" },
            new MimeTypeExtSig(".htt", "text/webviewhtml" },
            new MimeTypeExtSig(".htx", "text/html" },
            new MimeTypeExtSig(".hvd", "application/vnd.yamaha.hv-dic" },
            new MimeTypeExtSig(".hvp", "application/vnd.yamaha.hv-voice" },
            new MimeTypeExtSig(".hvs", "application/vnd.yamaha.hv-script" },
            new MimeTypeExtSig(".hxa", "application/xml" },
            new MimeTypeExtSig(".hxc", "application/xml" },
            new MimeTypeExtSig(".hxd", "application/octet-stream" },
            new MimeTypeExtSig(".hxe", "application/xml" },
            new MimeTypeExtSig(".hxf", "application/xml" },
            new MimeTypeExtSig(".hxh", "application/octet-stream" },
            new MimeTypeExtSig(".hxi", "application/octet-stream" },
            new MimeTypeExtSig(".hxk", "application/xml" },
            new MimeTypeExtSig(".hxq", "application/octet-stream" },
            new MimeTypeExtSig(".hxr", "application/octet-stream" },
            new MimeTypeExtSig(".hxs", "application/octet-stream" },
            new MimeTypeExtSig(".hxt", "text/html" },
            new MimeTypeExtSig(".hxv", "application/xml" },
            new MimeTypeExtSig(".hxw", "application/octet-stream" },

            new MimeTypeExtSig(".i2g", "application/vnd.intergeo" },
            new MimeTypeExtSig(".ica", "application/x-ica" },
            new MimeTypeExtSig(".ical", "text/calendar" },
            new MimeTypeExtSig(".icalendar", "text/calendar" },
            new MimeTypeExtSig(".icc", "application/vnd.iccprofile" },
            new MimeTypeExtSig(".ice", "x-conference/x-cooltalk" },
            new MimeTypeExtSig(".icm", "application/vnd.iccprofile" },  
            new MimeTypeExtSig(".ics", "text/calendar" },
            new MimeTypeExtSig(".icz", "text/calendar" },
            
            new MimeTypeExtSig(".ief", "image/ief" },
            new MimeTypeExtSig(".iefs", "image/ief" },
            new MimeTypeExtSig(".ifb", "text/calendar" },
            new MimeTypeExtSig(".ifm", "application/vnd.shana.informed.formdata" },
            new MimeTypeExtSig(".iges", "model/iges" },
            new MimeTypeExtSig(".igl", "application/vnd.igloader" },
            new MimeTypeExtSig(".igm", "application/vnd.insors.igm" },
            new MimeTypeExtSig(".igs", "model/iges" },
            new MimeTypeExtSig(".igx", "application/vnd.micrografx.igx" },
            new MimeTypeExtSig(".iif", "application/vnd.shana.informed.interchange" },
            new MimeTypeExtSig(".iii", "application/x-iphone" },
            new MimeTypeExtSig(".ima", "application/x-ima" },
            new MimeTypeExtSig(".imap", "application/x-httpd-imap" },
            new MimeTypeExtSig(".img", "application/octet-stream" },
            new MimeTypeExtSig(".imp", "application/vnd.accpac.simply.imp" },
            new MimeTypeExtSig(".ims", "application/vnd.ms-ims" },
            new MimeTypeExtSig(".indd", "application/x-indesign" },
            new MimeTypeExtSig(".inf", "application/octet-stream" },

            new MimeTypeExtSig(".ink", "application/inkml+xml" },
            new MimeTypeExtSig(".inkml", "application/inkml+xml" },
            
            new MimeTypeExtSig(".ins", "application/x-internet-signup" },
            new MimeTypeExtSig(".install",   "application/x-install-instructions" },
            new MimeTypeExtSig(".iota", "application/vnd.astraea-software.iota" },
            new MimeTypeExtSig(".ip",  "application/x-ip2" },
            new MimeTypeExtSig(".ipa", "application/x-itunes-ipa" },
            new MimeTypeExtSig(".ipfix", "application/ipfix" },
            new MimeTypeExtSig(".ipg", "application/x-itunes-ipg" },
            new MimeTypeExtSig(".ipk", "application/vnd.shana.informed.package" },

            new MimeTypeExtSig(".ipsw", "application/x-itunes-ipsw" },
            new MimeTypeExtSig(".iqy", "text/x-ms-iqy" },
            new MimeTypeExtSig(".irm", "application/vnd.ibm.rights-management" },
            new MimeTypeExtSig(".irp", "application/vnd.irepository.package+xml" },
            new MimeTypeExtSig(".isma", "application/octet-stream" },
            new MimeTypeExtSig(".ismv", "application/octet-stream" }, 
            new MimeTypeExtSig(".isoimg", "application/x-iso9660-image" },
            new MimeTypeExtSig(".isp", "application/x-internet-signup" },
            new MimeTypeExtSig(".isu", "video/x-isvideo" },
            new MimeTypeExtSig(".it",  "audio/it" },
            new MimeTypeExtSig(".ite", "application/x-itunes-ite" },
            new MimeTypeExtSig(".itlp", "application/x-itunes-itlp" },
            new MimeTypeExtSig(".itms", "application/x-itunes-itms" },
            new MimeTypeExtSig(".itp", "application/vnd.shana.informed.formtemplate" },
            new MimeTypeExtSig(".itpc", "application/x-itunes-itpc" },
            new MimeTypeExtSig(".its", "application/its+xml" },
            new MimeTypeExtSig(".iv",  "application/x-inventor" },
            new MimeTypeExtSig(".IVF", "video/x-ivf" },
            new MimeTypeExtSig(".ivp", "application/vnd.immervision-ivp" },
            new MimeTypeExtSig(".ivr", "i-world/i-vrml" },
            new MimeTypeExtSig(".ivu", "application/vnd.immervision-ivu" },
            new MimeTypeExtSig(".ivy", "application/x-livescreen" },
            new MimeTypeExtSig(".jad", "text/vnd.sun.j2me.app-descriptor" },
            new MimeTypeExtSig(".jade", "text/jade" },
            new MimeTypeExtSig(".jam", "application/vnd.jam" },

            new MimeTypeExtSig(".jardiff",   "application/x-java-archive-diff" },
            new MimeTypeExtSig(".jav", "text/x-java-source" },
            new MimeTypeExtSig(".java", "application/octet-stream" },
            new MimeTypeExtSig(".jck", "application/liquidmotion" },
            new MimeTypeExtSig(".jcm", "application/x-java-commerce" },
            new MimeTypeExtSig(".jcz", "application/liquidmotion" },
            new MimeTypeExtSig(".jfif", "image/pjpeg" },
            new MimeTypeExtSig(".jfif-tbnl", "image/jpeg" },
            new MimeTypeExtSig(".jhc", "image/jphc" },
            new MimeTypeExtSig(".jisp", "application/vnd.jisp" },
            new MimeTypeExtSig(".jls", "image/jls" },
            new MimeTypeExtSig(".jlt", "application/vnd.hp-jlyt" },
            new MimeTypeExtSig(".jmz", "application/x-jmol" },
            new MimeTypeExtSig(".jng", "image/x-jng" },
            new MimeTypeExtSig(".jnlp", "application/x-java-jnlp-file" },
            new MimeTypeExtSig(".joda", "application/vnd.joost.joda-archive" },
            new MimeTypeExtSig(".jp2", "image/jp2" },
            new MimeTypeExtSig(".jpb", "application/octet-stream" },
            new MimeTypeExtSig(".jpe", "image/jpeg" },
            new MimeTypeExtSig(".jpeg", "image/jpeg" },
            new MimeTypeExtSig(".jpf", "image/jpx" },
            new MimeTypeExtSig(".jpgm", "video/jpm" },
            new MimeTypeExtSig(".jpgv", "video/jpeg" },
            new MimeTypeExtSig(".jph", "image/jph" },
            new MimeTypeExtSig(".jpm", "video/jpm" },
            new MimeTypeExtSig(".jps", "image/x-jps" },
            new MimeTypeExtSig(".jpx", "image/jpx" },
            new MimeTypeExtSig(".js",  "application/javascript" },
            new MimeTypeExtSig(".jsm", "text/javascript" },
            new MimeTypeExtSig(".json", "application/json" },
            new MimeTypeExtSig(".json5", "application/json5" },
            new MimeTypeExtSig(".jsonld", "application/ld+json" },
            new MimeTypeExtSig(".jsonml", "application/jsonml+json" },
            new MimeTypeExtSig(".jsx", "text/jscript" },
            new MimeTypeExtSig(".jsxbin", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".jt",  "model/jt" },
            new MimeTypeExtSig(".jut", "image/jutvision" },
            new MimeTypeExtSig(".jxl", "image/jxl" },
            new MimeTypeExtSig(".jxr", "image/jxr" },
            new MimeTypeExtSig(".jxra", "image/jxra" },
            new MimeTypeExtSig(".jxrs", "image/jxrs" },
            new MimeTypeExtSig(".jxs", "image/jxs" },
            new MimeTypeExtSig(".jxsc", "image/jxsc" },
            new MimeTypeExtSig(".jxsi", "image/jxsi" },
            new MimeTypeExtSig(".jxss", "image/jxss" },
            new MimeTypeExtSig(".k25", "image/x-kodak-k25" },
            new MimeTypeExtSig(".kar", "audio/midi" },
            new MimeTypeExtSig(".karbon", "application/vnd.kde.karbon" },
            new MimeTypeExtSig(".kdbx", "application/x-keepass2" },
            new MimeTypeExtSig(".kdc", "image/x-kodak-kdc" },
            new MimeTypeExtSig(".key", "application/vnd.apple.keynote" },
            new MimeTypeExtSig(".kfo", "application/vnd.kde.kformula" },
            new MimeTypeExtSig(".kia", "application/vnd.kidspiration" },
            new MimeTypeExtSig(".kil", "application/x-killustrator" },
            new MimeTypeExtSig(".kml", "application/vnd.google-earth.kml+xml" },
            new MimeTypeExtSig(".kmz", "application/vnd.google-earth.kmz" },
            new MimeTypeExtSig(".kne", "application/vnd.kinar" },
            new MimeTypeExtSig(".knp", "application/vnd.kinar" },
            new MimeTypeExtSig(".kon", "application/vnd.kde.kontour" },
            new MimeTypeExtSig(".kpr", "application/vnd.kde.kpresenter" },
            new MimeTypeExtSig(".kpt", "application/vnd.kde.kpresenter" },
            new MimeTypeExtSig(".kpxx", "application/vnd.ds-keypoint" },
            new MimeTypeExtSig(".ksh", "text/x-scriptksh" },
            new MimeTypeExtSig(".ksp", "application/vnd.kde.kspread" },
            new MimeTypeExtSig(".kth", "application/x-iwork-keynote-sffkth" },
            new MimeTypeExtSig(".ktr", "application/vnd.kahootz" },
            new MimeTypeExtSig(".ktx", "image/ktx" },
            new MimeTypeExtSig(".ktx2", "image/ktx2" },
            new MimeTypeExtSig(".ktz", "application/vnd.kahootz" },
            new MimeTypeExtSig(".kwd", "application/vnd.kde.kword" },
            new MimeTypeExtSig(".kwt", "application/vnd.kde.kword" },
            new MimeTypeExtSig(".la",  "audio/nspaudio" },
            new MimeTypeExtSig(".lam", "audio/x-liveaudio" },
            new MimeTypeExtSig(".lasxml", "application/vnd.las.las+xml" },
            new MimeTypeExtSig(".latex", "application/x-latex" },
            new MimeTypeExtSig(".lbd", "application/vnd.llamagraphics.life-balance.desktop" },
            new MimeTypeExtSig(".lbe", "application/vnd.llamagraphics.life-balance.exchange+xml" },
            new MimeTypeExtSig(".les", "application/vnd.hhe.lesson-player" },
            new MimeTypeExtSig(".less", "text/less" },
            new MimeTypeExtSig(".lgr", "application/lgr+xml" },
            new MimeTypeExtSig(".lha", "application/x-lzh-compressed" },
            new MimeTypeExtSig(".lhs", "text/x-literate-haskell" },
            new MimeTypeExtSig(".lhx", "application/octet-stream" },
            new MimeTypeExtSig(".library-ms",    "application/windows-library+xml" },
            new MimeTypeExtSig(".link66", "application/vnd.route66.link66+xml" },
            new MimeTypeExtSig(".list", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".list3820",  "application/vnd.ibm.modcap" },
            new MimeTypeExtSig(".listafp",   "application/vnd.ibm.modcap" },
            new MimeTypeExtSig(".lit", "application/x-ms-reader" },
            new MimeTypeExtSig(".litcoffee", "text/coffeescript" },
            new MimeTypeExtSig(".lma", "audio/nspaudio" },
            new MimeTypeExtSig(".lnk", "application/x-ms-shortcut" },
            new MimeTypeExtSig(".loadtest",  "application/xml" },
            new MimeTypeExtSig(".log", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".lostxml",   "application/lost+xml" },
            new MimeTypeExtSig(".lpk", "application/octet-stream" },
            new MimeTypeExtSig(".lrf", "application/octet-stream" },
            new MimeTypeExtSig(".lrm", "application/vnd.ms-lrm" },
            new MimeTypeExtSig(".lsf", "video/x-la-asf" },
            new MimeTypeExtSig(".lsp", "text/x-scriptlisp" },
            new MimeTypeExtSig(".lst", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".lsx", "video/x-la-asf" },
            new MimeTypeExtSig(".ltf", "application/vnd.frogans.ltf" },
            new MimeTypeExtSig(".ltx", "text/x-tex" },
            new MimeTypeExtSig(".lua", "text/x-lua" },
            new MimeTypeExtSig(".luac", "application/x-lua-bytecode" },
            new MimeTypeExtSig(".lvp", "audio/vnd.lucent.voice" },
            new MimeTypeExtSig(".lwp", "application/vnd.lotus-wordpro" },
            new MimeTypeExtSig(".lzh", "application/octet-stream" },
            new MimeTypeExtSig(".lzx", "application/x-lzx" },
            new MimeTypeExtSig(".m",   "text/x-m" },
            new MimeTypeExtSig(".m13", "application/x-msmediaview" },
            new MimeTypeExtSig(".m14", "application/x-msmediaview" },
            new MimeTypeExtSig(".m1v", "video/mpeg" },
            new MimeTypeExtSig(".m21", "application/mp21" },
            new MimeTypeExtSig(".m2a", "audio/mpeg" },
            new MimeTypeExtSig(".m2t", "video/vnd.dlna.mpeg-tts" },
            new MimeTypeExtSig(".m2ts", "video/vnd.dlna.mpeg-tts" },
            new MimeTypeExtSig(".m2v", "video/mpeg" },
            new MimeTypeExtSig(".m3a", "audio/mpeg" },
            new MimeTypeExtSig(".m3u", "audio/x-mpegurl" },
            new MimeTypeExtSig(".m3u8", "audio/x-mpegurl" },
            new MimeTypeExtSig(".m4a", "audio/m4a" },
            new MimeTypeExtSig(".m4b", "audio/m4b" },
            new MimeTypeExtSig(".m4p", "audio/m4p" },
            new MimeTypeExtSig(".m4r", "audio/x-m4r" },
            new MimeTypeExtSig(".m4s", "video/iso.segment" },
            new MimeTypeExtSig(".m4u", "video/vnd.mpegurl" },
            new MimeTypeExtSig(".m4v", "video/x-m4v" },
            new MimeTypeExtSig(".ma",  "application/mathematica" },
            new MimeTypeExtSig(".mac", "image/x-macpaint" },
            new MimeTypeExtSig(".mads", "application/mads+xml" },
            new MimeTypeExtSig(".maei", "application/mmt-aei+xml" },
            new MimeTypeExtSig(".mag", "application/vnd.ecowin.chart" },
            new MimeTypeExtSig(".mak", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".maker", "application/vnd.framemaker" },
            new MimeTypeExtSig(".man", "application/x-troff-man" },
            new MimeTypeExtSig(".manifest",  "application/x-ms-manifest" },
            new MimeTypeExtSig(".map", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".mar", "application/octet-stream" },
            new MimeTypeExtSig(".markdown",  "text/markdown" },
            new MimeTypeExtSig(".master", "application/xml" },
            new MimeTypeExtSig(".mathml", "application/mathml+xml" },
            new MimeTypeExtSig(".mb",  "application/mathematica" },
            new MimeTypeExtSig(".mbd", "application/mbedlet" },
            new MimeTypeExtSig(".mbk", "application/vnd.mobius.mbk" },
            new MimeTypeExtSig(".mbox", "application/mbox" },
            new MimeTypeExtSig(".mc$", "application/x-magic-cap-package-10" },
            new MimeTypeExtSig(".mc1", "application/vnd.medcalcdata" },
            new MimeTypeExtSig(".mcd", "application/vnd.mcd" },
            new MimeTypeExtSig(".mcf", "text/mcf" },
            new MimeTypeExtSig(".mcp", "application/netmc" },
            new MimeTypeExtSig(".mcurl", "text/vnd.curl.mcurl" },
            new MimeTypeExtSig(".md",  "text/markdown" },
            new MimeTypeExtSig(".mda", "application/msaccess" },
            new MimeTypeExtSig(".mdb", "application/x-msaccess" },
            new MimeTypeExtSig(".mde", "application/msaccess" },
            new MimeTypeExtSig(".mdi", "image/vnd.ms-modi" },
            new MimeTypeExtSig(".mdp", "application/octet-stream" },
            new MimeTypeExtSig(".mdx", "text/mdx" },
            new MimeTypeExtSig(".me",  "application/x-troff-me" },
            new MimeTypeExtSig(".mesh", "model/mesh" },
            new MimeTypeExtSig(".meta4", "application/metalink4+xml" },
            new MimeTypeExtSig(".metalink",  "application/metalink+xml" },
            new MimeTypeExtSig(".mets", "application/mets+xml" },
            new MimeTypeExtSig(".mfm", "application/vnd.mfmp" },
            new MimeTypeExtSig(".mfp", "application/x-shockwave-flash" },
            new MimeTypeExtSig(".mft", "application/rpki-manifest" },
            new MimeTypeExtSig(".mgp", "application/vnd.osgeo.mapguide.package" },
            new MimeTypeExtSig(".mgz", "application/vnd.proteus.magazine" },
            new MimeTypeExtSig(".mht",  "message/rfc822" },
            new MimeTypeExtSig(".mhtml", "message/rfc822" },

            new MimeTypeExtSig(".mie", "application/x-mie" },
            new MimeTypeExtSig(".mif", "application/vnd.mif" },
            new MimeTypeExtSig(".mime", "message/rfc822" },
            new MimeTypeExtSig(".mix", "application/octet-stream" },
            new MimeTypeExtSig(".mj2", "video/mj2" },
            new MimeTypeExtSig(".mjf", "audio/x-vndaudioexplosionmjuicemediafile" },
            new MimeTypeExtSig(".mjp2", "video/mj2" },
            new MimeTypeExtSig(".mjpg", "video/x-motion-jpeg" },
            new MimeTypeExtSig(".mjs", "text/javascript" },
            new MimeTypeExtSig(".mk",  "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".mk3d", "video/x-matroska-3d" },
            new MimeTypeExtSig(".mka", "audio/x-matroska" },
            new MimeTypeExtSig(".mkd", "text/x-markdown" },
            new MimeTypeExtSig(".mks", "video/x-matroska" },
            new MimeTypeExtSig(".mkv", "video/x-matroska" },
            new MimeTypeExtSig(".mlp", "application/vnd.dolby.mlp" },
            new MimeTypeExtSig(".mm",  "application/x-freemind" },
            new MimeTypeExtSig(".mmd", "application/vnd.chipnuts.karaoke-mmd" },
            new MimeTypeExtSig(".mme", "application/base64" },
            new MimeTypeExtSig(".mmf", "application/x-smaf" },
            new MimeTypeExtSig(".mml", "text/mathml" },
            new MimeTypeExtSig(".mmr", "image/vnd.fujixerox.edmics-mmr" },
            new MimeTypeExtSig(".mng", "video/x-mng" },

            new MimeTypeExtSig(".mny", "application/x-msmoney" },
            new MimeTypeExtSig(".mobi", "application/x-mobipocket-ebook" },
            new MimeTypeExtSig(".moc", "text/x-moc" },
            new MimeTypeExtSig(".mod", "video/mpeg" },
            new MimeTypeExtSig(".mods", "application/mods+xml" },
            new MimeTypeExtSig(".moov", "video/quicktime" },
            new MimeTypeExtSig(".mov", "video/quicktime" },
            new MimeTypeExtSig(".movie", "video/x-sgi-movie" },
            new MimeTypeExtSig(".mp2", "video/mpeg" },
            new MimeTypeExtSig(".mp21", "application/mp21" },
            new MimeTypeExtSig(".mp2a", "audio/mpeg" },
            new MimeTypeExtSig(".mp2v", "video/mpeg" },



            new MimeTypeExtSig(".mpa", "video/mpeg" },
            new MimeTypeExtSig(".mpc", "application/vnd.mophun.certificate" },
            new MimeTypeExtSig(".mpd", "application/dash+xml" },
            new MimeTypeExtSig(".mpe", "video/mpeg" },
            new MimeTypeExtSig(".mpeg", "video/mpeg" },
            new MimeTypeExtSig(".mpega", "audio/mpeg" },
            new MimeTypeExtSig(".mpf", "application/vnd.ms-mediapackage" },
            new MimeTypeExtSig(".mpg", "video/mpeg" },

            new MimeTypeExtSig(".mpga", "audio/mpeg" },
            new MimeTypeExtSig(".mpkg", "application/vnd.apple.installer+xml" },
            new MimeTypeExtSig(".mpm", "application/vnd.blueice.multipass" },
            new MimeTypeExtSig(".mpn", "application/vnd.mophun.application" },
            new MimeTypeExtSig(".mpp", "application/vnd.ms-project" },
            new MimeTypeExtSig(".mpt", "application/vnd.ms-project" },
            new MimeTypeExtSig(".mpv", "application/x-project" },
            new MimeTypeExtSig(".mpv2", "video/mpeg" },
            new MimeTypeExtSig(".mpx", "application/x-project" },
            new MimeTypeExtSig(".mpy", "application/vnd.ibm.minipay" },
            new MimeTypeExtSig(".mqv", "video/quicktime" },
            new MimeTypeExtSig(".mqy", "application/vnd.mobius.mqy" },
            new MimeTypeExtSig(".mrc", "application/marc" },
            new MimeTypeExtSig(".mrcx", "application/marcxml+xml" },
            new MimeTypeExtSig(".mrw", "image/x-minolta-mrw" },
            new MimeTypeExtSig(".ms",  "application/x-troff-ms" },
            new MimeTypeExtSig(".mscml", "application/mediaservercontrol+xml" },
            new MimeTypeExtSig(".mseed", "application/vnd.fdsn.mseed" },
            new MimeTypeExtSig(".mseq", "application/vnd.mseq" },
            new MimeTypeExtSig(".msf", "application/vnd.epson.msf" },
            new MimeTypeExtSig(".msg", "application/vnd.ms-outlook" },
            new MimeTypeExtSig(".msh", "model/mesh" },
            new MimeTypeExtSig(".msi", "application/octet-stream" },
            new MimeTypeExtSig(".msix", "application/msix" },
            new MimeTypeExtSig(".msixbundle",    "application/msixbundle" },
            new MimeTypeExtSig(".msl", "application/vnd.mobius.msl" },
            new MimeTypeExtSig(".msm", "application/octet-stream" },
            new MimeTypeExtSig(".mso", "application/octet-stream" },
            new MimeTypeExtSig(".msp", "application/octet-stream" },
            new MimeTypeExtSig(".msty", "application/vnd.muvee.style" },
            new MimeTypeExtSig(".mtl", "model/mtl" },
            new MimeTypeExtSig(".mts", "video/vnd.dlna.mpeg-tts" },
            new MimeTypeExtSig(".mtx", "application/xml" },
            new MimeTypeExtSig(".mus", "application/vnd.musician" },
            new MimeTypeExtSig(".musd", "application/mmt-usd+xml" },
            new MimeTypeExtSig(".musicxml",  "application/vnd.recordare.musicxml+xml" },
            new MimeTypeExtSig(".mustache",  "text/html" },
            new MimeTypeExtSig(".mv",  "video/x-sgi-movie" },
            new MimeTypeExtSig(".mvb", "application/x-msmediaview" },
            new MimeTypeExtSig(".mvc", "application/x-miva-compiled" },
            new MimeTypeExtSig(".mvt", "application/vnd.mapbox-vector-tile" },
            new MimeTypeExtSig(".mwf", "application/vnd.mfer" },
            new MimeTypeExtSig(".mxf", "application/mxf" },
            new MimeTypeExtSig(".mxl", "application/vnd.recordare.musicxml" },
            new MimeTypeExtSig(".mxmf", "audio/mobile-xmf" },
            new MimeTypeExtSig(".mxml", "application/xv+xml" },
            new MimeTypeExtSig(".mxp", "application/x-mmxp" },
            new MimeTypeExtSig(".mxs", "application/vnd.triscape.mxs" },
            new MimeTypeExtSig(".mxu", "video/vnd.mpegurl" },
            new MimeTypeExtSig(".my",  "audio/make" },
            new MimeTypeExtSig(".mzz", "application/x-vndaudioexplosionmzz" },
            new MimeTypeExtSig(".n-gage", "application/vnd.nokia.n-gage.symbian.install" },
            new MimeTypeExtSig(".n3",  "text/n3" },
            new MimeTypeExtSig(".nap", "image/naplps" },
            new MimeTypeExtSig(".naplps", "image/naplps" },
            new MimeTypeExtSig(".nb",  "application/mathematica" },
            new MimeTypeExtSig(".nbp", "application/vnd.wolfram.player" },
            new MimeTypeExtSig(".nc",  "application/x-netcdf" },
            new MimeTypeExtSig(".ncm", "application/vndnokiaconfiguration-message" },
            new MimeTypeExtSig(".ncx", "application/x-dtbncx+xml" },
            new MimeTypeExtSig(".nef", "image/x-nikon-nef" },
            new MimeTypeExtSig(".nfo", "text/x-nfo" },
            new MimeTypeExtSig(".ngdat", "application/vnd.nokia.n-gage.data" },
            new MimeTypeExtSig(".nif", "image/x-niff" },
            new MimeTypeExtSig(".niff", "image/x-niff" },
            new MimeTypeExtSig(".nitf", "application/vnd.nitf" },
            new MimeTypeExtSig(".nix", "application/x-mix-transfer" },
            new MimeTypeExtSig(".nlu", "application/vnd.neurolanguage.nlu" },
            new MimeTypeExtSig(".nmbtemplate",   "application/x-iwork-numbers-sfftemplate" },
            new MimeTypeExtSig(".nml", "application/vnd.enliven" },
            new MimeTypeExtSig(".nnd", "application/vnd.noblenet-directory" },
            new MimeTypeExtSig(".nns", "application/vnd.noblenet-sealer" },
            new MimeTypeExtSig(".nnw", "application/vnd.noblenet-web" },
            new MimeTypeExtSig(".npx", "image/vnd.net-fpx" },
            new MimeTypeExtSig(".nq",  "application/n-quads" },
            new MimeTypeExtSig(".nrw", "image/x-nikon-nrw" },
            new MimeTypeExtSig(".nsc", "video/x-ms-asf" },
            new MimeTypeExtSig(".nsf", "application/vnd.lotus-notes" },
            new MimeTypeExtSig(".nt",  "application/n-triples" },
            new MimeTypeExtSig(".ntf", "application/vnd.nitf" },
            new MimeTypeExtSig(".numbers",   "application/vnd.apple.numbers" },
            new MimeTypeExtSig(".nvd", "application/x-navidoc" },
            new MimeTypeExtSig(".nwc", "application/x-nwc" },
            new MimeTypeExtSig(".nws", "message/rfc822" },
            new MimeTypeExtSig(".nzb", "application/x-nzb" },
            new MimeTypeExtSig(".o",   "application/x-object" },
            new MimeTypeExtSig(".oa2", "application/vnd.fujitsu.oasys2" },
            new MimeTypeExtSig(".oa3", "application/vnd.fujitsu.oasys3" },
            new MimeTypeExtSig(".oas", "application/vnd.fujitsu.oasys" },
            new MimeTypeExtSig(".obd", "application/x-msbinder" },
            new MimeTypeExtSig(".obgx", "application/vnd.openblox.game+xml" },
            new MimeTypeExtSig(".obj", "application/x-tgif" },
            new MimeTypeExtSig(".ocx", "application/octet-stream" },
            new MimeTypeExtSig(".oda", "application/oda" },
            new MimeTypeExtSig(".odb", "application/vnd.oasis.opendocument.database" },
            new MimeTypeExtSig(".odc", "application/vnd.oasis.opendocument.chart" },
            new MimeTypeExtSig(".odf", "application/vnd.oasis.opendocument.formula" },
            new MimeTypeExtSig(".odft", "application/vnd.oasis.opendocument.formula-template" },
            new MimeTypeExtSig(".odg", "application/vnd.oasis.opendocument.graphics" },
            new MimeTypeExtSig(".odh", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".odi", "application/vnd.oasis.opendocument.image" },
            new MimeTypeExtSig(".odl", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".odm", "application/vnd.oasis.opendocument.text-master" },
            new MimeTypeExtSig(".odp", "application/vnd.oasis.opendocument.presentation" },
            new MimeTypeExtSig(".ods", "application/vnd.oasis.opendocument.spreadsheet" },
            new MimeTypeExtSig(".odt", "application/vnd.oasis.opendocument.text" },
            new MimeTypeExtSig(".oex", "application/x-opera-extension" },

            new MimeTypeExtSig(".ogex", "model/vnd.opengex" },

            new MimeTypeExtSig(".omc", "application/x-omc" },
            new MimeTypeExtSig(".omcd", "application/x-omcdatamaker" },
            new MimeTypeExtSig(".omcr", "application/x-omcregerator" },
            new MimeTypeExtSig(".omdoc", "application/omdoc+xml" },
            new MimeTypeExtSig(".one", "application/onenote" },
            new MimeTypeExtSig(".onea", "application/onenote" },
            new MimeTypeExtSig(".onepkg", "application/onenote" },
            new MimeTypeExtSig(".onetmp", "application/onenote" },
            new MimeTypeExtSig(".onetoc", "application/onenote" },
            new MimeTypeExtSig(".onetoc2",   "application/onenote" },
            new MimeTypeExtSig(".opf", "application/oebps-package+xml" },
            new MimeTypeExtSig(".opml", "text/x-opml" },
            new MimeTypeExtSig(".oprc", "application/vnd.palm" },
            new MimeTypeExtSig(".opus", "audio/ogg" },
            new MimeTypeExtSig(".orderedtest",   "application/xml" },
            new MimeTypeExtSig(".orf", "image/x-olympus-orf" },
            new MimeTypeExtSig(".org", "application/vnd.lotus-organizer" },
            new MimeTypeExtSig(".osdx", "application/opensearchdescription+xml" },
            new MimeTypeExtSig(".osf", "application/vnd.yamaha.openscoreformat" },
            new MimeTypeExtSig(".osfpvg", "application/vnd.yamaha.openscoreformat.osfpvg+xml" },
            new MimeTypeExtSig(".osm", "application/vnd.openstreetmap.data+xml" },
            new MimeTypeExtSig(".otc", "application/vnd.oasis.opendocument.chart-template" },
            new MimeTypeExtSig(".otf", "application/font-sfnt" },
            new MimeTypeExtSig(".otg", "application/vnd.oasis.opendocument.graphics-template" },
            new MimeTypeExtSig(".oth", "application/vnd.oasis.opendocument.text-web" },
            new MimeTypeExtSig(".oti", "application/vnd.oasis.opendocument.image-template" },
            new MimeTypeExtSig(".otm", "application/vnd.oasis.opendocument.text-master" },
            new MimeTypeExtSig(".otp", "application/vnd.oasis.opendocument.presentation-template" },
            new MimeTypeExtSig(".ots", "application/vnd.oasis.opendocument.spreadsheet-template" },
            new MimeTypeExtSig(".ott", "application/vnd.oasis.opendocument.text-template" },
            new MimeTypeExtSig(".ova", "application/x-virtualbox-ova" },
            new MimeTypeExtSig(".ovf", "application/x-virtualbox-ovf" },
            new MimeTypeExtSig(".owl", "application/rdf+xml" },
            new MimeTypeExtSig(".oxps", "application/oxps" },
            new MimeTypeExtSig(".oxt", "application/vnd.openofficeorg.extension" },
            new MimeTypeExtSig(".oza", "application/x-oz-application" },
            new MimeTypeExtSig(".p",   "text/x-pascal" },
            new MimeTypeExtSig(".p10", "application/pkcs10" },
            new MimeTypeExtSig(".p12", "application/x-pkcs12" },
            new MimeTypeExtSig(".p7a", "application/x-pkcs7-signature" },
            new MimeTypeExtSig(".p7b", "application/x-pkcs7-certificates" },
            new MimeTypeExtSig(".p7c", "application/pkcs7-mime" },
            new MimeTypeExtSig(".p7m", "application/pkcs7-mime" },
            new MimeTypeExtSig(".p7r", "application/x-pkcs7-certreqresp" },
            new MimeTypeExtSig(".p7s", "application/pkcs7-signature" },
            new MimeTypeExtSig(".p8",  "application/pkcs8" },
            new MimeTypeExtSig(".pac", "application/x-ns-proxy-autoconfig" },
            new MimeTypeExtSig(".pages", "application/vnd.apple.pages" },
            new MimeTypeExtSig(".parquet",   "application/vnd.apache.parquet" },
            new MimeTypeExtSig(".part", "application/pro_eng" },
            new MimeTypeExtSig(".pas", "text/x-pascal" },
            new MimeTypeExtSig(".pat", "image/x-coreldrawpattern" },
            new MimeTypeExtSig(".paw", "application/vnd.pawaafile" },
            new MimeTypeExtSig(".pbd", "application/vnd.powerbuilder6" },
            new MimeTypeExtSig(".pbm", "image/x-portable-bitmap" },
            new MimeTypeExtSig(".pcap", "application/vnd.tcpdump.pcap" },
            new MimeTypeExtSig(".pcast", "application/x-podcast" },
            new MimeTypeExtSig(".pcf", "application/x-font-pcf" },
            new MimeTypeExtSig(".pcf.Z", "application/x-font" },
            new MimeTypeExtSig(".pcl", "application/vnd.hp-pcl" },
            new MimeTypeExtSig(".pclxl", "application/vnd.hp-pclxl" },
            new MimeTypeExtSig(".pct", "image/pict" },
            new MimeTypeExtSig(".pcurl", "application/vnd.curl.pcurl" },
            new MimeTypeExtSig(".pcx", "application/octet-stream" },
            new MimeTypeExtSig(".pcz", "application/octet-stream" },
            new MimeTypeExtSig(".pdb", "application/vnd.palm" },
            new MimeTypeExtSig(".pde", "text/x-processing" },
            new MimeTypeExtSig(".pef", "image/x-pentax-pef" },
            new MimeTypeExtSig(".pem", "application/x-x509-ca-cert" },
            new MimeTypeExtSig(".pfa", "application/x-font-type1" },
            new MimeTypeExtSig(".pfb", "application/octet-stream" },
            new MimeTypeExtSig(".pfm", "application/octet-stream" },
            new MimeTypeExtSig(".pfr", "application/font-tdpfr" },
            new MimeTypeExtSig(".pfunk", "audio/make" },
            new MimeTypeExtSig(".pfx", "application/x-pkcs12" },
            new MimeTypeExtSig(".pgm", "image/x-portable-graymap" },
            new MimeTypeExtSig(".pgn", "application/x-chess-pgn" },
            new MimeTypeExtSig(".pgp", "application/pgp-encrypted" },
            new MimeTypeExtSig(".php", "application/x-httpd-php" },
            new MimeTypeExtSig(".phps", "text/text" },
            new MimeTypeExtSig(".pic", "image/pict" },
            new MimeTypeExtSig(".pict", "image/pict" },
            new MimeTypeExtSig(".pkg", "application/octet-stream" },
            new MimeTypeExtSig(".pkgdef", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".pkgundef",  "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".pki", "application/pkixcmp" },
            new MimeTypeExtSig(".pkipath",   "application/pkix-pkipath" },
            new MimeTypeExtSig(".pko", "application/vnd.ms-pki.pko" },
            new MimeTypeExtSig(".pkpass", "application/vnd.apple.pkpass" },
            new MimeTypeExtSig(".pl",  "application/x-perl" },
            new MimeTypeExtSig(".plb", "application/vnd.3gpp.pic-bw-large" },
            new MimeTypeExtSig(".plc", "application/vnd.mobius.plc" },
            new MimeTypeExtSig(".plf", "application/vnd.pocketlearn" },
            new MimeTypeExtSig(".pls", "audio/scpls" },
            new MimeTypeExtSig(".plx", "application/x-pixclscript" },
            new MimeTypeExtSig(".pm",  "application/x-perl" },
            new MimeTypeExtSig(".pm4", "application/x-pagemaker" },
            new MimeTypeExtSig(".pm5", "application/x-pagemaker" },
            new MimeTypeExtSig(".pma", "application/x-perfmon" },
            new MimeTypeExtSig(".pmc", "application/x-perfmon" },
            new MimeTypeExtSig(".pml", "application/x-perfmon" },
            new MimeTypeExtSig(".pmr", "application/x-perfmon" },
            new MimeTypeExtSig(".pmw", "application/x-perfmon" },
            new MimeTypeExtSig(".png", "image/png" },
            new MimeTypeExtSig(".pnm", "image/x-portable-anymap" },
            new MimeTypeExtSig(".pnt", "image/x-macpaint" },
            new MimeTypeExtSig(".pntg", "image/x-macpaint" },
            new MimeTypeExtSig(".pnz", "image/png" },

            
            new MimeTypeExtSig(".portpkg",   "application/vnd.macports.portpkg" },
            new MimeTypeExtSig(".pot", "application/vnd.ms-powerpoint" },
            new MimeTypeExtSig(".potm", "application/vnd.ms-powerpoint.template.macroEnabled.12" },
            new MimeTypeExtSig(".potx", "application/vnd.openxmlformats-officedocument.presentationml.template" },
            new MimeTypeExtSig(".pov", "model/x-pov" },
            new MimeTypeExtSig(".ppa", "application/vnd.ms-powerpoint" },
            new MimeTypeExtSig(".ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12" },
            new MimeTypeExtSig(".ppd", "application/vnd.cups-ppd" },
            new MimeTypeExtSig(".ppm", "image/x-portable-pixmap" },
            new MimeTypeExtSig(".pps", "application/vnd.ms-powerpoint" },
            new MimeTypeExtSig(".ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12" },
            new MimeTypeExtSig(".ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow" },


            new MimeTypeExtSig(".ppz", "application/mspowerpoint" },
            new MimeTypeExtSig(".pqa", "application/vnd.palm" },
            new MimeTypeExtSig(".prc", "application/x-mobipocket-ebook" },
            new MimeTypeExtSig(".pre", "application/vnd.lotus-freelance" },
            new MimeTypeExtSig(".prf", "application/pics-rules" },
            new MimeTypeExtSig(".prm", "application/octet-stream" },
            new MimeTypeExtSig(".provx", "application/provenance+xml" },
            new MimeTypeExtSig(".prt", "application/pro_eng" },
            new MimeTypeExtSig(".prx", "application/octet-stream" },
            new MimeTypeExtSig(".ps",  "application/postscript" },
            new MimeTypeExtSig(".psb", "application/vnd.3gpp.pic-bw-small" },
            new MimeTypeExtSig(".psc1", "application/PowerShell" },
            new MimeTypeExtSig(".psd", "application/octet-stream" },
            new MimeTypeExtSig(".psess", "application/xml" },
            new MimeTypeExtSig(".psf", "application/x-font-linux-psf" },
            new MimeTypeExtSig(".pskcxml",   "application/pskc+xml" },
            new MimeTypeExtSig(".psm", "application/octet-stream" },
            new MimeTypeExtSig(".psp", "application/octet-stream" },
            new MimeTypeExtSig(".pst", "application/vnd.ms-outlook" },
            new MimeTypeExtSig(".pti", "image/prs.pti" },
            new MimeTypeExtSig(".ptid", "application/vnd.pvi.ptid1" },
            new MimeTypeExtSig(".pub", "application/x-mspublisher" },
            new MimeTypeExtSig(".pvb", "application/vnd.3gpp.pic-bw-var" },
            new MimeTypeExtSig(".pvu", "paleovu/x-pv" },
            new MimeTypeExtSig(".pwn", "application/vnd.3m.post-it-notes" },
            new MimeTypeExtSig(".pwz", "application/vnd.ms-powerpoint" },


            new MimeTypeExtSig(".pya", "audio/vnd.ms-playready.media.pya" },
            new MimeTypeExtSig(".pyc", "applicaiton/x-bytecodepython" },
            new MimeTypeExtSig(".pyo", "model/vnd.pytha.pyox" },
            new MimeTypeExtSig(".pyox", "model/vnd.pytha.pyox" },
            new MimeTypeExtSig(".pyv", "video/vnd.ms-playready.media.pyv" },
            new MimeTypeExtSig(".qam", "application/vnd.epson.quickanime" },
            new MimeTypeExtSig(".qbo", "application/vnd.intu.qbo" },
            new MimeTypeExtSig(".qcp", "audio/vndqcelp" },
            new MimeTypeExtSig(".qd3", "x-world/x-3dmf" },
            new MimeTypeExtSig(".qd3d", "x-world/x-3dmf" },
            new MimeTypeExtSig(".qfx", "application/vnd.intu.qfx" },
            new MimeTypeExtSig(".qht", "text/x-html-insertion" },
            new MimeTypeExtSig(".qhtm", "text/x-html-insertion" },
            new MimeTypeExtSig(".qif", "image/x-quicktime" },
            new MimeTypeExtSig(".qps", "application/vnd.publishare-delta-tree" },
            new MimeTypeExtSig(".qt",  "video/quicktime" },
            new MimeTypeExtSig(".qtc", "video/x-qtc" },
            new MimeTypeExtSig(".qti", "image/x-quicktime" },
            new MimeTypeExtSig(".qtif", "image/x-quicktime" },
            new MimeTypeExtSig(".qtl", "application/x-quicktimeplayer" },
            new MimeTypeExtSig(".qwd", "application/vnd.quark.quarkxpress" },
            new MimeTypeExtSig(".qwt", "application/vnd.quark.quarkxpress" },
            new MimeTypeExtSig(".qxb", "application/vnd.quark.quarkxpress" },
            new MimeTypeExtSig(".qxd", "application/octet-stream" },
            new MimeTypeExtSig(".qxl", "application/vnd.quark.quarkxpress" },
            new MimeTypeExtSig(".qxt", "application/vnd.quark.quarkxpress" },
            new MimeTypeExtSig(".ra",  "audio/x-pn-realaudio" },
            new MimeTypeExtSig(".raf", "image/x-fuji-raf" },
            new MimeTypeExtSig(".ram", "audio/x-pn-realaudio" },
            new MimeTypeExtSig(".raml", "application/raml+yaml" },
            new MimeTypeExtSig(".rapd", "application/route-apd+xml" },

            new MimeTypeExtSig(".ras", "image/x-cmu-raster" },
            new MimeTypeExtSig(".rast", "image/cmu-raster" },
            new MimeTypeExtSig(".rat", "application/rat-file" },
            new MimeTypeExtSig(".raw", "image/x-panasonic-rw" },


            new MimeTypeExtSig(".rcprofile", "application/vnd.ipunplugged.rcprofile" },
            
            new MimeTypeExtSig(".rdf", "application/rdf+xml" },
            new MimeTypeExtSig(".rdlc", "application/xml" },
            new MimeTypeExtSig(".rdp", "application/rdp" },
            new MimeTypeExtSig(".rdz", "application/vnd.data-vision.rdz" },
            new MimeTypeExtSig(".reg", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".relo", "application/p2p-overlay+xml" },
            new MimeTypeExtSig(".rep", "application/vnd.businessobjects" },
            new MimeTypeExtSig(".res", "application/x-dtbresource+xml" },
            new MimeTypeExtSig(".resx", "application/xml" },
            new MimeTypeExtSig(".rexx", "text/x-scriptrexx" },
            new MimeTypeExtSig(".rf",  "image/vnd.rn-realflash" },
            new MimeTypeExtSig(".rgb", "image/x-rgb" },
            new MimeTypeExtSig(".rgs", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".rif", "application/reginfo+xml" },
            new MimeTypeExtSig(".rip", "audio/vnd.rip" },
            new MimeTypeExtSig(".ris", "application/x-research-info-systems" },
            new MimeTypeExtSig(".rl",  "application/resource-lists+xml" },
            new MimeTypeExtSig(".rlc", "image/vnd.fujixerox.edmics-rlc" },
            new MimeTypeExtSig(".rld", "application/resource-lists-diff+xml" },
            new MimeTypeExtSig(".rm",  "application/vnd.rn-realmedia" },
            new MimeTypeExtSig(".rmi", "audio/mid" },
            new MimeTypeExtSig(".rmm", "audio/x-pn-realaudio" },
            new MimeTypeExtSig(".rmp", "application/vnd.rn-rn_music_package" },
            new MimeTypeExtSig(".rms", "application/vnd.jcp.javame.midlet-rms" },
            new MimeTypeExtSig(".rmvb", "application/vnd.rn-realmedia-vbr" },
            new MimeTypeExtSig(".rnc", "application/relax-ng-compact-syntax" },
            new MimeTypeExtSig(".rng", "application/xml" },
            new MimeTypeExtSig(".rnx", "application/vndrn-realplayer" },
            new MimeTypeExtSig(".roa", "application/rpki-roa" },
            new MimeTypeExtSig(".roff", "application/x-troff" },
            new MimeTypeExtSig(".rp",  "image/vndrn-realpix" },
            new MimeTypeExtSig(".rp9", "application/vnd.cloanto.rp9" },
            new MimeTypeExtSig(".rpm", "audio/x-pn-realaudio-plugin" },
            new MimeTypeExtSig(".rpss", "application/vnd.nokia.radio-presets" },
            new MimeTypeExtSig(".rpst", "application/vnd.nokia.radio-preset" },
            new MimeTypeExtSig(".rq",  "application/sparql-query" },
            new MimeTypeExtSig(".rqy", "text/x-ms-rqy" },
            new MimeTypeExtSig(".rs",  "application/rls-services+xml" },
            new MimeTypeExtSig(".rsat", "application/atsc-rsat+xml" },
            new MimeTypeExtSig(".rsd", "application/rsd+xml" },
            new MimeTypeExtSig(".rsheet", "application/urc-ressheet+xml" },
            new MimeTypeExtSig(".rss", "application/rss+xml" },
            new MimeTypeExtSig(".rt",  "text/vndrn-realtext" },
            new MimeTypeExtSig(".rtf", "application/rtf" },
            new MimeTypeExtSig(".rtx", "text/richtext" },
            new MimeTypeExtSig(".ruleset",   "application/xml" },
            new MimeTypeExtSig(".run", "application/x-makeself" },
            new MimeTypeExtSig(".rusd", "application/route-usd+xml" },
            new MimeTypeExtSig(".rv",  "video/vndrn-realvideo" },
            new MimeTypeExtSig(".rvt", "application/octet-stream" },
            new MimeTypeExtSig(".rw2", "image/x-panasonic-rw2" },
            new MimeTypeExtSig(".rwl", "image/x-panasonic-rw2" },
            new MimeTypeExtSig(".s",   "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".s3m", "audio/s3m" }, 
            new MimeTypeExtSig(".saf", "application/vnd.yamaha.smaf-audio" },
            new MimeTypeExtSig(".safariextz",    "application/x-safari-safariextz" },
            new MimeTypeExtSig(".sass", "text/x-sass" },
            new MimeTypeExtSig(".saveme", "application/octet-stream" },
            new MimeTypeExtSig(".sbk", "application/x-tbook" },
            new MimeTypeExtSig(".sbml", "application/sbml+xml" },
            new MimeTypeExtSig(".sc",  "application/vnd.ibm.secure-container" },
            new MimeTypeExtSig(".scd", "application/x-msschedule" },
            new MimeTypeExtSig(".scm", "application/vnd.lotus-screencam" },
            new MimeTypeExtSig(".scq", "application/scvp-cv-request" },
            new MimeTypeExtSig(".scr", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".scs", "application/scvp-cv-response" },
            new MimeTypeExtSig(".scss", "text/x-scss" },
            new MimeTypeExtSig(".sct", "text/scriptlet" },
            new MimeTypeExtSig(".scurl", "text/vnd.curl.scurl" },
            new MimeTypeExtSig(".sd2", "audio/x-sd2" },
            new MimeTypeExtSig(".sda", "application/vnd.stardivision.draw" },
            new MimeTypeExtSig(".sdc", "application/vnd.stardivision.calc" },
            new MimeTypeExtSig(".sdd", "application/vnd.stardivision.impress" },
            new MimeTypeExtSig(".sdkd", "application/vnd.solent.sdkm+xml" },
            new MimeTypeExtSig(".sdkm", "application/vnd.solent.sdkm+xml" },
            new MimeTypeExtSig(".sdml", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".sdoc", "application/sdoc" },
            new MimeTypeExtSig(".sdp", "application/sdp" },
            new MimeTypeExtSig(".sdr", "application/sounder" },
            new MimeTypeExtSig(".sdw", "application/vnd.stardivision.writer" },
            new MimeTypeExtSig(".sea", "application/octet-stream" },
            new MimeTypeExtSig(".searchConnector-ms", "application/windows-search-connector+xml" },
            new MimeTypeExtSig(".see", "application/vnd.seemail" },
            new MimeTypeExtSig(".seed", "application/vnd.fdsn.seed" },
            new MimeTypeExtSig(".sema", "application/vnd.sema" },
            new MimeTypeExtSig(".semd", "application/vnd.semd" },
            new MimeTypeExtSig(".semf", "application/vnd.semf" },
            new MimeTypeExtSig(".senmlx", "application/senml+xml" },
            new MimeTypeExtSig(".sensmlx",   "application/sensml+xml" },
            new MimeTypeExtSig(".ser", "application/java-serialized-object" },
            new MimeTypeExtSig(".set", "application/set" },
            new MimeTypeExtSig(".setpay", "application/set-payment-initiation" },
            new MimeTypeExtSig(".setreg", "application/set-registration-initiation" },
            new MimeTypeExtSig(".settings",  "application/xml" },
            new MimeTypeExtSig(".sfd-hdstx", "application/vnd.hydrostatix.sof-data" },
            new MimeTypeExtSig(".sfs", "application/vnd.spotfire.sfs" },
            new MimeTypeExtSig(".sfv", "text/x-sfv" },
            new MimeTypeExtSig(".sgf", "application/x-go-sgf" },
            new MimeTypeExtSig(".sgi", "image/sgi" },
            new MimeTypeExtSig(".sgimb",   "application/x-sgimb" },
            new MimeTypeExtSig(".sgl", "application/vnd.stardivision.writer-global" },
            new MimeTypeExtSig(".sgm", "text/sgml" },
            new MimeTypeExtSig(".sgml", "text/sgml" },
            new MimeTypeExtSig(".sh",  "application/x-sh" },
            new MimeTypeExtSig(".shar", "application/x-shar" },
            new MimeTypeExtSig(".shex", "text/shex" },
            new MimeTypeExtSig(".shf", "application/shf+xml" },
            new MimeTypeExtSig(".shtml", "text/html" },
            new MimeTypeExtSig(".sid", "image/x-mrsid-image" },
            new MimeTypeExtSig(".sieve", "application/sieve" },
            new MimeTypeExtSig(".sig", "application/pgp-signature" },
            new MimeTypeExtSig(".sil", "audio/silk" },
            new MimeTypeExtSig(".silo", "model/mesh" },
            new MimeTypeExtSig(".sis", "application/vnd.symbian.install" },
            new MimeTypeExtSig(".sisx", "application/vnd.symbian.install" },
            new MimeTypeExtSig(".sit", "application/x-stuffit" },
            new MimeTypeExtSig(".sitemap",   "application/xml" },
            new MimeTypeExtSig(".sitx", "application/x-stuffitx" },
            new MimeTypeExtSig(".siv", "application/sieve" },
            new MimeTypeExtSig(".skd", "application/vnd.koan" },
            new MimeTypeExtSig(".skin", "application/xml" },
            new MimeTypeExtSig(".skm", "application/vnd.koan" },
            new MimeTypeExtSig(".skp", "application/x-koan" },
            new MimeTypeExtSig(".skt", "application/vnd.koan" },
            new MimeTypeExtSig(".sl",  "application/x-seelogo" },
            new MimeTypeExtSig(".sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12" },
            new MimeTypeExtSig(".sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide" },
            new MimeTypeExtSig(".slim", "text/slim" },
            new MimeTypeExtSig(".slk", "application/vnd.ms-excel" },
            new MimeTypeExtSig(".slm", "text/slim" },
            new MimeTypeExtSig(".sln", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".sls", "application/route-s-tsid+xml" },
            new MimeTypeExtSig(".slt", "application/vnd.epson.salt" },
            new MimeTypeExtSig(".slupkg-ms", "application/x-ms-license" },
            new MimeTypeExtSig(".sm",  "application/vnd.stepmania.stepchart" },
            new MimeTypeExtSig(".smd", "audio/x-smd" },
            new MimeTypeExtSig(".smf", "application/vnd.stardivision.math" },
            new MimeTypeExtSig(".smi", "application/octet-stream" },
            new MimeTypeExtSig(".smil", "application/smil+xml" },
            new MimeTypeExtSig(".smv", "video/x-smv" },
            new MimeTypeExtSig(".smx", "audio/x-smd" },
            new MimeTypeExtSig(".smz", "audio/x-smd" },
            new MimeTypeExtSig(".smzip", "application/vnd.stepmania.package" },
            new MimeTypeExtSig(".snd", "audio/basic" },
            new MimeTypeExtSig(".snf", "application/x-font-snf" },
            new MimeTypeExtSig(".snippet",   "application/xml" },
            new MimeTypeExtSig(".snp", "application/octet-stream" },
            new MimeTypeExtSig(".so",  "application/octet-stream" },
            new MimeTypeExtSig(".sol", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".sor", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".spc", "application/x-pkcs7-certificates" },
            new MimeTypeExtSig(".spdx", "text/spdx" },
            new MimeTypeExtSig(".spf", "application/vnd.yamaha.smaf-phrase" },
            new MimeTypeExtSig(".spl", "application/futuresplash" },
            new MimeTypeExtSig(".spot", "text/vnd.in3d.spot" },
            new MimeTypeExtSig(".spp", "application/scvp-vp-response" },
            new MimeTypeExtSig(".spq", "application/scvp-vp-request" },
            new MimeTypeExtSig(".spr", "application/x-sprite" },
            new MimeTypeExtSig(".sprite", "application/x-sprite" },
            new MimeTypeExtSig(".spx", "audio/ogg" },
            new MimeTypeExtSig(".sql", "application/sql" },
            new MimeTypeExtSig(".sr2", "image/x-sony-sr2" },
            new MimeTypeExtSig(".src", "application/x-wais-source" },
            new MimeTypeExtSig(".srf", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".srt", "application/x-subrip" },
            new MimeTypeExtSig(".sru", "application/sru+xml" },
            new MimeTypeExtSig(".srx", "application/sparql-results+xml" },
            new MimeTypeExtSig(".ssdl", "application/ssdl+xml" },
            new MimeTypeExtSig(".sse", "application/vnd.kodak-descriptor" },
            new MimeTypeExtSig(".ssf", "application/vnd.epson.ssf" },
            new MimeTypeExtSig(".ssi", "text/x-server-parsed-html" },

            new MimeTypeExtSig(".ssm", "application/streamingmedia" },
            new MimeTypeExtSig(".ssml", "application/ssml+xml" },
            new MimeTypeExtSig(".sst", "application/vnd.ms-pki.certstore" },
            new MimeTypeExtSig(".st",  "application/vnd.sailingtracker.track" },
            new MimeTypeExtSig(".stc", "application/vnd.sun.xml.calc.template" },
            new MimeTypeExtSig(".std", "application/vnd.sun.xml.draw.template" },
            new MimeTypeExtSig(".step", "application/step" },
            new MimeTypeExtSig(".stf", "application/vnd.wt.stf" },
            new MimeTypeExtSig(".sti", "application/vnd.sun.xml.impress.template" },
            new MimeTypeExtSig(".stk", "application/hyperstudio" },
            new MimeTypeExtSig(".stl", "application/vnd.ms-pki.stl" },
            new MimeTypeExtSig(".stp", "application/step" },
            new MimeTypeExtSig(".stpx", "model/step+xml" },
            new MimeTypeExtSig(".stpxz", "model/step-xml+zip" },
            new MimeTypeExtSig(".stpz", "model/step+zip" },
            new MimeTypeExtSig(".str", "application/vnd.pg.format" },
            new MimeTypeExtSig(".stw", "application/vnd.sun.xml.writer.template" },
            new MimeTypeExtSig(".sty", "text/x-tex" },
            new MimeTypeExtSig(".styl", "text/stylus" },
            new MimeTypeExtSig(".stylus", "text/stylus" },
            new MimeTypeExtSig(".sub", "text/vnd.dvb.subtitle" },
            new MimeTypeExtSig(".sus", "application/vnd.sus-calendar" },
            new MimeTypeExtSig(".susp", "application/vnd.sus-calendar" },
            new MimeTypeExtSig(".sv4cpio",   "application/x-sv4cpio" },
            new MimeTypeExtSig(".sv4crc", "application/x-sv4crc" },
            new MimeTypeExtSig(".svc", "application/xml" },
            new MimeTypeExtSig(".svd", "application/vnd.svd" },
            new MimeTypeExtSig(".svf", "image/vnddwg" },
            new MimeTypeExtSig(".svg", "image/svg+xml" },
            new MimeTypeExtSig(".svgz", "image/svg+xml" },
            new MimeTypeExtSig(".svr", "application/x-world" },
            new MimeTypeExtSig(".swa", "application/x-director" },

        ;
            new MimeTypeExtSig(".swi", "application/vnd.arastra.swi" },
            new MimeTypeExtSig(".swidtag",   "application/swid+xml" },
            new MimeTypeExtSig(".sxc", "application/vnd.sun.xml.calc" },
            new MimeTypeExtSig(".sxd", "application/vnd.sun.xml.draw" },
            new MimeTypeExtSig(".sxg", "application/vnd.sun.xml.writer.global" },
            new MimeTypeExtSig(".sxi", "application/vnd.sun.xml.impress" },
            new MimeTypeExtSig(".sxm", "application/vnd.sun.xml.math" },
            new MimeTypeExtSig(".sxw", "application/vnd.sun.xml.writer" },
            new MimeTypeExtSig(".t",   "application/x-troff" },
            new MimeTypeExtSig(".t3",  "application/x-t3vm-image" },
            new MimeTypeExtSig(".t38", "image/t38" },
            new MimeTypeExtSig(".taglet", "application/vnd.mynfc" },
            new MimeTypeExtSig(".talk", "text/x-speech" },
            new MimeTypeExtSig(".tao", "application/vnd.tao.intent-module-archive" },
            new MimeTypeExtSig(".tap", "image/vnd.tencent.tap" },

            new MimeTypeExtSig(".taz", "application/x-gtar" },
            new MimeTypeExtSig(".tbk", "application/toolbook" },
            new MimeTypeExtSig(".tcap", "application/vnd.3gpp2.tcap" },
            new MimeTypeExtSig(".tcl", "application/x-tcl" },
            new MimeTypeExtSig(".tcsh", "text/x-scripttcsh" },
            new MimeTypeExtSig(".td",  "application/urc-targetdesc+xml" },
            new MimeTypeExtSig(".teacher",   "application/vnd.smart.teacher" },
            new MimeTypeExtSig(".tei", "application/tei+xml" },
            new MimeTypeExtSig(".teicorpus", "application/tei+xml" },
            new MimeTypeExtSig(".template",  "application/x-iwork-pages-sfftemplate" },
            new MimeTypeExtSig(".testrunconfig", "application/xml" },
            new MimeTypeExtSig(".testsettings",  "application/xml" },
            new MimeTypeExtSig(".tex", "application/x-tex" },
            new MimeTypeExtSig(".texi", "application/x-texinfo" },
            new MimeTypeExtSig(".texinfo",   "application/x-texinfo" },
            new MimeTypeExtSig(".text", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".tfi", "application/thraud+xml" },
            new MimeTypeExtSig(".tfm", "application/x-tex-tfm" },
            new MimeTypeExtSig(".tfx", "image/tiff-fx" },
            new MimeTypeExtSig(".tga", "image/x-tga" },
            new MimeTypeExtSig(".tgz", "application/x-compressed" },
            new MimeTypeExtSig(".thmx", "application/vnd.ms-officetheme" },
            new MimeTypeExtSig(".thn", "application/octet-stream" },

            new MimeTypeExtSig(".tk",  "application/x-tcl" },
            new MimeTypeExtSig(".tlh", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".tli", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".tmo", "application/vnd.tmobile-livetv" },
            new MimeTypeExtSig(".toc", "application/octet-stream" },
            new MimeTypeExtSig(".toml", "application/toml" },

            new MimeTypeExtSig(".tpl", "application/vnd.groove-tool-template" },
            new MimeTypeExtSig(".tpt", "application/vnd.trid.tpt" },
            new MimeTypeExtSig(".tr",  "application/x-troff" },
            new MimeTypeExtSig(".tra", "application/vnd.trueapp" },
            new MimeTypeExtSig(".trig", "application/trig" },
            new MimeTypeExtSig(".trm", "application/x-msterminal" },
            new MimeTypeExtSig(".trx", "application/xml" },
            new MimeTypeExtSig(".ts",  "text/typescript" },
            new MimeTypeExtSig(".tsd", "application/timestamped-data" },
            new MimeTypeExtSig(".tsi", "audio/tsp-audio" },
            new MimeTypeExtSig(".tsp", "application/dsptype" },
            new MimeTypeExtSig(".tsv", "text/tab-separated-values" },
            new MimeTypeExtSig(".tsx", "application/typescript" },

            new MimeTypeExtSig( "application/font-sfnt" },
            new MimeTypeExtSig(".ttl", "text/turtle" },
            new MimeTypeExtSig(".ttml", "application/ttml+xml" },
            new MimeTypeExtSig(".tts", "video/vnd.dlna.mpeg-tts" },
            new MimeTypeExtSig(".turbot", "image/florian" },
            new MimeTypeExtSig(".twd", "application/vnd.simtech-mindmapper" },
            new MimeTypeExtSig(".twds", "application/vnd.simtech-mindmapper" },
            new MimeTypeExtSig(".txd", "application/vnd.genomatix.tuxedo" },
            new MimeTypeExtSig(".txf", "application/vnd.mobius.txf" },
            new MimeTypeExtSig(".txt", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".u32", "application/octet-stream" },
            new MimeTypeExtSig(".u3d", "model/u3d" },
            new MimeTypeExtSig(".u8dsn", "message/global-delivery-status" },
            new MimeTypeExtSig(".u8hdr", "message/global-headers" },
            new MimeTypeExtSig(".u8mdn", "message/global-disposition-notification" },
            new MimeTypeExtSig(".u8msg", "message/global" },
            new MimeTypeExtSig(".ubj", "application/ubjson" },
            new MimeTypeExtSig(".udeb", "application/x-debian-package" },
            new MimeTypeExtSig(".ufd", "application/vnd.ufdl" },
            new MimeTypeExtSig(".ufdl", "application/vnd.ufdl" },
            new MimeTypeExtSig(".uil", "text/x-uil" },
            new MimeTypeExtSig(".uls", "text/iuls" },
            new MimeTypeExtSig(".ulx", "application/x-glulx" },
            new MimeTypeExtSig(".umj", "application/vnd.umajin" },
            new MimeTypeExtSig(".uni", "text/uri-list" },
            new MimeTypeExtSig(".unis", "text/uri-list" },
            new MimeTypeExtSig(".unityweb",  "application/vnd.unity" },
            new MimeTypeExtSig(".unv", "application/i-deas" },
            new MimeTypeExtSig(".uo",  "application/vnd.uoml+xml" },
            new MimeTypeExtSig(".uoml", "application/vnd.uoml+xml" },
            new MimeTypeExtSig(".uri", "text/uri-list" },
            new MimeTypeExtSig(".uris", "text/uri-list" },
            new MimeTypeExtSig(".urls", "text/uri-list" },
            new MimeTypeExtSig(".usda", "model/vnd.usda" },
            new MimeTypeExtSig(".usdz", "model/vnd.usdz+zip" },
            new MimeTypeExtSig(".user", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".ustar", "application/x-ustar" },
            new MimeTypeExtSig(".utz", "application/vnd.uiq.theme" },
            new MimeTypeExtSig(".uu",  "text/x-uuencode" },
            new MimeTypeExtSig(".uue", "text/x-uuencode" },
            new MimeTypeExtSig(".uva", "audio/vnd.dece.audio" },
            new MimeTypeExtSig(".uvd", "application/vnd.dece.data" },
            new MimeTypeExtSig(".uvf", "application/vnd.dece.data" },
            new MimeTypeExtSig(".uvg", "image/vnd.dece.graphic" },
            new MimeTypeExtSig(".uvh", "video/vnd.dece.hd" },
            new MimeTypeExtSig(".uvi", "image/vnd.dece.graphic" },
            new MimeTypeExtSig(".uvm", "video/vnd.dece.mobile" },
            new MimeTypeExtSig(".uvp", "video/vnd.dece.pd" },
            new MimeTypeExtSig(".uvs", "video/vnd.dece.sd" },
            new MimeTypeExtSig(".uvt", "application/vnd.dece.ttml+xml" },
            new MimeTypeExtSig(".uvu", "video/vnd.uvvu.mp4" },
            new MimeTypeExtSig(".uvv", "video/vnd.dece.video" },
            new MimeTypeExtSig(".uvva", "audio/vnd.dece.audio" },
            new MimeTypeExtSig(".uvvd", "application/vnd.dece.data" },
            new MimeTypeExtSig(".uvvf", "application/vnd.dece.data" },
            new MimeTypeExtSig(".uvvg", "image/vnd.dece.graphic" },
            new MimeTypeExtSig(".uvvh", "video/vnd.dece.hd" },
            new MimeTypeExtSig(".uvvi", "image/vnd.dece.graphic" },
            new MimeTypeExtSig(".uvvm", "video/vnd.dece.mobile" },
            new MimeTypeExtSig(".uvvp", "video/vnd.dece.pd" },
            new MimeTypeExtSig(".uvvs", "video/vnd.dece.sd" },
            new MimeTypeExtSig(".uvvt", "application/vnd.dece.ttml+xml" },
            new MimeTypeExtSig(".uvvu", "video/vnd.uvvu.mp4" },
            new MimeTypeExtSig(".uvvv", "video/vnd.dece.video" },
            new MimeTypeExtSig(".uvvx", "application/vnd.dece.unspecified" },
            new MimeTypeExtSig(".uvvz", "application/vnd.dece.zip" },
            new MimeTypeExtSig(".uvx", "application/vnd.dece.unspecified" },
            new MimeTypeExtSig(".uvz", "application/vnd.dece.zip" },

            new MimeTypeExtSig(".vbk", "video/mpeg" },
            new MimeTypeExtSig(".vbox", "application/x-virtualbox-vbox" },
            new MimeTypeExtSig(".vbox-extpack",  "application/x-virtualbox-vbox-extpack" },
            new MimeTypeExtSig(".vbproj", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".vbs", "text/vbscript" },
            new MimeTypeExtSig(".vcard", "text/vcard" },
            new MimeTypeExtSig(".vcd", "application/x-cdlink" },
            new MimeTypeExtSig(".vcf", "text/x-vcard" },
            new MimeTypeExtSig(".vcg", "application/vnd.groove-vcard" },
            new MimeTypeExtSig(".vcproj", "application/xml" },


            new MimeTypeExtSig(".vcx", "application/vnd.vcx" },
            new MimeTypeExtSig(".vcxproj",   "application/xml" },
            new MimeTypeExtSig(".vda", "application/vda" },
            new MimeTypeExtSig(".vdi", "application/x-virtualbox-vdi" },
            new MimeTypeExtSig(".vdo", "video/vdo" },


            new MimeTypeExtSig(".vew", "application/groupwise" },
            new MimeTypeExtSig(".vhd", "application/x-virtualbox-vhd" },
            new MimeTypeExtSig(".vis", "application/vnd.visionary" },
            new MimeTypeExtSig(".viv", "video/vnd.vivo" },
            new MimeTypeExtSig(".vivo", "video/vivo" },
            new MimeTypeExtSig(".vmd", "application/vocaltec-media-desc" },
            new MimeTypeExtSig(".vmdk", "application/x-virtualbox-vmdk" },
            new MimeTypeExtSig(".vmf", "application/vocaltec-media-file" },

            new MimeTypeExtSig(".vob", "video/x-ms-vob" },
            new MimeTypeExtSig(".voc", "audio/voc" },
            new MimeTypeExtSig(".vor", "application/vnd.stardivision.writer" },
            new MimeTypeExtSig(".vos", "video/vosaic" },
            new MimeTypeExtSig(".vox", "application/x-authorware-bin" },
            new MimeTypeExtSig(".vqe", "audio/x-twinvq-plugin" },
            new MimeTypeExtSig(".vqf", "audio/x-twinvq" },
            new MimeTypeExtSig(".vql", "audio/x-twinvq-plugin" },
            new MimeTypeExtSig(".vrml", "model/vrml" },
            new MimeTypeExtSig(".vrt", "x-world/x-vrt" },
            new MimeTypeExtSig(".vscontent", "application/xml" },


            new MimeTypeExtSig(".vsf", "application/vnd.vsf" },
            new MimeTypeExtSig(".vsi", "application/ms-vsi" },
            new MimeTypeExtSig(".vsix", "application/vsix" },

            new MimeTypeExtSig(".vsmdi", "application/xml" },




            new MimeTypeExtSig(".vtf", "image/vnd.valve.source.texture" },
            new MimeTypeExtSig(".vtt", "text/vtt" },
            new MimeTypeExtSig(".vtu", "model/vnd.vtu" },

            new MimeTypeExtSig(".vxml", "application/voicexml+xml" },
            new MimeTypeExtSig(".w3d", "application/x-director" },
            new MimeTypeExtSig(".w60", "application/wordperfect60" },
            new MimeTypeExtSig(".w61", "application/wordperfect61" },
            new MimeTypeExtSig(".w6w", "application/msword" },
            new MimeTypeExtSig(".wad", "application/x-doom" },
            new MimeTypeExtSig(".wadl", "application/vnd.sun.wadl+xml" },
            new MimeTypeExtSig(".war", "application/java-archive" },
            new MimeTypeExtSig(".wasm", "application/wasm" },
            new MimeTypeExtSig(".wax", "audio/x-ms-wax" },
            new MimeTypeExtSig(".wb1", "application/x-qpro" },
            new MimeTypeExtSig(".wbk", "application/msword" },
            new MimeTypeExtSig(".wbmp", "image/vnd.wap.wbmp" },
            new MimeTypeExtSig(".wbs", "application/vnd.criticaltools.wbs+xml" },
            new MimeTypeExtSig(".wbxml", "application/vnd.wap.wbxml" },
            new MimeTypeExtSig(".wcm", "application/vnd.ms-works" },
            new MimeTypeExtSig(".wdb", "application/vnd.ms-works" },
            new MimeTypeExtSig(".wdp", "image/vnd.ms-photo" },
            new MimeTypeExtSig(".web", "application/vndxara" },

            new MimeTypeExtSig(".webapp", "application/x-web-app-manifest+json" },
            new MimeTypeExtSig(".webarchive",    "application/x-safari-webarchive" },

            new MimeTypeExtSig(".webmanifest",   "application/manifest+json" },

            new MimeTypeExtSig(".webtest",   "application/xml" },
            new MimeTypeExtSig(".wg",  "application/vnd.pmi.widget" },
            new MimeTypeExtSig(".wgsl", "text/wgsl" },
            new MimeTypeExtSig(".wgt", "application/widget" },
            new MimeTypeExtSig(".wif", "application/watcherinfo+xml" },
            new MimeTypeExtSig(".wiq", "application/xml" },
            new MimeTypeExtSig(".wiz", "application/msword" },
            new MimeTypeExtSig(".wk1", "application/x-123" },
            new MimeTypeExtSig(".wks", "application/vnd.ms-works" },
            new MimeTypeExtSig(".WLMP", "application/wlmoviemaker" },
            new MimeTypeExtSig(".wlpginstall",   "application/x-wlpg-detect" },
            new MimeTypeExtSig(".wlpginstall3",  "application/x-wlpg3-detect" },
            new MimeTypeExtSig(".wm",  "video/x-ms-wm" },

        ;
            new MimeTypeExtSig(".wmd", "application/x-ms-wmd" },
            new MimeTypeExtSig(".wmf", "application/x-msmetafile" },
            new MimeTypeExtSig(".wml", "text/vnd.wap.wml" },
            new MimeTypeExtSig(".wmlc", "application/vnd.wap.wmlc" },
            new MimeTypeExtSig(".wmls", "text/vnd.wap.wmlscript" },
            new MimeTypeExtSig(".wmlsc", "application/vnd.wap.wmlscriptc" },
            new MimeTypeExtSig(".wmp", "video/x-ms-wmp" },

            new MimeTypeExtSig(".wmx", "video/x-ms-wmx" },
            new MimeTypeExtSig(".wmz", "application/x-ms-wmz" },
            new MimeTypeExtSig(".woff", "application/font-woff" },
            new MimeTypeExtSig(".woff2", "application/font-woff2" },
            new MimeTypeExtSig(".word", "application/msword" },
            new MimeTypeExtSig(".wp",  "application/wordperfect" },
            new MimeTypeExtSig(".wp5", "application/wordperfect" },
            new MimeTypeExtSig(".wp6", "application/wordperfect" },
            new MimeTypeExtSig(".wpd", "application/vnd.wordperfect" },
            new MimeTypeExtSig(".wpl", "application/vnd.ms-wpl" },
            new MimeTypeExtSig(".wps", "application/vnd.ms-works" },
            new MimeTypeExtSig(".wq1", "application/x-lotus" },
            new MimeTypeExtSig(".wqd", "application/vnd.wqd" },
            new MimeTypeExtSig(".wri", "application/x-mswrite" },
            new MimeTypeExtSig(".wrl", "x-world/x-vrml" },
            new MimeTypeExtSig(".wrz", "x-world/x-vrml" },
            new MimeTypeExtSig(".wsc", "text/scriptlet" },
            new MimeTypeExtSig(".wspolicy",  "application/wspolicy+xml" },
            new MimeTypeExtSig(".wsrc", "application/x-wais-source" },
            new MimeTypeExtSig(".wtb", "application/vnd.webturbo" },
            new MimeTypeExtSig(".wtk", "application/x-wintalk" },
            new MimeTypeExtSig(".wvx", "video/x-ms-wvx" },
            new MimeTypeExtSig(".wz",  "application/x-wingz" },
            new MimeTypeExtSig(".x",   "application/directx" },
            new MimeTypeExtSig(".x-png", "image/png" },
            new MimeTypeExtSig(".x32", "application/x-authorware-bin" },
            new MimeTypeExtSig(".x3d", "application/vnd.hzn-3d-crossword" },
            new MimeTypeExtSig(".x3db", "model/x3d+binary" },
            new MimeTypeExtSig(".x3dbz", "model/x3d+binary" },
            new MimeTypeExtSig(".x3dv", "model/x3d+vrml" },
            new MimeTypeExtSig(".x3dvz", "model/x3d+vrml" },
            new MimeTypeExtSig(".x3dz", "model/x3d+541xml" },
            new MimeTypeExtSig(".x3f", "image/x-sigma-x3f" },
            new MimeTypeExtSig(".xaf", "x-world/x-vrml" },
            new MimeTypeExtSig(".xaml", "application/xaml+xml" },
            new MimeTypeExtSig(".xap", "application/x-silverlight-app" },
            new MimeTypeExtSig(".xar", "application/vnd.xara" },
            new MimeTypeExtSig(".xav", "application/xcap-att+xml" },
            new MimeTypeExtSig(".xbap", "application/x-ms-xbap" },
            new MimeTypeExtSig(".xbd", "application/vnd.fujixerox.docuworks.binder" },
            new MimeTypeExtSig(".xbm", "image/x-xbitmap" },
            new MimeTypeExtSig(".xca", "application/xcap-caps+xml" },
            new MimeTypeExtSig(".xcf", "image/x-xcf" },
            new MimeTypeExtSig(".xcs", "application/calendar+xml" },
            new MimeTypeExtSig(".xdf", "application/xcap-diff+xml" },
            new MimeTypeExtSig(".xdm", "application/vnd.syncml.dm+xml" },
            new MimeTypeExtSig(".xdp", "application/vnd.adobe.xdp+xml" },
            new MimeTypeExtSig(".xdr", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".xdssc", "application/dssc+xml" },
            new MimeTypeExtSig(".xdw", "application/vnd.fujixerox.docuworks" },
            new MimeTypeExtSig(".xel", "application/xcap-el+xml" },
            new MimeTypeExtSig(".xenc", "application/xenc+xml" },
            new MimeTypeExtSig(".xer", "application/patch-ops-error+xml" },
            new MimeTypeExtSig(".xfdf", "application/vnd.adobe.xfdf" },
            new MimeTypeExtSig(".xfdl", "application/vnd.xfdl" },
            new MimeTypeExtSig(".xgz", "xgl/drawing" },
            new MimeTypeExtSig(".xht", "application/xhtml+xml" },
            new MimeTypeExtSig(".xhtm", "application/vnd.pwg-xhtml-print+xml" },
            new MimeTypeExtSig(".xhtml", "application/xhtml+xml" },
            new MimeTypeExtSig(".xhvml", "application/xv+xml" },
            new MimeTypeExtSig(".xif", "image/vnd.xiff" },


            new MimeTypeExtSig(".xlf", "application/x-xliff+xml" },
            new MimeTypeExtSig(".xm",  "audio/xm" },
            new MimeTypeExtSig(".xmp", "application/octet-stream" },
            new MimeTypeExtSig(".xmta", "application/xml" },
            new MimeTypeExtSig(".xmz", "xgl/movie" },
            new MimeTypeExtSig(".xns", "application/xcap-ns+xml" },
            new MimeTypeExtSig(".xo",  "application/vnd.olpc-sugar" },
            new MimeTypeExtSig(".xof", "x-world/x-vrml" },
            new MimeTypeExtSig(".XOML", "text/plain" , MimeSignature._TEXT),
            new MimeTypeExtSig(".xop", "application/xop+xml" },
            new MimeTypeExtSig(".xpi", "application/x-xpinstall" },
            new MimeTypeExtSig(".xpix", "application/x-vndls-xpix" },
            new MimeTypeExtSig(".xpl", "application/xproc+xml" },
            new MimeTypeExtSig(".xpm", "image/x-xpixmap" },
            new MimeTypeExtSig(".xpr", "application/vnd.is-xpr" },
            new MimeTypeExtSig(".xps", "application/vnd.ms-xpsdocument" },
            new MimeTypeExtSig(".xpw", "application/vnd.intercon.formnet" },
            new MimeTypeExtSig(".xpx", "application/vnd.intercon.formnet" },
            new MimeTypeExtSig(".xsm", "application/vnd.syncml+xml" },
            new MimeTypeExtSig(".xsn", "application/octet-stream" },
            new MimeTypeExtSig(".xspf", "application/xspf+xml" },
            new MimeTypeExtSig(".xsr", "video/x-amt-showrun" },
            new MimeTypeExtSig(".xss", "application/xml" },
            new MimeTypeExtSig(".xtp", "application/octet-stream" },
            new MimeTypeExtSig(".xul", "application/vnd.mozilla.xul+xml" },
            new MimeTypeExtSig(".xvm", "application/xv+xml" },
            new MimeTypeExtSig(".xvml", "application/xv+xml" },
            new MimeTypeExtSig(".xwd", "image/x-xwindowdump" },
            new MimeTypeExtSig(".xyz", "chemical/x-xyz" },

            new MimeTypeExtSig(".x_b", "model/vnd.parasolid.transmit.binary" },
            new MimeTypeExtSig(".x_t", "model/vnd.parasolid.transmit.text" },
            new MimeTypeExtSig(".yaml", "application/yaml" },
            new MimeTypeExtSig(".yang", "application/yang" },
            new MimeTypeExtSig(".yin", "application/yin+xml" },
            new MimeTypeExtSig(".yml", "application/yaml" },
            new MimeTypeExtSig(".ymp", "text/x-suse-ymp" },
            new MimeTypeExtSig(".z",   "application/x-compress" },
            new MimeTypeExtSig(".z1",  "application/x-zmachine" },
            new MimeTypeExtSig(".z2",  "application/x-zmachine" },
            new MimeTypeExtSig(".z3",  "application/x-zmachine" },
            new MimeTypeExtSig(".z4",  "application/x-zmachine" },
            new MimeTypeExtSig(".z5",  "application/x-zmachine" },
            new MimeTypeExtSig(".z6",  "application/x-zmachine" },
            new MimeTypeExtSig(".z7",  "application/x-zmachine" },
            new MimeTypeExtSig(".z8",  "application/x-zmachine" },
            new MimeTypeExtSig(".zaz", "application/vnd.zzazz.deck+xml" },

            new MimeTypeExtSig(".zir", "application/vnd.zul" },
            new MimeTypeExtSig(".zirz", "application/vnd.zul" },
            new MimeTypeExtSig(".zmm", "application/vnd.handheld-entertainment+xml" },
            new MimeTypeExtSig(".zoo", "application/octet-stream" },
            new MimeTypeExtSig(".zsh", "text/x-scriptzsh" }
        */


        #endregion public static MimeTypeExtSig[] MimeSignatureMap

        // public static string DefaultMimeType = DEFAULTMIMETYPE

        /// <summary>
        /// GetMimeType
        /// </summary>
        /// <param name="fileBytes"><see cref="byte[]">byte[] binary array</see></param>
        /// <param name="fileName">save filename</param>
        /// <returns>detected mime type by binary byte pattern, 
        /// if no specific mime type detect => default application/octet-stream</returns>
        public static string GetMimeType(byte[] fileBytes, string fileName)
        {

            string mime = Constants.DEFAULT_MIMETYPE;

            //Ensure that the filename isn't empty or null
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return mime;
            }

            //Get the file extension
            string extension = Path.GetExtension(fileName) == null
                           ? string.Empty
                           : Path.GetExtension(fileName).ToLower();


            var fields = Static.Utils.GetAllFields(typeof(MimeType));
            // foreach (field in fields) {
            foreach (var mimeTypeSig in MimeSignatureMap)
            {
                foreach (var sigByteEntry in mimeTypeSig.SigBytesList)
                {
                    if (fileBytes.Take(mimeTypeSig.SigBytesLen).SequenceEqual(sigByteEntry))
                    {
                        mime = mimeTypeSig.MimeTyp;
                        return mime;
                    }
                }
            }

            return mime;
        }

        /*

            //Get the MIME Type
            if (fileBytes.Take(2).SequenceEqual(BMP))
            {
                mime =  			"image/bmp";
            }
            else if (fileBytes.Take(8).SequenceEqual(DOC))
            {
                mime =  			"application/msword";
            }
            else if (fileBytes.Take(2).SequenceEqual(EXE_DLL))
            {
                mime =  			"application/x-msdownload"; //both use same mime type
            }
            else if (fileBytes.Take(4).SequenceEqual(GIF))
            {
                mime =  			"image/gif";
            }
            else if (fileBytes.Take(4).SequenceEqual(ICO))
            {
                mime =  			"image/x-icon";
            }
            else if (fileBytes.Take(3).SequenceEqual(JPG))
            {
                mime =  			"image/jpeg";
            }
            else if (fileBytes.Take(3).SequenceEqual(MP3))
            {
                mime =  			"audio/mpeg";
            }
            else if (fileBytes.Take(14).SequenceEqual(OGG))
            {
                if (extension == ".OGX")
                {
                    mime =  			"application/ogg";
                }
                else if (extension == ".OGA")
                {
                    mime =  			"audio/ogg";
                }
                else
                {
                    mime =  			"video/ogg";
                }
            }
            else if (fileBytes.Take(7).SequenceEqual(PDF))
            {
                mime =  			"application/pdf";
            }
            else if (fileBytes.Take(16).SequenceEqual(PNG))
            {
                mime =  			"image/png";
            }
            else if (fileBytes.Take(7).SequenceEqual(RAR))
            {
                mime =  			"application/x-rar-compressed";
            }
            else if (fileBytes.Take(3).SequenceEqual(SWF))
            {
                mime =  			"application/x-shockwave-flash";
            }
            else if (fileBytes.Take(4).SequenceEqual(TIFF))
            {
                mime =  			"image/tiff";
            }
            else if (fileBytes.Take(11).SequenceEqual(TORRENT))
            {
                mime =  			"application/x-bittorrent";
            }
            else if (fileBytes.Take(5).SequenceEqual(TTF))
            {
                mime =  			"application/x-font-ttf";
            }
            else if (fileBytes.Take(4).SequenceEqual(WAV_AVI))
            {
                mime = extension == ".AVI" ?  			"video/x-msvideo" :  			"audio/x-wav";
            }
            else if (fileBytes.Take(16).SequenceEqual(WMV_WMA))
            {
                mime = extension == ".WMA" ?  			"audio/x-ms-wma" :  			"video/x-ms-wmv";
            }
            else if (fileBytes.Take(4).SequenceEqual(ZIP_DOCX))
            {
                mime = extension == ".DOCX" ?  			"application/vnd.openxmlformats-officedocument.wordprocessingml.document" :  			"application/x-zip-compressed";
            }

            return mime;
                }
        */

        /// <summary>
        /// GetFileExtForMimeTypeApache
        /// </summary>
        /// <param name="mimeString">Mime type string in format genericType/specificType, e.g.:
        /// image/bmp
        /// image/gif
        /// image/x-icon
        /// image/jpeg
        /// audio/mpeg
        /// audio/ogg
        /// application/msword
        /// </param>
        /// <returns>extension based on windows / dos rules with 3 <see cref="char"/></returns>
        public static string GetFileExtForMimeTypeApache(string mimeString)
        {
            //Ensure that the mimeString isn't empty or null
            if (string.IsNullOrWhiteSpace(mimeString))
            {
                mimeString = "application/octet-stream";
            }


            foreach (var mimeEntry in MimeSignature.MimeSignatureMap)
            {
                string mimeTypeString = mimeEntry.MimeTyp;
                if (mimeString.Equals(mimeTypeString, StringComparison.InvariantCultureIgnoreCase))
                {
                    string fileExtension = mimeEntry.FileExt;
                    Util.Area23Log.Log($"MimeType {mimeString} = {fileExtension.Replace(".", "")}");
                    break;
                }
            }


            for (int i = 0; i < MimeSignature.MimeSignatureMap.Length; i++)
            {
                string mimeTypeString = MimeSignature.MimeSignatureMap[i].MimeTyp;
                if (mimeString.Equals(mimeTypeString, StringComparison.InvariantCultureIgnoreCase))
                {
                    string fileExtension = MimeSignature.MimeSignatureMap[i].FileExt;
                    fileExtension = fileExtension.Replace(".", "");
                    Util.Area23Log.Log($"MimeType {mimeString} = {fileExtension}");
                    return fileExtension;
                }
            }

            return "hex";
        }

        public static bool IsMimeTypeImage(string mimeString)
        {
            //Ensure that the mimeString isn't empty or null
            if (string.IsNullOrWhiteSpace(mimeString))
                mimeString = "application/octet-stream";

            //Get the file extension
            switch (mimeString.ToLower())
            {
                case "image/avif":
                case "image/bmp":
                case "image/gif":
                case "image/ief":
                case "image/jpg":
                case "image/jpeg":
                case "image/png":
                case "image/vnd.adobe.photoshop":
                case "image/svg":
                case "image/svg+xml":
                case "image/tiff":
                case "image/vnd.xiff":
                case "image/x-icon":
                case "image/x-pcx":
                case "image/x-pict":
                case "image/x-rgb":
                case "image/x-xbitmap":
                case "image/x-xpixmap":
                case "image/xcf": return true;
                default: break;
            }
            return false;
        }

        public static bool IsMimeTypeAudio(string mimeString)
        {
            //Ensure that the mimeString isn't empty or null
            if (string.IsNullOrWhiteSpace(mimeString))
                mimeString = "application/octet-stream";

            //Get the file extension
            switch (mimeString.ToLower())
            {
                case "audio/3gpp":
                case "audio/3gpp2":
                case "audio/aac":
                case "audio/aiff":
                case "audio/basic":
                case "audio/midi":
                case "audio/mp4":
                case "audio/mpeg":
                case "audio/ogg":
                case "audio/webm":
                case "audio/x-aiff":
                case "audio/x-mpegurl":
                case "audio/x-wav":
                case "audio/x-ms-wax":
                case "audio/x-ms-wma": return true;
                default: break;
            }
            return false;
        }

        public static bool IsMimeTypeVideo(string mimeString)
        {
            //Ensure that the mimeString isn't empty or null
            if (string.IsNullOrWhiteSpace(mimeString))
                mimeString = "application/octet-stream";

            //Get the file extension
            switch (mimeString.ToLower())
            {
                case "video/3gpp":
                case "video/3gpp2":
                case "video/mp4":
                case "video/mpeg":
                case "video/ogg":
                case "video/quicktime":
                case "video/vnd.mpegurl":
                case "video/webm":
                case "video/x-f4v":
                case "video/x-flv":
                case "video/x-m4v":

                case "video/x-msvideo":
                case "video/x-ms-wmv":
                case "video/x-sgi-movie":
                    return true;
                default: break;
            }
            return false;
        }

        public static bool IsMimeTypeDocument(string mimeString)
        {
            //Ensure that the mimeString isn't empty or null
            if (string.IsNullOrWhiteSpace(mimeString))
                mimeString = "application/octet-stream";

            //Get the file extension
            switch (mimeString.ToLower())
            {
                case "application/msword":
                case "application/pdf":
                case "application/postscript":
                case "application/rtf":
                case "application/vnd.ms-excel":
                case "application/vnd.ms-powerpoint":
                case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                case "application/vnd.oasis.opendocument.presentation":
                case "application/vnd.oasis.opendocument.text":
                case "application/vnd.visio":
                    return true;
                default: break;
            }
            return false;
        }


        /// <summary>
        /// Get Image mime type for image bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetMimeTypeForImageBytes(byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            using (System.Drawing.Image img = System.Drawing.Image.FromStream(ms))
            {
                return ImageCodecInfo.GetImageEncoders().First(codec => codec.FormatID == img.RawFormat.Guid).MimeType;
            }
        }

    }

}