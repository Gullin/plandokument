using System.IO;
using System.Web;

namespace Plan.Plandokument
{
	public class Utility
	{
        protected static string logDirectory = "~/log/";
        protected static string zipDirectory = "~/zipTemp/";

        /// <summary>
        /// Kontrollerar om loggkatalog existerar annars skapar
        /// </summary>
        protected static void logDirectoryExist()
        {
            // Get the absolute path to the log directory, skapa katalogen om den inte finns
            string localLogDirectory = HttpContext.Current.Server.MapPath(logDirectory);
            if (!Directory.Exists(localLogDirectory))
            {
                Directory.CreateDirectory(localLogDirectory);
            }
        }

        /// <summary>
        /// Kontrollerar om temporär katalog för pakatering filer till zip existerar annars skapar
        /// </summary>
        protected static void zipDirectoryExist()
        {
            // Get the absolute path to the log directory, skapa katalogen om den inte finns
            string localZipDirectory = HttpContext.Current.Server.MapPath(zipDirectory);
            if (!Directory.Exists(localZipDirectory))
            {
                Directory.CreateDirectory(localZipDirectory);
            }
        }
    }
}