using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Data.Entities
{
    [Table("employeeloanschedule")]
    public class LoanSchedule : EmployeeDataEntity
    {
        public const string STATUS_IN_PROGRESS = "In Progress";
        public const string STATUS_ON_HOLD = "On hold";
        public const string STATUS_CANCELLED = "Cancelled";
        public const string STATUS_COMPLETE = "Complete";

        public int? LoanTypeID { get; set; }

        public int? BonusID { get; set; }

        public string LoanNumber { get; set; }

        public DateTime DedEffectiveDateFrom { get; set; }

        public decimal TotalLoanAmount { get; set; }

        public string DeductionSchedule { get; set; }

        public decimal DeductionAmount { get; set; }

        public decimal TotalBalanceLeft { get; set; }

        public string Status { get; set; }

        public decimal DeductionPercentage { get; set; }

        // better if set this to either private set or protected set or just as readonly
        // there are domain methods to alter this
        [Column("NoOfPayPeriod")]
        public decimal TotalPayPeriod { get; internal set; }

        // there are domain methods to alter this
        public int LoanPayPeriodLeft { get; internal set; }

        public string Comments { get; set; }

        public string LoanName { get; set; }

        public decimal OriginalDeductionAmount { get; set; }

        /// <summary>
        /// Employee's basic monthly salary when this loan was created
        /// </summary>
        public decimal BasicMonthlySalary { get; set; }

        [ForeignKey("LoanTypeID")]
        public Product LoanType { get; set; }

        public ICollection<LoanTransaction> LoanTransactions { get; set; }

        [ForeignKey("EmployeeID")]
        public Employee Employee { get; set; }

        public ICollection<LoanPaymentFromBonus> LoanPaymentFromBonuses { get; set; }

        public ICollection<YearlyLoanInterest> YearlyLoanInterests { get; set; }

        public bool IsUnEditable => Status == STATUS_CANCELLED || Status == STATUS_COMPLETE;

        public bool HasTransactions => LoanTransactions != null && LoanTransactions.Any();

        /// <summary>
        /// Recomputes TotalPayPeriod. Call this everytime TotalLoanAmount has changed.
        /// </summary>
        public void RecomputeTotalPayPeriod() => TotalPayPeriod = ComputePayPeriod(TotalLoanAmount);

        /// <summary>
        /// Recomputes PayPeriodLeft. Call this everytime TotalBalanceLeft has changed.
        /// This also update the Status.
        /// If the loan was IN PROGRESS and after the recomputation
        /// there is pay periods left, this will become COMPLETE.
        /// If the loan was COMPLETE and after the recomputation
        /// the pay period left is greater than 0, the status will revert back to IN PROGRESS
        /// </summary>
        public void RecomputePayPeriodLeft()
        {
            LoanPayPeriodLeft = ComputePayPeriod(TotalBalanceLeft);

            if ((Status == STATUS_IN_PROGRESS && LoanPayPeriodLeft == 0) ||
                (Status == STATUS_IN_PROGRESS && TotalBalanceLeft <= 0))
            {
                Status = STATUS_COMPLETE;
            }
            else if (Status == STATUS_COMPLETE && LoanPayPeriodLeft > 0 ||
                (Status == STATUS_IN_PROGRESS && TotalBalanceLeft > 0))
            {
                Status = STATUS_IN_PROGRESS;
            }
        }

        public void RetractLoanTransactions(IEnumerable<LoanTransaction> loanTransactions)
        {
            TotalBalanceLeft += loanTransactions.Sum(x => x.PrincipalAmount);
            RecomputePayPeriodLeft();
        }

        public bool IsEligibleForGoldwingsInterest()
        {
            return TotalLoanAmount > BasicMonthlySalary &&
                DeductionPercentage > 0;
        }

        private int ComputePayPeriod(decimal baseAmount)
        {
            if (DeductionAmount == 0 || baseAmount == 0)
                return 0;

            if (DeductionAmount > baseAmount)
                return 1;

            return Convert.ToInt32(Math.Ceiling(baseAmount / DeductionAmount));
        }
    }
}
