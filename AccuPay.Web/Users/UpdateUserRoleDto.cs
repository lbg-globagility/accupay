using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Users
{
    public class UpdateUserRoleDto
    {
        [Required]
        public Guid UserId { get; set; }

        public Guid? RoleId { get; set; }
    }
}
