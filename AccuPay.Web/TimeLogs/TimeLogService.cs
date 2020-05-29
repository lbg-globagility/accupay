using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Imports;
using AccuPay.Infrastructure.Services.Excel;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogService
    {
        private TimeLogDataService _service;
        private readonly TimeLogImportParser _importParser;

        public TimeLogService(TimeLogDataService service, TimeLogImportParser importParser)
        {
            _service = service;
            _importParser = importParser;
        }

        public async Task<PaginatedList<TimeLogDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2;
            var paginatedList = await _service.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<TimeLogDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        internal async Task Import(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != TimeLogsReader.PreferredExtension)
                throw new InvalidFormatException("Only .txt files are supported.");

            FileStream fileStream;
            using (fileStream = new FileStream(Path.GetTempFileName(), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            if (fileStream?.Name == null)
                throw new Exception("Unable to parse text file.");

            int organizationId = 2;
            int userId = 1;
            var parsedResult = await _importParser.Parse(fileStream.Name, organizationId: organizationId, userId: userId);

            await _service.SaveImportAsync(parsedResult.GeneratedTimeLogs, parsedResult.GeneratedTimeAttendanceLogs);
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
