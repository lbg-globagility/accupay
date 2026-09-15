using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Overtimes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers.SelfService
{
    [Route("api/self-service/[controller]")]
    [ApiController]
    public class OvertimesController : ControllerBase
    {
        private readonly OvertimeService _overtimeService;
        private readonly ICurrentUser _currentUser;
        private readonly OvertimeEmailService _emailService;

        public OvertimesController(
            OvertimeService overtimeService,
            ICurrentUser currentUser,
            OvertimeEmailService emailService)
        {
            _overtimeService = overtimeService;
            _currentUser = currentUser;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<PaginatedList<OvertimeDto>> List([FromQuery] OvertimePageOptions options)
        {
            options.EmployeeId = _currentUser.EmployeeId;
            var overtimes = await _overtimeService.PaginatedList(options);

            return overtimes;
        }

        [HttpPost]
        public async Task<OvertimeDto> Create([FromBody] SelfServiceCreateOvertimeDto dto)
        {
            var overtime = await _overtimeService.Create(dto);

            return overtime;
        }

        [HttpPut]
        public async Task<ActionResult<OvertimeDto>> Update([FromBody] SelfServiceUpdateOvertimeDto dto)
        {
            var overtime = await _overtimeService.UpdateSelfService(dto);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery] string employeeNumber, [FromQuery] DateTime date)
        {
            var deleted = await _overtimeService.DeleteSelfService(employeeNumber, date);

            if (!deleted) return NotFound();

            return Ok();
        }

        [HttpPost("filings/{id}/send-approval-email")]
        public async Task<ActionResult> SendFilingForApprovalEmail(int id)
        {
            var success = await _emailService.SendFilingForApprovalEmailAsync(id);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OvertimeDto>> GetById(int id)
        {
            var overtime = await _overtimeService.GetById(id);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }
    }
}
