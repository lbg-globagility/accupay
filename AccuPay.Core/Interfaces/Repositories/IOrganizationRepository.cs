using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IOrganizationRepository : ISavableRepository<Organization>
    {
        Task<bool> CheckIfNameExistsAsync(string name, int? id);

        Task<Organization> GetByIdWithAddressAsync(int id);

        Task<Organization> GetFirst(int clientId);

        Task<ICollection<UserRoleData>> GetUserRolesAsync(int organizationId);

        Task<(ICollection<Organization> organizations, int total)> List(OrganizationPageOptions options, int clientId);
    }
}
