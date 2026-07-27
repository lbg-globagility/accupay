using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Leaves;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers.SelfService
{
    [Route("api/self-service/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly LeaveService _leaveService;
        private readonly ILeaveRepository _leaveRepository;
        private readonly ICurrentUser _currentUser;
        private readonly LeaveEmailService _emailService;

        public LeavesController(
            LeaveService leaveService,
            ILeaveRepository leaveRepository,
            ICurrentUser currentUser,
            LeaveEmailService emailService)
        {
            _leaveService = leaveService;
            _leaveRepository = leaveRepository;
            _currentUser = currentUser;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<PaginatedList<LeaveDto>> List([FromQuery] LeavePageOptions options)
        {
            options.EmployeeId = _currentUser.EmployeeId;
            var dtos = await _leaveService.PaginatedList(options);

            return dtos;
        }

        [HttpPost]
        public async Task<ActionResult<LeaveDto>> CreateLeave([FromBody] SelfServiceCreateLeaveDto dto)
        {
            return await _leaveService.Create(dto);
        }

        [HttpPost("filings/{id}/send-approval-email")]
        public async Task<ActionResult> SendFilingForApprovalEmail(int id)
        {
            var success = await _emailService.SendFilingForApprovalEmailAsync(id);
            if (!success) return NotFound();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete()
        {
        }

        [HttpGet("leave-types")]
        public async Task<ICollection<string>> GetLeaveTypes()
        {
            return await _leaveService.GetLeaveTypes();
        }

        [HttpGet("leave-statuses")]
        public ActionResult<ICollection<string>> GetLeaveStatusesAsync()
        {
            return _leaveRepository.GetStatusList();
        }
    }
}
