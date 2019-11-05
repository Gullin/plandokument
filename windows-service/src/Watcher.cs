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
            watcher.Path = ConfigShared.WatchedFolder;

            // Villka förändringar som ska bevakas
            watcher.NotifyFilter = NotifyFilters.LastWrite
                                 | NotifyFilters.FileName;

            watcher.Filter = ConfigShared.WatchFilter;

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
            if (!Directory.Exists(ConfigShared.ThumnailsFolder))
            {
                return;
            }

            try
            {
                foreach (var file in Directory.EnumerateFiles(ConfigShared.ThumnailsFolder, Path.GetFileNameWithoutExtension(e.Name) + "_thumnail*." + ConfigShared.ThumnailsExtension))
                {
                    LoggEvent.Logger.WriteEntry("Raderar fil: " + file, EventLogEntryType.Information, LoggEvent.LoggEventID++);
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                LoggEvent.Logger.WriteEntry("Vid radering av: " + e.FullPath + " uppstod felet - " + ex.Message, EventLogEntryType.Error, LoggEvent.LoggEventID++);
            }

            if (Directory.GetFiles(ConfigShared.ThumnailsFolder, "*." + ConfigShared.ThumnailsExtension).Length == 0)
            {
                Directory.Delete(ConfigShared.ThumnailsFolder, false);
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
                string _newFile = ConfigShared.ThumnailsFolder + "\\" + Path.GetFileNameWithoutExtension(e.Name) + "_thumnail";
                LoggEvent.Logger.WriteEntry("Skapat: " + _newFile + ConfigShared.ThumnailsSuffixes[0] + "." + ConfigShared.ThumnailsExtension + " och " + _newFile + ConfigShared.ThumnailsSuffixes[1] + "." + ConfigShared.ThumnailsExtension, 
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
            string _oldFileThumnailL = ConfigShared.ThumnailsFolder + "\\" + Path.GetFileNameWithoutExtension(_oldFile.Name) + "_thumnail" + ConfigShared.ThumnailsSuffixes[0] + "." + ConfigShared.ThumnailsExtension;
            string _oldFileThumnailS = ConfigShared.ThumnailsFolder + "\\" + Path.GetFileNameWithoutExtension(_oldFile.Name) + "_thumnail" + ConfigShared.ThumnailsSuffixes[1] + "." + ConfigShared.ThumnailsExtension;

            try
            {
                if (File.Exists(_oldFileThumnailL))
                {
                    File.Move(
                        _oldFileThumnailL,
                        ConfigShared.ThumnailsFolder + "\\" + Path.GetFileNameWithoutExtension(_newFile.Name) + "_thumnail" + ConfigShared.ThumnailsSuffixes[0] + "." + ConfigShared.ThumnailsExtension
                        );
                }
                if (File.Exists(_oldFileThumnailS))
                {
                    File.Move(
                    _oldFileThumnailS,
                    ConfigShared.ThumnailsFolder + "\\" + Path.GetFileNameWithoutExtension(_newFile.Name) + "_thumnail" + ConfigShared.ThumnailsSuffixes[1] + "." + ConfigShared.ThumnailsExtension
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