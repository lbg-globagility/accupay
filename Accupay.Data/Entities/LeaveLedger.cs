using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("leaveledger")]
    public class LeaveLedger
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int? RowID { get; set; }

        public virtual int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual DateTime Created { get; set; }

        public virtual int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime? LastUpd { get; set; }

        public virtual int? LastUpdBy { get; set; }

        public virtual int? EmployeeID { get; set; }

        public virtual int? ProductID { get; set; }

        public virtual int? LastTransactionID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [ForeignKey("LastTransactionID")]
        public virtual LeaveTransaction LastTransaction { get; set; }

        [InverseProperty("LeaveLedger")]
        public virtual IList<LeaveTransaction> LeaveTransactions { get; set; }
    }
}