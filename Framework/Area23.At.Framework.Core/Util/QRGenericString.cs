using QRCoder.Core;

namespace Area23.At.Framework.Core.Util
{
    public class QRGenericString : PayloadGenerator.Payload
    {
        private String qrGenericString = string.Empty;
        internal String QrString { get => qrGenericString; set => qrGenericString = value; } 

        public QRGenericString(string qrString = "")  { this.qrGenericString = qrString; }

        public override string ToString() { return qrGenericString; }
    }
}