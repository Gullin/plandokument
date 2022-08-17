using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Plan.Plandokument
{
	public class UtilityLog : Utility
	{

        // Standardvärden
        //private static string BaseFileNameDefult { get; } = "Audit";
        private static string _name;
        private static string fileName {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["auditLogFileName"].ToString()) ? "Audit" : ConfigurationManager.AppSettings["auditLogFileName"].ToString();
            }

            set { _name = value; }
        }
        public static string logFile = logDirectory + fileName + ".log";
        private static double MaxFileByteSizeDefault { get; } = 10;
        private static int MaxFilesTotalDeafult { get; } = 1;


        // All methods are static, so this can be private
        public UtilityLog()
        { }

        //TODO: Error-log, logga async/await
        // Log an Exception
        /// <summary>
        /// Loggar undantaget till fil och ev. e-postsignal om felet och bifogad loggfil
        /// </summary>
        /// <param name="logMessage">Meddelande som ska loggas.</param>
        /// <param name="logLevel">Loggningsnivå</param>
        public static void Log(string logMessage, LogLevel logLevel)
        {
            Logger(logMessage, DateTime.Now, logLevel);
        }

        /// <summary>
        /// Loggar undantaget till fil och ev. e-postsignal om felet och bifogad loggfil
        /// </summary>
        /// <param name="logMessage">Meddelande som ska loggas.</param>
        /// <param name="dateTimeStamp">Datum och tid för meddelandet.</param>
        /// <param name="logLevel">Loggningsnivå</param>
        public static void Log(string logMessage, DateTime dateTimeStamp, LogLevel logLevel)
        {
            Logger(logMessage, dateTimeStamp, logLevel);
        }


        private static void Logger(string logMessage, DateTime dateTimeStamp, LogLevel logLevel)
        {
            logDirectoryExist();

            // Get the absolute path to the log file, skapa logg-fil om den inte finns
            //string baseFileName = ConfigurationManager.AppSettings["auditLogFileName"].ToString();
            //if (string.IsNullOrEmpty(baseFileName))
            //{
            //    baseFileName = BaseFileNameDefult;
            //}
            //string logFile = logDirectory + fileName + ".log";
            if (!File.Exists(logFile))
            {
                using (FileStream fs = File.Create(logFile))
                {
                    fs.Close();
                }
            }

            // Tilldelar värde till maximal storlek för loggfiler
            double maxFileByteSize = MaxFileByteSizeDefault;
            if (double.TryParse(ConfigurationManager.AppSettings["auditLogFileMaxByteSize"].ToString(), out maxFileByteSize))
            {
                if (maxFileByteSize < 0.005)
                    maxFileByteSize = MaxFileByteSizeDefault;
            }
            maxFileByteSize = maxFileByteSize * 1024.0 * 1024.0;

            // Tilldelar värde för max antal loggfiler eller standardvärdet
            int maxFilesTotal = MaxFilesTotalDeafult;
            if (int.TryParse(ConfigurationManager.AppSettings["auditLogFileMaxTotal"].ToString(), out maxFilesTotal))
            {
                if (maxFilesTotal < 1)
                    maxFilesTotal = MaxFilesTotalDeafult;
            }

            // Läs filstorlek för basloggfil
            long fileSize;
            using (FileStream fileToReadSizeFrom = File.Open(logFile, FileMode.Open, FileAccess.Read))
            {
                fileSize = fileToReadSizeFrom.Length;
                fileToReadSizeFrom.Close();
            }

            // Om basfilen inte är för stor skriv till den, annars kopiera bort den och skapa ny tom basfil
            if (fileSize < maxFileByteSize)
            {
                // skriv till basfilen
                writeLoggTextToFile(logMessage, dateTimeStamp, logLevel, logFile);
            }
            else
            {
                //dela upp basfilen men ej till fler delfiler än inställning medger
                // Skapa felloggfiler enligt "Error.log" som övergripande med "ErrorYYYYMMDDTHHMISS.fff" som dellogfiler med max stycken enligt standard eller parameter
                string[] logFiles = Directory.GetFiles(logDirectory, fileName + "*.log", SearchOption.TopDirectoryOnly);
                int nbrLogFiles = logFiles.Length;
                int nbrMaxFileItteration = nbrLogFiles;
                int nbrFileItteration = 0;
                while (nbrLogFiles >= maxFilesTotal)
                {
                    // Bryter while-loopen om antalet ittereringar blir fler än potentiella filer att radera
                    // om de ej raderats p.g.a. att de innehåller basfilnamnet men ej datumstämpel på korrekt sätt
                    if (nbrFileItteration >= nbrMaxFileItteration)
                        break;
                    nbrFileItteration++;


                    // Rensa äldsta dellog-fil tills max antal minus en existerar
                    // Hitta äldsta
                    string oldestFileWithPath = string.Empty;
                    DateTime? oldestTimeStamp = null;
                    // YYYYMMDDTHHMISS.fff
                    Regex timestampRegex = new Regex("([2-3][0-9]{3})(0[1-9]|1[0-2])(0[1-9]|[1-2][0-9]|3[0-1])T([0-1][0-9]|2[0-4])([0-5][0-9])([0-5][0-9]).([0-999])");
                    foreach (string file in logFiles)
                    {
                        // Kontrollerar från höger om skiljetecken för katalog (fysisk eller virtuell finns) existerar
                        int position = file.LastIndexOf(@"\");
                        if (position == -1)
                        {
                            position = file.LastIndexOf(@"/");

                        }

                        // Endast filnamn med ändelse
                        string tempFileName;
                        string tempDirectoryPath = string.Empty;
                        if (position == -1)
                        {
                            // antar är fil utan kataloger
                            tempFileName = file;
                        }
                        else
                        {
                            tempFileName = file.Substring(position + 1);
                            tempDirectoryPath = file.Substring(0, position);
                        }

                        // Om filen inte är basloggfilen
                        if (tempFileName != fileName + ".log")
                        {
                            // Om filens 15 tecken efter lika många tecken som basloggfilens antal tecken stämmer med tidsstämpelns teckenuppsättning
                            string potentialFileTimeSuffix = tempFileName.Substring(fileName.Length, 19);
                            if (timestampRegex.IsMatch(potentialFileTimeSuffix))
                            {
                                DateTime tempTimeStamp = new DateTime(Convert.ToInt16(potentialFileTimeSuffix.Substring(0, 4)),     // år
                                                                      Convert.ToInt16(potentialFileTimeSuffix.Substring(4, 2)),     // månad
                                                                      Convert.ToInt16(potentialFileTimeSuffix.Substring(6, 2)),     // dag
                                                                      Convert.ToInt16(potentialFileTimeSuffix.Substring(9, 2)),     // timme
                                                                      Convert.ToInt16(potentialFileTimeSuffix.Substring(11, 2)),    // minut
                                                                      Convert.ToInt16(potentialFileTimeSuffix.Substring(13, 2)),    // sekund
                                                                      Convert.ToInt16(potentialFileTimeSuffix.Substring(16, 3)));   // millisekund

                                if (oldestTimeStamp != null)
                                {
                                    if (tempTimeStamp < oldestTimeStamp)
                                    {
                                        oldestTimeStamp = tempTimeStamp;
                                        oldestFileWithPath = tempDirectoryPath + "\\" + tempFileName;
                                    }
                                }
                                else
                                {
                                    oldestTimeStamp = tempTimeStamp;
                                    oldestFileWithPath = tempDirectoryPath + "\\" + tempFileName;
                                }
                            }
                        }

                    }

                    // Om äldsta filen existerar, radera den
                    if (File.Exists(oldestFileWithPath))
                    {
                        FileInfo file = new FileInfo(oldestFileWithPath);
                        file.Delete();
                        nbrLogFiles--;
                    }


                    if (!string.IsNullOrEmpty(oldestFileWithPath))
                    {
                        // Minska vektorn med sökbara filer med den potentiellt raderbara
                        int arrayIdxDeletedFile = Array.IndexOf(logFiles, oldestFileWithPath);
                        List<string> tmpStringArray = new List<string>(logFiles);
                        tmpStringArray.RemoveAt(arrayIdxDeletedFile);
                        logFiles = tmpStringArray.ToArray();
                    }
                }


                // Kopiera undan basfil med tidsstämpel om antalet filer i inställningar tillåter
                if (maxFilesTotal >= 1)
                {
                    // Formatera datum-tids-sträng för unik
                    string fileTimeSuffix = DateTime.Now.ToString("yyyyMMddTHHmmss.fff");
                    File.Copy(logFile, logDirectory + fileName + fileTimeSuffix + ".log");
                }

                // Tömmer basfilen till noll byte
                using (FileStream fs = File.Open(logFile, FileMode.Truncate, FileAccess.Write))
                {
                    fs.Close();
                }

                // Skriver till basfilen
                writeLoggTextToFile(logMessage, dateTimeStamp, logLevel, logFile);

            }

        }


        /// <summary>
        /// Skriver meddelandet till logg-/textfil enligt visst format under förutsättning att filen existerar.
        /// </summary>
        /// <param name="logMessage">Meddelande som ska loggas.</param>
        /// <param name="dateTimeStamp">Datum och tid för meddelandet.</param>
        /// <param name="logFile">Fil som logg skrivs till.</param>
        private static void writeLoggTextToFile(string logMessage, DateTime dateTimeStamp, LogLevel logLevel, string logFile)
        {
            // Open the log file for append and write the log
            using (StreamWriter sw = new StreamWriter(logFile, true))
            {
                string _dateTimeStamp = dateTimeStamp.ToString("yyyy-MM-dd HH:mm:ss");
                //sw.WriteLine($"{_dateTimeStamp}|\t{logLevel.ToString()}|\t{logMessage}");
                sw.WriteLine(string.Format("{0,20} {1,7} {2}",
                     _dateTimeStamp + "|",
                     logLevel.ToString() + "|",
                     logMessage
                    ));


                sw.Close();
            }
        }


    }
}