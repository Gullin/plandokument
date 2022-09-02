using Npgsql;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Collections;
using System.IO;
using System.Web.Hosting;
using System.Reflection;

namespace Plan.Plandokument
{

    public class CacheMeta
    {
        public int NumberOfTotalCaches { get; set; }
        public int NumberOfApplicationCaches { get; set; }
        public long AvailableBytes { get; set; }
        public long AvailablePhysicalMemory { get; set; }
        public List<Caches> Caches { get; set; }
    }

    public class Caches
    {
        public string Key { get; set; }
        public string Type { get; set; }
    }

    public class CachedDocuments
    {
        public string PathSettingsRoot { get; set; }
        public List<FileInfo> Documents { get; set; }
    }

    public class PlanCache : System.Web.Services.WebService
    {

        public static CacheMeta GetCacheMeta()
        {
            Cache cache = new Cache();
            CacheMeta cacheMeta = new CacheMeta
            {
                NumberOfTotalCaches = cache.Count,
                AvailableBytes = cache.EffectivePrivateBytesLimit,
                AvailablePhysicalMemory = cache.EffectivePercentagePhysicalMemoryLimit
            };

            int nbrOfApplicationCaches = 0;
            IDictionaryEnumerator en = HttpContext.Current.Cache.GetEnumerator();
            cacheMeta.Caches = new List<Caches>();
            while (en.MoveNext())
            {

                // Lägger endast till cachar som är skapad explicit i applikationen (eliminera systemcachar)
                if (en.Key.ToString().Contains("C_"))
                {
                    cacheMeta.Caches.Add(new Caches()
                    {
                        Key = en.Key.ToString(),
                        Type = "Application"
                    });
                    nbrOfApplicationCaches++;
                }
                else
                {
                    cacheMeta.Caches.Add(new Caches()
                    {
                        Key = en.Key.ToString(),
                        Type = "System"
                    });
                }
            }

            cacheMeta.NumberOfApplicationCaches = nbrOfApplicationCaches;

            return cacheMeta;
        }

        /// <summary>
        /// Returnerar basinformationen för planern från cache. Existerar cachen ej skapas den.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPlanBasisCache()
        {
            DataTable cachedPlans = (DataTable)HttpRuntime.Cache["C_Plans"];

            if (cachedPlans != null)
            {
                return cachedPlans;
            }
            else
            {
                setPlanCache();
                return (DataTable)HttpRuntime.Cache["C_Plans"];
            }
        }


        /// <summary>
        /// Returnerar plandokumenttyperna från cache. Existerar cachen ej skapas den.
        /// </summary>
        /// <returns></returns>
        public static List<Documenttype> GetPlandocumenttypesCache()
        {
            List<Documenttype> cachedDocumenttypes = (List<Documenttype>)HttpRuntime.Cache["C_Documenttypes"];

            if (cachedDocumenttypes != null)
            {
                return cachedDocumenttypes;
            }
            else
            {
                setDocumenttypesCache();
                return (List<Documenttype>)HttpRuntime.Cache["C_Documenttypes"];
            }
        }


        /// <summary>
        /// Returnerar plan och fastigheter som berör varandra från cache. Existerar cachen ej skapas den.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPlanBerorFastighetCache()
        {
            DataTable cachedPlanBerorFastighet = (DataTable)HttpRuntime.Cache["C_PlanBerorFastighet"];

            if (cachedPlanBerorFastighet != null)
            {
                return cachedPlanBerorFastighet;
            }
            else
            {
                setPlanBerorFastighetCache();
                return (DataTable)HttpRuntime.Cache["C_PlanBerorFastighet"];
            }
        }


        /// <summary>
        /// Returnerar plan beröra eller berör varandra från cache. Existerar cachen ej skapas den.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPlanBerorPlanCache()
        {
            DataTable cachedPlanBerorPlan = (DataTable)HttpRuntime.Cache["C_PlanBerorPlan"];

            if (cachedPlanBerorPlan != null)
            {
                return cachedPlanBerorPlan;
            }
            else
            {
                setPlanBerorPlanCache();
                return (DataTable)HttpRuntime.Cache["C_PlanBerorPlan"];
            }
        }


        /// <summary>
        /// Returnerar plans plandokument från cache. Existerar cachen ej skapas den.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CachedDocuments> GetPlanDocumentsCache()
        {
            IEnumerable<CachedDocuments> cachedPlanDocuments = (IEnumerable<CachedDocuments>)HttpRuntime.Cache["C_PlanDocuments"];

            if (cachedPlanDocuments != null)
            {
                return cachedPlanDocuments;
            }
            else
            {
                setPlanDocumentsCache();
                return (IEnumerable<CachedDocuments>)HttpRuntime.Cache["C_PlanDocuments"];
            }
        }




        /// <summary>
        /// Plockar bort cachen för planregistrets grundläggande information
        /// </summary>
        public static void RemoveCachedPlanBasis()
        {
            if (CacheExistsPlanBasis())
            {
                HttpRuntime.Cache.Remove("C_Plans");
            }
        }


        /// <summary>
        /// Plockar bort cachen för dokumenttyper
        /// </summary>
        public static void RemoveCachePlandocumenttypes()
        {
            if (CacheExistsPlanBasis())
            {
                HttpRuntime.Cache.Remove("C_Documenttypes");
            }
        }


        /// <summary>
        /// Plockar bort cachen för planers berörskretsar till fastigheter
        /// </summary>
        public static void RemoveCachedPlanBerorFastighet()
        {
            if (CacheExistsPlanBasis())
            {
                HttpRuntime.Cache.Remove("C_PlanBerorFastighet");
            }
        }


        /// <summary>
        /// Plockar bort cache för planers påverkan på varandra
        /// </summary>
        public static void RemoveCachedPlanBerorPlan()
        {
            if (CacheExistsPlanBasis())
            {
                HttpRuntime.Cache.Remove("C_PlanBerorPlan");
            }
        }


        /// <summary>
        /// Plockar bort cache för planers plandokument
        /// </summary>
        public static void RemoveCachedPlanDocuments()
        {
            if (CacheExistsPlanBasis())
            {
                HttpRuntime.Cache.Remove("C_PlanDocuments");
            }
        }




        /// <summary>
        /// Kontrollerar om grundplaninformationen är cachad
        /// </summary>
        /// <returns></returns>
        public static bool CacheExistsPlanBasis()
        {
            var cache = HttpRuntime.Cache["C_Plans"];

            if (cache != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        
        /// <summary>
        /// Kontrollerar om dokumenttyperna är cachade 
        /// </summary>
        /// <returns></returns>
        public static bool CacheExistsPlandocumenttypes()
        {
            var cache = HttpRuntime.Cache["C_Documenttypes"];

            if (cache != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Kontrollerar om dokumenttyperna är cachade 
        /// </summary>
        /// <returns></returns>
        public static bool CacheExistsPlanBerorFastighet()
        {
            var cache = HttpRuntime.Cache["C_PlanBerorFastighet"];

            if (cache != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Kontrollerar om planberoende är cachade 
        /// </summary>
        /// <returns></returns>
        public static bool CacheExistsPlanBerorPlan()
        {
            var cache = HttpRuntime.Cache["C_PlanBerorPlan"];

            if (cache != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Kontrollerar om plandokumenten är cachade 
        /// </summary>
        /// <returns></returns>
        public static bool CacheExistsPlanDocuments()
        {
            var cache = HttpRuntime.Cache["C_PlanDocuments"];

            if (cache != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }






        /// <summary>
        /// Förfluten tid från inställt cache-intervall
        /// </summary>
        /// <returns>Tidsspan</returns>
        public static TimeSpan CacheElapsed()
        {
            return CacheTime("elapsed");
        }


        /// <summary>
        /// Kvarvarande tid för inställt cache-intervall
        /// </summary>
        /// <returns>Tidsspan</returns>
        public static TimeSpan CacheDuration()
        {
            return CacheTime("duration");
        }


        /// <summary>
        /// Basfuktion för cache-intervallets kvarvarande eller förfluten tid
        /// </summary>
        /// <param name="elapsedOrDuration">Antar elapsed eller duration som växlar</param>
        /// <returns>Tidsspan</returns>
        private static TimeSpan CacheTime(string elapsedOrDuration)
        {
            // Kontrollerar om värde är datatyp int annars sätts antal dagar till noll
            string cacheDaysInConfig = ConfigurationManager.AppSettings["CacheNbrOfDays"].ToString();
            Int64 cacheDays;
            if (!Int64.TryParse(cacheDaysInConfig, out cacheDays))
            {
                cacheDays = 0;
            }
            else
            {
                cacheDays = Convert.ToInt64(cacheDaysInConfig);
            }

            // Kontrollerar om värde uppfyller formatet hh:mi:ss, om inte eller tomt sätts tillfället för cachning som standardtid
            string cacheTimeInConfig = ConfigurationManager.AppSettings["CacheTime"].ToString();
            DateTime cacheTime;
            if (!DateTime.TryParseExact(cacheTimeInConfig, "HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out cacheTime))
            {
                throw new Exception("Cache utgångstid fel format/värde i inställningar.");
            }
            else
            {
                cacheTime = DateTime.ParseExact(cacheTimeInConfig, "HH:mm:ss", CultureInfo.CurrentCulture);
            }

            DateTime dateTimeNow = DateTime.Now;



            switch (elapsedOrDuration)
            {
                case "elapsed":

                    #region Starttid
                    DateTime DateTimeTemp = dateTimeNow;

                    if (cacheDays > 0 && dateTimeNow.TimeOfDay <= cacheTime.TimeOfDay)
                    {
                        DateTimeTemp = dateTimeNow.AddDays(-cacheDays);
                    }

                    
                    DateTime cacheStart = new DateTime(DateTimeTemp.Year,
                        DateTimeTemp.Month,
                        DateTimeTemp.Day,
                        cacheTime.Hour,
                        cacheTime.Minute,
                        cacheTime.Second);

                    return dateTimeNow.Subtract(cacheStart);

                #endregion

                case "duration":
                    #region Sluttid
                    //if (cacheDays > 0 && dateTimeNow.TimeOfDay < cacheTime.TimeOfDay)
                    //{
                    //    cacheDays += 1;
                    //}

                    DateTimeTemp = dateTimeNow.AddDays(cacheDays);

                    DateTime cacheEnd = new DateTime(DateTimeTemp.Year,
                        DateTimeTemp.Month,
                        DateTimeTemp.Day,
                        cacheTime.Hour,
                        cacheTime.Minute,
                        cacheTime.Second);

                    return cacheEnd.Subtract(dateTimeNow);
                    #endregion
                default:
                    throw new Exception("Ohanterat värde som inparameter");
            }
        }


        

        /// <summary>
        /// Skapar cache för basinformationen till planer
        /// </summary>
        public static void setPlanCache()
        {
            initPlanCache();
        }
        /// <summary>
        /// Skapa cache för basinformationen till planer med samma signatur som för delegate CacheItemRemovedCallback.
        /// Existerar p.g.a. callback och reinitiering av cache när cache slutar existera.
        /// </summary>
        private static void setPlanCache(string key, object value, CacheItemRemovedReason reason)
        {
            LogCacheRemovedReason(key, reason);
            initPlanCache();
        }
        /// <summary>
        /// Initierar cache för basinformationen till planer
        /// </summary>
        [Obsolete("initPlanCacheOld is deprecated due to connection to Oracle.", true)]
        private static void initPlanCacheOld()
        {
            //string conStr = ConfigurationManager.AppSettings["OracleOleDBConString"].ToString();
            string sql = string.Empty;

            sql = "SELECT TO_CHAR(plan_id) AS nyckel, akt, akt_egn AS akttidigare, akt_pb AS aktegen, planfk, plannamn, " +
                  "       CASE " +
                  "         WHEN SYSDATE BETWEEN TO_DATE(dat_genomf_f, 'YYYY-MM-DD') AND TO_DATE(dat_genomf_t, 'YYYY-MM-DD') " +
                  "         THEN 1 " +
                  "         ELSE 0 " +
                  "       END AS isgenomf, " +
                  "       dat_beslut, dat_genomf_f, dat_genomf_t, dat_lagakraft, planavgift " +
                  "FROM   gis_v_planytor " +
                  "GROUP BY plan_id, akt, akt_egn, akt_pb, planfk, plannamn, " +
                  "       CASE " +
                  "         WHEN SYSDATE BETWEEN TO_DATE(dat_genomf_f, 'YYYY-MM-DD') AND TO_DATE(dat_genomf_t, 'YYYY-MM-DD') " +
                  "         THEN 1 " +
                  "         ELSE 0 " +
                  "       END, " +
                  "       dat_beslut, dat_genomf_f, dat_genomf_t, dat_lagakraft, planavgift";

            DataTable dtPlans = new DataTable();
            //OleDbConnection con = new OleDbConnection(conStr);
            OleDbConnection con = UtilityDatabase.GetOleDbConncection();
            OleDbCommand com = new OleDbCommand(sql, con);
            OleDbDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dtPlans.Load(dr);

            dr.Close();
            dr.Dispose();

            DateTime cacheExpiration = setCacheExpiration();

            // Callback för när cache försvinner
            CacheItemRemovedCallback onCachedRemoved = new CacheItemRemovedCallback(setPlanCache);

            // Skapa cach av alla planer
            Cache cache = HttpRuntime.Cache;
            cache.Insert("C_Plans", dtPlans, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);

            dtPlans.Dispose();
            con.Close();
            con.Dispose();
        }
        /// <summary>
        /// Initierar cache för basinformationen till planer
        /// </summary>
        private static void initPlanCache()
        {
            string sqlPlanGeometries = SqlTemplates.GetPlanGeometriBas;
            string sqlPlanRegister = SqlTemplates.GetPlanRegisterBas;

            // Databasinforamtion från PostgreSQL/PostGIS
            DataTable dtPlanGeometries = new DataTable();
            NpgsqlConnection npgsqlCon = UtilityDatabase.GetNpgsqlConnectionForDBGeodata();
            NpgsqlCommand npgsqlCom = new NpgsqlCommand(sqlPlanGeometries, npgsqlCon);
            NpgsqlDataReader npgsqlDr;

            npgsqlCom.Connection.Open();
            npgsqlDr = npgsqlCom.ExecuteReader();

            dtPlanGeometries.Load(npgsqlDr);

            npgsqlDr.CloseAsync();
            npgsqlDr.DisposeAsync();


            // Databasinforamtion från MS SQL Server
            DataTable dtPlanRegister = new DataTable();
            SqlConnection msSqlServerCon = UtilityDatabase.GetMSSQLServerConnectionForDBFB();
            SqlCommand msSqlServerCom = new SqlCommand(sqlPlanRegister, msSqlServerCon);
            SqlDataReader msSqlServerDr;

            msSqlServerCom.Connection.Open();
            msSqlServerDr = msSqlServerCom.ExecuteReader();

            dtPlanRegister.Load(msSqlServerDr);

            msSqlServerDr.Close();
            msSqlServerDr.Dispose();


            //Join data from PostgreSQL och MS SQL Server
            //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/method-based-query-syntax-examples-join-linq-to-dataset
            //https://www.c-sharpcorner.com/UploadFile/0c1bb2/join-two-datatable-using-linq-in-Asp-Net-C-Sharp/
            DataTable dtPlans = new DataTable();
            dtPlans = LINQResultToDataTable(
                dtPlanGeometries.AsEnumerable().Join(
                dtPlanRegister.AsEnumerable().DefaultIfEmpty(),
                pg => pg.Field<string>("lmakt"),
                pr => pr.Field<string>("lmakt"),
                (pg, pr) => new
                {
                    NYCKEL = pr.Field<string>("plan_id"),
                    AKT = pg.Field<string>("lmakt"),
                    AKTTIDIGARE = pr.Field<string>("egn_akt"),
                    AKTEGEN = pg.Field<string>("akt_pb"),
                    PLANFK = pr.Field<string>("planfk"),
                    PLANNAMN = pr.Field<string>("plannamn"),
                    ISGENOMF = pr.Field<Int32>("isgenomf"),
                    DAT_BESLUT = pr.Field<string>("dat_beslut"),
                    DAT_GENOMF_F = pr.Field<string>("dat_genomf_f"),
                    DAT_GENOMF_T = pr.Field<string>("dat_genomf_t"),
                    DAT_LAGAKRAFT = pr.Field<string>("dat_lagakraft"),
                    PLANAVGIFT = pg.Field<string>("planavgift"),
                }).GroupBy(g => new
                {
                    g.NYCKEL,
                    g.AKT,
                    g.AKTTIDIGARE,
                    g.AKTEGEN,
                    g.PLANFK,
                    g.PLANNAMN,
                    g.ISGENOMF,
                    g.DAT_BESLUT,
                    g.DAT_GENOMF_F,
                    g.DAT_GENOMF_T,
                    g.DAT_LAGAKRAFT,
                    g.PLANAVGIFT
                }).SelectMany(p => p.ToList()));


            DateTime cacheExpiration = setCacheExpiration();

            // Callback för när cache försvinner
            CacheItemRemovedCallback onCachedRemoved = new CacheItemRemovedCallback(setPlanCache);

            // Skapa cach av alla planer
            Cache cache = HttpRuntime.Cache;
            cache.Insert("C_Plans", dtPlans, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);

            dtPlanGeometries.Dispose();
            npgsqlCon.Close();
            npgsqlCon.Dispose();
        }



        /// <summary>
        /// Skapa cache för plandokumenttyper
        /// </summary>
        public static void setDocumenttypesCache()
        {
            initDocumenttypesCache();
        }
        /// <summary>
        /// Skapa cache för plandokumenttyper med samma signatur som för delegate CacheItemRemovedCallback.
        /// Existerar p.g.a. callback och reinitiering av cache när cache slutar existera.
        /// </summary>
        private static void setDocumenttypesCache(string key, object value, CacheItemRemovedReason reason)
        {
            LogCacheRemovedReason(key, reason);
            initDocumenttypesCache();
        }
        /// <summary>
        /// Initierar cache för dokumenttyper
        /// </summary>
        private static void initDocumenttypesCache()
        {
            DateTime cacheExpiration = setCacheExpiration();

            Documenttypes documenttypes = new Documenttypes();
            List<Documenttype> listOfDocumenttypes = documenttypes.GetDocumenttypes;

            // Callback för när cache försvinner
            CacheItemRemovedCallback onCachedRemoved = new CacheItemRemovedCallback(setDocumenttypesCache);

            // Skapa cach av alla planer
            Cache cache = HttpRuntime.Cache;
            cache.Insert("C_Documenttypes", listOfDocumenttypes, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);
        }
        

        /// <summary>
        /// Skapar cache med fastigheter som berörs av resp. plan
        /// </summary>
        public static void setPlanBerorFastighetCache()
        {
            initPlanBerorFastighetCache();
        }
        /// <summary>
        /// Skapa cache med fastigheter som berörs av resp. plan med samma signatur som för delegate CacheItemRemovedCallback.
        /// Existerar p.g.a. callback och reinitiering av cache när cache slutar existera.
        /// </summary>
        private static void setPlanBerorFastighetCache(string key, object value, CacheItemRemovedReason reason)
        {
            LogCacheRemovedReason(key, reason);
            initPlanBerorFastighetCache();
        }
        /// <summary>
        /// Initierar cache med fastigheter som berörs av resp. plan
        /// </summary>
        [Obsolete("initPlanCacheOld is deprecated due to connection to Oracle.", true)]
        private static void initPlanBerorFastighetCacheOld()
        {
            //string conStr = ConfigurationManager.AppSettings["OracleOleDBConString"].ToString();
            string sql = string.Empty;

            sql = "SELECT TO_CHAR(plan_id) AS nyckel, fastighet_id AS nyckel_fastighet, UPPER(fastighet) AS fastighet " +
                  "FROM   gis_v_planberorfastighet";

            DataTable dtPlanBerorFastighet = new DataTable();
            //OleDbConnection con = new OleDbConnection(conStr);
            OleDbConnection con = UtilityDatabase.GetOleDbConncection();
            OleDbCommand com = new OleDbCommand(sql, con);
            OleDbDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dtPlanBerorFastighet.Load(dr);

            dr.Close();
            dr.Dispose();

            DateTime cacheExpiration = setCacheExpiration();

            // Callback för när cache försvinner
            CacheItemRemovedCallback onCachedRemoved = new CacheItemRemovedCallback(setPlanBerorFastighetCache);

            // Skapa cach av alla planer
            Cache cache = HttpRuntime.Cache;
            cache.Insert("C_PlanBerorFastighet", dtPlanBerorFastighet, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);

            dtPlanBerorFastighet.Dispose();
            con.Close();
            con.Dispose();
        }
        /// <summary>
        /// Initierar cache med fastigheter som berörs av resp. plan
        /// </summary>
        private static void initPlanBerorFastighetCache()
        {
            string sql = SqlTemplates.GetPlanBerorFastighet;

            DataTable dtPlanBerorFastighet = new DataTable();
            SqlConnection con = UtilityDatabase.GetMSSQLServerConnectionForDBFB();
            SqlCommand com = new SqlCommand(sql, con);
            SqlDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dtPlanBerorFastighet.Load(dr);

            dr.Close();
            dr.Dispose();

            DateTime cacheExpiration = setCacheExpiration();

            // Callback för när cache försvinner
            CacheItemRemovedCallback onCachedRemoved = new CacheItemRemovedCallback(setPlanBerorFastighetCache);

            // Skapa cach av alla planer
            Cache cache = HttpRuntime.Cache;
            cache.Insert("C_PlanBerorFastighet", dtPlanBerorFastighet, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);

            dtPlanBerorFastighet.Dispose();
            con.Close();
            con.Dispose();
        }


        /// <summary>
        /// Skapar cache med planer som berör eller har berörts av andra planer
        /// </summary>
        public static void setPlanBerorPlanCache()
        {
            initPlanBerorPlanCache();
        }
        /// <summary>
        /// Skapa cache med planer som berörs eller har berörts av andra planer med samma signatur som för delegate CacheItemRemovedCallback.
        /// Existerar p.g.a. callback och reinitiering av cache när cache slutar existera.
        /// </summary>
        private static void setPlanBerorPlanCache(string key, object value, CacheItemRemovedReason reason)
        {
            LogCacheRemovedReason(key, reason);
            initPlanBerorPlanCache();
        }
        /// <summary>
        /// Initierar cache med planer som berör eller har berörts av andra planer
        /// </summary>
        [Obsolete("initPlanCacheOld is deprecated due to connection to Oracle.", true)]
        private static void initPlanBerorPlanCacheOld()
        {
            string sql = string.Empty;

            sql = "SELECT TO_CHAR(plan_id) AS nyckel, planfk, beskrivning AS beskrivning, TO_CHAR(pav_plan_id) AS nyckel_pavarkan, pav AS paverkan, pav_planfk, pav_status AS status_pavarkan, registrerat_beslut AS registrerat_beslut " +
                  "FROM   gis_v_planpaverkade";

            DataTable dtPlanBerorPlan = new DataTable();
            //OleDbConnection con = new OleDbConnection(conStr);
            OleDbConnection con = UtilityDatabase.GetOleDbConncection();
            OleDbCommand com = new OleDbCommand(sql, con);
            OleDbDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dtPlanBerorPlan.Load(dr);

            dr.Close();
            dr.Dispose();

            DateTime cacheExpiration = setCacheExpiration();

            // Callback för när cache försvinner
            CacheItemRemovedCallback onCachedRemoved = new CacheItemRemovedCallback(setPlanBerorPlanCache);

            // Skapa cach av alla planer
            Cache cache = HttpRuntime.Cache;
            cache.Insert("C_PlanBerorPlan", dtPlanBerorPlan, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);

            dtPlanBerorPlan.Dispose();
            con.Close();
            con.Dispose();
        }
        /// <summary>
        /// Initierar cache med planer som berör eller har berörts av andra planer
        /// </summary>
        private static void initPlanBerorPlanCache()
        {
            string sql = SqlTemplates.GetPlanBerorPlan;

            DataTable dtPlanBerorPlan = new DataTable();
            SqlConnection con = UtilityDatabase.GetMSSQLServerConnectionForDBFB();
            SqlCommand com = new SqlCommand(sql, con);
            SqlDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dtPlanBerorPlan.Load(dr);

            dr.Close();
            dr.Dispose();

            DateTime cacheExpiration = setCacheExpiration();

            // Callback för när cache försvinner
            CacheItemRemovedCallback onCachedRemoved = new CacheItemRemovedCallback(setPlanBerorPlanCache);

            // Skapa cach av alla planer
            Cache cache = HttpRuntime.Cache;
            cache.Insert("C_PlanBerorPlan", dtPlanBerorPlan, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);

            dtPlanBerorPlan.Dispose();
            con.Close();
            con.Dispose();
        }



        /// <summary>
        /// Skapar cache med plandokument från kataloger definierade i inställningar
        /// </summary>
        public static void setPlanDocumentsCache()
        {
            PlanCache pc = new PlanCache();
            pc.initPlanDocumentsCache();
            pc.Dispose();
        }
        /// <summary>
        /// Skapar cache med plandokument från kataloger definierade i inställningar med samma signatur som för delegate CacheItemRemovedCallback.
        /// Existerar p.g.a. callback och reinitiering av cache när cache slutar existera.
        /// </summary>
        private static void setPlanDocumentsCache(string key, object value, CacheItemRemovedReason reason)
        {
            LogCacheRemovedReason(key, reason);

            PlanCache pc = new PlanCache();
            pc.initPlanDocumentsCache();
            pc.Dispose();
        }
        /// <summary>
        /// Initierar cache med plandokument från kataloger definierade i inställningar
        /// </summary>
        private void initPlanDocumentsCache()
        {
            string[] directoryRoots = ConfigurationManager.AppSettings["filesRotDirectory"].ToString().Split(',');
            string[] searchedFileExtentions = ConfigurationManager.AppSettings["fileExtentions"].ToString().Split(',');
            string appSetSubDirCrawl = ConfigurationManager.AppSettings["subDirectoryCrawl"].ToString();

            List<FileInfo> filesInRootDirectoriesTemp = new List<FileInfo>();
            CachedDocuments documents;
            List<CachedDocuments> allDocuments = new List<CachedDocuments>();
            foreach (string root in directoryRoots)
            {
                //DirectoryInfo searchedDirectory = new DirectoryInfo(Server.MapPath("~") + "\\" + root);
                //DirectoryInfo searchedDirectory = new DirectoryInfo(HostingEnvironment.MapPath("~") + "\\" + root);
                DirectoryInfo searchedDirectory;
                // Avgör om sökvägen är absolut eller relativ (det samma som virtuell i denna applikation)
                if (Path.IsPathRooted(root))
                {
                    searchedDirectory = new DirectoryInfo(
                        root
                        );
                }
                else
                {
                    searchedDirectory = new DirectoryInfo(
                        HostingEnvironment.MapPath(@"~/" + root)
                        );
                }

                foreach (string ext in searchedFileExtentions)
                {
                    documents = new CachedDocuments();
                    documents.Documents = new List<FileInfo>();

                    if (Boolean.TryParse(appSetSubDirCrawl, out bool searchSubDirectory))
                    {
                        if (searchSubDirectory)
                        {
                            allDocuments.AddRange(
                                searchedDirectory.EnumerateFiles("*" + ext, SearchOption.AllDirectories)
                                .GroupBy(f => f.DirectoryName)
                                .Select(
                                    grp => new CachedDocuments()
                                    {
                                        PathSettingsRoot = EnsureEndingPathCarachter(EnsureCleanPath(root + grp.Key.Replace(searchedDirectory.FullName, ""))),
                                        Documents = grp.ToList()
                                    }
                                    )
                                );
                        }
                        else
                        {
                            documents.PathSettingsRoot = root;
                            documents.Documents.AddRange(searchedDirectory.EnumerateFiles("*" + ext));
                        }
                    }
                    else
                    {
                        allDocuments.AddRange(
                            searchedDirectory.EnumerateFiles("*" + ext, SearchOption.AllDirectories)
                            .GroupBy(f => f.DirectoryName)
                            .Select(
                                grp => new CachedDocuments()
                                {
                                    PathSettingsRoot = EnsureEndingPathCarachter(EnsureCleanPath(root + grp.Key.Replace(searchedDirectory.FullName, ""))),
                                    Documents = grp.ToList()
                                }
                                )
                            );

                    }

                }

            }

            IEnumerable<CachedDocuments> filesInRootDirectories = allDocuments;

            DateTime cacheExpiration = setCacheExpiration();

            // Callback för när cache försvinner
            CacheItemRemovedCallback onCachedRemoved = new CacheItemRemovedCallback(setPlanDocumentsCache);

            // Skapa cache av alla plandokument
            Cache cache = HttpRuntime.Cache;
            cache.Insert("C_PlanDocuments", filesInRootDirectories, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);
        }


        /// <summary>
        /// Sätter utgångstid till cachar baserat på inställningar
        /// </summary>
        /// <returns>DateTime objekt för när cache går ut</returns>
        private static DateTime setCacheExpiration()
        {
            DateTime toDay = DateTime.Now;

            // Dela upp tiden på timmar, minuter och sekunder
            string[] time = ConfigurationManager.AppSettings["CacheTime"].ToString().Split(':');
            DateTime cacheExpiration;

            // Kontrollerar om värde är datatyp int annars sätts antal dagar innan tömning av cach som standard till en dag
            string cachDaysInConfig = ConfigurationManager.AppSettings["CacheNbrOfDays"].ToString();
            Int64 cachDays;
            if (!Int64.TryParse(cachDaysInConfig, out cachDays))
            {
                cachDays = 1;
            }
            else
            {
                cachDays = Convert.ToInt64(cachDaysInConfig);
            }

            // Kontrollerar om värde uppfyller formatet hh:mi:ss, om inte eller tomt sätts tillfället för cachning som standardtid
            string cachTimeInConfig = ConfigurationManager.AppSettings["CacheTime"].ToString();
            DateTime cachTime;
            if (!DateTime.TryParseExact(cachTimeInConfig, "HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out cachTime))
            {
                cachTime = DateTime.Now;
            }
            else
            {
                cachTime = DateTime.ParseExact(cachTimeInConfig, "HH:mm:ss", CultureInfo.CurrentCulture);
            }

            DateTime expirationDate = toDay.AddDays(Convert.ToDouble(cachDays));

            cacheExpiration = new DateTime(expirationDate.Year,
                                           expirationDate.Month,
                                           expirationDate.Day,
                                           cachTime.Hour,
                                           cachTime.Minute,
                                           cachTime.Second);
            return cacheExpiration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirtyPath"></param>
        /// <returns></returns>
        private static string EnsureCleanPath(string dirtyPath)
        {
            string[] pathPart = dirtyPath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

            if (Path.IsPathRooted(dirtyPath))
            {
                return String.Join("\\", pathPart);
            }
            else
            {
                return String.Join("/", pathPart);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string EnsureEndingPathCarachter(string path)
        {
            const string slash = "/";
            const string backslash = "\\";

            if (!path.EndsWith(slash) && !path.EndsWith(backslash))
            {
                if (Path.IsPathRooted(path))
                {
                    return path += backslash;
                }
                else
                {
                    return path += slash;
                }
            }
            else
            {
                return path;
            }
        }

        /// <summary>
        /// Abstraktionsmetod för att logga cached removed p.g.a. flera förekomster.
        /// </summary>
        /// <param name="key">Chache-nyckel</param>
        /// <param name="reason">Enum för anledningen till tömnning av cache</param>
        private static void LogCacheRemovedReason(string key, CacheItemRemovedReason reason)
        {
            UtilityLog.Log($"Cache '{key}' tömdes med anledningen '{reason.ToString()}'", Utility.LogLevel.WARN);
        }

        /// <summary>
        /// Konverterar LINQ-resultat Enumerable Objekt till DataTable
        /// </summary>
        /// <typeparam name="T">Objekt</typeparam>
        /// <param name="items">Resultat från LINQ</param>
        /// <returns></returns>
        private static DataTable LINQResultToDataTable<T>(IEnumerable<T> items)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] columns = null;
            if (items == null) return dt;
            foreach (T Record in items)
            {
                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type IcolType = GetProperty.PropertyType;
                        if ((IcolType.IsGenericType) && (IcolType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            IcolType = IcolType.GetGenericArguments()[0];
                        }
                        dt.Columns.Add(new DataColumn(GetProperty.Name, IcolType));
                    }
                }
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo p in columns)
                {
                    dr[p.Name] = p.GetValue(Record, null) == null ? DBNull.Value : p.GetValue(Record, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Konverterar LINQ-resultat List-objekt till DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        private static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        /// <summary>
        /// Exporterar vald DataTable till CSV-fil. CSV-filen sparas i katalogen till applikationsroten. Filnamn sätts samma som tabellnamnet, annars default unikt enl. formatet "DataTableNoName_{GUID}".
        /// </summary>
        /// <param name="table">DataTable som önskas exporteras</param>
        /// <see cref="DataTableToCSVDefaultOutputDirectory(DataTable, string)"/>
        private static void DataTableToCSVDefaultOutputDirectory(DataTable table)
        {
            DataTableToCSVDefaultOutputDirectory(
                table,
                (string.IsNullOrWhiteSpace(table.TableName) ? "DataTableNoName_" + Guid.NewGuid().ToString() : table.TableName)
                );
        }
        /// <summary>
        /// Exporterar vald DataTable till CSV-fil. CSV-filen sparas i katalogen till applikationsroten. Om filnamn är tomt sätts default unikt enl. formatet "DataTableNoName_{GUID}".
        /// </summary>
        /// <param name="table">DataTable som önskas exporteras</param>
        /// <param name="filename">Valfritt filnamn på CSV-fil</param>
        /// <see cref="DataTableToCSVDefaultOutputDirectory(DataTable)"/>
        private static void DataTableToCSVDefaultOutputDirectory(DataTable table, string filename)
        {
            //StreamWriter sw = new StreamWriter(HttpRuntime.AppDomainAppPath, false);
            //StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath("~"), false);
            //StreamWriter sw = new StreamWriter(HostingEnvironment.MapPath("~"), false);
            //StreamWriter sw = new StreamWriter("c:\\temp\\plandokument\\", false);
            StreamWriter sw = new StreamWriter(HttpRuntime.AppDomainAppPath + (string.IsNullOrWhiteSpace(filename) ? "DataTableNoName_" + Guid.NewGuid().ToString() : filename) + ".csv", false);
            // headers    
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sw.Write(table.Columns[i]);
                if (i < table.Columns.Count - 1)
                {
                    sw.Write(";");
                }
            }
            sw.Write(sw.NewLine);

            // content
            foreach (DataRow dr in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(';'))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < table.Columns.Count - 1)
                    {
                        sw.Write(";");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
    }
}
