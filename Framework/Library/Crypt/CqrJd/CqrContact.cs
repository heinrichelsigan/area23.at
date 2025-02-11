using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Crypt.CqrJd
{

    /// <summary>
    /// CqrContact is a contact for CqrJd
    /// </summary>
    public class CqrContact
    {
        public int ContactId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string SecretKey { get; set; }

        public string NameEmail { get => Name + ((string.IsNullOrEmpty(Email)) ? string.Empty : ("<" + Email + ">")); }


        public string ImageBase64 { get; set; }


        public CqrContact()
        {
        }

        public CqrContact(int contactId, string name, string email, string mobile, string address)
        {
            this.ContactId = contactId;
            this.Name = name;
            this.Email = email;
            this.Mobile = mobile;
            this.Address = address;
        }

        public virtual string ToJson()
        {
            CqrContact cqrContact = new CqrContact(ContactId, Name, Email, Mobile, Address);
            string jsonString = JsonConvert.SerializeObject(cqrContact, Formatting.Indented);
            return jsonString;
        }

        public virtual CqrContact FromJson(string jsonText)
        {
            CqrContact cqrContactJson;
            try
            {
                cqrContactJson = JsonConvert.DeserializeObject<CqrContact>(jsonText);
                if (cqrContactJson != null && cqrContactJson.ContactId > -1 && !string.IsNullOrEmpty(cqrContactJson?.Name))
                {
                    this.ContactId = cqrContactJson.ContactId;
                    this.Name = cqrContactJson.Name;
                    this.Email = cqrContactJson.Email;
                    this.Mobile = cqrContactJson.Mobile;
                    this.Address = cqrContactJson.Address;
                    // this.ImageBase64 = cqrContactJson.ImageBase64;
                    return cqrContactJson;
                }
            }
            catch (Exception exJson)
            {
                Area23Log.LogStatic(exJson);
            }

            return null;
        }

        public override string ToString()
        {
            return (
                "NameEmail: " + this.NameEmail + ";" + Environment.NewLine +
                "ContactId: " + this.ContactId + ";" + Environment.NewLine +
                "Name: " + this.Name + ";" + Environment.NewLine +
                "Email: " + this.Email + ";" + Environment.NewLine +
                "Mobile: " + this.Mobile + ";" + Environment.NewLine +
                "Address: " + this.Address + ";" + Environment.NewLine +
                this.ImageBase64 + Environment.NewLine
                );
        }

        /// <summary>
        /// <see cref="object[]">RowParams</see> gets an object array of row parameters to show in <see cref="System.Windows.Forms.DataGridView"/>
        /// </summary>
        public object[] GetRowParams()
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
