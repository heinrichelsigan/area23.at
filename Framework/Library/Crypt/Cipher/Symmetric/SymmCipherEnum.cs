using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace Area23.At.Framework.Library.Crypt.Cipher.Symmetric
{

    [DefaultValue("Aes")]
    public enum SymmCipherEnum : byte
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

        ZenMatrix = 0xf

    }

}
