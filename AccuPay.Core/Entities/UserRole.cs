using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    public class UserRole : IdentityUserRole<int>
    {
        public int OrganizationId { get; set; }

        public UserRole()
        {
        }

        public UserRole(int userId, int roleId, int organizationId)
        {
            UserId = userId;
            RoleId = roleId;
            OrganizationId = organizationId;
        }

        [ForeignKey("RoleId")]
        public virtual AspNetRole Role { get; set; }
    }
}
