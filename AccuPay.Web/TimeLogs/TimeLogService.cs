using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Imports;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogService
    {
        private TimeLogDataService _service;
        private readonly TimeLogImportParser _importParser;
        private readonly ICurrentUser _currentUser;

        public TimeLogService(TimeLogDataService service,
                              TimeLogImportParser importParser,
                              ICurrentUser currentUser)
        {
            _service = service;
            _importParser = importParser;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<TimeLogDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            var paginatedList = await _service.GetPaginatedListAsync(options, _currentUser.OrganizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<TimeLogDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<PaginatedList<EmployeeTimeLogsDto>> ListByEmployee(PageOptions options, DateTime dateFrom, DateTime dateTo)
        {
            var (employees, total, timelogs) = await _service.ListByEmployee(_currentUser.OrganizationId, options, dateFrom, dateTo);
            var dtos = employees.Select(t => ConvertToDto(t, timelogs)).ToList();

            return new PaginatedList<EmployeeTimeLogsDto>(dtos, total, ++options.PageIndex, options.PageSize);
        }

        internal async Task<ActionResult<TimeLogDto>> Create(CreateTimeLogDto dto)
        {
            // TODO: validations
            int userId = 1;

            var timeLog = new TimeLog
            {
                OrganizationID = _currentUser.OrganizationId,
                CreatedBy = userId,
                EmployeeID = dto.EmployeeId,
                LogDate = dto.Date
            };

            ApplyChanges(dto, timeLog);

            await _service.CreateAsync(timeLog);

            return ConvertToDto(timeLog);
        }

        internal async Task<TimeLogDto> Update(int id, UpdateTimeLogDto dto)
        {
            // TODO: validations
            var timeLog = await _service.GetByIdAsync(id);
            if (timeLog == null) return null;

            timeLog.LastUpdBy = 1;

            ApplyChanges(dto, timeLog);

            await _service.UpdateAsync(timeLog);

            return ConvertToDto(timeLog);
        }

        internal async Task<TimeLogDto> GetByIdWithEmployeeAsync(int id)
        {
            var timelog = await _service.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(timelog);
        }

        internal async Task<TimeLogDto> GetByIdAsync(int id)
        {
            var timelog = await _service.GetByIdAsync(id);

            return ConvertToDto(timelog);
        }

        internal async Task Delete(int id)
        {
            await _service.DeleteAsync(id);
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

            int userId = 1;
            var parsedResult = await _importParser.Parse(fileStream.Name, organizationId: _currentUser.OrganizationId, userId: userId);

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

        private static void ApplyChanges(CrudTimeLogDto dto, TimeLog timeLog)
        {
            timeLog.BranchID = dto.BranchId;
            timeLog.TimeInFull = dto.StartTime;
            timeLog.TimeOutFull = dto.EndTime;
        }

        private static EmployeeTimeLogsDto ConvertToDto(Employee employee, ICollection<TimeLog> timeLogs)
        {
            var dto = new EmployeeTimeLogsDto()
            {
                EmployeeId = employee.RowID,
                EmployeeNo = employee.EmployeeNo,
                FullName = employee.FullName,
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
                StartTime = timeLog.TimeStampIn,
                EndTime = timeLog.TimeStampOut,
            };

            return dto;
        }
    }
}
