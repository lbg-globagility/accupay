using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILeaveRepository : ISavableRepository<Leave>
    {
        Task<ICollection<Leave>> GetAllApprovedByDatePeriodAsync(int organizationId, TimePeriod datePeriod);

        Task<ICollection<Leave>> GetAllApprovedByEmployeeAndDatePeriod(int organizationId, int employeeId, TimePeriod datePeriod);

        Task<ICollection<Leave>> GetByDatePeriodAsync(int organizationId, TimePeriod datePeriod);

        Task<ICollection<Leave>> GetByEmployeeAndDatePeriodAsync(int organizationId, int[] employeeIds, TimePeriod datePeriod);

        Task<ICollection<Leave>> GetByEmployeeAsync(int employeeId);

        Task<Leave> GetByIdWithEmployeeAsync(int id);

        Task<PaginatedList<Leave>> GetPaginatedListAsync(LeavePageOptions options, int organizationId);

        List<string> GetStatusList();

        Task<ICollection<Leave>> GetUnusedApprovedLeavesByTypeAsync(int employeeId, Leave leave, DateTime firstDayOfTheYear, DateTime lastDayOfTheYear);
    }
}
