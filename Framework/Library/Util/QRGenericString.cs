using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Util;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area23.At.Framework.Library.Util
{
    public class QRGenericString : PayloadGenerator.Payload
    {
        private String qrGenericString = string.Empty;
        internal String QrString { get => qrGenericString; set => qrGenericString = value; } 

        public QRGenericString(string qrString = "")  { this.qrGenericString = qrString; }

        public override string ToString() { return qrGenericString; }
    }
}