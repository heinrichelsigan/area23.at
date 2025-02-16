﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;
using static QRCoder.Core.PayloadGenerator.SwissQrCode;
using System.Text.Json.Serialization;

namespace Area23.At.Framework.Core.CqrXs.CqrMsg
{

    /// <summary>
    /// CqrContact is a contact for CqrJd
    /// </summary>EL
    [JsonObject]
    [Serializable]    
    public class CqrContact : MsgContent
    {

        #region properties
        
        public int ContactId { get; set; }

        public Guid Cuid { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string? Mobile { get; set; }

        public string? Address { get; set; }

        public string? SecretKey { get; set; }

        public CqrImage? ContactImage { get; set; }

        public string? NameEmail { get => string.IsNullOrEmpty(Email) ? Name : $"{Name} <{Email}>"; }

        #endregion properties

        #region constructors

        public CqrContact() : base()
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

        public CqrContact(string cs, MsgEnum msgArt = MsgEnum.JsonSerialized)
        {
            FromJson<CqrContact>(cs);
        }

        public CqrContact(int contactId, string name, string email, string? mobile, string? address) : base()
        {
            ContactId = contactId;
            Name = name;
            Email = email;
            Mobile = mobile;
            Address = address;
        }

        public CqrContact(Guid guid, string name, string email, string? mobile, string? address) : base()
        {
            Cuid = guid;
            Name = name;
            Email = email;
            Mobile = mobile;
            Address = address;
        }

        public CqrContact(int contactId, string name, string email, string? mobile, string? address, CqrImage? cqrImage) : 
            this(contactId, name, email, mobile, address)
        {
            ContactImage = cqrImage;
        }

        public CqrContact(int contactId, Guid cuid, string name, string email, string? mobile, string? address, CqrImage? cqrImage) : 
            this(contactId, name, email, mobile, address)
        {
            Cuid = cuid;
            ContactImage = cqrImage;
        }

        public CqrContact(int contactId, string name, string email, string? mobile, string? address, Image? image) : 
            this(contactId, name, email, mobile, address)
        {
            ContactImage = CqrImage.FromDrawingImage(image);
        }

        public CqrContact(int contactId, Guid cuid, string name, string email, string? mobile, string? address, Image? image) : this(contactId, name, email, mobile, address)
        {
            Cuid = cuid;
            ContactImage = CqrImage.FromDrawingImage(image);
        }

        #endregion constructors

        #region members

        public override string ToJson()
        {
            CqrContact cqrContact = new CqrContact(ContactId, Cuid, Name, Email, Mobile, Address, ContactImage);
            string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(cqrContact, Formatting.Indented);
            this._rawMessage = jsonString;
            return jsonString;
        }

        public override TType? FromJson<TType>(string jsonText) where TType : default
        {
            TType? tt = Newtonsoft.Json.JsonConvert.DeserializeObject<TType>(jsonText);
            try
            {
                if (tt != null && tt is CqrContact cqrContactJson)
                {
                    if (cqrContactJson != null && cqrContactJson.ContactId > -1 && !string.IsNullOrEmpty(cqrContactJson?.Name))
                    {
                        ContactId = cqrContactJson.ContactId;
                        Cuid = cqrContactJson.Cuid;
                        Name = cqrContactJson.Name;
                        Email = cqrContactJson.Email;
                        Mobile = cqrContactJson.Mobile;
                        Address = cqrContactJson.Address;
                        ContactImage = cqrContactJson.ContactImage;
                        _message = cqrContactJson.Message;
                        _hash = cqrContactJson.Hash;
                        _rawMessage = cqrContactJson.RawMessage;
                        return tt;
                    }
                }
            }
            catch (Exception exJson)
            {
                Area23Log.LogStatic(exJson);
            }

            return default(TType);
        }



        public override string ToString()
        {
            return                
                $"\"ContactId\": \t\"{ContactId}\";{Environment.NewLine}" +
                $"\"Cuid\": \t\"{Cuid}\";{Environment.NewLine}" +
                $"\"Name\": \t\"{Name ?? ""}\";{Environment.NewLine}" +
                $"\"Email\": \t\"{Email ?? ""}\";{Environment.NewLine}" +
                $"\"Mobile\": \t\"{Mobile ?? ""}\";{Environment.NewLine}" +
                $"\"Address\": \t\"{Address ?? ""}\";{Environment.NewLine}" +
                $"\"NameEmail\": \t\"{NameEmail ?? ""}\";{Environment.NewLine}" +
                ((ContactImage != null) ?
                    $"\"ContactImage\": \t{Environment.NewLine}" +
                    $"\"ContactImage.ImageFileName\": \t\"{ContactImage.ImageFileName}\";{Environment.NewLine}" +
                    $"\"ContactImage.ImageMimeType\": \t\"{ContactImage.ImageMimeType}\";{Environment.NewLine}" +
                    $"\"ContactImage.ImageBase64\": \t\"{ContactImage.ImageBase64}\";{Environment.NewLine}" 
                    : "")
                ;
        }

        /// <summary>
        /// <see cref="object[]">RowParams</see> gets an object array of row parameters to show in <see cref="DataGridView"/>
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
