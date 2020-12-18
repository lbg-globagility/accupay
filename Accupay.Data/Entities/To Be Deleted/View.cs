/*
 Delete this once AuditTrail Table is finally deleted.
 */

using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("view")]
    internal class View : BaseEntity
    {
        public int? OrganizationID { get; set; }
    }
}
