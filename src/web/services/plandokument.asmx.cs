using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
//using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using Plan.Plandokument.MapLayerDefinition;
using Plan.Plandokument.jTable;
using Npgsql;

namespace Plan.Plandokument
{
    /// <summary>
    /// 
    /// </summary>
    [WebService(Namespace = "Landskrona.Apps.Plan.Dokument.Ws")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WsPlandokument : System.Web.Services.WebService
    {

        [WebMethod(EnableSession=true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string nbrOfSearchedPlans()
        {
            int nbrSearchedPlans;

            if (Session["PlanerAntal"] == null)
                nbrSearchedPlans = 0;
            else
                nbrSearchedPlans = Convert.ToInt32(Session["PlanerAntal"]);

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(nbrSearchedPlans);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string documentConditionOfSearchedPlans()
        {
            string document;

            if (Session["PlanHandling"] == null)
                document = string.Empty;
            else
                document = (string)(Session["PlanHandling"]);

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(document);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string columnConditionOfSearchedPlans()
        {
            string column;

            if (Session["Begrepp"] == null)
                column = string.Empty;
            else
                column = (string)(Session["Begrepp"]);

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(column);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string searchedPlans()
        {
            string plans;

            if (Session["SearchedPlans"] == null)
                plans = string.Empty;
            else
                plans = string.Join(", ", (string[])(Session["SearchedPlans"]));

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(plans);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getStatTotNbrPlans()
        {
            // Hämtar alla planer från cache
            Cache cache = HttpRuntime.Cache;
            DataTable dtPlans = PlanCache.GetPlanBasisCache();

            int nbrPlans = (from p in dtPlans.AsEnumerable()
                            select p).Count();

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(nbrPlans);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getStatNbrPlanTypes()
        {
            // Hämtar alla planer från cache
            Cache cache = HttpRuntime.Cache;
            DataTable dtPlans = PlanCache.GetPlanBasisCache();

            DataTable dt = new DataTable();
            dt.Columns.Add("PLANFK", typeof(string));
            dt.Columns.Add("ANTAL", typeof(Int32));

            IEnumerable<DataRow> queryResult = Enumerable.Empty<DataRow>();

            // Filtrering av cachade planer genom linq
            queryResult = from t in dtPlans.AsEnumerable()
                          group t by t["PLANFK"] into g
                          select dt.LoadDataRow( new object[]
                          { 
                              g.Key, 
                              g.Count()
                          },
                          false);

            //DataTable dtTemp = new DataTable();
            DataTable dtResult = dt.Clone();
            DataRow drResult;

            drResult = dtResult.NewRow();

            // Om match hittas bland planer
            if (queryResult != null && queryResult.Count<DataRow>() > 0)
            {
                dtResult = queryResult.CopyToDataTable();

                drResult["PLANFK"] = dt.Rows[0][0]; ;
                drResult["ANTAL"] = dt.Rows[0][1]; ;
            }

            return getObjectAsJson(getTableSorted(dtResult, "PLANFK", "ASC"));
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getStatNbrPlanImplement()
        {
            return new JavaScriptSerializer()
                .Serialize(
                    PlanCache.GetPlanBasisCache()
                        .AsEnumerable()
                        .Where(rows => rows.Field<int>("isgenomf") == 1).Count().ToString()
                );
        }


        //TODO: Kontrollera för om sökning görs mot samma begrepp med värde som finns vid flera tillfällen
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getPlanInfo()
        {
            // Kontrollera om sökning är gjord efter speciellt begrepp (kolumn)
            string begrepp = string.Empty;
            if (Session["Begrepp"] != null)
            {
                begrepp = Session["Begrepp"].ToString();
            }

            // Sökta planer
            string[] searched = (string[])Session["SearchedPlans"];

            // Hämtar alla planer från cache
            Cache cache = HttpRuntime.Cache;
            DataTable dtPlans = PlanCache.GetPlanBasisCache();

            // Hämtar berörda fastigheter från cache om sökning görs på fastighet (begrepp)
            // Hämtas endast från cache när inget begrepp eller sökt specifikt på fastighetsnyckel eller fastighetsbeteckning
            string parcelBlockUnitSearchSign = "";
            DataTable dtPlanBerorFastighet = null;
            if (string.IsNullOrWhiteSpace(begrepp) || begrepp.ToUpper() == "FASTIGHET" || begrepp.ToUpper() == "FASTIGHETNYCKEL")
            {
                parcelBlockUnitSearchSign = ConfigurationManager.AppSettings["URLParcelBlockUnitSign"].ToString();
                dtPlanBerorFastighet = PlanCache.GetPlanBerorFastighetCache();
            }

            // Datatabell för resultat av sökning
            DataTable dtResult = new DataTable();
            DataColumn dc = new DataColumn("BEGREPP");
            dtResult.Columns.Add(dc);
            dc = new DataColumn("NYCKEL");
            dtResult.Columns.Add(dc);
            dc = new DataColumn("AKT");
            dtResult.Columns.Add(dc);
            dc = new DataColumn("AKTTIDIGARE");
            dtResult.Columns.Add(dc);
            dc = new DataColumn("AKTEGEN");
            dtResult.Columns.Add(dc);
            dc = new DataColumn("PLANFK");
            dtResult.Columns.Add(dc);
            dc = new DataColumn("PLANNAMN");
            dtResult.Columns.Add(dc);
            dc = new DataColumn("ISGENOMF");
            dtResult.Columns.Add(dc);
            dc = new DataColumn("SEARCHEDSTRING");
            dtResult.Columns.Add(dc);


            // Om sökparametrar (sökta planer)
            if (searched != null)
            {
                // Temporära datatabell-objekt till för byggning resulterande datatabell som returneras
                DataTable dt = new DataTable();
                DataRow drResult;

                // För varje sökt planDoc
                foreach (string potentialPlan in searched)
                {
                    IEnumerable<DataRow> queryResult = Enumerable.Empty<DataRow>();

                    // Om kolumn är definierad, söker på definerad kolumn annars mot alla
                    if (!string.IsNullOrWhiteSpace(begrepp))
                    {

                        // Hantera sökning efter planDoc på planinformation eller fastighet
                        DataTable dtFiltered = null;
                        DataView dv = new DataView(dtPlanBerorFastighet);
                        if (begrepp.ToUpper() != "FASTIGHET" && begrepp.ToUpper() != "FASTIGHETNYCKEL")
                        {
                            // Filtrering av cachade planer genom linq
                            if (!potentialPlan.ToLower().Contains("1282k-") && begrepp.ToUpper() == "AKTTIDIGARE")
                            {
                                string akttidigarePlan = "1282k-" + potentialPlan;
                                queryResult = from t in dtPlans.AsEnumerable()
                                              where (string.IsNullOrEmpty(t.Field<string>(begrepp.ToUpper())) ? "x" : t.Field<string>(begrepp.ToUpper()).ToLower()) == akttidigarePlan.ToString().ToLower()
                                              select t;
                            }
                            else
                            {
                                queryResult = from t in dtPlans.AsEnumerable()
                                              where (string.IsNullOrEmpty(t.Field<string>(begrepp.ToUpper())) ? "x" : t.Field<string>(begrepp.ToUpper()).ToLower()) == potentialPlan.ToString().ToLower()
                                              select t;
                            }
                        }
                        else
                        {
                            if (begrepp.ToUpper() == "FASTIGHET")
                            {
                                dv.RowFilter = "FASTIGHET = '" +  potentialPlan.ToUpper().Replace(parcelBlockUnitSearchSign, ":") + "'";
                                dtFiltered = dv.ToTable();
                                dv.Dispose();
                            }
                            if (begrepp.ToUpper() == "FASTIGHETNYCKEL")
                            {
                                    // Om datatyp decimal, gör filtreringen
                                    decimal value;
                                    if (Decimal.TryParse(potentialPlan, out value))
                                    {
                                        dv.RowFilter = "NYCKEL_FASTIGHET = '" + value + "'";
                                        dtFiltered = dv.ToTable();
                                        dv.Dispose();
                                    }
                            }
                            if (dtFiltered != null)
                            {
                                queryResult = from t in dtPlans.AsEnumerable()
                                              where dtFiltered.Rows.Cast<DataRow>().Any(
                                                s => object.Equals(s["NYCKEL"], t["NYCKEL"])
                                              )
                                              select t;
                            }
                        }

                        // Importerar sökträff i resultattabell
                        // Om match hittas bland planer
                        if (queryResult != null && queryResult.Count<DataRow>() > 0)
                        {
                            dt = queryResult.CopyToDataTable();

                            foreach (DataRow row in dt.Rows)
                            {
                                drResult = dtResult.NewRow();

                                drResult["BEGREPP"] = begrepp;
                                drResult["NYCKEL"] = row["NYCKEL"];
                                drResult["AKT"] = row["AKT"];
                                drResult["AKTTIDIGARE"] = row["AKTTIDIGARE"];
                                drResult["AKTEGEN"] = row["AKTEGEN"];
                                drResult["PLANFK"] = row["PLANFK"];
                                drResult["PLANNAMN"] = row["PLANNAMN"];
                                drResult["ISGENOMF"] = row["ISGENOMF"];
                                drResult["SEARCHEDSTRING"] = potentialPlan;
                            
                                dtResult.Rows.Add(drResult);
                            }
                        }
                        else
                        {
                            drResult = dtResult.NewRow();

                            drResult["BEGREPP"] = begrepp;
                            drResult["SEARCHEDSTRING"] = potentialPlan;
                        
                            dtResult.Rows.Add(drResult);
                        }

                        if (dtFiltered != null)
                        {
                            dtFiltered.Clear();
                            dtFiltered.Dispose();
                        }
                    }
                    else
                    {

                        // Begrepp (kolumner) att villkora mot
                        string[] columns = { "NYCKEL", "AKT", "AKTTIDIGARE", "AKTEGEN", "FASTIGHET", "FASTIGHETNYCKEL" };

                        // För varje begrepp
                        foreach (string column in columns)
                        {
                            if (column.ToUpper() != "FASTIGHET" && column.ToUpper() != "FASTIGHETNYCKEL")
                            {
                                // Filtrering av cachade planer genom linq
                                if (!potentialPlan.ToLower().Contains("1282k-") && column.ToUpper() == "AKTTIDIGARE")
                                {
                                    string akttidigarePlan = "1282k-" + potentialPlan;
                                    queryResult = from t in dtPlans.AsEnumerable()
                                                  where (string.IsNullOrEmpty(t.Field<string>(column.ToUpper())) ? "x" : t.Field<string>(column.ToUpper()).ToLower()) == akttidigarePlan.ToString().ToLower()
                                                  select t;
                                }
                                else
                                {
                                    queryResult = from t in dtPlans.AsEnumerable()
                                                  where (string.IsNullOrEmpty(t.Field<string>(column.ToUpper())) ? "x" : t.Field<string>(column.ToUpper()).ToLower()) == potentialPlan.ToString().ToLower()
                                                  select t;
                                }
                            }

                            DataTable dtFiltered = null;
                            DataView dv = new DataView(dtPlanBerorFastighet);
                            if (column.ToUpper() == "FASTIGHET" || column.ToUpper() == "FASTIGHETNYCKEL")
                            {
                                if (column.ToUpper() == "FASTIGHET")
                                {
                                    dv.RowFilter = "FASTIGHET = '" + potentialPlan.ToUpper().Replace(parcelBlockUnitSearchSign, ":") + "'";
                                    dtFiltered = dv.ToTable();
                                    dv.Dispose();
                                }
                                if (column.ToUpper() == "FASTIGHETNYCKEL")
                                {
                                    // Om datatyp decimal, gör filtreringen
                                    decimal value;
                                    if (Decimal.TryParse(potentialPlan, out value))
                                    {
                                        dv.RowFilter = "NYCKEL_FASTIGHET = '" + value + "'";
                                        dtFiltered = dv.ToTable();
                                        dv.Dispose();
                                    }
                                }
                                if (dtFiltered != null)
                                {
                                    queryResult = from t in dtPlans.AsEnumerable()
                                                  where dtFiltered.Rows.Cast<DataRow>().Any(
                                                    s => object.Equals(s["NYCKEL"], t["NYCKEL"])
                                                  )
                                                  select t;
                                }
                            }

                            // Importerar sökträff i resultattabell
                            // Om match hittas bland planer
                            if (queryResult != null && queryResult.Count<DataRow>() > 0)
                            {
                                dt = queryResult.CopyToDataTable();

                                foreach (DataRow row in dt.Rows)
                                {
                                    drResult = dtResult.NewRow();

                                    drResult["BEGREPP"] = column.ToLower();
                                    drResult["NYCKEL"] = row["NYCKEL"];
                                    drResult["AKT"] = row["AKT"];
                                    drResult["AKTTIDIGARE"] = row["AKTTIDIGARE"];
                                    drResult["AKTEGEN"] = row["AKTEGEN"];
                                    drResult["PLANFK"] = row["PLANFK"]; ;
                                    drResult["PLANNAMN"] = row["PLANNAMN"]; ;
                                    drResult["ISGENOMF"] = row["ISGENOMF"];
                                    drResult["SEARCHEDSTRING"] = potentialPlan;

                                    dtResult.Rows.Add(drResult);
                                }
                            }
                            else
                            {
                                drResult = dtResult.NewRow();

                                drResult["BEGREPP"] = column.ToLower();
                                drResult["SEARCHEDSTRING"] = potentialPlan;

                                dtResult.Rows.Add(drResult);
                            }

                            if (dtFiltered != null)
                            {
                                dtFiltered.Clear();
                                dtFiltered.Dispose();
                            }
                        }
                    }
                }
                dt.Dispose();
            }

            // Datatabell för returnering av sökresultat, distinkta värden
            DataTable dtResultDistinct = new DataTable();
            dtResultDistinct = dtResult.Clone();

            // Skapa distinkt resultat beroende av sökning med begrepp eller wild card
            DataTable tmp1DTDistinct = new DataTable();
            tmp1DTDistinct = getTableSorted(dtResult.DefaultView.ToTable(true), "BEGREPP", "ASC");

            foreach (DataRow dr1 in tmp1DTDistinct.Rows)
            {
                if (!string.IsNullOrWhiteSpace(dr1["NYCKEL"].ToString()))
                {
                    if (dtResultDistinct.Rows.Count != 0)
                    {
                        bool exist = false;
                        foreach (DataRow drDistinct in dtResultDistinct.Rows)
                        {
                            if (drDistinct["NYCKEL"] == dr1["NYCKEL"])
                            {
                                if (drDistinct["BEGREPP"].ToString() != dr1["BEGREPP"].ToString())
                                {
                                    drDistinct["BEGREPP"] += ", " + dr1["BEGREPP"].ToString();
                                }
                                drDistinct["SEARCHEDSTRING"] += ", " + dr1["SEARCHEDSTRING"].ToString();
                                exist = true;
                            }
                        }
                        if (!exist)
                        {
                            dtResultDistinct.ImportRow(dr1);
                        }
                    }
                    else
                    {
                        dtResultDistinct.ImportRow(dr1);
                    }
                }
                else
                {
                    dtResultDistinct.ImportRow(dr1);
                }
            }


            // Plockar bort tabell för temporär lagring av cachade planer och berörda fastigheter från minnet
            if (dtPlans != null)
            {
                dtPlans.Dispose();
            }
            if (dtPlanBerorFastighet != null)
            {
                dtPlanBerorFastighet.Dispose();
            }
            if (tmp1DTDistinct != null)
            {
                tmp1DTDistinct.Dispose();
            }

            // Loggning Sessionens Request sökning
            DataTable dtRequestLog = new DataTable();
            DataColumn dcRequestLog = new DataColumn("NBRSEARCHED", Type.GetType("System.Int32"));
            dtRequestLog.Columns.Add(dcRequestLog);
            dcRequestLog = new DataColumn("NBRHITS", Type.GetType("System.Int32"));
            dtRequestLog.Columns.Add(dcRequestLog);
            dcRequestLog = new DataColumn("WHEN", Type.GetType("System.DateTime"));
            dtRequestLog.Columns.Add(dcRequestLog);
            dcRequestLog = new DataColumn("TIME", Type.GetType("System.Double"));
            dtRequestLog.Columns.Add(dcRequestLog);

            DataRow drRequestLog = dtRequestLog.NewRow();
            drRequestLog["NBRSEARCHED"] = (Int32)Session["PlanerAntal"];
            // Antal funna planer
            DataView dvTempCountPlanHits = dtResultDistinct.AsDataView();
            dvTempCountPlanHits.RowFilter = "NYCKEL IS NOT NULL";
            drRequestLog["NBRHITS"] = dvTempCountPlanHits.Count;
            drRequestLog["WHEN"] = (DateTime)Session["TimeStart"];
            drRequestLog["TIME"] = DateTime.Now.Subtract( (DateTime)Session["TimeStart"] ).TotalMilliseconds;

            dtRequestLog.Rows.Add(drRequestLog);

            UtilityRequest.WriteRequestStatToFile(dtRequestLog);
            UtilityRequest.WriteRequestStatToDb(dtRequestLog);

            return getObjectAsJson(getTableSorted(dtResultDistinct, "AKT", "ASC", "BEGREPP", "ASC"));
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public void jtGetAllPlanInfo(string jtSorting, bool checkHasDocument)
        {
            JTPlans plans = new JTPlans(checkHasDocument);
            plans.Result = "OK";

            plans.sortPlans(jtSorting);

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.Write(jsonSerializer.Serialize(plans));
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getPlansDocs(List<object> planIds)
        {
            Documents planDocs = new Documents(planIds);

            //return getObjectAsJson(getTableSorted(planDocs.SearchedPlansDocuments, "EXTENTION", "ASC", "DOCUMENTTYPE", "ASC"));
            return getObjectAsJson(getTableSorted(planDocs.SearchedPlansDocuments, "DOCUMENTTYPE", "ASC", "EXTENTION", "ASC"));
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getPlansGeometryAsGeoJson(List<object> planIds)
        {
            DataTable dtPlans = PlanCache.GetPlanBasisCache();
            string planAktsAsCsv = string.Join(",",
                dtPlans.AsEnumerable()
                    .Where(row => planIds.Contains(row["NYCKEL"]))
                    .Select(row => $"'{row["AKT"].ToString()}'"));

            DataTable dtPlanGeometriesAsGeoJSON = new DataTable();
            NpgsqlConnection npgsqlCon = UtilityDatabase.GetNpgsqlConnectionForDBGeodata();
            NpgsqlCommand npgsqlCom = new NpgsqlCommand(
                SqlTemplates.GetPlanGeometriAsGeoJson.Replace("@search_string", planAktsAsCsv),
                npgsqlCon);
            NpgsqlDataReader npgsqlDr;

            npgsqlCom.Connection.Open();
            npgsqlDr = npgsqlCom.ExecuteReader();

            dtPlanGeometriesAsGeoJSON.Load(npgsqlDr);

            npgsqlDr.CloseAsync();
            npgsqlDr.DisposeAsync();

            //return getObjectAsJson(dtPlanGeometriesAsGeoJSON);

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(dtPlanGeometriesAsGeoJSON.Rows[0]["result"]);

            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //return jsonSerializer.Serialize(UtilityException.FlattenException(new NotImplementedException()));
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getPlansGeoJson()
        {
            DataTable dtPlans = PlanCache.GetPlanBasisCache();

            DataTable dtPlanGeoJSON = new DataTable();
            NpgsqlConnection npgsqlCon = UtilityDatabase.GetNpgsqlConnectionForDBGeodata();
            NpgsqlCommand npgsqlCom = new NpgsqlCommand(
                SqlTemplates.GetPlanGeoJson,
                npgsqlCon);
            NpgsqlDataReader npgsqlDr;

            npgsqlCom.Connection.Open();
            npgsqlDr = npgsqlCom.ExecuteReader();

            dtPlanGeoJSON.Load(npgsqlDr);

            npgsqlDr.CloseAsync();
            npgsqlDr.DisposeAsync();

            return getObjectAsJson(dtPlanGeoJSON);

            //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            //return jsonSerializer.Serialize(dtPlanGeoJSON.Rows[0]["result"]);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getPlansBerorPlans(List<object> planIds)
        {
            PlanBerorPlan plansBerorPlans = new PlanBerorPlan(planIds);

            return getObjectAsJson(plansBerorPlans.BerordaPlaner);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getDokumenttyper()
        {
            // Hämtar alla dokumenttyper från cache
            Cache cache = HttpRuntime.Cache;
            List<Documenttype> listDocumenttyper = PlanCache.GetPlandocumenttypesCache();

            return getObjectAsJson(listDocumenttyper);
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public void jtGetPlansDocs(List<object> planIds)
        {
            Session["PlanHandling"] = "dokument";

            Documents planDocs = new Documents(planIds);
            JTPlanDocuments planDocuments = new JTPlanDocuments(planDocs.SearchedPlansDocuments);
            planDocuments.Result = "OK";

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            HttpContext.Current.Response.StatusCode = 200;
            HttpContext.Current.Response.Write(jsonSerializer.Serialize(planDocuments));
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getDocsZipped(List<object> planDocsPaths, string zipFileNamePart)
        {
            PackagingZip zipFile = new PackagingZip();
            string zipFileNamePath = zipFile.zipFiles(planDocsPaths, zipFileNamePart);

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(zipFileNamePath);

        }


        private string convertRgbsToHexColor(string rgba)
        {
            string hex = string.Empty;

            List<string> rgbaValues = rgba.Split(',').ToList();

            // Opacity
            double hexMax = 255.0;
            double opacity = Convert.ToDouble(rgbaValues[3].Replace('.', ','));
            Int16 hexOpacity = Convert.ToInt16(hexMax * opacity);

            // #AARRGGBB
            hex = hexOpacity.ToString("X2") +
                  Convert.ToInt16(rgbaValues[0]).ToString("X2") +
                  Convert.ToInt16(rgbaValues[1]).ToString("X2") +
                  Convert.ToInt16(rgbaValues[2]).ToString("X2");

            return hex;
        }

        /// <summary>
        /// Konverterar datatabell till lista av dictionary.
        /// (Anledningen är avsaknad av stöd för serialisering av datatabell till JSON i .NET)
        /// </summary>
        /// <param name="dataTable">
        /// Datatabell enligt .Net DataTable.
        /// </param>
        /// <returns>
        /// JSON-serialiserad string.
        /// </returns>
        private string getObjectAsJson(DataTable dataTable)
        {
            List<Dictionary<string, Object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, Object> row = new Dictionary<string, object>();

            foreach (DataRow dr in dataTable.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(rows);
        }

        private string getObjectAsJson(List<Documenttype> list)
        {
            List<Dictionary<string, Object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, Object> row = new Dictionary<string, object>();

            foreach (var dt in list)
            {
                row = new Dictionary<string, object>();
                row.Add("Type", dt.Type);
                row.Add("UrlFilter", dt.UrlFilter);
                row.Add("Suffix", dt.Suffix);
                row.Add("Description", dt.Description);
                row.Add("IsPlanhandling", dt.IsPlanhandling);
                rows.Add(row);
            }

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(rows);
        }

        /// <summary>
        /// Sorterar datatabell efter önskad kolumn och sorteringsordning. Tar endast hänsyn till en kolumn och en sorteringsordning.
        /// </summary>
        /// <param name="sortableTable">
        /// Datatabell att sortera.
        /// </param>
        /// <param name="sortableColumn">
        /// Kolumn i datatabell som datatabellen ska sorteras på.
        /// </param>
        /// <param name="sortOrder">
        /// Sorteringsordning DESC = fallande | ASC = stigande
        /// </param>
        /// <returns>
        /// Sorterad datatabell.
        /// </returns>
        private DataTable getTableSorted(DataTable sortableTable, string sortableColumn, string sortOrder)
        {
            DataView v = sortableTable.DefaultView;
            v.Sort = sortableColumn + " " + sortOrder;
            sortableTable = v.ToTable();
            return sortableTable;
        }

        /// <summary>
        /// Sorterar datatabell efter önskad kolumn och sorteringsordning. Tar endast hänsyn till en kolumn och en sorteringsordning.
        /// </summary>
        /// <param name="sortableTable">
        /// Datatabell att sortera.
        /// </param>
        /// <param name="sortableColumn">
        /// Kolumn i datatabell som datatabellen ska sorteras på.
        /// </param>
        /// <param name="sortOrder">
        /// Sorteringsordning DESC = fallande | ASC = stigande
        /// </param>
        /// <returns>
        /// Sorterad datatabell.
        /// </returns>
        private DataTable getTableSorted(DataTable sortableTable, string sortableColumn1, string sortOrder1, string sortableColumn2, string sortOrder2)
        {
            DataView v = sortableTable.DefaultView;
            v.Sort = sortableColumn1 + " " + sortOrder1 + ", " + sortableColumn2 + " " + sortOrder2;
            sortableTable = v.ToTable();
            return sortableTable;
        }
    }
}
