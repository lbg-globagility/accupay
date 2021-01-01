using Microsoft.AspNetCore.Identity;

namespace AccuPay.Core.Entities
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public int OrganizationId { get; set; }
    }
}
