using Org.BouncyCastle.Utilities;
using System.ComponentModel;

namespace Area23.At.Framework.Library.Core.Cipher.Symm
{

    [DefaultValue("Aes")]
    public enum SymmCipherEnum
    {
        Aes     =   0x0,

        Camellia =  0x1,

        Cast6   =   0x2,

        Gost28147 = 0x3,
        Idea    =   0x4,
        Noekeon =   0x5,

        RC6     =   0x6,

        Seed    =   0x7,
        Serpent =   0x8,
        SkipJack =  0x9,

        Tea     =   0xa,        
        ThreeFish = 0xb,
        TripleDes = 0xc,
        TwoFish =   0xd,

        XTea =      0xe,
        ZenMatrix = 0xf

        // Cast5 =  0x02,
        // RC2  =   0x09,
        // RC532 =  0x0a,
        // Tnepres= 0x24,

    }



}
