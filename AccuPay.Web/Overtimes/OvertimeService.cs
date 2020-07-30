using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.Overtimes
{
    public class OvertimeService
    {
        private readonly OvertimeRepository _repository;
        private readonly OvertimeDataService _service;
        private readonly ICurrentUser _currentUser;

        public OvertimeService(OvertimeRepository repository, OvertimeDataService dataService, ICurrentUser currentUser)
        {
            _repository = repository;
            _service = dataService;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<OvertimeDto>> PaginatedList(PageOptions options, OvertimeFilter filter)
        {
            var paginatedList = await _repository.GetPaginatedListAsync(
                options,
                _currentUser.OrganizationId,
                filter.Term,
                filter.DateFrom,
                filter.DateTo);

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
    }
}
