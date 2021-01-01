namespace AccuPay.Core.Services
{
    public class UserRoleIdData
    {
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public int? RoleId { get; set; }

        public UserRoleIdData(int organizationId, int userId, int? roleId)
        {
            OrganizationId = organizationId;
            UserId = userId;
            RoleId = roleId;
        }
    }
}