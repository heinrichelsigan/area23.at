using Area23.At.Framework.Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using System.IO;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace OneLogin.Saml
{

    public class AuthRequest
    {
        public string id;
        private string issue_instant;
        private AppSettings appSettings;
        private AccountSettings accountSettings;

        public enum AuthRequestFormat
        {
            Base64 = 1
        }

        public AuthRequest(AppSettings appSettings, AccountSettings accountSettings)
        {
            this.appSettings = appSettings;
            this.accountSettings = accountSettings;

            id = "_" + System.Guid.NewGuid().ToString();
            issue_instant = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        }

        public string GetRequest(AuthRequestFormat format)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;

                using (XmlWriter xw = XmlWriter.Create(sw, xws))
                {
                    xw.WriteStartElement("samlp", "AuthnRequest", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("ID", id);
                    xw.WriteAttributeString("Version", "2.0");
                    xw.WriteAttributeString("IssueInstant", issue_instant);
                    xw.WriteAttributeString("ProtocolBinding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST");
                    xw.WriteAttributeString("AssertionConsumerServiceURL", appSettings.assertionConsumerServiceUrl);

                    xw.WriteStartElement("saml", "Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                    xw.WriteString(appSettings.issuer);
                    xw.WriteEndElement();

                    xw.WriteStartElement("samlp", "NameIDPolicy", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("Format", "urn:oasis:names:tc:SAML:2.0:nameid-format:unspecified");
                    xw.WriteAttributeString("AllowCreate", "true");
                    xw.WriteEndElement();

                    xw.WriteStartElement("samlp", "RequestedAuthnContext", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("Comparison", "exact");

                    xw.WriteStartElement("saml", "AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion");
                    xw.WriteString("urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport");
                    xw.WriteEndElement();

                    xw.WriteEndElement(); // RequestedAuthnContext

                    xw.WriteEndElement();
                }

                if (format == AuthRequestFormat.Base64)
                {
                    byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(sw.ToString());
                    return System.Convert.ToBase64String(toEncodeAsBytes);
                }

                return null;
            }
        }
    }
}