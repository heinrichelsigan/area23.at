using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Area23.At.Mono.Util
{
    public class Contact
    {
        public Guid ContactId { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string SecretKey { get; set; }

        public string ImageBase64 { get; set; }
    }
}