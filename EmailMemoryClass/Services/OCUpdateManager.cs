using Microsoft.Win32;
using Squirrel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace EmailMemoryClass
{
    public class OCUpdateManager
    {
        readonly System.Timers.Timer _timer;
        public EventHandler OnStart;
        public bool _isUpdateRunning = false;
        public string type;
        public static string repoUrl = "https://github.com/dashwort/OutlookClipboard";

        public OCUpdateManager()
        {
            _timer = new System.Timers.Timer(360 * 1000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
            type = "auto";

            OnStart += OnManagerStart;
            OnStart?.Invoke(this, EventArgs.Empty);
        }

        async void OnManagerStart(object sender, EventArgs e)
        {
            try
            {
                Logger.Log("Calling update manager on start event", "Update");
                await CheckForUpdatesGithub();
            }
            catch (Exception)
            {
                Logger.Log("Failed to build update object", "Error");
            }
        }

        async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!_isUpdateRunning)
                    await Task.Run(CheckForUpdatesGithub);
            }
            catch (Exception)
            {
                Logger.Log("Failed to build update object", "Error");
            }
        }

        public async Task CheckForUpdatesGithub()
        {
            _isUpdateRunning = true;

            using (var mgr = UpdateManager.GitHubUpdateManager(repoUrl))
            {
                try
                {
                    var updateInfo = await mgr.Result.CheckForUpdate();

                    if (updateInfo.ReleasesToApply.Any())
                    {
                        var versionCount = updateInfo.ReleasesToApply.Count;
                        Logger.Log($"{versionCount} update(s) found.", "Update");

                        Logger.Log("Downloading updates", "Update");
                        var updateResult = await mgr.Result.UpdateApp();

                        var versionWord = versionCount > 1 ? "versions" : "version";
                        var message = new System.Text.StringBuilder().AppendLine($"Your app is {versionCount} {versionWord} behind.").
                                                          AppendLine($"Your application will update to version {updateResult.Version.ToString()} on application restart.").ToString();

                        var result = MessageBox.Show(message, "Application Update", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        message = $"Download complete. Version {updateResult.Version} will take effect when App is restarted.";
                        Logger.Log(message, "Update");
                    }
                    else
                    {
                        if (type == "manual")
                        {
                            var UpdateResult = MessageBox.Show("No updates detected.", "Application Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        Logger.Log($"No updates detected {DateTime.Now}", "Update");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error whilst updating: {ex.Message}, {ex.InnerException}", "Error");
                }
                finally
                {
                    // Application really needs to be disposed, otherwise it will leak a mutex when closed prematurely. 
                    mgr.Result.Dispose();
                    mgr.Dispose();
                    _isUpdateRunning = false;
                }
            }
        }

        public static void RunAtStartup()
        {
            using (var mgr = new UpdateManager(repoUrl))
            {
                mgr.CreateRunAtWindowsStartupRegistry();
            }
        }

        public static void RemoveAtStartup()
        {
            using (var mgr = new UpdateManager(repoUrl))
            {
                mgr.RemoveRunAtWindowsStartupRegistry();
            }
        }
    }

    public static class UpdateManagerExtensions
    {
        private static RegistryKey OpenRunAtWindowsStartupRegistryKey() =>
            Registry.CurrentUser.OpenSubKey(
                "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public static void CreateRunAtWindowsStartupRegistry(this UpdateManager updateManager)
        {
            Logger.Log("Creating startup shortcut", "Update");
            using (var startupRegistryKey = OpenRunAtWindowsStartupRegistryKey())
                startupRegistryKey.SetValue(
                    updateManager.ApplicationName,
                    Path.Combine(updateManager.RootAppDirectory, $"{updateManager.ApplicationName}.exe"));
        }

        public static void RemoveRunAtWindowsStartupRegistry(this UpdateManager updateManager)
        {
            Logger.Log("Removing startup shortcut", "Update");
            using (var startupRegistryKey = OpenRunAtWindowsStartupRegistryKey())
                startupRegistryKey.DeleteValue(updateManager.ApplicationName);
        }
    }
}
