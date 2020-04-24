using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Data.Entities
{
    [Table("employeeloanschedule")]
    public class LoanSchedule
    {
        [Key]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        public int? EmployeeID { get; set; }

        public int? LoanTypeID { get; set; }

        public int? BonusID { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public string LoanNumber { get; set; }

        public DateTime DedEffectiveDateFrom { get; set; }

        public DateTime? DedEffectiveDateTo { get; set; }

        public decimal TotalLoanAmount { get; set; }

        public string DeductionSchedule { get; set; }

        public decimal DeductionAmount { get; set; }

        public decimal TotalBalanceLeft { get; set; }

        public string Status { get; set; }

        public decimal DeductionPercentage { get; set; }

        public decimal NoOfPayPeriod { get; set; }

        public int? LoanPayPeriodLeft { get; set; } // This should not be nullable but nullable sa database

        public string Comments { get; set; }

        public string LoanName { get; set; }

        [ForeignKey("LoanTypeID")]
        public virtual Product LoanType { get; set; }

        public virtual ICollection<LoanTransaction> LoanTransactions { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public string EmployeeFullName =>
                        $"{Employee?.LastName}, {Employee?.FirstName} {Employee?.MiddleInitial}";

        public string EmployeeNumber => Employee?.EmployeeNo;

        public virtual void Credit(int? payPeriodID, int organizationId)
        {
            var currentDeductionAmount = DeductionAmount > TotalBalanceLeft ? TotalBalanceLeft : DeductionAmount;
            var newBalance = LastEntry()?.TotalBalance ?? TotalBalanceLeft - currentDeductionAmount;

            var transaction = new LoanTransaction()
            {
                Created = DateTime.Now,
                LastUpd = DateTime.Now,
                OrganizationID = organizationId,
                EmployeeID = EmployeeID,
                PayPeriodID = payPeriodID,
                LoanPayPeriodLeft = LoanPayPeriodLeft == null ? 0 : Convert.ToInt32(LoanPayPeriodLeft) - 1,
                TotalBalance = newBalance,
                Amount = currentDeductionAmount
            };

            LoanTransactions.Add(transaction);
        }

        public virtual LoanTransaction LastEntry() => LoanTransactions.LastOrDefault();

        public virtual void Rollback() => LoanTransactions.Remove(LastEntry());
    }
}