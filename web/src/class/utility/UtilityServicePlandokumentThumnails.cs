using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Web;

namespace Plan.Plandokument
{

    public class UtilityServicePlandokumentThumnails
    {
        /// <summary>
        /// Verifierar om Windows tjänst existerar
        /// </summary>
        /// <param name="ServiceName">Windows tjänstenamn</param>
        /// <returns>Sant om Windows tjänst existerar, annars falskt </returns>
        public static bool ServiceExists(string ServiceName)
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(ServiceName));
        }


        /// <summary>
        /// Startar en Windows tjänst genom dess namn
        /// </summary>
        /// <param name="ServiceName">Windows tjänstenamn</param>
        public static bool StartService(string ServiceName)
        {
            ServiceController sc = new ServiceController();
            sc.ServiceName = ServiceName;

            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                // Startar tjänsten om dess nuvarande status är stoppad
                try
                {
                    // Startar tjänsten och väntar tills status är "Stopped"
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running);
                }
                catch (InvalidOperationException ex)
                {
                    UtilityException.LogException(ex, "Plandokument Thumnails : StartService", false);
                    return false;
                }

                return true;
            }

            return false;
        }


        /// <summary>
        /// Stoppar Windows tjänst som kör
        /// </summary>
        /// <param name="ServiceName">Windows tjänstenamn</param>
        public static bool StopService(string ServiceName)
        {
            ServiceController sc = new ServiceController();
            sc.ServiceName = ServiceName;

            if (sc.Status == ServiceControllerStatus.Running)
            {
                // Stoppar tjänsten om dess nuvarande status är "Running"
                try
                {
                    // Stoppar tjänsten och väntar tills status är "Stopped"
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                catch (InvalidOperationException ex)
                {
                    UtilityException.LogException(ex, "Plandokument Thumnails : StopService", false);
                    return false;
                }

                return true;
            }

            return false;
        }


        /// <summary>
        /// Verifierar om Windows tjänsten kör
        /// </summary>
        /// <param name="ServiceName">Windows tjänstenamn</param>
        public static bool ServiceIsRunning(string ServiceName)
        {
            ServiceController sc = new ServiceController();
            sc.ServiceName = ServiceName;

            if (sc.Status == ServiceControllerStatus.Running)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Startar om Windows tjänst
        /// </summary>
        /// <param name="ServiceName"></param>
        public static bool RebootService(string ServiceName)
        {
            if (ServiceExists(ServiceName))
            {
                if (ServiceIsRunning(ServiceName))
                {
                    StopService(ServiceName);
                }

                StartService(ServiceName);

                return true;
            }

            return false;
        }
    }
}