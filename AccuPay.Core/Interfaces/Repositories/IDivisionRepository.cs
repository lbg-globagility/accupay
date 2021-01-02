using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IDivisionRepository : ISavableRepository<Division>
    {
        Task<IEnumerable<Division>> GetAllAsync(int organizationId);

        Task<ICollection<Division>> GetAllParentsAsync(int organizationId);

        Task<Division> GetByIdWithParentAsync(int id);

        List<string> GetDivisionTypeList();

        Task<Division> GetOrCreateDefaultDivisionAsync(int organizationId, int userId);

        Task<PaginatedList<Division>> List(PageOptions options, int organizationId, string searchTerm = "");
    }
}
