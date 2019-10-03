using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Threading;
using System.Configuration;
using System.Collections.Specialized;

namespace Plan.Plandokument
{

    class ServiceConfig
    {
        public static string ServiceName { get; } = ((NameValueCollection)ConfigurationManager.GetSection("ThumnailsService"))["ServiceName"];
    }

    /// <summary>
    /// Summary description for cache
    /// </summary>
    [WebService(Namespace = "Landskrona.Apps.Plan.Dokument.Ws")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    [System.ComponentModel.ToolboxItem(false)]
    public class Kontrollpanel : System.Web.Services.WebService
    {

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlanBasis()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlanBasis());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlanBerorFastighet()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlanBerorFastighet());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlandocumenttypes()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlandocumenttypes());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheExistsPlanBerorPlan()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheExistsPlanBerorPlan());

        }


        [WebMethod]
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




        [WebMethod]
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


        [WebMethod]
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


        [WebMethod]
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


        [WebMethod]
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




        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheMeta()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.GetCacheMeta());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheTimeDuration()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheDuration());

        }


        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string CacheTimeElapsed()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(PlanCache.CacheElapsed());

        }



        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string ThumnailServiceExists()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(UtilityServicePlandokumentThumnails.ServiceExists(ServiceConfig.ServiceName));

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string ThumnailStartService()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(UtilityServicePlandokumentThumnails.StartService(ServiceConfig.ServiceName));

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string ThumnailStopService()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(UtilityServicePlandokumentThumnails.StopService(ServiceConfig.ServiceName));

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string ThumnailServiceIsRunning()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(UtilityServicePlandokumentThumnails.ServiceIsRunning(ServiceConfig.ServiceName));

        }

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
        public string ThumnailRebootService()
        {

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            return jsonSerializer.Serialize(UtilityServicePlandokumentThumnails.RebootService(ServiceConfig.ServiceName));

        }


    }
}
