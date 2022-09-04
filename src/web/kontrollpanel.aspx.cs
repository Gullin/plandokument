using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using Npgsql;
using System.Data;

namespace Plan.Plandokument
{
    public partial class kontrollpanel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string _user = Plandokument.User.GetLogin(System.Web.HttpContext.Current.User.Identity.Name);

            //TODO: AUTHENICATED ADMIN, kontrollera mot PostGIS-grupp (behöver användare vara med i Settings.config, PostgreSQl-användare och med i GIS-ADMIN-gruppen
            if (!(HttpContext.Current.User.Identity.IsAuthenticated && Plandokument.User.Admins.Contains(_user)))
            {
                Response.Redirect("~/not-authenticated.aspx", true);
            }
            else
            {
                DataTable dtUser = new DataTable();
                NpgsqlConnection npgsqlCon = UtilityDatabase.GetNpgsqlConnectionForDBGeodata();
                NpgsqlCommand npgsqlCom = new NpgsqlCommand(
                    SqlTemplates.GetUserIdFullName.Replace("@user_id", _user),
                    npgsqlCon);
                NpgsqlDataReader npgsqlDr;

                npgsqlCom.Connection.Open();
                npgsqlDr = npgsqlCom.ExecuteReader();

                dtUser.Load(npgsqlDr);

                npgsqlDr.CloseAsync();
                npgsqlDr.DisposeAsync();

                string userFullName = String.Empty;
                if (dtUser.Rows.Count > 0)
                {
                    userFullName = dtUser.Rows[0]["first_name"].ToString() + " " + dtUser.Rows[0]["last_name"].ToString();
                }

                lblUser.Text = $"Inloggad som [ {_user} ] {userFullName}";
                lblUser.ToolTip = "Autentiserad som administratör genom SSO (Singel Sign On)";
            }


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
                        NyCacheEfterAntalDagar.Text = $"Efter: {_result} st. dagar";
                    }
                    else
                    {
                        NyCacheEfterAntalDagar.Text = $"Efter: <span class=\"amplify\">{_result} st. dag</span>";
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

    public static class User
    {
        public static List<string> Admins
        {
            get
            {
                return AdminlistInConfig.Split(',').ToList();
            }
        }

        public static List<string> GetDomains()
        {
            List<string> _list = new List<string>();
            foreach (string item in Admins)
            {
                int stop = item.IndexOf("\\");
                _list.Add((stop > -1) ? item.Substring(0, stop) : string.Empty);
            }
            return _list;
        }

        public static string GetLogin(string user)
        {
            int stop = user.IndexOf("\\");
            return (stop > -1) ? user.Substring(stop + 1, user.Length - stop - 1) : string.Empty;
        }

        private static string AdminlistInConfig
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["admins"];
            }
        }
    }

}