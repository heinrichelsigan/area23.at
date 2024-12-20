using Org.BouncyCastle.Utilities;
using System.ComponentModel;

namespace Area23.At.Framework.Library.Core.Cipher.Symm
{

    [DefaultValue("Aes")]
    public enum SymmCipherEnum
    {
        Aes         =   0x0,
        
        BlowFish    =   0x1,
        Fish2       =   0x2,
        Fish3       =   0x3,

        Camellia    =   0x4,       
        RC532       =   0x5,    
        Cast6       =   0x6,
        Gost28147   =   0x7,
        Idea        =   0x8,
        
        Des3        =   0x9,

        Seed        =   0xa,        
        SkipJack    =   0xb,
        Serpent     =   0xc,
        Tea         =   0xd,        
        XTea        =   0xe,

        ZenMatrix   =   0xf

    }


}
