using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Salaries.Models;
using AccuPay.Web.Salaries.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalariesController : ApiControllerBase
    {
        private readonly SalaryService _service;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SalariesController(SalaryService salaryService, IHostingEnvironment hostingEnvironment)
        {
            _service = salaryService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Permission(PermissionTypes.SalaryRead)]
        public async Task<ActionResult<PaginatedList<SalaryDto>>> List([FromQuery] PageOptions options, string term, int employeeId)
        {
            return await _service.List(options, term, employeeId);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.SalaryRead)]
        public async Task<ActionResult<SalaryDto>> GetById(int id)
        {
            var allowance = await _service.GetById(id);

            if (allowance == null)
                return NotFound();
            else
                return allowance;
        }

        [HttpGet("latest")]
        [Permission(PermissionTypes.SalaryRead)]
        public async Task<ActionResult<SalaryDto>> GetLatest(int employeeId)
        {
            var salary = await _service.GetLatest(employeeId);

            if (salary is null)
                return NotFound();
            else
                return salary;
        }

        [HttpPost]
        [Permission(PermissionTypes.SalaryCreate)]
        public async Task<ActionResult<SalaryDto>> Create([FromBody] CreateSalaryDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.SalaryUpdate)]
        public async Task<ActionResult<SalaryDto>> Update(int id, [FromBody] UpdateSalaryDto dto)
        {
            var allowance = await _service.Update(id, dto);

            if (allowance == null)
                return NotFound();
            else
                return allowance;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.SalaryDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var allowance = await _service.GetById(id);

            if (allowance == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("accupay-salary-template")]
        [Permission(PermissionTypes.SalaryRead)]
        public ActionResult GetEmployeeTemplate()
        {
            return Excel(_hostingEnvironment.ContentRootPath + "/ImportTemplates", "accupay-salary-template.xlsx");
        }
    }
}
