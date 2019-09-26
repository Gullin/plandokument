using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Plan.Plandokument
{
    public partial class kontrollpanel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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


                // Cache-dagar
                if (int.TryParse(ConfigurationManager.AppSettings["CacheNbrOfDays"], out int _result))
                {
                    if (_result < 1 || _result > 1)
                    {
                        NyCacheEfterAntalDagar.Text = $"Efter antal dagar: {_result} st. dagar";
                    }
                    else
                    {
                        NyCacheEfterAntalDagar.Text = $"Efter dag: <span class=\"amplify\">{_result} st. dag</span>";
                    }
                }
                else
                {
                    NyCacheEfterAntalDagar.CssClass = "error";
                    NyCacheEfterAntalDagar.Text = "fel, antalet dagar innan cache kunde inte parsas";
                }
                // Cache-klockslag
                if (DateTime.TryParseExact(ConfigurationManager.AppSettings["CacheTime"], "HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime cachTime))
                {
                    NyCacheKlockan.Text = "Tid på dygnet: <span class=\"amplify\">" + cachTime.ToString("HH:mm:ss") + "</span>";
                }
                else
                {
                    NyCacheKlockan.CssClass = "error";
                    NyCacheKlockan.Text = "Klockslag för cache är inte inställd. System sätter applikationens starttid som klockslag.";
                }
            }
        }
    }
}