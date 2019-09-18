using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Plan.WindowsService
{
    class Config
    {
        public static string WatchedFolder { get; } = @"C:\dev\_sandbox\ThumnailImages\files";

        public static string WatchFilter { get; } = "*.tif";

        public static string ThumnailsFolder { get; } = @"C:\dev\_sandbox\ThumnailImages\files\Thumnails";

        public static string ThumnailsExtension { get; } = "jpg";

        public static string[] ThumnailsSuffixes { get; } = { "-l", "-s" };

        public static int ImageQuality { get; } = 75;

        public static int[] MaxDimensions { get; } = { 2000, 150 };
    }

    public class Watcher
    {
        public Watcher()
        {

        }

        public static void Init(FileSystemWatcher watcher)
        {
            watcher.Path = Config.WatchedFolder;

            // Villka förändringar som ska bevakas
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;

            watcher.Filter = Config.WatchFilter;

            // Lägger till event-hanterare
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;

            // Starta bevakning
            watcher.EnableRaisingEvents = true;
        }


        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            try
            {
                foreach (var file in Directory.EnumerateFiles(@Config.ThumnailsFolder, Path.GetFileNameWithoutExtension(e.Name) + "_thumnail*." + Config.ThumnailsExtension))
                {
                    LoggEvent.Logger.WriteEntry("Raderar fil: " + file, EventLogEntryType.Information, LoggEvent.LoggEventID++);
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                LoggEvent.Logger.WriteEntry("Vid radering av: " + e.FullPath + " uppstod felet - " + ex.Message, EventLogEntryType.Error, LoggEvent.LoggEventID++);
            }
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            if (!IsFileReady(e.FullPath)) return; //first notification the file is arriving

            LoggEvent.Logger.WriteEntry("Ändrad fil: " + e.FullPath, EventLogEntryType.Information, LoggEvent.LoggEventID++);
            string _newFile = Config.ThumnailsFolder + "\\" + Path.GetFileNameWithoutExtension(e.Name) + "_thumnail";


            if (Config.ImageQuality < 0 || Config.ImageQuality > 100)
                throw new ArgumentOutOfRangeException("Bildkvaliteten måste vara mellan 0 och 100.");


            try
            {
                using (FileStream fs = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read))
                {

                    // Komprimering
                    ImageCodecInfo pngEncoder = GetEncoderInfo(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, Config.ImageQuality);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    // Utkatalog
                    if (!Directory.Exists(Config.ThumnailsFolder))
                    {
                        LoggEvent.Logger.WriteEntry("Skapar katalog: " + Config.ThumnailsFolder, EventLogEntryType.Information, LoggEvent.LoggEventID++);
                        Directory.CreateDirectory(Config.ThumnailsFolder);
                    }

                    // Skapar Thumnails
                    try
                    {

                        using (Image _image = Image.FromStream(fs))
                        {
                            if (_image.Width > Config.MaxDimensions[0] || _image.Height > Config.MaxDimensions[0])
                            {
                                using (Image _newImage = ScaleImage(_image, Config.MaxDimensions[0], Config.MaxDimensions[0]))
                                {
                                    LoggEvent.Logger.WriteEntry("Skapar: " + _newFile + Config.ThumnailsSuffixes[0] + "." + Config.ThumnailsExtension, EventLogEntryType.Information, LoggEvent.LoggEventID++);
                                    _newImage.Save(_newFile + Config.ThumnailsSuffixes[0] + "." + Config.ThumnailsExtension, pngEncoder, myEncoderParameters);
                                }
                            }


                            if (_image.Width > Config.MaxDimensions[1] || _image.Height > Config.MaxDimensions[1])
                            {
                                using (Image _newImage = ScaleImage(_image, Config.MaxDimensions[1], Config.MaxDimensions[1]))
                                {
                                    LoggEvent.Logger.WriteEntry("Skapar: " + _newFile + Config.ThumnailsSuffixes[1] + "." + Config.ThumnailsExtension, EventLogEntryType.Information, LoggEvent.LoggEventID++);
                                    _newImage.Save(_newFile + Config.ThumnailsSuffixes[1] + "." + Config.ThumnailsExtension, pngEncoder, myEncoderParameters);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        LoggEvent.Logger.WriteEntry(ex.Message, EventLogEntryType.Error, LoggEvent.LoggEventID++);
                    }


                }
            }
            catch (Exception ex)
            {
                LoggEvent.Logger.WriteEntry(ex.Message, EventLogEntryType.Error, LoggEvent.LoggEventID++);
            }
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            FileInfo _oldFile = new FileInfo(e.OldFullPath);
            FileInfo _newFile = new FileInfo(e.FullPath);
            string _oldFileThumnailL = Config.ThumnailsFolder + "\\" + Path.GetFileNameWithoutExtension(_oldFile.Name) + "_thumnail" + Config.ThumnailsSuffixes[0] + "." + Config.ThumnailsExtension;
            string _oldFileThumnailS = Config.ThumnailsFolder + "\\" + Path.GetFileNameWithoutExtension(_oldFile.Name) + "_thumnail" + Config.ThumnailsSuffixes[1] + "." + Config.ThumnailsExtension;

            try
            {
                if (File.Exists(_oldFileThumnailL))
                {
                    File.Move(
                        _oldFileThumnailL,
                        Config.ThumnailsFolder + "\\" + Path.GetFileNameWithoutExtension(_newFile.Name) + "_thumnail" + Config.ThumnailsSuffixes[0] + "." + Config.ThumnailsExtension
                        );
                }
                if (File.Exists(_oldFileThumnailS))
                {
                    File.Move(
                    _oldFileThumnailS,
                    Config.ThumnailsFolder + "\\" + Path.GetFileNameWithoutExtension(_newFile.Name) + "_thumnail" + Config.ThumnailsSuffixes[1] + "." + Config.ThumnailsExtension
                    );
                }
            }
            catch (Exception ex)
            {
                LoggEvent.Logger.WriteEntry(ex.Message, EventLogEntryType.Error, LoggEvent.LoggEventID++);
            }
        }



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


        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            try
            {
                using (var graphics = Graphics.FromImage(newImage))
                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            }
            catch (Exception ex)
            {
                LoggEvent.Logger.WriteEntry(ex.Message, EventLogEntryType.Error, LoggEvent.LoggEventID++);
            }

            return newImage;
        }


        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}