using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services;
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
        private readonly RoleDataService _roleDataService;
        private readonly ICurrentUser _currentUser;

        public RoleService(
            RoleManager<AspNetRole> roles,
            RoleRepository roleRepository,
            PermissionRepository permissionRepository,
            RoleDataService roleDataService,
            ICurrentUser currentUser)
        {
            _roles = roles;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _roleDataService = roleDataService;
            _currentUser = currentUser;
        }

        public async Task<RoleDto> GetById(int roleId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);

            return ConvertToDto(role);
        }

        public async Task<RoleDto> GetCurrentRole()
        {
            var role = await _roleRepository.GetByUserAndOrganizationAsync(_currentUser.UserId, _currentUser.OrganizationId);

            if (role is null)
            {
                return null;
            }

            return ConvertToDto(role);
        }

        public async Task<ICollection<UserRoleDto>> GetUserRoles()
        {
            var userRoles = await _roleRepository.GetUserRoles(_currentUser.OrganizationId);
            var dtos = userRoles.Select(t => ConvertToDto(t)).ToList();

            return dtos;
        }

        public async Task UpdateUserRoles(ICollection<UpdateUserRoleDto> dtos)
        {
            var updatedUserRoles = dtos
                .Select(x => x.ToUserRole(_currentUser.OrganizationId))
                .ToList();

            await _roleDataService.UpdateUserRolesAsync(updatedUserRoles, _currentUser.ClientId);
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

        public async Task<RoleDto> Update(int roleId, UpdateRoleDto dto)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);

            role.Name = dto.Name;

            await MapRolePermissions(role, dto.RolePermissions);

            await _roleDataService.UpdateAsync(role);

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
                    role.SetPermission(permission,
                                       rolePermissionDto.Read,
                                       rolePermissionDto.Create,
                                       rolePermissionDto.Update,
                                       rolePermissionDto.Delete);
                }
                else
                {
                    role.RemovePermission(permission);
                }
            }
        }

        private RoleDto ConvertToDto(AspNetRole role)
        {
            var dto = new RoleDto()
            {
                Id = role.Id,
                Name = role.Name,
                IsAdmin = role.IsAdmin
            };

            dto.RolePermissions = role.RolePermissions?
                .Select(t => ConvertToDto(t)).ToList();

            return dto;
        }

        private RolePermissionDto ConvertToDto(RolePermission rolePermission)
        {
            var dto = new RolePermissionDto()
            {
                PermissionId = rolePermission.PermissionId,
                PermissionName = rolePermission.Permission?.Name,
                Read = rolePermission.Read,
                Create = rolePermission.Create,
                Update = rolePermission.Update,
                Delete = rolePermission.Delete
            };

            return dto;
        }

        private UserRoleDto ConvertToDto(UserRole userRole)
        {
            var dto = new UserRoleDto()
            {
                UserId = userRole.UserId,
                RoleId = userRole.RoleId
            };

            return dto;
        }
    }
}
