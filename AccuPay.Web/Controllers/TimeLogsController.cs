using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using AccuPay.Web.Core.Extensions;
using AccuPay.Web.TimeLogs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeLogsController : ControllerBase
    {
        private readonly TimeLogService _timeLogService;

        public TimeLogsController(TimeLogService timeLogService)
        {
            _timeLogService = timeLogService;
        }

        [HttpPost("filter")]
        public async Task<PaginatedList<TimeLogDto>> fsdfsd([FromQuery] PageOptions options, [FromBody] TimeLogFilter body)
        {
            int variableOrganizationId = 5;//_currentUser.OrganizationId
            TimePeriod timePeriod = new TimePeriod(body.StartDate, body.EndDate);
            var employeeIds = body.EmployeeIds;

            var query = await _timeLogService.GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(employeeIds, timePeriod);

            //if (!string.IsNullOrWhiteSpace(term))
            //{
            //    query = query.Where(s => $"{s.Employee.EmployeeNo}{s.Employee.FullNameWithMiddleInitialLastNameFirst}".Contains(term, StringComparison.OrdinalIgnoreCase));
            //}

            query = options.Sort switch
            {
                "name" => query.OrderBy(t => t.Employee.FullNameWithMiddleInitialLastNameFirst, options.Direction),
                _ => query.OrderBy(t => t.Employee.FullNameWithMiddleInitialLastNameFirst)
            };

            var roles = query.Page(options).ToList();
            var count = query.Count();

            var dtos = roles.Select(s => TimeLogDto.Convert(s)).ToList();

            return new PaginatedList<TimeLogDto>(dtos, count, options.PageIndex, options.PageSize);
        }

        //[HttpPost("filter")]
        //public ActionResult Test([FromBody] TimeLogFilter body, [FromQuery] PageOptions options)
        //{
        //    return Ok();
        //}
    }

    public class TimeLogFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int[] EmployeeIds { get; set; }
    }
}
