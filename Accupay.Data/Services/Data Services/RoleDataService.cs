using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class RoleDataService
    {
        private readonly RoleRepository _repository;

        public RoleDataService(RoleRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(AspNetRole role)
        {
            await SanitizeEntity(role);
            await _repository.CreateAsync(role);
        }

        public async Task UpdateAsync(AspNetRole role)
        {
            if (role.IsAdmin)
                throw new BusinessLogicException("`Admin` roles cannot be modified.");

            await SanitizeEntity(role);
            await _repository.UpdateAsync(role);
        }

        private async Task SanitizeEntity(AspNetRole role)
        {
            if (role.ClientId == 0)
                throw new BusinessLogicException("Client is required.");

            if (string.IsNullOrWhiteSpace(role.Name))
                throw new BusinessLogicException("Name is required.");

            if (await _repository.CheckForDuplicateAsync(role.Name, role.Id))
                throw new BusinessLogicException("Name already exists.");

            role.NormalizedName = role.Name.Trim().ToUpper();
        }

        public async Task UpdateUserRolesAsync(
            ICollection<UserRole> added,
            ICollection<UserRole> deleted)
        {
            await _repository.UpdateUserRolesAsync(added, deleted);
        }
    }
}