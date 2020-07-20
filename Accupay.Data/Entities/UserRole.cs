using Microsoft.AspNetCore.Identity;
using System;

namespace AccuPay.Data.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public int OrganizationId { get; set; }

        public UserRole()
        {
        }

        public UserRole(Guid userId, Guid roleId, int organizationId)
        {
            UserId = userId;
            RoleId = roleId;
            OrganizationId = organizationId;
        }
    }
}
