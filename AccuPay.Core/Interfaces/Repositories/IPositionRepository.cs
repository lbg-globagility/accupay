using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPositionRepository : ISavableRepository<Position>
    {
        Task<List<Position>> CreateManyAsync(List<string> jobNames, int organizationId, int userId);

        Task<ICollection<Position>> GetAllAsync(int organizationId);

        Task<Position> GetByIdWithDivisionAsync(int positionId);

        Task<Position> GetByNameAsync(int organizationId, string positionName);

        Task<Position> GetByNameWithDivisionAsync(int organizationId, string positionName);

        Task<Position> GetFirstPositionAsync();

        Task<PaginatedList<Position>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "");
    }
}
