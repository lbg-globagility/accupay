using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ICertificationRepository : ISavableRepository<Certification>
    {
        Task<ICollection<Certification>> GetByEmployeeAsync(int employeeId);
    }
}
