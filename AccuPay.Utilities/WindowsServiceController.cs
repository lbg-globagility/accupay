using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.ServiceProcess;

namespace AccuPay.Utilities
{
    public class WindowsServiceController
    {
        private readonly string serviceName;
        private readonly string ipAddress;

        public bool IsRunningStatus => Status == ServiceControllerStatus.Running;

        public bool IsStoppedStatus => Status == ServiceControllerStatus.Stopped;

        public ServiceControllerStatus Status
        {
            get
            {
                using (ServiceController service = GetService())
                {
                    return service.Status;
                }
            }
        }

        private ServiceController GetService()
        {
            return new ServiceController(serviceName, ipAddress);
        }

        public WindowsServiceController(string serviceName, string ipAddress)
        {
            if (ipAddress == null) ipAddress = GetLocalIpAddress();

            this.serviceName = serviceName;
            this.ipAddress = ipAddress;
        }

        // this method will throw an exception if the service is NOT in Running status.
        public void RestartService()
        {
            using (ServiceController service = GetService())
            {
                try
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Can not restart the Windows Service {serviceName}", ex);
                }
            }
        }

        // this method will throw an exception if the service is NOT in Running status.
        public void StopService()
        {
            using (ServiceController service = GetService())
            {
                try
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Can not Stop the Windows Service [{serviceName}]", ex);
                }
            }
        }

        // this method will throw an exception if the service is NOT in Stopped status.
        public void StartService()
        {
            using (ServiceController service = GetService())
            {
                try
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Can not Start the Windows Service [{serviceName}]", ex);
                }
            }
        }

        // if service running then restart the service if the service is stopped then start it.
        // this method will not throw an exception.
        public void StartOrRestart()
        {
            if (IsRunningStatus)
                RestartService();
            else if (IsStoppedStatus)
                StartService();
        }

        // stop the service if it is running. if it is already stopped then do nothing.
        // this method will not throw an exception if the service is in Stopped status.
        public void StopServiceIfRunning()
        {
            using (ServiceController service = GetService())
            {
                try
                {
                    if (!IsRunningStatus)
                        return;

                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Can not Stop the Windows Service [{serviceName}]", ex);
                }
            }
        }

        public static string GetLocalIpAddress()
        {
            UnicastIPAddressInformation mostSuitableIp = null;

            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;

                var properties = network.GetIPProperties();

                if (properties.GatewayAddresses.Count == 0)
                    continue;

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    if (!address.IsDnsEligible)
                    {
                        if (mostSuitableIp == null)
                            mostSuitableIp = address;
                        continue;
                    }

                    // The best IP is the IP got from DHCP server
                    if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    {
                        if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                            mostSuitableIp = address;
                        continue;
                    }

                    return address.Address.ToString();
                }
            }

            return mostSuitableIp != null
                ? mostSuitableIp.Address.ToString()
                : "";
        }
    }
}