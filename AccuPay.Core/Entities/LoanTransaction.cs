using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
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
        public int LoanID { get; set; }

        [Column("TotalBalanceLeft")]
        public decimal TotalBalance { get; set; }

        /// <summary>
        /// Total deduction amount. This includes the InterestAmount.
        /// </summary>
        public decimal DeductionAmount { get; set; }

        public decimal InterestAmount { get; set; }

        [ForeignKey("LoanID")]
        public Loan Loan { get; set; }

        [ForeignKey("PayPeriodID")]
        public PayPeriod PayPeriod { get; set; }

        [ForeignKey("PaystubID")]
        public Paystub Paystub { get; set; }

        public DateTime? PayPeriodPayToDate => PayPeriod?.PayToDate;

        public decimal PrincipalAmount => DeductionAmount - InterestAmount;

        [NotMapped]
        public Product Product { get; private set; }

        public bool IsNotLoanOfMorningSun => Product != null ? Product.IsNotLoanOfMorningSun : false;
        public bool IsSssLoanOfMorningSun => Product != null ? Product.IsSssLoanOfMorningSun : false;
        public bool IsPhilHealthLoanOfMorningSun => Product != null ? Product.IsPhilHealthLoanOfMorningSun : false;
        public bool IsHDMFLoanOfMorningSun => Product != null ? Product.IsHDMFLoanOfMorningSun : false;

        public LoanTransaction Clone()
        {
            return new LoanTransaction()
            {
                Created = Created,
                CreatedBy = CreatedBy,
                DeductionAmount = DeductionAmount,
                EmployeeID = EmployeeID,
                InterestAmount = InterestAmount,
                LastUpd = LastUpd,
                LastUpdBy = LastUpdBy,
                Loan = Loan,
                LoanID = LoanID,
                LoanPayPeriodLeft = LoanPayPeriodLeft,
                OrganizationID = OrganizationID,
                PayPeriod = PayPeriod,
                PayPeriodID = PayPeriodID,
                Paystub = Paystub,
                PaystubID = PaystubID,
                RowID = RowID,
                TotalBalance = TotalBalance,
            };
        }

        internal void SetProduct(Product product)
        {
            Product = product;
        }
    }
}
