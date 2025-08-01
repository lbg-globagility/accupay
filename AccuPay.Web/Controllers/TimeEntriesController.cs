using AccuPay.Core.Helpers;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.TimeEntries;
using AccuPay.Web.TimeEntries.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeEntriesController : ControllerBase
    {
        private readonly TimeEntryService _service;

        public TimeEntriesController(TimeEntryService service)
        {
            _service = service;
        }

        [HttpGet("{payPeriodId}")]
        [Permission(PermissionTypes.TimeEntryRead)]
        public async Task<ActionResult<TimeEntryPayPeriodDto>> Details(int? payPeriodId)
        {
            return await _service.GetDetails(payPeriodId);
        }

        [HttpGet("{payPeriodId}/employees")]
        [Permission(PermissionTypes.TimeEntryRead)]
        public async Task<ActionResult<PaginatedList<TimeEntryEmployeeDto>>> List(int payPeriodId, [FromQuery] PageOptions options, string term)
        {
            return await _service.PaginatedEmployeeList(payPeriodId, options, term);
        }

        [HttpGet("{payPeriodId}/employees/{employeeId}")]
        public async Task<ActionResult<ICollection<TimeEntryDto>>> GetAllTimeEntries(int payPeriodId, int employeeId)
        {
            var dtos = await _service.GetTimeEntries(payPeriodId, employeeId);
            return dtos.ToList();
        }

        [HttpPost("{payPeriodId}/generate")]
        [Permission(PermissionTypes.TimeEntryCreate)]
        public async Task<ActionResult> Generate(int payPeriodId)
        {
            await _service.Generate(payPeriodId);
            return Ok();
        }
    }
}
