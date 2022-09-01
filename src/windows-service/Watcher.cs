using Plan.Shared.Thumnails;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace Plan.WindowsService
{

    /// <summary>
    /// Håller inställningar för windows-tjänsten. Både hårdkodat och hämtade från AppSettings.
    /// </summary>
    class ConfigWatcher
    {
        public static string WatchedFolder { get; } = @ConfigurationManager.AppSettings["WatchedFolder"].ToString();

        public static string WatchFilter { get; } = "*.tif";

        public static string ThumnailsFolder { get; } = @ConfigurationManager.AppSettings["ThumnailsFolder"].ToString();

        public static string ThumnailsExtension { get; } = "jpg";

        public static string[] ThumnailsSuffixes { get; } = { "-l", "-s" };

        public static int ImageQuality { get; } = 75;

        public static int[] MaxDimensions { get; } = { 2000, 150 };
    }


    /// <summary>
    /// Hanterar katalogbevakningen
    /// </summary>
    public class Watcher
    {
        /// <summary>
        /// Initierar bevakning av katalog för ändrade filer.
        /// Katalog tas från konfiguration.
        /// </summary>
        /// <param name="watcher">Filbevakningsobjekt</param>
        public static void Init(FileSystemWatcher watcher)
        {
            //watcher.Path = ConfigShared.WatchedFolder;
            watcher.Path = Utility.GetServiceAppSettings("WatchedFolder");

            // Villka förändringar som ska bevakas
            watcher.NotifyFilter = NotifyFilters.LastWrite
                                 | NotifyFilters.FileName;

            watcher.Filter = ConfigWatcher.WatchFilter;

            //watcher.InternalBufferSize = 65536; // 64 KB råder docs.microsoft ska vara max (4 KB är minimum)
            watcher.InternalBufferSize = 1048576; // 1 MB


            // Lägger till event-hanterare
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnDeleted);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.Error += new ErrorEventHandler(OnError);
            watcher.Disposed += new EventHandler(OnDisposed);

            // Starta bevakning
            watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Event för när fil raderas. Båda humnails bilder large (l) och small (s) raderas
        /// </summary>
        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (!Directory.Exists(Utility.GetServiceAppSettings("ThumnailsFolder")))
            {
                return;
            }

            try
            {
                //foreach (var file in Directory.EnumerateFiles(ConfigShared.ThumnailsFolder, Path.GetFileNameWithoutExtension(e.Name) + "_thumnail*." + ConfigShared.ThumnailsExtension))
                foreach (var file in Directory.EnumerateFiles(Utility.GetServiceAppSettings("ThumnailsFolder"), Path.GetFileNameWithoutExtension(e.Name) + "_thumnail*." + ConfigWatcher.ThumnailsExtension))
                {
                    LoggEvent.Logger.WriteEntry("Raderar fil: " + file, EventLogEntryType.Information, LoggEvent.LoggEventID++);
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                LoggEvent.Logger.WriteEntry("Vid radering av: " + e.FullPath + " uppstod felet - " + ex.Message, EventLogEntryType.Error, LoggEvent.LoggEventID++);
            }

            if (Directory.GetFiles(Utility.GetServiceAppSettings("ThumnailsFolder"), "*." + ConfigWatcher.ThumnailsExtension).Length == 0)
            {
                Directory.Delete(Utility.GetServiceAppSettings("ThumnailsFolder"), false);
            }
        }

        /// <summary>
        /// Event för när fil ändras. Thumnails Bildfiler skapas om.
        /// </summary>
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Thread.Sleep(500);

            if (!IsFileReady(e.FullPath)) return; //first notification the file is arriving

            LoggEvent.Logger.WriteEntry("Ändrad fil: " + e.FullPath, EventLogEntryType.Information, LoggEvent.LoggEventID++);
            try
            {
                ManageImages.CreateThumnailFiles(e);
                string _newFile = Utility.GetServiceAppSettings("ThumnailsFolder") + "\\" + Path.GetFileNameWithoutExtension(e.Name) + "_thumnail";
                LoggEvent.Logger.WriteEntry("Skapat: " + _newFile + ConfigWatcher.ThumnailsSuffixes[0] + "." + ConfigWatcher.ThumnailsExtension + " och " + _newFile + ConfigWatcher.ThumnailsSuffixes[1] + "." + ConfigWatcher.ThumnailsExtension, 
                    EventLogEntryType.Information, LoggEvent.LoggEventID++);
            }
            catch (Exception ex)
            {
                LoggEvent.Logger.WriteEntry(ex.Message, EventLogEntryType.Error, LoggEvent.LoggEventID++);
            }
        }

        /// <summary>
        /// Event för när fil döps om. Ändrar thumnails bildfilers namn för large (l) och small (s) därefter.
        /// </summary>
        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            FileInfo _oldFile = new FileInfo(e.OldFullPath);
            FileInfo _newFile = new FileInfo(e.FullPath);
            string _oldFileThumnailL = Utility.GetServiceAppSettings("ThumnailsFolder") + "\\" + Path.GetFileNameWithoutExtension(_oldFile.Name) + "_thumnail" + ConfigWatcher.ThumnailsSuffixes[0] + "." + ConfigWatcher.ThumnailsExtension;
            string _oldFileThumnailS = Utility.GetServiceAppSettings("ThumnailsFolder") + "\\" + Path.GetFileNameWithoutExtension(_oldFile.Name) + "_thumnail" + ConfigWatcher.ThumnailsSuffixes[1] + "." + ConfigWatcher.ThumnailsExtension;

            try
            {
                if (File.Exists(_oldFileThumnailL))
                {
                    File.Move(
                        _oldFileThumnailL,
                        Utility.GetServiceAppSettings("ThumnailsFolder") + "\\" + Path.GetFileNameWithoutExtension(_newFile.Name) + "_thumnail" + ConfigWatcher.ThumnailsSuffixes[0] + "." + ConfigWatcher.ThumnailsExtension
                        );
                }
                if (File.Exists(_oldFileThumnailS))
                {
                    File.Move(
                    _oldFileThumnailS,
                    Utility.GetServiceAppSettings("ThumnailsFolder") + "\\" + Path.GetFileNameWithoutExtension(_newFile.Name) + "_thumnail" + ConfigWatcher.ThumnailsSuffixes[1] + "." + ConfigWatcher.ThumnailsExtension
                    );
                }
            }
            catch (Exception ex)
            {
                LoggEvent.Logger.WriteEntry(ex.Message, EventLogEntryType.Error, LoggEvent.LoggEventID++);
            }
        }

        /// <summary>
        /// Event för när filbevakningsobjektet återvinns av GC:n. Loggar händelsen.
        /// </summary>
        private static void OnDisposed(object sender, EventArgs e)
        {
            LoggEvent.Logger.WriteEntry("FileSystemWatcher har återvunnits (disposed)", EventLogEntryType.Warning, LoggEvent.LoggEventID++);
        }

        /// <summary>
        /// Event för när filbevakningsobjektet returnerar fel. Felet loggas.
        /// </summary>
        private static void OnError(object sender, ErrorEventArgs e)
        {
            Exception ex = e.GetException();
            string errorType = ex.GetType().ToString();
            string errorMessage = ex.Message;
            LoggEvent.Logger.WriteEntry($"FileSystemWatcher meddelar fel ({errorType}): {errorMessage}", EventLogEntryType.Warning, LoggEvent.LoggEventID++);
        }

        /// <summary>
        /// Kontrollerar om fil är åtkomlig, indikerar att skrivningen av filen är färdig
        /// </summary>
        /// <param name="fullPath">Full sökväg till fil</param>
        /// <returns>Sant om filen är åtkomlig, annars falskt</returns>
        private static bool IsFileReady(string fullPath)
        {
            // Ett fel per fil istället för flera när polling-metoden används
            try
            {
                // Om filen inte kan öppnas används kopieras den fortfarande
                using (var file = File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
            catch (Exception ex)
            {
                LoggEvent.Logger.WriteEntry(ex.Message, EventLogEntryType.Error, LoggEvent.LoggEventID++);
                return false;
            }
        }
    }
}