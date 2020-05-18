using Accupay.Web.Core.Auth;
using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Extensions;
using AccuPay.Web.Employees.Models;
using AccuPay.Web.Employees.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly CurrentUser _currentUser;

        public EmployeesController(EmployeeService employeeService, CurrentUser currentUser)
        {
            _employeeService = employeeService;
            _currentUser = currentUser;
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

        [HttpGet]
        public async Task<PaginatedList<EmployeeDto>> List([FromQuery] PageOptions options, string term = "")
        {
            int variableOrganizationId = 5;//_currentUser.OrganizationId
            var query = await _employeeService.GetAllAsync(variableOrganizationId);

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(s => s.FullNameWithMiddleInitialLastNameFirst.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            query = options.Sort switch
            {
                "name" => query.OrderBy(t => t.FullNameWithMiddleInitialLastNameFirst, options.Direction),
                _ => query.OrderBy(t => t.FullNameWithMiddleInitialLastNameFirst)
            };

            var roles = query.Page(options).ToList();
            var count = query.Count();

            var dtos = roles.Select(s => EmployeeDto.Convert(s)).ToList();

            return new PaginatedList<EmployeeDto>(dtos, count, options.PageIndex, options.PageSize);
        }
    }
}
