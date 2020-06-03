using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Users
{
    public class UpdateRoleDto
    {
        [Required]
        public string Name { get; set; }
    }
}
