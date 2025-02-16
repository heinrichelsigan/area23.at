using Area23.At.Framework.Core.CqrXs.CqrMsg;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area23.At.Framework.Core.CqrXs
{
    public class CqrSettings 
    {
        // TODO: replace it in C# 9.0 to private static readonly lock _lock
        protected static readonly object _lock = true;

        protected static readonly Lazy<CqrSettings> _instance =
            new Lazy<CqrSettings>(() => new CqrSettings());

        
        #region properties

        public static CqrSettings Instance { get => _instance.Value; }

        public DateTime TimeStamp { get; set; }

        public DateTime? SaveStamp { get; set; }

        public CqrContact MyContact { get; set; }

        public List<CqrContact> Contacts { get; set; }

        public List<string> FriendIPs { get; set; }

        public List<string> MyIPs { get; set; }

        public List<string> Proxies { get; set; }

        public List<string> SecretKeys { get; set; }

        #endregion properties

        #region ctor CqrSettings() CqrSettings(DateTime timeStamp) => Load()

        /// <summary>
        /// CqrSettings constructor maybe needed public for NewTonSoftJson serializing object
        /// </summary>
        public CqrSettings()
        {
            TimeStamp = DateTime.Now;
            Contacts = new List<CqrContact>();
            FriendIPs = new List<string>();
            MyIPs = new List<string>();
            Proxies = new List<string>();
            SecretKeys = new List<string>();
            MyContact = new CqrContact();
        }


        /// <summary>
        /// ctor with inital timestamp
        /// </summary>
        /// <param name="timeStamp"></param>
        public CqrSettings(DateTime timeStamp) : this()
        {
            TimeStamp = timeStamp;
            Load();
        }

        #endregion ctor CqrSettings() CqrSettings(DateTime timeStamp) => Load()

        #region static members Load() Save(Settings? settings)

        /// <summary>
        /// loads json serialized Settings data string from 
        /// <see cref="LibPaths.AppDirPath"/> + <see cref="Constants.JSON_SAVE_FILE"/>
        /// and deserialize it to singleton instance <see cref="CqrSettings"/> of <seealso cref="Lazy{Settings}"/>
        /// </summary>
        /// <returns>singelton <see cref="CqrSettings.Instance"/></returns>
        public static CqrSettings? Load()
        {
            string settingsJsonString = string.Empty;
            CqrSettings? settings = null;
            string fileName = LibPaths.SystemDirPath + Constants.JSON_SETTINGS_FILE;
            try
            {
                if (!File.Exists(fileName) && Directory.Exists(LibPaths.AppPath))
                {
                    File.CreateText(fileName);
                }

                settingsJsonString = File.ReadAllText(fileName);
                settings = JsonConvert.DeserializeObject<CqrSettings>(settingsJsonString);
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
        /// json serializes <see cref="CqrSettings"/> and 
        /// saves json serialized data string to 
        /// <see cref="LibPaths.AppDirPath"/> + <see cref="Constants.JSON_SAVE_FILE"/>
        /// </summary>
        /// <param name="CqrSettings">settings to save</param>
        /// <returns>true on successfully save</returns>
        public static bool Save(CqrSettings? settings)
        {
            string saveString = string.Empty;
            if (settings == null)
                settings = CqrSettings.Instance;
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

        #endregion static members Load() Save(CqrSettings? settings)

    }

}
