using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Shifts.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Web.Shifts.Services
{
    public class ShiftService
    {
        private readonly EmployeeDutyScheduleRepository _repository;

        public ShiftService(EmployeeDutyScheduleRepository shiftRepository)
        {
            _repository = shiftRepository;
        }

        public async Task<PaginatedList<ShiftDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2; // temporary OrganizationID
            var paginatedList = await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<ShiftDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        internal async Task<ShiftDto> Create(CreateShiftDto dto)
        {
            // TODO: validations
            var shift = new EmployeeDutySchedule
            {
                OrganizationID = 5,
                CreatedBy = 1,
                EmployeeID = dto.EmployeeId
            };
            ApplyChanges(dto, shift);

            await _repository.CreateAsync(shift);

            return ConvertToDto(shift);
        }

        internal async Task<ShiftDto> Update(int id, UpdateShiftDto dto)
        {
            // TODO: validations
            var shift = await _repository.GetByIdAsync(id);
            if (shift == null) return null;

            shift.LastUpdBy = 1;

            ApplyChanges(dto, shift);

            await _repository.UpdateAsync(shift);

            return ConvertToDto(shift);
        }

        private static void ApplyChanges(ICrudShiftDto dto, EmployeeDutySchedule x)
        {
            x.DateSched = dto.DateSched;
            x.StartTime = dto.StartTime;
            x.EndTime = dto.EndTime;
            x.BreakStartTime = dto.BreakStartTime;
            x.BreakLength = dto.BreakLength;
            x.IsRestDay = dto.IsRestDay;
            x.ShiftHours = dto.ShiftHours;
            x.WorkHours = dto.WorkHours;
        }

        private static ShiftDto ConvertToDto(EmployeeDutySchedule shift)
        {
            if (shift == null) return null;

            return new ShiftDto()
            {
                Id = shift.RowID,
                EmployeeNumber = shift.Employee?.EmployeeNo,
                EmployeeName = shift.Employee?.FullNameWithMiddleInitialLastNameFirst,
                DateSched = shift.DateSched,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime,
                BreakStartTime = shift.BreakStartTime,
                BreakLength = shift.BreakLength,
                IsRestDay = shift.IsRestDay,
                ShiftHours = shift.ShiftHours,
                WorkHours = shift.WorkHours
            };
        }

    }
}
