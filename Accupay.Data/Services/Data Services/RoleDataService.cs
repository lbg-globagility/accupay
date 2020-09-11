using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class RoleDataService
    {
        private readonly AspNetUserRepository _userRepository;
        private readonly RoleRepository _roleRepository;
        private readonly OrganizationRepository _organizationRepository;

        public RoleDataService(
            AspNetUserRepository userRepository,
            RoleRepository roleRepository,
            OrganizationRepository organizationRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task CreateAsync(AspNetRole role)
        {
            await SanitizeEntity(role);
            await _roleRepository.CreateAsync(role);
        }

        public async Task UpdateAsync(AspNetRole role)
        {
            await SanitizeEntity(role);

            if (role.IsAdmin)
                throw new BusinessLogicException("`Admin` roles cannot be modified.");

            await _roleRepository.UpdateAsync(role);
        }

        public async Task DeleteAsync(int roleId)
        {
            var role = await _roleRepository.GetById(roleId);

            if (role == null)
                throw new BusinessLogicException("Role does not exists.");

            if (role.IsAdmin)
                throw new BusinessLogicException("`Admin` roles cannot be deleted.");

            if (await _roleRepository.HasUsersAsync(role.Id))
                throw new BusinessLogicException("Role already has users therefore cannot be deleted.");

            await _roleRepository.DeleteAsync(role);
        }

        private async Task SanitizeEntity(AspNetRole role)
        {
            if (role == null)
                throw new BusinessLogicException("Invalid data.");

            if (role.ClientId == 0)
                throw new BusinessLogicException("Client is required.");

            if (string.IsNullOrWhiteSpace(role.Name))
                throw new BusinessLogicException("Name is required.");

            if (await _roleRepository.CheckForDuplicateAsync(role.Name, role.Id))
                throw new BusinessLogicException("Name already exists.");

            role.NormalizedName = role.Name.Trim().ToUpper();
        }

        public async Task UpdateUserRolesAsync(ICollection<UserRoleIdData> updatedUserRoles, int clientId)
        {
            var userRoles = await _roleRepository.GetUserRoles();

            var users = (await _userRepository.List(PageOptions.AllData, clientId)).users;
            var roles = (await _roleRepository.List(PageOptions.AllData, clientId)).roles;
            var organizations = (await _organizationRepository.List(OrganizationPageOptions.AllData, clientId)).organizations;

            var added = new Collection<UserRole>();
            var deleted = new Collection<UserRole>();

            foreach (var updatedUserRole in updatedUserRoles)
            {
                var existingUserRole = userRoles
                    .Where(x => x.OrganizationId == updatedUserRole.OrganizationId)
                    .Where(x => x.UserId == updatedUserRole.UserId)
                    .FirstOrDefault();

                // if the role changed
                if (existingUserRole?.RoleId != updatedUserRole?.RoleId)
                {
                    if (existingUserRole != null)
                    {
                        deleted.Add(existingUserRole);
                    }

                    if (updatedUserRole.RoleId.HasValue && updatedUserRole.RoleId != 0)
                    {
                        var user = users.FirstOrDefault(x => x.Id == updatedUserRole.UserId);
                        if (user == null)
                            throw new BusinessLogicException("One of the user does not belong to the client.");

                        var role = roles.FirstOrDefault(x => x.Id == updatedUserRole.RoleId);
                        if (role == null)
                            throw new BusinessLogicException("One of the role does not belong to the client.");

                        var organization = organizations.FirstOrDefault(x => x.RowID == updatedUserRole.OrganizationId);
                        if (organization == null)
                            throw new BusinessLogicException("One of the organization does not belong to the client.");

                        var newUserRole = new UserRole(
                            updatedUserRole.UserId,
                            updatedUserRole.RoleId.Value,
                            updatedUserRole.OrganizationId);

                        added.Add(newUserRole);
                    }
                }
            }

            await _roleRepository.UpdateUserRolesAsync(added, deleted);
        }
    }
}