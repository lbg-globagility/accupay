using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    public class LeaveTransactionType
    {
        public const string Credit = "Credit";

        public const string Debit = "Debit";
    }

    [Table("leavetransaction")]
    public class LeaveTransaction
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

        public virtual int? LeaveLedgerID { get; set; }

        public virtual int? PayPeriodID { get; set; }

        public virtual int? PaystubID { get; set; }

        public virtual int? ReferenceID { get; set; }

        public virtual DateTime TransactionDate { get; set; }

        public string Description { get; set; }

        public virtual string Type { get; set; }

        public virtual decimal Balance { get; set; }

        public virtual decimal Amount { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("LeaveLedgerID")]
        public virtual LeaveLedger LeaveLedger { get; set; }

        [ForeignKey("PayPeriodID")]
        public virtual PayPeriod PayPeriod { get; set; }

        [ForeignKey("ReferenceID")]
        public virtual Leave Leave { get; set; }

        [ForeignKey("PaystubID")]
        public virtual Paystub Paystub { get; set; }

        public bool IsCredit => Type.Trim().ToUpper() == LeaveTransactionType.Credit.ToUpper();

        public bool IsDebit => Type.Trim().ToUpper() == LeaveTransactionType.Debit.ToUpper();
    }
}
