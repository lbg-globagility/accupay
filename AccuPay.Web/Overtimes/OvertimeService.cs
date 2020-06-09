using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Overtimes
{
    public class OvertimeService
    {
        private readonly OvertimeDataService _service;
        private readonly ICurrentUser _currentUser;

        public OvertimeService(OvertimeDataService service, ICurrentUser currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<OvertimeDto>> PaginatedList(PageOptions options, OvertimeFilter filter)
        {
            var paginatedList = await _service.GetPaginatedListAsync(options,
                                                                     _currentUser.OrganizationId,
                                                                     filter.Term,
                                                                     filter.DateFrom,
                                                                     filter.DateTo);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<OvertimeDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<OvertimeDto> GetById(int id)
        {
            var officialBusiness = await _service.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(officialBusiness);
        }

        public async Task<OvertimeDto> Create(CreateOvertimeDto dto)
        {
            int userId = 1;
            var overtime = new Overtime()
            {
                EmployeeID = dto.EmployeeId,
                CreatedBy = userId,
                OrganizationID = _currentUser.OrganizationId,
            };
            ApplyChanges(dto, overtime);

            await _service.SaveAsync(overtime);

            return ConvertToDto(overtime);
        }

        public async Task<OvertimeDto> Update(int id, UpdateOvertimeDto dto)
        {
            var overtime = await _service.GetByIdAsync(id);
            if (overtime == null) return null;

            int userId = 1;
            overtime.LastUpdBy = userId;

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
            return _service.GetStatusList();
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
