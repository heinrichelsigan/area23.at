using Area23.At.Framework.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Area23.At.Framework.Library.Zfx
{

    [DefaultValue(None)]
    public enum ZipType
    {
        None =  0x00,
        Zip =   0x10,
        GZip =  0x20,
        BZip2 = 0x30,
        Z7 =    0x40
    }


    public static class ZipTypeExtensions
    {
       
        public static ZipType[] GetZipTypes()
        {
            List<ZipType> list = new List<ZipType>();
            foreach (string encName in Enum.GetNames(typeof(ZipType)))
            {
                list.Add((ZipType)Enum.Parse(typeof(ZipType), encName));
            }

            return list.ToArray();
        }

        public static ZipType GetZipType(string zipTypeStr)
        {
            if (!string.IsNullOrEmpty(zipTypeStr))
            {
                switch (zipTypeStr.ToLower().Replace("menu", ""))
                {
                    case "zip": return ZipType.Zip;
                    case "gzip": return ZipType.GZip;
                    case "bzip2": return ZipType.BZip2;
                    case "7z": return ZipType.Z7;
                    case "none":
                    default: break;
                }
            }
            return ZipType.None;
        }

        public static ZipType GetZipTypeFromValue(short zValue)
        {
            zValue = (short)((zValue % 0x100) - (zValue % 0x10));
            foreach (ZipType zType in GetZipTypes())
            {
                if ((short)zType == zValue)
                    return zType;
            }
            return ZipType.None;
        }

        /// <summary>
        /// Generic zip extension method for <see cref="ZipType"/>
        /// </summary>
        /// <param name="inBytes"></param>
        /// <returns>zipped bytes</returns>
        public static byte[] Zip(this ZipType zipType, byte[] inBytes)
        {
            if (inBytes == null || inBytes.Length == 0)
                throw new InvalidOperationException("byte[] Zip(this ZipType zipType, byte[] inBytes = NULL)");

            switch (zipType)
            {
                case ZipType.BZip2:
                    return BZip2.BZip(inBytes);
                case ZipType.GZip:
                    return GZ.GZipBytes(inBytes);
                case ZipType.Zip:
                    return WinZip.Zip(inBytes);
                case ZipType.Z7: // TODO
                case ZipType.None:
                    return inBytes;
                default: // Asset(0)
                    break;
            }

            return new byte[0];
        }

        /// <summary>
        /// Generic unzip extension method for <see cref="ZipType"/>
        /// </summary>
        /// <param name="zipType">this the <see cref="ZipType"/>/param>
        /// <param name="compressedBytes"></param>
        /// <returns>decompressed bytes</returns>
        public static byte[] Unzip(this ZipType zipType, byte[] compressedBytes)
        {
            if (compressedBytes == null || compressedBytes.Length == 0)
                throw new InvalidOperationException("byte[] Unzip(this ZipType zipType, byte[] compressedBytes = NULL)");

            switch (zipType)
            {
                case ZipType.BZip2:
                    return BZip2.BUnZip(compressedBytes);
                case ZipType.GZip:
                    return GZ.GUnZipBytes(compressedBytes);
                case ZipType.Zip:
                    return WinZip.UnZip(compressedBytes);
                case ZipType.Z7: // TODO
                case ZipType.None:
                    return compressedBytes;
                default: // Asset(0)
                    break;
            }

            return new byte[0];
        }



        /// <summary>
        /// ZipFileExtension returns file extension
        /// </summary>
        /// <param name="zipt">this ZipType zipt</param>
        /// <param name="pipeString">pipe string <see cref="Crypt.Cipher.CipherPipe.PipeString"/></param>
        /// <returns>zip file extension for windoes & unix</returns>
        public static string ZipFileExtension(this ZipType zipt, string pipeString = "")
        {
            string extPre = string.IsNullOrEmpty(pipeString) ? "" : "." + pipeString;
            switch (zipt)
            {
                case ZipType.GZip: return string.Format(".gz{0}", extPre);
                case ZipType.BZip2: return string.Format("bz2{0}", extPre);
                case ZipType.Zip: return string.Format("zip{0}", extPre);
                case ZipType.Z7: return string.Format("7z{0}", extPre);
                case ZipType.None:
                default: return extPre;
            }
        }

    }
}
