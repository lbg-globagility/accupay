using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AccuPay.Web.Users
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<RolePermissionDto> RolePermissions { get; set; }

        public RoleDto()
        {
            RolePermissions = new Collection<RolePermissionDto>();
        }
    }
}
