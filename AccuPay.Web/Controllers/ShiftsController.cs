using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Shifts.Models;
using AccuPay.Web.Shifts.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
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
            var shift = await _repository.GetByIdAsync(id);

            if (shift == null) return NotFound();

            await _repository.DeleteAsync(id);

            return Ok();
        }

    }
}
