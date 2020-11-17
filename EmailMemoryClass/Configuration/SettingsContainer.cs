using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Serialization;

namespace EmailMemoryClass.Configuration
{
    public class SettingsContainer
    {
        #region fields
        string _settingsFile;
        Timer _servicesTimer;
        List<AccountConfig> _accounts;
        ApplicationConfiguration _appConfig;
        #endregion

        #region properties
        public string SettingsFile
        {
            get { return _settingsFile; }
            set { _settingsFile = value; }
        }

        public List<AccountConfig> Accounts
        {
            get { return _accounts; }
            set { _accounts = value; }
        }

        public ApplicationConfiguration ApplicationConfig
        {
            get { return _appConfig; }
            set { _appConfig = value; }
        }
        #endregion

        public SettingsContainer()
        {
            //
        }

        public SettingsContainer(bool proceed)
        {
            if(proceed)
            {
                SettingsFile = CalculateConfigPath();
                CheckFile();
                LoadFromFile();
                StartTimer();

                if (Accounts == null)
                    Accounts = new List<AccountConfig>();

                if (ApplicationConfig == null)
                    ApplicationConfig = new ApplicationConfiguration();
            }
        }

       
        async void ServicesTimerElapsed(object sender, ElapsedEventArgs e)
        {
            await Logger.CheckForEntriesAsync();
        }

        #region methods
        void StartTimer()
        {
            _servicesTimer = new Timer(10000) { AutoReset = true };
            _servicesTimer.Start();
            _servicesTimer.Elapsed += ServicesTimerElapsed;
        }

        void LoadFromFile()
        {
            var loadedConfig = ConstructFromXml(SettingsFile);
            Accounts = loadedConfig.Accounts;
        }

        string CalculateConfigPath()
        {
            var appdata = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var currentuser = Environment.UserName;
            return  $"{appdata}\\OutlookClipboard\\{currentuser}\\app.config";
        }

        void CheckFile()
        {
            var parent = Directory.GetParent(SettingsFile);
            Logger.Log("Configuration Directory: " + parent.FullName);

            if (!parent.Exists)
            {
                Directory.CreateDirectory(parent.FullName);
                Logger.Log("Configuration directory does not exist");
            }

            if (!File.Exists(SettingsFile))
                SaveToXml(SettingsFile);
        }

        SettingsContainer ConstructFromXml(string FileName)
        {
            Logger.Log("Loading from file " + FileName);
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var serializer = new XmlSerializer(typeof(SettingsContainer));
                return serializer.Deserialize(stream) as SettingsContainer;
            }
        }

        void SaveToXml(string FileName)
        {
            Logger.Log("Saving to file " + FileName);
            using (var writer = new System.IO.StreamWriter(FileName))
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }

        public bool SaveChanges()
        {
            int attempts = 0;
            bool success = false;

            try
            {
                while (!success)
                {
                    success = SaveFile();
                    attempts++;

                    if (attempts > 4)
                        break;
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        bool SaveFile()
        {
            bool success = false;

            try
            {
                if (File.Exists(SettingsFile))
                    File.Delete(SettingsFile);

                SaveToXml(SettingsFile);

                if (File.Exists(SettingsFile))
                    success = true;
            }
            catch (Exception)
            {
                Logger.Log("Failed to save file... retrying", "Error");
                success = false;
            }

            return success;
        }
        #endregion
    }


}