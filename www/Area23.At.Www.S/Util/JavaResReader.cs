using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web;
using System.Xml;
using Area23.At.Www.S;
using Area23.At.Www.S.Util;
using Area23.At.Www.S.Properties;

namespace Area23.At.Www.S.Util
{
    public static class JavaResReader
    {
        public static string GetValue(string key, string langCode = "")
        {
            string retVal = Properties.Resource.ResourceManager.GetString(key);
            return (string.IsNullOrEmpty(retVal)) ? key : retVal;
        }
    }
}