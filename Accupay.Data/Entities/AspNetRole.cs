using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AccuPay.Data.Entities
{
    public class AspNetRole : IdentityRole<Guid>
    {
        public int ClientId { get; set; }

        public virtual ICollection<RolePermission> Permissions { get; set; }

        public AspNetRole()
        {
            Id = Guid.NewGuid();
            Permissions = new Collection<RolePermission>();
        }
    }
}
