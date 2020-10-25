using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media;
using Caliburn.Micro;
using EmailMemoryClass;

namespace WpfUI.ViewModels
{
    public class InitialStartupViewModel : Screen, IDisposable
    {
        private bool _complete;
        private System.Timers.Timer _statusTimer;
        private string _searchTag;


        private bool _canAdd;
        public bool CanAdd
        {
            get { return _canAdd; }
            set 
            { 
                _canAdd = value;
                NotifyOfPropertyChange();
            }
        }

        private string _account1;
        private string _account2;
        private string _account3;

        private Brush _account1Colour;
        private Brush _account2Colour;
        private Brush _account3Colour;

        private string _account1DisplayName;
        private string _account2DisplayName;
        private string _account3DisplayName;

        public string Account1Displayname 
        {
            get { return _account1DisplayName; }
            set 
            { 
                _account1DisplayName = value;
                NotifyOfPropertyChange();
            }
        }
        public string Account2Displayname
        {
            get { return _account2DisplayName; }
            set
            {
                _account2DisplayName = value;
                NotifyOfPropertyChange();
            }
        }
        public string Account3Displayname
        {
            get { return _account3DisplayName; }
            set
            {
                _account3DisplayName = value;
                NotifyOfPropertyChange();
            }
        }

        public Brush Account1Colour
        {
            get { return _account1Colour; }
            set 
            { 
                _account1Colour = value;
                NotifyOfPropertyChange();
            }
        }
        public Brush Account2Colour
        {
            get { return _account2Colour; }
            set
            {
                _account2Colour = value;
                NotifyOfPropertyChange();
            }
        }
        public Brush Account3Colour
        {
            get { return _account3Colour; }
            set
            {
                _account3Colour = value;
                NotifyOfPropertyChange();
            }
        }

        public string Account1
        {
            get { return _account1; }
            set 
            { 
                _account1 = value;
                NotifyOfPropertyChange();
            }
        }
        public string Account2
        {
            get { return _account2; }
            set
            {
                _account2 = value;
                NotifyOfPropertyChange();
            }
        }
        public string Account3
        {
            get { return _account3; }
            set
            {
                _account3 = value;
                NotifyOfPropertyChange();
            }
        }

        public bool Complete
        {
            get { return _complete; }
            set { _complete = value; }
        }

        public string SearchTag
        {
            get { return _searchTag; }
            set { _searchTag = value; }
        }

        public InitialStartupViewModel()
        {
            SetStartupValues();

            _statusTimer = new System.Timers.Timer(200) { AutoReset = true };
            _statusTimer.Elapsed += UITimerElapsed;
            _statusTimer.Start();
        }

        void UITimerElapsed(object sender, ElapsedEventArgs e)
        {
            SetColourStatus();
            CanSave();
        }

        void SetColourStatus()
        {
            Account1Colour = GetColourAndStatus(Account1, Account1Displayname);
            Account2Colour = GetColourAndStatus(Account2, Account2Displayname);
            Account3Colour = GetColourAndStatus(Account3, Account3Displayname);
        }

        Brush GetColourAndStatus(string account1, string account1Displayname)
        {
            Brush colour;

            if (string.IsNullOrEmpty(account1) && string.IsNullOrEmpty(account1Displayname))
            {
                colour = Brushes.Gray;
            } else if (!string.IsNullOrEmpty(account1) || !string.IsNullOrEmpty(account1Displayname))
            {
                if(OutlookSearch.IsValidEmail(account1) && !string.IsNullOrEmpty(account1Displayname))
                {
                    colour = Brushes.Green;
                } else
                {
                    colour = Brushes.Red;
                }
            } else
            {
                colour = Brushes.Red;
            }

           return colour;
        }

        void SetStartupValues()
        {
            Account1Colour = Brushes.Gray;
            Account2Colour = Brushes.Gray;
            Account3Colour = Brushes.Gray;
        }

        public void Save()
        {
            if (Account1Colour == Brushes.Green)
            {
                var accountConfig = new AccountConfig()
                {
                    EmailAddress = Account1,
                    DisplayName = Account1Displayname,
                    DisplayIndex = 1,
                    IsConfigured = true,
                    SearchTag = this.SearchTag
                };

                Bootstrapper.AccountConfiguration.Accounts.Add(accountConfig);
            }

            if (Account2Colour == Brushes.Green)
            {
                var accountConfig = new AccountConfig()
                {
                    EmailAddress = Account2,
                    DisplayName = Account2Displayname,
                    DisplayIndex = 2,
                    IsConfigured = true,
                    SearchTag = this.SearchTag
                };

                Bootstrapper.AccountConfiguration.Accounts.Add(accountConfig);
            }

            if (Account3Colour == Brushes.Green)
            {
                var accountConfig = new AccountConfig()
                {
                    EmailAddress = Account3,
                    DisplayName = Account3Displayname,
                    DisplayIndex = 3,
                    IsConfigured = true,
                    SearchTag = this.SearchTag
                };

                Bootstrapper.AccountConfiguration.Accounts.Add(accountConfig);
            }

            Bootstrapper.AccountConfiguration.SaveChanges();
            Complete = true;
        }

        public void CanSave()
        {
            if (Account1Colour != Brushes.Gray && Account1Colour == Brushes.Green)
            {
                CanAdd = true;
            }

            if(Account2Colour == Brushes.Red || Account3Colour == Brushes.Red)
            {
                CanAdd = false;
            }

            if (string.IsNullOrEmpty(SearchTag))
            {
                CanAdd = false;
            }
                
        }

        public void Dispose()
        {
            _statusTimer.Stop();
        }
    }
}
