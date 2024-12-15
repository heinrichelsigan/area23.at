using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Zfx
{
    [DefaultValue(None)]
    public enum ZipType
    {
        None = 0x0,
        Zip = 0x1,
        GZip = 0x2,
        BZip2 = 0x3,
        Z7 = 0x4
    }
}
