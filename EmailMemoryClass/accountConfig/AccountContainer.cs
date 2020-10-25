using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EmailMemoryClass
{
    public class AccountContainer
    {
        private string _settingsFile;

        public string SettingsFile
        {
            get { return _settingsFile; }
            set { _settingsFile = value; }
        }

        public AccountContainer()
        {
            
        }

        public AccountContainer(bool proceed)
        {
            if(proceed)
            {
                SettingsFile = CalculateConfigPath();
                CheckFile();
                LoadFromFile();

                if(Accounts == null)
                {
                    Accounts = new List<AccountConfig>();
                }
            }
        }

        private List<AccountConfig> accounts;

        public List<AccountConfig> Accounts
        {
            get { return accounts; }
            set { accounts = value; }
        }

        #region methods
        void LoadFromFile()
        {
            var loadedConfig = ConstructFromXml(SettingsFile);
            Accounts = loadedConfig.Accounts;
        }

        string CalculateConfigPath()
        {
            var appdata = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var currentuser = Environment.UserName;
            return  $"{appdata}\\EmailMemory\\{currentuser}\\app.config";
        }

        void CheckFile()
        {
            var parent = Directory.GetParent(SettingsFile);

            if (!parent.Exists)
                Directory.CreateDirectory(parent.FullName);

            if (!File.Exists(SettingsFile))
                SaveToXml(SettingsFile);
        }

        AccountContainer ConstructFromXml(string FileName)
        {
            Console.WriteLine("Loading from file " + FileName);
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var serializer = new XmlSerializer(typeof(AccountContainer));
                return serializer.Deserialize(stream) as AccountContainer;
            }
        }

        void SaveToXml(string FileName)
        {
            Console.WriteLine("Saving to file " + FileName);
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
                Console.WriteLine("Failed to save file... retrying");
                success = false;
            }

            return success;
        }
        #endregion
    }


}