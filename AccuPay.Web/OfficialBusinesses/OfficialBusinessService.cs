using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.OfficialBusinesses
{
    public class OfficialBusinessService
    {
        private readonly OfficialBusinessDataService _dataService;
        private readonly OfficialBusinessRepository _repository;
        private readonly ICurrentUser _currentUser;

        public OfficialBusinessService(OfficialBusinessDataService dataService, OfficialBusinessRepository repository, ICurrentUser currentUser)
        {
            _dataService = dataService;
            _currentUser = currentUser;
            _repository = repository;
        }

        public async Task<PaginatedList<OfficialBusinessDto>> PaginatedList(PageOptions options, OfficialBusinessFilter filter)
        {
            // TODO: sort and desc in repository
            int organizationId = _currentUser.OrganizationId;
            var paginatedList = await _repository.GetPaginatedListAsync(
                options,
                organizationId,
                filter.Term,
                filter.DateFrom,
                filter.DateTo);

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
                CreatedBy = _currentUser.UserId,
                OrganizationID = organizationId,
            };
            ApplyChanges(dto, officialBusiness);

            await _dataService.SaveAsync(officialBusiness);

            return ConvertToDto(officialBusiness);
        }

        public async Task<OfficialBusinessDto> Update(int id, UpdateOfficialBusinessDto dto)
        {
            var officialBusiness = await _repository.GetByIdAsync(id);
            if (officialBusiness == null) return null;

            officialBusiness.LastUpdBy = _currentUser.UserId;

            ApplyChanges(dto, officialBusiness);

            await _dataService.SaveAsync(officialBusiness);

            return ConvertToDto(officialBusiness);
        }

        public async Task Delete(int id)
        {
            await _dataService.DeleteAsync(id);
        }

        public List<string> GetStatusList()
        {
            return _repository.GetStatusList();
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
