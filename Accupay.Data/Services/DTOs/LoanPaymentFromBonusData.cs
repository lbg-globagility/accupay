using AccuPay.Data.Entities;
using System;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class LoanPaymentFromBonusData
    {
        private readonly decimal _originalAmountPayment;
        //private bool _isFullPayment;

        public LoanPaymentFromBonusData(Bonus bonus, Loan loan)
        {
            BonusAmount = bonus.BonusAmount.Value;
            EffectiveStartDate = bonus.EffectiveStartDate;
            EffectiveEndDate = bonus.EffectiveEndDate;
            Frequency = bonus.AllowanceFrequency;
            BonusId = bonus.RowID.Value;
            BonusType = bonus.BonusType;

            Loan = loan;
            DeductionAmount = loan.DeductionAmount;

            LoanId = loan.RowID.Value;
            var loanPaymentFromBonus = bonus.LoanPaymentFromBonuses.FirstOrDefault(l => l.LoanId == LoanId);

            bool hasItems = false;

            if (loanPaymentFromBonus != null)
            {
                LoanPaymentFromBonus = loanPaymentFromBonus;

                Id = loanPaymentFromBonus.Id;

                AmountPayment = loanPaymentFromBonus.AmountPayment;
                _originalAmountPayment = AmountPayment;

                hasItems = loanPaymentFromBonus.Items?.Any() ?? false;
            }

            IsFullPayment = AmountPayment == 0 ? false : AmountPayment == MaxAvailablePayment;

            if (loan.Status != Loan.STATUS_COMPLETE)
                IsEditable = !hasItems;

            IsNew = Id == 0;

            TotalPayment = bonus.LoanPaymentFromBonuses?.Where(l => l.BonusId == BonusId).Where(l => l.LoanId != LoanId).Sum(l => l.AmountPayment) ?? 0;
        }

        public int Id { get; }
        public int LoanId { get; }
        public int BonusId { get; }
        public string BonusType { get; }
        public decimal BonusAmount { get; }
        public DateTime EffectiveStartDate { get; }
        public DateTime EffectiveEndDate { get; }
        public string Frequency { get; }
        public bool IsEditable { get; }
        public bool IsNew { get; }
        public decimal DeductionAmount { get; }
        public decimal AmountPayment { get; set; }
        public decimal TotalPayment { get; }
        public Loan Loan { get; }
        public LoanPaymentFromBonus LoanPaymentFromBonus { get; }

        public bool IsFulfilled
        {
            get
            {
                return Loan.TotalBalanceLeft <= TotalPayment + AmountPayment;
            }
        }

        public decimal ValidPayment
        {
            get
            {
                return ModDivision(AmountPayment, DeductionAmount);
            }
        }

        public bool IsFullPayment { get; set; }

        public void SetNoAmountPayment()
        {
            AmountPayment = 0;
        }

        public void SetValidAmountPayment()
        {
            AmountPayment = ModDivision(MaxAvailablePayment, DeductionAmount);
        }

        private decimal ModDivision(decimal dividend, decimal divisor)
        {
            var remainder = dividend % divisor;
            if (dividend - remainder == 0)
                return dividend;
            else
                return dividend - remainder;
        }

        public decimal CurrentBonusAmount
        {
            get
            {
                var totalSharedLoanPayment = ExclusiveCurrentBonusAmount + AmountPayment;
                return BonusAmount - totalSharedLoanPayment;
            }
        }

        public decimal ExclusiveCurrentBonusAmount
        {
            get
            {
                return BonusAmount - TotalPayment;
            }
        }

        public decimal InclusiveCurrentBonusAmount
        {
            get
            {
                return BonusAmount - (TotalPayment + AmountPayment);
            }
        }

        public bool IsExcessivePayment
        {
            get
            {
                return AmountPayment > ExclusiveCurrentBonusAmount;
            }
        }

        public decimal MaxAvailablePayment
        {
            get
            {
                var totalBalanceLeft = Loan.TotalBalanceLeft;

                var insufficientToPayMinimumDeductionAmount = ExclusiveCurrentBonusAmount < DeductionAmount;
                if (insufficientToPayMinimumDeductionAmount)
                    return ExclusiveCurrentBonusAmount;

                var sufficientToPayBalance = ExclusiveCurrentBonusAmount - totalBalanceLeft > -1;
                if (sufficientToPayBalance)
                    return totalBalanceLeft;
                else
                    return ExclusiveCurrentBonusAmount;
            }
        }

        public bool HasChanged
        {
            get
            {
                return AmountPayment != _originalAmountPayment;
            }
        }

        public LoanPaymentFromBonus Export(int organizationId, int userId)
        {
            if (!IsNew)
            {
                LoanPaymentFromBonus.AmountPayment = AmountPayment;
                LoanPaymentFromBonus.LastUpdBy = userId;
                return LoanPaymentFromBonus;
            }
            else
                return new LoanPaymentFromBonus()
                {
                    OrganizationID = organizationId,
                    CreatedBy = userId,
                    AmountPayment = AmountPayment,
                    BonusId = BonusId,
                    LoanId = LoanId
                };
        }
    }
}
