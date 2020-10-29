using EmailMemoryClass;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace WpfUI
{
    public class OCUpdateManager
    {
        readonly System.Timers.Timer _timer;
        public EventHandler OnStart;
        public bool _isUpdateRunning = false;
        public string type;

        public OCUpdateManager()
        {
            _timer = new System.Timers.Timer(60 * 1000) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
            type = "auto";

            OnStart += OnManagerStart;
            OnStart?.Invoke(this, EventArgs.Empty);
        }

        async void OnManagerStart(object sender, EventArgs e)
        {
            Logger.Log("Calling update manager on start event");

            await CheckForUpdatesGithub();
        }

        async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (!_isUpdateRunning)
                await Task.Run(CheckForUpdatesGithub);
        }

        public async Task CheckForUpdatesGithub()
        {
            string repoUrl = "https://github.com/dashwort/OutlookClipboard";
            _isUpdateRunning = true;

            using (var mgr = UpdateManager.GitHubUpdateManager(repoUrl))
            {
                try
                {
                    var updateInfo = await mgr.Result.CheckForUpdate();

                    if (updateInfo.ReleasesToApply.Any())
                    {
                        var versionCount = updateInfo.ReleasesToApply.Count;
                        Logger.Log($"{versionCount} update(s) found.");

                        Logger.Log("Downloading updates");
                        var updateResult = await mgr.Result.UpdateApp();

                        var versionWord = versionCount > 1 ? "versions" : "version";
                        var message = new System.Text.StringBuilder().AppendLine($"Your app is {versionCount} {versionWord} behind.").
                                                          AppendLine($"Your application will update to version {updateResult.Version.ToString()} on application restart.").ToString();

                        var result = MessageBox.Show(message, "Application Update", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        message = $"Download complete. Version {updateResult.Version} will take effect when App is restarted.";
                        Logger.Log(message);
                    }
                    else
                    {
                        if (type == "manual")
                        {
                            var UpdateResult = MessageBox.Show("No updates detected.", "Application Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        Logger.Log($"No updates detected {DateTime.Now}");
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
    }
}
