using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Users
{
    public class RoleService
    {
        private readonly RoleManager<AspNetRole> _roles;
        private readonly RoleRepository _roleRepository;
        private readonly ICurrentUser _currentUser;

        public RoleService(RoleManager<AspNetRole> roles,
                           RoleRepository roleRepository,
                           ICurrentUser currentUser)
        {
            _roles = roles;
            _roleRepository = roleRepository;
            _currentUser = currentUser;
        }

        public async Task<RoleDto> GetById(Guid roleId)
        {
            var role = await _roles.FindByIdAsync(roleId.ToString());

            return ConvertToDto(role);
        }

        public async Task<RoleDto> Create(CreateRoleDto dto)
        {
            var role = new AspNetRole();
            role.ClientId = _currentUser.ClientId;
            role.Name = dto.Name;

            var result = await _roles.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception();
            }

            return ConvertToDto(role);
        }

        public async Task<RoleDto> Update(Guid roleId, UpdateRoleDto dto)
        {
            var role = await _roles.FindByIdAsync(roleId.ToString());
            role.Name = dto.Name;

            var result = await _roles.UpdateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception();
            }

            return ConvertToDto(role);
        }

        public async Task<PaginatedList<RoleDto>> List(PageOptions options)
        {
            var (roles, total) = await _roleRepository.List(options, _currentUser.ClientId);
            var dtos = roles.Select(t => ConvertToDto(t)).ToList();

            return new PaginatedList<RoleDto>(dtos, total, 1, 1);
        }

        private RoleDto ConvertToDto(AspNetRole role)
        {
            var dto = new RoleDto()
            {
                Id = role.Id,
                Name = role.Name
            };

            return dto;
        }
    }
}
