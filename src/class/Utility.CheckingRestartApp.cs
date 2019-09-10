using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace Plan.Plandokument
{

    public class CheckingRestartApp : IDisposable
    {

        // Avgör status för CheckingRestartApp
        public bool Cancelled { get; set; } = false;
        // Frekvensen (default värde), i sekunder, kontroll av app:n sker
        private int CheckFrequency = 180;
        AutoResetEvent WaitHandle = new AutoResetEvent(false);
        object SyncLock = new Object();

        public CheckingRestartApp()
        {
        }
        

        ///<summary> Startar bakgrundstråd
        ///</summary>
        ///<param name="checkFrequency">Intervall, i millisekunder, kontroll av app</param>
        public void Start(int checkFrequency)
        {
            // *** Ensure that any waiting instances are shut down
            //this.WaitHandle.Set();
            this.CheckFrequency = checkFrequency;
            this.Cancelled = false;
            Thread t = new Thread(Run);
            t.Start();
        }

        
        ///<summary>Orsakar stopp av processen. Om operationen fortfarande är igång stoppas efter den är slut. </summary>
        public void Stop()
        {
            lock (this.SyncLock)
            {
                if (Cancelled)
                    return;
                this.Cancelled = true;
                this.WaitHandle.Set();
            }
        }

        
        ///<summary>Kör den egentliga processens loop.</summary>
        private void Run()
        {
            // *** Start out  waiting
            this.WaitHandle.WaitOne(this.CheckFrequency * 1000, true);
            while (!Cancelled)
            {
                // *** Http Ping to force the server to stay alive 
                Debug.WriteLine("Ping");
                this.PingServer();
                // *** Put in 
                this.WaitHandle.WaitOne(this.CheckFrequency * 1000, true);
            }
        }


        public void PingServer()
        {
            try
            {
                WebClient http = new WebClient();
                //string Result = http.DownloadString(ConfigurationManager.AppSettings["pingUrl"].ToString());
                StackTrace stackTrace = new StackTrace();

                // Get calling method name
                Debug.WriteLine(stackTrace.GetFrame(3).GetMethod().Name);
                Debug.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);
                Debug.WriteLine("####################" + DateTime.Now.ToString());

            }
            catch
            {
                throw;
            }
        }


        public void Dispose()
        {
            this.Stop();
        }

    }

}