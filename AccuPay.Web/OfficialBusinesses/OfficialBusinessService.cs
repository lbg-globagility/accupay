using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.OfficialBusinesses
{
    public class OfficialBusinessService
    {
        private readonly OfficialBusinessRepository _repository;

        public OfficialBusinessService(OfficialBusinessRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<OfficialBusinessDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2; // temporary OrganizationID
            var paginatedList = await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<OfficialBusinessDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<OfficialBusinessDto> GetById(int id)
        {
            var officialBusiness = await _repository.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(officialBusiness);
        }

        public async Task<OfficialBusinessDto> Create(CreateOfficialBusinessDto dto)
        {
            // TODO: validations

            int organizationId = 2; // temporary OrganizationID
            int userId = 1; // temporary User Id
            var officialBusiness = new OfficialBusiness()
            {
                EmployeeID = dto.EmployeeId,
                CreatedBy = userId,
                OrganizationID = organizationId,
            };
            ApplyChanges(dto, officialBusiness);

            await _repository.SaveAsync(officialBusiness);

            return ConvertToDto(officialBusiness);
        }

        public async Task<OfficialBusinessDto> Update(int id, UpdateOfficialBusinessDto dto)
        {
            // TODO: validations

            var officialBusiness = await _repository.GetByIdAsync(id);
            if (officialBusiness == null) return null;

            int userId = 1; // temporary User Id
            officialBusiness.LastUpdBy = userId;

            ApplyChanges(dto, officialBusiness);

            await _repository.SaveAsync(officialBusiness);

            return ConvertToDto(officialBusiness);
        }

        private static void ApplyChanges(ICrudOfficialBusinessDto dto, OfficialBusiness officialBusiness)
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
