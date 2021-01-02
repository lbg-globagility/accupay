/*
 Delete this once AuditTrail Table is finally deleted.
 */

using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("view")]
    public class View : BaseEntity
    {
        public int? OrganizationID { get; set; }
    }
}
