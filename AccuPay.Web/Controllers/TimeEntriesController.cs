using AccuPay.Data.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.TimeEntries;
using AccuPay.Web.TimeEntries.Models;
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

        [HttpGet("{payPeriodId}")]
        [Permission(PermissionTypes.EmployeeTimeEntryRead)]
        public async Task<ActionResult<TimeEntryPayPeriodDto>> Details(int payPeriodId)
        {
            return await _service.GetDetails(payPeriodId);
        }

        [HttpGet("{payPeriodId}/employees")]
        [Permission(PermissionTypes.EmployeeTimeEntryRead)]
        public async Task<ActionResult<PaginatedList<TimeEntryEmployeeDto>>> List(int payPeriodId, [FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedEmployeeList(payPeriodId, options, term);
        }

        [HttpPost("{payPeriodId}/generate")]
        [Permission(PermissionTypes.EmployeeTimeEntryCreate)]
        public async Task<ActionResult> Generate(int payPeriodId)
        {
            await _service.Generate(payPeriodId);
            return Ok();
        }
    }
}
