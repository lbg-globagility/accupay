using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogService
    {
        private TimeLogDataService _service;

        public TimeLogService(TimeLogDataService service)
        {
            _service = service;
        }

        public async Task<PaginatedList<TimeLogDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2;
            var paginatedList = await _service.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<TimeLogDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        private static TimeLogDto ConvertToDto(TimeLog timeLog)
        {
            if (timeLog == null) return null;

            return new TimeLogDto()
            {
                Id = timeLog.RowID.Value,
                EmployeeId = timeLog.EmployeeID.Value,
                EmployeeNumber = timeLog.Employee?.EmployeeNo,
                EmployeeName = timeLog.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeType = timeLog.Employee?.EmployeeType,
                Date = timeLog.LogDate,
                StartTime = timeLog.TimeInFull,
                EndTime = timeLog.TimeOutFull,
                BranchId = timeLog.BranchID,
                BranchName = timeLog.Branch?.Name
            };
        }
    }
}
