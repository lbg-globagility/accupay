using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ISalaryRepository : ISavableRepository<Salary>
    {
        Task<ICollection<Salary>> GetAllAsync(int organizationId);

        Task<ICollection<Salary>> GetByCutOffAsync(int organizationId, DateTime cutoffEnd);

        Task<ICollection<Salary>> GetByEmployeeAndEffectiveFromAsync(int employeeId, DateTime date);

        Task<ICollection<Salary>> GetByEmployeeAsync(int employeeId);

        Task<Salary> GetByIdWithEmployeeAsync(int id);

        Task<ICollection<Salary>> GetByMultipleEmployeeAsync(int[] employeeIds, DateTime cutoffEnd);

        Task<Salary> GetLatest(int employeeId);

        Task<PaginatedList<Salary>> List(PageOptions options, int organizationId, string searchTerm = "", int? employeeId = null);

        Task<List<Salary>> GetSalariesByIds(int[] rowIds);

        Task<List<Salary>> GetMultipleSalariesAsync(int organizationId,
            DateTime dateFrom,
            DateTime dateTo);
    }
}
