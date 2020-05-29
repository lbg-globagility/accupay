using AccuPay.Data.Helpers;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeLogsController : ControllerBase
    {
        private readonly TimeLogService _service;

        public TimeLogsController(TimeLogService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<TimeLogDto>>> List([FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedList(options, term);
        }
    }
}
