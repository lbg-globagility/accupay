using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Imports;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Shifts.Models;
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

        public ShiftService(EmployeeDutyScheduleDataService service, ShiftImportParser importParser)
        {
            _service = service;
            _importParser = importParser;
        }

        public async Task<PaginatedList<ShiftDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2;
            var paginatedList = await _service.GetPaginatedListAsync(options, organizationId, searchTerm);

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
            int organizationId = 2;
            int userId = 1;

            var shift = new EmployeeDutySchedule
            {
                OrganizationID = organizationId,
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

        internal async Task Import(ImportShiftDto dto)
        {
            if (Path.GetExtension(dto.File.FileName) != _importParser.XlsxExtension)
                throw new InvalidFormatException();

            Stream stream = new MemoryStream();
            await dto.File.CopyToAsync(stream);

            if (stream == null)
                throw new Exception("Unable to parse excel file.");

            int organizationId = 2;
            int userId = 1;
            var parsedResult = await _importParser.Parse(stream, organizationId);

            await _service.BatchApply(parsedResult.ValidRecords, organizationId: organizationId, userId: userId);
        }

        private static void ApplyChanges(CrudShiftDto dto, EmployeeDutySchedule shift)
        {
            shift.StartTime = dto.StartTime.TimeOfDay;
            shift.EndTime = dto.EndTime.TimeOfDay;
            shift.BreakStartTime = dto.BreakStartTime.TimeOfDay;
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
