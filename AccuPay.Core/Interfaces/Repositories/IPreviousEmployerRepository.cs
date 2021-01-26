using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPreviousEmployerRepository : ISavableRepository<PreviousEmployer>
    {
        Task<ICollection<PreviousEmployer>> GetListByEmployeeAsync(int employeeId);
    }
}
