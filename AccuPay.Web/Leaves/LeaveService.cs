using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Leaves
{
    public class LeaveService
    {
        private readonly LeaveRepository _repository;
        private readonly Data.Services.LeaveService _service;

        public LeaveService(LeaveRepository repository, Data.Services.LeaveService service)
        {
            _repository = repository;
            _service = service;
        }

        public async Task<PaginatedList<LeaveDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2; // temporary OrganizationID
            var paginatedList = await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<LeaveDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<LeaveDto> GetById(int id)
        {
            var leave = await _repository.GetByIdAsync(id);

            return ConvertToDto(leave);
        }

        public async Task<LeaveDto> Create(CreateLeaveDto dto)
        {
            // TODO: validations

            int organizationId = 2; // temporary OrganizationID
            int userId = 1; // temporary User Id
            var leave = new Leave()
            {
                EmployeeID = dto.EmployeeId,
                CreatedBy = userId,
                OrganizationID = organizationId,
            };
            ApplyChanges(dto, leave);

            // use SaveManyAsync temporarily for validating leave balance
            await _service.SaveManyAsync(new List<Leave> { leave }, organizationId);

            return ConvertToDto(leave);
        }

        public async Task<LeaveDto> Update(int id, UpdateLeaveDto dto)
        {
            // TODO: validations

            var leave = await _repository.GetByIdAsync(id);
            if (leave == null) return null;

            int organizationId = 2; // temporary OrganizationID
            int userId = 1; // temporary User Id
            leave.LastUpdBy = userId;

            ApplyChanges(dto, leave);

            // use SaveManyAsync temporarily for validating leave balance
            await _service.SaveManyAsync(new List<Leave> { leave }, organizationId);

            return ConvertToDto(leave);
        }

        private static void ApplyChanges(ICrudLeaveDto dto, Leave leave)
        {
            leave.LeaveType = dto.LeaveType;
            leave.Status = dto.Status;
            leave.StartDate = dto.StartDate;
            leave.StartTime = dto.StartTime?.TimeOfDay;
            leave.EndTime = dto.EndTime?.TimeOfDay;
            leave.Reason = dto.Reason;
            leave.Comments = dto.Comments;
        }

        private static LeaveDto ConvertToDto(Leave leave)
        {
            if (leave == null) return null;

            return new LeaveDto()
            {
                Id = leave.RowID.Value,
                EmployeeNumber = leave.Employee?.EmployeeNo,
                EmployeeName = leave.Employee?.FullNameWithMiddleInitialLastNameFirst,
                LeaveType = leave.LeaveType,
                StartTime = leave.StartTimeFull,
                EndTime = leave.EndTimeFull,
                StartDate = leave.StartDate,
                EndDate = leave.ProperEndDate,
                Status = leave.Status,
                Reason = leave.Reason,
                Comments = leave.Comments
            };
        }
    }
}
