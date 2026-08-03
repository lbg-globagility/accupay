using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Overtimes;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeLogsController : ControllerBase
    {
        private readonly TimeLogService _service;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeApproverRepository _employeeApproverRepository;

        public TimeLogsController(TimeLogService service, IConfiguration configuration, IEmployeeApproverRepository employeeApproverRepository)
        {
            _service = service;
            _configuration = configuration;
            _employeeApproverRepository = employeeApproverRepository;
        }

        [HttpGet("employees")]
        [Permission(PermissionTypes.TimeLogRead)]
        public async Task<ActionResult<PaginatedList<EmployeeTimeLogsDto>>> ListByEmployee(
            [FromQuery] TimeLogsByEmployeePageOptions options)
        {
            return await _service.ListByEmployee(options);
        }

        [HttpPost]
        [Permission(PermissionTypes.TimeLogUpdate)]
        public async Task<ActionResult> Update([FromBody] ICollection<UpdateTimeLogDto> dtos)
        {
            await _service.BatchApply(dtos);

            return Ok();
        }

        [HttpPost("import")]
        [Permission(PermissionTypes.TimeLogCreate)]
        public async Task<ActionResult<TimeLogImportResultDto>> Import([FromForm] IFormFile file)
        {
            var result = await _service.Import(file);

            if (result == null)
                return NotFound();
            else
                return result;
        }

        [HttpPost("filings/{id}/approve")]
        [Permission(PermissionTypes.TimeLogUpdate)]
        public async Task<ActionResult<TimeLogDto>> ApproveFiling(int id)
        {
            var dto = await _service.ApproveFiling(id);
            return dto;
        }

        // Token-verified GET that approves and returns a simple HTML page (for email link)
        [HttpGet("filings/{id}/approve")]
        [AllowAnonymous]
        public async Task<IActionResult> ApproveFilingWithToken(int id, [FromQuery] string token)
        {
            var secret = _configuration["App:ApprovalTokenSecret"] ?? string.Empty;
            if (!ApprovalTokenHelper.ValidateToken(token, id, secret, out var error))
            {
                var errHtml = $"<html><body><h3>Approval failed</h3><p>{System.Net.WebUtility.HtmlEncode(error)}</p></body></html>";
                return Content(errHtml, "text/html");
            }

            try
            {
                await _service.ApproveFiling(id);
                var okHtml = "<html><body><h3>Filing Approved</h3><p>The timelog filing was successfully approved.</p></body></html>";
                return Content(okHtml, "text/html");
            }
            catch (System.Exception ex)
            {
                var errHtml = $"<html><body><h3>Approval failed</h3><p>{System.Net.WebUtility.HtmlEncode(ex.Message)}</p></body></html>";
                return Content(errHtml, "text/html");
            }
        }

        [HttpPost("filings/{id}/reject")]
        [Permission(PermissionTypes.TimeLogUpdate)]
        public async Task<ActionResult> RejectFiling(int id)
        {
            await _service.RejectFiling(id);
            return Ok();
        }

        // Token-verified GET that rejects and returns a simple HTML page (for email link)
        [HttpGet("filings/{id}/reject")]
        [AllowAnonymous]
        public async Task<IActionResult> RejectFilingWithToken(int id, [FromQuery] string token)
        {
            var secret = _configuration["App:ApprovalTokenSecret"] ?? string.Empty;
            if (!ApprovalTokenHelper.ValidateToken(token, id, secret, out var error))
            {
                var errHtml = $"<html><body><h3>Rejection failed</h3><p>{System.Net.WebUtility.HtmlEncode(error)}</p></body></html>";
                return Content(errHtml, "text/html");
            }

            try
            {
                await _service.RejectFiling(id);
                var okHtml = "<html><body><h3>Filing Rejected</h3><p>The timelog filing was successfully rejected.</p></body></html>";
                return Content(okHtml, "text/html");
            }
            catch (System.Exception ex)
            {
                var errHtml = $"<html><body><h3>Rejection failed</h3><p>{System.Net.WebUtility.HtmlEncode(ex.Message)}</p></body></html>";
                return Content(errHtml, "text/html");
            }
        }
        [HttpGet("employee")]
        public async Task<ActionResult<PaginatedList<TimeLogDto>>> ListForCurrentEmployee([FromQuery] TimeLogsByEmployeePageOptions options)
        {
            var result = await _service.ListForCurrentEmployee(options);
            return result;
        }
        [HttpGet("employee/{employeeId}/pending")]
        [AllowAnonymous]
        public async Task<ActionResult<List<EmployeeTimelogFilingDto>>> GetPendingByEmployee(int employeeId, [FromQuery] int employeeApproverId, [FromQuery] string token)
        {
            var secret = _configuration["App:ApprovalTokenSecret"] ?? string.Empty;
            if (!ApprovalTokenHelper.ValidateToken(token, employeeApproverId, secret, out var error, out var approverEmail))
                return BadRequest(error);

            var employeeApprover = await _employeeApproverRepository.GetByIdAsync(employeeApproverId);
            if (employeeApprover == null || employeeApprover.EmployeeID != employeeId)
                return NotFound();

            if (!string.Equals(employeeApprover.Approver?.EmailAddress, approverEmail, System.StringComparison.OrdinalIgnoreCase))
                return BadRequest("Token does not match approver.");
            return await _service.GetPendingByEmployee(employeeId);
        }
    }
}
