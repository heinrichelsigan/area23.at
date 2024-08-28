using Area23.At.Framework.Library;
using Area23.At.Www.U;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Area23.At.Www.U.Util
{
    public class Utf8Symbol
    {
        const char AMP = '&';
        const char HASH = '#';
        const char SEMICOLON = ';';
        const string AMP_HTML = "&amp;";
        const string HASH_HTML = "&#35;";

        public char[] Symbol { get => System.Text.UTF8Encoding.UTF8.GetChars(Bytes, 0, Bytes.Length); }

        public string SymbolString { get => System.Text.UTF8Encoding.UTF8.GetString(Bytes, 0, Bytes.Length); }

        public string Name { get; set; } 

        public long Utf8L { get; set; }

        public int Utf8Int { get => (int)Utf8L; }

        public byte[] Bytes { get; set; }
        // public byte[] Bytes { get => Utf8I

        /// <summary>
        /// HtmlCode
        /// gets html code of Utf8 symbol $"&#{Uft8Int:d};"
        /// </summary>
        public string HtmlCode { get => String.Format("&#{0:d};", Utf8Int); }   // $"&#{Uft8Int:d};" 

        /// <summary>
        /// HtmlEncoded
        /// gets html code hmtl encoded $"&amp;&#35;{Uft8Int:d};"
        /// </summary>
        public string HtmlEncoded { get => String.Format("{0}{1}{2:d}{3}", AMP_HTML, HASH_HTML, Utf8Int, SEMICOLON); } // "&amp;&#35;{0:d};" 

        /// <summary>
        /// HexCode
        /// gets html hex code $"&#x{Uft8Int:x};"
        /// </summary>
        public string HexCode { get => String.Format("{0}{1}x{2:x}{3}", AMP, HASH, Utf8Int, SEMICOLON); }  // $"&#x{Uft8Int:x};"

        /// <summary>
        /// HexHtmlEncoded 
        /// gets html hex code hmtl encoded $"&amp;&#35;x{Uft8Int:x};"
        /// </summary>
        public string HexHtmlEncoded { get => String.Format("{0}{1}x{2:x}{3}", AMP_HTML, HASH_HTML, Utf8Int, SEMICOLON); } // $"&amp;&#35;x{Uft8Int:x};"

        public Utf8Symbol(long utf8l)
        {
            Utf8L = utf8l;
            this.Name = Utf8L.ToString();
            if (Utf8L < 256)
            {
                Bytes = new Byte[1];
                Bytes[0] = (byte)Utf8L;
                return;
            }
            String x = String.Format("{0:x}", Utf8L);
            if (x.Length % 2 == 1)
                x = "0" + x;            
            Bytes = new byte[x.Length / 2];
            for (int b = 0; b < x.Length; b += 2)
            {
                try
                {
                    String bs = x.Substring(b, 2);
                    Byte by = Convert.ToByte(bs, 16);
                    Bytes[b / 2] = by;    // Convert.ToByte(x.Substring(b, 2));
                }
                catch (Exception ex)
                {
                    Area23Log.LogStatic(ex);
                    // throw; 
                }
            }
            
        }
    }
}