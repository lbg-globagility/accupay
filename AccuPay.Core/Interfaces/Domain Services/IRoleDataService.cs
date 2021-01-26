using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IRoleDataService
    {
        Task CreateAsync(AspNetRole role);

        Task DeleteAsync(int roleId);

        Task UpdateAsync(AspNetRole role);

        Task UpdateUserRolesAsync(ICollection<UserRoleIdData> updatedUserRoles, int clientId);
    }
}
