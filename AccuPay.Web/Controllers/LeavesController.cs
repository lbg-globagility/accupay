using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Leaves;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeavesController : ApiControllerBase
    {
        private readonly LeaveService _service;
        private readonly ILeaveRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public LeavesController(
            LeaveService leaveService,
            ILeaveRepository repository,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _service = leaveService;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        [HttpPost("filings/{id}/approve")]
        [Permission(PermissionTypes.LeaveUpdate)]
        public async Task<ActionResult<LeaveDto>> ApproveFiling(int id)
        {
            return await _service.ApproveFiling(id);
        }

        [HttpGet("filings/{id}/approve")]
        [AllowAnonymous]
        public async Task<IActionResult> ApproveFilingWithToken(int id, [FromQuery] string token)
        {
            return await ChangeFilingStatusWithToken(id, token, true);
        }

        [HttpPost("filings/{id}/reject")]
        [Permission(PermissionTypes.LeaveUpdate)]
        public async Task<ActionResult<LeaveDto>> RejectFiling(int id)
        {
            return await _service.RejectFiling(id);
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
            if (!ApprovalTokenHelper.ValidateToken(token, id, secret, out var error))
                return HtmlResult($"{action} failed", error);

            try
            {
                if (approve) await _service.ApproveFiling(id);
                else await _service.RejectFiling(id);
                return HtmlResult($"Leave Filing {(approve ? "Approved" : "Rejected")}",
                    $"The leave filing was successfully {(approve ? "approved" : "rejected")}.");
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
        [Permission(PermissionTypes.LeaveRead)]
        public async Task<ActionResult<PaginatedList<LeaveDto>>> List([FromQuery] LeavePageOptions options)
        {
            return await _service.PaginatedList(options);
        }

        [HttpGet("{id}")]
        [Permission(PermissionTypes.LeaveRead)]
        public async Task<ActionResult<LeaveDto>> GetById(int id)
        {
            var leave = await _service.GetById(id);

            if (leave == null)
                return NotFound();
            else
                return leave;
        }

        [HttpPost]
        [Permission(PermissionTypes.LeaveCreate)]
        public async Task<ActionResult<LeaveDto>> Create([FromBody] CreateLeaveDto dto)
        {
            return await _service.Create(dto);
        }

        [HttpPut("{id}")]
        [Permission(PermissionTypes.LeaveUpdate)]
        public async Task<ActionResult<LeaveDto>> Update(int id, [FromBody] UpdateLeaveDto dto)
        {
            var leave = await _service.Update(id, dto);

            if (leave == null)
                return NotFound();
            else
                return leave;
        }

        [HttpDelete("{id}")]
        [Permission(PermissionTypes.LeaveDelete)]
        public async Task<ActionResult> Delete(int id)
        {
            var leave = await _service.GetById(id);

            if (leave == null) return NotFound();

            await _service.Delete(id);

            return Ok();
        }

        [HttpGet("types")]
        [Permission(PermissionTypes.LeaveRead)]
        public async Task<ActionResult<ICollection<string>>> GetLeaveTypes()
        {
            return await _service.GetLeaveTypes();
        }

        [HttpGet("statuslist")]
        [Permission(PermissionTypes.LeaveRead)]
        public ActionResult<ICollection<string>> GetStatusList()
        {
            return _repository.GetStatusList();
        }

        [HttpGet("ledger")]
        [Permission(PermissionTypes.LeaveRead)]
        public async Task<ActionResult<PaginatedList<LeaveBalanceDto>>> GetLeaveBalance([FromQuery] PageOptions options, string term)
        {
            return await _service.GetLeaveBalance(options, term);
        }

        [HttpGet("ledger/{id}")]
        [Permission(PermissionTypes.LeaveRead)]
        public async Task<ActionResult<PaginatedList<LeaveTransactionDto>>> GetLedger([FromQuery] PageOptions options, string type, int id)
        {
            return await _service.ListTransactions(options, id, type);
        }

        [HttpGet("accupay-leave-template")]
        [Permission(PermissionTypes.LeaveRead)]
        public ActionResult GetLeaveTemplate()
        {
            return Excel(_hostingEnvironment.ContentRootPath + "/ImportTemplates", "accupay-leave-template.xlsx");
        }
    }
}
