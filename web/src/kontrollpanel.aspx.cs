using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            }
        }
    }
}