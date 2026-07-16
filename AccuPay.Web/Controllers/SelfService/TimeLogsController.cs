using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers.SelfService
{
    [Route("api/self-service/[controller]")]
    [Authorize]
    [ApiController]
    public class TimeLogsController : ControllerBase
    {
        private readonly TimeLogService _service;
        private readonly TimeLogEmailService _emailService;

        public TimeLogsController(TimeLogService service, TimeLogEmailService emailService)
        {
            _service = service;
            _emailService = emailService;
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

        [HttpPost("filings")]
        public async Task<ActionResult> CreateFiling([FromBody] CreateEmployeeTimelogFilingDto dto)
        {
            var filing = await _service.CreateFiling(dto);
            return Ok(new { Id = filing.RowID, Status = filing.Status });
        }
        [HttpPost("filings/{id}/send-approval-email")]
        [Permission(PermissionTypes.TimeLogUpdate)]
        public async Task<ActionResult> SendFilingForApprovalEmail(int id)
        {
            var success = await _emailService.SendFilingForApprovalEmailAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
    }
}
