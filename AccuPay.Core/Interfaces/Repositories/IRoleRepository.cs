using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IRoleRepository : IBaseRepository
    {
        Task<bool> CheckForDuplicateAsync(string name, int? excludeId = null);

        Task CreateAsync(AspNetRole role);

        Task DeleteAsync(AspNetRole role);

        Task<AspNetRole> GetByIdAsync(int roleId);

        AspNetRole GetByUserAndOrganization(int userId, int organizationId);

        Task<AspNetRole> GetByUserAndOrganizationAsync(int userId, int organizationId);

        Task<ICollection<UserRole>> GetUserRoles();

        Task<ICollection<UserRole>> GetUserRoles(int organizationId);

        Task<bool> HasUsersAsync(int roleId);

        Task<(ICollection<AspNetRole> roles, int total)> List(PageOptions options, int clientId);

        Task UpdateAsync(AspNetRole role);

        Task UpdateUserRolesAsync(ICollection<UserRole> added, ICollection<UserRole> deleted);
    }
}
