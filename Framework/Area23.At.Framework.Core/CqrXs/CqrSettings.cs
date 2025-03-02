using Area23.At.Framework.Core.CqrXs.CqrMsg;
using Area23.At.Framework.Core.Static;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
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
        /// <param name="jsonFileName">file name (incl. path), that contains serialized <see cref="CqrSettings"/> json</param>
        /// <returns>singelton <see cref="CqrSettings.Instance"/></returns>
        public static CqrSettings? Load(string? jsonFileName = null)
        {
            string settingsJsonString = string.Empty;
            CqrSettings? settings = null;
            jsonFileName = jsonFileName ?? LibPaths.SystemDirPath + Constants.JSON_SETTINGS_FILE;
            try
            {
                if (File.Exists(jsonFileName))
                {
                    settingsJsonString = File.ReadAllText(jsonFileName);
                    settings = JsonConvert.DeserializeObject<CqrSettings>(settingsJsonString);
                }
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
            
            return _instance.Value;
        }

        /// <summary>
        /// json serializes <see cref="CqrSettings"/> and 
        /// saves json serialized data string to 
        /// <see cref="LibPaths.AppDirPath"/> + <see cref="Constants.JSON_SAVE_FILE"/>
        /// </summary>
        /// <param name="CqrSettings">settings to save</param>
        /// <param name="jsonFileName">filename (incl. path), where writing serialized <see cref="CqrSettings"/> json</param>
        /// <returns>true on successfully save</returns>
        public static bool Save(CqrSettings? settings = null, string? jsonFileName = null)
        {
            settings = settings ?? CqrSettings.Instance;
            jsonFileName = jsonFileName ?? LibPaths.SystemDirPath + Constants.JSON_SETTINGS_FILE;
            try
            {
                settings.SaveStamp = DateTime.Now;
                string saveString = JsonConvert.SerializeObject(settings);
                File.WriteAllText(jsonFileName, saveString);
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
