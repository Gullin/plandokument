﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Threading;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using Plan.Shared.Thumnails;
using Plan.Plandokument.SQLite;
using Plan.Plandokument;
using System.Data.SQLite;
using System.Drawing;
using System.Data;

namespace Plan.Plandokument
{

    /// <summary>
    /// Tjänster anpassade för applikationens behov i kontrollpanelen
    /// </summary>
    [WebService(Namespace = "Landskrona.Apps.Plan.Dokument.Ws")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    public class Kontrollpanel : System.Web.Services.WebService
    {

        #region Logs
        /// <summary>
        /// Hämtar logg. Filtreras genom att hämta nödvändiga rader om filstorlek förändrats.
        /// </summary>
        /// <param name="latestFileSize">Filstorlek vid tidigare läsning av logg</param>
        /// <param name="latestRow">Antalet rader vid senaste läsning av logg</param>
        /// <returns></returns>
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string GetLog(string latestFileSize, string latestRow)
        {

            // TODO: https://www.nimaara.com/counting-lines-of-a-text-file/
            // TODO: https://stackoverflow.com/questions/1262965/how-do-i-read-a-specified-line-in-a-text-file

            // Filstorlek
            long fileSize = 0;
            Int64 nbrLatestRow = Convert.ToInt64(latestRow);
            // Nollställer senast läst rad om senast filstorlek är större än logfil
            if (long.Parse(latestFileSize) > fileSize)
            {
                nbrLatestRow = 0;
            }

            var lineCounter = 0L;
            var lineDiffCounter = 0L;
            StringBuilder sb = new StringBuilder();
            string line = String.Empty;
            if (File.Exists(UtilityLog.logFile))
            {
                using (FileStream fileToReadSizeFrom = File.Open(UtilityLog.logFile, FileMode.Open, FileAccess.Read))
                {
                    // Filstorlek
                    fileSize = fileToReadSizeFrom.Length;

                    // Anvnder samma stream för att inte låsa fil (StreamReader låser som standard fil)
                    using (var reader = new StreamReader(fileToReadSizeFrom))
                    {


                        while ((line = reader.ReadLine()) != null)
                        {
                            lineCounter++;
                            if (lineCounter > nbrLatestRow)
                            {
                                lineDiffCounter++;
                                sb.Append(line + "\n");
                            }
                        }
                    }

                    fileToReadSizeFrom.Close();
                }

            }


            //Int64 nbrLatestRow = Convert.ToInt64(latestRow);


            // Nollställer senast läst rad om senast filstorlek är större än logfil
            //if (long.Parse(latestFileSize) > fileSize)
            //{
            //    nbrLatestRow = 0;
            //}



            // Hämtar rader från logg och adderar radbrytning för javascript
            //var lineCounter = 0L;
            //var lineDiffCounter = 0L;
            //StringBuilder sb = new StringBuilder();
            //string line = String.Empty;
            //using (var reader = new StreamReader(UtilityLog.logFile))
            //{
            //    while ((line = reader.ReadLine()) != null)
            //    {
            //        lineCounter++;
            //        if (lineCounter > nbrLatestRow)
            //        {
            //            lineDiffCounter++;
            //            sb.Append(line + "\n");
            //        }
            //    }
            //}


            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer() { MaxJsonLength = 2147483644 };

            return jsonSerializer.Serialize(new { FileSize = fileSize, FileLineCounts = lineCounter, NewLineCounts = lineDiffCounter, Content = sb.ToString() });

        }
        #endregion



        #region Stats
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatTotalByDayRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatTotalByDayRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatTotalByMonthRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatTotalByMonthRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatTotalByYearRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatTotalByYearRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatTotalRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatTotalRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatPeriodRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatPeriodRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatRunningTotalRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatRunningTotalRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatRunningTotalByDayRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatRunningTotalByDayRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatRunningTotalByMonthRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatRunningTotalByMonthRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatRunningTotalByYearRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatRunningTotalByYearRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatRunningHitsRequests()
        {
            return JSONHelpers.getObjectAsJson(
                StatData.StatRunningHitsRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatRunningSearchRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatRunningSearchRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatSearchRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatSearchRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatSearchtimeRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatSearchtimeRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatSearchtimeByDayRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatSearchtimeByDayRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatSearchtimeByMonthRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatSearchtimeByMonthRequests()
                );

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string StatSearchtimeByYearRequests()
        {

            return JSONHelpers.getObjectAsJson(
                StatData.StatSearchtimeByYearRequests()
                );

        }

        #endregion



        #region Cache
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlanBasis()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlanBasis());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlanBerorFastighet()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlanBerorFastighet());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlandocumenttypes()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlandocumenttypes());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlanBerorPlan()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlanBerorPlan());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlanDocuments()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlanDocuments());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsAll()
        {
            bool planBasis = PlanCache.CacheExistsPlanBasis();
            bool planBerorFastighet = PlanCache.CacheExistsPlanBerorFastighet();
            bool planDocumenttypes = PlanCache.CacheExistsPlandocumenttypes();
            bool planBerorPlan = PlanCache.CacheExistsPlanBerorPlan();
            bool planDocuments = PlanCache.CacheExistsPlanDocuments();
            bool exists = false;

            if (planBasis && planBerorFastighet && planDocumenttypes && planBerorPlan && planDocuments)
            {
                exists = true;
            }

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(exists);

        }




        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheRefreshPlanBasis()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            try
            {
                if (PlanCache.CacheExistsPlanBasis())
                {
                    PlanCache.RemoveCachedPlanBasis();
                }
                else
                {
                    PlanCache.setPlanCache();
                }
                return jsonSerializer.Serialize(true.ToString());
            }
            catch
            {
                return jsonSerializer.Serialize(false.ToString());
            }

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheRefreshPlanBerorFastighet()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            try
            {
                if (PlanCache.CacheExistsPlanBerorFastighet())
                {
                    PlanCache.RemoveCachedPlanBerorFastighet();
                }
                else
                {
                    PlanCache.setPlanBerorFastighetCache();
                }
                return jsonSerializer.Serialize(true.ToString());
            }
            catch
            {
                return jsonSerializer.Serialize(false.ToString());
            }

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheRefreshPlandocumenttypes()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            try
            {
                if (PlanCache.CacheExistsPlandocumenttypes())
                {
                    PlanCache.RemoveCachePlandocumenttypes();
                }
                else
                {
                    PlanCache.setDocumenttypesCache();
                }
                return jsonSerializer.Serialize(true.ToString());
            }
            catch
            {
                return jsonSerializer.Serialize(false.ToString());
            }

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheRefreshPlanBerorPlan()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            try
            {
                if (PlanCache.CacheExistsPlanBerorPlan())
                {
                    PlanCache.RemoveCachedPlanBerorPlan();
                }
                else
                {
                    PlanCache.setPlanBerorPlanCache();
                }
                return jsonSerializer.Serialize(true.ToString());
            }
            catch
            {
                return jsonSerializer.Serialize(false.ToString());
            }

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheRefreshPlanDocuments()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            try
            {
                if (PlanCache.CacheExistsPlanDocuments())
                {
                    PlanCache.RemoveCachedPlanDocuments();
                }
                else
                {
                    PlanCache.setPlanDocumentsCache();
                }
                return jsonSerializer.Serialize(true.ToString());
            }
            catch
            {
                return jsonSerializer.Serialize(false.ToString());
            }

        }




        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheMeta()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.GetCacheMeta());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheTimeDuration()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheDuration());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheTimeElapsed()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheElapsed());

        }
        #endregion



        #region Service
        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string ServiceMeta()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(UtilityServicePlandokumentThumnails.GetServiceMeta());

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string ThumnailServiceExists()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(UtilityServicePlandokumentThumnails.ServiceExists(ConfigShared.ServiceName));

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string ThumnailStartService()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(UtilityServicePlandokumentThumnails.StartService(ConfigShared.ServiceName));

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string ThumnailStopService()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(UtilityServicePlandokumentThumnails.StopService(ConfigShared.ServiceName));

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string ThumnailServiceIsRunning()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(UtilityServicePlandokumentThumnails.ServiceIsRunning(ConfigShared.ServiceName));

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string ThumnailRebootService()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(UtilityServicePlandokumentThumnails.RebootService(ConfigShared.ServiceName));

        }
        #endregion



        #region Thumnail
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string GetPlansDocsAll()
        {
            Documents planDocs = new Documents();

            return JSONHelpers.getObjectAsJson(DataTableHelpers.getTableSorted(planDocs.SearchedPlansDocuments, "DOCUMENTTYPE", "ASC", "EXTENTION", "ASC"));
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CreateThumnails(List<object> planImages)
        {
            bool successful = false;
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            try
            {
                foreach (string planImage in planImages)
                {
                    ManageImages.CreateThumnailFiles(
                        Utility.EnsureEndingSlash(ConfigShared.WatchedFolder) + Path.GetFileName(planImage)
                        );

                    UtilityLog.Log(
                        $"Miniatyrbild SKAPAD {Utility.EnsureEndingSlash(ConfigShared.WatchedFolder) + Path.GetFileName(planImage)}",
                        Utility.LogLevel.INFORM
                        );
                }

                successful = true;

                return jsonSerializer.Serialize(successful);
            }
            catch (Exception exc)
            {
                UtilityException.LogException(exc, $"Plandokument Thumnails : CreateThumnails", false);

                return jsonSerializer.Serialize(false.ToString());
            }
        }

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string DeleteThumnails(List<object> planImages)
        {
            bool successful = false;

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            try
            {
                foreach (string planImage in planImages)
                {
                    ManageImages.DeleteThumnailFiles(
                        Utility.EnsureEndingSlash(ConfigShared.WatchedFolder) + Path.GetFileName(planImage)
                        );

                    UtilityLog.Log(
                        $"Miniatyrbild RADERAD {Utility.EnsureEndingSlash(ConfigShared.WatchedFolder) + Path.GetFileName(planImage)}",
                        Utility.LogLevel.INFORM
                        );
                }

                successful = true;

                return jsonSerializer.Serialize(successful);
            }
            catch (Exception exc)
            {
                UtilityException.LogException(exc, $"Plandokument Thumnails : DeleteThumnails", false);

                return jsonSerializer.Serialize(false.ToString());
            }
        }


        #endregion


    }
}
