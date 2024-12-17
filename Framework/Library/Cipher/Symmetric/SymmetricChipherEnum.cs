using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace Area23.At.Framework.Library.Cipher.Symmetric
{

    [DefaultValue("None")]
    public enum SymmetricCipherEnum
    {
        Aes         =       0x0,

        Camellia    =       0x1,

        // Cast5 =  0x02,
        Cast6       =       0x2,
        
        Gost28147   =       0x3,
        Idea        =       0x4,
        Noekeon     =       0x5,

        // RC2  =   0x09,
        //RC532 =   0x0a,
        RC6         =       0x6,

        Seed        =       0x7,
        Serpent     =       0x8,
        SkipJack    =       0x9,

        Tea         =       0xa,
        // Tnepres= 0x24,
        ThreeFish   =       0xb,
        TripleDes   =       0xc,
        TwoFish     =       0xd,
        
        XTea        =       0xe,
        ZenMatrix   =       0xf        

    }



    public static class SymmCipherMapper
    {      

        public static SymmetricCipherEnum[] KeyBytesToSymmCipherPipeline(byte[] keyBytes)
        {
            int MAXPIPE = 8;
            Dictionary<ushort, SymmetricCipherEnum> symDict = new Dictionary<ushort, SymmetricCipherEnum>();
            List<SymmetricCipherEnum> symmMatrixPipe = new List<SymmetricCipherEnum>();

            ushort scnt = 0;
            foreach (SymmetricCipherEnum symmC in Enum.GetValues(typeof(SymmetricCipherEnum)))
            {
                symDict.Add(scnt++, symmC);
            }

            string hexString = string.Empty;
            for (int kcnt = 0; kcnt < keyBytes.Length && symmMatrixPipe.Count < MAXPIPE; kcnt++)
            {
                hexString = string.Format("{0:x2}", keyBytes[kcnt]);
                sbyte msb = (sbyte)(hexString[0]);
                SymmetricCipherEnum sym0 = symDict[(ushort)msb];
                symmMatrixPipe.Add(sym0);
                sbyte lsb = (sbyte)(hexString[1]);
                SymmetricCipherEnum sym1 = symDict[(ushort)lsb];
                symmMatrixPipe.Add(sym1);
            }

            return symmMatrixPipe.ToArray();
        }

    }

}
