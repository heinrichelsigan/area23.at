using Area23.At.Framework.Library.Cqr;
using Area23.At.Framework.Library.Cqr.Msg;
using System.Net;

namespace Area23.At.Framework.Core.Net.WebHttp
{

    /// <summary>
    /// WebServiceSoap implements a static WebServiceSoap Request via <see cref="CqrService"/>
    /// and maily provides
    /// <see cref="ExternalClientIpFromServer()"
    /// <see cref="ExternalClientIpv6FromServer()"/>
    /// funtionality.
    /// </summary>
    public static class WebServiceSoap
    {
        
        /// <summary>
        /// static constructor
        /// </summary>
        static WebServiceSoap()
        { 
        }


        /// <summary>
        /// ExternalClientIpFromServer gets external network ip for client from server
        /// </summary>
        /// <returns>external official gateway <see cref="IPAddress">ip address</see> of client</returns>
        public static IPAddress ExternalClientIpFromServer()
        {
            CqrService client = new CqrService();            
            string resp = client.GetIPAddress();
            return IPAddress.Parse(resp);
        }

        /// <summary>
        /// ExternalClientIpv6FromServer gets external network ip for client from server
        /// </summary>
        /// <returns>external official gateway <see cref="IPAddress">ip address</see> of client</returns>
        public static IPAddress ExternalClientIpv6FromServer()
        {
            CqrService client = new CqrService();
            string resp = client.GetIPAddress();
            return IPAddress.Parse(resp);
        }


    }

}
