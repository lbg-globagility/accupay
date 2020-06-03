using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Users
{
    public class CreateRoleDto
    {
        [Required]
        public string Name { get; set; }

        public ICollection<RolePermissionDto> RolePermissions { get; set; }
    }
}
