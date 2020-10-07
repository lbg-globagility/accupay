using System;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace AccuPay.Utilities
{
    public class WSMService
    {
        private static HttpClient Client = new HttpClient();

        private readonly string _serverIpAddress;
        private readonly string _serviceName;

        public WSMService(string serverIpAddress, string serviceName)
        {
            _serverIpAddress = serverIpAddress;
            _serviceName = serviceName;
        }

        public async Task<HttpResponseMessage> RestartService()
        {
            return await Post("Restart");
        }

        public async Task<HttpResponseMessage> StopService()
        {
            return await Post("Stop");
        }

        public async Task<HttpResponseMessage> StartService()
        {
            return await Post("Start");
        }

        public async Task<HttpResponseMessage> StartOrRestart()
        {
            return await Post("StartOrRestart");
        }

        public async Task<HttpResponseMessage> StopIfRunning()
        {
            return await Post("StopServiceIfRunning");
        }

        private async Task<HttpResponseMessage> Post(string parameter)
        {
            string url = GetUrl(parameter);
            return await Client.PostAsync(url, null);
        }

        public async Task<ServiceControllerStatus> GetStatus()
        {
            string url = GetUrl("status");
            var status = await Client.GetStringAsync(url);

            Enum.TryParse(status, out ServiceControllerStatus controlStatus);

            return controlStatus;
        }

        private string GetUrl(string parameter = null)
        {
            var baseUrl = $"http://{_serverIpAddress}:90/api/manager/{_serviceName}";

            if (parameter == null)
            {
                return baseUrl;
            }
            else
            {
                return $"http://{_serverIpAddress}:90/api/manager/{_serviceName}/{parameter}";
            }
        }
    }
}