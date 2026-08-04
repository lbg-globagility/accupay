using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly ICurrentUser _currentUser;

        public TimeLogsController(TimeLogService service, TimeLogEmailService emailService, ITimeLogRepository timeLogRepository, ICurrentUser currentUser)
        {
            _service = service;
            _emailService = emailService;
            _timeLogRepository = timeLogRepository;
            _currentUser = currentUser;
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
        public async Task<ActionResult> SendFilingForApprovalEmail(int id)
        {
            var success = await _emailService.SendFilingForApprovalEmailAsync(id);
            if (!success) return NotFound();
            return Ok();
        }
        // NEW: Update filing endpoint
        [HttpPut("filings/{id}")]
        public async Task<ActionResult> UpdateFiling(int id, [FromBody] UpdateEmployeeTimelogFilingDto dto)
        {
            var filing = await _timeLogRepository.GetFilingByIdAsync(id);
            if (filing == null) return NotFound();
            if (filing.EmployeeID != _currentUser.EmployeeId) return NotFound();
            if (filing.Status != EmployeeTimelogFiling.StatusPending)
                throw new Exception("Only pending leave filings can be edited.");
            if (filing.IsNotifyEmail)
                throw new Exception("Emailed filings can no longer be edited.");
            // Update allowed fields
            filing.EntryType = dto.EntryType;
            filing.LogDate = dto.LogDate;
            filing.Time = dto.Time;
            filing.Reason = dto.Reason;
            filing.DecidedBy = dto.DecidedBy;
            
            await _timeLogRepository.UpdateFilingAsync(filing);

            return Ok();
        }
        [HttpDelete("filings/{id}")]
        public async Task<ActionResult> DeleteFiling(int id)
        {
            var filing = await _timeLogRepository.GetFilingByIdAsync(id);
            if (filing == null) return NotFound();
            if (filing.EmployeeID != _currentUser.EmployeeId) return NotFound();
            if (filing.Status != EmployeeTimelogFiling.StatusPending)
                throw new Exception("Only pending leave filings can be deleted.");
            if (filing.IsNotifyEmail)
                throw new Exception("Emailed filings can no longer be deleted.");

            await _timeLogRepository.DeleteFilingAsync(filing);

            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult<PaginatedList<EmployeeTimelogFilingDto>>> TimelogFilingList([FromQuery] TimeLogsByEmployeePageOptions options)
        {
            var result = await _service.ListFilingForCurrentEmployee(options);
            return result;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeTimelogFilingDto>> TimelogFilingList(int id)
        {
            return await _service.GetById(id);
        }
    }
}
