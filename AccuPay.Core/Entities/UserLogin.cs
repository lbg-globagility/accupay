using Microsoft.AspNetCore.Identity;

namespace AccuPay.Core.Entities
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public int OrganizationId { get; set; }
    }
}
