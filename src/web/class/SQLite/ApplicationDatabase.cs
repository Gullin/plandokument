using System;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Web.Script.Serialization;
using System.Dynamic;
using System.CodeDom;
using System.Text.RegularExpressions;
using System.Globalization;

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

        internal static DataTable StatPeriodTotalRequests()
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

    internal static class SqlFilterHelper
    {

        internal static string StatBuildFilterBetweenYears(string sql, string column, string from, string to)
        {
            int f, t;

            if ((
                    (from.Length == 2 || from.Length == 4)
                    && (to.Length == 2 || to.Length == 4)
                )
                && int.TryParse(from, out f)
                && int.TryParse(to, out t)
                && f <= t
                )
            {
                return FilterStatBetweenDates(sql, column,
                    (from.Length == 2 ? DateTime.Now.Year.ToString("yyyy").Substring(0,2) + from: from) + "-01-01",
                    (to.Length == 2 ? DateTime.Now.Year.ToString("yyyy").Substring(0, 2) + to : to) + "-12-31",
                    period.year
                    );
            }
            else
            {
                throw new FormatException();
            }
        }

        internal static string StatBuildFilterBetweenMonth(string sql, string column, string from, string to)
        {
            //TODO: om from = "2023-1-1" och to = "2024-12-1", ger det 2023-01-01 : 2024-12-31 medan förväntas 2023-01-01 : 2024-12-01

            // Om 4 tecken antas vara år
            if (from.Length == 4 && int.TryParse(from, out _)) from = from.ToString() + "-01-01";
            if (to.Length == 4 && int.TryParse(to, out _)) to= to.ToString() + "-12-31";

            // Om 4-1|2 kombinationer av tecken parsas automatiskt 
            Regex re = new Regex(@"^\d{4}\-([1-9]|(0[1-9]|10|11|12))$");
            if (re.IsMatch(from)) from = DateTime.Parse(from).ToString("yyyy-MM-dd");
            if (re.IsMatch(to)) to = String.Join("-",
                DateTime.Parse(to).ToString("yyyy"),
                DateTime.Parse(to).ToString("MM"),
                DateTime.DaysInMonth(DateTime.Parse(to).Year, DateTime.Parse(to).Month)
                );

            // Om 1|2-1|2 kombinationer av tecken antas vara innevarande sekel med år och månad
            re = new Regex(@"^([0-9]|[0-9][0-9])\-([1-9]|(0[1-9]|10|11|12))$");
            if (re.IsMatch(from))
            {
                string[] parts = from.Split(new char[] { '-' }, 2);
                // Om parts[0], som ses som årtal, är större än innevarande år ses det som seklet innan. Annars innevarande sekel
                from = DateTime.Parse(
                        int.Parse(parts[0]) > int.Parse(DateTime.Now.Year.ToString().Substring(2, 2)) ?
                            (int.Parse(DateTime.Now.Year.ToString().Substring(0, 2)) - 1).ToString() + "-" + parts[1] :
                            DateTime.Now.Year.ToString().Substring(0, 2) + "-" + parts[1]
                    ).ToString("yyyy-MM-dd");
            }
            if (re.IsMatch(to))
            {
                string[] parts = to.Split(new char[] { '-' }, 2);
                // Om parts[0], som ses som årtal, är större än innevarande år ses det som seklet innan. Annars innevarande sekel
                to = DateTime.Parse(
                        int.Parse(parts[0]) > int.Parse(DateTime.Now.Year.ToString().Substring(2, 2)) ?
                            (int.Parse(DateTime.Now.Year.ToString().Substring(0, 2)) - 1).ToString() + "-" + parts[1] :
                            DateTime.Now.Year.ToString().Substring(0, 2) + "-" + parts[1]
                    ).ToString("yyyy-MM-dd");
            }

            //TODO: Kontrollera denna!?
            // Om 4-1|2-1|2 kombinationer av tecken parsas automatiskt
            re = new Regex(@"^\d{4}\-([1-9]|(0[1-9]|1[0-2]))\-([0-9]|[0-2][0-9]|3[0-1])$");
            if (re.IsMatch(from)) from = DateTime.Parse(from).ToString("yyyy-MM-dd");
            if (re.IsMatch(to)) to = String.Join("-",
                DateTime.Parse(to).ToString("yyyy"),
                DateTime.Parse(to).ToString("MM"),
                DateTime.DaysInMonth(DateTime.Parse(to).Year, DateTime.Parse(to).Month)
                );

            // Om 1|2 tecken antas vara månad, from större än to antas vara seklet innan
            DateTime f, t;
            re = new Regex(@"^([1-9]|(0[1-9]|10|11|12))$");
            if (re.IsMatch(from) || re.IsMatch(to))
            {
                if(re.IsMatch(from) && DateTime.TryParseExact(to, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out t))
                {
                    if (int.Parse(from) > t.Month)
                    {
                        from = DateTime.Parse((t.Year - 1).ToString() + "-" + from).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        from = DateTime.Parse((t.Year).ToString() + "-" + from).ToString("yyyy-MM-dd");
                    }
                    // Görs inget med to, anses vara färdig om innehar formatet yyyy-MM-dd
                }
                else if(DateTime.TryParseExact(from, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out f) && re.IsMatch(to))
                {
                    if (f.Year == DateTime.Now.Year && f.Month > int.Parse(to))
                    {
                        from = new DateTime(f.Year - 1, f.Month, f.Day).ToString("yyyy-MM-dd");
                    }

                    to = new DateTime(DateTime.Now.Year, int.Parse(to), DateTime.DaysInMonth(DateTime.Now.Year, int.Parse(to))).ToString("yyyy-MM-dd");

                    if (DateTime.Parse(from) > DateTime.Parse(to))
                    {
                        throw new FormatException();
                    }
                }
                else if (re.IsMatch(from) && re.IsMatch(to))
                {
                    if (int.Parse(from) > int.Parse(to))
                    {
                        from = DateTime.Parse((DateTime.Now.Year - 1).ToString() + "-" + from).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        from = DateTime.Parse((DateTime.Now.Year).ToString() + "-" + from).ToString("yyyy-MM-dd");
                    }

                    to = DateTime.Parse((DateTime.Now.Year).ToString() + "-" + to).ToString("yyyy-MM-dd");
                }
            }


            return FilterStatBetweenDates(sql, column, from, to, period.month);
        }


        private static string FilterStatBetweenDates(string sql, string column, string fromDate, string toDate, period period = period.year)
        {
            try
            {
                string formater = string.Empty;
                string from = string.Empty;
                string to = string.Empty;
                switch (period)
                {
                    case period.year:
                        formater = "%Y";
                        from = DateTime.Parse(fromDate).Year.ToString("yyyy");
                        to = DateTime.Parse(toDate).Year.ToString("yyyy");
                        break;
                    case period.month:
                        formater = "%Y-%m";
                        from = DateTime.Parse(fromDate).Year.ToString("yyyy") + "-" + DateTime.Parse(fromDate).Month.ToString("MM");
                        to = DateTime.Parse(toDate).Year.ToString("yyyy") + "-" + DateTime.Parse(toDate).Month.ToString("MM");
                        break;
                    case period.day:
                        formater = "%Y-%m-%d";
                        from = DateTime.Parse(fromDate).ToString("yyyy-MM-dd");
                        to = DateTime.Parse(toDate).ToString("yyyy-MM-dd");
                        break;
                    default:
                        break;
                }
                sql.Insert(sql.LastIndexOf("group by", StringComparison.OrdinalIgnoreCase) - 1, $"WHERE strftime('{formater}', {column}) BETWEEN '{to}' AND '{from}' ");

                return sql;
            }
            catch
            {
                throw;
            }
        }

        enum period
        {
            year,
            month,
            day
        }

    }
}