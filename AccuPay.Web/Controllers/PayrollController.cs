using AccuPay.Data.Services;
using AccuPay.Web.Payroll;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        private readonly PayrollService _service;

        public PayrollController(PayrollService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<PayrollResultDto>> Start([FromServices] PayrollResources resources,
                                                                [FromBody] StartPayrollDto dto)
        {
            try
            {
                var result = await _service.Start(resources, dto);

                return result;
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{latest}")]
        public async Task<ActionResult> GetLatest()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById()
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<ActionResult> List()
        {
            throw new NotImplementedException();
        }
    }
}
