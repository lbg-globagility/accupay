using Microsoft.AspNetCore.Identity;

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

        public bool IsDepartmentManager { get; private set; }
    }
}
