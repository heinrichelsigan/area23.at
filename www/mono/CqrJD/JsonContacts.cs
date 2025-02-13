using Area23.At;
using Area23.At.Framework.Library;
using Area23.At.Framework.Library.Crypt.CqrJd;
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
        static HashSet<CqrContact> _contacts;
        internal static string JsonContactsFileName { get => Framework.Library.Util.JsonHelper.JsonContactsFile; }

        static JsonContacts()
        {
            _contacts = LoadJsonContacts();
        }

        internal static HashSet<CqrContact> LoadJsonContacts()
        {
            object locker = new object();
            lock (locker)
            {
                if (!System.IO.File.Exists(JsonContactsFileName))
                    System.IO.File.Create(JsonContactsFileName);
            }
            object cyberfyber = new object();
            Thread.Sleep(100);
            lock (cyberfyber)
            {
                string jsonText = System.IO.File.ReadAllText(JsonContactsFileName);
                _contacts = JsonConvert.DeserializeObject<HashSet<CqrContact>>(jsonText);
                if (_contacts == null || _contacts.Count == 0)
                    _contacts = new HashSet<CqrContact>();
                HttpContext.Current.Application[Constants.JSON_CONTACTS] = _contacts;
            }
            return _contacts;
        }

        internal static void SaveJsonContacts(HashSet<CqrContact> contacts)
        {
            JsonSerializerSettings jsets = new JsonSerializerSettings();
            jsets.Formatting = Formatting.Indented;
            string jsonString = JsonConvert.SerializeObject(contacts, Formatting.Indented);
            System.IO.File.WriteAllText(JsonContactsFileName, jsonString);
            HttpContext.Current.Application[Constants.JSON_CONTACTS] = contacts;
        }
    }
}