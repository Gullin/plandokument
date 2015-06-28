using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OSGeo.MapGuide;
using Plan.Plandokument;

namespace Plan.Plandokument
{
    public partial class Default : System.Web.UI.Page
    {
        private static string strUrlSpliter = ConfigurationManager.AppSettings["URLQueryStringSeparator"];
        private char[] chrUrlSpliter = new char[] { Convert.ToChar(strUrlSpliter) };
        private string searchConditions = string.Empty;
        private string searchString = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Krävs när aspx-fil använder sig av <%# %>, databind, och inte <%= %>, samma som Response.Write, .
            Page.Header.DataBind();

            if (!IsPostBack)
            {
                // AssemblyVersionOverride Version VersionPrefix VersionSuffix
                // Versionering på sida
                string version = ApplicationAssemblyUtility.GetApplicationVersionNumber();
                string versionPrefix = ConfigurationManager.AppSettings["VersionPrefix"];
                string versionSuffix = ConfigurationManager.AppSettings["VersionSuffix"];
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["AssemblyVersionOverride"]))
                {
                    version = ConfigurationManager.AppSettings["Version"];
                }
                if (string.IsNullOrWhiteSpace(versionPrefix))
                {
                    version = "v" + version;
                }
                else
                {
                    version = versionPrefix + version;
                }
                version += versionSuffix;
                lblVersion.Text = version;


                // Copyright på sida
                DateTime dateTime = DateTime.Now;
                lblCopyrightYear.Text = dateTime.Year.ToString() + " " + ApplicationAssemblyUtility.GetApplicationCopyright().ToString();


                // Sökning genom URL Routing
                // Sökta planhandlingar (dokument tillhörande planen)
                searchConditions = Convert.ToString(RouteData.Values["villkor"]);
                // Sökta planer (teckenseparerad textsträng)
                searchString = Convert.ToString(RouteData.Values["plan"]);

                // Sökning genom URL-parametrisering, om existerar prioriteras det före URL Routing (ser det som att system programatisk postar)
                // Sökning sker endast genom id (anpassas därför endast för planDoc-ID, direktsökning, fastighet-ID, genom berörkretsar
                string paramUrlHandling = string.Empty;
                string paramUrlBegrepp = string.Empty;
                if (Request[ConfigurationManager.AppSettings["UrlParameterSearchString"]] != null)
                {
                    searchString = Request[ConfigurationManager.AppSettings["UrlParameterSearchString"]];

                    // Default-värde: "dokument" = motsvarar en visning av alla dokumenttyper
                    if (Request[ConfigurationManager.AppSettings["UrlParameterDocumentType"]] != null)
                        paramUrlHandling = Request[ConfigurationManager.AppSettings["UrlParameterDocumentType"]];
                    else
                        paramUrlHandling = "dokument";

                    // Default-värde: "" = motsvarar en sökning på planDoc-ID
                    if (Request[ConfigurationManager.AppSettings["UrlParameterSearchType"]] != null)
                        paramUrlBegrepp = Request[ConfigurationManager.AppSettings["UrlParameterSearchType"]];
                    else
                        paramUrlBegrepp = "";

                    if (string.IsNullOrEmpty(paramUrlBegrepp))
                        searchConditions = paramUrlHandling;
                    else
                        searchConditions = paramUrlHandling + strUrlSpliter + paramUrlBegrepp;
                }

                if (!string.IsNullOrEmpty(searchConditions.Trim()) && !string.IsNullOrEmpty(searchString.Trim()) && searchString != "N/A")
                {
                    createSearchSession(searchConditions, searchString);
                }
                else
                {
                    //fungerar ej p.g.a. round trip utan sökbegrepp som rensa sessionen
                    //Session.Clear();
                }

            }

        }

        private void createSearchSession(string conditions, string searchString)
        {
            Session["TimeStart"] = DateTime.Now;
            string[] cleanConditions = searchedConditionsAsCleanStringArray(conditions);
            Session["PlanHandling"] = cleanConditions[0].ToString();
            Session["Begrepp"] = cleanConditions[1].ToString();
            string[] cleanSearchedPlans = searchStringAsCleanStringArray(searchString);
            Session["SearchedPlans"] = cleanSearchedPlans;
            Session["PlanerAntal"] = countSearchedPlans(cleanSearchedPlans);
            // Skapas endast ny om ny sökning görs efter att kartsessionen raderats, styrs av sessionState TimeOute i web.config
            if (Session["MapSiteSessionID"] == null)
            {
                Session["MapSiteSessionID"] = createMapSiteSession();
            }
        }

        private string createMapSiteSession()
        {
            // Initierar kartsite och kartsession 
            string mapWebTierInit = ConfigurationManager.AppSettings["MGWebTierInit"].ToString();
            string mapUserName = ConfigurationManager.AppSettings["MGUserName"].ToString();
            string mapUserPass = ConfigurationManager.AppSettings["MGUserPass"].ToString();

            MapGuideApi.MgInitializeWebTier(mapWebTierInit);
            MgUserInformation userInfo = new MgUserInformation(mapUserName, mapUserPass);
            MgSite mapSite = new MgSite();
            mapSite.Open(userInfo);
            

            string mapSiteSessionID = mapSite.CreateSession();

            //mapSite.Close();

            return mapSiteSessionID;
        }

        private Boolean isPlansFromUrlSingel(string[] plans)
        {
            if (plans.Length == 1)
                return true;
            else
                return false;
        }
        
        private int countSearchedPlans(string[] plans)
        {
            // Inga eftersökta ... eller default värde i så fall returneras noll till antal
            if (plans != null && plans.Length >= 1 && plans[0] != "N/A")
            {
                    return plans.Length;
            }
            else
                return 0;
        }

        private string[] searchStringAsCleanStringArray(string searchedPlans)
        {
            List<string> plans = toArrayOfSearchedPlans(searchedPlans).Select(p => p.Trim().ToLower()).ToList();
            plans.RemoveAll(p => string.IsNullOrWhiteSpace(p));

            return plans.ToArray();
        }

        private string[] searchStringAsCleanStringArray(string[] plans)
        {
            List<string> cleanPlans = plans.Select(p => p.Trim().ToLower()).ToList();
            cleanPlans.RemoveAll(p => string.IsNullOrWhiteSpace(p));

            return cleanPlans.ToArray();
        }

        private string[] toArrayOfSearchedPlans(string searchedPlans)
        {
            string[] plans = searchedPlans.Split(chrUrlSpliter).Select(p => p.Trim()).ToArray();
            return plans;
        }

        private string[] searchedConditionsAsCleanStringArray(string conditions)
        {
            string[] cleanConditions = new string[2];
            //Kontrollera om strängen innehåller två delar [typ av handling],[sökbegrepp]

            // Om sträng innehåller tecken för delning annars antas villkor default vara vilken typ av hadling som efterfrågas
            if (conditions.Contains(strUrlSpliter))
            {
                string[] tmpConditions = conditions.Split(chrUrlSpliter, StringSplitOptions.RemoveEmptyEntries);

                // Villkorssträng, om avgränsande tecken finns, förutsätts vara uppbyggd av två delar, [sökt handling] och [sökbegrepp/kolumn som planDoc-ID, aktbeteckning etc.]
                if (tmpConditions.Length == 2)
                {
                    cleanConditions[0] = tmpConditions[0].Trim().ToLower();
                    cleanConditions[1] = tmpConditions[1].Trim().ToLower();
                }
                else
                    throw new Exception("Villkor i URL är felkonstruerat. Förväntas en eller två delar separerade av '" + strUrlSpliter + "' ([sökt handling]" + strUrlSpliter + "[sökbart begrepp]). Delen sökbart begrepp är dock inget krav.");
            }
            else
            {
                cleanConditions[0] = conditions.Trim().ToLower();
                cleanConditions[1] = string.Empty;
            }

            return cleanConditions;
        }


    }
}