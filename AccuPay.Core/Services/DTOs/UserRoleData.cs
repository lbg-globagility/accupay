using AccuPay.Core.Entities;

namespace AccuPay.Core.Services
{
    public class UserRoleData
    {
        public int OrganizationId { get; set; }
        public AspNetUser User { get; set; }
        public AspNetRole Role { get; set; }
        public bool IsDepartmentManager { get; private set; }
        public bool IsHighTierApprover { get; private set; }
    }
}
