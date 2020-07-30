using Microsoft.AspNetCore.Identity;
using System;

namespace AccuPay.Data.Entities
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
    }
}