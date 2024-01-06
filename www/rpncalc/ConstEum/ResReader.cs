using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Xml;
using area23.at.mono.rpncalc;
using area23.at.mono.rpncalc.ConstEnum;
using area23.at.mono.rpncalc.Properties;

namespace area23.at.mono.rpncalc.ConstEnum
{
    public static class ResReader
    {
        public static string GetValueFromKey(string key, string langCode = "")
        {
            string retVal = Properties.Resource.ResourceManager.GetString(key);
            
            return (!string.IsNullOrEmpty(retVal)) ? retVal : key;            
        }

    }
}