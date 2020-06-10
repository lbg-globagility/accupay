using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Imports;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Shifts.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Shifts.Services
{
    public class ShiftService
    {
        private readonly EmployeeDutyScheduleDataService _service;
        private readonly ShiftImportParser _importParser;
        private readonly ICurrentUser _currentUser;

        public ShiftService(EmployeeDutyScheduleDataService service,
                            ShiftImportParser importParser,
                            ICurrentUser currentUser)
        {
            _service = service;
            _importParser = importParser;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<ShiftDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            var paginatedList = await _service.GetPaginatedListAsync(options,
                                                                    _currentUser.OrganizationId,
                                                                    searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<ShiftDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        internal async Task<ShiftDto> GetById(int id)
        {
            var shift = await _service.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(shift);
        }

        internal async Task<ShiftDto> Create(CreateShiftDto dto)
        {
            // TODO: validations
            int userId = 1;

            var shift = new EmployeeDutySchedule
            {
                OrganizationID = _currentUser.OrganizationId,
                CreatedBy = userId,
                EmployeeID = dto.EmployeeId,
                DateSched = dto.Date
            };

            ApplyChanges(dto, shift);

            await _service.CreateAsync(shift);

            return ConvertToDto(shift);
        }

        internal async Task<ShiftDto> Update(int id, UpdateShiftDto dto)
        {
            // TODO: validations
            var shift = await _service.GetByIdAsync(id);
            if (shift == null) return null;

            shift.LastUpdBy = 1;

            ApplyChanges(dto, shift);

            await _service.UpdateAsync(shift);

            return ConvertToDto(shift);
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

            int userId = 1;
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
                StartTime = shift.ShiftStartTimeFull,
                EndTime = shift.ShiftEndTimeFull,
                BreakStartTime = shift.ShiftBreakStartTimeFull,
                BreakLength = shift.BreakLength,
                IsOffset = shift.IsRestDay
            };
        }
    }
}
