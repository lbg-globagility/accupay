using Microsoft.AspNetCore.Identity;
using System;

namespace AccuPay.Data.Entities
{
    public class AspNetUser : IdentityUser<int>
    {
        public AspNetUser()
        {
            Status = AspNetUserStatus.Pending;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int ClientId { get; set; }

        public AspNetUserStatus Status { get; set; }

        public int? OriginalImageId { get; set; }

        public virtual File OriginalImage { get; set; }

        public int UserLevel { get; set; }

        public string DesktopPassword { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string FullName => $"{FirstName} {LastName}".Trim();

        public int CreatedById { get; set; }

        public int? EmployeeId { get; set; }
    }
}