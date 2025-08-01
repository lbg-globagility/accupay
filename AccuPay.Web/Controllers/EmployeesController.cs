using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Files;
using AccuPay.Web.Employees.Models;
using AccuPay.Web.Employees.Services;
using AccuPay.Web.Positions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AccuPay.Core.Services.Imports.Employees.EmployeeImportParser;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ApiControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly PositionService _positionService;

        public EmployeesController(EmployeeService employeeService, IHostingEnvironment hostingEnvironment, PositionService positionService)
        {
            _employeeService = employeeService;
            _hostingEnvironment = hostingEnvironment;
            _positionService = positionService;
        }

        [HttpPost]
        [Permission(PermissionTypes.EmployeeCreate)]
        public async Task<ActionResult<EmployeeDto>> Create([FromBody] CreateEmployeeDto dto)
        {
            return await _employeeService.Create(dto);
        }

        [HttpGet("positions")]
        [Permission(PermissionTypes.EmployeeCreate)]
        public async Task<IEnumerable<PositionDto>> GetAllJobPosition([FromQuery] PageOptions options, string term)
        {
            options.All = true;

            var positionPaginatedList = await _positionService.PaginatedList(options, term);

            return positionPaginatedList.Items;
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.EmployeeUpdate)]
        public async Task<ActionResult<EmployeeDto>> Update(int id, [FromBody] UpdateEmployeeDto dto)
        {
            return await _employeeService.Update(id, dto);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.EmployeeRead)]
        public async Task<ActionResult<EmployeeDto>> GeyById(int id)
        {
            return await _employeeService.GetById(id);
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
            return await _employeeService.List(options);
        }

        [HttpGet("employee-image")]
        [Permission(PermissionTypes.EmployeeUpdate)]
        public async Task<ActionResult> GenerateEmployeesImages()
        {
            await _employeeService.GenerateEmployeesImages();

            return Ok();
        }

        [HttpGet("accupay-employeelist-template")]
        [Permission(PermissionTypes.EmployeeRead)]
        public ActionResult GetEmployeeTemplate()
        {
            return Excel(_hostingEnvironment.ContentRootPath + "/ImportTemplates", "accupay-employeelist-template.xlsx");
        }

        [HttpGet("employment-statuses")]
        [Permission(PermissionTypes.EmployeeRead)]
        public async Task<ActionResult<ICollection<string>>> GetEmploymentStatuses()
        {
            var employmentStatuses = await _employeeService.GetEmploymentStatuses();

            return employmentStatuses.ToList();
        }

        [HttpPost("import")]
        [Permission(PermissionTypes.EmployeeCreate)]
        public async Task<EmployeeImportParserOutput> Import([FromForm] IFormFile file)
        {
            return await _employeeService.Import(file);
        }
    }
}
