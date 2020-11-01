using Caliburn.Micro;
using EmailMemoryClass;
using System;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfUI.ViewModels
{
    public class EmailListViewModel : Screen
    {
        #region fields
        private BindableCollection<Email> _mailItems = new BindableCollection<Email>();
        private Email _selectedEmail;
        private OutlookSearch _account;
        private System.Timers.Timer _statusTimer;
        private string _status;
        private int _displayIndex;
        private string _configIcon;
        private string _copyIcon;
        private string _pauseIcon;
        SolidColorBrush _statusColour;
        private string _timeRemaining;
        #endregion

        #region properties
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
                try
                {
                    _status = value;
                    NotifyOfPropertyChange();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                }
              
            }
        }

        public SolidColorBrush StatusColour
        {
            get { return _statusColour; }
            set
            {
                try
                {
                    _statusColour = value;
                    NotifyOfPropertyChange();
                }
                catch (Exception ex)
                {
                    Logger.Log(ex.Message);
                }

            }
        }

        public string ConfigIcon
        {
            get { return _configIcon; }
            set
            {
                _configIcon = value;
                NotifyOfPropertyChange();
            }
        }

        public string CopyIcon
        {
            get { return _copyIcon; }
            set
            {
                _copyIcon = value;
                NotifyOfPropertyChange();
            }
        }

        public string PauseIcon
        {
            get { return _pauseIcon; }
            set
            {
                _pauseIcon = value;
                NotifyOfPropertyChange();
            }
        }

        public int DisplayIndex
        {
            get { return _displayIndex; }
            set { _displayIndex = value; }
        }

        public string TimeRemaining
        {
            get { return _timeRemaining; }
            set 
            { 
                _timeRemaining = value;
                NotifyOfPropertyChange();
            }
        }
        #endregion


        public EmailListViewModel(AccountConfig account)
        {
            if(account != null)
            {
                Account = new OutlookSearch(account);
                Account.OnSearchComplete += EmailSearchComplete;

                _statusTimer = new System.Timers.Timer(1000) { AutoReset = true };
                _statusTimer.Elapsed += UITimerElapsed;
                _statusTimer.Start();

                DisplayIndex = account.DisplayIndex;
                DisplayName = account.DisplayName;

                _account.OnSearchErrorOccurred += OutlookErrorOccurred;
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }
        }

        #region Events
        public void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CopyLastFwdBody();
        }
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            OutlookErrorOccurred(this, e);
        }

        private void UITimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateStatus();
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            try
            {
                var timeRemaining = Math.Round(_account._timer.TimeLeft/1000).ToString();
                TimeRemaining = $"Time Remaining: {timeRemaining}";
            }
            catch (Exception ex)
            {
                Logger.Log("Failed to update UI timer","Error");
            }
            
        }

        private void EmailSearchComplete(object sender, EventArgs e)
        {
            if (Account.EmailsFound.Count != 0)
            {
                MailItems.Clear();
                MailItems.AddRange(Account.EmailsFound);
            }
            else
            {
                MailItems.Add(new Email() { Subject = "No emails detected - review settings" });
            }
        }

        private void OutlookErrorOccurred(object sender, EventArgs e)
        {
            _account.OutlookErrorOccurred(sender);
        }

        #endregion

        #region methods
        public void CopySR()
        {
            try
            {
                Clipboard.SetText(SelectedEmail.SRNumber);
                Status = "Copied SR to clipboard";
                StatusColour = Brushes.Green;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        public void CopyLastFwdBody()
        {
            try
            {
                if(SelectedEmail != null)
                {
                    Clipboard.SetText(SelectedEmail.LastMailAsFwd);
                    Status = "Copied last email to clipboard";
                    StatusColour = Brushes.Green;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        public void CopyFullBody()
        {
            try
            {
                if (SelectedEmail != null)
                {
                    Clipboard.SetText(SelectedEmail.BodyText);
                    Status = "Copied full trail to clipboard";
                    StatusColour = Brushes.Green;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        public async Task OpenInOutlook()
        {
            try
            {
                if (SelectedEmail != null)
                {
                    await Task.Run(() => Account.FindEmail(SelectedEmail));
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        private void UpdateStatus()
        {
            if (_account.HasError)
            {
                Status = "Error occured, attempting recovery...";
                StatusColour = Brushes.Red;
            }
            else if (_account.HasFirstRunComplete)
            {
                Status = "Loading..... Please wait";
                StatusColour = Brushes.Black;
            }
            else if (_account._isSearchRunning)
            {
                Status = "Searching... Please wait";
                StatusColour = Brushes.Black;
            }
            else
            {
                Status = string.Empty;
                StatusColour = Brushes.Black;
            }
        }

        public void Minimise()
        {
            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Hide();
        }

        protected override void OnDeactivate(bool close)
        {
            _account.Dispose();

            base.OnDeactivate(close);
        }
        #endregion
    }
}
