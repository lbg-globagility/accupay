using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.OfficialBusinesses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers.SelfService
{
    [Route("api/self-service/official-businesses")]
    [ApiController]
    [Authorize]
    public class OfficialBusinessesController : ControllerBase
    {
        private readonly OfficialBusinessService _officialBusinessService;
        private readonly ICurrentUser _currentUser;

        public OfficialBusinessesController(OfficialBusinessService officialBusinessService, ICurrentUser currentUser)
        {
            _officialBusinessService = officialBusinessService;
            _currentUser = currentUser;
        }

        [HttpPost]
        public async Task<ActionResult<OfficialBusinessDto>> Create([FromBody] SelfServiceCreateOfficialBusinessDto dto)
        {
            var result = await _officialBusinessService.Create(dto);

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<OfficialBusinessDto>>> List([FromQuery] OfficialBusinessPageOptions options)
        {
            options.EmployeeId = _currentUser.EmployeeId;

            return await _officialBusinessService.PaginatedList(options);
        }
    }
}
