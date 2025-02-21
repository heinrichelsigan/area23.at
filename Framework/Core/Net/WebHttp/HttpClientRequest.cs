using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.Cipher.Symmetric;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.Net.WebHttp
{


    /// <summary>
    /// HttpClientRequest encapsulation
    /// </summary>
    public static class HttpClientRequest
    {

        private static HttpClient httpClientR;
        public static HttpClient HttpClientR { get => httpClientR; }


        static HttpClientRequest()
        {

            // headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br, zstd");
            // httpClientR = new HttpClient();
            // TODO:
            // httpClientR.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            // httpClientR.DefaultRequestHeaders.Add("AcceptLanguage", "en-US");
            // httpClientR.DefaultRequestHeaders.Add("Host", "cqrxs.eu");
            // httpClientR.DefaultRequestHeaders.Add("UserAgent", "cqrxs.eu");
            // wclient.BaseAddress = "https://cqrxs.eu/";
        }

        public static HttpClient GetHttpClient(string baseAddr, string secretKey, string hostName = "cqrxs.eu", System.Text.Encoding? encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            httpClientR = new HttpClient();
            httpClientR.BaseAddress = new Uri(baseAddr);
            httpClientR.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            httpClientR.DefaultRequestHeaders.Add("AcceptLanguage", "en-US");
            httpClientR.DefaultRequestHeaders.Add("Host", hostName);
            httpClientR.DefaultRequestHeaders.Add("UserAgent", "cqrxs.eu");

            if (!string.IsNullOrEmpty(secretKey))
            {
                string hexString = DeEnCoder.KeyToHex(CryptHelper.PrivateUserKey(secretKey));
                httpClientR.DefaultRequestHeaders.Add("Authorization", $"Basic {hexString}");
            }

            return httpClientR;
        }


        public static HttpClient GetHttpClient(string baseAddr, string hostName = "cqrxs.eu", System.Text.Encoding? encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            httpClientR = new HttpClient();
            httpClientR.BaseAddress = new Uri(baseAddr);
            httpClientR.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            httpClientR.DefaultRequestHeaders.Add("AcceptLanguage", "en-US");
            httpClientR.DefaultRequestHeaders.Add("Host", hostName);
            httpClientR.DefaultRequestHeaders.Add("UserAgent", "cqrxs.eu");

            return httpClientR;
        }

        public static IDictionary<string, string> GetHeaders(string hostName = "cqrxs.eu")
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            dict.Add("AcceptLanguage", "en-US");
            dict.Add("Host", hostName);
            dict.Add("UserAgent", "cqrxs.eu");

            return dict;
        }


        public async static Task<HttpResponseMessage> GetClientIp(string url)
        {
            Uri uri = new Uri(url);
            httpClientR = HttpClientRequest.GetHttpClient(url, "cqrxs.eu", Encoding.UTF8);
            return await httpClientR.GetAsync(uri);
        }

        public static bool PostCqrMsg(string url, string msg)
        {
            Uri uri = new Uri(url);
            httpClientR = HttpClientRequest.GetHttpClient(url, "locahost", Encoding.UTF8);

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, uri);
            req.RequestUri = uri;
            req.Content = new StringContent(msg);

            HttpResponseMessage res = httpClientR.Send(req);
            return res.IsSuccessStatusCode;
        }

        public static async Task<HttpResponseMessage> GetClientIPFormArea23()
        {
            string url = "https://cqrxs.eu/net/R.aspx";
            return await GetClientIp(url);
        }


        public static IPAddress? GetClientIP()
        {
            string myIp = GetClientIPFormArea23().Result.ToString();
            if (myIp.Contains("<body>"))
            {
                myIp = myIp.Substring(myIp.IndexOf("<body>"));
                if (myIp.Contains("</body>"))
                    myIp = myIp.Substring(0, myIp.IndexOf("</body>")).Replace("<body>", "").Replace("</body>", "");
            }
            return IPAddress.Parse(myIp);
        }



    }


}
