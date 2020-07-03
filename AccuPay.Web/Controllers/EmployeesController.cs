using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Files;
using AccuPay.Web.Employees.Models;
using AccuPay.Web.Employees.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        [Permission(PermissionTypes.EmployeeCreate)]
        public async Task<ActionResult<EmployeeDto>> Create([FromBody] CreateEmployeeDto dto)
        {
            return await _employeeService.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.EmployeeUpdate)]
        public async Task<ActionResult<EmployeeDto>> Update(int id, [FromBody] EmployeeDto dto)
        {
            return await _employeeService.Update(id, dto);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.EmployeeRead)]
        public async Task<ActionResult<EmployeeDto>> GeyById(int id)
        {
            return await _employeeService.GetById(id); ;
        }

        [HttpGet("{id}/image")]
        [AllowAnonymous]
        public async Task<ActionResult<Stream>> GetImage(int id, [FromServices] IFilesystem filesystem)
        {
            var path = await _employeeService.GetImagePathById(id);

            if (string.IsNullOrWhiteSpace(path))
            {
                return NotFound("Employee image not found.");
            }

            var stream = await filesystem.Get(path);

            return stream;
        }

        [HttpGet]
        [Permission(PermissionTypes.EmployeeRead)]
        public async Task<PaginatedList<EmployeeDto>> List([FromQuery] EmployeePageOptions options)
        {
            return await _employeeService.PaginatedList(options);
        }

        [HttpGet("employee-image")]
        [Permission(PermissionTypes.EmployeeUpdate)]
        public async Task<ActionResult> GenerateEmployeesImages()
        {
            await _employeeService.GenerateEmployeesImages();

            return Ok();
        }
    }
}
