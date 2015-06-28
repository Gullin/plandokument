using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using Ionic.Zip;
using System.Linq;
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

                string ZipFileToCreate = string.Empty;
                using (ZipFile zip = new ZipFile())
                {
                    foreach (String file in files)
                    {
                        ZipEntry e = zip.AddFile(HttpContext.Current.Server.MapPath(file), "");
                    }

                    zip.Comment = String.Format("This zip archive was created by the CreateZip example application on machine '{0}'",
                       System.Net.Dns.GetHostName());

                    ZipFileToCreate = zipDirectory.Replace("~/", "") + zipFileNamePart.Replace("/", "_") + "-" + DateTime.Now.ToString("yyyyMMddTHHmmss.fff") + ".zip";

                    zip.Save(HttpContext.Current.Server.MapPath(ZipFileToCreate));

                }

                // Kontrollerar tiden det tar att paka filerna. Om kortare tid än i if-satsen fördröjs processen
                double runTime = DateTime.Now.Subtract(startRun).TotalMilliseconds;
                if (runTime < 2000.0)
                {
                    int sleepTime = Convert.ToInt32(Math.Ceiling(2000.0 - runTime));
                    System.Threading.Thread.Sleep(sleepTime);
                }

                return ZipFileToCreate;

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
                        }
                    }
                }
            }

        }


	}
}