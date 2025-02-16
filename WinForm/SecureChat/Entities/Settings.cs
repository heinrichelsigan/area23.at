using Area23.At.Framework.Core;
using Area23.At.Framework.Core.CqrXs;
using Area23.At.Framework.Core.CqrXs.CqrMsg;
using Area23.At.Framework.Core.CqrXs.CqrSrv;
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

    public class Settings : IDisposable
    {
        // TODO: replace it in C# 9.0 to private static readonly lock _lock
        private static readonly object _lock = true;

        private static readonly Lazy<Settings> _instance =
            new Lazy<Settings>(() => new Settings());

        private static bool _disposed = false;
        private static bool _destructed = false;

        #region properties
        public static Settings Instance { get => _instance.Value; }

        public DateTime TimeStamp { get; set; }

        public DateTime? SaveStamp { get; set; }

        public Contact MyContact { get; set; }

        public List<Contact> Contacts { get; set; }

        public List<string> FriendIPs { get; set; }

        public List<string> MyIPs { get; set; }

        public List<string> Proxies { get; set; }

        public List<string> SecretKeys { get; set; }

        #endregion properties

        #region ctor Settings() Settings(DateTime timeStamp) => Load()

        /// <summary>
        /// Settings constructor maybe needed public for NewTonSoftJson serializing object
        /// </summary>
        public Settings()
        {
            TimeStamp = DateTime.Now;
            Contacts = new List<Contact>();
            FriendIPs = new List<string>();
            MyIPs = new List<string>();
            Proxies = new List<string>();
            SecretKeys = new List<string>();
            MyContact = new Contact() { ContactId = 0 };
        }

        public Settings(DateTime timeStamp) : this()
        {
            TimeStamp = timeStamp;
            Load();
        }

        #endregion ctor Settings() Settings(DateTime timeStamp) => Load()

        #region static members Load() Save(Settings? settings)

        /// <summary>
        /// loads json serialized Settings data string from 
        /// <see cref="LibPaths.AppDirPath"/> + <see cref="Constants.JSON_SAVE_FILE"/>
        /// and deserialize it to singleton instance <see cref="Settings"/> of <seealso cref="Lazy{Settings}"/>
        /// </summary>
        /// <returns>singelton <see cref="Settings.Instance"/></returns>
        public static Settings? Load()
        {
            string settingsJsonString = string.Empty;
            Settings? settings = null;
            string fileName = LibPaths.SystemDirPath + Constants.JSON_SETTINGS_FILE;
            try
            {
                if (!File.Exists(fileName) && Directory.Exists(LibPaths.AppPath))
                {
                    File.CreateText(fileName);
                }

                settingsJsonString = File.ReadAllText(fileName);
                settings = JsonConvert.DeserializeObject<Settings>(settingsJsonString);
            }
            catch (Exception ex)
            {
                CqrException.SetLastException(ex);
            }

            if (settings != null)
            {
                _instance.Value.Contacts = settings.Contacts;
                _instance.Value.MyContact = settings.MyContact;
                _instance.Value.FriendIPs = settings.FriendIPs;
                _instance.Value.MyIPs = settings.MyIPs;
                _instance.Value.Proxies = settings.Proxies;
                _instance.Value.TimeStamp = settings.TimeStamp;
                _instance.Value.SaveStamp = settings.SaveStamp;
            }
            return settings;
        }

        /// <summary>
        /// json serializes <see cref="Settings"/> and 
        /// saves json serialized data string to 
        /// <see cref="LibPaths.AppDirPath"/> + <see cref="Constants.JSON_SAVE_FILE"/>
        /// </summary>
        /// <param name="settings">settings to save</param>
        /// <returns>true on successfully save</returns>
        public static bool Save(Settings? settings)
        {
            string saveString = string.Empty;
            if (settings == null)
                settings = Settings.Instance;
            try
            {
                settings.SaveStamp = DateTime.Now;
                saveString = JsonConvert.SerializeObject(settings);
                File.WriteAllText(LibPaths.SystemDirPath + Constants.JSON_SETTINGS_FILE, saveString);
            }
            catch (Exception ex)
            {
                CqrException.SetLastException(ex);
                return false;
            }
            return true;
        }

        #endregion static members Load() Save(Settings? settings)

        #region Dispose() Dispose(bool disposing) ~Settings()

        public void Dispose()
        {
            this.Dispose(false);
        }

        public bool Dispose(bool disposing)
        {
            if (!_disposed || disposing)
            {
                lock (_lock)
                {
                    _destructed = true;
                    _disposed = Settings.Save(Instance);
                }
            }

            return _disposed;
        }

        //~Settings()
        //{
        //    if (!_destructed)
        //    {
        //        if (!Instance.Dispose(true))
        //        {
        //            string fileName = LibPaths.SystemDirPath + Constants.JSON_SAVE_FILE;
        //            throw new CqrException($"~Settings() couldn't save settings to to {fileName}.", CqrException.LastException);
        //        }
        //    }
        //    TimeStamp = DateTime.Now;
        //    _disposed = true;
        //    SaveStamp = null;
        //    this.Contacts.Clear();
        //    this.FriendIPs.Clear();
        //    this.Proxies.Clear();
        //    this.MyIPs.Clear();
        //    // TODO: Only way to destruct a singelton is to set _instance Lazy<T> to null
        //    // think about the risk, that reflection could change a private static non readonly field
        //    // so I decided to let the GC handle this
        //    // _instance = null;            
        //}

        #endregion Dispose() Dispose(bool disposing) ~Settings()

    }

}
