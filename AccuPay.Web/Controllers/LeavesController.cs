using AccuPay.Data.Helpers;
using AccuPay.Web.Leaves;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        private readonly LeaveService _leaveService;

        public LeavesController(LeaveService leaveService) => _leaveService = leaveService;

        [HttpGet]
        public async Task<ActionResult<PaginatedList<LeaveDto>>> List([FromForm] PageOptions options, string searchTerm)
        {
            return await _leaveService.PaginatedList(options, searchTerm);
        }
    }
}
