using System.ComponentModel;

namespace Area23.At.Framework.Library.Core.Cipher
{

    [DefaultValue("None")]
    public enum ChipherEnum
    {
        None        =   0x00,

        DES3        =   0x01,
        FISH2       =   0x02,
        FISH3       =   0x03,
        Aes         =   0x04,

        CAST5       =   0x05,
        CAST6       =   0x06,
        Camellia    =   0x07,

        Gost28147   =   0x08,
        IDEA        =   0x09,
        Noekeon     =   0x0a,
        Rijndael    =   0x0b,

        RC2         =   0x0c,
        RC532       =   0x0d,
        RC6         =   0x0e,

        Rsa         =   0x0f,

        SEED        =   0x20,
        SERPENT     =   0x21,
        SkipJack    =   0x22,

        Tea         =   0x23,
        Tnepres     =   0x24,

        XTea        =   0x25,
        ZenMatrix   =   0x26,

        Null        =   0xff

    }

}
