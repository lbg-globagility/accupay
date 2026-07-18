
using AccuPay.Core.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using AccuPay.Core.Helpers;

namespace AccuPay.Core.Interfaces
{
    public interface IApproverRepository : ISavableRepository<Approver>
    {
        Task<PaginatedList<Approver>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "");
        Task<Approver> GetByIdWithOrganizationAsync(int id);
    }
}
