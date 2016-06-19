using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBox.Client
{
    public class StorageConnectionSettings
    {
        private string _url;
        public string Url
        {
            get
            {
                //Fixes slash bug:
                return string.Format("{0}{1}", _url, _url.EndsWith("/") ? "" : "/");
            }
            set
            {
                _url = value;
            }
        }
        public string ApplicationKey { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DeviceDescription { get; set; }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Gets the settings from configuration file.
        /// </summary>
        /// <returns></returns>
        public static StorageConnectionSettings LoadSettingsFromConfigFile()
        {
            StorageConnectionSettings settings = new StorageConnectionSettings()
            {
                Url = Properties.Settings.Default.StorageServiceEndPoint,
                ApplicationKey = Properties.Settings.Default.ApplicationKey,
                UserName = Properties.Settings.Default.UserName,
                Password = Properties.Settings.Default.Password,
                DeviceDescription = Properties.Settings.Default.DeviceDescription
            };

            return settings;
        }
    }
}
