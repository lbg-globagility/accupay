using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IBranchRepository
    {
        Task<int?> CreateAsync(Branch branch);

        Task DeleteAsync(Branch branch);

        ICollection<Branch> GetAll();

        Task<ICollection<Branch>> GetAllAsync();

        Task<Branch> GetByIdAsync(int id);

        Task<ICollection<Branch>> GetManyByIdsAsync(int[] ids);

        Task<bool> HasCalendarAsync(PayCalendar payCalendar);

        Task UpdateAsync(Branch branch);
    }
}
