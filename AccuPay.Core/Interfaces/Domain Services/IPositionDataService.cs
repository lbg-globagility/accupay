using AccuPay.Core.Entities;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPositionDataService : IBaseSavableDataService<Position>
    {
        Task<Position> GetByNameOrCreateAsync(string positionName, int organizationId, int currentlyLoggedInUserId);
    }
}
