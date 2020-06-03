using AccuPay.Web.TimeEntries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeEntriesController : ControllerBase
    {
        private readonly TimeEntryService _service;

        public TimeEntriesController(TimeEntryService service)
        {
            _service = service;
        }

        [HttpPost("generate/{payPeriodId}")]
        public async Task<ActionResult> Generate(int payPeriodId)
        {
            await _service.Generate(payPeriodId);
            return Ok();
        }
    }
}
