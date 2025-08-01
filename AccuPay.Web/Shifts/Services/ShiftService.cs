using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Shifts.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AccuPay.Core.Services.Imports.ShiftImportParser;

namespace AccuPay.Web.Shifts.Services
{
    public class ShiftService
    {
        private readonly IShiftRepository _repository;
        private readonly IShiftDataService _service;
        private readonly IShiftImportParser _importParser;
        private readonly ICurrentUser _currentUser;

        public ShiftService(
            IShiftRepository repository,
            IShiftDataService service,
            IShiftImportParser importParser,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _service = service;
            _importParser = importParser;
            _currentUser = currentUser;
        }

        internal async Task<PaginatedList<EmployeeShiftsDto>> ListByEmployee(ShiftsByEmployeePageOptions options)
        {
            var (employees, total, shifts) = await _repository.ListByEmployeeAsync(_currentUser.OrganizationId, options);
            var dtos = employees.Select(t => ConvertToDto(t, shifts)).ToList();

            return new PaginatedList<EmployeeShiftsDto>(dtos, total, ++options.PageIndex, options.PageSize);
        }

        internal async Task BatchApply(ICollection<ShiftDto> dtos)
        {
            var employeeIds = dtos.Select(t => t.EmployeeId).ToList();
            var dateFrom = dtos.Select(t => t.Date).Min();
            var dateTo = dtos.Select(t => t.Date).Max();

            var shifts = await _repository
                .GetByMultipleEmployeeAndBetweenDatePeriodAsync(_currentUser.OrganizationId, employeeIds, new TimePeriod(dateFrom, dateTo));

            var added = new List<Shift>();
            var updated = new List<Shift>();
            var deleted = new List<Shift>();

            foreach (var dto in dtos)
            {
                var existingShift = shifts
                    .Where(t => t.DateSched == dto.Date)
                    .Where(t => t.EmployeeID == dto.EmployeeId)
                    .FirstOrDefault();

                var hasData = dto.StartTime.HasValue && dto.EndTime.HasValue;

                if (existingShift is null)
                {
                    if (hasData)
                    {
                        var newShift = new Shift()
                        {
                            OrganizationID = _currentUser.OrganizationId,
                            EmployeeID = dto.EmployeeId,
                            DateSched = dto.Date,
                            StartTimeFull = dto.StartTime,
                            EndTimeFull = dto.EndTime,
                            ShiftBreakStartTimeFull = dto.BreakStartTime,
                            BreakLength = dto.BreakLength,
                            IsRestDay = dto.IsOffset
                        };

                        added.Add(newShift);
                    }
                }
                else
                {
                    if (hasData)
                    {
                        existingShift.StartTimeFull = dto.StartTime;
                        existingShift.EndTimeFull = dto.EndTime;
                        existingShift.ShiftBreakStartTimeFull = dto.BreakStartTime;
                        existingShift.BreakLength = dto.BreakLength;
                        existingShift.IsRestDay = dto.IsOffset;

                        updated.Add(existingShift);
                    }
                    else
                    {
                        deleted.Add(existingShift);
                    }
                }
            }

            await _service.SaveManyAsync(
                currentlyLoggedInUserId: _currentUser.UserId,
                added: added,
                updated: updated,
                deleted: deleted);
        }

        internal async Task<ShiftImportParserOutput> Import(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != _importParser.XlsxExtension)
                throw new InvalidFormatException();

            Stream stream = new MemoryStream();
            await file.CopyToAsync(stream);

            if (stream == null)
                throw new Exception("Unable to parse excel file.");

            int userId = _currentUser.UserId;
            var parsedResult = await _importParser.Parse(stream, _currentUser.OrganizationId);

            if (parsedResult.ValidRecords.Any()) await _service.BatchApply(parsedResult.ValidRecords, organizationId: _currentUser.OrganizationId, currentlyLoggedInUserId: userId);

            return parsedResult;
        }

        private static EmployeeShiftsDto ConvertToDto(Employee employee, ICollection<Shift> shifts)
        {
            var dto = new EmployeeShiftsDto()
            {
                EmployeeId = employee.RowID.Value,
                EmployeeNo = employee.EmployeeNo,
                FullName = employee.FullNameWithMiddleInitialLastNameFirst,
            };

            dto.Shifts = shifts
                .Where(t => t.EmployeeID.Value == employee.RowID)
                .Select(t => ConvertToEmployeeSfhitDto(t))
                .ToList();

            return dto;
        }

        private static EmployeeDutyScheduleDto ConvertToEmployeeSfhitDto(Shift dutySchedule)
        {
            return EmployeeDutyScheduleDto.Convert(dutySchedule);
        }
    }
}
