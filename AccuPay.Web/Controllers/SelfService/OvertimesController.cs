using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Overtimes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers.SelfService
{
    [Route("api/self-service/[controller]")]
    [ApiController]
    public class OvertimesController : ControllerBase
    {
        private readonly OvertimeService _overtimeService;
        private readonly ICurrentUser _currentUser;

        public OvertimesController(OvertimeService overtimeService, ICurrentUser currentUser)
        {
            _overtimeService = overtimeService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<PaginatedList<OvertimeDto>> List([FromQuery] OvertimePageOptions options)
        {
            options.EmployeeId = _currentUser.EmployeeId;
            var overtimes = await _overtimeService.PaginatedList(options);

            return overtimes;
        }

        [HttpPost]
        public async Task<OvertimeDto> Create([FromBody] SelfServiceCreateOvertimeDto dto)
        {
            var overtime = await _overtimeService.Create(dto);

            return overtime;
        }
    }
}
