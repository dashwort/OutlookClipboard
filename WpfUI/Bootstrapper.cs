using Caliburn.Micro;
using EmailMemoryClass;
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
        private static AccountContainer _accounts;

        public static AccountContainer AccountConfiguration
        {
            get { return _accounts; }
            set { _accounts = value; }
        }

        public Bootstrapper()
        {
            Bootstrapper.AccountConfiguration = new AccountContainer(true);
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
