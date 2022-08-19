using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plan.Shared.Thumnails
{
    /// <summary>
    /// Håller inställningar för windows-tjänsten. Både hårdkodat och hämtade från AppSettings.
    /// </summary>
    public class ConfigShared
    {
        public static string WatchedFolder { get; } = @ConfigurationManager.AppSettings["WatchedFolder"].ToString();

        public static string WatchFilter { get; } = "*.tif";

        public static string ThumnailsFolder { get; } = @ConfigurationManager.AppSettings["ThumnailsFolder"].ToString();

        public static string ThumnailsExtension { get; } = "jpg";

        public static string[] ThumnailsSuffixes { get; } = { "-l", "-s" };

        public static int ImageQuality { get; } = 75;

        public static int[] MaxDimensions { get; } = { 2000, 150 };
    }


    public class ManageImages
    {
        /// <summary>
        /// Skapar två tumnagelbilder (mindre mer komprimerade kopier av inskickad bild), en mindre och en större.
        /// </summary>
        /// <param name="e">Resultat från en event när fil ändras.</param>
        public static void CreateThumnailFiles(FileSystemEventArgs e)
        {
            CreateThumnailFiles(e.Name);
        }

        /// <summary>
        /// Skapar två tumnagelbilder (mindre mer komprimerade kopier av inskickad bild), en mindre och en större.
        /// </summary>
        /// <param name="filePath">Fullständig sökväg till bild som är underlag till tumnagelbilder.</param>
        /// <exception cref="ArgumentOutOfRangeException">Komprimering till tumnagelbild måste vara mellan 0 och 100.</exception>
        public static void CreateThumnailFiles(string filePath)
        {
            string _newFile = ConfigShared.ThumnailsFolder + "\\" + Path.GetFileNameWithoutExtension(filePath) + "_thumnail";


            if (ConfigShared.ImageQuality < 0 || ConfigShared.ImageQuality > 100)
                throw new ArgumentOutOfRangeException("Bildkvaliteten måste vara mellan 0 och 100.");


            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {

                    // Komprimering
                    ImageCodecInfo pngEncoder = GetEncoderInfo(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, ConfigShared.ImageQuality);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    // Utkatalog
                    if (!Directory.Exists(ConfigShared.ThumnailsFolder))
                    {
                        Directory.CreateDirectory(ConfigShared.ThumnailsFolder);
                    }

                    // Skapar Thumnails
                    try
                    {

                        using (Image _image = Image.FromStream(fs))
                        {
                            if (_image.Width > ConfigShared.MaxDimensions[0] || _image.Height > ConfigShared.MaxDimensions[0])
                            {
                                using (Image _newImage = ScaleImage(_image, ConfigShared.MaxDimensions[0], ConfigShared.MaxDimensions[0]))
                                {
                                    _newImage.Save(_newFile + ConfigShared.ThumnailsSuffixes[0] + "." + ConfigShared.ThumnailsExtension, pngEncoder, myEncoderParameters);
                                }
                            }


                            if (_image.Width > ConfigShared.MaxDimensions[1] || _image.Height > ConfigShared.MaxDimensions[1])
                            {
                                using (Image _newImage = ScaleImage(_image, ConfigShared.MaxDimensions[1], ConfigShared.MaxDimensions[1]))
                                {
                                    _newImage.Save(_newFile + ConfigShared.ThumnailsSuffixes[1] + "." + ConfigShared.ThumnailsExtension, pngEncoder, myEncoderParameters);
                                }
                            }
                        }

                    }
                    catch
                    {
                        throw;
                    }


                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Skalar bilden proportionellt
        /// </summary>
        /// <param name="image">Bild</param>
        /// <param name="maxWidth">Maximal bredd som önskas</param>
        /// <param name="maxHeight">Maximal höjd som önskas</param>
        /// <returns>Bild</returns>
        private static Image ScaleImage(Image image, int maxWidth, int maxHeight)
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
            catch
            {
                throw;
            }

            return newImage;
        }



        /// <summary>
        /// Returnerar bildformatets Codec
        /// </summary>
        /// <param name="format">Bildformat som Codec önskas för</param>
        /// <returns>Bildformatets Codec</returns>
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
