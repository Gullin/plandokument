using System;
using System.Configuration;

namespace Plan.Plandokument
{
    public partial class planlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Krävs när aspx-fil använder sig av <%# %>, databind, och inte <%= %>, samma som Response.Write, .
            Page.Header.DataBind();

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