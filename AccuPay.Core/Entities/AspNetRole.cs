﻿using AccuPay.Utilities.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AccuPay.Core.Entities
{
    public class AspNetRole : IdentityRole<int>
    {
        public int ClientId { get; set; }

        public bool IsAdmin { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        public AspNetRole()
        {
            RolePermissions = new Collection<RolePermission>();
        }

        public RolePermission GetPermission(string permissionName)
        {
            return RolePermissions
                .FirstOrDefault(p => p.Permission?.Name.ToTrimmedLowerCase() == permissionName.ToTrimmedLowerCase());
        }

        public bool HasPermission(string permissionName, string action)
        {
            var rolePermission = GetPermission(permissionName);

            if (rolePermission is null)
            {
                return false;
            }

            switch (action)
            {
                case "read":
                    return rolePermission.Read;

                case "create":
                    return rolePermission.Create;

                case "update":
                    return rolePermission.Update;

                case "delete":
                    return rolePermission.Delete;

                default:
                    return false;
            }
        }

        public void SetPermission(Permission permission, bool read = false, bool create = false, bool update = false, bool delete = false)
        {
            var rolePermission = RolePermissions.FirstOrDefault(p => p.PermissionId == permission.Id);

            if (rolePermission is null)
            {
                rolePermission = new RolePermission();
                rolePermission.PermissionId = permission.Id;
                rolePermission.Permission = permission;

                RolePermissions.Add(rolePermission);
            }

            rolePermission.Read = read;
            rolePermission.Create = create;
            rolePermission.Update = update;
            rolePermission.Delete = delete;
        }

        public void RemovePermission(Permission permission)
        {
            var rolePermission = RolePermissions.FirstOrDefault(t => t.Id == permission.Id);
            RolePermissions.Remove(rolePermission);
        }
    }
}