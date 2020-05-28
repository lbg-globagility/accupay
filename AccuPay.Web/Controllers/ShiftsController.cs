using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Shifts.Models;
using AccuPay.Web.Shifts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly ShiftService _service;
        private readonly EmployeeDutyScheduleRepository _repository;

        public ShiftsController(ShiftService shiftService, EmployeeDutyScheduleRepository repository)
        {
            _service = shiftService;
            _repository = repository;
        }


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

        [HttpPost("import")]
        public async Task<ActionResult> Import([FromForm] ImportShiftDto dto)
        {
            await _service.Import(dto);

            return Ok();
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
            var shift = await _repository.GetByIdAsync(id);

            if (shift == null) return NotFound();

            await _repository.DeleteAsync(id);

            return Ok();
        }

    }
}
