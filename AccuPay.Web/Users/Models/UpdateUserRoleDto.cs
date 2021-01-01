using AccuPay.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Users
{
    public class UpdateUserRoleDto
    {
        [Required]
        public int UserId { get; set; }

        public int? RoleId { get; set; }

        public UserRoleIdData ToUserRole(int organizationId)
        {
            return new UserRoleIdData(
                organizationId: organizationId,
                userId: UserId,
                roleId: RoleId
            );
        }
    }
}
