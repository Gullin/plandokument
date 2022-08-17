using System.IO;
using System.Web;

namespace Plan.Plandokument
{
	public class Utility
	{
        protected static string appPath = HttpRuntime.AppDomainAppPath;
        //public static string logDirectory = "~/log/";
        public static string logDirectory = appPath + "log\\";
        //protected static string zipDirectory = appPath + "zipTemp\\";
        // Behöver vara virtuell/relativ p.g.a. att den används i länkning på webbsida
        protected static string zipDirectory = "~/zipTemp/";
        public enum LogLevel
        {
            INFORM,
            WARN,
            ERROR,
            STATS
        }


        /// <summary>
        /// Kontrollerar om loggkatalog existerar annars skapar
        /// </summary>
        protected static void logDirectoryExist()
        {
            // Get the absolute path to the log directory, skapa katalogen om den inte finns
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
        }

        /// <summary>
        /// Kontrollerar om temporär katalog för pakatering filer till zip existerar annars skapar
        /// </summary>
        protected static void zipDirectoryExist()
        {
            string zipPathToCreate = appPath + zipDirectory.Replace("~/", "").Replace(@"/", "\\");

            // Get the absolute path to the log directory, skapa katalogen om den inte finns
            if (!Directory.Exists(zipPathToCreate))
            {
                Directory.CreateDirectory(zipPathToCreate);
            }
        }
    }
}