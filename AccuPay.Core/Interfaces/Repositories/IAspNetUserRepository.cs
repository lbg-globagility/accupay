using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAspNetUserRepository
    {
        Task CreateAsync(AspNetUser user);

        Task<AspNetUser> GetByIdAsync(int userId);

        Task<AspNetUser> GetByUserNameAsync(string userName);

        Task<ICollection<UserRoleData>> GetUserRolesAsync(int userId);

        Task<ICollection<AspNetUser>> GetUsersWithoutImageAsync();

        Task<(ICollection<AspNetUser> users, int total)> List(PageOptions options, int clientId, string searchTerm = "", bool includeDeleted = false);

        Task SoftDeleteAsync(AspNetUser user);

        Task UpdateAsync(AspNetUser user);
    }
}
