using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly ICurrentUser _currentUser;

        public ReportsController(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet("sss-report/{month}/{year}")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult> GetPayslips(int month, int year)
        {
            UriBuilder uriBuilder = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttps,
                Host = "localhost",
                Port = 44379,
                Path = $"/api/reports/sss-report/{_currentUser.OrganizationId}/{month}/{year}"
            };

            var uri = new Uri(uriBuilder.ToString());

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                HttpContent content = response.Content;
                var contentStream = await content.ReadAsStreamAsync();
                return File(contentStream, "application/pdf", "payslip.pdf");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
