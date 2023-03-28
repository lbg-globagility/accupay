using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    public abstract class OrganizationalEntity : AuditableEntity
    {
        public int? OrganizationID { get; set; }

        [ForeignKey("OrganizationID")]
        public virtual Organization Organization { get; set; }
    }
}
