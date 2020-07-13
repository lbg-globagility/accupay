using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Imports;
using AccuPay.Data.ValueObjects;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Shifts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Shifts.Services
{
    public class ShiftService
    {
        private readonly EmployeeDutyScheduleRepository _repository;
        private readonly EmployeeDutyScheduleDataService _service;
        private readonly ShiftImportParser _importParser;
        private readonly ICurrentUser _currentUser;

        public ShiftService(
            EmployeeDutyScheduleRepository repository,
            EmployeeDutyScheduleDataService service,
            ShiftImportParser importParser,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _service = service;
            _importParser = importParser;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<ShiftDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            var paginatedList = await _repository.GetPaginatedListAsync(options,
                                                                    _currentUser.OrganizationId,
                                                                    searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<ShiftDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        internal async Task<ShiftDto> GetById(int id)
        {
            var shift = await _repository.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(shift);
        }

        internal async Task<ShiftDto> Create(CreateShiftDto dto)
        {
            // TODO: validations
            var shift = new EmployeeDutySchedule
            {
                OrganizationID = _currentUser.OrganizationId,
                CreatedBy = _currentUser.DesktopUserId,
                EmployeeID = dto.EmployeeId,
                DateSched = dto.Date
            };

            ApplyChanges(dto, shift);

            await _service.CreateAsync(shift);

            return ConvertToDto(shift);
        }

        internal async Task BatchApply(ICollection<ShiftDto> dtos)
        {
            var employeeIds = dtos.Select(t => t.EmployeeId).ToList();
            var dateFrom = dtos.Select(t => t.Date).Min();
            var dateTo = dtos.Select(t => t.Date).Max();

            var shifts = await _repository
                .GetByMultipleEmployeeAndBetweenDatePeriodAsync(_currentUser.OrganizationId, employeeIds, new TimePeriod(dateFrom, dateTo));

            var added = new List<EmployeeDutySchedule>();
            var updated = new List<EmployeeDutySchedule>();
            var deleted = new List<EmployeeDutySchedule>();

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
                        var newShift = new EmployeeDutySchedule()
                        {
                            OrganizationID = _currentUser.OrganizationId,
                            EmployeeID = dto.EmployeeId,
                            CreatedBy = _currentUser.DesktopUserId,
                            LastUpdBy = _currentUser.DesktopUserId,
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
                        existingShift.LastUpdBy = _currentUser.DesktopUserId;

                        updated.Add(existingShift);
                    }
                    else
                    {
                        deleted.Add(existingShift);
                    }
                }
            }

            await _service.ChangeManyAsync(added, updated, deleted);
        }

        public async Task Delete(int id)
        {
            await _service.DeleteAsync(id);
        }

        internal async Task Import(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != _importParser.XlsxExtension)
                throw new InvalidFormatException();

            Stream stream = new MemoryStream();
            await file.CopyToAsync(stream);

            if (stream == null)
                throw new Exception("Unable to parse excel file.");

            int userId = _currentUser.DesktopUserId;
            var parsedResult = await _importParser.Parse(stream, _currentUser.OrganizationId);

            await _service.BatchApply(parsedResult.ValidRecords, organizationId: _currentUser.OrganizationId, userId: userId);
        }

        private static void ApplyChanges(CrudShiftDto dto, EmployeeDutySchedule shift)
        {
            shift.StartTime = dto.StartTime.TimeOfDay;
            shift.EndTime = dto.EndTime.TimeOfDay;
            shift.BreakStartTime = dto.BreakStartTime?.TimeOfDay;
            shift.BreakLength = dto.BreakLength;
            shift.IsRestDay = dto.isOffset;

            shift.ComputeShiftHours();
        }

        private static ShiftDto ConvertToDto(EmployeeDutySchedule shift)
        {
            if (shift == null) return null;

            return new ShiftDto()
            {
                Id = shift.RowID,
                EmployeeId = shift.EmployeeID.Value,
                EmployeeNumber = shift.Employee?.EmployeeNo,
                EmployeeName = shift.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeType = shift.Employee?.EmployeeType,
                Date = shift.DateSched,
                StartTime = shift.StartTimeFull,
                EndTime = shift.EndTimeFull,
                BreakStartTime = shift.ShiftBreakStartTimeFull,
                BreakLength = shift.BreakLength,
                IsOffset = shift.IsRestDay
            };
        }

        internal async Task<PaginatedList<EmployeeShiftsDto>> ListByEmployee(ShiftsByEmployeePageOptions options)
        {
            var (employees, total, shifts) = await _service.ListByEmployeeAsync(_currentUser.OrganizationId, options);
            var dtos = employees.Select(t => ConvertToDto(t, shifts)).ToList();

            return new PaginatedList<EmployeeShiftsDto>(dtos, total, ++options.PageIndex, options.PageSize);
        }

        private static EmployeeShiftsDto ConvertToDto(Employee employee, ICollection<EmployeeDutySchedule> shifts)
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

        private static EmployeeDutyScheduleDto ConvertToEmployeeSfhitDto(EmployeeDutySchedule dutySchedule)
        {
            return EmployeeDutyScheduleDto.Convert(dutySchedule);
        }
    }
}
