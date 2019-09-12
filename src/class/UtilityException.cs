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
	public class UtilityException : Utility
	{

        // All methods are static, so this can be private
        private UtilityException()
        { }


        //TODO: Error-log, logga async/await
        // Log an Exception
        /// <summary>
        /// Loggar undantaget till fil och ev. e-postsignal om felet och bifogad loggfil
        /// </summary>
        /// <param name="exc">Undantaget</param>
        /// <param name="source">Källan till felet</param>
        /// <param name="mailNotify">Om e-postsignal om fel ska skickas</param>
        public static void LogException(Exception exc, string source, bool mailNotify)
        {
            // Standardvärden
            string baseFileNameDefult = "Error";
            double maxFileByteSizeDefault = 10;
            int maxFilesTotalDeafult = 1;

            logDirectoryExist();

            // Get the absolute path to the log file, skapa logg-fil om den inte finns
            string baseFileName = ConfigurationManager.AppSettings["errorFileName"].ToString();
            if (string.IsNullOrEmpty(baseFileName))
            {
                baseFileName = baseFileNameDefult;
            }
            string logFile = logDirectory + baseFileName + ".log";
            if (!File.Exists(logFile))
            {
                using (FileStream fs = File.Create(logFile))
                {
                    fs.Close();
                }
            }

            // Tilldelar värde till maximal storlek för loggfiler
            double maxFileByteSize = maxFileByteSizeDefault;
            if (double.TryParse(ConfigurationManager.AppSettings["errorFileMaxByteSize"].ToString(), out maxFileByteSize))
            {
                if (maxFileByteSize < 0.005)
                    maxFileByteSize = maxFileByteSizeDefault;
            }
            maxFileByteSize = maxFileByteSize * 1024.0 * 1024.0;

            // Tilldelar värde för max antal loggfiler eller standardvärdet
            int maxFilesTotal = maxFilesTotalDeafult;
            if (int.TryParse(ConfigurationManager.AppSettings["errorFileMaxTotal"].ToString(), out maxFilesTotal))
            {
                if (maxFilesTotal < 1)
                    maxFilesTotal = maxFilesTotalDeafult;
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
                writeLoggTextToFile(exc, source, logFile);
            }
            else
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
                            string potentialFileTimeSuffix = tempFileName.Substring(baseFileName.Length, 19);
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
                    if(File.Exists(oldestFileWithPath))
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
                    File.Copy(logFile, logDirectory + baseFileName + fileTimeSuffix + ".log");
                }

                // Tömmer basfilen till noll byte
                using (FileStream fs = File.Open(logFile, FileMode.Truncate, FileAccess.Write))
                {
                    fs.Close();
                }

                // Skriver till basfilen
                writeLoggTextToFile(exc, source, logFile);

            }

            if (mailNotify)
            {
                notifySystemOps(exc, logFile);
            }

        }

        /// <summary>
        /// Skriver felet (inre och yttre undantaget) till logg-/textfil enligt visst format under förutsättning att filen existerar.
        /// </summary>
        /// <param name="exc">Undantaget</param>
        /// <param name="source">Information om var i koden felet uppstod</param>
        /// <param name="logFile">Fil felet ska loggas till</param>
        private static void writeLoggTextToFile(Exception exc, string source, string logFile)
        {
            // Open the log file for append and write the log
            using (StreamWriter sw = new StreamWriter(logFile, true))
            {
                sw.WriteLine("********** {0} **********", DateTime.Now);

                if (exc != null)
                {
                    sw.Write("Exception Type: ");
                    sw.WriteLine(exc.GetType().ToString());
                    sw.WriteLine("Exception: " + exc.Message);
                    sw.WriteLine("Source: " + source);
                    sw.WriteLine("Stack Trace: ");
                    if (exc.StackTrace != null)
                    {
                        sw.WriteLine(exc.StackTrace);
                        sw.WriteLine();
                    }
                    else
                    {
                        sw.WriteLine(" - ");
                        sw.WriteLine();
                    }

                    if (exc.InnerException != null)
                    {
                        sw.Write("Inner Exception Type: ");
                        sw.WriteLine(exc.InnerException.GetType().ToString());
                        sw.Write("Inner Exception: ");
                        sw.WriteLine(exc.InnerException.Message);
                        sw.Write("Inner Source: ");
                        sw.WriteLine(exc.InnerException.Source);
                        if (exc.InnerException.StackTrace != null)
                        {
                            sw.WriteLine("Inner Stack Trace: ");
                            sw.WriteLine(exc.InnerException.StackTrace);
                        }
                        else
                        {
                            sw.WriteLine(" - ");
                            sw.WriteLine();
                        }
                    }
                }
                sw.Close();
            }
        }

        /// <summary>
        /// Notifiera systemoperatör om fel uppstått. Fel mailas endast om ett dygn passerat sedan senast signal.
        /// </summary>
        /// <param name="exc"></param>
        private static void notifySystemOps(Exception exc, string logFile)
        {
            //throw new NotImplementedException();

            #region Hantering av tidpunkt för e-postande del 1(2)
            logDirectoryExist();

            string fileExceptionLatestNotice = logDirectory + "/" + "exceptionLatestNotice.log";
            fileExceptionLatestNotice = HttpContext.Current.Server.MapPath(fileExceptionLatestNotice);
            if (!File.Exists(fileExceptionLatestNotice))
            {
                using (FileStream fs = File.Create(fileExceptionLatestNotice))
                {
                    fs.Close();
                }
            }

            string latestNoticeString = string.Empty;
            using (StreamReader sr = new StreamReader(fileExceptionLatestNotice))
            {
                latestNoticeString = sr.ReadLine();
                sr.Close();
            }
            DateTime latestNoticeDatetime = DateTime.MinValue;
            if (!string.IsNullOrEmpty(latestNoticeString))
            {
                DateTime.TryParse(latestNoticeString, out latestNoticeDatetime);
            }
            #endregion

            // Om fel ej tidigare inträffat eller om viss tidsrymd förflutet sedan senaste meddelande om fel skickats (under förutsättning att värdet i filen kan parsas till datum/tid)
            // Skicka nödvändigt och
            // uppdatera tiden i filen
            DateTime nu = DateTime.Now;
            if (latestNoticeDatetime == DateTime.MinValue || nu.Subtract(latestNoticeDatetime).TotalDays > 1)
            {
                // Bygger och skicka mail
                ///Senaste exception (med inner) och loggfil som bifogad fil
                #region Mail
                StringBuilder body = new StringBuilder();
                body.Append("<b>Exception Type:</b> ");
                body.AppendLine(exc.GetType().ToString());
                body.Append("<b>Exception:</b> ");
                body.AppendLine(exc.Message);
                body.Append("<b>Stack Trace:</b> ");
                if (exc.StackTrace != null)
                {
                    body.AppendLine(exc.StackTrace);
                }
                else
                {
                    body.AppendLine(" - ");
                }
                if (exc.InnerException != null)
                {
                    body.Append("<b>Inner Exception Type:</b> ");
                    body.AppendLine(exc.InnerException.GetType().ToString());
                    body.Append("<b>Inner Exception:</b> ");
                    body.AppendLine(exc.InnerException.Message);
                    body.Append("<b>Inner Source:</b> ");
                    body.AppendLine(exc.InnerException.Source);
                    if (exc.InnerException.StackTrace != null)
                    {
                        body.Append("<b>Inner Stack Trace:</b> ");
                        body.AppendLine(exc.InnerException.StackTrace);
                    }
                    else
                    {
                        body.AppendLine(" - ");
                    }
                }


                try
                {
                    using (UtilitySmtpClientExtender mailclient = new UtilitySmtpClientExtender())
                    {
                        if (mailclient.IsToBeSent)
                        {
                            MailAddress to = new MailAddress(ConfigurationManager.AppSettings["to"].ToString());
                            MailAddress from = new MailAddress(ConfigurationManager.AppSettings["from"].ToString());
                            Attachment attachment = new Attachment(logFile);
                            MailMessage mail = new MailMessage(to, from);
                            mail.Attachments.Add(attachment);
                            mail.Subject = ConfigurationManager.AppSettings["subject"].ToString() + exc.Message.ToString();
                            mail.Body = body.ToString();
                            mail.IsBodyHtml = true;

                            Int32 port = Int32.TryParse(ConfigurationManager.AppSettings["port"].ToString(), out port) ? port : 25;
                            bool isSsl = bool.TryParse(ConfigurationManager.AppSettings["ssl"].ToString(), out isSsl) ? isSsl : true;
                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["timeOut"].ToString()))
                            {
                                mailclient.Timeout = Int32.TryParse(ConfigurationManager.AppSettings["timeOut"].ToString(), out port) ? port : 100000;
                            }
                            mailclient.Host = ConfigurationManager.AppSettings["host"].ToString();
                            mailclient.Port = port;
                            mailclient.UseDefaultCredentials = false;
                            mailclient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["user"].ToString(), ConfigurationManager.AppSettings["password"].ToString());
                            mailclient.EnableSsl = isSsl;


                            mailclient.Send(mail);
                        }
                    }
                }
                catch (Exception smtpExc)
                {
                    UtilityException.LogException(smtpExc, "Fel i felloggningens e-postfunktion", false);
                }
                #endregion

                #region Hantering av tidpunkt för e-postande del 2(2)
                // Logg senast skickat fel, uppdatera tid i textfil (databas)
                // Tömmer filen
                using (FileStream fs = File.Open(fileExceptionLatestNotice, FileMode.Truncate, FileAccess.Write))
                {
                    fs.Close();
                }

                // Skriver in ny tid
                using (StreamWriter sw = new StreamWriter(fileExceptionLatestNotice))
                {
                    sw.WriteLine(nu.ToString());
                    sw.Close();
                }
                #endregion
            }
        }

    }
}