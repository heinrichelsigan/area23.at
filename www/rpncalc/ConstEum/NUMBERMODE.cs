using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace area23.at.mono.rpncalc.ConstEnum
{
    public enum NUMBERMODE
    {
        UNKNOWN     = 0,
        BINARY      = 2,
        OCTAL       = 8,
        DECIMAL     = 10, 
        HEXADECIMAL = 16
    }
    
    public static class NUMBERMODE_Extensions
    {
        public static int GetValue(this NUMBERMODE mode)
        {
            switch(mode)
            {                
                case NUMBERMODE.BINARY: return 2;
                case NUMBERMODE.OCTAL: return 8;
                case NUMBERMODE.DECIMAL: return 10;
                case NUMBERMODE.HEXADECIMAL: return 16;
                default: return 0;
            }
        }
    }
}