using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Plan.WindowsService
{
    class LoggEvent
    {
        public static EventLog Logger { get; set; } = new EventLog();
        public static string LogEventSource { get; set; } = "Plandokument Thumnails Service";
        public static string LogName { get; set; } = "Plandokument Thumnails Service";
        public static int LoggEventID { get; set; } = 0;
    }

    public partial class ServicePlandokumentThumnails : ServiceBase
    {
        public FileSystemWatcher fileWatcher = new FileSystemWatcher();

        public ServicePlandokumentThumnails()
        {
            InitializeComponent();

            if (!System.Diagnostics.EventLog.SourceExists(LoggEvent.LogEventSource))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    LoggEvent.LogEventSource, LoggEvent.LogName);
            }
            LoggEvent.Logger.Source = LoggEvent.LogEventSource;
            LoggEvent.Logger.Log = LoggEvent.LogName;
        }

        protected override void OnStart(string[] args)
        {
            LoggEvent.Logger.WriteEntry("Start av " + this.ServiceName, EventLogEntryType.Information, LoggEvent.LoggEventID++);

            try
            {
                LoggEvent.Logger.WriteEntry("Initierar bevakning av " + ConfigWatcher.WatchedFolder, EventLogEntryType.Information, LoggEvent.LoggEventID++);
                Watcher.Init(fileWatcher);
            }
            catch (Exception ex)
            {

                LoggEvent.Logger.WriteEntry(ex.Message, EventLogEntryType.Information, LoggEvent.LoggEventID++);
                throw;
            }
        }

        protected override void OnStop()
        {
            LoggEvent.Logger.WriteEntry("Stoppar " + this.ServiceName, EventLogEntryType.Information, LoggEvent.LoggEventID++);
        }
    }
}
