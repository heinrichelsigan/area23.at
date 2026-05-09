using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.Controls
{
    public class HeaderLink
    {
        public string DivId { get; set; }
        public string DivCss { get; set; }

        public Uri UUri { get; set; }

        public string UHref { get => UUri.ToString(); }

        public string UTitle { get; set; }

        public string UId { get => "uH" + DivId.Substring(1); }
    }
}