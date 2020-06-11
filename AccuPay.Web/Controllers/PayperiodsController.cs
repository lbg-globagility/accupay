using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Payroll;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayperiodsController : ControllerBase
    {
        private readonly PayperiodService _payperiodService;
        private readonly PaystubService _paystubService;

        public PayperiodsController(PayperiodService payperiodService, PaystubService paystubService)
        {
            _payperiodService = payperiodService;
            _paystubService = paystubService;
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

        [HttpGet("{id}/paystubs")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<ICollection<PaystubDto>>> GetPaystubs(int id)
        {
            var dtos = await _paystubService.GetByPayperiod(id);
            return dtos.ToList();
        }

        [HttpGet]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult<PaginatedList<PayperiodDto>>> List([FromQuery] PageOptions options)
        {
            return await _payperiodService.List(options);
        }
    }
}
