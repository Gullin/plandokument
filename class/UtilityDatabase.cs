using System;
using System.Configuration;
using System.Data.OleDb;

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
    }
}