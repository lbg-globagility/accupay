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
        public int? RowID { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? OrganizationID { get; set; }

        public int? PayPeriodID { get; set; }

        public int? EmployeeID { get; set; }

        public int? PaystubID { get; set; }

        public int LoanPayPeriodLeft { get; set; }

        [Column("EmployeeLoanRecordID")]
        public int LoanScheduleID { get; set; }

        [Column("TotalBalanceLeft")]
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// Total deduction amount. This includes the InterestAmount.
        /// </summary>
        public decimal DeductionAmount { get; set; }

        public decimal InterestAmount { get; set; }

        [ForeignKey("LoanScheduleID")]
        public LoanSchedule LoanSchedule { get; set; }

        [ForeignKey("PayPeriodID")]
        public PayPeriod PayPeriod { get; set; }

        [ForeignKey("PaystubID")]
        public Paystub Paystub { get; set; }

        public DateTime? PayPeriodPayToDate => PayPeriod?.PayToDate;

        public decimal PrincipalAmount => DeductionAmount - InterestAmount;
    }
}
