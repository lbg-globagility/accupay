/*
 Delete this once AuditTrail Table is finally deleted.
 */

using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("audittrail")]
    internal class AuditTrail : BaseEntity
    {
        public int? OrganizationID { get; set; }
    }
}
