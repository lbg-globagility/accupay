using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Leaves;
using AccuPay.Web.Shifts.Models;
using AccuPay.Web.Shifts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers.SelfService
{
    [Route("api/self-service/[controller]")]
    [ApiController]
    [Authorize]
    public class ShiftsController : ControllerBase
    {
        private readonly ICurrentUser _currentUser;
        private readonly ShiftService _shiftService;
        public ShiftsController(ICurrentUser currentUser, ShiftService shiftService)
        {
            _currentUser= currentUser;
            _shiftService= shiftService;
        }
        [HttpGet]
        public async Task<PaginatedList<EmployeeShiftsDto>> List([FromQuery] ShiftsByEmployeePageOptions options)
        {
            options.EmployeeId = _currentUser.EmployeeId;
            var dtos = await _shiftService.ListByEmployee(options);

            return dtos;
        }

        [HttpGet("today")]
        public async Task<ActionResult<EmployeeDutyScheduleDto>> GetToday()
        {
            var dto = await _shiftService.GetTodayShift();

            if (dto == null)
                return NotFound();

            return dto;
        }

        [HttpPost]
        public async Task<ActionResult<List<EmployeeDutyScheduleDto>>> Create([FromBody] SelfServiceCreateShiftDto dto)
        {
            return await _shiftService.CreateRange(dto);
        }
    }
}
