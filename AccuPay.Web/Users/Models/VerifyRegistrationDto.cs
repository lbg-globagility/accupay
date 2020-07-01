using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Users
{
    public class VerifyRegistrationDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
