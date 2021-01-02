using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IEmploymentPolicyRepository
    {
        Task Create(EmploymentPolicy employmentPolicy);

        Task<ICollection<EmploymentPolicy>> GetAllAsync();

        Task<ICollection<EmploymentPolicyType>> GetAllTypes();

        Task<EmploymentPolicy> GetById(int employmentPolicyId);

        Task<(ICollection<EmploymentPolicy> employmentPolicies, int total)> List(PageOptions options);

        Task Update(EmploymentPolicy employmentPolicy);
    }
}
