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
    [ApiController]
    [Authorize]
    public class TimeLogsController : ControllerBase
    {
        private readonly TimeLogService _service;
        private readonly TimeLogEmailService _emailService;
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public TimeLogsController(TimeLogService service, TimeLogEmailService emailService, ITimeLogRepository timeLogRepository, IEmployeeRepository employeeRepository)
        {
            _service = service;
            _emailService = emailService;
            _timeLogRepository = timeLogRepository;
            _employeeRepository = employeeRepository;
        }

        [HttpPost]
        public async Task<TimeLogDto> CheckIn([FromBody] SelfServiceCreateTimeLogDto dto)
        {
            var timelog = await _service.CheckIn(dto);
            return timelog;
        }

        [HttpPut]
        public async Task<TimeLogDto> CheckOut([FromBody] SelfServiceCreateTimeLogDto dto)
        {
            var timelog = await _service.Checkout(dto);
            return timelog;
        }
        [HttpPut("lunch-in")]
        public async Task<TimeLogDto> LunchIn([FromBody] SelfServiceCreateTimeLogDto dto)
        {
            var timelog = await _service.LunchIn(dto);
            return timelog;
        }
        [HttpPut("lunch-out")]
        public async Task<TimeLogDto> LunchOut([FromBody] SelfServiceCreateTimeLogDto dto)
        {
            var timelog = await _service.LunchOut(dto);
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
        [HttpPut("filings")]
        public async Task<ActionResult> UpdateFiling([FromBody] UpdateEmployeeTimelogFilingDto dto)
        {
            var employee = await _employeeRepository.GetByEmployeeNumberAsync(dto.EmployeeNumber);
            if (employee == null) return NotFound();

            var filing = await _timeLogRepository.GetPendingFilingByEmployeeDateAndEntryTypeAsync(employee.RowID.Value, dto.LogDate, dto.EntryType);
            if (filing == null) return NotFound();
            if (filing.IsNotifyEmail)
                throw new Exception("Emailed filings can no longer be edited.");
            // Update allowed fields
            filing.LogDate = dto.LogDate;
            filing.Time = dto.Time.TimeOfDay;
            filing.Reason = dto.Reason;
            filing.DecidedBy = dto.DecidedBy;
            filing.LastUpdBy = SelfServiceUser.Id;

            await _timeLogRepository.UpdateFilingAsync(filing);

            return Ok();
        }
        [HttpDelete("filings")]
        public async Task<ActionResult> DeleteFiling([FromQuery] string employeeNumber, [FromQuery] DateTime date, [FromQuery] string entryType)
        {
            var employee = await _employeeRepository.GetByEmployeeNumberAsync(employeeNumber);
            if (employee == null) return NotFound();

            var filing = await _timeLogRepository.GetPendingFilingByEmployeeDateAndEntryTypeAsync(employee.RowID.Value, date, entryType);
            if (filing == null) return NotFound();
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
