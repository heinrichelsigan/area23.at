using Area23.At.Framework.Library.Cqr.Msg;
using Area23.At.Framework.Library.Static;
using Area23.At.Framework.Library.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Area23.At.Mono.Util
{
    /// <summary>
    /// JsonContacts 
    /// </summary>
    public static class JsonContacts
    {

        static object _lock = new object();
        static HashSet<CContact> _contacts;
        internal static string JsonContactsFileName { get => Framework.Library.Static.JsonHelper.JsonContactsFile; }


        /// <summary>
        /// JsonContacts
        /// </summary>
        static JsonContacts()
        {
            _contacts = LoadJsonContacts();
        }

        #region LoadSaveJsonContacts

        /// <summary>
        /// LoadJsonContacts
        /// </summary>
        /// <returns><see cref="HashSet{CqrContact}"/></returns>
        internal static HashSet<CContact> LoadJsonContacts()
        {
            HashSet<CContact> deserializedContacts = new HashSet<CContact>(), contacts = new HashSet<CContact>();
            lock (_lock)
            {
                if (!System.IO.File.Exists(JsonContactsFileName))
                    System.IO.File.Create(JsonContactsFileName);
            }
            //Thread.Sleep(8);
            lock (_lock)
            {
                string jsonText = System.IO.File.ReadAllText(JsonContactsFileName);
                deserializedContacts = JsonConvert.DeserializeObject<HashSet<CContact>>(jsonText);
            }
            if (deserializedContacts != null && deserializedContacts.Count > 0)
            {
                foreach (CContact deContact in deserializedContacts)
                {
                    deContact.SerializedMsg = string.Empty;
                    string serializedMsg = JsonConvert.SerializeObject(deContact);
                    deContact.SerializedMsg = serializedMsg;
                    contacts.Add(deContact);
                }
            }
            if (contacts != null)
            {
                _contacts = contacts;
            }

            //if (BaseWebService.UseApplicationState)
            //        HttpContext.Current.Application[Constants.JSON_CONTACTS] = _contacts;
            //if (BaseWebService.UseAmazonElasticCache)
            //{
            //    string dictContactsJson = JsonConvert.SerializeObject(_contacts);
            //    RedIs.Db.StringSet(Constants.JSON_CONTACTS, dictContactsJson);                   
            //}

            return _contacts;
        }

        /// <summary>
        /// Method to persist Json Contacts
        /// </summary>
        /// <param name="contacts">contacts to save</param>
        internal static void SaveJsonContacts(HashSet<CContact> contacts)
        {
            _contacts = _contacts ?? new HashSet<CContact>();
            if (contacts != null && contacts.Count > 0 && contacts.Count > _contacts.Count)
                _contacts = contacts;
            HashSet<CContact> serializingContacts = new HashSet<CContact>();
            foreach (CContact cContact in _contacts)
            {
                if (cContact != null && !string.IsNullOrEmpty(cContact.NameEmail))
                {
                    cContact.SerializedMsg = string.Empty;
                    serializingContacts.Add(cContact);
                }
            }
            JsonSerializerSettings jsets = new JsonSerializerSettings();
            jsets.Formatting = Formatting.Indented;
            lock (_lock)
            {
                string jsonString = JsonConvert.SerializeObject(serializingContacts, Formatting.Indented);
                System.IO.File.WriteAllText(JsonContactsFileName, jsonString);
            }

            //if (BaseWebService.UseApplicationState)
            //    HttpContext.Current.Application[Constants.JSON_CONTACTS] = contacts;
            //if (BaseWebService.UseAmazonElasticCache)
            //{
            //    string dictContactsJson = JsonConvert.SerializeObject(_contacts);
            //    RedIs.Db.StringSet(Constants.JSON_CONTACTS, dictContactsJson);
            //}

        }

        #endregion LoadSaveJsonContacts

        #region GetAddUpdate

        /// <summary>
        /// GetContacts get contacts from json file
        /// </summary>
        /// <returns><see cref="HashSet{CqrContact}"/></returns>
        internal static HashSet<CContact> GetContacts(bool loadFromJson = false)
        {
            if (_contacts == null || _contacts.Count < 1 || loadFromJson)
                _contacts = JsonContacts.LoadJsonContacts();

            return _contacts;
        }

        /// <summary>
        /// Adds or updates a contact to json contacts
        /// </summary>
        /// <param name="ccontact">contact to add</param>
        /// <returns>modified fouund contact</returns>
        internal static CContact AddContact(CContact ccontact)
        {
            _contacts = JsonContacts.GetContacts(true);
            CContact foundCt = JsonContacts.FindContactByNameEmail(_contacts, ccontact);
            if (foundCt != null)
            {
                foundCt.ContactId = (ccontact.ContactId > 0) ? ccontact.ContactId : foundCt.ContactId;
                if (foundCt.Cuid == null || foundCt.Cuid == Guid.Empty)
                    foundCt.Cuid = Guid.NewGuid();
                if (!string.IsNullOrEmpty(ccontact.Address))
                    foundCt.Address = ccontact.Address;
                if (ccontact.Mobile != null && ccontact.Mobile.Length > 1)
                    foundCt.Mobile = ccontact.Mobile;
                if (ccontact._message != null && !string.IsNullOrEmpty(ccontact._message))
                    foundCt._message = ccontact._message;

                foundCt.ContactImage = null;
                UpdateContact(foundCt);
            }
            else
            {
                if (ccontact.Cuid == null || ccontact.Cuid == Guid.Empty)
                    ccontact.Cuid = Guid.NewGuid();
                foundCt = new CContact(ccontact, ccontact._message, ccontact._hash);
                foundCt.ContactImage = null;
                foundCt._message = ccontact._message;

                _contacts.Add(foundCt);
                JsonContacts.SaveJsonContacts(_contacts);
            }

            return foundCt;
        }


        /// <summary>
        /// Updates a contact
        /// </summary>
        /// <param name="ccontact">contact to update</param>
        internal static void UpdateContact(CContact ccontact)
        {
            CContact toAddContact = null;
            if (ccontact == null || string.IsNullOrEmpty(ccontact.Email))
                return;

            string chatRoomNr = (ccontact._message != null && !string.IsNullOrEmpty(ccontact._message)) ? ccontact._message : "";
            HashSet<CContact> contacts = new HashSet<CContact>();

            foreach (CContact ct in _contacts)
            {
                if ((ct.Cuid == ccontact.Cuid && ct.Email.Equals(ccontact.Email, StringComparison.CurrentCultureIgnoreCase) ||
                    ct.NameEmail.Equals(ccontact.NameEmail, StringComparison.CurrentCultureIgnoreCase)))
                {
                    toAddContact = new CContact(ccontact, chatRoomNr, ccontact.Hash);
                    toAddContact.Mobile = ccontact.Mobile;
                    toAddContact.ContactImage = null;
                    toAddContact.Cuid = (ccontact.Cuid != null && ccontact.Cuid != Guid.Empty) ? ccontact.Cuid : Guid.NewGuid();
                    toAddContact._message = chatRoomNr;
                    contacts.Add(toAddContact);
                }
                else
                    contacts.Add(ct);
            }
            _contacts = contacts;
            JsonContacts.SaveJsonContacts(_contacts);
        }


        [Obsolete("UpdateContacts is never used", false)]
        internal static void UpdateContacts(CContact contact, CSrvMsg<string> chatRoomMsg, string chatRoomNr)
        {
            bool foundCt = false;
            CContact toDelContact = null;
            if (contact == null || string.IsNullOrEmpty(contact.Email))
                return;

            if ((chatRoomMsg.Sender.Cuid == contact.Cuid && chatRoomMsg.Sender.Email.Equals(contact.Email, StringComparison.CurrentCultureIgnoreCase)) ||
                (chatRoomMsg.Sender.NameEmail.Equals(contact.NameEmail, StringComparison.CurrentCultureIgnoreCase)))
            {
                chatRoomMsg.Sender = new CContact(contact, chatRoomNr, contact.Hash);
            }
            for (int i = 0; i < chatRoomMsg.Recipients.Count; i++)
            {
                if ((chatRoomMsg.Recipients.ElementAt(i).Cuid == contact.Cuid &&
                        chatRoomMsg.Recipients.ElementAt(i).Name.Equals(contact.Name, StringComparison.CurrentCultureIgnoreCase)) ||
                    (chatRoomMsg.Recipients.ElementAt(i).Cuid == contact.Cuid &&
                        chatRoomMsg.Recipients.ElementAt(i).Email.Equals(contact.Email, StringComparison.CurrentCultureIgnoreCase)) ||
                    chatRoomMsg.Recipients.ElementAt(i).NameEmail.Equals(contact.NameEmail, StringComparison.CurrentCultureIgnoreCase))
                {
                    toDelContact = chatRoomMsg.Recipients.ElementAt(i);
                    foundCt = true;
                    break;
                }
            }
            if (foundCt)
            {
                if (chatRoomMsg.Recipients.Remove(toDelContact))
                {

                    CContact cToAdd = new CContact(contact, chatRoomNr, contact._hash);
                    cToAdd._message = chatRoomNr;
                    chatRoomMsg.Recipients.Add(cToAdd);
                }
            }

            JsonChatRoom.SaveChatRoom(chatRoomMsg, chatRoomMsg.CRoom);
            // (new JsonChatRoom(_chatRoomNumber)).SaveJsonChatRoom(chatRoomMsg, chatRoomNr);

            foundCt = false;
            GetContacts();
            for (int j = 0; j < _contacts.Count; j++)
            {
                if ((_contacts.ElementAt(j).Cuid == contact.Cuid) &&
                    (_contacts.ElementAt(j).Name.Equals(contact.Name) ||
                        _contacts.ElementAt(j).NameEmail.Equals(contact.NameEmail)))
                {
                    toDelContact = _contacts.ElementAt(j);
                    foundCt = true;
                    break;
                }
            }
            if (foundCt)
            {
                if (_contacts.Remove(toDelContact))
                {
                    CContact c2Add = new CContact(contact, chatRoomNr, contact.Hash);
                    c2Add._message = chatRoomNr;
                    _contacts.Add(c2Add);
                }
            }

            JsonContacts.SaveJsonContacts(_contacts);

        }

        #endregion GetAddUpdate

        #region SearchFindContacts

        /// <summary>
        /// FindContactByNameEmail
        /// </summary>
        /// <param name="contacts">contacts to search</param>
        /// <param name="searchContact">contact to find</param>
        /// <returns></returns>
        public static CContact FindContactByNameEmail(HashSet<CContact> contacts, CContact searchContact)
        {
            CContact foundC = FindContactByNameEmail(contacts, searchContact.Name, searchContact.Email, searchContact.Mobile);
            return foundC;
        }

        /// <summary>
        /// FindContactByNameEmail find contact by name email
        /// </summary>
        /// <param name="contacts">contacts to search</param>
        /// <param name="cName">name to find</param>
        /// <param name="cEmail">email to find</param>
        /// <param name="cMobile">mobile to find</param>
        /// <returns><see cref="CqrContact"/></returns>
        public static CContact FindContactByNameEmail(HashSet<CContact> contacts, string cName, string cEmail, string cMobile)
        {

            if (!string.IsNullOrEmpty(cName) || !string.IsNullOrEmpty(cEmail))
            {
                string cNameEmail = string.IsNullOrEmpty(cEmail) ? cName : $"{cName} <{cEmail}>";
                string cPhone = cMobile ?? string.Empty;

                foreach (CContact c in contacts)
                {
                    if (c != null && !string.IsNullOrEmpty(c.NameEmail))
                    {
                        if (c.NameEmail.Equals(cNameEmail, StringComparison.CurrentCultureIgnoreCase) ||
                            c.Email.Equals(cEmail, StringComparison.CurrentCultureIgnoreCase) ||
                                (!string.IsNullOrEmpty(c.Mobile) &&
                                    c.Mobile.Equals(cPhone, StringComparison.CurrentCultureIgnoreCase) &&
                                    c.Name.Equals(cName, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            return c;
                        }
                    }
                }
            }

            return null;
        }

        #endregion SearchFindContacts

    }

}