using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Employees.Models;
using AccuPay.Web.Employees.Services;
using AccuPay.Web.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeUsersController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeUsersController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("employees")]
        [Permission(PermissionTypes.UserCreate)]
        public async Task<ActionResult<PaginatedList<EmployeeDto>>> GetUnregisteredEmployeeAsync([FromQuery] EmployeePageOptions options)
        {
            var result = await _employeeService.GetUnregisteredEmployeeAsync(options, options.SearchTerm);
            return Ok(result);
        }
    }
}
