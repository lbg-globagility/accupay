using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAllowanceTypeRepository
    {
        Task<AllowanceType> CreateAsync(AllowanceType allowanceType);

        Task<List<AllowanceType>> CreateManyAsync(List<AllowanceType> notYetExistsAllowanceTypes);

        Task DeleteAsync(int id);

        Task<ICollection<AllowanceType>> GetAllAsync();

        Task<AllowanceType> GetByIdAsync(int id);

        Task<PaginatedList<AllowanceType>> GetPaginatedListAsync(PageOptions options, string searchTerm = "");

        Task UpdateAsync(AllowanceType allowanceType);
    }
}
