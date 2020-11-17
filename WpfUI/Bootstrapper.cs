using Caliburn.Micro;
using EmailMemoryClass;
using EmailMemoryClass.Configuration;
using EmailMemoryClass.outlookSearch;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using WpfUI.ViewModels;

namespace WpfUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private static SettingsContainer _applicationConfiguration;
        private static OCUpdateManager _updateManager;

        public static SettingsContainer ApplicationConfiguration
        {
            get { return _applicationConfiguration; }
            set { _applicationConfiguration = value; }
        }

        public static OCUpdateManager UpdateManager
        {
            get { return _updateManager; }
            set { _updateManager = value; }
        }

        public Bootstrapper()
        {
            Bootstrapper.ApplicationConfiguration = new SettingsContainer(true);
            Bootstrapper.UpdateManager = new OCUpdateManager();
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        public static void RestartApplication()
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C choice /C Y /N /D Y /T 1 & START \"\" \"" + Assembly.GetEntryAssembly().Location + "\"";
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
            Info.FileName = "cmd.exe";
            Process.Start(Info);
            Process.GetCurrentProcess().Kill();
        }

    }
}
