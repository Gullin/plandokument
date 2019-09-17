using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Web;
using System.Web.Caching;

namespace Plan.Plandokument
{
    public static class PlanCache
    {
        /// <summary>
        /// Returnerar plandokumenttyperna från cache. Existerar cachen ej skapas den.
        /// </summary>
        /// <returns></returns>
        public static List<Documenttype> GetPlandocumenttypesCache()
        {
            List<Documenttype> cachedDocumenttypes = (List<Documenttype>)HttpRuntime.Cache["Documenttypes"];

            if (cachedDocumenttypes != null)
            {
                return cachedDocumenttypes;
            }
            else
            {
                setDocumenttypesCache();
                return (List<Documenttype>)HttpRuntime.Cache["Documenttypes"];
            }
        }


        /// <summary>
        /// Returnerar basinformationen för planern från cache. Existerar cachen ej skapas den.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPlanBasisCache()
        {
            DataTable cachedPlans = (DataTable)HttpRuntime.Cache["Plans"];

            if (cachedPlans != null)
            {
                return cachedPlans;
            }
            else
            {
                setPlanCache();
                return (DataTable)HttpRuntime.Cache["Plans"];
            }
        }


        /// <summary>
        /// Returnerar plan och fastigheter som berör varandra från cache. Existerar cachen ej skapas den.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPlanBerorFastighetCache()
        {
            DataTable cachedPlanBerorFastighet = (DataTable)HttpRuntime.Cache["PlanBerorFastighet"];

            if (cachedPlanBerorFastighet != null)
            {
                return cachedPlanBerorFastighet;
            }
            else
            {
                setPlanBerorFastighetCache();
                return (DataTable)HttpRuntime.Cache["PlanBerorFastighet"];
            }
        }


        /// <summary>
        /// Returnerar plan beröra eller berör varandra från cache. Existerar cachen ej skapas den.
        /// </summary>
        /// <returns></returns>
        public static DataTable GetPlanBerorPlanCache()
        {
            DataTable cachedPlanBerorPlan = (DataTable)HttpRuntime.Cache["PlanBerorPlan"];

            if (cachedPlanBerorPlan != null)
            {
                return cachedPlanBerorPlan;
            }
            else
            {
                setPlanBerorPlanCache();
                return (DataTable)HttpRuntime.Cache["PlanBerorPlan"];
            }
        }





        /// <summary>
        /// Kontrollerar om grundplaninformationen är cachad
        /// </summary>
        /// <returns></returns>
        public static bool CacheExistsPlanBasis()
        {
            var cache = HttpRuntime.Cache["Plans"];

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
            var cache = HttpRuntime.Cache["Documenttypes"];

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
            var cache = HttpRuntime.Cache["PlanBerorFastighet"];

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
            var cache = HttpRuntime.Cache["PlanBerorPlan"];

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
        /// Skapa cache för plandokumenttyper
        /// </summary>
        private static void setDocumenttypesCache()
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
            cache.Insert("Documenttypes", listOfDocumenttypes, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);
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
        public static void initPlanCache()
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
            cache.Insert("Plans", dtPlans, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);

            dtPlans.Dispose();
            con.Close();
            con.Dispose();
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
        public static void initPlanBerorFastighetCache()
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
            cache.Insert("PlanBerorFastighet", dtPlanBerorFastighet, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);

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
        public static void setPlanBerorPlanCache(string key, object value, CacheItemRemovedReason reason)
        {
            LogCacheRemovedReason(key, reason);
            initPlanBerorPlanCache();
        }
        /// <summary>
        /// Initierar cache med planer som berör eller har berörts av andra planer
        /// </summary>
        public static void initPlanBerorPlanCache()
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
            cache.Insert("PlanBerorPlan", dtPlanBerorPlan, null, cacheExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, onCachedRemoved);

            dtPlanBerorPlan.Dispose();
            con.Close();
            con.Dispose();
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
        /// Abstraktionsmetod för att logga cached removed p.g.a. flera förekomster.
        /// </summary>
        /// <param name="key">Chache-nyckel</param>
        /// <param name="reason">Enum för anledningen till tömnning av cache</param>
        private static void LogCacheRemovedReason(string key, CacheItemRemovedReason reason)
        {
            UtilityLog.Log($"Cache '{key}' tömdes med anledningen '{reason.ToString()}'", Utility.LogLevel.WARN);
        }
    }
}
