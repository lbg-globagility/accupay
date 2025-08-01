using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Core.Services.Imports;
using AccuPay.Core.ValueObjects;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogService
    {
        private readonly ITimeLogDataService _dataService;
        private readonly ITimeLogImportParser _importParser;
        private readonly ICurrentUser _currentUser;
        private readonly ITimeLogRepository _repository;

        public TimeLogService(
            ITimeLogDataService service,
            ITimeLogImportParser importParser,
            ICurrentUser currentUser,
            ITimeLogRepository repository)
        {
            _dataService = service;
            _importParser = importParser;
            _currentUser = currentUser;
            _repository = repository;
        }

        public async Task<PaginatedList<EmployeeTimeLogsDto>> ListByEmployee(TimeLogsByEmployeePageOptions options)
        {
            var (employees, total, timelogs) = await _repository.ListByEmployeeAsync(_currentUser.OrganizationId, options);
            var dtos = employees.Select(t => ConvertToDto(t, timelogs)).ToList();

            return new PaginatedList<EmployeeTimeLogsDto>(dtos, total, ++options.PageIndex, options.PageSize);
        }

        internal async Task BatchApply(ICollection<UpdateTimeLogDto> dtos)
        {
            var employeeIds = dtos.Select(t => t.EmployeeId).ToList();
            var dateFrom = dtos.Select(t => t.Date).Min();
            var dateTo = dtos.Select(t => t.Date).Max();

            var timeLogs = await _repository
                .GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(employeeIds, new TimePeriod(dateFrom, dateTo));

            var added = new List<TimeLog>();
            var updated = new List<TimeLog>();
            var deleted = new List<TimeLog>();

            foreach (var dto in dtos)
            {
                var existingTimeLog = timeLogs
                    .Where(t => t.LogDate == dto.Date)
                    .Where(t => t.EmployeeID == dto.EmployeeId)
                    .FirstOrDefault();

                var hasData = dto.StartTime != null || dto.EndTime != null;

                if (existingTimeLog is null)
                {
                    if (hasData)
                    {
                        var newTimeLog = new TimeLog()
                        {
                            OrganizationID = _currentUser.OrganizationId,
                            EmployeeID = dto.EmployeeId,
                            LogDate = dto.Date,
                            TimeInFull = dto.StartTime,
                            TimeOutFull = dto.EndTime
                        };

                        added.Add(newTimeLog);
                    }
                }
                else
                {
                    if (hasData)
                    {
                        existingTimeLog.TimeInFull = dto.StartTime;
                        existingTimeLog.TimeOutFull = dto.EndTime;

                        updated.Add(existingTimeLog);
                    }
                    else
                    {
                        deleted.Add(existingTimeLog);
                    }
                }
            }

            await _dataService.SaveManyAsync(
                currentlyLoggedInUserId: _currentUser.UserId,
                added: added,
                updated: updated,
                deleted: deleted);
        }

        internal async Task<TimeLogImportResultDto> Import(IFormFile file)
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

            var parsedResult = await _importParser.Parse(
                importFile: fileStream.Name,
                organizationId: _currentUser.OrganizationId,
                userId: _currentUser.UserId);

            var invalidDtos = parsedResult.InvalidRecords.Select(x => ConvertToImportDetailsDto(x));
            var timeLogs = parsedResult.GeneratedTimeLogs.Select(x => ConvertToDto(x));

            return new TimeLogImportResultDto()
            {
                InvalidRecords = invalidDtos,
                GeneratedTimeLogs = timeLogs
            };
        }

        private static TimeLogImportDetailsDto ConvertToImportDetailsDto(TimeLogImportModel parsedResult)
        {
            return new TimeLogImportDetailsDto()
            {
                EmployeeNumber = parsedResult.EmployeeNumber,
                EmployeeName = parsedResult.EmployeeFullName,
                DateAndTime = parsedResult.DateTime,
                ErrorMessage = parsedResult.ErrorMessage,
                LineContent = parsedResult.LineContent,
                LineNumber = parsedResult.LineNumber,
                Type = parsedResult.Type
            };
        }

        private static TimeLogDto ConvertToDto(TimeLog timeLog)
        {
            if (timeLog == null) return null;

            return new TimeLogDto()
            {
                Id = timeLog.RowID,
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

        private static EmployeeTimeLogsDto ConvertToDto(Employee employee, ICollection<TimeLog> timeLogs)
        {
            var dto = new EmployeeTimeLogsDto()
            {
                EmployeeId = employee.RowID,
                EmployeeNo = employee.EmployeeNo,
                FullName = employee.FullNameWithMiddleInitialLastNameFirst,
                TimeLogs = timeLogs
                    .Where(t => t.EmployeeID == employee.RowID)
                    .Select(t => ConvertToEmployeeTimeLogDto(t))
                    .ToList()
            };

            return dto;
        }

        private static EmployeeTimeLogsDto.EmployeeTimeLogDto ConvertToEmployeeTimeLogDto(TimeLog timeLog)
        {
            var dto = new EmployeeTimeLogsDto.EmployeeTimeLogDto()
            {
                Id = timeLog.RowID,
                Date = timeLog.LogDate,
                StartTime = timeLog.TimeInFull,
                EndTime = timeLog.TimeOutFull,
            };

            return dto;
        }
    }
}
