using Area23.At.Framework.Library.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;

namespace Area23.At.Framework.Library.Static
{

    /// <summary>
    /// Static Utils, thanks to
    /// <see cref="https://newtonsoft.com/json/">Newtonsoft Json.NET</seealso>
    /// <see cref="https://stackoverflow.com/">Stackoverflow</see>
    /// <see cref="https://stackoverflow.com/questions/4123590/serialize-an-object-to-xml">xml serializer thread on stackoverflow</see>/
    /// <seeslo cref="https://gist.github.com/riyadparvez/4467678">gist.github.com/riyadparvez</seealso> for generic reflection

    /// </summary>
    public static class Utils
    {

        /// <summary>
        /// SwapT a generic swapper
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t0">refernce in t0, reference out t1</param>
        /// <param name="t1">efernce in t1, reference out t0</param>
        /// <returns>an array with 2 elements with original positions</returns>
        public static T[] SwapT<T>(ref T t0, ref T t1)
        {
            T[] tt = new T[2];
            tt[0] = t0;
            tt[1] = t1;
            t0 = tt[1];
            t1 = tt[0];

            return tt;
        }


        /// <summary>
        /// Generic null setter for an array of objects
        /// </summary>
        /// <param name="os"></param>
        /// <returns></returns>
        public static bool SetNull(params object[] os)
        {
            if (os == null || os.Length == 0)
                return true;

            bool error = false;
            for (int i = 0; i < os.Length; i++)
            {
                object o = os[i];
                try
                {
                    if (o != null)
                        o = null;
                }
                catch (Exception exNull)
                {
                    error = true;
                    Area23Log.Log($"Error in Ext SetNull(params object[] os): {o} {exNull.Message} ...");
                }
            }

            return !error;
        }

        /// <summary>
        /// generic null setter for an array of T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool SetNullT<T>(params T[] ts) where T : class
        {
            bool error = false;
            if (ts == null || ts.Length == 0)
                return true;

            for (int it = 0; it < ts.Length; it++)
            {
                T t = ts[it];
                try
                {
                    if (t != null)
                        t = null;
                }
                catch (Exception exNull)
                {
                    error = true;
                    Area23Log.Log($"Error in Ext SetNullT<T>(params T[] ts) {t.ToString()} {exNull.Message} ....");
                }
            }

            return !error;

        }


        /// <summary>
        /// SerializeToXml gemeric to xml serialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">object to serialize</param>
        /// <returns>xml serialized string</returns>
        public static string SerializeToXml<T>(T obj)
        {
            string xml = string.Empty;
            try
            {
                StringWriter writer = new StringWriter();
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);
                xml = writer.ToString();
            }
            catch (Exception exSerialize)
            {
                Area23Log.Log($"Exception {exSerialize.GetType()} in static byte[]? SerializeToXml<T = {obj.GetType()}>(T obj, out serialized)  {exSerialize.Message}\n");
                Area23Log.Log(exSerialize);
            }

            return xml;

        }


        /// <summary>
        /// DeserializeFromXml generic T from xml deserializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml">sml serialized string</param>
        /// <returns>generic T</returns>
        public static T DeserializeFromXml<T>(string xml)
        {
            T result = default;
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (var tr = new StringReader(xml))
                {
                    result = (T)ser.Deserialize(tr);
                }
            }
            catch (Exception exDeserialize)
            {
                Area23Log.Log($"Exception {exDeserialize.GetType()} in static T? ({result.GetType()}) DeserializeFromXml<T = {result.GetType()}>(string xml) {exDeserialize.Message}\n");
                Area23Log.Log(exDeserialize);
            }

            return result;
        }

        /// <summary>
        /// SerializeToJsonl gemeric to json serialize
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">object to serialize</param>
        /// <returns>json serialized string</returns>
        public static string SerializeToJsonl<T>(T t) => Newtonsoft.Json.JsonConvert.SerializeObject(t);

        /// <summary>
        /// DeserializeFromJson generic deserialize a json serialized string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">json serialized string</param>
        /// <returns>generic object T</returns>
        public static T DeserializeFromJson<T>(string json) => Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);



        /// <summary>
        /// Get all the fields of a class
        /// </summary>
        /// <param name="type">Type object of that class</param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> GetAllFields(this Type type)
        {
            if (type == null)
            {
                return Enumerable.Empty<FieldInfo>();
            }

            BindingFlags flags = BindingFlags.Public |
                                 // BindingFlags.NonPublic |
                                 BindingFlags.Static |
                                 BindingFlags.Instance |
                                 BindingFlags.DeclaredOnly;

            return type.GetFields(flags).Union(type.BaseType.GetAllFields());
        }

        /// <summary>
        /// Get all properties of a class
        /// </summary>
        /// <param name="type">Type object of that class</param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            if (type == null)
            {
                return Enumerable.Empty<PropertyInfo>();
            }

            BindingFlags flags = BindingFlags.Public |
                                 // BindingFlags.NonPublic |
                                 BindingFlags.Static |
                                 BindingFlags.Instance |
                                 BindingFlags.DeclaredOnly;

            return type.GetProperties(flags).Union(type.BaseType.GetAllProperties());
        }


        public static bool DenyExtensionInOut(string url = "")
        {
            if (string.IsNullOrEmpty(url))
                url = HttpContext.Current.Request.Url.ToString().ToLower();
            else
                url = url.ToLower();

            foreach (string deniedExt in Constants.DENIED_EXTENSIONS)
            {
                if (url.EndsWith(deniedExt, StringComparison.InvariantCultureIgnoreCase))
                    return true;
                string restUrl = url.Substring(url.LastIndexOf("."));
                if (restUrl.EndsWith(deniedExt) || restUrl.Contains(deniedExt))
                    return true;
            }

            return false;
        }

        public static bool AllowExtensionInOut(string url = "")
        {
            bool allow = false;
            if (string.IsNullOrEmpty(url))
                url = HttpContext.Current.Request.Url.ToString().ToLower();
            else
                url = url.ToLower();

            foreach (string deniedExt in Constants.DENIED_EXTENSIONS)
            {
                if (url.EndsWith(deniedExt, StringComparison.InvariantCultureIgnoreCase))
                    return false;
            }

            foreach (string allowedExt in Constants.ALLOWED_EXTENSIONS)
            {
                if ((allow = url.EndsWith(allowedExt)))
                    break;

                if (url.LastIndexOf(".") > -1)
                {
                    string restUrl = url.Substring(url.LastIndexOf("."));
                    if ((allow = (restUrl.EndsWith(allowedExt) || restUrl.Contains(allowedExt))))
                        break;
                }
            }

            return allow;

        }

    }



    /// <summary>
    /// Static class alternative for System.Drawing.Color Extension Methods
    /// </summary>
    public static class ColorFrom
    {
        #region ColorFrom static methods

        /// <summary>
        /// FromHtml gets color from hexadecimal rgb string html standard
        /// static method Supu.Framework.Extensions.ColorFrom.FromHtml(string hex) 
        /// is an alternative to System.Drawing.Color.FromHtml(string hex) extension method
        /// </summary>
        /// <param name="hex">hexadecimal rgb string with starting #</param>
        /// <returns>Color, that was defined by hexadecimal html standarized #rrggbb string</returns>
        public static System.Drawing.Color FromHtml(string hex)
        {

            if (String.IsNullOrWhiteSpace(hex) || hex.Length != 7 || !hex.StartsWith("#"))
                throw new ArgumentException(
                    String.Format("Area23.At.Framework.Library.ColorForm.FromHtml(string hex = {0}), hex must be an rgb string in format \"#rrggbb\" like \"#3f230e\"!", hex));

            System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml(hex);
            return _color;
        }

        /// <summary>
        /// FromXrgb gets color from hexadecimal rgb string
        /// static method Supu.Framework.Extensions.ColorFrom.FromXrgb(string hex) 
        /// is an alternative to System.Drawing.Color.FromXrgb(string hex) extension method
        /// </summary>
        /// <param name="hex">hexadecimal rgb string with starting #</param>
        /// <returns>Color, that was defined by hexadecimal rgb string</returns>
        public static System.Drawing.Color FromXrgb(string hex)
        {
            if (String.IsNullOrWhiteSpace(hex) || hex.Length != 7 || !hex.StartsWith("#"))
                throw new ArgumentException(
                    String.Format("Area23.At.Framework.Library.ColorForm.FromXrgb(string hex = {0}), hex must be an rgb string in format \"#rrggbb\" like \"#3f230e\"!", hex));

            string rgbWork = hex.TrimStart("#".ToCharArray());
            string colSeg = rgbWork.Substring(0, 2);
            colSeg = (colSeg.Contains("00")) ? "0" : colSeg.TrimStart("0".ToCharArray());
            int r = Convert.ToUInt16(colSeg, 16);
            colSeg = rgbWork.Substring(2, 2);
            colSeg = (colSeg.Contains("00")) ? "0" : colSeg.TrimStart("0".ToCharArray());
            int g = Convert.ToUInt16(colSeg, 16);
            colSeg = rgbWork.Substring(4, 2);
            colSeg = (colSeg.Contains("00")) ? "0" : colSeg.TrimStart("0".ToCharArray());
            int b = Convert.ToUInt16(colSeg, 16);

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        #endregion ColorFrom static methods

    }

}
