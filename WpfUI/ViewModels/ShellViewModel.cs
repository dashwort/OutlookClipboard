using Caliburn.Micro;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
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
        private List<DisplayContainer> _views;
        private AppConfigViewModel _configWindow = new AppConfigViewModel();
        private WindowManager WindowManager = new WindowManager();

        private DisplayContainer _primary;
        private DisplayContainer _secondary;
        private DisplayContainer _tertiary;
        //private Window _keyWindowInstance = new Window();

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

        #endregion

        #region constructor
        public ShellViewModel()
        {
            RegisterEvents();
            LoadViews();
            DisplayViews();
            ActivateItem(Primary.View);
        }

        void RegisterEvents()
        {
            
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
                Console.WriteLine(ex.Message);
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

        protected override void OnViewLoaded(object view)
        {
            base.OnViewReady(view);

            if (Application.Current.MainWindow != null)
                Application.Current.MainWindow.Visibility = Visibility.Hidden;
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
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
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
                Console.WriteLine(ex.Message);
            }
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
