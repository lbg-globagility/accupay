using AccuPay.Core.Helpers;
using AccuPay.Core.Services.Imports.Overtimes;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Dto;
using AccuPay.Web.Overtimes;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class OvertimesController : ApiControllerBase
    {
        private readonly OvertimeService _service;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public OvertimesController(
            OvertimeService service,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        [HttpPost("filings/{id}/approve")]
        [Permission(PermissionTypes.OvertimeUpdate)]
        public async Task<ActionResult<OvertimeDto>> ApproveFiling(int id, [FromBody] ApproveFilingDto dto)
        {
            return await _service.ApproveFiling(id, dto?.DecidedBy);
        }

        [HttpGet("filings/{id}/approve")]
        [AllowAnonymous]
        public async Task<IActionResult> ApproveFilingWithToken(int id, [FromQuery] string token)
        {
            return await ChangeFilingStatusWithToken(id, token, true);
        }

        [HttpPost("filings/{id}/reject")]
        [Permission(PermissionTypes.OvertimeUpdate)]
        public async Task<ActionResult<OvertimeDto>> RejectFiling(int id, [FromBody] RejectFilingDto dto)
        {
            return await _service.RejectFiling(id, dto?.DecidedBy);
        }

        [HttpGet("filings/{id}/reject")]
        [AllowAnonymous]
        public async Task<IActionResult> RejectFilingWithToken(int id, [FromQuery] string token)
        {
            return await ChangeFilingStatusWithToken(id, token, false);
        }

        private async Task<IActionResult> ChangeFilingStatusWithToken(int id, string token, bool approve)
        {
            var action = approve ? "Approval" : "Rejection";
            var secret = _configuration["App:ApprovalTokenSecret"] ?? string.Empty;
            if (!ApprovalTokenHelper.ValidateToken(token, id, secret, out var error, out var decidedBy))
                return HtmlResult($"{action} failed", error);

            try
            {
                if (approve) await _service.ApproveFiling(id, decidedBy);
                else await _service.RejectFiling(id, decidedBy);
                return HtmlResult($"Overtime Filing {(approve ? "Approved" : "Rejected")}",
                    $"The overtime filing was successfully {(approve ? "approved" : "rejected")}.");
            }
            catch (System.Exception ex)
            {
                return HtmlResult($"{action} failed", ex.Message);
            }
        }

        private ContentResult HtmlResult(string heading, string message)
        {
            var html = $"<html><body><h3>{System.Net.WebUtility.HtmlEncode(heading)}</h3><p>{System.Net.WebUtility.HtmlEncode(message)}</p></body></html>";
            return Content(html, "text/html");
        }

        [HttpGet]
        [Permission(PermissionTypes.OvertimeRead)]
        public async Task<ActionResult<PaginatedList<OvertimeDto>>> List([FromQuery] OvertimePageOptions options)
        {
            return await _service.PaginatedList(options);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.OvertimeRead)]
        public async Task<ActionResult<OvertimeDto>> GetById(int id)
        {
            var overtime = await _service.GetById(id);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }

        [HttpPost]
        [Permission(PermissionTypes.OvertimeCreate)]
        public async Task<ActionResult<OvertimeDto>> Create([FromBody] CreateOvertimeDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.OvertimeUpdate)]
        public async Task<ActionResult<OvertimeDto>> Update(int id, [FromBody] UpdateOvertimeDto dto)
        {
            var overtime = await _service.Update(id, dto);

            if (overtime == null)
                return NotFound();
            else
                return overtime;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.OvertimeDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var overtime = await _service.GetById(id);

            if (overtime == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("statuslist")]
        [Permission(PermissionTypes.OvertimeRead)]
        public ActionResult<ICollection<string>> GetStatusList()
        {
            return _service.GetStatusList();
        }

        [HttpGet("accupay-overtime-template")]
        [Permission(PermissionTypes.OvertimeRead)]
        public ActionResult GetOvertimeTemplate()
        {
            return Excel(_hostingEnvironment.ContentRootPath + "/ImportTemplates", "accupay-overtime-template.xlsx");
        }

        [HttpPost("import")]
        [Permission(PermissionTypes.OvertimeCreate)]
        public async Task<OvertimeImportParserOutput> Import([FromForm] IFormFile file)
        {
            return await _service.Import(file);
        }
    }
}
