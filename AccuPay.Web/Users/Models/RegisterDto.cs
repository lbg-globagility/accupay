using System;

namespace AccuPay.Web.Users
{
    public class RegisterDto
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
