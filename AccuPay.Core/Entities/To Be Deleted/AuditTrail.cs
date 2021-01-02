/*
 Delete this once AuditTrail Table is finally deleted.
 */

using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("audittrail")]
    public class AuditTrail : BaseEntity
    {
        public int? OrganizationID { get; set; }
    }
}
