using System.IO;

namespace Plan.Plandokument
{
	public class SqlTemplates : Utility
	{
		public static string GetPlanRegisterBas { get; set; }
		public static string GetPlanGeometriBas { get; set; }
		public static string GetPlanBerorFastighet { get; set; }
		public static string GetPlanBerorPlan { get; set; }

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
        }

    }
}