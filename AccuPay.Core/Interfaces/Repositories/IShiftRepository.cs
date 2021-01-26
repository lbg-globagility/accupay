using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IShiftRepository : ISavableRepository<Shift>
    {
        Task<ICollection<Shift>> GetByDatePeriodAsync(int organizationId, TimePeriod datePeriod);

        Task<ICollection<Shift>> GetByEmployeeAndDatePeriodAsync(int organizationId, int employeeId, TimePeriod datePeriod);

        Task<ICollection<Shift>> GetByEmployeeAndDatePeriodAsync(int organizationId, int[] employeeIds, TimePeriod datePeriod);

        Task<ICollection<Shift>> GetByEmployeeAndDatePeriodWithEmployeeAsync(int organizationId, int[] employeeIds, TimePeriod datePeriod);

        Task<ICollection<Shift>> GetByMultipleEmployeeAndBetweenDatePeriodAsync(int organizationId, IEnumerable<int> employeeIds, TimePeriod timePeriod);

        Task<(ICollection<Employee> employees, int total, ICollection<Shift>)> ListByEmployeeAsync(int organizationId, ShiftsByEmployeePageOptions options);
    }
}
