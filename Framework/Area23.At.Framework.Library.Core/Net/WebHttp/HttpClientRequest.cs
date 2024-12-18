using Area23.At.Framework.Library.Core.Cipher.Symm;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Net.WebHttp
{
    public static class HttpClientRequest
    {
        private static HttpClient httpClientR;
        public static HttpClient HttpClientR { get => httpClientR; }


        static HttpClientRequest() {

            // headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br, zstd");
            // httpClientR = new HttpClient();
            // TODO:
            // httpClientR.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            // httpClientR.DefaultRequestHeaders.Add("AcceptLanguage", "en-US");
            // httpClientR.DefaultRequestHeaders.Add("Host", "area23.at");
            // httpClientR.DefaultRequestHeaders.Add("UserAgent", "cqrxs.eu");
            // wclient.BaseAddress = "https://area23.at/";
        }

        public static HttpClient GetHttpClient(string baseAddr, string secretKey, string keyIv = "", System.Text.Encoding? encoding = null)        
        {            
            encoding = encoding ?? Encoding.UTF8;

            httpClientR = new HttpClient();           
            httpClientR.BaseAddress = new Uri(baseAddr);
            httpClientR.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            httpClientR.DefaultRequestHeaders.Add("AcceptLanguage", "en-US");
            httpClientR.DefaultRequestHeaders.Add("Host", "area23.at");
            httpClientR.DefaultRequestHeaders.Add("UserAgent", "cqrxs.eu");

            if (!string.IsNullOrEmpty(secretKey))
            {
                string hexString = EnDeCoding.DeEnCoder.KeyHexString(CryptHelper.PrivateUserKey(secretKey));
                if (!string.IsNullOrEmpty(keyIv))
                {
                    hexString = EnDeCoding.DeEnCoder.KeyHexString(CryptHelper.PrivateKeyWithUserHash(secretKey, keyIv));
                }
                httpClientR.DefaultRequestHeaders.Add("Authorization", $"Basic {hexString}");
            }

            return httpClientR;
        }


        public static HttpClient GetHttpClient(string baseAddr, System.Text.Encoding? encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;

            httpClientR = new HttpClient();
            httpClientR.BaseAddress = new Uri(baseAddr);
            httpClientR.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            httpClientR.DefaultRequestHeaders.Add("AcceptLanguage", "en-US");
            httpClientR.DefaultRequestHeaders.Add("Host", "area23.at");
            httpClientR.DefaultRequestHeaders.Add("UserAgent", "cqrxs.eu");

            return httpClientR;
        }

        public static IDictionary<string, string> GetContent()
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            dict.Add("AcceptLanguage", "en-US");
            dict.Add("Host", "area23.at");
            dict.Add("UserAgent", "cqrxs.eu"); 


            return dict;
        }


        public async static Task<HttpResponseMessage> GetClientIp(string url)
        {
            Uri uri = new Uri(url);
            httpClientR = HttpClientRequest.GetHttpClient(url, Encoding.UTF8);
            return await httpClientR.GetAsync(uri);
        }

        public static async Task<HttpResponseMessage> GetClientIPFormArea23()
        {
            IPAddress? myAddr;
            string url = "https://area23.at/net/R.aspx";
            return await GetClientIp(url);
            
        }


        public static IPAddress? GetClientIP()
        {
            string myIp = GetClientIPFormArea23().Result.ToString();
            if (myIp.Contains("<body>"))
            {
                myIp.Substring(myIp.IndexOf("<body>"), myIp.LastIndexOf("</body>"));
                if (myIp.Contains(">") && myIp.Contains("<"))
                    myIp.Substring(myIp.IndexOf(">"), myIp.LastIndexOf("<"));
            }
            return IPAddress.Parse(myIp);
        }
        


    }


}
