using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Users
{
    public class CreateUserDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public int? EmployeeId { get; set; }
    }
}
