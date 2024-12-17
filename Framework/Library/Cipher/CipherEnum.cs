using System.ComponentModel;

namespace Area23.At.Framework.Library.Cipher
{

    [DefaultValue("None")]
    public enum CipherEnum
    {
        None    =   0x00,

        DES3    =   0x01,
        FISH2   =   0x02,
        FISH3   =   0x03,
        AES     =   0x04,

        Cast5   =   0x05,
        Cast6   =   0x06,
        Camellia =  0x07,

        Gost28147 = 0x08,
        Idea    =   0x09,
        Noekeon =   0x0a,
        Rijndael =  0x0b,

        RC2     =   0x0c,
        RC532   =   0x0d,
        RC6     =   0x0e,

        Rsa     =   0x0f,

        Seed    =   0x20,
        Serpent =   0x21,
        SkipJack =  0x22,

        Tea     =   0x23,
        Tnepres =   0x24,

        XTea    =   0x25,
        ZenMatrix = 0x26

    }

}
