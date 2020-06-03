using Microsoft.AspNetCore.Identity;
using System;

namespace AccuPay.Data.Entities
{
    public class AspNetRole : IdentityRole<Guid>
    {
        public int ClientId { get; set; }

        public AspNetRole()
        {
            Id = Guid.NewGuid();
        }
    }
}
