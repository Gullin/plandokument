using System;
using System.IO;
using System.Data.SQLite;

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
}