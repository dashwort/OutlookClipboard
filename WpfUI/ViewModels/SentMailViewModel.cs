using Caliburn.Micro;
using EmailMemoryClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WpfUI.ViewModels
{
    public class SentMailViewModel : Screen
    {
        private BindableCollection<Email> _mailItems = new BindableCollection<Email>();
        private Email _selectedEmail;
        private OutlookSearch _account;
        private System.Timers.Timer _statusTimer;
        private string _status;

        public BindableCollection<Email> MailItems
        {
            get { return _mailItems; }
            set { _mailItems = value; }
        }

        public OutlookSearch Account
        {
            get { return _account; }
            set
            {
                _account = value;
                NotifyOfPropertyChange();
            }
        }

        public Email SelectedEmail
        {
            get { return _selectedEmail; }
            set
            {
                _selectedEmail = value;
                NotifyOfPropertyChange();
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyOfPropertyChange();
            }
        }

        public SentMailViewModel(string email)
        {
            Account = new OutlookSearch() { EmailAddress = email };
            Account.OnSearchComplete += EmailSearchComplete;

            _statusTimer = new System.Timers.Timer(200) { AutoReset = true };
            _statusTimer.Elapsed += UITimerElapsed;
            _statusTimer.Start();

            _account.OnSearchErrorOccurred += OutlookErrorOccurred;

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            OutlookErrorOccurred(this, e);
        }

        private void UITimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (_account.HasFirstRunComplete)
            {
                Status = "Loading..... Please wait";
            }
            else if (_account._isSearchRunning)
            {
                Status = "Searching... Please wait";
            }
            else
            {
                Status = string.Empty;
            }
        }

        private void ListView_MouseDoubleClick()
        {
            //double click item
            Console.WriteLine("EmailMemoryClass");
        }

        private void EmailSearchComplete(object sender, EventArgs e)
        {
            MailItems.Clear();
            MailItems.AddRange(Account.EmailsFound);
        }

        protected override void OnDeactivate(bool close)
        {
            _account.Dispose();

            base.OnDeactivate(close);
        }

        private void OutlookErrorOccurred(object sender, EventArgs e)
        {
            _account.OutlookErrorOccurred();
        }


    }
}
