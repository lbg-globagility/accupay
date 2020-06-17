using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet]
        [Permission(PermissionTypes.TimeLogRead)]
        public async Task<ActionResult<PaginatedList<TimeLogDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("employees")]
        [Permission(PermissionTypes.TimeLogRead)]
        public async Task<ActionResult<PaginatedList<EmployeeTimeLogsDto>>> ListByEmployee(
            [FromQuery] PageOptions options,
            DateTime dateFrom,
            DateTime dateTo,
            string searchTerm)
        {
            return await _service.ListByEmployee(options, dateFrom, dateTo, searchTerm);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.TimeLogRead)]
        public async Task<ActionResult<TimeLogDto>> GetById(int id)
        {
            var timelog = await _service.GetByIdWithEmployeeAsync(id);

            if (timelog == null)
                return NotFound();
            else
                return timelog;
        }

        //[HttpPost]
        //[Permission(PermissionTypes.TimeLogCreate)]
        //public async Task<ActionResult<TimeLogDto>> Create([FromBody] CreateTimeLogDto dto)
        //{
        //    return await _service.Create(dto);
        //}

        [HttpPost]
        [Permission(PermissionTypes.TimeLogUpdate)]
        public async Task<ActionResult> Update([FromBody] ICollection<UpdateTimeLogDto> dtos)
        {
            await _service.Update(dtos);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.TimeLogDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var timelog = await _service.GetByIdAsync(id);

            if (timelog == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpPost("import")]
        [Permission(PermissionTypes.TimeLogCreate)]
        public async Task<ActionResult> Import([FromForm] IFormFile file)
        {
            await _service.Import(file);

            return Ok();
        }
    }
}
