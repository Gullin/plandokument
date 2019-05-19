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
    public class PlanCache
    {
        public static void isPlandocumenttypesCache()
        {
            List<Documenttype> cachedDocumenttypes = (List<Documenttype>)HttpRuntime.Cache["Documenttypes"];

            if (cachedDocumenttypes != null)
            {
                HttpRuntime.Cache.Remove("Documenttypes");
                setDocumenttypesCache();
            }
            else
            {
                setDocumenttypesCache();
            }
        }


        // Kontrollera om grundplaninformationen 
        public static void isPlanBasisCache()
        {
            DataTable cachedPlans = (DataTable)HttpRuntime.Cache["Plans"];

            if (cachedPlans != null)
            {
                HttpRuntime.Cache.Remove("Plans");
                setPlanCache();
            }
            else
            {
                setPlanCache();
            }
        }


        // Kontrollera om information om vilka planer fastighet berör 
        public static void isPlanBerorFastighetCache()
        {
            DataTable cachedPlanBerorFastighet = (DataTable)HttpRuntime.Cache["PlanBerorFastighet"];

            if (cachedPlanBerorFastighet != null)
            {
                HttpRuntime.Cache.Remove("PlanBerorFastighet");
                setPlanBerorFastighetCache();
            }
            else
            {
                setPlanBerorFastighetCache();
            }
        }



        // Cacha plandokumenttyper från domänfil
        private static void setDocumenttypesCache()
        {
            DateTime cacheExpiration = setCacheExpiration();

            Documenttypes documenttypes = new Documenttypes();
            List<Documenttype> listOfDocumenttypes = documenttypes.GetDocumenttypes;

            // Skapa cach av alla planer
            Cache cache = HttpRuntime.Cache;
            cache.Insert("Documenttypes", listOfDocumenttypes, null, cacheExpiration, Cache.NoSlidingExpiration);
        }


        // Cacha begränsad information för alla planer
        public static void setPlanCache()
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

            // Skapa cach av alla planer
            Cache cache = HttpRuntime.Cache;
            cache.Insert("Plans", dtPlans, null, cacheExpiration, Cache.NoSlidingExpiration);

            dtPlans.Dispose();
            con.Close();
            con.Dispose();
        }

        public static void setPlanBerorFastighetCache()
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

            // Skapa cach av alla planer
            Cache cache = HttpRuntime.Cache;
            cache.Insert("PlanBerorFastighet", dtPlanBerorFastighet, null, cacheExpiration, Cache.NoSlidingExpiration);

            dtPlanBerorFastighet.Dispose();
            con.Close();
            con.Dispose();
        }

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
            if (!DateTime.TryParseExact(cachTimeInConfig, "hh:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out cachTime))
            {
                cachTime = DateTime.Now;
            }
            else
            {
                cachTime = DateTime.ParseExact(cachTimeInConfig, "hh:mm:ss", CultureInfo.CurrentCulture);
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
    }
}