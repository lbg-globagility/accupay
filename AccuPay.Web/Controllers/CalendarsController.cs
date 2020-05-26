using AccuPay.Web.Calendars;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarsController : ControllerBase
    {
        private readonly CalendarService _service;

        public CalendarsController(CalendarService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<CalendarDto>> Create([FromBody] CreateCalendarDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarDto>> GetById(int id)
        {
            return await _service.GetById(id);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CalendarDto>> Update(int id, [FromBody] UpdateCalendarDto dto)
        {
            return await _service.Update(id, dto);
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<CalendarDto>>> List()
        {
            var dtos = await _service.List();
            return dtos.ToList();
        }
    }
}
