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

        public async Task UpdateAsync(AspNetRole role)
        {
            if (role.IsAdmin)
                throw new BusinessLogicException("`Admin` roles cannot be modified.");

            await _repository.UpdateAsync(role);
        }

        public async Task UpdateUserRolesAsync(
            ICollection<UserRole> added,
            ICollection<UserRole> deleted)
        {
            await _repository.UpdateUserRolesAsync(added, deleted);
        }
    }
}