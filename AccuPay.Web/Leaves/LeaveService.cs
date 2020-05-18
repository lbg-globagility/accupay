using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Leaves
{
    public class LeaveService
    {
        private readonly LeaveRepository _repository;

        public LeaveService(LeaveRepository repository) => _repository = repository;

        public async Task<PaginatedList<LeaveDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO, sort and desc in repository

            int organizationId = 2; // temporary OrganizationID
            var paginatedList = await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List
                        .Select(x => new LeaveDto()
                        {
                            Id = x.RowID.Value,
                            EmployeeNumber = x.Employee?.EmployeeNo,
                            EmployeeName = x.Employee.FullNameWithMiddleInitialLastNameFirst,
                            LeaveType = x.LeaveType,
                            StartTime = x.StartTimeFull,
                            EndTime = x.EndTimeFull,
                            StartDate = x.StartDate,
                            EndDate = x.ProperEndDate,
                            Status = x.Status,
                            Reason = x.Reason,
                            Comments = x.Comments
                        });

            return new PaginatedList<LeaveDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }
    }
}
