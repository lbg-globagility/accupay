using AccuPay.Data.Entities;
using System;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class LoanPaymentFromBonusModel
    {
        private readonly decimal _originalAmountPayment;
        private bool _isFullPayment;

        public LoanPaymentFromBonusModel(Bonus bonus, LoanSchedule loanSchedule)
        {
            BonusAmount = bonus.BonusAmount.Value;
            EffectiveStartDate = bonus.EffectiveStartDate;
            EffectiveEndDate = bonus.EffectiveEndDate;
            Frequency = bonus.AllowanceFrequency;
            BonusId = bonus.RowID.Value;

            LoanSchedule = loanSchedule;
            DeductionAmount = loanSchedule.DeductionAmount;

            LoanId = loanSchedule.RowID.Value;
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

            _isFullPayment = AmountPayment == 0 ? false : AmountPayment == MaxAvailablePayment;

            if (loanSchedule.Status != LoanSchedule.STATUS_COMPLETE)
                IsEditable = !hasItems;

            IsNew = Id == 0;

            TotalPayment = bonus.LoanPaymentFromBonuses?.Where(l => l.BonusId == BonusId).Where(l => l.LoanId != LoanId).Sum(l => l.AmountPayment) ?? 0;
        }

        public int Id { get; }
        public int LoanId { get; }
        public int BonusId { get; }
        public decimal BonusAmount { get; }
        public DateTime EffectiveStartDate { get; }
        public DateTime EffectiveEndDate { get; }
        public string Frequency { get; }
        public bool IsEditable { get; }
        public bool IsNew { get; }
        public decimal DeductionAmount { get; }
        public decimal AmountPayment { get; set; }
        public decimal TotalPayment { get; }
        public LoanSchedule LoanSchedule { get; }
        public virtual LoanPaymentFromBonus LoanPaymentFromBonus { get; set; }

        public bool IsFulfilled
        {
            get
            {
                return LoanSchedule.TotalBalanceLeft <= TotalPayment + AmountPayment;
            }
        }

        public decimal ValidPayment
        {
            get
            {
                return ModDivision(AmountPayment, DeductionAmount);
            }
        }

        public bool IsFullPayment
        {
            get
            {
                return _isFullPayment;
            }
            set
            {
                _isFullPayment = value;

                if (value)
                    AmountPayment = ModDivision(MaxAvailablePayment, DeductionAmount);
                else
                    AmountPayment = 0;
            }
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
                var totalBalanceLeft = LoanSchedule.TotalBalanceLeft;

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
