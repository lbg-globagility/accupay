using AccuPay.Core.Helpers;
using AccuPay.Core.Repositories;
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
        private readonly LeaveRepository _leaveRepository;
        private readonly ICurrentUser _currentUser;

        public LeavesController(LeaveService leaveService, LeaveRepository leaveRepository, ICurrentUser currentUser)
        {
            _leaveService = leaveService;
            _leaveRepository = leaveRepository;
            _currentUser = currentUser;
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
