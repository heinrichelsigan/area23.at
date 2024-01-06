using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace area23.at.mono.rpncalc.ConstEnum
{
    public enum NUMMODE
    {
        UNKNOWN = 0,
        BINARY = 2,
        OCTAL = 8,
        DECIMAL = 10,
        HEXADECIMAL = 16
    }
    
    public static class NUMMODE_Extensions
    {
        public static int GetValue(this NUMMODE mode)
        {
            switch (mode)
            {
                case NUMMODE.BINARY: return 2;
                case NUMMODE.OCTAL: return 8;
                case NUMMODE.DECIMAL: return 10;
                case NUMMODE.HEXADECIMAL: return 16;
                default: break;
            }
            return 0;
        }
    }
}