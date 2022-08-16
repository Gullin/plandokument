using Npgsql;
using System;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Plan.Plandokument
{
    public class UtilityDatabase : Utility
    {
        internal static OleDbConnection GetOleDbConncection()
        {
            string user = Environment.GetEnvironmentVariable("lkrgisuser", EnvironmentVariableTarget.Machine);
            string password = Environment.GetEnvironmentVariable("lkrgispassword", EnvironmentVariableTarget.Machine);
            string service = Environment.GetEnvironmentVariable("lkrgisservice", EnvironmentVariableTarget.Machine);
            string connectionStr = string.Empty;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                string appSettingConnection = ConfigurationManager.AppSettings["OracleOleDBConString"].ToString();
                if (!string.IsNullOrWhiteSpace(appSettingConnection))
                {
                    connectionStr = appSettingConnection;
                }
                else
                {
                    throw new Exception("Anslutningsuppgifter till databas existerar ej");
                }
            }
            else
            {
                connectionStr = "Provider=OraOLEDB.Oracle;Data Source=" + service + ";User Id=" + user + ";Password=" + password + ";";
            }

            return new OleDbConnection(connectionStr);
        }


        internal static NpgsqlConnection GetNpgsqlConnectionForDBGeodata()
        {
            return new NpgsqlConnection(ConfigurationManager.AppSettings["PostgreSQLNpgsqlConStringGeodata"].ToString());
        }


        internal static SqlConnection GetMSSQLServerConnectionForDBFB()
        {
            return new SqlConnection(ConfigurationManager.AppSettings["MSSQLServerConStringFB"].ToString());
        }

        /// <summary>
        /// Testmetod för möjlighet att ansluta till specifik PostgreSQL-databas geodata
        /// </summary>
        internal static bool ExistsNpgsqlConnectionForDBGeodata()
        {
            NpgsqlConnection con = GetNpgsqlConnectionForDBGeodata();
            try
            {
                con.Open();
                return true;
            }
            catch (Exception exc)
            {
                UtilityException.LogException(exc, "Testanslutning", false);
                return false;
            }
            finally
            {
                con.Dispose();
            }
        }
        /// <summary>
        /// Testmetod för möjlighet att ansluta till specifik PostgreSQL-databas geodata
        /// </summary>
        internal static bool ExistsMSSQLServerConnectionForDBFB()
        {
            SqlConnection con = UtilityDatabase.GetMSSQLServerConnectionForDBFB();
            try
            {
                con.Open();
                return true;
            }
            catch (Exception exc)
            {
                UtilityException.LogException(exc, "Testanslutning", false);
                return false;
            }
            finally
            {
                con.Dispose();
            }
        }

    }
}