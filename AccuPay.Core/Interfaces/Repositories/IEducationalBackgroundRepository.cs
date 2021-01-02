using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IEducationalBackgroundRepository : ISavableRepository<EducationalBackground>
    {
        Task<ICollection<EducationalBackground>> GetByEmployeeAsync(int employeeId);
    }
}
