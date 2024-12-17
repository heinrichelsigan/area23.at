using Area23.At.Framework.Library.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Area23.At.WinForm.SecureChat.Entities
{
    public class Settings
    {
        private static readonly Lazy<Settings> _instance =
            new Lazy<Settings>(() => new Settings());
        public static Settings Instance { get => _instance.Value; }

        public Contact? MyContact { get; set; }

        public List<Contact> Contacts { get; set; }

        public List<string> FriendIPs { get; set; }
        
        public List<string> MyIPs { get; set; }

        public List<string> Proxies {  get; set; }

        public Settings() 
        {            
            Contacts = new List<Contact>();
            FriendIPs = new List<string>();
            MyIPs = new List<string>();
            Proxies = new List<string>();
        }

        public static bool Save(Settings? settings)
        {
            string saveString = string.Empty;
            if (settings == null)
                settings = Settings.Instance;
            try
            {
                saveString = JsonConvert.SerializeObject(settings);
                File.WriteAllText(LibPaths.AppDirPath + Constants.JSON_SETTINGS_FILE, saveString);
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
                return false;
            }
            return true;
        }

        public static Settings? Load()
        {
            string settingsJsonString = string.Empty;
            Settings? settings = null;
            try
            {
                settingsJsonString = File.ReadAllText(LibPaths.AppDirPath + Constants.JSON_SETTINGS_FILE);
                settings = JsonConvert.DeserializeObject<Settings>(settingsJsonString);
            }
            catch (Exception ex)
            {
                Area23Log.LogStatic(ex);
            }

            if (settings != null)
            {
                _instance.Value.Contacts = settings.Contacts;
                _instance.Value.MyContact = settings.MyContact;
                _instance.Value.FriendIPs = settings.FriendIPs;
                _instance.Value.MyIPs = settings.MyIPs;
                _instance.Value.Proxies = settings.Proxies;
            }
            return settings;
        }

    }
}
