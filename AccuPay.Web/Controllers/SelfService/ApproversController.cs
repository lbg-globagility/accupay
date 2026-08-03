using AccuPay.Web.Appraisers;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers.SelfService
{
    [Route("api/self-service/[controller]")]
    [ApiController]
    [Authorize]
    public class ApproversController : ControllerBase
    {
        private readonly ApproverService _service;
        private readonly ICurrentUser _currentUser;

        public ApproversController(ApproverService service, ICurrentUser currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<ActionResult<List<SelfServiceApproverDto>>> List()
        {
            return await _service.GetByEmployeeId(_currentUser.EmployeeId.Value);
        }
    }
}
