using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.Util
{
    public enum RPNType
    {
        False = 0,
        MathOp1 = 1,
        MathOp2 = 2,
        Number = 3
    }

    public enum RPNRad
    {
        DEG = 0,
        RAD = 1,
        GRAD = 2
    }
}