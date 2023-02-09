using AccuPay.Core.Entities;
using AccuPay.Core.Services.Policies;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IListOfValueRepository : ISavableRepository<ListOfValue>
    {
        List<string> ConvertToStringList(IEnumerable<ListOfValue> listOfValues, string columnName = "DisplayValue");

        Task<ICollection<ListOfValue>> GetDeductionSchedulesAsync();

        Task<ICollection<ListOfValue>> GetDutyReportProvidersAsync();

        Task<ICollection<ListOfValue>> GetDutyShiftPoliciesAsync();

        Task<ICollection<ListOfValue>> GetEmployeeChecklistsAsync();

        Task<ICollection<ListOfValue>> GetEmployeeDisciplinaryPenalties();

        Task<ICollection<ListOfValue>> GetFilteredListOfValuesAsync(Expression<Func<ListOfValue, bool>> filter);

        ICollection<ListOfValue> GetLeaveConvertiblePolicies();

        ICollection<ListOfValue> GetListOfValues(string type, bool checkIfActive = true);

        Task<ICollection<ListOfValue>> GetListOfValuesAsync(string type, bool checkIfActive = true);

        Task<ListOfValue> GetPolicyAsync(string type, string lic, int organizationId);

        Task<WeeklyPayPeriodPolicy> GetWeeklyPayPeriodPolicyByOrganization(int organizationId, int year);

        Task<ListOfValue> GetListOfValueOfWeeklyPayPeriodPolicyByOrganization(int organizationId);

        Task CreateIfNotExistsAsync(int userId,
            string type,
            string lic,
            string displayValue,
            bool isByOrganization = false,
            int? organizationId = null);
    }
}
