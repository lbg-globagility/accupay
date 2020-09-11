using AccuPay.Data.Entities;

namespace AccuPay.Data.Services
{
    public class UserRoleData
    {
        public int OrganizationId { get; set; }
        public AspNetUser User { get; set; }
        public AspNetRole Role { get; set; }
    }
}