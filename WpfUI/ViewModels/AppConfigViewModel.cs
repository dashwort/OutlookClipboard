using Caliburn.Micro;
using EmailMemoryClass;
using EmailMemoryClass.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace WpfUI.ViewModels
{
    public class AppConfigViewModel : Screen
    {
        Timer _statusTimer;
        AccountConfig _selectedConfig;
        string _accountField;
        bool _canRemove;
        BindableCollection<AccountConfig> accounts = new BindableCollection<AccountConfig>();
        BindableCollection<AccountConfig> accountsBackup = new BindableCollection<AccountConfig>();


        public AppConfigViewModel()
        {
            LoadConfig();
            _statusTimer = new Timer(200) { AutoReset = true };
            _statusTimer.Elapsed += UITimerElapsed;
            _statusTimer.Start();
        }

        public bool CanRemove
        {
            get { return _canRemove; }
            set
            {
                _canRemove = value;
                NotifyOfPropertyChange();
            }
        }

        #region Properties
        public BindableCollection<AccountConfig> Accounts
        {
            get { return accounts; }
            set 
            { 
                accounts = value;
                NotifyOfPropertyChange();
            }
        }

        public AccountConfig SelectedConfiguration
        {
            get { return _selectedConfig; }
            set
            {
                _selectedConfig = value;
                NotifyOfPropertyChange(() => SelectedConfiguration);
            }
        }

        public BindableCollection<AccountConfig> AccountsBackup
        {
            get { return accountsBackup; }
            set { accountsBackup = value; }
        }

        /// <summary>
        /// Used for testing the account text box field
        /// </summary>
        public string AccountField
        {
            get { return _accountField; }
            set
            {
                _accountField = value;
                NotifyOfPropertyChange(() => AccountField);
            }
        }
        #endregion

        void LoadConfig()
        {
            if(Bootstrapper.ApplicationConfiguration.Accounts.Count > 0)
            {
                Accounts.AddRange(Bootstrapper.ApplicationConfiguration.Accounts);
            }
        }

        void UITimerElapsed(object sender, ElapsedEventArgs e)
        {
            CanRemove = !(SelectedConfiguration is null);
        }

        public void SaveConfig()
        {
            string error = string.Empty;

            if (CheckConfig(out error))
            {
                // set backup in case of failure
                AccountsBackup.Clear();
                AccountsBackup.AddRange(Bootstrapper.ApplicationConfiguration.Accounts);

                Bootstrapper.ApplicationConfiguration.Accounts.Clear();

                foreach (var entry in Accounts)
                {
                    Bootstrapper.ApplicationConfiguration.Accounts.Add(entry);
                }

                if (!Bootstrapper.ApplicationConfiguration.SaveChanges())
                {
                    MessageBox.Show("Failed to save changes", "Configuration not saved", MessageBoxButton.OK, MessageBoxImage.Error);
                    Bootstrapper.ApplicationConfiguration.Accounts.AddRange(AccountsBackup);
                } else
                {
                    MessageBox.Show("Configuration Updated, restarting application to take effect", "Configuration Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    Bootstrapper.RestartApplication();
                }
            }
            else
            {
                MessageBox.Show(error, "Configuration not saved", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        bool CheckConfig(out string error)
        {
            var errorBuilder = new StringBuilder();
            bool success = false;

            List<int> indexes = new List<int>();

            foreach (var account in Accounts)
            {
                if(indexes.Contains(account.DisplayIndex))
                {
                    success = false;
                    errorBuilder.AppendLine(account.EmailAddress + " index is not unique");
                }
                else
                {
                    indexes.Add(account.DisplayIndex);
                    success = true;
                }

                if (account.SearchTag == "Enter a search tag")
                {
                    success = false;
                    errorBuilder.AppendLine(account.EmailAddress + " search tag is at default value");
                }

                if(!OutlookSearch.IsAccountValid(account.EmailAddress))
                {
                    success = false;
                    errorBuilder.AppendLine(account.EmailAddress + " is not configured in outlook. The account needs to be configured with full read/write access.");
                }
            }

            error = errorBuilder.ToString();

            return success;
        }

        public void RemoveItem()
        {
            Accounts.Remove(SelectedConfiguration);
        }

        public bool CanAdd()
        {
            return OutlookSearch.IsValidEmail(AccountField) && Accounts.Count < 3 && Accounts.Count >= 0;
        }

        public void Add(string AccountField)
        {
            try
            {
                if (OutlookSearch.IsAccountValid(AccountField))
                {
                    var account = new AccountConfig()
                    {
                        EmailAddress = AccountField,
                        IsConfigured = true
                    };

                    if (account != null)
                        Accounts.Add(account);
                } else
                {
                    MessageBox.Show("Failed to add account");
                }

            }
            catch (Exception ex)
            {
                Logger.Log($"Error {ex.Message}");
            }
        }

    }
}
