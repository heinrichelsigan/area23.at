using Area23.At.Framework.Library.Core.Crypt.Cipher.Symmetric;
using System.ComponentModel;

namespace Area23.At.Framework.Library.Core.Crypt.Cipher
{

    [DefaultValue("Aes")]
    public enum CipherEnum : byte
    {
        Aes =       0x00,

        BlowFish =  0x01,
        Fish2 =     0x02,
        Fish3 =     0x03,
        Camellia =  0x04,

        RC532 =     0x5,
        Cast6 =     0x6,
        Gost28147 = 0x07,
        Idea =      0x08,

        Des3 =      0x09,

        Seed =      0x0a,
        SkipJack =  0x0b,
        Serpent =   0x0c,
        Tea =       0x0d,
        XTea =      0x0e,

        ZenMatrix = 0x0f,


        Rijndael =  0x10,
        Noekeon =   0x11,
        RC2 =       0x12,
        Rsa =       0x13,
        Tnepres =   0x14,
        Cast5 =     0x15,
        RC6 =       0x16

    }

}
