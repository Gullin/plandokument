﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Web;
using Plan.Shared.Thumnails;

namespace Plan.Plandokument
{

    public class ServiceMeta
    {
        public string ServiceName { get; set; }
        public string ServiceDisplayName { get; set; }
        public string ServiceDescription { get; set; }
        public string WatchedFolder { get; set; }
        public string ThumnailsFolder { get; set; }
    }

    public class UtilityServicePlandokumentThumnails
    {
        public static ServiceMeta GetServiceMeta()
        {
            ServiceMeta serviceMeta = new ServiceMeta
            {
                ServiceName = ConfigShared.ServiceName,
                ServiceDisplayName = ConfigShared.ServiceDisplayName,
                ServiceDescription = ConfigShared.ServiceDescription,
                WatchedFolder = ConfigShared.WatchedFolder,
                ThumnailsFolder = ConfigShared.ThumnailsFolder
            };

            return serviceMeta;
        }

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
            try
            {
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    // Startar tjänsten om dess nuvarande status är stoppad
                    try
                    {
                        // Startar tjänsten och väntar tills status är "Stopped"
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running);
                    }
                    catch (InvalidOperationException exc)
                    {
                        UtilityException.LogException(exc, $"Plandokument Thumnails : StartService {ServiceName}", false);
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception exc)
            {
                UtilityException.LogException(exc, $"Plandokument Thumnails : StartService {ServiceName}", true);
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

            try
            {
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
                        UtilityException.LogException(ex, $"Plandokument Thumnails : StopService {ServiceName}", false);
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception exc)
            {
                UtilityException.LogException(exc, $"Plandokument Thumnails : StopService {ServiceName}", true);
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

            try
            {
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                UtilityException.LogException(exc, $"Plandokument Thumnails : ServiceIsRunning {ServiceName}", true);
            }

            return false;
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
                    if (!StopService(ServiceName))
                    {
                        return false;
                    }
                }

                if (!StartService(ServiceName))
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }

}