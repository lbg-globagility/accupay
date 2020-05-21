using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Overtimes
{
    public class OvertimeService
    {
        private readonly OvertimeRepository _repository;

        public OvertimeService(OvertimeRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<OvertimeDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2;
            var paginatedList = await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<OvertimeDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<OvertimeDto> GetById(int id)
        {
            var officialBusiness = await _repository.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(officialBusiness);
        }

        public async Task<OvertimeDto> Create(CreateOvertimeDto dto)
        {
            // TODO: validations

            int organizationId = 2;
            int userId = 1;
            var overtime = new Overtime()
            {
                EmployeeID = dto.EmployeeId,
                CreatedBy = userId,
                OrganizationID = organizationId,
            };
            ApplyChanges(dto, overtime);

            await _repository.SaveAsync(overtime);

            return ConvertToDto(overtime);
        }

        public async Task<OvertimeDto> Update(int id, UpdateOvertimeDto dto)
        {
            // TODO: validations

            var overtime = await _repository.GetByIdAsync(id);
            if (overtime == null) return null;

            int userId = 1;
            overtime.LastUpdBy = userId;

            ApplyChanges(dto, overtime);

            await _repository.SaveAsync(overtime);

            return ConvertToDto(overtime);
        }

        private static void ApplyChanges(ICrudOvertimeDto dto, Overtime overtime)
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
    }
}
