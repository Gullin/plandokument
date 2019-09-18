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
        public static int LoggEventID { get; set; } = 0;
    }

    public partial class ServicePlandokumentThumnails : ServiceBase
    {
        public static string LogEventSource { get; set; } = "PlandokumentThumnails";
        public static string LogEvent { get; set; } = "ThumnailsLog";
        private int EventId { get; set; } = 0;
        public FileSystemWatcher fileWatcher = new FileSystemWatcher();

        public ServicePlandokumentThumnails()
        {
            InitializeComponent();

            //eventLog = new EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(LogEventSource))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    LogEventSource, LogEvent);
            }
            LoggEvent.Logger.Source = LogEventSource;
            LoggEvent.Logger.Log = LogEvent;
        }

        protected override void OnStart(string[] args)
        {
            LoggEvent.Logger.WriteEntry("Start av " + this.ServiceName, EventLogEntryType.Information, LoggEvent.LoggEventID++);

            try
            {
                LoggEvent.Logger.WriteEntry("Initierar bevakning av " + Config.WatchedFolder, EventLogEntryType.Information, LoggEvent.LoggEventID++);
                Watcher.Init(fileWatcher);
            }
            catch (Exception ex)
            {

                LoggEvent.Logger.WriteEntry(ex.Message, EventLogEntryType.Information, LoggEvent.LoggEventID++);
                throw;
            }

            //Timer timer = new Timer();
            //timer.Interval = 10000; // 10 seconds
            //timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            //timer.Start();
        }

        //private void OnTimer(object sender, ElapsedEventArgs e)
        //{
        //    eventLog.WriteEntry("Monitorerar Tjänsten ", EventLogEntryType.Information, EventId++);
        //}

        protected override void OnStop()
        {
            LoggEvent.Logger.WriteEntry("Stoppar " + this.ServiceName, EventLogEntryType.Information, LoggEvent.LoggEventID++);
        }
    }
}
