using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Clients
{
    public class CreateClientDto
    {
        [Required]
        public string Name { get; set; }

        public string TradeName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactPerson { get; set; }

        public UserDto User { get; set; }

        public class UserDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }
        }
    }
}
