using Microsoft.AspNetCore.Identity;

namespace AccuPay.Data.Entities
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public int OrganizationId { get; set; }
    }
}
