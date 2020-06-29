using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Payroll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PayperiodsController : ControllerBase
    {
        private readonly PayperiodService _payperiodService;
        private readonly PaystubService _paystubService;

        private static readonly HttpClient _httpClient = new HttpClient();

        public PayperiodsController(PayperiodService payperiodService, PaystubService paystubService)
        {
            _payperiodService = payperiodService;
            _paystubService = paystubService;
        }

        [HttpGet]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<PaginatedList<PayperiodDto>>> List([FromQuery] PageOptions options)
        {
            return await _payperiodService.List(options);
        }

        [HttpPost]
        [Permission(PermissionTypes.PayPeriodUpdate)]
        public async Task<ActionResult<PayperiodDto>> Start([FromBody] StartPayrollDto dto)
        {
            try
            {
                var result = await _payperiodService.Start(dto);

                return result;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("{id}/calculate")]
        [Permission(PermissionTypes.PayPeriodUpdate)]
        public async Task<ActionResult<PayrollResultDto>> Calculate([FromServices] PayrollResources resources,
                                                                    int id)
        {
            try
            {
                var result = await _payperiodService.Calculate(resources, id);

                return result;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("latest")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<PayperiodDto>> GetLatest()
        {
            return await _payperiodService.GetLatest();
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<PayperiodDto>> GetById(int id)
        {
            return await _payperiodService.GetById(id);
        }

        [HttpGet("{payperiodId}/paystubs")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<PaginatedList<PaystubDto>>> GetPaystubs(int payperiodId, [FromQuery] PageOptions options, string searchTerm)
        {
            return await _paystubService.PaginatedList(options, payperiodId);
        }

        [HttpGet("{payperiodId}/payslips")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult> GetPayslips(int payperiodId)
        {
            UriBuilder uriBuilder = new UriBuilder
            {
                Scheme = Uri.UriSchemeHttps,
                Host = "localhost",
                Port = 44379,
                Path = $"/api/reports/payslip/{payperiodId}"
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
