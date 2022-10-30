using System.IO;

namespace Plan.Plandokument
{
	public class SqlTemplates : Utility
	{
		public static string GetPlanRegisterBas { get; set; }
		public static string GetPlanGeometriBas { get; set; }
		public static string GetPlanBerorFastighet { get; set; }
        public static string GetPlanBerorPlan { get; set; }
        public static string ExistsAppDbStatRequest { get; set; }
        public static string CreateAppDbStatRequest { get; set; }
        public static string GetTotalDbStatRequests { get; set; }
        public static string GetTotalByDayDbStatRequests { get; set; }
        public static string GetTotalByMonthDbStatRequests { get; set; }
        public static string GetTotalByYearDbStatRequests { get; set; }
        public static string GetPeriodDbStatRequests { get; set; }
        public static string GetRunningTotalDbStatRequests { get; set; }
        public static string GetRunningTotalByDayDbStatRequests { get; set; }
        public static string GetRunningTotalByMonthDbStatRequests { get; set; }
        public static string GetRunningTotalByYearDbStatRequests { get; set; }
        public static string GetRunningHitsDbStatRequests { get; set; }
        public static string GetRunningSearchDbStatRequests { get; set; }
        public static string GetSearchDbStatRequests { get; set; }
        public static string GetSearchtimeDbStatRequests { get; set; }
        public static string GetSearchtimeByDayDbStatRequests { get; set; }
        public static string GetSearchtimeByMonthDbStatRequests { get; set; }
        public static string GetSearchtimeByYearDbStatRequests { get; set; }
        public static string InsertAppDbStatRequest { get; set; }
        public static string GetUserIdFullName { get; set; }

        static SqlTemplates()
		{
			string baseScriptFolder = Utility.appPath + @"/static-resources/sql/";

            GetPlanRegisterBas = new FileInfo(baseScriptFolder + @"/mssqlserver/get-plan-register-bas.sql")
                .OpenText().ReadToEnd();

            GetPlanGeometriBas = new FileInfo(baseScriptFolder + @"/postgresql/get-plan-geometri-bas.pgsql")
                .OpenText().ReadToEnd();

			GetPlanBerorFastighet = new FileInfo(baseScriptFolder + @"/mssqlserver/get-plan-beror-fastighet.sql")
                .OpenText().ReadToEnd();

            GetPlanBerorPlan = new FileInfo(baseScriptFolder + @"/mssqlserver/get-plan-beror-plan.sql")
                .OpenText().ReadToEnd();

            ExistsAppDbStatRequest = new FileInfo(baseScriptFolder + @"/sqlite/check-exists-stat-requests.sql")
                .OpenText().ReadToEnd();

            CreateAppDbStatRequest = new FileInfo(baseScriptFolder + @"/sqlite/create-stat-requests.sql")
                .OpenText().ReadToEnd();

            GetTotalDbStatRequests = new FileInfo(baseScriptFolder + @"/sqlite/get-stat-requests-count.sql")
                .OpenText().ReadToEnd();

            GetTotalByDayDbStatRequests = SetByDay(
                new FileInfo(baseScriptFolder + @"/sqlite/get-stat-requests-count-by-period.sql")
                .OpenText().ReadToEnd()
                );

            GetTotalByMonthDbStatRequests = SetByMonth(
                new FileInfo(baseScriptFolder + @"/sqlite/get-stat-requests-count-by-period.sql")
                .OpenText().ReadToEnd()
                );

            GetTotalByYearDbStatRequests = SetByYear(
                new FileInfo(baseScriptFolder + @"/sqlite/get-stat-requests-count-by-period.sql")
                .OpenText().ReadToEnd()
                );

            GetPeriodDbStatRequests = new FileInfo(baseScriptFolder + @"/sqlite/get-stat-requests-period.sql")
                .OpenText().ReadToEnd();

            GetRunningTotalDbStatRequests = new FileInfo(baseScriptFolder + @"/sqlite/get-stat-running-total-requests.sql")
                .OpenText().ReadToEnd();

            GetRunningTotalByDayDbStatRequests = SetByDay(
                new FileInfo(baseScriptFolder + @"/sqlite/get-stat-running-total-requests-by-period.sql")
                .OpenText().ReadToEnd()
                );

            GetRunningTotalByMonthDbStatRequests = SetByMonth(
                new FileInfo(baseScriptFolder + @"/sqlite/get-stat-running-total-requests-by-period.sql")
                .OpenText().ReadToEnd()
                );

            GetRunningTotalByYearDbStatRequests = SetByYear(
                new FileInfo(baseScriptFolder + @"/sqlite/get-stat-running-total-requests-by-period.sql")
                .OpenText().ReadToEnd()
                );

            GetRunningHitsDbStatRequests = new FileInfo(baseScriptFolder + @"/sqlite/get-stat-running-total-hits-count.sql")
                .OpenText().ReadToEnd();

            GetRunningSearchDbStatRequests = new FileInfo(baseScriptFolder + @"/sqlite/get-stat-running-total-search-count.sql")
                .OpenText().ReadToEnd();

            GetSearchDbStatRequests = new FileInfo(baseScriptFolder + @"/sqlite/get-stat-search-count.sql")
                .OpenText().ReadToEnd();

            GetSearchtimeDbStatRequests = new FileInfo(baseScriptFolder + @"/sqlite/get-stat-searchtime.sql")
                .OpenText().ReadToEnd();

            GetSearchtimeByDayDbStatRequests = SetByDay(
                new FileInfo(baseScriptFolder + @"/sqlite/get-stat-searchtime-by-period.sql")
                .OpenText().ReadToEnd()
                );

            GetSearchtimeByMonthDbStatRequests = SetByMonth(
                new FileInfo(baseScriptFolder + @"/sqlite/get-stat-searchtime-by-period.sql")
                .OpenText().ReadToEnd()
                );

            GetSearchtimeByYearDbStatRequests = SetByYear(
                new FileInfo(baseScriptFolder + @"/sqlite/get-stat-searchtime-by-period.sql")
                .OpenText().ReadToEnd()
                );

            InsertAppDbStatRequest = new FileInfo(baseScriptFolder + @"/sqlite/insert-stat-requests.sql")
                .OpenText().ReadToEnd();

            GetUserIdFullName = new FileInfo(baseScriptFolder + @"/postgresql/get-username-by-user-id.pgsql")
                .OpenText().ReadToEnd();
        }

        private static string SetByDay(string sql)
        {
            return sql.Replace("@period", "'%Y-%m-%d'");
        }

        private static string SetByMonth(string sql)
        {
            return sql.Replace("@period", "'%Y-%m'");
        }
        private static string SetByYear(string sql)
        {
            return sql.Replace("@period", "'%Y'");
        }

    }
}