using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Library.Crypt.CqrJd
{


    /// <summary>
    /// CqrContact is a contact for CqrJd
    /// </summary>
    [DataContract(Name = "CqrContact")]
    [Description("cqrxs.eu contact")]
    public class CqrContact
    {

        #region properties

        public int ContactId { get; set; }

        public Guid Cuid { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string SecretKey { get; set; }


        public CqrImage ContactImage { get; set; }


        public string NameEmail { get => string.IsNullOrEmpty(Email) ? Name : $"{Name} <{Email}>"; }

        
        #endregion properties

        #region constructors

        public CqrContact()
        {
            ContactId = -1;
            Cuid = Guid.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Mobile = string.Empty;
            Address = string.Empty;
            SecretKey = string.Empty;
            ContactImage = null;
        }

        public CqrContact(int contactId, string name, string email, string mobile, string address)
        {
            this.ContactId = contactId;
            this.Name = name;
            this.Email = email;
            this.Mobile = mobile;
            this.Address = address;
        }

        public CqrContact(Guid guid, string name, string email, string mobile, string address)
        {
            this.Cuid = guid;
            this.Name = name;
            this.Email = email;
            this.Mobile = mobile;
            this.Address = address;
        }

        public CqrContact(int contactId, string name, string email, string mobile, string address, CqrImage cqrImage) : this(contactId, name, email, mobile, address)
        {
            ContactImage = cqrImage;
        }

        public CqrContact(int contactId, Guid cuid, string name, string email, string mobile, string address, CqrImage cqrImage) : this(contactId, name, email, mobile, address)
        {
            Cuid = cuid;
            ContactImage = cqrImage;
        }

        public CqrContact(int contactId, string name, string email, string mobile, string address, Image image) : this(contactId, name, email, mobile, address)
        {
            ContactImage = CqrImage.FromDrawingImage(image);
        }

        public CqrContact(int contactId, Guid cuid, string name, string email, string mobile, string address, Image image) : this(contactId, name, email, mobile, address)
        {
            Cuid = cuid;
            ContactImage = CqrImage.FromDrawingImage(image);
        }

        #endregion constructors

        #region members

        public virtual string ToJson()
        {
            CqrContact cqrContact = new CqrContact(ContactId, Cuid, Name, Email, Mobile, Address, ContactImage);
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
                    this.Cuid = cqrContactJson.Cuid;
                    this.Name = cqrContactJson.Name;
                    this.Email = cqrContactJson.Email;
                    this.Mobile = cqrContactJson.Mobile;
                    this.Address = cqrContactJson.Address;
                    this.ContactImage = cqrContactJson.ContactImage;
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
                "Cuid: " + this.Cuid + ";" + Environment.NewLine +
                "Name: " + this.Name + ";" + Environment.NewLine +
                "Email: " + this.Email + ";" + Environment.NewLine +
                "Mobile: " + this.Mobile + ";" + Environment.NewLine +
                "Address: " + this.Address + ";" + Environment.NewLine +
                "ImageFileName: " + this.ContactImage.ImageFileName + ";" + Environment.NewLine +
                "ImageMimeType: " + this.ContactImage.ImageMimeType + ";" + Environment.NewLine +
                "ImageBase64: " + this.ContactImage?.ImageBase64 + Environment.NewLine
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

        #endregion members

    }

}
