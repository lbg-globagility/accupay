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
        private readonly PayrollResources _payrollResources;
        private readonly DbContextOptionsService _dbContextOptionsService;

        public PayrollController(PayrollResources resources, DbContextOptionsService dbContextOptionsService)
        {
            _payrollResources = resources;
            _dbContextOptionsService = dbContextOptionsService;
        }

        [HttpPost]
        public async Task<ActionResult> Start([FromBody] StartPayrollDto dto)
        {
            await _payrollResources.Load(1, 1, 241, dto.CutoffStart, dto.CutoffEnd);

            var employees = _payrollResources.Employees;
            foreach (var employee in employees)
            {
                var generation = new PayrollGeneration(_dbContextOptionsService);
                generation.DoProcess(employee, _payrollResources, 1, 1);
            }

            return BadRequest();
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
