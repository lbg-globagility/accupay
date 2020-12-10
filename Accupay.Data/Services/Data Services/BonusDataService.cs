using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services.DTOs;
using AccuPay.Data.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class BonusDataService
    {
        private readonly BonusRepository _bonusRepository;
        private readonly LoanPaymentFromBonusRepository _loanPaymentFromBonusRepository;
        private readonly LoanRepository _loanRepository;
        private readonly PayPeriodRepository _payPeriodRepository;

        public BonusDataService(
            BonusRepository bonusRepository,
            LoanPaymentFromBonusRepository loanPaymentFromBonusRepository,
            LoanRepository loanRepository,
            PayPeriodRepository payPeriodRepository)
        {
            _bonusRepository = bonusRepository;
            _loanPaymentFromBonusRepository = loanPaymentFromBonusRepository;
            _loanRepository = loanRepository;
            _payPeriodRepository = payPeriodRepository;
        }

        public async Task DeleteAsync(Bonus bonus)
        {
            var thisBonus = await GetByIdAsync(bonus.RowID.Value);

            var loanPaymentFromBonuses = thisBonus.LoanPaymentFromBonuses.ToList();

            var usedAsLoanPaymentInPaystub = loanPaymentFromBonuses
                .Where(lb => lb.Items.Any()).Any();

            var preparedLoanPaymentFromBonus = loanPaymentFromBonuses
                .Where(lb => !lb.Items.Any()).Any();

            if (usedAsLoanPaymentInPaystub)
            {
                var loanText = loanPaymentFromBonuses.Where(lb => lb.Items.Any()).Count() > 1 ? "one or more Loans" : "a Loan";
                throw new BusinessLogicException($"This Bonus has already been used as payment on {loanText}, therefore this can't be deleted.");
            }
            else if (preparedLoanPaymentFromBonus)
            {
                var loanText = loanPaymentFromBonuses.Where(lb => !lb.Items.Any()).Count() > 1 ? "one or more Loans" : "a Loan";
                throw new BusinessLogicException($"This Bonus has already been used as payment on {loanText}. You may unset this Bonus as Loan Payment to proceed deleting.");
            }

            await _bonusRepository.DeleteAsync(bonus);
        }

        public async Task CreateAsync(Bonus bonus)
        {
            await _bonusRepository.CreateAsync(bonus);
        }

        public async Task UpdateAsync(Bonus updatedBonus, Bonus oldBonus, bool usesLoanDeductFromBonus)
        {
            await ValidateUsedBonusForLoanPayment(updatedBonus: updatedBonus, oldBonus: oldBonus, usesLoanDeductFromBonus);

            await UpdateAsync(updatedBonus);
        }

        public async Task UpdateAsync(Bonus bonus)
        {
            await _bonusRepository.UpdateAsync(bonus);
        }

        private async Task ValidateUsedBonusForLoanPayment(Bonus updatedBonus, Bonus oldBonus, bool useLoanDeductFromBonus)
        {
            if (!useLoanDeductFromBonus) return;

            var loanPaymentFromBonuses = await _loanPaymentFromBonusRepository.GetByBonusIdAsync(updatedBonus.RowID.Value);

            var loansPaidByThisBonus = loanPaymentFromBonuses.Where(b => b.Items.Any());
            if (loansPaidByThisBonus.Any())
            {
                updatedBonus.ProductID = oldBonus.ProductID;
                updatedBonus.AllowanceFrequency = oldBonus.AllowanceFrequency;
                updatedBonus.EffectiveStartDate = oldBonus.EffectiveStartDate;
                updatedBonus.EffectiveEndDate = oldBonus.EffectiveEndDate;
                updatedBonus.BonusAmount = oldBonus.BonusAmount;

                var loanText = loansPaidByThisBonus.Count() > 1 ? "one or more Loans" : "a Loan";
                throw new BusinessLogicException($"This Bonus has already been used as payment on {loanText}, therefore this can't be changed.");
            }
            else
            {
                var loanIds = loanPaymentFromBonuses
                    .Select(b => b.LoanId)
                    .ToArray();

                var loans = (await _loanRepository.GetByEmployeeAsync(updatedBonus.EmployeeID.Value))
                    .Where(l => loanIds.Contains(l.RowID.Value))
                    .ToList();

                var models = new List<LoanScheduleWithEndDateData>();
                foreach (var loan in loans)
                {
                    var coveredPeriods = await _payPeriodRepository.GetLoanScheduleRemainingPayPeriodsAsync(loan);
                    models.Add(new LoanScheduleWithEndDateData(loan, coveredPeriods));
                }

                var loansUncoveredByThisBonus = models
                    .Where(m => !(m.DedEffectiveDateFrom <= updatedBonus.EffectiveStartDate && updatedBonus.EffectiveStartDate <= m.DedEffectiveDateTo))
                    .ToList();
                if (loansUncoveredByThisBonus.Any())// bonus effective dates became out of period of loans
                {
                    var ids = loansUncoveredByThisBonus.Select(m => m.Id).ToArray();
                    var affectedLoanPaymentFromBonusList = loanPaymentFromBonuses
                        .Where(l => ids.Contains(l.LoanId))
                        .ToList();
                    affectedLoanPaymentFromBonusList.ForEach(l =>
                    {
                        l.AmountPayment = 0;// set to zero and it'll be deleted
                    });

                    await _loanPaymentFromBonusRepository.SaveManyAsync(affectedLoanPaymentFromBonusList);
                }
                else// bonus amount shrunk
                {
                    var totalPayment = loanPaymentFromBonuses.Sum(lb => lb.AmountPayment);

                    if ((updatedBonus.BonusAmount - totalPayment) < 0)
                    {
                        updatedBonus.BonusAmount = oldBonus.BonusAmount;
                        throw new BusinessLogicException("Bonus Amount shrunk resulting to Loan Payment(s) became insufficient. Try correcting the Loan Payment(s) first then edit this Bonus.");
                    }
                }
            }
        }

        public List<string> GetFrequencyList()
        {
            return _bonusRepository.GetFrequencyList();
        }

        public async Task<IEnumerable<Bonus>> GetByEmployeeAsync(int employeeId)
        {
            return await _bonusRepository.GetByEmployeeAsync(employeeId);
        }

        public async Task<ICollection<Bonus>> GetByPayPeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            return await _bonusRepository.GetByPayPeriodAsync(organizationId, timePeriod);
        }

        public async Task<ICollection<Bonus>> GetByEmployeeAndPayPeriodAsync(int organizationId, int employeeId, TimePeriod timePeriod)
        {
            return await _bonusRepository.GetByEmployeeAndPayPeriodAsync(organizationId: organizationId, employeeId: employeeId, timePeriod);
        }

        public async Task<ICollection<Bonus>> GetByEmployeeAndPayPeriodForLoanPaymentAsync(int organizationId, int employeeId, TimePeriod timePeriod)
        {
            return await _bonusRepository.GetByEmployeeAndPayPeriodForLoanPaymentAsync(organizationId: organizationId, employeeId: employeeId, timePeriod);
        }

        public async Task<Bonus> GetByIdAsync(int id)
        {
            return await _bonusRepository.GetByIdAsync(id);
        }
    }
}
