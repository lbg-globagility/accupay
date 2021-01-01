using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class BonusDataService : BaseEmployeeDataService<Bonus>, IBonusDataService
    {
        private const string UserActivityName = "Bonus";

        private readonly LoanRepository _loanRepository;
        private readonly LoanPaymentFromBonusRepository _loanPaymentFromBonusRepository;
        private readonly ProductRepository _productRepository;

        public BonusDataService(
            BonusRepository repository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy,
            LoanRepository loanRepository,
            LoanPaymentFromBonusRepository loanPaymentFromBonusRepository,
            ProductRepository productRepository) :

            base(repository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Bonus",
                entityNamePlural: "Bonuses")
        {
            _loanRepository = loanRepository;
            _loanPaymentFromBonusRepository = loanPaymentFromBonusRepository;
            _productRepository = productRepository;
        }

        protected override string GetUserActivityName(Bonus entity)
        {
            return UserActivityName;
        }

        protected override string CreateUserActivitySuffixIdentifier(Bonus entity)
        {
            return $" with type '{entity.BonusType}' and start date '{entity.EffectiveStartDate.ToShortDateString()}'";
        }

        protected override async Task SanitizeEntity(Bonus entity, Bonus oldEntity, int currentlyLoggedInUserId)
        {
            await base.SanitizeEntity(
                entity: entity,
                oldEntity: oldEntity,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            if (entity.ProductID == null)
                throw new BusinessLogicException("Bonus type is required.");

            if (entity.EffectiveStartDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753.");

            if (entity.EffectiveEndDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753.");

            if (entity.EffectiveEndDate.Date < entity.EffectiveStartDate)
                throw new BusinessLogicException("End date cannot be greater than start date.");
        }

        public override async Task DeleteAsync(int id, int currentlyLoggedInUserId)
        {
            var bonus = await _repository.GetByIdAsync(id);

            var loanPaymentFromBonuses = bonus.LoanPaymentFromBonuses.ToList();

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

            await base.DeleteAsync(bonus.RowID.Value, currentlyLoggedInUserId);
        }

        protected override async Task AdditionalSaveValidation(Bonus bonus, Bonus oldBonus)
        {
            if (bonus.IsNewEntity || !_policy.UseLoanDeductFromBonus) return;

            var loanPaymentFromBonuses = await _loanPaymentFromBonusRepository
                .GetByBonusIdAsync(bonus.RowID.Value);

            var loansPaidByThisBonus = loanPaymentFromBonuses.Where(b => b.Items.Any());
            if (loansPaidByThisBonus.Any())
            {
                bonus.ProductID = oldBonus.ProductID;
                bonus.AllowanceFrequency = oldBonus.AllowanceFrequency;
                bonus.EffectiveStartDate = oldBonus.EffectiveStartDate;
                bonus.EffectiveEndDate = oldBonus.EffectiveEndDate;
                bonus.BonusAmount = oldBonus.BonusAmount;

                var loanText = loansPaidByThisBonus.Count() > 1 ? "one or more Loans" : "a Loan";
                throw new BusinessLogicException($"This Bonus has already been used as payment on {loanText}, therefore this can't be changed.");
            }
            else
            {
                var loanIds = loanPaymentFromBonuses
                    .Select(b => b.LoanId)
                    .ToArray();

                var loans = (await _loanRepository.GetByEmployeeAsync(bonus.EmployeeID.Value))
                    .Where(l => loanIds.Contains(l.RowID.Value))
                    .ToList();

                var models = new List<LoanWithEndDateData>();
                foreach (var loan in loans)
                {
                    var coveredPeriods = await _payPeriodRepository.GetLoanRemainingPayPeriodsAsync(loan);
                    models.Add(new LoanWithEndDateData(loan, coveredPeriods));
                }

                var loansUncoveredByThisBonus = models
                    .Where(m => !((m.DedEffectiveDateFrom <= bonus.EffectiveStartDate && bonus.EffectiveStartDate <= m.DedEffectiveDateTo) ||
                        (m.DedEffectiveDateFrom <= bonus.EffectiveEndDate && bonus.EffectiveEndDate <= m.DedEffectiveDateTo)))
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

                    if ((bonus.BonusAmount - totalPayment) < 0)
                    {
                        bonus.BonusAmount = oldBonus.BonusAmount;
                        throw new BusinessLogicException("Bonus Amount shrunk resulting to Loan Payment(s) became insufficient. Try correcting the Loan Payment(s) first then edit this Bonus.");
                    }
                }
            }
        }

        protected override async Task PostDeleteAction(Bonus entity, int currentlyLoggedInUserId)
        {
            // supplying Product data for saving useractivity
            entity.Product = await _productRepository.GetByIdAsync(entity.ProductID.Value);

            await base.PostDeleteAction(entity, currentlyLoggedInUserId);
        }

        protected override async Task PostSaveAction(Bonus entity, Bonus oldEntity, SaveType saveType)
        {
            // supplying Product data for saving useractivity
            entity.Product = await _productRepository.GetByIdAsync(entity.ProductID.Value);

            if (oldEntity != null)
            {
                oldEntity.Product = await _productRepository.GetByIdAsync(oldEntity.ProductID.Value);
            }

            await base.PostSaveAction(entity, oldEntity, saveType);
        }

        protected override async Task RecordUpdate(Bonus bonus, Bonus oldBonus)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldBonus)}.";

            if (bonus.ProductID != oldBonus.ProductID)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldBonus.RowID.Value,
                    Description = $"Updated type from '{oldBonus.BonusType}' to '{bonus.BonusType}' {suffixIdentifier}",
                    ChangedEmployeeId = oldBonus.EmployeeID
                });
            if (bonus.AllowanceFrequency != oldBonus.AllowanceFrequency)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldBonus.RowID.Value,
                    Description = $"Updated frequency from '{oldBonus.AllowanceFrequency}' to '{bonus.AllowanceFrequency}' {suffixIdentifier}",
                    ChangedEmployeeId = oldBonus.EmployeeID
                });
            if (bonus.EffectiveStartDate != oldBonus.EffectiveStartDate)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldBonus.RowID.Value,
                    Description = $"Updated start date from '{oldBonus.EffectiveStartDate.ToShortDateString()}' to '{bonus.EffectiveStartDate.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldBonus.EmployeeID
                });
            if (bonus.EffectiveEndDate != oldBonus.EffectiveEndDate)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldBonus.RowID.Value,
                    Description = $"Updated end date from '{oldBonus.EffectiveEndDate.ToShortDateString()}' to '{bonus.EffectiveEndDate.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldBonus.EmployeeID
                });
            if (bonus.BonusAmount != oldBonus.BonusAmount)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldBonus.RowID.Value,
                    Description = $"Updated amount from '{oldBonus.BonusAmount}' to '{bonus.BonusAmount}' {suffixIdentifier}",
                    ChangedEmployeeId = oldBonus.EmployeeID
                });

            if (changes.Any())
            {
                await _userActivityRepository.CreateRecordAsync(
                    bonus.LastUpdBy.Value,
                    UserActivityName,
                    bonus.OrganizationID.Value,
                    UserActivityRepository.RecordTypeEdit,
                    changes);
            }
        }
    }
}
