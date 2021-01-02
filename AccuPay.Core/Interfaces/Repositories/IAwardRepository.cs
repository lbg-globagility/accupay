using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAwardRepository : ISavableRepository<Award>
    {
        Task<ICollection<Award>> GetByEmployeeAsync(int employeeId);
    }
}
