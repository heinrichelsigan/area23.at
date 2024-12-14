using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Net.WebHttp
{
    public static class WebClientRequest
    {
        private static WebClient wclient;
        public static WebClient WClient { get => wclient; }

        private static readonly WebHeaderCollection headers = new WebHeaderCollection();
        public static WebHeaderCollection Headers { get => headers; }

        static WebClientRequest() {

            headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br, zstd");
            headers.Add(HttpRequestHeader.Connection, "keep-alive");
            headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            // TODO:
            headers.Add(HttpRequestHeader.ContentMd5, "");
            headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
            headers.Add(HttpRequestHeader.Host, "area23.at");
            headers.Add(HttpRequestHeader.UserAgent, "cqrxs.eu");

            // wclient.BaseAddress = "https://area23.at/";
            // TODO: always forms credentials
            // webclient.Credentials
            wclient = new WebClient()
            {
                Encoding = Encoding.UTF8,
                Headers = headers
            };

        }

    }


}
