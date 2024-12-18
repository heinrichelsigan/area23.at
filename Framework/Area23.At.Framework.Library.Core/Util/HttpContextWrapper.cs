using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Core.Util
{
    /// <summary>
    /// HttpContextWrapper a asp.net <see cref="Current">HttpContext.Current wrapper</see>
    /// <see cref="Microsoft.AspNetCore.Http"/> and <seealso cref="Microsoft.AspNetCore.Http.Extensions"/>
    /// </summary>
    public static class HttpContextWrapper
    {
        public const string ACCEPT_LANGUAGE = "Accept-Language";
        private static IHttpContextAccessor m_httpContextAccessor;
        public static IServiceProvider ServiceProvider;
        private static System.Globalization.CultureInfo locale;
        private static string defaultLang = "en";

        /// <summary>
        /// Current is main accessor in HttpContextWrapper
        /// </summary>
        public static HttpContext Current { get => m_httpContextAccessor.HttpContext; }

        public static Uri RequestUri { get => new Uri(Current.Request.GetDisplayUrl()); }

        /// <summary>
        /// Culture Info from HttpContext.Current.Request.Headers[ACCEPT_LANGUAGE]
        /// </summary>
        public static System.Globalization.CultureInfo Locale
        {
            get
            {
                if (locale == null)
                {
                    try
                    {
                        if (Current.Request != null && Current.Request.Headers != null &&
                            Current.Request.Headers.ContainsKey(ACCEPT_LANGUAGE))
                        {
                            string? firstLang = null;
                            Microsoft.Extensions.Primitives.StringValues strValues;
                            if (Current.Request.Headers.TryGetValue(ACCEPT_LANGUAGE, out strValues))
                                firstLang = strValues.FirstOrDefault();

                            defaultLang = string.IsNullOrEmpty(firstLang) ? "en" : firstLang;
                        }

                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                    catch (Exception)
                    {
                        defaultLang = "en";
                        locale = new System.Globalization.CultureInfo(defaultLang);
                    }
                }

                return locale;
            }
        }

        public static string ISO2Lang { get => Locale.TwoLetterISOLanguageName; }


        /// <summary>
        /// private static constructor in static classes
        /// </summary>
        static HttpContextWrapper() { }

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            m_httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// GetJSession gets a typesafe internally json serialized session variable
        /// </summary>
        /// <typeparam name="T">typesafe type</typeparam>
        /// <param name="key">session state access key</param>
        /// <returns>typesafe nullable type</returns>
        public static T? GetJSession<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "HttpContextWrapper: GetSession(string key), key must be neither null nor string.empty!");
            if (Current.Session == null || Current.Session.Keys == null || Current.Session.Keys.Count() < 1)
                throw new InvalidOperationException($"HttpContextWrapper.Current.Session is null or HttpContextWrapper.Current.Session.Keys is null or empty.");
            if (Current.Session.Keys.Contains(key))
                throw new InvalidOperationException($"HttpContextWrapper.Current.Session.Keys doesn't contain a key with name {key}.");

            string? jSerSez = Current.Session.GetString(key);
            T? typeSafeSession = !string.IsNullOrEmpty(jSerSez) ? JsonConvert.DeserializeObject<T>(jSerSez) : default;

            return typeSafeSession;
        }

        /// <summary>
        /// SetJSession sets a typesafe internally json serialized session variable
        /// </summary>
        /// <typeparam name="T">typesafe since generic templates and inhouse Framewerk > 1.x (dominic cat)</typeparam>
        /// <param name="key">session state access key</param>
        public static void SetJSession<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key", "HttpContextWrapper: TrySetSession(string key, T value), key must be neither null nor string.empty!");

            if (value == null) // we don't need to serialize null values
                Current.Session.SetString(key, null);

            string jSerSez = JsonConvert.SerializeObject(value);
            Current.Session.SetString(key, jSerSez);
        }

    }

}
