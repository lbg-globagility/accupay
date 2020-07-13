using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Shifts.Models;
using AccuPay.Web.Shifts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShiftsController : ApiControllerBase
    {
        private readonly ShiftService _service;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ShiftsController(ShiftService shiftService, IHostingEnvironment hostingEnvironment)
        {
            _service = shiftService;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        [Permission(PermissionTypes.ShiftRead)]
        public async Task<ActionResult<PaginatedList<ShiftDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.ShiftRead)]
        public async Task<ActionResult<ShiftDto>> GetById(int id)
        {
            var shift = await _service.GetById(id);

            if (shift == null)
                return NotFound();
            else
                return shift;
        }

        [HttpPost]
        [Permission(PermissionTypes.ShiftCreate)]
        public async Task<ActionResult<ShiftDto>> Create([FromBody] CreateShiftDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut]
        [Permission(PermissionTypes.ShiftUpdate)]
        public async Task<ActionResult<ShiftDto>> Update([FromBody] ICollection<ShiftDto> dtos)
        {
            await _service.BatchApply(dtos);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.ShiftDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var shift = await _service.GetById(id);

            if (shift == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpPost("import")]
        [Permission(PermissionTypes.ShiftCreate)]
        public async Task<ActionResult> Import([FromForm] IFormFile file)
        {
            await _service.Import(file);

            return Ok();
        }

        [HttpGet("accupay-shiftschedule-template")]
        [Permission(PermissionTypes.SalaryRead)]
        public ActionResult GetEmployeeTemplate()
        {
            return Excel(_hostingEnvironment.ContentRootPath + "/ImportTemplates", "accupay-shiftschedule-template.xlsx");
        }

        [HttpGet("employees")]
        [Permission(PermissionTypes.ShiftRead)]
        public async Task<ActionResult<PaginatedList<EmployeeShiftsDto>>> ListByEmployee(
            [FromQuery] ShiftsByEmployeePageOptions options)
        {
            return await _service.ListByEmployee(options);
        }
    }
}
