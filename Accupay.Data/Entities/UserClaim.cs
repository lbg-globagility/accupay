using Microsoft.AspNetCore.Identity;

namespace AccuPay.Data.Entities
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public int OrganizationId { get; set; }
    }
}
