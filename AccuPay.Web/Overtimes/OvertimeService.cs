using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Imports.Overtimes;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AccuPay.Web.Overtimes
{
    public class OvertimeService
    {
        private readonly OvertimeRepository _repository;
        private readonly OvertimeDataService _service;
        private readonly ICurrentUser _currentUser;
        private readonly OvertimeImportParser _importParser;
        private readonly OvertimeDataService _dataService;

        public OvertimeService(
            OvertimeRepository repository,
            OvertimeDataService dataService,
            ICurrentUser currentUser,
            OvertimeImportParser importParser,
            OvertimeDataService overtimeDataService)
        {
            _repository = repository;
            _service = dataService;
            _currentUser = currentUser;
            _importParser = importParser;
            _dataService = overtimeDataService;
        }

        public async Task<PaginatedList<OvertimeDto>> PaginatedList(OvertimePageOptions options)
        {
            var paginatedList = await _repository.GetPaginatedListAsync(options, _currentUser.OrganizationId);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<OvertimeDto> GetById(int id)
        {
            var officialBusiness = await _repository.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(officialBusiness);
        }

        public async Task<OvertimeDto> Create(CreateOvertimeDto dto)
        {
            var overtime = new Overtime()
            {
                EmployeeID = dto.EmployeeId,
                CreatedBy = _currentUser.UserId,
                OrganizationID = _currentUser.OrganizationId,
            };
            ApplyChanges(dto, overtime);

            await _service.SaveAsync(overtime);

            return ConvertToDto(overtime);
        }

        public async Task<OvertimeDto> Create(SelfServiceCreateOvertimeDto dto)
        {
            var overtime = new Overtime()
            {
                EmployeeID = _currentUser.EmployeeId,
                CreatedBy = _currentUser.UserId,
                OrganizationID = _currentUser.OrganizationId,
            };

            overtime.OTStartDate = dto.StartDate;
            overtime.OTStartTime = dto.StartTime.TimeOfDay;
            overtime.OTEndTime = dto.EndTime.TimeOfDay;
            overtime.Reason = dto.Reason;

            await _service.SaveAsync(overtime);

            return ConvertToDto(overtime);
        }

        public async Task<OvertimeDto> Update(int id, UpdateOvertimeDto dto)
        {
            var overtime = await _repository.GetByIdAsync(id);
            if (overtime == null) return null;

            overtime.LastUpdBy = _currentUser.UserId;

            ApplyChanges(dto, overtime);

            await _service.SaveAsync(overtime);

            return ConvertToDto(overtime);
        }

        public async Task Delete(int id)
        {
            await _service.DeleteAsync(id);
        }

        public List<string> GetStatusList()
        {
            return _repository.GetStatusList();
        }

        private static void ApplyChanges(CrudOvertimeDto dto, Overtime overtime)
        {
            overtime.Status = dto.Status;
            overtime.OTStartDate = dto.StartDate;
            overtime.OTStartTime = dto.StartTime.TimeOfDay;
            overtime.OTEndTime = dto.EndTime.TimeOfDay;
            overtime.Reason = dto.Reason;
            overtime.Comments = dto.Comments;
        }

        private static OvertimeDto ConvertToDto(Overtime overtime)
        {
            if (overtime == null) return null;

            return new OvertimeDto()
            {
                Id = overtime.RowID.Value,
                EmployeeId = overtime.EmployeeID.Value,
                EmployeeNumber = overtime.Employee?.EmployeeNo,
                EmployeeName = overtime.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeType = overtime.Employee?.EmployeeType,
                StartTime = overtime.OTStartTimeFull,
                EndTime = overtime.OTEndTimeFull,
                StartDate = overtime.OTStartDate,
                EndDate = overtime.OTEndDate,
                Status = overtime.Status,
                Reason = overtime.Reason,
                Comments = overtime.Comments
            };
        }

        internal async Task<OvertimeImportParserOutput> Import(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != _importParser.XlsxExtension)
                throw new InvalidFormatException();

            Stream stream = new MemoryStream();
            await file.CopyToAsync(stream);

            if (stream == null)
                throw new Exception("Unable to parse excel file.");

            int userId = _currentUser.UserId;
            var parsedResult = await _importParser.Parse(stream, _currentUser.OrganizationId);

            await _dataService.BatchApply(parsedResult.ValidRecords, organizationId: _currentUser.OrganizationId, userId: userId);

            return parsedResult;
        }
    }
}
