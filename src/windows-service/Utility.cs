using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Plan.WindowsService
{
    public static class Utility
    {

        public static string GetServiceParamConfig(string key)
        {
            //Debugger.Launch();

            Assembly service = Assembly.GetAssembly(typeof(ServicePlandokumentThumnails));
            string assemblyPath = service.Location;

            var config = ConfigurationManager.OpenExeConfiguration(assemblyPath);
            AppSettingsSection appSettings = config.AppSettings as AppSettingsSection;

            return appSettings.Settings[key].Value;

        }

    }
}
