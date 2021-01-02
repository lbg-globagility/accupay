using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IDisciplinaryActionRepository : ISavableRepository<DisciplinaryAction>
    {
        Task<ICollection<DisciplinaryAction>> GetByEmployeeAsync(int employeeId);
    }
}
