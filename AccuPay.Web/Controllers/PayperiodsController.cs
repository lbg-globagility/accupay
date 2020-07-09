using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Payroll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PayperiodsController : ReportsControllerBase
    {
        private readonly PayperiodService _payperiodService;
        private readonly PaystubService _paystubService;
        private readonly PayPeriodRepository _payPeriodRepository;

        public PayperiodsController(
            PayperiodService payperiodService,
            PaystubService paystubService,
            PayPeriodRepository payPeriodRepository,
            ICurrentUser currentUser,
            IConfiguration configuration) : base(currentUser, configuration)
        {
            _payperiodService = payperiodService;
            _paystubService = paystubService;
            _payPeriodRepository = payPeriodRepository;
        }

        [HttpGet]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<PaginatedList<PayPeriodDto>>> List([FromQuery] PageOptions options)
        {
            return await _payperiodService.List(options);
        }

        [HttpGet("year/{year}")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<ICollection<PayPeriodDto>>> GetYearlyPayPeriods(int year)
        {
            return await _payperiodService.GetYearlyPayPeriods(year);
        }

        [HttpPost]
        [Permission(PermissionTypes.PayPeriodUpdate)]
        public async Task<PayPeriodDto> Start([FromBody] StartPayrollDto dto)
        {
            return await _payperiodService.Start(dto);
        }

        [HttpPost("{id}/calculate")]
        [Permission(PermissionTypes.PayPeriodUpdate)]
        public async Task<ActionResult<PayrollResultDto>> Calculate(
            [FromServices] PayrollResources resources,
            int id)
        {
            try
            {
                if ((await _payPeriodRepository.GetByIdAsync(id)) == null)
                {
                    return NotFound();
                }

                var result = await _payperiodService.Calculate(resources, id);

                return result;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("{id}/close")]
        [Permission(PermissionTypes.PayPeriodUpdate)]
        public async Task<ActionResult> Close(int id)
        {
            if ((await _payPeriodRepository.GetByIdAsync(id)) == null)
            {
                return NotFound();
            }
            await _payperiodService.Close(id);

            return Ok();
        }

        [HttpGet("latest")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<PayPeriodDto>> GetLatest()
        {
            return await _payperiodService.GetLatest();
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<PayPeriodDto>> GetById(int id)
        {
            return await _payperiodService.GetById(id);
        }

        [HttpGet("{payperiodId}/paystubs")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<ICollection<PaystubDto>>> GetPaystubs(int payperiodId)
        {
            var paystubs = await _paystubService.GetAll(payperiodId);
            return paystubs.ToList();
        }

        [HttpGet("{payperiodId}/payslips")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult> GetPayslips(int payperiodId)
        {
            var path = $"payslip/{payperiodId}";
            return await GetPDF(path, "payslip.pdf");
        }
    }
}
