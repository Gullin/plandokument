using System;
using System.Configuration;
using System.Diagnostics;
using System.Web.Routing;
using OSGeo.MapGuide;

namespace Plan.Plandokument
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            UtilityLog.Log("Start, webbapplikation - " + 
                ApplicationAssemblyUtility.GetApplicationVersionNumber() + ", " +
                ApplicationAssemblyUtility.GetApplicationCopyright() + " (debug='" +
                ApplicationAssemblyUtility.AssemblyIsDebugBuild(ApplicationAssemblyUtility.ApplicationAssembly).ToString() + "')",
                Utility.LogLevel.INFORM);

            // Starta ping-ning av webb applikation
            if (Boolean.TryParse(ConfigurationManager.AppSettings["shouldPing"], out bool result))
            {
                if (result)
                {
                    CheckingRestartApp checkingRestartApp = new CheckingRestartApp();
                    checkingRestartApp.Start(int.Parse(ConfigurationManager.AppSettings["pingIntervall"]));
                    // Lyssna efter när ping görs
                    checkingRestartApp.OnPinged += new EventHandler(Log_OnPinged);
                    UtilityLog.Log("Start av intern ping av webbapplikation", Utility.LogLevel.INFORM);
                }
            }


            Plan.Plandokument.PlanCache.GetPlandocumenttypesCache();
            Plan.Plandokument.PlanCache.GetPlanBasisCache();
            Plan.Plandokument.PlanCache.GetPlanBerorFastighetCache();
            Plan.Plandokument.PlanCache.GetPlanBerorPlanCache();
            
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string url = Request.Url.ToString();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            UtilityLog.Log("Ohanterat fel i webbapplikation, detaljer i Error.log", Utility.LogLevel.ERROR);

            // Get the exception object.
            Exception exc = Server.GetLastError();

            UtilityException.LogException(exc, "Okänd sida", true);

            //Redirect HTTP errors to HttpError page
            //Server.Transfer("HttpErrorPage.aspx");

            // Clear the error from the server
            Server.ClearError();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // Om kartsessions finns lagrad i applikationens session
            string sessionVariableName = "MapSiteSessionID";
            if (Session[sessionVariableName] != null)
            {
                // Rensar bort kartsession, både kartsessions-ID från sessionsvariabeln och från kartserver
                string mapWebTierInit = ConfigurationManager.AppSettings["MGWebTierInit"].ToString();
                string mapSessionID = Session[sessionVariableName].ToString();
                Session.Remove(sessionVariableName);
                MapGuideApi.MgInitializeWebTier(mapWebTierInit);
                MgUserInformation userInfo = new MgUserInformation(mapSessionID);
                MgSiteConnection siteConnection = new MgSiteConnection();
                siteConnection.Open(userInfo);
                MgSite mapSite = siteConnection.GetSite();
                mapSite.DestroySession(mapSite.GetCurrentSession());
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
            UtilityLog.Log("Avslutar webbapplikation", Utility.LogLevel.WARN);

            // Tvinga app:n att starta om direkt
            if (Boolean.TryParse(ConfigurationManager.AppSettings["shouldPing"], out bool result))
            {
                if (result)
                {
                    new CheckingRestartApp().PingServer();
                }
            }
        }


        private void Log_OnPinged(object sender, EventArgs e)
        {
            UtilityLog.Log("Ping av webbapplikation på adressen " + ConfigurationManager.AppSettings["pingUrl"].ToString(), Utility.LogLevel.INFORM);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            // Ordningen av registrering av routs är viktig

            // Ignoerade sökvägar vid routing, fungerar ej. Behöver lösas i vardera aspx-fils länkar
            //routes.Add(new Route("pic/*", new StopRoutingHandler()));
            //routes.Ignore("pic/{*pathInfo}");

            routes.MapPageRoute("PlandokumentAlla",
                                "dokument/alla",
                                "~/planlist.aspx",
                                false);

            // Alias
            routes.MapPageRoute("AdminKontrollpanel",
                                "dokument/kontrollpanel",
                                "~/kontrollpanel.aspx",
                                false);
            routes.MapPageRoute("Admin",
                                "dokument/admin",
                                "~/kontrollpanel.aspx",
                                false);
            routes.MapPageRoute("Administration",
                                "dokument/administration",
                                "~/kontrollpanel.aspx",
                                false);
            routes.MapPageRoute("AdminDashboard",
                                "dokument/dashboard",
                                "~/kontrollpanel.aspx",
                                false);
            routes.MapPageRoute("AdminSystem",
                                "dokument/system",
                                "~/kontrollpanel.aspx",
                                false);


            // Alias
            routes.MapPageRoute("PlandokumentOm",
                                "dokument/om",
                                "~/om.aspx",
                                false);
            routes.MapPageRoute("PlandokumentHjalp",
                                "dokument/hjalp",
                                "~/om.aspx",
                                false);
            routes.MapPageRoute("PlandokumentHelp",
                                "dokument/help",
                                "~/om.aspx",
                                false);

            // Alias
            routes.MapPageRoute("PlandokumentVersion",
                                "dokument/version",
                                "~/versioninfo.aspx",
                                false);
            routes.MapPageRoute("PlandokumentInformation",
                                "dokument/information",
                                "~/versioninfo.aspx",
                                false);
            routes.MapPageRoute("PlandokumentInfo",
                                "dokument/info",
                                "~/versioninfo.aspx",
                                false);

            routes.MapPageRoute("Plandokument",
                                "{villkor}/{*plan}",
                                "~/plandokument.aspx",
                                false,
                                new RouteValueDictionary { { "villkor", "dokument" }, { "plan", "N/A"} });
        }


     }
}