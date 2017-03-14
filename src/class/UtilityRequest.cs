using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace Plan.Plandokument
{
	public class UtilityRequest : Utility
	{
        private UtilityRequest()
        { }

        //TODO: Request-log, logga async/await
        /// <summary>
        /// Loggar sökningens statistik till fil på server.
        /// </summary>
        /// <param name="searchRequest">Datatabell innehållande 4 st. kolumner enligt följande; antal sökta, antal träffar, tidpunkt, tidsåtgång.
        /// <paramref name="NBRSEARCHED"/>
        /// <paramref name="NBRHITS"/>
        /// <paramref name="WHEN"/>
        /// <paramref name="TIME"/>
        /// </param>
        /// <value>Test</value>
        public static void LogRequestStatsAsync(DataTable searchRequest)
        {
            try
            {
                // Tilldelar värde för max antal loggfiler, noll ingen loggning, negativt värde oändligt antal filer eller annars antalet
                // om inget anges eller inget tal antas negativt värde
                int maxFilesTotal = int.TryParse(ConfigurationManager.AppSettings["requestStatFileMaxTotal"].ToString(), out maxFilesTotal) ? maxFilesTotal : -1;

                if (maxFilesTotal != 0)
                {
                    // Standardvärden
                    string baseFileNameDefult = "RequestStatistik";
                    double maxFileByteSizeDefault = 10;
                    string fileHeader = "Occurred;NbrSearched;NbrHits;SearchTime (notDocsMap)";

                    logDirectoryExist();

                    // Get the absolute path to the log file
                    string baseFileName = ConfigurationManager.AppSettings["requestStatFileName"].ToString();
                    if (string.IsNullOrEmpty(baseFileName))
                    {
                        baseFileName = baseFileNameDefult;
                    }
                    string logFile = logDirectory + baseFileName + ".log";
                    logFile = HttpContext.Current.Server.MapPath(logFile);
                    if (!File.Exists(logFile))
                    {
                        FileStream fs = File.Create(logFile);
                        fs.Close();
                        using (StreamWriter swHeader = new StreamWriter(logFile, true))
                        {
                            swHeader.WriteLine(fileHeader);
                        }
                    }

                    // Tilldelar värde till maximal storlek för loggfiler
                    double maxFileByteSize = maxFileByteSizeDefault;
                    if (double.TryParse(ConfigurationManager.AppSettings["requestStatFileMaxByteSize"].ToString(), out maxFileByteSize))
                    {
                        if (maxFileByteSize < 0.005)
                            maxFileByteSize = maxFileByteSizeDefault;
                    }
                    maxFileByteSize = maxFileByteSize * 1024.0 * 1024.0;


                    // Läs filstorlek för basloggfil
                    FileStream fileToReadSizeFrom = File.Open(logFile, FileMode.Open, FileAccess.Read);
                    long fileSize = fileToReadSizeFrom.Length;
                    fileToReadSizeFrom.Close();
                    fileToReadSizeFrom.Dispose();

                    // Om basfilen inte är för stor skriv till den, annars kopiera bort den och skapa ny tom basfil
                    if (fileSize < maxFileByteSize)
                    {
                        writeRequestStatToFile(searchRequest, logFile);
                    }
                    else
                    {
                        if (maxFilesTotal > 0)
                        {
                            //dela upp basfilen men ej till fler delfiler än inställning medger
                            // Skapa felloggfiler enligt "Error.log" som övergripande med "ErrorYYYYMMDDTHHMISS.fff" som dellogfiler med max stycken enligt standard eller parameter
                            string[] logFiles = Directory.GetFiles(logDirectory, baseFileName + "*.log", SearchOption.TopDirectoryOnly);
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
                                    if (tempFileName != baseFileName + ".log")
                                    {
                                        // Om filens 15 tecken efter lika många tecken som basloggfilens antal tecken stämmer med tidsstämpelns teckenuppsättning
                                        string potentialFileTimeSuffix = tempFileName.Substring(baseFileName.Length, 15);
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
                        }


                        // Formatera datum-tids-sträng för unik
                        string fileTimeSuffix = DateTime.Now.ToString("yyyyMMddTHHmmss.fff");
                        // Kopiera undan basfil med tidsstämpel om antalet filer i inställningar tillåter
                        File.Copy(logFile, logDirectory + "\\" + baseFileName + fileTimeSuffix + ".log");

                        // Tömmer basfilen till noll byte
                        FileStream fs = File.Open(logFile, FileMode.Truncate, FileAccess.Write);
                        fs.Close();
                        
                        // Skriver rubriker på nytt
                        using (StreamWriter swHeader = new StreamWriter(logFile, true))
                        {
                            swHeader.WriteLine(fileHeader);
                        }

                        // Skriver till basfilen
                        writeRequestStatToFile(searchRequest, logFile);

                    }


                }
            }
            catch (Exception exc)
            {
                UtilityException.LogException(exc, "Loggning Statistik : LogRequestStats", true);
            }
        }

        private static void writeRequestStatToFile(DataTable searchRequest, string logFile)
        {
            // skriv till basfilen
            // Open the log file for append and write the log
            using (StreamWriter sw = new StreamWriter(logFile, true))
            {
                foreach (DataRow dr in searchRequest.Rows)
                {
                    sw.WriteLine(dr["WHEN"].ToString() + ";" +
                                            dr["NBRSEARCHED"].ToString() + ";" +
                                            dr["NBRHITS"].ToString() + ";" +
                                            dr["TIME"].ToString());
                }
            }
        }
	}
}