using Area23.At.Framework.Library.Crypt.Cipher.Symmetric;
using System.ComponentModel;

namespace Area23.At.Framework.Library.Crypt.Cipher
{

    [DefaultValue("Aes")]
    public enum CipherEnum : byte
    {
        Aes =       0x0,

        BlowFish =  0x1,
        Camellia =  0x2,
        Cast6 =     0x3,
        Des3 =      0x4,
        Fish2 =     0x5,
        Fish3 =     0x6,
        Gost28147 = 0x7,

        Idea =      0x8,
        RC532 =     0x9,
        Seed =      0xa,
        SkipJack =  0xb,
        Serpent =   0xc,
        Tea =       0xd,
        XTea =      0xe,

        ZenMatrix = 0xf,


        Cast5 =     0x10,
        Noekeon =   0x11,
        RC2 =       0x12,
        RC564 =     0x13,
        RC6 =       0x14,
        Tnepres =   0x15,
        
        Rsa =       0x40
    }

}
