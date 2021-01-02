using AccuPay.Core.Entities;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IUserDataService
    {
        Task CreateAsync(AspNetUser user, bool isEncrypted = false);

        Task SoftDeleteAsync(int id, int deletedByUserId, int clientId);

        Task UpdateAsync(AspNetUser user, bool isEncrypted = false);
    }
}
