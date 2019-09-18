using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Plan.WindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            this.AfterInstall += new InstallEventHandler(ProjectInstaller_AfterInstall);
            //this.AfterUninstall += new InstallEventHandler(ProjectInstaller_AfterUninstall);
        }

        private void ProjectInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(serviceInstaller1.ServiceName))
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
}
