namespace AccuPay.Core.Entities
{
    public abstract class OrganizationalEntity : AuditableEntity
    {
        public int? OrganizationID { get; set; }
    }
}
