using AccuPay.Utilities;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    public class ReportsControllerBase : ControllerBase
    {
        private const int DefaultPort = 80;
        private const string BaseApi = "api/reports/";

        protected static readonly HttpClient _httpClient = new HttpClient();
        protected readonly ICurrentUser _currentUser;
        protected readonly IConfiguration _configuration;

        public ReportsControllerBase(ICurrentUser currentUser, IConfiguration configuration)
        {
            _currentUser = currentUser;
            _configuration = configuration;
        }

        public async Task<ActionResult> GetPDF(string path, string fileName)
        {
            var port = ObjectUtils.ToNullableInteger(_configuration["ReportsAPI:Port"]);

            UriBuilder uriBuilder = new UriBuilder
            {
                Scheme = _configuration["ReportsAPI:Scheme"],
                Host = _configuration["ReportsAPI:Host"],
                Port = port ?? DefaultPort,
            };

            uriBuilder.Path = BaseApi + path;

            var uri = new Uri(uriBuilder.ToString());

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                HttpContent content = response.Content;
                var contentStream = await content.ReadAsStreamAsync();
                return File(contentStream, "application/pdf", fileName);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
