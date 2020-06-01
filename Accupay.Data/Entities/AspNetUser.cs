using Microsoft.AspNetCore.Identity;
using System;

namespace AccuPay.Data.Entities
{
    public class AspNetUser : IdentityUser<Guid>
    {
        public AspNetUser()
        {
            Id = Guid.NewGuid();
            Status = AspNetUserStatus.Pending;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? OrganizationId { get; set; }

        public int ClientId { get; set; }

        public AspNetUserStatus Status { get; set; }
    }
}
