using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core
{
    internal static class HttpContextWrapper
    {
        public static IServiceProvider ServiceProvider;

        static HttpContextWrapper()
        { }


        private static Microsoft.AspNetCore.Http.IHttpContextAccessor m_httpContextAccessor;


        public static void Configure(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            m_httpContextAccessor = httpContextAccessor;
        }


        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get
            {
                return m_httpContextAccessor.HttpContext;
            }
        }
    }
}
