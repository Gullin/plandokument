using System;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Web.Script.Serialization;
using System.Dynamic;

namespace Plan.Plandokument.SQLite
{
	internal class ApplicationDatabase
	{
		private static string _database;
		private static string _connectionString;

		static ApplicationDatabase()
		{
			_database = Utility.logDirectory + "PlandokumentAppDb.sqlite";
			_connectionString = $"Data Source={_database};Version=3;";
		}

		/// <summary>
		/// Returnerar hårdkodad databasfilnamn
		/// </summary>
		/// <returns>Filnamn på SQLite-databasen inkl. filändelse</returns>
		internal static string GetDatabase()
		{
			return _database;
		}

		internal static void CreateDatabase()
		{
			SQLiteConnection.CreateFile(_database);
		}

		internal static string GetConnectionString()
		{
			return _connectionString;
		}

		internal static bool DatabaseExists()
		{
			return File.Exists(_database);
		}

		internal static void InitializeDatabase()
		{
			SQLiteConnection dbCon = new SQLiteConnection(_connectionString);
			try
			{
				SQLiteCommand cmd = new SQLiteCommand();
				cmd.Connection = dbCon;
				dbCon.Open();

				cmd.CommandText = SqlTemplates.ExistsAppDbStatRequest;
				cmd.Parameters.AddWithValue("@table_name", "stat_requests");
				cmd.Prepare();

				SQLiteDataReader sQLiteDataReader = cmd.ExecuteReader();
				string tableExistsValue = "false";
				while (sQLiteDataReader.Read())
				{
					tableExistsValue = sQLiteDataReader.GetString(0);
				}
				sQLiteDataReader.Close();

				Boolean tableExists;
				if (Boolean.TryParse(tableExistsValue, out tableExists))
				{
					if (!tableExists)
					{
						cmd.CommandText = SqlTemplates.CreateAppDbStatRequest;
						cmd.ExecuteNonQuery();
					}
				}
				dbCon.Close();
				dbCon.Dispose();
			}
			catch (Exception exc)
			{
				UtilityException.LogException(exc, "Applikationsdatabas Initiering", false);
			}
			finally
			{
				dbCon.Dispose();
			}
		}
	}

    internal static class StatData
    {
		internal static DataTable StatTotalRequests()
        {

            return GetData(SqlTemplates.GetTotalDbStatRequests);

        }

        internal static DataTable StatTotalByDayRequests()
        {

            return GetData(SqlTemplates.GetTotalByDayDbStatRequests);

        }

        internal static DataTable StatTotalByMonthRequests()
        {

            return GetData(SqlTemplates.GetTotalByMonthDbStatRequests);

        }

        internal static DataTable StatTotalByYearRequests()
        {

            return GetData(SqlTemplates.GetTotalByYearDbStatRequests);

        }

        internal static DataTable StatPeriodRequests()
        {

            return GetData(SqlTemplates.GetPeriodDbStatRequests);

        }

        internal static DataTable StatRunningTotalRequests()
        {

            return GetData(SqlTemplates.GetRunningTotalDbStatRequests);

        }

        internal static DataTable StatRunningTotalByDayRequests()
        {

            return GetData(SqlTemplates.GetRunningTotalByDayDbStatRequests);

        }

        internal static DataTable StatRunningTotalByMonthRequests()
        {

            return GetData(SqlTemplates.GetRunningTotalByMonthDbStatRequests);

        }

        internal static DataTable StatRunningTotalByYearRequests()
        {

            return GetData(SqlTemplates.GetRunningTotalByYearDbStatRequests);

        }

        internal static DataTable StatRunningHitsRequests()
        {

            return GetData(
                SqlTemplates.GetRunningHitsDbStatRequests
                );

        }

        internal static DataTable StatRunningSearchRequests()
        {
            
            return GetData(
                SqlTemplates.GetRunningSearchDbStatRequests
                );

        }

        internal static DataTable StatSearchRequests()
        {

            return GetData(
                SqlTemplates.GetSearchDbStatRequests
                );

        }

        internal static DataTable StatSearchtimeRequests()
        {

            return GetData(
                SqlTemplates.GetSearchtimeDbStatRequests
                );

        }

        internal static DataTable StatSearchtimeByDayRequests()
        {

            return GetData(
                SqlTemplates.GetSearchtimeByDayDbStatRequests
                );

        }

        internal static DataTable StatSearchtimeByMonthRequests()
        {

            return GetData(
                SqlTemplates.GetSearchtimeByMonthDbStatRequests
                );

        }

        internal static DataTable StatSearchtimeByYearRequests()
        {

            return GetData(
                SqlTemplates.GetSearchtimeByYearDbStatRequests
                );

        }

        private static DataTable GetData(string sql)
        {

            SQLiteConnection dbCon = new SQLiteConnection(ApplicationDatabase.GetConnectionString());

            DataTable dataTable = null;

            try
            {
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.CommandText = sql;
                cmd.Connection = dbCon;
                dbCon.Open();

                SQLiteDataReader reader = cmd.ExecuteReader();

                dataTable = new DataTable();
                dataTable.Load(reader);

                dbCon.Close();
                dbCon.Dispose();
            }
            catch (Exception exc)
            {
                UtilityException.LogException(exc, "Request Statistics from DB", false);
            }
            finally
            {
                dbCon.Dispose();
            }

            return dataTable;

        }

    }
}