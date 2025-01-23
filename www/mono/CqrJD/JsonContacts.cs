using Area23.At;
using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Util;
using Area23.At.Mono.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace Area23.At.Mono.CqrJD
{
    public static class JsonContacts
    {
        static object _lock = new object();
        static HashSet<Contact> _contacts;

        internal static HashSet<Contact> LoadJsonContacts()
        {
            lock (_lock)
            {
                if (!System.IO.File.Exists(Framework.Library.Util.JsonHelper.JsonContactsFile))
                    System.IO.File.Create(Framework.Library.Util.JsonHelper.JsonContactsFile);
            }
            Thread.Sleep(100);
            lock (_lock)
            {
                string jsonText = System.IO.File.ReadAllText(Framework.Library.Util.JsonHelper.JsonContactsFile);
                _contacts = JsonConvert.DeserializeObject<HashSet<Contact>>(jsonText);
                if (_contacts == null || _contacts.Count == 0)
                    _contacts = new HashSet<Contact>();
                HttpContext.Current.Application[Constants.JSON_CONTACTS] = _contacts;
            }
            return _contacts;
        }

        internal static void SaveJsonContacts(HashSet<Contact> contacts)
        {
            JsonSerializerSettings jsets = new JsonSerializerSettings();
            jsets.Formatting = Formatting.Indented;
            string jsonString = JsonConvert.SerializeObject(contacts, Formatting.Indented);
            System.IO.File.WriteAllText(Framework.Library.Util.JsonHelper.JsonContactsFile, jsonString);
            HttpContext.Current.Application[Constants.JSON_CONTACTS] = contacts;
        }
    }
}