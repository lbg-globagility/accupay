using Microsoft.AspNetCore.Identity;

namespace AccuPay.Core.Entities
{
    public class UserToken : IdentityUserToken<int>
    {
        public int OrganizationId { get; set; }
    }
}
