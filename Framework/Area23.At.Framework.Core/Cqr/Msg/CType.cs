﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Cqr.Msg
{

    [Serializable]
    public enum CType
    {
        None = 0,
        Json = 1,
        Xml = 2,
        Mime = 3,
        Raw = 4       
    }

    public enum MsgKind
    {
        Server = 0,
        Client = 1
    }
}
