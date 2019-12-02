using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Web;

namespace Plan.Plandokument
{
    public class PackagingZip : Utility
	{

        /// <summary>
        /// Paketerar önskade plandokument i zip-format
        /// </summary>
        /// <param name="files">Lista med sökvägar till plandokument</param>
        /// <param name="zipFileNamePart">Suffix för skapad zip-fil (kompletteras automatiskt med datum och tidsstämpel inkl. millisekunder för unikhet)</param>
        /// <returns>Sökväg till skapad zip-fil</returns>
        public string zipFiles(List<object> files, string zipFileNamePart)
        {
            DateTime startRun = DateTime.Now;

            try
            {
                zipDirectoryExist();

                deleteOldZipFiles();

                // zip-fil med path som skapas som arkiv
                string zipFile = zipFileNamePart.Replace("/", "_") + "-" + DateTime.Now.ToString("yyyyMMddTHHmmss.fff") + ".zip";
                string ZipFileToCreate = HttpContext.Current.Server.MapPath(
                    zipDirectory.Replace("~/", "") + zipFile
                    );

                // Om filen, mot förmodan, skulle existera raderas den.
                if (File.Exists(ZipFileToCreate))
                {
                    File.Delete(ZipFileToCreate); 
                }

                // Skapar zip-akrkiv och packar valda filer
                using (ZipArchive zip = ZipFile.Open(ZipFileToCreate, ZipArchiveMode.Update))
                {
                    foreach (String file in files)
                    {
                        zip.CreateEntryFromFile(
                            HttpContext.Current.Server.MapPath(file), 
                            file.Substring(file.LastIndexOf('/') + 1)
                            );
                    }

                    UtilityLog.Log($"Paket med plandokument skapat, {ZipFileToCreate} ({files.Count.ToString()} st. filer)", Utility.LogLevel.INFORM);
                }

                // Kontrollerar tiden det tar att paka filerna. Om kortare tid än i if-satsen fördröjs processen
                double runTime = DateTime.Now.Subtract(startRun).TotalMilliseconds;
                if (runTime < 2000.0)
                {
                    int sleepTime = Convert.ToInt32(Math.Ceiling(2000.0 - runTime));
                    System.Threading.Thread.Sleep(sleepTime);
                }

                return zipDirectory.Replace("~/", "") + zipFile;
            }
            catch (System.Exception ex)
            {
                throw;
            }

        }


        private void deleteOldZipFiles()
        {
            string[] zipFiles = Directory.GetFiles(HttpContext.Current.Server.MapPath(zipDirectory), "*.zip", SearchOption.TopDirectoryOnly);

            // För alla zip-filer
            foreach (string zipFile in zipFiles)
            {
                string timeSuffixInZiptemp = zipFile.Substring(zipFile.Length - 23);
                string timeSuffixInZip = timeSuffixInZiptemp.Substring(0, 19);
                DateTime dateTimeStamp;
                if (DateTime.TryParseExact(timeSuffixInZip, "yyyyMMddTHHmmss.fff", new CultureInfo("sv-SE"), DateTimeStyles.None, out dateTimeStamp))
                {
                    // Raderar z-fil om äldre än 12 h
                    if (DateTime.Now.Subtract(dateTimeStamp).TotalHours > 12.0)
                    {
                        if (File.Exists(zipFile))
                        {
                            FileInfo file = new FileInfo(zipFile);
                            file.Delete();

                            UtilityLog.Log($"Paket med plandokument raderat, {zipFile}", Utility.LogLevel.INFORM);
                        }
                    }
                }
            }

        }


	}
}