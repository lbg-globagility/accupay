using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Core.Services.Imports;
using AccuPay.Core.ValueObjects;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Core.Auth;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public TimeLogService(
            ITimeLogDataService service,
            ITimeLogImportParser importParser,
            ICurrentUser currentUser,
            ITimeLogRepository repository,
            IMapper mapper)
        {
            _dataService = service;
            _importParser = importParser;
            _currentUser = currentUser;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<EmployeeTimeLogsDto>> ListByEmployee(TimeLogsByEmployeePageOptions options)
        {
            var (employees, total, timelogs) = await _repository.ListByEmployeeAsync(_currentUser.OrganizationId, options);
            var dtos = employees.Select(t => ConvertToDto(t, timelogs)).ToList();

            return new PaginatedList<EmployeeTimeLogsDto>(dtos, total, ++options.PageIndex, options.PageSize);
        }

        public async Task<PaginatedList<TimeLogDto>> ListForCurrentEmployee(TimeLogsByEmployeePageOptions options)
        {
            if (!_currentUser.EmployeeId.HasValue)
                throw new Exception("Current user is not associated with an employee.");

            var datePeriod = new TimePeriod(options.DateFrom, options.DateTo);

            var timeLogs = await _repository.GetLatestByEmployeeAndDatePeriodAsync(
                _currentUser.EmployeeId.Value,
                datePeriod);
            var total = timeLogs.Count;

            var paged = timeLogs
                .OrderBy(t => t.LogDate)
                .Skip(options.Offset)
                .Take(options.PageSize)
                .Select(t => ConvertToDto(t))
                .ToList();

            return new PaginatedList<TimeLogDto>(paged, total, ++options.PageIndex, options.PageSize);
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
                BranchName = timeLog.Branch?.Name,
                CreatedBy = timeLog.CreatedBy,
                LastUpd = timeLog.LastUpd,
                Created = timeLog.Created,
                LastUpdBy = timeLog.LastUpdBy,
                LunchIn = timeLog.LunchInFull,
                LunchOut = timeLog.LunchOutFull

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
        internal async Task<TimeLogDto> CheckIn(SelfServiceCreateTimeLogDto timeLog)
        {
            if (timeLog == null) throw new ArgumentNullException(nameof(timeLog));
            var date = timeLog.Date.Date;

            var existingForDate = await _repository.GetLatestByEmployeeAndDatePeriodAsync(
                timeLog.EmployeeId,
                new TimePeriod(date, date));

            if (existingForDate != null && existingForDate.Any(t => t.TimeInFull != null))
            {
                throw new AccuPay.Core.Exceptions.BusinessLogicException("Employee already checked in for the specified date.");
            }

            var newTimelog = new TimeLog();
            newTimelog.EmployeeID = timeLog.EmployeeId;
            newTimelog.TimeInFull = timeLog.StartTime;
            newTimelog.LogDate = date;
            newTimelog.CreatedBy = _currentUser.UserId;
            newTimelog.OrganizationID = _currentUser.OrganizationId;
            newTimelog.TimeStampIn = timeLog.StartTime;
            newTimelog.BranchID = timeLog.BranchId;
            await _repository.SaveAsync(newTimelog);

            var dto = new TimeLogDto()
            {
                Id = newTimelog.RowID,
                EmployeeId = newTimelog.EmployeeID ?? 0,
                StartTime = newTimelog.TimeInFull,
                Date = newTimelog.LogDate,
                BranchId = newTimelog.BranchID
            };
            return dto;
        }
        internal async Task<TimeLogDto> Checkout(int Id, SelfServiceCreateTimeLogDto timeLog)
        {
            var existingTimeLog = _repository.GetById(Id);

            if (existingTimeLog == null)
                throw new Exception("Time log not found.");

            if (existingTimeLog.TimeOutFull != null)
                throw new AccuPay.Core.Exceptions.BusinessLogicException("Time log already checked out for the specified record.");

            existingTimeLog.TimeOutFull = timeLog.EndTime;
            existingTimeLog.LastUpdBy = _currentUser.UserId;
            existingTimeLog.TimeStampOut = timeLog.EndTime;

            await _repository.UpdateAsync(existingTimeLog);

            var dto = new TimeLogDto()
            {
                Id = existingTimeLog.RowID,
                EmployeeId = existingTimeLog.EmployeeID ?? 0,
                EndTime = existingTimeLog.TimeOutFull,
                Date = existingTimeLog.LogDate,
                BranchId = existingTimeLog.BranchID
            };

            return dto;
        }
        internal async Task<TimeLogDto> LunchOut(int Id, SelfServiceCreateTimeLogDto timeLog)
        {
            var existingTimeLog = _repository.GetById(Id);

            if (existingTimeLog == null)
                throw new Exception("Time log not found.");

            if (existingTimeLog.LunchOutFull != null)
                throw new AccuPay.Core.Exceptions.BusinessLogicException("Time log has already lunch out for the specified record.");

            existingTimeLog.LunchOutFull = timeLog.LunchOut;
            existingTimeLog.LastUpdBy = _currentUser.UserId;
            existingTimeLog.TimeStampLunchOut = timeLog.LunchOut;

            await _repository.UpdateAsync(existingTimeLog);

            var dto = new TimeLogDto()
            {
                Id = existingTimeLog.RowID,
                EmployeeId = existingTimeLog.EmployeeID ?? 0,
                LunchOut = existingTimeLog.LunchOutFull,
                Date = existingTimeLog.LogDate,
                BranchId = existingTimeLog.BranchID
            };

            return dto;
        }
        internal async Task<TimeLogDto> LunchIn(int Id, SelfServiceCreateTimeLogDto timeLog)
        {
            var existingTimeLog = _repository.GetById(Id);

            if (existingTimeLog == null)
                throw new Exception("Time log not found.");

            if (existingTimeLog.LunchInFull != null)
                throw new AccuPay.Core.Exceptions.BusinessLogicException("Time log has already lunch in for the specified record.");

            existingTimeLog.LunchInFull = timeLog.LunchIn;
            existingTimeLog.LastUpdBy = _currentUser.UserId;
            existingTimeLog.TimeStampLunchIn = timeLog.LunchIn;

            await _repository.UpdateAsync(existingTimeLog);

            var dto = new TimeLogDto()
            {
                Id = existingTimeLog.RowID,
                EmployeeId = existingTimeLog.EmployeeID ?? 0,
                LunchIn = existingTimeLog.LunchInFull,
                Date = existingTimeLog.LogDate,
                BranchId = existingTimeLog.BranchID
            };

            return dto;
        }

        public async Task<EmployeeTimelogFiling> CreateFiling(CreateEmployeeTimelogFilingDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var filing = new EmployeeTimelogFiling
            {
                EmployeeID = dto.EmployeeId,
                OrganizationID = _currentUser.OrganizationId,
                EntryType = dto.EntryType,
                LogDate = dto.LogDate,
                Time = dto.Time.TimeOfDay,
                Reason = dto.Reason,
                Status = EmployeeTimelogFiling.StatusPending
            };

            // Set audit created by
            filing.CreatedBy = _currentUser.UserId;

            await _repository.CreateFilingAsync(filing);

            return filing;
        }
        public async Task<TimeLogDto> ApproveFiling(int filingId, string decidedBy = null)
        {
            var filing = await _repository.GetFilingByIdAsync(filingId);

            if (filing == null)
                throw new Exception("Filing not found.");

            if (filing.Status == EmployeeTimelogFiling.StatusApproved)
                throw new Exception("Filing already approved.");

            if (filing.Status != EmployeeTimelogFiling.StatusPending)
                throw new Exception("Only pending filings can be approved.");

            // Normalize date
            var date = filing.LogDate.Date;

            // Build timeFull (DateTime) from LogDate date + Time span
            var timeFull = date.Add(filing.Time);

            // Find existing timelog for that employee and date (latest)
            var existing = (await _repository.GetLatestByEmployeeAndDatePeriodAsync(
                filing.EmployeeID.Value,
                new TimePeriod(date, date))).FirstOrDefault(t => t.LogDate.Date == date);

            // Decide action by EntryType
            var entryType = filing.EntryType;

            TimeLog affectedTimeLog = existing;

            switch (entryType)
            {
                case EmployeeTimelogFiling.CheckInType:
                    if (existing == null)
                    {
                        affectedTimeLog = new TimeLog()
                        {
                            OrganizationID = filing.OrganizationID,
                            EmployeeID = filing.EmployeeID,
                            LogDate = date,
                            TimeInFull = timeFull,
                            TimeStampIn = timeFull,
                            CreatedBy = null
                        };

                        await _repository.SaveAsync(affectedTimeLog);
                    }
                    else
                    {
                        existing.TimeInFull = timeFull;
                        existing.TimeStampIn = timeFull;
                        existing.LastUpdBy = null;

                        await _repository.UpdateAsync(existing);
                    }
                    break;

                case EmployeeTimelogFiling.CheckOutType:
                    if (existing == null)
                    {
                        affectedTimeLog = new TimeLog()
                        {
                            OrganizationID = filing.OrganizationID,
                            EmployeeID = filing.EmployeeID,
                            LogDate = date,
                            TimeOutFull = timeFull,
                            TimeStampOut = timeFull,
                            CreatedBy = null
                        };

                        await _repository.SaveAsync(affectedTimeLog);
                    }
                    else
                    {
                        existing.TimeOutFull = timeFull;
                        existing.TimeStampOut = timeFull;
                        existing.LastUpdBy = null;

                        await _repository.UpdateAsync(existing);
                    }
                    break;
                case EmployeeTimelogFiling.LunchOutType:
                    if (existing == null)
                    {
                        affectedTimeLog = new TimeLog()
                        {
                            OrganizationID = filing.OrganizationID,
                            EmployeeID = filing.EmployeeID,
                            LogDate = date,
                            LunchOutFull = timeFull,
                            TimeStampLunchOut = timeFull,
                            CreatedBy = null
                        };

                        await _repository.SaveAsync(affectedTimeLog);
                    }
                    else
                    {
                        existing.LunchOutFull = timeFull;
                        existing.TimeStampLunchOut = timeFull;
                        existing.LastUpdBy = null;

                        await _repository.UpdateAsync(existing);
                    }
                    break;

                case EmployeeTimelogFiling.LunchInType:
                    if (existing == null)
                    {
                        affectedTimeLog = new TimeLog()
                        {
                            OrganizationID = filing.OrganizationID,
                            EmployeeID = filing.EmployeeID,
                            LogDate = date,
                            LunchInFull = timeFull,
                            TimeStampLunchIn = timeFull,
                            CreatedBy = null
                        };

                        await _repository.SaveAsync(affectedTimeLog);
                    }
                    else
                    {
                        existing.LunchInFull = timeFull;
                        existing.TimeStampLunchIn = timeFull;
                        existing.LastUpdBy = null;

                        await _repository.UpdateAsync(existing);
                    }
                    break;

            }

            // Mark filing approved and save
            filing.Status = EmployeeTimelogFiling.StatusApproved;
            filing.DecidedBy = decidedBy;
            filing.LastUpdBy = GetCurrentUserIdOrNull();
            await _repository.UpdateFilingAsync(filing);

            // return DTO of affected TimeLog
            return ConvertToDto(affectedTimeLog);
        }
        public async Task<bool> RejectFiling(int filingId, string decidedBy = null)
        {
            var filing = await _repository.GetFilingByIdAsync(filingId);

            if (filing == null)
                throw new Exception("Filing not found.");

            if (filing.Status == EmployeeTimelogFiling.StatusRejected)
                throw new Exception("Filing already rejected.");

            if (filing.Status != EmployeeTimelogFiling.StatusPending)
                throw new Exception("Only pending filings can be rejected.");

            filing.Status = EmployeeTimelogFiling.StatusRejected;
            filing.DecidedBy = decidedBy;
            filing.LastUpdBy = GetCurrentUserIdOrNull();

            await _repository.UpdateFilingAsync(filing);

            return true;
        }

        // Anonymous email-link approvals/rejections have no authenticated user (UserId is 0),
        // so LastUpdBy should stay null instead of being misattributed to user 0.
        private int? GetCurrentUserIdOrNull()
        {
            return _currentUser.UserId > 0 ? (int?)_currentUser.UserId : null;
        }
        public async Task<PaginatedList<EmployeeTimelogFilingDto>> ListFilingForCurrentEmployee(TimeLogsByEmployeePageOptions options)
        {
            if (!_currentUser.EmployeeId.HasValue)
                throw new Exception("Current user is not associated with an employee.");

            var datePeriod = new TimePeriod(options.DateFrom, options.DateTo);

            var filing = await _repository.GetLatestFilingByEmployeeAndDatePeriodAsync(
                _currentUser.EmployeeId.Value,
                datePeriod);
            var total = filing.Count;

            var paged = filing
                .OrderBy(t => t.LogDate)
                .Skip(options.Offset)
                .Take(options.PageSize)
                .ToList();

            var map= paged.Select(x => _mapper.Map<EmployeeTimelogFilingDto>(x));

            return new PaginatedList<EmployeeTimelogFilingDto>(map, total, ++options.PageIndex, options.PageSize);
        }

        public async Task<EmployeeTimelogFilingDto> GetById(int id)
        {
            var filing = await _repository.GetFilingByIdAsync(id);
            return _mapper.Map<EmployeeTimelogFilingDto>(filing);
        }

        public async Task<PaginatedList<EmployeeTimelogFilingDto>> PaginatedListFilings(TimeLogFilingPageOptions options)
        {
            var paginatedList = await _repository.GetFilingPaginatedListAsync(
                options,
                _currentUser.OrganizationId);

            return paginatedList.Select(x => _mapper.Map<EmployeeTimelogFilingDto>(x));
        }

    }
}
