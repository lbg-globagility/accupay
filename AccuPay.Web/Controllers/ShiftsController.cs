using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Shifts.Models;
using AccuPay.Web.Shifts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShiftsController : ControllerBase
    {
        private readonly ShiftService _service;

        public ShiftsController(ShiftService shiftService) => _service = shiftService;

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

        [HttpPut("{id}")]

        [Permission(PermissionTypes.ShiftUpdate)]
        public async Task<ActionResult<ShiftDto>> Update(int id, [FromBody] UpdateShiftDto dto)
        {
            var shift = await _service.Update(id, dto);

            if (shift == null)
                return NotFound();
            else
                return shift;
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
    }
}
