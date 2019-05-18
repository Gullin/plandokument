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
using OSGeo.MapGuide;
using Plan.Plandokument.MapLayerDefinition;
using Plan.Plandokument.jTable;

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
            DataTable dtPlans = (DataTable)cache["Plans"];

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
            DataTable dtPlans = (DataTable)cache["Plans"];

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
            string conStr = ConfigurationManager.AppSettings["OracleOleDBConString"].ToString();
            string sql = string.Empty;

            sql = "SELECT COUNT(*) AS antal " + 
                  "FROM   gis_v_planytor " + 
                  "WHERE  SYSDATE BETWEEN TO_DATE(dat_genomf_f, 'YYYY-MM-DD') AND TO_DATE(dat_genomf_t, 'YYYY-MM-DD')";
            
            DataTable dtImplement = new DataTable();
            OleDbConnection con = new OleDbConnection(conStr);
            OleDbCommand com = new OleDbCommand(sql, con);
            OleDbDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dtImplement.Load(dr);

            dr.Close();
            dr.Dispose();

            //return getDatatableAsJson(dtImplement);

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(dtImplement.Rows[0]["ANTAL"].ToString());
        }


        //TODO: Kontrollera för om sökning görs mot samma begrepp med värde som finns vid flera tilfällen
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
            DataTable dtPlans = (DataTable)cache["Plans"];

            // Hämtar berörda fastigheter från cache om sökning görs på fastighet (begrepp)
            // Hämtas endast från cache när inget begrepp eller sökt specifikt på fastighetsnyckel eller fastighetsbeteckning
            string parcelBlockUnitSearchSign = "";
            DataTable dtPlanBerorFastighet = null;
            if (string.IsNullOrWhiteSpace(begrepp) || begrepp.ToUpper() == "FASTIGHET" || begrepp.ToUpper() == "FASTIGHETNYCKEL")
            {
                parcelBlockUnitSearchSign = ConfigurationManager.AppSettings["URLParcelBlockUnitSign"].ToString();
                dtPlanBerorFastighet = (DataTable)cache["PlanBerorFastighet"];
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

            bool isBegrepp = false;

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

                    //TODO: Vända på forech- och if-satsen, kontrollen om "begrepp" är använt kontrolleras för varje planDoc (onödigt) på samma sätt med boolean-värdet "isBegrepp"
                    // Om kolumn är definierad, söker på definerad kolumn annars mot alla
                    if (!string.IsNullOrWhiteSpace(begrepp))
                    {
                        isBegrepp = true;

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
                        isBegrepp = false;

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

            UtilityRequest.LogRequestStatsAsync(dtRequestLog);

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

            return getObjectAsJson(getTableSorted(planDocs.SearchedPlansDocuments, "EXTENTION", "ASC", "DOCUMENTTYPE", "ASC"));
        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string getDokumenttyper()
        {
            Documenttypes dt = new Documenttypes();
            List<Documenttype> lista = dt.GetDocumenttypes; 

            return getObjectAsJson(lista);
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
        public string getPlanMapImageAsBase64String(string planID, string imageWidth, string imageHeight)
        {
            try
            {
                MgUserInformation userInfo = null;
                string mapWebTierInit = ConfigurationManager.AppSettings["MGWebTierInit"].ToString();
                MapGuideApi.MgInitializeWebTier(mapWebTierInit);

                // Om kartsession finns för applikationssession använd den annars skapa ny kartssitesession
                string mapSiteSessionID = null;
                if (Session["MapSiteSessionID"] != null)
                {
                    mapSiteSessionID = Session["MapSiteSessionID"].ToString();
                    userInfo = new MgUserInformation(mapSiteSessionID);
                }
                else
                {
                    // Initierar kartsite och kartsession 
                    string mapUserName = ConfigurationManager.AppSettings["MGUserName"].ToString();
                    string mapUserPass = ConfigurationManager.AppSettings["MGUserPass"].ToString();

                    userInfo = new MgUserInformation(mapUserName, mapUserPass);
                    MgSite mapSite = new MgSite();
                    mapSite.Open(userInfo);

                    userInfo.Dispose();

                    mapSiteSessionID = mapSite.CreateSession();

                    //mapSite.Close();

                    Session["MapSiteSessionID"] = mapSiteSessionID;

                    userInfo = new MgUserInformation(mapSiteSessionID);
                }

                //bool test = resSvc.ResourceExists(mapResId);

                string mapSurfaceFactor = ConfigurationManager.AppSettings["MGMapSurfaceFactor"].ToString();
                string mapRes = ConfigurationManager.AppSettings["MGMapResource"].ToString();
                string planRes = ConfigurationManager.AppSettings["MGPlanytorResource"].ToString();
                string planClassName = ConfigurationManager.AppSettings["MGPlanytorClassName"].ToString();
                string planFilterColumn = ConfigurationManager.AppSettings["MGPlanytorFilterColumn"].ToString();
                string planGeometryColumn = ConfigurationManager.AppSettings["MGPlanytorGeometryColumn"].ToString();
                string planytorStrokeRgbaColor = ConfigurationManager.AppSettings["MGPlanytorStrokeRgbaColor"].ToString();
                string planytorForegroundRgbaColor = ConfigurationManager.AppSettings["MGPlanytorForegroundRgbaColor"].ToString();


                string mapImageSizeFromServer = ConfigurationManager.AppSettings["MGMapImageSizeFromServer"].ToString();

                // Standardvärde för storlek på kartbild, används om värde ej finns i Settings.config eller skickas in som parametrar i webbmetod
                string mapImageWidthPixel = "400";
                string mapImageHeightPixel = "300";
                // Väljer bredd och höjd på kartbild om värde ska finnas i Settings.config samt indikeras att de ska användas
                // annars förväntas värde skickas med i webbmetod
                if (mapImageSizeFromServer.ToLower() == "true")
                {
                    mapImageWidthPixel = ConfigurationManager.AppSettings["MGMapImageWidth"].ToString();
                    mapImageHeightPixel = ConfigurationManager.AppSettings["MGMapImageHeight"].ToString();
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(imageWidth) && !string.IsNullOrWhiteSpace(imageHeight))
                    {
                        mapImageWidthPixel = imageWidth;
                        mapImageHeightPixel = imageHeight;
                    }
                }

                MgSiteConnection siteConnection = new MgSiteConnection();
                siteConnection.Open(userInfo);

                MgResourceService resSvc = (MgResourceService)siteConnection.CreateService(MgServiceType.ResourceService);
                MgResourceIdentifier mapResId = new MgResourceIdentifier(mapRes);
                MgFeatureService featSvc = (MgFeatureService)siteConnection.CreateService(MgServiceType.FeatureService);
                MgResourceIdentifier planResId = new MgResourceIdentifier(planRes);

                // Filter för planDoc efter planDoc-ID
                MgFeatureQueryOptions featureQuery = new MgFeatureQueryOptions();
                featureQuery.SetFilter(planFilterColumn + " = " + planID);

                MgFeatureReader featureReader = featSvc.SelectFeatures(planResId, planClassName, featureQuery);
                MgByteReader byteReaderGeometry = null;
                MgAgfReaderWriter agfReaderWriter = new MgAgfReaderWriter();
                MgGeometryCollection geometryCollection = new MgGeometryCollection();
                int featureCount = 0;
                try
                {
                    while (featureReader.ReadNext())
                    {
                        byteReaderGeometry = featureReader.GetGeometry(planGeometryColumn);
                        MgGeometry districtGeometry = agfReaderWriter.Read(byteReaderGeometry);
                        geometryCollection.Add(districtGeometry);

                        featureCount++;
                    }
                }
                finally
                {
                    featureReader.Close();
                }

                MgGeometryFactory geometryFactory = new MgGeometryFactory();
                MgMultiGeometry multiGeometry = geometryFactory.CreateMultiGeometry(geometryCollection);

                MgMap map = new MgMap(siteConnection);

                MgEnvelope envelope = multiGeometry.Envelope();

                // Anpassar ev. punkt till komma i tal som hanteras som textsträng för kommande konvertering
                if (mapImageHeightPixel.IndexOf(".") != -1)
                {
                    mapImageHeightPixel = mapImageHeightPixel.Replace(".", ",");
                }
                if (mapImageWidthPixel.IndexOf(".") != -1)
                {
                    mapImageWidthPixel = mapImageWidthPixel.Replace(".", ",");
                }

                // Önskad bilds höjd och bredd i punkter
                double imageHeightPixel = Convert.ToDouble(mapImageHeightPixel);
                double imageWidthPixel = Convert.ToDouble(mapImageWidthPixel);

                map.DisplayDpi = 120;

                double heightEnvelopeN = envelope.Height;
                double widthEnvelopeE = envelope.Width;

                // Anpassar utbredningen på sökta planer (envelope) till bildens format för att bevara skalriktighet
                string mapFarthest = string.Empty;
                if (heightEnvelopeN > widthEnvelopeE)
                    mapFarthest = "height";
                else
                    mapFarthest = "width";

                string imageFarthest = string.Empty;
                if (imageHeightPixel > imageWidthPixel)
                    imageFarthest = "height";
                else
                    imageFarthest = "width";

                double scale = 1.0;
                const double inch = 2.54;

                // Ändring av kartans utbredning och addering av utrymme i kartans bild runt planavgränsningen
                // Map = avgränsning enligt planytan (utbredning i kartan), Image = önskad bild att skapa med kartan
                // Om: kartans höjd är längst & bildens bredd är längst
                if (mapFarthest == "height" && imageFarthest == "width")
                {
                    scale = imageWidthPixel / imageHeightPixel * inch;

                    widthEnvelopeE = imageWidthPixel / imageHeightPixel * heightEnvelopeN * scale;
                }
                // Om: kartans bredd är längst & bildens höjd är längst
                else if (mapFarthest == "width" && imageFarthest == "height")
                {
                    scale = imageHeightPixel / imageWidthPixel * inch;

                    heightEnvelopeN = imageHeightPixel / imageWidthPixel * widthEnvelopeE * scale;
                }
                // Om: kartans höjd är längst & bildens höjd är längst
                else if (mapFarthest == "height" && imageFarthest == "height")
                {
                    double compareSide = (heightEnvelopeN / (imageHeightPixel / imageWidthPixel));
                    bool isCompareSideFarthest = false;
                    if (compareSide > widthEnvelopeE)
                    {
                        isCompareSideFarthest = true;
                    }
                    else
                    {
                        isCompareSideFarthest = false;
                    }

                    scale = imageHeightPixel / imageWidthPixel * inch;

                    if (isCompareSideFarthest)
                    {
                        widthEnvelopeE = heightEnvelopeN / (imageHeightPixel / imageWidthPixel) * scale;
                    }
                    else
                    {
                        heightEnvelopeN = (imageHeightPixel / imageWidthPixel) * widthEnvelopeE * scale;
                    }
                }
                // Om(annars): kartans bredd är längst & bildens bredd är längst
                else
                {
                    double compareSide = (widthEnvelopeE / (imageWidthPixel / imageHeightPixel));
                    bool isCompareSideFarthest = false;
                    if (compareSide > heightEnvelopeN)
                    {
                        isCompareSideFarthest = true;
                    }
                    else
                    {
                        isCompareSideFarthest = false;
                    }

                    scale = imageWidthPixel / imageHeightPixel * inch;

                    if (isCompareSideFarthest)
                    {
                        heightEnvelopeN = widthEnvelopeE / (imageWidthPixel / imageHeightPixel) * scale;
                    }
                    else
                    {
                        widthEnvelopeE = heightEnvelopeN * (imageWidthPixel / imageHeightPixel) * scale;
                    }
                }

                double mapSurfaceFactorDbl = Convert.ToDouble(mapSurfaceFactor.Replace('.', ','));
                double newHeightN = heightEnvelopeN * mapSurfaceFactorDbl;
                double newWidthE = widthEnvelopeE * mapSurfaceFactorDbl;
                MgCoordinate lowerLeft = envelope.LowerLeftCoordinate;
                MgCoordinate upperRight = envelope.UpperRightCoordinate;
                envelope = new MgEnvelope(lowerLeft.X - (newWidthE - widthEnvelopeE) / 2,
                                          lowerLeft.Y - (newHeightN - heightEnvelopeN) / 2,
                                          upperRight.X + (newWidthE - widthEnvelopeE) / 2,
                                          upperRight.Y + (newHeightN - heightEnvelopeN) / 2);



                map.Create(resSvc, mapResId, mapResId.Name);


                // Skapa lagerdefinition i XML
                DefineAreaLayer areaLayer = new DefineAreaLayer();
                areaLayer.FeatureName = planClassName;
                areaLayer.FeatureSourceName = planRes;
                areaLayer.GeometryColumnName = planGeometryColumn;
                areaLayer.Filter = planFilterColumn + " = " + planID;

                LayerScaleRangeCollection lsrCollection = new LayerScaleRangeCollection();

                LayerScaleRange lsr = new LayerScaleRange();
                // MinScale applikations-default till 0 (inklusive) om utelämnat, MaxScale applikations-default till kartans maxskala (exklusive) om utelämnat
                //lsr.MinScale = "0";
                //lsr.MaxScale = "100000000";
                AreaTypeStyle ats = new AreaTypeStyle();

                AreaRuleCollection arCollection = new AreaRuleCollection();

                AreaRule ar = new AreaRule();
                //ar.Filter = planFilterColumn + " = " + planID;
                ar.LegendLabel = "Plan" + planID;
                AreaSymbolization2D symb2D = new AreaSymbolization2D();
                Fill fill = new Fill();
                fill.BackgroundColor = "FFFF0000";
                fill.FillPattern = "Solid";
                fill.ForegroundColor = convertRgbsToHexColor(planytorForegroundRgbaColor);
                Stroke stroke = new Stroke();
                stroke.Color = convertRgbsToHexColor(planytorStrokeRgbaColor);
                stroke.LineStyle = "Solid";
                stroke.Thickness = "1";
                stroke.Unit = "Points";
                symb2D.Fill = fill;
                symb2D.Stroke = stroke;
                ar.Symbolization2D = symb2D;
                arCollection.Add(ar);


                ats.AreaRules = arCollection;

                lsr.AreaTypeStyle = ats;

                lsrCollection.Add(lsr);

                areaLayer.LayerScaleRanges = lsrCollection;

                XmlDocument xmlFile = new XmlDocument();
                //XDocument xmlFile = new XDocument();
                // om returnerande av xml-dokument
                //xmlFile = areaLayer.CreateLayerDefinitionAsXmlDocument();
                // om returnerande av xml-sträng
                xmlFile.LoadXml(areaLayer.CreateLayerDefinitionAsXmlString());
                //xmlFile = areaLayer.CreateLayerDefinitionAsXDocument();
                //xmlFile.Save(Server.MapPath(this.Context.Request.ApplicationPath) + "XmlTestLayerDefinition.xml");


                using (MemoryStream msNewPlanLayer = new MemoryStream())
                {
                    xmlFile.Save(msNewPlanLayer);
                    msNewPlanLayer.Position = 0L;
                    //Note we do this to ensure our XML content is free of any BOM characters
                    byte[] layerDefinition = msNewPlanLayer.ToArray();
                    Encoding utf8 = Encoding.UTF8;
                    String layerDefStr = new String(utf8.GetChars(layerDefinition));
                    layerDefinition = new byte[layerDefStr.Length - 1];
                    int byteCount = utf8.GetBytes(layerDefStr, 1, layerDefStr.Length - 1, layerDefinition, 0);
                    // Save the new layer definition to the session repository  
                    MgByteSource byteSource = new MgByteSource(layerDefinition, layerDefinition.Length);
                    MgResourceIdentifier layerResourceID = new MgResourceIdentifier("Session:" + mapSiteSessionID + "//" + "planytor" + ".LayerDefinition"); //"SearchedPlan" + planID + ".LayerDefinition");
                    resSvc.SetResource(layerResourceID, byteSource.GetReader(), null);

                    MgLayer newPlanLayer = new MgLayer(layerResourceID, resSvc);
                    newPlanLayer.SetName("Sökta planer");
                    newPlanLayer.SetVisible(true);
                    newPlanLayer.SetLegendLabel("Sökta planer");
                    newPlanLayer.SetDisplayInLegend(true);
                    MgLayerCollection layerCollection = map.GetLayers();
                    if (!layerCollection.Contains(newPlanLayer))
                    {
                        // Insert the new layer at position 0 so it is at the top
                        // of the drawing order
                        layerCollection.Insert(0, newPlanLayer);
                    }
                    else
                    {
                        layerCollection.Remove(newPlanLayer);
                        layerCollection.Insert(0, newPlanLayer);
                    }

                    map.Save();
                }

                double mapScale = map.ViewScale;



                // XML-dokument till ren text
                //StringWriter stringWriter = new StringWriter();
                //XmlWriter xmlTextWriter = XmlWriter.Create(stringWriter);
                //xmlFile.WriteTo(xmlTextWriter);
                //xmlTextWriter.Flush();
                string xmlSelection = string.Empty;
                //string xmlSelection = stringWriter.GetStringBuilder().ToString();

                MgSelection selection = null;
                if (!string.IsNullOrEmpty(xmlSelection))
                {
                    selection = new MgSelection(map, xmlSelection);
                }
                else
                {
                    selection = new MgSelection(map);
                }


                MgColor color = new MgColor("255,255,255");

                // Skapar bild av kartan
                MgRenderingService renderingService = (MgRenderingService)siteConnection.CreateService(MgServiceType.RenderingService);
                //MgByteReader byteReader = renderingService.RenderMap(map, selection, "PNG");
                MgByteReader byteReader = renderingService.RenderMap(map, selection, envelope, Convert.ToInt32(imageWidthPixel), Convert.ToInt32(imageHeightPixel), color, "PNG");
                MemoryStream ms = new MemoryStream();
                byte[] byteBuffer = new byte[1024];
                int numBytes = byteReader.Read(byteBuffer, 1024);
                while (numBytes > 0)
                {
                    ms.Write(byteBuffer, 0, numBytes);
                    numBytes = byteReader.Read(byteBuffer, 1024);
                }
                byte[] mapImageByte = ms.ToArray();
                string imageBase64String = Convert.ToBase64String(mapImageByte);



                map.Dispose();
                siteConnection.Dispose();

                DataTable dtResult = new DataTable();
                DataColumn dc = new DataColumn("MAPIMAGEBASE64");
                dtResult.Columns.Add(dc);
                dc = new DataColumn("WIDTH");
                dtResult.Columns.Add(dc);
                dc = new DataColumn("HEIGHT");
                dtResult.Columns.Add(dc);
                DataRow dr = dtResult.NewRow();
                dr["MAPIMAGEBASE64"] = imageBase64String;
                dr["WIDTH"] = imageWidthPixel;
                dr["HEIGHT"] = imageHeightPixel;
                dtResult.Rows.Add(dr);

                //TODO: MAP: Vad kan returneras, base64 eller länk där bild temporärt genereras på server
                //JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                //return jsonSerializer.Serialize(imageBase64String);

                return getObjectAsJson(dtResult);
            }
            catch (System.Exception ex)
            {
                UtilityException.LogException(ex, "Webbmetod : getPlanMapImageAsBase64String", true);
                return null;
            }
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
