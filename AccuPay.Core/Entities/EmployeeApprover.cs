using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("employeeapprover")]
    public class EmployeeApprover : AuditableEntity
    {
        public int ApproverID { get; set; }

        public int EmployeeID { get; set; }

        [ForeignKey("ApproverID")]
        public virtual Approver Approver { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }
    }
}
