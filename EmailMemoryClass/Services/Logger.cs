using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailMemoryClass
{
    public class Logger
    {

        public string FilePathLocation { get; private set; } 
        public string ShortName { get; private set; } = "\\EventLog";
        public string Extension { get; private set; } = ".csv";
        public string FileName { get; private set; }
        public int MaxFiles { get; private set; } = 9;
        public int MaxSize { get; private set; } = 10 * 1024 * 1024; // (10mb) File to big? Create new

        public static List<string> entriesToWrite = new List<string>();

        public static bool isRunning = false;

        public char Delimiter = '\t';

        private static Stopwatch stopwatch;

        public Logger()
        {
            FileName = ShortName + Extension;
            FilePathLocation = Logger.CalculateConfigPath();
        }

        public static string CalculateConfigPath()
        {
            var appdata = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var currentuser = Environment.UserName;
            return $"{appdata}\\OutlookClipboard\\{currentuser}";
        }

        public static void Log(string input, string type = "Info")
        {
            // slightly wasteful to allocate a new object each time but object allocation isnt that intensive
            try
            {
                Logger LogFile = new Logger();
                Console.WriteLine(type + ":" + input);

                LogFile.CheckLogFile(LogFile.FilePathLocation + LogFile.FileName);
                LogFile.LogEvent(input, type);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when writing log file, heres the error: {ex.Message} \nInner Exception:{ex.InnerException}");
            }
        }


        /// <summary>
        /// used for measuring the time of event and writing to log file
        /// </summary>
        /// <param name="path"></param>
        public static void StartClock()
        {
            Logger.stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public static void StopClock(string methodName)
        {
            stopwatch.Stop();
            Logger.Log(methodName + " action took " + stopwatch.ElapsedMilliseconds.ToString() + "ms", "Verbose");
        }

        public void CheckLogFile(string path)
        {
            List<string> FilesInDir = new List<string>(); // list of files in directory that match EventLog

            if (!Directory.Exists(FilePathLocation))
            {
                Directory.CreateDirectory(FilePathLocation);
            }

            if (!File.Exists(path))
            {
                File.AppendAllText(path, $"Date{Delimiter} Event{Delimiter} Type\n");
            }


            if (File.ReadAllBytes(path).Length >= MaxSize) // (10mb) File to big? Create new
            {
                foreach (string file in System.IO.Directory.GetFiles(FilePathLocation, "*")) // return all files in directory
                {
                    if (System.IO.Path.GetFileNameWithoutExtension(file) == ShortName)
                    {
                        FilesInDir.Add(Path.GetFileNameWithoutExtension(file)); //add file to list
                    }
                }

                // delete oldest file
                if (File.Exists(FilePathLocation + ShortName + $".{(MaxFiles - 1)}"))
                {
                    File.Delete(FilePathLocation + ShortName + $".{(MaxFiles - 1)}");
                }

                for (int i = (MaxFiles - 2); i >= 1; i--)
                {
                    string log = FilePathLocation + ShortName + $".{i}";
                    string Incrementlog = FilePathLocation + ShortName + $".{(i + 1)}";

                    if (File.Exists(log))
                    {
                        File.Move(log, Incrementlog);
                    }
                }

                File.Move(path, (FilePathLocation + ShortName + ".1"));
            }
        }

        public static bool FileNotInUse(string path)
        {
            try
            {
                bool canwrite = false;

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    canwrite = fs.CanWrite;
                }

                return canwrite;
            }
            catch (Exception)

            {
                return false;
            }
        }

        public void LogEvent(string Entry, string type)
        {
            try
            {
                string Date = FormatStatisticsDate();
                Entry = $"{Date}{Delimiter} {Entry}{Delimiter} {type} \r";

                if (!File.Exists(FilePathLocation + FileName))
                {
                    File.AppendAllText(FilePathLocation + FileName, $"Date{Delimiter} Event{Delimiter} Type");
                }

                entriesToWrite.Add(Entry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static string FormatStatisticsDate()
        {
            DateTime now = DateTime.UtcNow;
            return now.ToString("dd/MMM/yyyy hh:mm:ss");
        }

        public static async Task CheckForEntriesAsync()
        {
            if (!isRunning)
            {
                Logger.isRunning = true;
                var log = new Logger();

                while (entriesToWrite.Count > 10)
                {
                    try
                    {
                        var line = entriesToWrite[entriesToWrite.Count - 1];

                        if (await log.WriteAsync(line))
                            entriesToWrite.RemoveAt(entriesToWrite.Count - 1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error writing to log file in async task. Error {ex.Message}");
                    }
                }

                Logger.isRunning = false;
            }
            else
                Console.WriteLine("write task is already running");
        }

        private async Task<bool> WriteAsync(string Entry)
        {
            if (FileNotInUse(FilePathLocation + FileName))
            {
                await Task.Run(() => File.AppendAllText((FilePathLocation + FileName), Entry));
                return true;
            }

            return false;
        }
    }
}
