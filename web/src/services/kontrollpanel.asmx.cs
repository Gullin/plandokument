using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Threading;

namespace Plan.Plandokument
{
    /// <summary>
    /// Summary description for cache
    /// </summary>
    [WebService(Namespace = "Landskrona.Apps.Plan.Dokument.Ws")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Kontrollpanel : System.Web.Services.WebService
    {

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlanBasis()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlanBasis());

        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlanBerorFastighet()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlanBerorFastighet());

        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlandocumenttypes()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlandocumenttypes());

        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlanBerorPlan()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlanBerorPlan());

        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsAll()
        {
            bool planBasis = PlanCache.CacheExistsPlanBasis();
            bool planBerorFastighet = PlanCache.CacheExistsPlanBerorFastighet();
            bool planDocumenttypes = PlanCache.CacheExistsPlandocumenttypes();
            bool planBerorPlan = PlanCache.CacheExistsPlanBerorPlan();
            bool exists = false;

            if (planBasis && planBerorFastighet && planDocumenttypes && planBerorPlan)
            {
                exists = true;
            }

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(exists);

        }




        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheRefreshPlanBasis()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            try
            {
                if (PlanCache.CacheExistsPlanBasis())
                {
                    PlanCache.RemoveCachedPlanBasis();
                }
                else
                {
                    PlanCache.setPlanCache();
                }
                return jsonSerializer.Serialize(true.ToString());
            }
            catch
            {
                return jsonSerializer.Serialize(false.ToString());
            }

        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheRefreshPlanBerorFastighet()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            try
            {
                if (PlanCache.CacheExistsPlanBerorFastighet())
                {
                    PlanCache.RemoveCachedPlanBerorFastighet();
                }
                else
                {
                    PlanCache.setPlanBerorFastighetCache();
                }
                return jsonSerializer.Serialize(true.ToString());
            }
            catch
            {
                return jsonSerializer.Serialize(false.ToString());
            }

        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheRefreshPlandocumenttypes()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            try
            {
                if (PlanCache.CacheExistsPlandocumenttypes())
                {
                    PlanCache.RemoveCachePlandocumenttypes();
                }
                else
                {
                    PlanCache.setDocumenttypesCache();
                }
                return jsonSerializer.Serialize(true.ToString());
            }
            catch
            {
                return jsonSerializer.Serialize(false.ToString());
            }

        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheRefreshPlanBerorPlan()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            try
            {
                if (PlanCache.CacheExistsPlanBerorPlan())
                {
                    PlanCache.RemoveCachedPlanBerorPlan();
                }
                else
                {
                    PlanCache.setPlanBerorPlanCache();
                }
                return jsonSerializer.Serialize(true.ToString());
            }
            catch
            {
                return jsonSerializer.Serialize(false.ToString());
            }

        }




        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheTimeDuration()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheDuration());

        }


        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheTimeElapsed()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheElapsed());

        }

    }
}
