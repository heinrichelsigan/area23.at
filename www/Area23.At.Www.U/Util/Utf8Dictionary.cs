using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Area23.At.Www.U.Util
{
    public class Utf8Dictionary : Dictionary<long, Utf8Symbol>
    {
        public const long StartUtf8 = 32;
        public const long EndUtf8 = 65536 * 4;
        private static readonly Lazy<Utf8Dictionary> instance = new Lazy<Utf8Dictionary>(() => new Utf8Dictionary(true));
        public static Utf8Dictionary Uft8DictSingle => instance.Value;

        public Utf8Dictionary() : base() { }

        public Utf8Dictionary(bool init) : this()
        {
            if (init)
            {
                for (long l0 = StartUtf8; l0 < 16384; l0++)
                {
                    this.Add(l0, new Utf8Symbol(l0));
                }

                for (long l1 = 118784; l1 < 122880; l1++)
                {
                    this.Add(l1, new Utf8Symbol(l1));
                }

                for (long l2 = 126976; l2 < 130047; l2++)
                {
                    this.Add(l2, new Utf8Symbol(l2));
                }
            }
        }
    }
}