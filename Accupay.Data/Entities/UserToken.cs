using Microsoft.AspNetCore.Identity;

namespace AccuPay.Data.Entities
{
    public class UserToken : IdentityUserToken<int>
    {
        public int OrganizationId { get; set; }
    }
}
