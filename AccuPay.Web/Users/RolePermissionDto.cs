namespace AccuPay.Web.Users
{
    public class RolePermissionDto
    {
        public int PermissionId { get; set; }

        public bool Read { get; set; }

        public bool Create { get; set; }

        public bool Update { get; set; }

        public bool Delete { get; set; }
    }
}
