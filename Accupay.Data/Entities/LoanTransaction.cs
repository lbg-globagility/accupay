using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("scheduledloansperpayperiod")]
    public class LoanTransaction
    {
        [Key]
        public virtual int? RowID { get; set; }

        public virtual DateTime Created { get; set; }

        public virtual int? CreatedBy { get; set; }

        public virtual DateTime LastUpd { get; set; }

        public virtual int? LastUpdBy { get; set; }

        public virtual int? OrganizationID { get; set; }

        public virtual int? PayPeriodID { get; set; }

        public virtual int? EmployeeID { get; set; }

        public virtual int? PaystubID { get; set; }

        public virtual int LoanPayPeriodLeft { get; set; }

        [Column("EmployeeLoanRecordID")]
        public virtual int LoanScheduleID { get; set; }

        [Column("TotalBalanceLeft")]
        public virtual decimal TotalBalance { get; set; }

        [Column("DeductionAmount")]
        public virtual decimal Amount { get; set; }

        [ForeignKey("LoanScheduleID")]
        public virtual LoanSchedule LoanSchedule { get; set; }

        [ForeignKey("PayPeriodID")]
        public virtual PayPeriod PayPeriod { get; set; }

        [ForeignKey("PaystubID")]
        public virtual Paystub Paystub { get; set; }

        public DateTime? PayPeriodPayToDate => PayPeriod?.PayToDate;
    }
}