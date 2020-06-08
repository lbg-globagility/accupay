using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.OfficialBusinesses
{
    public class OfficialBusinessService
    {
        private readonly OfficialBusinessDataService _service;
        private readonly ICurrentUser _currentUser;

        public OfficialBusinessService(OfficialBusinessDataService service, ICurrentUser currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<OfficialBusinessDto>> PaginatedList(PageOptions options, OfficialBusinessFilter filter)
        {
            // TODO: sort and desc in repository
            int organizationId = _currentUser.OrganizationId;
            var paginatedList = await _service.GetPaginatedListAsync(options,
                                                                     organizationId,
                                                                     filter.Term,
                                                                     filter.DateFrom,
                                                                     filter.DateTo);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<OfficialBusinessDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<OfficialBusinessDto> GetById(int id)
        {
            var officialBusiness = await _service.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(officialBusiness);
        }

        public async Task<OfficialBusinessDto> Create(CreateOfficialBusinessDto dto)
        {
            int organizationId = _currentUser.OrganizationId;
            int userId = 1;
            var officialBusiness = new OfficialBusiness()
            {
                EmployeeID = dto.EmployeeId,
                CreatedBy = userId,
                OrganizationID = organizationId,
            };
            ApplyChanges(dto, officialBusiness);

            await _service.SaveAsync(officialBusiness);

            return ConvertToDto(officialBusiness);
        }

        public async Task<OfficialBusinessDto> Update(int id, UpdateOfficialBusinessDto dto)
        {
            var officialBusiness = await _service.GetByIdAsync(id);
            if (officialBusiness == null) return null;

            int userId = 1;
            officialBusiness.LastUpdBy = userId;

            ApplyChanges(dto, officialBusiness);

            await _service.SaveAsync(officialBusiness);

            return ConvertToDto(officialBusiness);
        }

        public async Task Delete(int id)
        {
            await _service.DeleteAsync(id);
        }

        public List<string> GetStatusList()
        {
            return _service.GetStatusList();
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
