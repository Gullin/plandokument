using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using Plan.Shared.Thumnails;

namespace Plan.WindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller _serviceProcessInstaller { get; set; }
        private ServiceInstaller _serviceInstaller { get; set; }

        public ProjectInstaller()
        {
            InitializeComponent();

            _serviceProcessInstaller = new ServiceProcessInstaller { Account = ServiceAccount.LocalSystem };
            _serviceInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Automatic,
                ServiceName = Utility.GetServiceAppSettings("serviceName"),
                DisplayName = Utility.GetServiceAppSettings("serviceDisplayName"),
                Description = Utility.GetServiceAppSettings("serviceDescription")
            };

            Installers.Add(_serviceProcessInstaller);
            Installers.Add(_serviceInstaller);
            //this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            //this.ServiceProcessInstaller,
            //this.serviceAdmin});

            //Debugger.Launch();

            this.AfterInstall += new InstallEventHandler(ProjectInstaller_AfterInstall);
            //this.AfterUninstall += new InstallEventHandler(ProjectInstaller_AfterUninstall);
        }

        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(_serviceInstaller.ServiceName))
            {
                sc.Start();
            }
        }


        //ERROR: Fungerar ej! Kommer fel i händelsekedjan vilket gör att avinstallationen blir korrupt
        //private void ProjectInstaller_AfterUninstall(object sender, InstallEventArgs e)
        //{
        //    try
        //    {
        //        EventLog.Delete(LoggEvent.LogName);
        //        EventLog.DeleteEventSource(LoggEvent.LogEventSource);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //        //LoggEvent.Logger.WriteEntry("Eventlogg gick inte att radera p.g.a. följande: " + ex.Message, EventLogEntryType.Error, LoggEvent.LoggEventID++);
        //    }
        //}
    }

    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("value", DefaultValue = "", IsRequired = true)]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }

    public class ThumnailsService : ConfigurationSection
    {
        [ConfigurationProperty("serviceName")]
        public ServiceElement ServiceName
        {
            get { return (ServiceElement)base["serviceName"]; }
        }

        [ConfigurationProperty("serviceDisplayName")]
        public ServiceElement ServiceDisplayName
        {
            get { return (ServiceElement)base["serviceDisplayName"]; }
        }

        [ConfigurationProperty("serviceDescription")]
        public ServiceElement ServiceDescription
        {
            get { return (ServiceElement)base["serviceDescription"]; }
        }
    }

}
