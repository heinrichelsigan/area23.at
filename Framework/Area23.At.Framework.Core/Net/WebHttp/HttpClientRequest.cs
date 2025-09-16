using Area23.At.Framework.Core.Cqr;
using Area23.At.Framework.Core.Crypt.Cipher;
using Area23.At.Framework.Core.Crypt.EnDeCoding;
using Area23.At.Framework.Core.Net.NameService;
using Area23.At.Framework.Core.Static;
using Area23.At.Framework.Core.Util;
using System.Net;
using System.Text;

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
                string hexString = EnDeCodeHelper.KeyToHex(CryptHelper.PrivateUserKey(secretKey));
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


        public async static Task<HttpResponseMessage> GetClientIpByUrl(string url)
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

        public static async Task<HttpResponseMessage> GetClientBodyFromArea23(bool area23 = false, string urlR = "https://cqrxs.eu/net/R.aspx")
        {
            string url = (area23) ? "https://area23.at/net/R.aspx" : urlR;
            return await GetClientIpByUrl(url);
        }


        public static IPAddress? GetClientIP(string urlR = "https://cqrxs.eu/net/R.aspx")
        {
            string myIp = GetClientBodyFromArea23(true).Result.ToString();
            if (myIp.Contains("<body>"))
            {
                myIp = myIp.Substring(myIp.IndexOf("<body>"));
                if (myIp.Contains("</body>"))
                    myIp = myIp.Substring(0, myIp.IndexOf("</body>")).Replace("<body>", "").Replace("</body>", "");
            }
            IPAddress ipClient = IPAddress.Parse(myIp);
            List<IPAddress> cqrXsEuIpList = DnsHelper.GetIpAddrsByHostName(Constants.CQRXS_EU);
            cqrXsEuIpList.AddRange(DnsHelper.GetIpAddrsByHostName(Constants.AREA23_AT));
            foreach (IPAddress euIp in cqrXsEuIpList)
            {
                if (euIp == null)
                    continue;
                try
                {
                    if (Extensions.BytesCompare(ipClient.GetAddressBytes(), euIp.GetAddressBytes()) == 0)
                        throw new InvalidOperationException($"{ipClient.AddressFamily} {ipClient.Address} equals {euIp.Address}");
                }
                catch (Exception ex)
                {
                    CqrException.SetLastException(ex);
                    Area23Log.LogOriginMsgEx("HttpClientRequest", "Error on getting external client ip", ex);
                    return null;
                }
            }

            return ipClient;
        }



    }


}
