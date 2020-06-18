using AccuPay.Web.Calendars;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CalendarsController : ControllerBase
    {
        private readonly CalendarService _service;

        public CalendarsController(CalendarService service)
        {
            _service = service;
        }

        [HttpPost]
        [Permission(PermissionTypes.CalendarCreate)]
        public async Task<ActionResult<CalendarDto>> Create([FromBody] CreateCalendarDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.CalendarRead)]
        public async Task<ActionResult<CalendarDto>> GetById(int id)
        {
            return await _service.GetById(id);
        }

        [HttpGet("{id}/days")]
        [Permission(PermissionTypes.CalendarRead)]
        public async Task<ActionResult<ICollection<CalendarDayDto>>> GetDays(int id, int year)
        {
            var dtos = await _service.GetDays(id, year);
            return dtos.ToList();
        }

        [HttpPut("{id}/days")]
        [Permission(PermissionTypes.CalendarUpdate)]
        public async Task<ActionResult> UpdateDays(int id, [FromBody] ICollection<CalendarDayDto> dtos2)
        {
            await _service.UpdateDays(id, dtos2);

            return Ok();
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.CalendarUpdate)]
        public async Task<ActionResult<CalendarDto>> Update(int id, [FromBody] UpdateCalendarDto dto)
        {
            return await _service.Update(id, dto);
        }

        [HttpGet]
        [Permission(PermissionTypes.CalendarRead)]
        public async Task<ActionResult<ICollection<CalendarDto>>> List()
        {
            var dtos = await _service.List();
            return dtos.ToList();
        }

        [HttpGet("day-types")]
        [Permission(PermissionTypes.CalendarRead)]
        public async Task<ActionResult<ICollection<DayTypeDto>>> DayTypes()
        {
            var dtos = await _service.GetDayTypes();
            return dtos.ToList();
        }
    }
}
