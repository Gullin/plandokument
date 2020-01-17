using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Services;

namespace Plan.Plandokument
{

    /// <summary>
    /// 
    /// </summary>
    public static class FindTypes
    {
        internal static string Exact { get; } = "Exact";
        internal static string IsPart { get; } = "IsPart";
        internal static string Unmanaged { get; } = "Unmanaged";
    }



    /// <summary>
    /// Klassen är behållare för dokument från filserver som är hittade efter sökta planer
    /// </summary>
    public class Documents : System.Web.Services.WebService
	{
        /// <summary>
        /// Egenskap 
        /// </summary>
        public DataTable SearchedPlansDocuments { get; private set; }


        public Documents(List<object> planIds)
        {
            this.SearchedPlansDocuments = plansDocs(planIds);
        }


        /// <summary>
        /// Söker planers tillhörande dokument
        /// </summary>
        /// <param name="planIds">Lista med plannyckelsobjekt</param>
        /// <returns>Tabell med funna dokument för egna tidigare beteckning och formell aktbeteckning</returns>
        private DataTable plansDocs(List<object> planIds)
        {
            // Alla planer från cach
            DataTable cachedPlans = PlanCache.GetPlanBasisCache();

            // Tabell för filtrerad sökta planinformationen för vidare sökning
            DataTable dtSearchedPlans = cachedPlans.Clone();

            // Filtrera alla planer från ej sökta
            IEnumerable<DataRow> drs = from filteringdPlans in cachedPlans.AsEnumerable()
                                       where planIds.Contains(filteringdPlans.Field<string>("NYCKEL"))
                                       select filteringdPlans;

            foreach (DataRow dr in drs)
            {
                dtSearchedPlans.ImportRow(dr);
            }


            cachedPlans.Dispose();

            

            // Definierar tabell med filinformation för returnering av metodens resultat
            DataTable dtFileResult = new DataTable();
            DataColumn cl = new DataColumn("PATH", System.Type.GetType("System.String"));
            dtFileResult.Columns.Add(cl);
            cl = new DataColumn("NAME", System.Type.GetType("System.String"));
            dtFileResult.Columns.Add(cl);
            cl = new DataColumn("EXTENTION", System.Type.GetType("System.String"));
            dtFileResult.Columns.Add(cl);
            cl = new DataColumn("SIZE", System.Type.GetType("System.Int64"));
            dtFileResult.Columns.Add(cl);
            cl = new DataColumn("PLAN_ID", System.Type.GetType("System.String"));
            dtFileResult.Columns.Add(cl);
            cl = new DataColumn("DOCUMENTTYPE", System.Type.GetType("System.String"));
            dtFileResult.Columns.Add(cl);
            cl = new DataColumn("FINDTYPE", System.Type.GetType("System.String"));
            dtFileResult.Columns.Add(cl);
            cl = new DataColumn("DOCUMENTPART", System.Type.GetType("System.String"));
            dtFileResult.Columns.Add(cl);
            cl = new DataColumn("THUMNAILPATH", System.Type.GetType("System.String"));
            dtFileResult.Columns.Add(cl);
            // N/A = ingen thumnail
            // L = liten
            // S = stor
            // L,S = liten och stor
            cl = new DataColumn("THUMNAILINDICATION", System.Type.GetType("System.String"));
            dtFileResult.Columns.Add(cl);



            // Hämtar alla dokumenttyper från cache
            Cache cache = HttpRuntime.Cache;
            List<Documenttype> listDocumenttyper = PlanCache.GetPlandocumenttypesCache();



            string documentPrefix = "DP";
            string documentSuffix = (string)Session["PlanHandling"];

            // Sökning av filnamn sker efter två olika namnkonventioner,
            // ena grundar sig på formell aktbeteckning (yngre dokument) och andra på nummerserie (äldre dokument).
            // Prioritering görs i ordningen formell aktbeteckning och nummerserie.
            #region Söker efter filer på formell aktbeteckning
            foreach (DataRow dr in dtSearchedPlans.Rows)
            {
                string documentAkt = dr["akt"].ToString().Replace('/', '_');

                // Om begrepp "handling" itereras alla dokumenttyper igenom som ses som planhandling enligt vektorn ovan
                if (documentSuffix == "handling")
                {
                    foreach (var item in listDocumenttyper)
                    {
                        // kontrollera om filsuffix är planhandling
                        if (Convert.ToBoolean(item.IsPlanhandling))
                        {
                            string suffix = String.IsNullOrEmpty(item.Suffix) ? item.Suffix : "_" + item.Suffix;
                            string searchedFile = documentPrefix + documentAkt + suffix;

                            // Om sökt begrepp inte är tomt
                            if (!string.IsNullOrWhiteSpace(documentAkt))
                            {
                                findFile(searchedFile, dr["nyckel"].ToString(), documentPrefix + documentAkt, dtFileResult);
                            }
                        }
                    }
                }
                else if (documentSuffix == "dokument")
                {
                    // för varje "rad" (par av dokumenttyp och logiskt värde)
                    // alla komibinationer av alla dokumentsuffix/dokumenttyp
                    foreach (var item in listDocumenttyper)
                    {
                        if (!string.IsNullOrEmpty(item.Type)) {
                            string suffix = String.IsNullOrEmpty(item.Suffix) ? item.Suffix : "_" + item.Suffix;
                            string searchedFile = documentPrefix + documentAkt + suffix;

                            // Om sökt begrepp inte är tomt
                            if (!string.IsNullOrWhiteSpace(documentAkt))
                            {
                                findFileInCache(searchedFile, dr["nyckel"].ToString(), documentPrefix + documentAkt, dtFileResult);
                                //findFile(searchedFile, dr["nyckel"].ToString(), documentPrefix + documentAkt, dtFileResult);
                            }
                        }
                    }
                }
                else
                {
                    string suffix = string.Empty;
                    foreach (var item in listDocumenttyper)
                    {
                        if (documentSuffix == item.UrlFilter)
                        {
                            suffix = String.IsNullOrEmpty(item.Suffix) ? item.Suffix : "_" + item.Suffix;
                            break;
                        }
                    }
                    string searchedFile = documentPrefix + documentAkt + suffix;

                    // Om sökt begrepp inte är tomt
                    if (!string.IsNullOrWhiteSpace(documentAkt))
                    {
                        findFile(searchedFile, dr["nyckel"].ToString(), documentPrefix + documentAkt, dtFileResult);
                    }
                }
            }
            #endregion


            // Rensa bort de sökta planer som dokument hittats för efter formell planbeteckning
            foreach (DataRow dr in dtFileResult.Rows)
            {
                foreach (DataRow drDelete in dtSearchedPlans.Select("nyckel = '" + dr["PLAN_ID"].ToString() + "'"))
                {
                    drDelete.Delete();
                }
                dtSearchedPlans.AcceptChanges();
            }


            #region Söker efter filer på tidigare egen aktbeteckning
            foreach (DataRow dr in dtSearchedPlans.Rows)
            {
                string documentAkt = dr["akttidigare"].ToString().Replace("1282K-", "");

                // Om begrepp "handling" sökt för itereras igenom alla dokumenttyper som ses som planhandling enligt vektorn ovan
                if (documentSuffix == "handling")
                {
                    foreach (var item in listDocumenttyper)
                    {
                        // kontrollera om filsuffix är planhandling
                        if (Convert.ToBoolean(item.IsPlanhandling))
                        {
                            string suffix = String.IsNullOrEmpty(item.Suffix) ? item.Suffix : "_" + item.Suffix;
                            string searchedFile = documentPrefix + documentAkt + suffix;

                            // Om sökt begrepp inte är tomt
                            if (!string.IsNullOrWhiteSpace(documentAkt))
                            {
                                findFile(searchedFile, dr["nyckel"].ToString(), documentPrefix + documentAkt, dtFileResult);
                            }
                        }
                    }
                }
                else if (documentSuffix == "dokument")
                {
                    foreach (var item in listDocumenttyper)
                    {
                        if (!string.IsNullOrEmpty(item.Type))
                        {
                            string suffix = String.IsNullOrEmpty(item.Suffix) ? item.Suffix : "_" + item.Suffix;
                            string searchedFile = documentPrefix + documentAkt + suffix;

                            // Om sökt begrepp inte är tomt
                            if (!string.IsNullOrWhiteSpace(documentAkt))
                            {
                                findFile(searchedFile, dr["nyckel"].ToString(), documentPrefix + documentAkt, dtFileResult);
                            }
                        }
                    }
                }
                else
                {
                    string suffix = string.Empty;
                    foreach (var item in listDocumenttyper)
                    {
                        if (documentSuffix == item.UrlFilter)
                        {
                            suffix = String.IsNullOrEmpty(item.Suffix) ? item.Suffix : "_" + item.Suffix;
                            break;
                        }
                    }
                    string searchedFile = documentPrefix + documentAkt + suffix;

                    // Om sökt begrepp inte är tomt
                    if (!string.IsNullOrWhiteSpace(documentAkt))
                    {
                        findFile(searchedFile, dr["nyckel"].ToString(), documentPrefix + documentAkt, dtFileResult);
                    }
                }
            }
            #endregion



            // Komplettera med sökt plans plankartas ev. thumnails
            try
            {
                // För varje hittat dokument, hitta ev. thumnails till plankarta
                foreach (DataRow row in dtFileResult.Rows)
                {
                    string thumnailsRotDirectory = @ConfigurationManager.AppSettings["thumnailsRotDirectory"].ToString();
                    DirectoryInfo thumnailDirectory = new DirectoryInfo(
                        Server.MapPath(thumnailsRotDirectory)
                        );

                    // För dokumenten plankartor
                    if (row["DOCUMENTTYPE"].ToString() == "Karta")
                    {
                        // Söker thumnails och spara temporärt filinformationen i en lista
                        string fileFilter = Path.GetFileNameWithoutExtension(
                                thumnailDirectory.FullName + @"\" + row["NAME"].ToString()
                                ) + "_thumnail-*.jpg";
                        Regex regex = new Regex(
                            "(" + Path.GetFileNameWithoutExtension(
                                thumnailDirectory.FullName + @"\" + row["NAME"].ToString()
                                ) + "_thumnail-l.jpg)" + "|" +
                            "(" + Path.GetFileNameWithoutExtension(
                                thumnailDirectory.FullName + @"\" + row["NAME"].ToString()
                                ) + "_thumnail-s.jpg)"
                            );
                        List<FileInfo> filesFoundExact = thumnailDirectory.EnumerateFiles(fileFilter)
                            .Where(f => regex.IsMatch(@f.FullName)).ToList();


                        // Indikera i sökresultatet vilken typ av thumnails som hittades
                        bool thumnailS = false, thumnailL = false;
                        foreach (FileInfo file in filesFoundExact)
                        {
                            if (file.Name.Contains("-s"))
                            {
                                thumnailS = true;
                            }
                            if (file.Name.Contains("-l"))
                            {
                                thumnailL = true;
                            }
                        }
                        if (thumnailS && thumnailL)
                        {
                            row["THUMNAILINDICATION"] = "s,l";
                        }
                        else if (thumnailS)
                        {
                            row["THUMNAILINDICATION"] = "s";
                        }
                        else if (thumnailL)
                        {
                            row["THUMNAILINDICATION"] = "l";
                        }
                        else
                        {
                            row["THUMNAILINDICATION"] = "N/A";
                        }

                        row["THUMNAILPATH"] = thumnailsRotDirectory + "/";

                    }
                    else
                    {
                        // Ej karta, sätts N/A
                        row["THUMNAILINDICATION"] = "N/A";
                    }
                }
            }
            catch(Exception ex)
            {
                // Klassens namn för loggning
                string className = this.GetType().Name;
                // Metod i klassen som används
                string methodName = MethodBase.GetCurrentMethod().Name;

                UtilityException.LogException(ex, className + " : " + methodName, false);

                // Ej möjligt att uppdatera hänvisning till thumnail, sätts N/A för default hantering i klient
                dtFileResult.Columns["THUMNAILINDICATION"].Expression = "'N/A'";
            }


            return dtFileResult;
        }


        /// <summary>
        /// Funktion för att både söka på akt och tidigareakt (fastighetsregistrets kommunala) 
        /// </summary>
        /// <param name="searchedFile">Sökt filnamn (utan filändelse, inkl. ev. suffix)</param>
        /// <param name="planId">Plannyckel som referens till sökt plan</param>
        /// <param name="dokumentAkt">Akt filnamnsbaserat (exkl. ev. suffix)</param>
        /// <param name="dtFileResult">Resultattabell att fylla på med hittade filer</param>
        private void findFile(string searchedFile, string planId, string dokumentAkt, DataTable dtFileResult)
        {
            string[] directoryRoots = ConfigurationManager.AppSettings["filesRotDirectory"].ToString().Split(',');

            foreach (string root in directoryRoots)
            {
                DirectoryInfo searchedDirectory = new DirectoryInfo(Server.MapPath(root));
                //DirectoryInfo searchedDirectory = new DirectoryInfo(root); 
                getFileToDataTable(searchedDirectory, root, searchedFile, planId, dokumentAkt, dtFileResult);
            }
        }


        /// <summary>
        /// Söker plandokumentsfiler från cache
        /// </summary>
        /// <param name="searchedFile">Sökt filnamn (utan filändelse, inkl. ev. suffix)</param>
        /// <param name="planId">Plannyckel som referens till sökt plan</param>
        /// <param name="dokumentAkt">Akt filnamnsbaserat (exkl. ev. suffix)</param>
        /// <param name="dtFileResult">Resultattabell att fylla på med hittade filer</param>
        private void findFileInCache(string searchedFile, string planId, string dokumentAkt, DataTable dtFileResult)
        {
            Regex regEx = SearchFilter(searchedFile);
            IEnumerable<FileInfo> files = PlanCache.GetPlanDocumentsCache();
            List<FileInfo> searchFileResult = files.Where(f => regEx.IsMatch(f.Name)).ToList();
            CreateFileResult("test", planId, dokumentAkt, searchFileResult, dtFileResult);

        }


        /// <summary>
        /// Rekursiv sökning efter plandokumentsfil direkt från disk
        /// </summary>
        /// <param name="searchedDirectory">Rotkatalog av ramverket löst</param>
        /// <param name="virtualFilePath">Katalog/sökväg som sökta filer kan existera i</param>
        /// <param name="searchedFile">Sökt filnamn (utan filändelse, inkl. ev. suffix)</param>
        /// <param name="planId">Plannyckel som referens till sökt plan</param>
        /// <param name="dokumentAkt">Akt filnamnsbaserat (exkl. ev. suffix)</param>
        /// <param name="dtFileResult">Resultattabell att fylla på med hittade filer</param>
        private void getFileToDataTable(DirectoryInfo searchedDirectory, string virtualFilePath, string searchedFile, string planId, string dokumentAkt, DataTable dtFileResult)
        {
            List<FileInfo> files = null;
            DirectoryInfo[] subDirs = null;


            // Sök fil
            try
            {
                // Hanterar filändelser konfigurerade i applikationsinställningarna (AppSettings), 
                // om inget är konfigurerat selekteras alla filer
                string[] searchedFileExtentions = ConfigurationManager.AppSettings["fileExtentions"].ToString().Split(',');
                if (searchedFileExtentions == null || string.IsNullOrWhiteSpace(searchedFileExtentions[0]))
                {
                    files = searchedDirectory.EnumerateFiles(searchedFile + ".*").ToList();
                    files.AddRange(searchedDirectory.EnumerateFiles(searchedFile + ",*.*").ToList());
                }
                else
                {
                    foreach (string ext in searchedFileExtentions)
                    {
                        List<FileInfo> filesFoundExact = searchedDirectory.EnumerateFiles(searchedFile + ext).ToList();

                        if (files != null)
                        {
                            files.AddRange(filesFoundExact);
                        }
                        else
                        {
                            files = (from f in filesFoundExact
                                     select f).ToList();
                        }

                        List<FileInfo> filesFoundPart = searchedDirectory.EnumerateFiles(searchedFile + ",*" + ext).ToList();
                        files.AddRange(filesFoundPart);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // Klassens namn för loggning
                string className = this.GetType().Name;
                // Metod i klassen som används
                string methodName = MethodBase.GetCurrentMethod().Name;

                UtilityException.LogException(ex, className + " : " + methodName, true);
            }
            catch (DirectoryNotFoundException ex)
            {
                // Klassens namn för loggning
                string className = this.GetType().Name;
                // Metod i klassen som används
                string methodName = MethodBase.GetCurrentMethod().Name;

                UtilityException.LogException(ex, className + " : " + methodName, true);
            }



            // Om hittade filer
            if (files != null && files.Count > 0)
            {
                virtualFilePath = EnsureEndingSlash(virtualFilePath);

                CreateFileResult(virtualFilePath, planId, dokumentAkt, files, dtFileResult);

            }



            // Val genom applikationsinställningarna (AppSettings) om underkataloger ska genomsökas
            bool subDirCrawl = true;
            string appSetSubDirCrawl = ConfigurationManager.AppSettings["subDirectoryCrawl"].ToString();
            if (appSetSubDirCrawl == "true" || appSetSubDirCrawl == "false")
            {
                subDirCrawl = Convert.ToBoolean(appSetSubDirCrawl);
            }

            // Genomsöker underkataloger
            if (subDirCrawl)
            {
                // Alla underkataloger i gällande sökt katalog
                subDirs = searchedDirectory.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    // Rekursivt sök i alla underkataloger
                    getFileToDataTable(dirInfo, virtualFilePath + "/" + dirInfo.Name, searchedFile, planId, dokumentAkt, dtFileResult);
                }
            }
            


        }


        /// <summary>
        /// Bygger sökfilter enligt namnkonvention
        /// </summary>
        /// <param name="searchedFile">Sökt plandokument enl. namnkonvention</param>
        /// <returns>Sökfilter som reguljärt uttryck</returns>
        private static Regex SearchFilter(string searchedFile)
        {
            string[] searchedFileExtentions = ConfigurationManager.AppSettings["fileExtentions"].ToString().Split(',');
            string filter = "";
            if (searchedFileExtentions == null || string.IsNullOrWhiteSpace(searchedFileExtentions[0]))
            {
                filter = @"(\.[0-9a-öA-Ö]+)";
            }
            else
            {
                bool isFirst = true;
                foreach (string ext in searchedFileExtentions)
                {
                    if (isFirst)
                    {
                        filter = @"\" + @ext.ToLower() + @"|\" + @ext.ToUpper();
                        isFirst = false;
                    }
                    else
                    {
                        filter += "|" + @"\" + @ext.ToLower() + @"|\" + @ext.ToUpper();
                    }
                }
            }
            filter = @searchedFile + "(,[0-9a-öA-Ö]+)*(" + filter + ")";
            return new Regex(@filter);
        }


        /// <summary>
        /// Fyller global datatabell med sökresultat 
        /// </summary>
        /// <param name="virtualFilePath">Katalog/sökväg som sökta filer kan existera i</param>
        /// <param name="planId">Plannyckel som referens till sökt plan</param>
        /// <param name="dokumentAkt">Akt filnamnsbaserat (exkl. ev. suffix)</param>
        /// <param name="files">Lista med funna plandokument</param>
        /// <param name="dtFileResult">Global datatabell med sökresultat</param>
        private void CreateFileResult(string virtualFilePath, string planId, string dokumentAkt, List<FileInfo> files, DataTable dtFileResult)
        {
            DataRow drFile;
            // För varje hittad fil lagra information i publik datatabell
            foreach (FileInfo fi in files)
            {
                // Hantera suffix i filnamn
                string[] fileNameParts = fi.Name.Replace(dokumentAkt, "").Split('_');

                // Väljer ut sista delen av delad textsträng
                string lasFileNamePart = fileNameParts[fileNameParts.Length - 1];
                string documentType = string.Empty;

                // Klipper bort filändelsen genom att vända på textsträng samt vänder tillbaka för jämförelsen
                string potentialDocumentType = new string(lasFileNamePart.ToCharArray()
                                                .Reverse()
                                                .ToArray())
                                                .Substring(
                                                            (fi.Extension.Length),
                                                            (lasFileNamePart.Length - fi.Extension.Length));
                potentialDocumentType = new string(potentialDocumentType.ToCharArray().Reverse().ToArray());



                // Hanterar och kontrollerar för flera dokumentdelar
                string findtype = string.Empty;
                string findtypePart = string.Empty;
                try
                {
                    if (potentialDocumentType.Contains(","))
                    {
                        string[] findtypeParts = potentialDocumentType.Split(',');

                        // Följer ej namnkonventionen, för många möjligheter till dokumentdelar
                        if (findtypeParts.Length > 2)
                        {
                            findtype = FindTypes.Unmanaged;
                            throw new Exception("För många signaler om dokumentdelar. Kommatecken får utelämnas eller endast förekomma en gång.");
                        }
                        else if (findtypeParts.Length == 2)
                        {
                            if (string.IsNullOrWhiteSpace(findtypeParts[1]))
                            {
                                findtype = FindTypes.IsPart;
                            }
                            else if (!string.IsNullOrWhiteSpace(findtypeParts[1]))
                            {
                                findtypePart = findtypeParts[1].ToString();
                                findtype = FindTypes.IsPart;
                            }
                            else
                            {
                                findtype = FindTypes.Unmanaged;
                            }
                        }
                        else
                        {
                            throw new Exception("Innehåller kommatecken som signal om dokumentdelar, men kan inte hantera formen.");
                        }

                        potentialDocumentType = findtypeParts[0];
                    }
                    else
                    {
                        findtype = FindTypes.Exact;
                    }
                }
                catch (Exception ex)
                {

                    // Klassens namn för loggning
                    string className = this.GetType().Name;
                    // Metod i klassen som används
                    string methodName = MethodBase.GetCurrentMethod().Name;

                    UtilityException.LogException(ex, className + " : " + methodName, true);
                }



                // Hämtar alla dokumenttyper från cache
                List<Documenttype> listDocumenttyper = PlanCache.GetPlandocumenttypesCache();


                // Jämför mot alla suffix i dokumenttypdomänen
                // Två filnamnssuffix ovr och handling är för samling av bl.a. ej sorterade dokument och arv
                if (potentialDocumentType == "")
                {
                    documentType = "Karta";
                }
                else
                {
                    foreach (var item in listDocumenttyper)
                    {
                        if (potentialDocumentType == item.Suffix)
                        {
                            documentType = item.Type;
                        }
                    }
                    if (potentialDocumentType == "ovr" || potentialDocumentType == "handling")
                    {
                        documentType = "Övriga";
                    }
                }
                if (string.IsNullOrEmpty(documentType))
                {
                    //TODO: DOKUMENTTYP: Vad händer om dokumenttyp inte kan fastställas vid sökträff
                }




                // Adderar fil och dess information till datatabell med sökresultat
                drFile = dtFileResult.NewRow();
                // Fysisk fil-sökväg, fungerar ej för hyperlänk. Dokument behöver finnas som relativ sökväg till webbapplikationen.
                //drFile["PATH"] = searchedDirectory.FullName;
                drFile["PATH"] = virtualFilePath;
                drFile["NAME"] = fi.Name;
                drFile["EXTENTION"] = fi.Extension;
                // Filstorlek i Byte
                drFile["SIZE"] = fi.Length;
                drFile["PLAN_ID"] = planId;
                drFile["DOCUMENTTYPE"] = documentType;
                drFile["FINDTYPE"] = findtype;
                drFile["DOCUMENTPART"] = findtypePart;
                dtFileResult.Rows.Add(drFile);
            }
        }


        /// <summary>
        /// Tillser att textsträng slutar med front slash
        /// </summary>
        /// <param name="virtualFilePath">Sökväg</param>
        /// <returns>Textsträng med avslutande frontslash</returns>
        private static string EnsureEndingSlash(string virtualFilePath)
        {
            // ser till så att kataloger slutar med slash
            string pathEnd = virtualFilePath.Substring(virtualFilePath.Length - 1, 1);
            if (!(pathEnd == "/" || pathEnd == "\\"))
            {
                virtualFilePath += "/";
            }

            return virtualFilePath;
        }
    }
}