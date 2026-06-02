using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ISoloParentBeneficiaryDataService : IBaseSavableDataService<SoloParentBeneficiary>
    {
        Task<ICollection<SoloParentBeneficiary>> GetAllByOrganizationIdAsync(int orgId);

        Task<SoloParentBeneficiary> GetByEmployeeIdAsync(int employeeId);

        Task<bool> IsEmployeeBeneficiaryAsync(int employeeId);
    }
}
