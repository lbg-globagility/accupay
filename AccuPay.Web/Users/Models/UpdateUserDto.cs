using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Users
{
    public class UpdateUserDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
