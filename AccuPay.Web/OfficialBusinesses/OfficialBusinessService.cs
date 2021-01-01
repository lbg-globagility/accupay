using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services;
using AccuPay.Core.Services.Imports.OfficialBusiness;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AccuPay.Web.OfficialBusinesses
{
    public class OfficialBusinessService
    {
        private readonly OfficialBusinessDataService _dataService;
        private readonly OfficialBusinessRepository _repository;
        private readonly ICurrentUser _currentUser;
        private readonly OfficialBusinessImportParser _importParser;

        public OfficialBusinessService(OfficialBusinessDataService dataService, OfficialBusinessRepository repository, ICurrentUser currentUser, OfficialBusinessImportParser importParser)
        {
            _dataService = dataService;
            _currentUser = currentUser;
            _repository = repository;
            _importParser = importParser;
        }

        public async Task<PaginatedList<OfficialBusinessDto>> PaginatedList(OfficialBusinessPageOptions options)
        {
            // TODO: sort and desc in repository
            int organizationId = _currentUser.OrganizationId;
            var paginatedList = await _repository.GetPaginatedListAsync(options, organizationId);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<OfficialBusinessDto> GetById(int id)
        {
            var officialBusiness = await _repository.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(officialBusiness);
        }

        public async Task<OfficialBusinessDto> Create(CreateOfficialBusinessDto dto)
        {
            int organizationId = _currentUser.OrganizationId;
            var officialBusiness = new OfficialBusiness()
            {
                EmployeeID = dto.EmployeeId,
                OrganizationID = organizationId,
            };
            ApplyChanges(dto, officialBusiness);

            await _dataService.SaveAsync(officialBusiness, _currentUser.UserId);

            return ConvertToDto(officialBusiness);
        }

        public async Task<OfficialBusinessDto> Create(SelfServiceCreateOfficialBusinessDto dto)
        {
            var officialBusiness = new OfficialBusiness()
            {
                EmployeeID = _currentUser.EmployeeId,
                OrganizationID = _currentUser.OrganizationId
            };

            officialBusiness.StartDate = dto.Date;
            officialBusiness.StartTime = dto.StartTime?.TimeOfDay;
            officialBusiness.EndTime = dto.EndTime?.TimeOfDay;
            officialBusiness.Reason = dto.Reason;

            await _dataService.SaveAsync(officialBusiness, _currentUser.UserId);

            return ConvertToDto(officialBusiness);
        }

        public async Task<OfficialBusinessDto> Update(int id, UpdateOfficialBusinessDto dto)
        {
            var officialBusiness = await _repository.GetByIdAsync(id);
            if (officialBusiness == null) return null;

            ApplyChanges(dto, officialBusiness);

            await _dataService.SaveAsync(officialBusiness, _currentUser.UserId);

            return ConvertToDto(officialBusiness);
        }

        public async Task Delete(int id)
        {
            await _dataService.DeleteAsync(
                id: id,
                currentlyLoggedInUserId: _currentUser.UserId);
        }

        public List<string> GetStatusList()
        {
            return _repository.GetStatusList();
        }

        internal async Task<OfficialBusinessImportParserOutput> Import(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != _importParser.XlsxExtension)
                throw new InvalidFormatException();

            Stream stream = new MemoryStream();
            await file.CopyToAsync(stream);

            if (stream == null)
                throw new Exception("Unable to parse excel file.");

            int userId = _currentUser.UserId;
            var parsedResult = await _importParser.Parse(stream, _currentUser.OrganizationId);

            await _dataService.BatchApply(
                parsedResult.ValidRecords,
                organizationId: _currentUser.OrganizationId,
                currentlyLoggedInUserId: userId);

            return parsedResult;
        }

        private static void ApplyChanges(CrudOfficialBusinessDto dto, OfficialBusiness officialBusiness)
        {
            officialBusiness.Status = dto.Status;
            officialBusiness.StartDate = dto.StartDate;
            officialBusiness.StartTime = dto.StartTime.TimeOfDay;
            officialBusiness.EndTime = dto.EndTime.TimeOfDay;
            officialBusiness.Reason = dto.Reason;
            officialBusiness.Comments = dto.Comments;
        }

        private static OfficialBusinessDto ConvertToDto(OfficialBusiness officialBusiness)
        {
            if (officialBusiness == null) return null;

            return new OfficialBusinessDto()
            {
                Id = officialBusiness.RowID.Value,
                EmployeeId = officialBusiness.EmployeeID.Value,
                EmployeeNumber = officialBusiness.Employee?.EmployeeNo,
                EmployeeName = officialBusiness.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeType = officialBusiness.Employee?.EmployeeType,
                StartTime = officialBusiness.StartTimeFull,
                EndTime = officialBusiness.EndTimeFull,
                StartDate = officialBusiness.ProperStartDate,
                EndDate = officialBusiness.ProperEndDate,
                Status = officialBusiness.Status,
                Reason = officialBusiness.Reason,
                Comments = officialBusiness.Comments
            };
        }
    }
}
