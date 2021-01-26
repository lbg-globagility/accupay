using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IEmployeeRepository : ISavableRepository<Employee>
    {
        Task<Employee> GetActiveEmployeeWithDivisionAndPositionAsync(int employeeId);

        Task<ICollection<Employee>> GetAllActiveAsync(int organizationId);

        Task<ICollection<Employee>> GetAllActiveWithDivisionAndPositionAsync(int organizationId);

        Task<ICollection<Employee>> GetAllActiveWithoutPayrollAsync(int payPeriodId, int organizationId);

        Task<ICollection<Employee>> GetAllActiveWithPositionAsync(int organizationId);

        Task<ICollection<Employee>> GetAllAsync(int organizationId);

        Task<ICollection<Employee>> GetAllWithDivisionAndPositionAsync(int organizationId);

        Task<ICollection<Employee>> GetAllWithPayrollAsync(int payPeriodId, int organizationId);

        Task<ICollection<Employee>> GetAllWithPositionAsync(int organizationId);

        Task<ICollection<Employee>> GetByBranchAsync(int branchId);

        Task<Employee> GetByEmployeeNumberAsync(string employeeNumber, int organizationId);

        Task<ICollection<Employee>> GetByMultipleEmployeeNumberAsync(string[] employeeNumbers, int organizationId);

        Task<ICollection<Employee>> GetByMultipleIdAsync(int[] employeeIdList);

        Task<ICollection<Employee>> GetByPositionAsync(int positionId);

        Task<Salary> GetCurrentSalaryAsync(int employeeId, DateTime? cutOffEnd = null);

        Task<ICollection<Employee>> GetEmployeesWithoutImageAsync();

        Task<ICollection<string>> GetEmploymentStatuses();

        Task<string> GetImagePathByIdAsync(int employeeId);

        Task<PaginatedList<Employee>> GetPaginatedListAsync(EmployeePageOptions options, int organizationId);

        Task<PaginatedList<Employee>> GetPaginatedListWithTimeEntryAsync(PageOptions options, int organizationId, int payPeriodId, string searchTerm = "");

        Task<decimal> GetSickLeaveBalance(int employeeId);

        Task<PaginatedList<Employee>> GetUnregisteredEmployeeAsync(PageOptions options, string searchTerm, int clientId, int organizationId);

        Task<decimal> GetVacationLeaveBalance(int employeeId);

        Task<IEnumerable<Employee>> SearchSimpleLocal(IEnumerable<Employee> employees, string searchValue);
    }
}
