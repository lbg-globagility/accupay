using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Leaves;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers.SelfService
{
    [Route("api/self-service/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly LeaveService _leaveService;
        private readonly ICurrentUser _currentUser;

        public LeavesController(LeaveService leaveService, ICurrentUser currentUser)
        {
            _leaveService = leaveService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<PaginatedList<LeaveDto>> List([FromQuery] LeavePageOptions options)
        {
            options.EmployeeId = _currentUser.EmployeeId;
            var dtos = await _leaveService.PaginatedList(options);

            return dtos;
        }

        [HttpPost("leave")]
        public async Task<ActionResult<LeaveDto>> CreateLeave([FromBody] CreateLeaveDto dto)
        {
            dto.EmployeeId = _currentUser.EmployeeId.Value;

            return await _leaveService.Create(dto);
        }

        [HttpDelete]
        public async Task Delete()
        {
        }
    }
}
