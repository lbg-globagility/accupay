using AccuPay.Data.Repositories;
using AccuPay.Web.Employees.Models;
using AccuPay.Web.Employees.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] EmployeeDto dto)
        {
            try
            {
                var employee = await _employeeService.Create(dto);
                return Ok(employee.RowID);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] EmployeeDto dto)
        {
            try
            {
                await _employeeService.Update(id, dto);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GeyById(int id)
        {
            var dto = await _employeeService.GeyByIdAsync(id);

            return dto;
        }
    }
}
