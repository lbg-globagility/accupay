using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccuPay.Utilities
{
    public class WSMService
    {
        private string ServerIpAddress { get; }
        private string ServiceName { get; }

        public WSMService(string serverIpAddress, string serviceName)
        {
            ServerIpAddress = serverIpAddress;
            ServiceName = serviceName;
        }

        public async Task<HttpResponseMessage> RestartService()
        {
            return await Post("RestartService");
        }

        public async Task<HttpResponseMessage> StopService()
        {
            return await Post("StopService");
        }

        public async Task<HttpResponseMessage> StartService()
        {
            return await Post("StartService");
        }

        public async Task<HttpResponseMessage> StartOrRestart()
        {
            return await Post("StartOrRestart");
        }

        public async Task<HttpResponseMessage> StopIfRunning()
        {
            return await Post("StopIfRunning");
        }

        private async Task<HttpResponseMessage> Post(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://{ServerIpAddress}:90/api/manager/{ServiceName}/");

                //var status = await clients.GetStringAsync("status");

                return await client.PostAsync(url, null);
            }
        }
    }
}