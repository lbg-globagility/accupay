using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Users
{
    public class UpdateUserRoleDto
    {
        [Required]
        public int UserId { get; set; }

        public int? RoleId { get; set; }
    }
}
