using AccuPay.Data.Helpers;
using AccuPay.Web.Shifts.Models;
using AccuPay.Web.Shifts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly ShiftService _service;

        public ShiftsController(ShiftService shiftService) => _service = shiftService;

        [HttpGet]
        public async Task<ActionResult<PaginatedList<ShiftDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShiftDto>> GetById(int id)
        {
            var shift = await _service.GetById(id);

            if (shift == null)
                return NotFound();
            else
                return shift;
        }

        [HttpPost]
        public async Task<ActionResult<ShiftDto>> Create([FromBody] CreateShiftDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ShiftDto>> Update(int id, [FromBody] UpdateShiftDto dto)
        {
            var shift = await _service.Update(id, dto);

            if (shift == null)
                return NotFound();
            else
                return shift;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var shift = await _service.GetById(id);

            if (shift == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpPost("import")]
        public async Task<ActionResult> Import([FromForm] IFormFile file)
        {
            await _service.Import(file);

            return Ok();
        }
    }
}
