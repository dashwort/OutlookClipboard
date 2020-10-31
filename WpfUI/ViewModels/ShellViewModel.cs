using Caliburn.Micro;
using EmailMemoryClass;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using WpfUI.Models;

namespace WpfUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        #region fields
        private List<DisplayContainer> _views;
        private AppConfigViewModel _configWindow = new AppConfigViewModel();
        private ApplicationSettingsViewModel _appConfig = new ApplicationSettingsViewModel();
        private WindowManager WindowManager = new WindowManager();

        private DisplayContainer _primary;
        private DisplayContainer _secondary;
        private DisplayContainer _tertiary;
        private string _title;
        #endregion

        #region Properties
        public List<DisplayContainer> AllViews
        {
            get { return _views; }
            set { _views = value; }
        }

        public DisplayContainer Primary
        {
            get { return _primary; }
            set 
            { 
                _primary = value;
                NotifyOfPropertyChange();
            }
        }
        public DisplayContainer Secondary
        {
            get { return _secondary; }
            set 
            { 
                _secondary = value;
                NotifyOfPropertyChange();
            }
        }
        public DisplayContainer Tertiary
        {
            get { return _tertiary; }
            set 
            { 
                _tertiary = value;
                NotifyOfPropertyChange();
            }
        }

        public string Title
        {
            get { return _title; }
            set 
            { 
                _title = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        #region ICommandWindowsTaskbar

        public ICommand DoubleClickCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => ShowAndHide(),
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }

        public ICommand LeftClickCommand
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => ShowAndHide(),
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }

        public ICommand ExitApplication
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => Application.Current.Shutdown(0),
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }

        public ICommand ResetPosition
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => ResetWindowPosition(),
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }

        public ICommand OpenLogDirectory
        {
            get
            {
                return new DelegateCommand
                {
                    CommandAction = () => OpenLogFolder()
                };
            }
        }

        #endregion

        #region constructor
        public ShellViewModel()
        {
            LoadViews();
            DisplayViews();
            UpdateVersionNumber();
            ActivateItem(Primary.View);
        }
        #endregion

        #region events
        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            RegisterHotkeySetup();
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewReady(view);

            //if (Application.Current.MainWindow != null)
            //    Application.Current.MainWindow.Visibility = Visibility.Hidden;
        }

        protected override void OnDeactivate(bool close)
        {
            if (close)
                UnregisterHotKey();

            base.OnDeactivate(close);
        }
        #endregion

        #region Methods

        void ResetWindowPosition()
        {
            try
            {
                Application.Current.MainWindow.Left = SystemParameters.PrimaryScreenWidth / 2;
                Application.Current.MainWindow.Top = SystemParameters.PrimaryScreenHeight / 2;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        void ShowAndHide()
        {
            switch (Application.Current.MainWindow.Visibility)
            {
                case Visibility.Visible:
                    Application.Current.MainWindow.Hide();
                    break;
                case Visibility.Hidden:
                    Application.Current.MainWindow.Show();
                    break;
                case Visibility.Collapsed:
                    Application.Current.MainWindow.Show();
                    break;
                default:
                    Application.Current.MainWindow.Show();
                    break;
            }
        }

        private void UpdateVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Title = $"Outlook Clipboard v.{versionInfo.FileVersion}";
        }

        private void OpenLogFolder()
        {
            try
            {
                Process.Start("explorer.exe", Logger.CalculateConfigPath());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, "Error");
            }
            
        }

        void DisplayViews()
        {
            var defaultView = new DisplayContainer();

            Primary = defaultView;
            Secondary = defaultView;
            Tertiary = defaultView;

            foreach (var View in AllViews)
            {
                switch (View.DisplayIndex)
                {
                    case 1:
                        Primary = View;
                        break;
                    case 2:
                        Secondary = View;
                        break;
                    case 3:
                        Tertiary = View;
                        break;
                    default:
                        Primary = View;
                        break;
                }
            }
        }

        void LoadViews()
        {
            if(AllViews == null)
                AllViews = new List<DisplayContainer>();

            if(Bootstrapper.AccountConfiguration.Accounts.Count > 0)
            {
                foreach (var account in Bootstrapper.AccountConfiguration.Accounts)
                {
                    AllViews.Add(new DisplayContainer(account));
                }
            } else
            {
                ShowConfigurationWizard();
            }
        }

        private void ShowConfigurationWizard()
        {
            using (var window = new InitialStartupViewModel())
            {
                WindowManager.ShowDialog(window);

                TryClose();
            }
        }

        public void LoadView()
        {
            try
            {
                if (Primary.ToLoad)
                    ActivateItem(Primary.View);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        public void LoadView2()
        {
            try
            {
                if(Secondary.ToLoad)
                    ActivateItem(Secondary.View);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        public void LoadView3()
        {
            try
            {
                if (Tertiary.ToLoad)
                    ActivateItem(Tertiary.View);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }

        public void LoadConfiguration()
        {
            try
            {
                ActivateItem(_configWindow);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
        }
        #endregion

        #region globalHotHotkeys
        private const uint MOD_NONE = 0x0000; //[NONE]
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS
        private HwndSource _source;
        private const int HOTKEY_ID = 9000;

        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey([In] IntPtr hWnd,[In] int id,[In] uint fsModifiers,[In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey([In] IntPtr hWnd,[In] int id);

        // called when hotkey is pressed
        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            ShowAndHide();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        void RegisterHotkeySetup()
        {
            if (Application.Current.MainWindow != null)
            {
                var helper = new WindowInteropHelper(Application.Current.MainWindow);
                _source = HwndSource.FromHwnd(helper.Handle);
                _source.AddHook(HwndHook);
                RegisterHotKey();
            }
        }

        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(Application.Current.MainWindow);
            const uint VK_F10 = 0x79;
            const uint MOD_CTRL = 0x0002;

            if (!RegisterHotKey(helper.Handle, HOTKEY_ID, MOD_CTRL, VK_F10))
            {
                Logger.Log("Warning unable to register hotkey");
            }
        }

        private void UnregisterHotKey()
        {
            //var helper = new WindowInteropHelper(Application.Current.MainWindow);
            //UnregisterHotKey(helper.Handle, HOTKEY_ID);
            //Logger.Log("Unregistering hotkeys");
        }
        #endregion
    }

    public class DelegateCommand : ICommand
    {
        public System.Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
