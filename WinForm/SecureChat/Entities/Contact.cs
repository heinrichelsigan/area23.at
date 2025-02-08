using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.WinForm.SecureChat.Entities
{

    /// <summary>
    /// Represents a contact entry
    /// </summary>
    public class Contact
    {
        public int ContactId { get; set; }
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Mobile { get; set; }

        public string? Address { get; set; }

        public string? SecretKey { get; set; }

        public string? NameEmail { get => Name + ((string.IsNullOrEmpty(Email)) ? string.Empty : ("<" + Email + ">")); }


        public string? ImageBase64 { get; set; }

        /// <summary>
        /// <see cref="object[]">RowParams</see> gets an object array of row parameters to show in <see cref="System.Windows.Forms.DataGridView"/>
        /// </summary>
        public object[] RowParams
        {
            get
            {
                List<object> oList = new List<object>();
                oList.Add(ContactId);
                oList.Add(Name);
                oList.Add(Email);
                oList.Add(Mobile);
                oList.Add(Address);
                return oList.ToArray();
            }
        }

    }

}
