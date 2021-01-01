using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeLogsController : ControllerBase
    {
        private readonly TimeLogService _service;

        public TimeLogsController(TimeLogService service)
        {
            _service = service;
        }

        [HttpGet("employees")]
        [Permission(PermissionTypes.TimeLogRead)]
        public async Task<ActionResult<PaginatedList<EmployeeTimeLogsDto>>> ListByEmployee(
            [FromQuery] TimeLogsByEmployeePageOptions options)
        {
            return await _service.ListByEmployee(options);
        }

        [HttpPost]
        [Permission(PermissionTypes.TimeLogUpdate)]
        public async Task<ActionResult> Update([FromBody] ICollection<UpdateTimeLogDto> dtos)
        {
            await _service.BatchApply(dtos);

            return Ok();
        }

        [HttpPost("import")]
        [Permission(PermissionTypes.TimeLogCreate)]
        public async Task<ActionResult<TimeLogImportResultDto>> Import([FromForm] IFormFile file)
        {
            var result = await _service.Import(file);

            if (result == null)
                return NotFound();
            else
                return result;
        }
    }
}
