using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Imports;
using AccuPay.Web.Shifts.Models;
using System;
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

        public ShiftService(EmployeeDutyScheduleRepository repository, EmployeeDutyScheduleDataService service, ShiftImportParser importParser)
        {
            _repository = repository;
            _service = service;
            _importParser = importParser;
        }

        public async Task<PaginatedList<ShiftDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2;
            var paginatedList = await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);

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
            int organizationId = 2;
            int userId = 1;

            var shift = new EmployeeDutySchedule
            {
                OrganizationID = organizationId,
                CreatedBy = userId,
                EmployeeID = dto.EmployeeId
            };
            ApplyChanges(dto, shift);

            await _repository.CreateAsync(shift);

            return ConvertToDto(shift);
        }

        internal async Task Import(ImportShiftDto dto)
        {
            Stream stream;
            using (stream = new MemoryStream())
            {
                await dto.File.CopyToAsync(stream);
            }

            if (stream == null)
                throw new Exception("Unable to parse excel file.");

            int organizationId = 2;
            int userId = 1;
            var parsedResult = await _importParser.Parse(stream, organizationId);

            await _service.BatchApply(parsedResult.ValidRecords, organizationId: organizationId, userId: userId);
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

        private static void ApplyChanges(ICrudShiftDto dto, EmployeeDutySchedule shift)
        {
            shift.DateSched = dto.DateSched;
            shift.StartTime = dto.StartTime.Value.TimeOfDay;
            shift.EndTime = dto.EndTime.Value.TimeOfDay;
            shift.BreakStartTime = dto.BreakStartTime.Value.TimeOfDay;
            shift.BreakLength = dto.BreakLength;
            shift.IsRestDay = dto.IsRestDay;

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
                DateSched = shift.DateSched,
                StartTime = shift.ShiftStartTimeFull,
                EndTime = shift.ShiftEndTimeFull,
                BreakStartTime = shift.ShiftBreakStartTimeFull,
                BreakLength = shift.BreakLength,
                IsRestDay = shift.IsRestDay
            };
        }
    }
}
