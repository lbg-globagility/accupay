using AccuPay.Core.Helpers;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers.SelfService
{
    [Route("api/self-service/[controller]")]
    [Authorize]
    [ApiController]
    public class TimeLogsController : ControllerBase
    {
        private readonly TimeLogService _service;

        public TimeLogsController(TimeLogService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<TimeLogDto> CheckIn([FromBody] SelfServiceCreateTimeLogDto dto)
        {
            var timelog = await _service.CheckIn(dto);
            return timelog;
        }

        [HttpPut("{id}")]
        public async Task<TimeLogDto> CheckOut(int id,[FromBody] SelfServiceCreateTimeLogDto dto)
        {
            var timelog = await _service.Checkout(id,dto);
            return timelog;
        }

        // New self-service paginated list for current employee
        [HttpGet("employee")]
        public async Task<ActionResult<PaginatedList<TimeLogDto>>> ListForCurrentEmployee([FromQuery] TimeLogsByEmployeePageOptions options)
        {
            var result = await _service.ListForCurrentEmployee(options);
            return result;
        }
    }
}
