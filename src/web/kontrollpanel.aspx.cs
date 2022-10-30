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
using Plan.Plandokument.SQLite;

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


                // Statistik från Request-data
                DataTable dataTable = new DataTable();
                dataTable = StatData.StatTotalRequests();
                if(dataTable.Rows.Count > 0)
                {
                    NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
                    nfi.NumberGroupSeparator = " ";
                    nfi.NumberDecimalSeparator = ",";
                    StatTotalRequests.Text = $"Förfrågningar (requests): {String.Format("{0:n0}", dataTable.Rows[0][0])} st.";
                }
                dataTable = StatData.StatSearchRequests();
                if (dataTable.Rows.Count > 0)
                {
                    StatSearchRequestsTotal.Text = $"Sökningar: {String.Format("{0:n0}", dataTable.Rows[0][0])} st.";
                    StatSearchRequestsHits.Text = $"Träffar: {String.Format("{0:n0}", dataTable.Rows[0][1])} st.";
                }
                dataTable = StatData.StatPeriodRequests();
                if (dataTable.Rows.Count > 0)
                {
                    StatPeriodRequestsFirst.Text = $"Första registrerade förfrågan: {dataTable.Rows[0][0].ToString()}";
                    StatPeriodRequestsLast.Text = $"Senaste registrerade förfrågan: {dataTable.Rows[0][1].ToString()}";
                    DateTime firstRequest;
                    DateTime lastRequest;
                    if (DateTime.TryParseExact(dataTable.Rows[0][0].ToString(),"yyyy-MM-dd HH:mm:ss",new CultureInfo("sv-SE"),DateTimeStyles.None, out firstRequest)
                        &&
                        DateTime.TryParseExact(dataTable.Rows[0][1].ToString(), "yyyy-MM-dd HH:mm:ss", new CultureInfo("sv-SE"), DateTimeStyles.None, out lastRequest))
                    {
                        TimeSpan periodOfRequests = lastRequest - firstRequest;
                        int years = (int)Math.Floor((double)(periodOfRequests.Days / 365));
                        periodOfRequests = periodOfRequests.Subtract(new TimeSpan(years * 365, 0, 0, 0));
                        StatPeriodRequestsEnduring.Text = $"Sökningar har gjorts inom en period av<br />{years.ToString()} år, {periodOfRequests.Days.ToString()} dagar, {periodOfRequests.Hours.ToString()} timmar, {periodOfRequests.Minutes.ToString()} minuter och {periodOfRequests.Seconds.ToString()} sekunder";
                    }
                }
                dataTable = StatData.StatSearchtimeRequests();
                if (dataTable.Rows.Count > 0)
                {
                    StatSearchtimeMinRequests.Text = $"Kortast: {String.Format("{0:n0}", dataTable.Rows[0][0])} ms.";
                    StatSearchtimeMaxRequests.Text = $"Längst: {String.Format("{0:n0}", dataTable.Rows[0][1])} ms.";
                    StatSearchtimeAverageRequests.Text = $"Genomsnitt: {String.Format("{0:n0}", dataTable.Rows[0][2])} ms.";
                }


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