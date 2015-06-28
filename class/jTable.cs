using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Plan.Plandokument;

namespace Plan.Plandokument.jTable
{
	public class JTPlans
	{

        // Antar endast värden OK eller ERROR
        public string Result { get; set; }
        // Lista med objekt innehållande informationen som ska utgöra rader i jTable
        public IList<Plan> Records { get; private set; }
        // Innehåller värde när egenskapen Result i sin tur har värde "ERROR"
        public string Message { get; set; }

        public JTPlans(bool checkPlanHasDocument)
        {
            // Hämtar alla planer från cache
            Cache cache = HttpRuntime.Cache;
            DataTable dtPlans = (DataTable)cache["Plans"];

            Records = buildListOfPlans(dtPlans, checkPlanHasDocument);
        }

        private IList<Plan> buildListOfPlans(DataTable dtPlans, bool checkPlanHasDocument)
        {
            DataColumnCollection columns = dtPlans.Columns;
            if (!columns.Contains("HASDOCUMENT"))
            {
                DataColumn dc = new DataColumn("HASDOCUMENT", System.Type.GetType("System.Boolean"));
                dtPlans.Columns.Add(dc);
            }

            if (checkPlanHasDocument)
            {
                dtPlans = planHasDocument(dtPlans);
            }
            
            IList<Plan> list = new List<Plan>();

            foreach (DataRow plan in dtPlans.Rows)
                list.Add(getPlan(plan));

            return list;
        }

        private Plan getPlan(DataRow drPlan)
        {
            Plan plan = new Plan();

            plan.Nyckel = int.Parse(drPlan["NYCKEL"].ToString());
            plan.Akt = drPlan["AKT"].ToString();
            plan.AktTidigare = drPlan["AKTTIDIGARE"].ToString();
            plan.AktEgen = drPlan["AKTEGEN"].ToString();
            plan.PlanFk = drPlan["PLANFK"].ToString();
            plan.PlanNamn = drPlan["PLANNAMN"].ToString();
            plan.IsGenomf = int.Parse(drPlan["ISGENOMF"].ToString());
            plan.DatBeslut = drPlan["DAT_BESLUT"].ToString();
            plan.DatGenomfFran = drPlan["DAT_GENOMF_F"].ToString();
            plan.DatGenomfTill = drPlan["DAT_GENOMF_T"].ToString();
            plan.DatLagaKraft = drPlan["DAT_LAGAKRAFT"].ToString();
            plan.PlanAvgift = drPlan["PLANAVGIFT"].ToString();
            plan.HasDocument = Convert.IsDBNull(drPlan["HASDOCUMENT"]) ? (bool?)null : Convert.ToBoolean(drPlan["HASDOCUMENT"].ToString());

            return plan;
        }

        private DataTable planHasDocument(DataTable dtPlans)
        {
            // Konvertera kolumn med plannycklar till lista med nycklar
            List<object> planIds = new List<object>();
            foreach (DataRow dr in dtPlans.Rows)
            {
                planIds.Add(dr["NYCKEL"].ToString());
            }

            DataTable dtPlansDocuments = new DataTable();
            Documents docsPlansDocuments = new Documents(planIds);
            dtPlansDocuments = docsPlansDocuments.SearchedPlansDocuments;

            foreach (DataRow rowPlan in dtPlans.Rows)
            {
                rowPlan["HASDOCUMENT"] = false;
                foreach (DataRow rowPlanDocs in dtPlansDocuments.Rows)
                {
                    if (rowPlan["NYCKEL"].ToString() == rowPlanDocs["PLAN_ID"].ToString())
                    {
                        rowPlan["HASDOCUMENT"] = true;
                        break;
                    }
                }
            }

            dtPlansDocuments.Dispose();
            docsPlansDocuments.Dispose();

            return dtPlans;
        }

        public IList<Plan> sortPlans(string jtSorting)
        {
            try
            {
                if (!string.IsNullOrEmpty(jtSorting))
                {
                    string[] sorter = jtSorting.Split(new char[] { ' ' });
                    IList<jTable.Plan> sortingListPlans = (List<jTable.Plan>)this.Records;

                    if (sorter[1].ToString() == "ASC")
                    {

                        if (sorter[0].ToString() == "Akt")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Akt).ToList();
                        }
                        else if (sorter[0].ToString() == "AktEgen")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.AktEgen).ToList();
                        }
                        else if (sorter[0].ToString() == "AktTidigare")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.AktTidigare).ToList();
                        }
                        else if (sorter[0].ToString() == "IsGenomf")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.IsGenomf).ToList();
                        }
                        else if (sorter[0].ToString() == "Nyckel")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Nyckel).ToList();
                        }
                        else if (sorter[0].ToString() == "PlanFk")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.PlanFk).ToList();
                        }
                        else if (sorter[0].ToString() == "PlanNamn")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.PlanNamn).ToList();
                        }
                        else if (sorter[0].ToString() == "DatBeslut")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.DatBeslut).ToList();
                        }
                        else if (sorter[0].ToString() == "HasDocument")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.HasDocument).ToList();
                        }
                        else
                        {
                            throw new Exception("Ingen sorteringskolumn definierad för ASC-sortering");
                        }
                    }
                    else if (sorter[1].ToString() == "DESC")
                    {
                        if (sorter[0].ToString() == "Akt")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Akt).ToList();
                        }
                        else if (sorter[0].ToString() == "AktEgen")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.AktEgen).ToList();
                        }
                        else if (sorter[0].ToString() == "AktTidigare")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.AktTidigare).ToList();
                        }
                        else if (sorter[0].ToString() == "IsGenomf")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.IsGenomf).ToList();
                        }
                        else if (sorter[0].ToString() == "Nyckel")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Nyckel).ToList();
                        }
                        else if (sorter[0].ToString() == "PlanFk")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.PlanFk).ToList();
                        }
                        else if (sorter[0].ToString() == "PlanNamn")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.PlanNamn).ToList();
                        }
                        else if (sorter[0].ToString() == "DatBeslut")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.DatBeslut).ToList();
                        }
                        else if (sorter[0].ToString() == "HasDocument")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.HasDocument).ToList();
                        }
                        else
                        {
                            throw new Exception("Ingen sorteringskolumn definierad för DESC-sortering");
                        }
                    }
                    else
                    {
                        throw new Exception("Ingen sortering definierad (ASC|DESC)");
                    }
                }
                else
                {
                    throw new Exception("Inga parametervärden för sortering");
                }
            }
            catch
            {
                throw;
            }
        }

    }

    public class JTPlanDocuments
    {
        // Antar endast värden OK eller ERROR
        public string Result { get; set; }
        // Lista med objekt innehållande informationen som ska utgöra rader i jTable
        public IList<PlanDocument> Records { get; private set; }
        // Innehåller värde när egenskapen Result i sin tur har värde "ERROR"
        public string Message { get; set; }

        public JTPlanDocuments(DataTable dtPlanDocuments)
        {
            Records = buildListOfPlanDocuments(dtPlanDocuments);
        }

        private IList<PlanDocument> buildListOfPlanDocuments(DataTable dtPlanDocuments)
        {
            IList<PlanDocument> list = new List<PlanDocument>();

            foreach (DataRow planDoc in dtPlanDocuments.Rows)
                list.Add(getPlanDoc(planDoc));

            return list;
        }

        private PlanDocument getPlanDoc(DataRow planDoc)
        {
            PlanDocument doc = new PlanDocument();

            doc.Path = planDoc["PATH"].ToString();
            doc.Name = planDoc["NAME"].ToString();
            doc.Extention = planDoc["EXTENTION"].ToString();
            doc.Size = int.Parse(planDoc["SIZE"].ToString());
            doc.PlanId = planDoc["PLAN_ID"].ToString();
            doc.DocumentType = planDoc["DOCUMENTTYPE"].ToString();

            return doc;
        }

    }

    public class Plan
    {

        public Int32 Nyckel { get; set; }
        public string Akt { get; set; }
        public string AktTidigare { get; set; }
        public string AktEgen { get; set; }
        public string PlanFk { get; set; }
        public string PlanNamn { get; set; }
        public Int32 IsGenomf { get; set; }
        public string DatBeslut { get; set; }
        public string DatGenomfFran { get; set; }
        public string DatGenomfTill { get; set; }
        public string DatLagaKraft { get; set; }
        public string PlanAvgift { get; set; }
        public bool? HasDocument { get; set; }

    }

    public class PlanDocument
    {

        public string Path { get; set; }
        public string Name { get; set; }
        public string Extention { get; set; }
        public Int64 Size { get; set; }
        public string PlanId { get; set; }
        public string DocumentType { get; set; }

    }
}