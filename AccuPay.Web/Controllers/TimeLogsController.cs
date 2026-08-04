using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Dto;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        public TimeLogsController(TimeLogService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
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
        public async Task<ActionResult<TimeLogDto>> ApproveFiling(int id, [FromBody] ApproveFilingDto dto)
        {
            var result = await _service.ApproveFiling(id, dto?.DecidedBy);
            return result;
        }

        // Token-verified GET that approves and returns a simple HTML page (for email link)
        [HttpGet("filings/{id}/approve")]
        [AllowAnonymous]
        public async Task<IActionResult> ApproveFilingWithToken(int id, [FromQuery] string token)
        {
            var secret = _configuration["App:ApprovalTokenSecret"] ?? string.Empty;
            if (!ApprovalTokenHelper.ValidateToken(token, id, secret, out var error, out var decidedBy))
            {
                var errHtml = $"<html><body><h3>Approval failed</h3><p>{System.Net.WebUtility.HtmlEncode(error)}</p></body></html>";
                return Content(errHtml, "text/html");
            }

            try
            {
                await _service.ApproveFiling(id, decidedBy);
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
        public async Task<ActionResult> RejectFiling(int id, [FromBody] RejectFilingDto dto)
        {
            await _service.RejectFiling(id, dto?.DecidedBy);
            return Ok();
        }

        // Token-verified GET that rejects and returns a simple HTML page (for email link)
        [HttpGet("filings/{id}/reject")]
        [AllowAnonymous]
        public async Task<IActionResult> RejectFilingWithToken(int id, [FromQuery] string token)
        {
            var secret = _configuration["App:ApprovalTokenSecret"] ?? string.Empty;
            if (!ApprovalTokenHelper.ValidateToken(token, id, secret, out var error, out var decidedBy))
            {
                var errHtml = $"<html><body><h3>Rejection failed</h3><p>{System.Net.WebUtility.HtmlEncode(error)}</p></body></html>";
                return Content(errHtml, "text/html");
            }

            try
            {
                await _service.RejectFiling(id, decidedBy);
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
    }
}
