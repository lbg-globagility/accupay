using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeLogsController : ControllerBase
    {
        private readonly TimeLogService _service;

        public TimeLogsController(TimeLogService service)
        {
            _service = service;
        }

        [HttpGet]
        [Permission(PermissionTypes.EmployeeTimeLogRead)]
        public async Task<ActionResult<PaginatedList<TimeLogDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.EmployeeTimeLogRead)]
        public async Task<ActionResult<TimeLogDto>> GetById(int id)
        {
            var timelog = await _service.GetByIdWithEmployeeAsync(id);

            if (timelog == null)
                return NotFound();
            else
                return timelog;
        }

        [HttpPost]
        [Permission(PermissionTypes.EmployeeTimeLogCreate)]
        public async Task<ActionResult<TimeLogDto>> Create([FromBody] CreateTimeLogDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.EmployeeTimeLogUpdate)]
        public async Task<ActionResult<TimeLogDto>> Update(int id, [FromBody] UpdateTimeLogDto dto)
        {
            var timeLog = await _service.Update(id, dto);

            if (timeLog == null)
                return NotFound();
            else
                return timeLog;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.EmployeeTimeLogDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var timelog = await _service.GetByIdAsync(id);

            if (timelog == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpPost("import")]
        [Permission(PermissionTypes.EmployeeTimeLogCreate)]
        public async Task<ActionResult> Import([FromForm] IFormFile file)
        {
            await _service.Import(file);

            return Ok();
        }
    }
}
