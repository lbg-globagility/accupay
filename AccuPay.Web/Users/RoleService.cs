using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Users
{
    public class RoleService
    {
        private readonly RoleManager<AspNetRole> _roles;
        private readonly RoleRepository _roleRepository;
        private readonly PermissionRepository _permissionRepository;
        private readonly ICurrentUser _currentUser;

        public RoleService(RoleManager<AspNetRole> roles,
                           RoleRepository roleRepository,
                           PermissionRepository permissionRepository,
                           ICurrentUser currentUser)
        {
            _roles = roles;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _currentUser = currentUser;
        }

        public async Task<RoleDto> GetById(Guid roleId)
        {
            var role = await _roleRepository.GetById(roleId);

            return ConvertToDto(role);
        }

        public async Task<RoleDto> Create(CreateRoleDto dto)
        {
            var role = new AspNetRole();
            role.ClientId = _currentUser.ClientId;
            role.Name = dto.Name;

            await MapRolePermissions(role, dto.RolePermissions);

            var result = await _roles.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new Exception();
            }

            return ConvertToDto(role);
        }

        public async Task<RoleDto> Update(Guid roleId, UpdateRoleDto dto)
        {
            var role = await _roleRepository.GetById(roleId);
            role.Name = dto.Name;

            await MapRolePermissions(role, dto.RolePermissions);

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

        private async Task MapRolePermissions(AspNetRole role, ICollection<RolePermissionDto> rolePermissionDtos)
        {
            var permissions = await _permissionRepository.GetAll();
            foreach (var permission in permissions)
            {
                var rolePermissionDto = rolePermissionDtos
                    .FirstOrDefault(t => t.PermissionId == permission.Id);

                if (rolePermissionDto != null)
                {
                    var rolePermission = new RolePermission();
                    rolePermission.PermissionId = permission.Id;
                    rolePermission.Read = rolePermissionDto.Read;
                    rolePermission.Create = rolePermissionDto.Create;
                    rolePermission.Update = rolePermissionDto.Update;
                    rolePermission.Delete = rolePermission.Delete;

                    role.Permissions.Add(rolePermission);
                }
                else
                {
                }
            }
        }

        private RoleDto ConvertToDto(AspNetRole role)
        {
            var dto = new RoleDto()
            {
                Id = role.Id,
                Name = role.Name
            };

            dto.RolePermissions = role.Permissions?
                .Select(t => ConvertToDto(t)).ToList();

            return dto;
        }

        private RolePermissionDto ConvertToDto(RolePermission rolePermission)
        {
            var dto = new RolePermissionDto()
            {
                PermissionId = rolePermission.PermissionId,
                Read = rolePermission.Read,
                Create = rolePermission.Create,
                Update = rolePermission.Update,
                Delete = rolePermission.Delete
            };

            return dto;
        }
    }
}
