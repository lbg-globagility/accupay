using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using AccuPay.Core.Services.Imports.Employees;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IEmployeeDataService : IBaseSavableDataService<Employee>
    {
        Task<List<Employee>> BatchApply(IReadOnlyCollection<EmployeeImportModel> validRecords, List<string> jobNames, int organizationId, int changedByUserId);

        Task ImportAsync(ICollection<EmployeeWithLeaveBalanceData> employeeWithLeaveBalanceModels, int organizationId, int userId);

        Task ImportAsync(ICollection<EmployeeWithLeaveBalanceData> employeeWithLeaveBalanceModels, int userId);
    }
}
