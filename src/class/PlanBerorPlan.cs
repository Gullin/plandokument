using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Plan.Plandokument
{

    public class PlanBerorPlan : System.Web.Services.WebService
    {

        /// <summary>
        /// Egenskap 
        /// </summary>
        public DataTable BerordaPlaner { get; private set; }


        public PlanBerorPlan(List<object> planIds)
        {
            this.BerordaPlaner = GetBerordaPlaner(planIds);
        }


        private DataTable GetBerordaPlaner(List<object> planIds)
        {
            // Alla berörsrelationer för planer
            DataTable cachedPlanBerorPlan = PlanCache.GetPlanBerorPlanCache();

            // Tabell för filtrerad sökta planinformationen för vidare sökning
            DataTable dtSearchedPlanBeroenderelationer = cachedPlanBerorPlan.Clone();

            // Filtrera berörsrelationer från alla planrelationer efter sökta planer
            IEnumerable<DataRow> drs = from filteringdPlans in cachedPlanBerorPlan.AsEnumerable()
                                       where planIds.Contains(filteringdPlans.Field<string>("NYCKEL"))
                                       select filteringdPlans;

            foreach (DataRow dr in drs)
            {
                dtSearchedPlanBeroenderelationer.ImportRow(dr);
            }


            cachedPlanBerorPlan.Dispose();


            return dtSearchedPlanBeroenderelationer;
        }


    }
}