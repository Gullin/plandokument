using System.IO;

namespace Plan.Plandokument
{
	public class SqlTemplates : Utility
	{
		public static string GetPlanRegisterBas { get; set; }
		public static string GetPlanGeometriBas { get; set; }
		public static string GetPlanBerorFastighet { get; set; }
        public static string GetPlanBerorPlan { get; set; }
        public static string CreateAppDbStatRequest { get; set; }
        public static string ExistsAppDbStatRequest { get; set; }
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

            CreateAppDbStatRequest = new FileInfo(baseScriptFolder + @"/sqlite/create-stat-requests.sql")
                .OpenText().ReadToEnd();

            ExistsAppDbStatRequest = new FileInfo(baseScriptFolder + @"/sqlite/check-exists-stat-requests.sql")
                .OpenText().ReadToEnd();

            InsertAppDbStatRequest = new FileInfo(baseScriptFolder + @"/sqlite/insert-stat-requests.sql")
                .OpenText().ReadToEnd();

            GetUserIdFullName = new FileInfo(baseScriptFolder + @"/postgresql/get-username-by-user-id.pgsql")
                .OpenText().ReadToEnd();
        }

    }
}