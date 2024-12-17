using Area23.At.Framework.Library.Core.Cipher.Symm;
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

            // headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br, zstd");
            
            headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            // TODO:
            // headers.Add(HttpRequestHeader.ContentMd5, "");
            // headers.Add(HttpRequestHeader.From, "true");
            // removed because make trouble in that very .Net1.1 version of WebClient abstraction
            // headers.Add(HttpRequestHeader.KeepAlive, "true");
            // headers.Add(HttpRequestHeader.Connection, "keep-alive");
            headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
            headers.Add(HttpRequestHeader.Host, "area23.at");
            headers.Add(HttpRequestHeader.UserAgent, "cqrxs.eu");            
            // wclient.BaseAddress = "https://area23.at/";
            // TODO: always forms credentials
            // webclient.Credentials
            

        }

        public static WebClient GetWebClient(string baseAddr, string secretKey, string keyIv = "", System.Text.Encoding? encoding = null)        
        {
            encoding = encoding ?? Encoding.UTF8;
            wclient = new WebClient();
            wclient.Encoding = encoding;
            if (!string.IsNullOrEmpty(secretKey))
            {
                string hexString = EnDeCoding.DeEnCoder.KeyHexString(CryptHelper.PrivateUserKey(secretKey));
                if (!string.IsNullOrEmpty(keyIv))
                {
                    hexString = EnDeCoding.DeEnCoder.KeyHexString(CryptHelper.PrivateKeyWithUserHash(secretKey, keyIv));
                }
                headers.Add(HttpRequestHeader.Authorization, "Basic " + hexString);
            }
            wclient.Headers = headers;
            wclient.BaseAddress = baseAddr; ;

            return wclient;
        }


        public static WebClient GetWebClient(string baseAddr, System.Text.Encoding? encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            wclient = new WebClient();
            wclient.Encoding = encoding;
            wclient.Headers = headers;
            wclient.BaseAddress = baseAddr; ;

            return wclient;
        }


    }


}
