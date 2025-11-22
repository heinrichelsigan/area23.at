using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.IO;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace OneLogin.Saml
{

    /// <summary>
    /// Certificate
    /// </summary>
    public class Certificate
    {
        public X509Certificate2 cert;

        public void LoadCertificate(string certificate)
        {
            cert = new X509Certificate2();
            cert.Import(StringToByteArray(certificate));
        }

        public void LoadCertificate(byte[] certificate)
        {
            cert = new X509Certificate2();
            cert.Import(certificate);
        }

        private byte[] StringToByteArray(string st)
        {
            byte[] bytes = new byte[st.Length];
            for (int i = 0; i < st.Length; i++)
            {
                bytes[i] = (byte)st[i];
            }
            return bytes;
        }
    }

}