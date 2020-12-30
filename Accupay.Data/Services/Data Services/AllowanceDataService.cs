using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services.Imports.Allowances;
using AccuPay.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class AllowanceDataService : BaseEmployeeDataService<Allowance>
    {
        private const string UserActivityName = "Allowance";

        private readonly AllowanceRepository _allowanceRepository;
        private readonly ProductRepository _productRepository;
        private readonly AllowanceTypeRepository _allowanceTypeRepository;

        public AllowanceDataService(
            AllowanceRepository allowanceRepository,
            AllowanceTypeRepository allowanceTypeRepository,
            PayPeriodRepository payPeriodRepository,
            ProductRepository productRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(allowanceRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Allowance")
        {
            _allowanceRepository = allowanceRepository;
            _productRepository = productRepository;
            _allowanceTypeRepository = allowanceTypeRepository;
        }

        public async Task BatchApply(
            IReadOnlyCollection<AllowanceImportModel> validRecords,
            int organizationId,
            int currentlyLoggedInUserId)
        {
            AllowanceType setAllowanceType(IGrouping<string, AllowanceImportModel> a) =>
                new AllowanceType()
                {
                    DisplayString = a.FirstOrDefault().AllowanceName,
                    Frequency = a.FirstOrDefault().AllowanceFrequency,
                    Name = a.FirstOrDefault().AllowanceName
                };

            var notYetExistsAllowanceTypes = validRecords
                .Where(a => a.IsAllowanceTypeNotYetExists)
                .GroupBy(a => a.AllowanceName)
                .Select(a => setAllowanceType(a))
                .ToList();

            if (notYetExistsAllowanceTypes.Any())
            {
                var newlyAddedAllowanceTypes = await _allowanceTypeRepository.CreateManyAsync(notYetExistsAllowanceTypes);

                var recordsWithoutAllowanceTypeId = validRecords
                    .Where(a => a.IsAllowanceTypeNotYetExists);

                foreach (var record in recordsWithoutAllowanceTypeId)
                {
                    var newlyAddedAllowanceType = newlyAddedAllowanceTypes.Where(at => at.Name == record.AllowanceName).FirstOrDefault();
                    record.AllowanceTypeId = newlyAddedAllowanceType.Id;
                }
            }

            List<Allowance> allowances = new List<Allowance>();
            foreach (var record in validRecords)
            {
                var allowance = new Allowance()
                {
                    AllowanceFrequency = record.AllowanceFrequency,
                    AllowanceTypeId = record.AllowanceTypeId,
                    Amount = record.Amount.Value,
                    EmployeeID = record.EmployeeId,
                    EffectiveEndDate = record.EffectiveEndDate,
                    EffectiveStartDate = record.EffectiveStartDate.Value,
                    OrganizationID = organizationId,
                };

                allowances.Add(allowance);
            }

            await SaveManyAsync(allowances, currentlyLoggedInUserId);
        }

        #region Overrides

        protected override string GetUserActivityName(Allowance allowance)
        {
            return UserActivityName;
        }

        protected override string CreateUserActivitySuffixIdentifier(Allowance allowance)
        {
            return $" with type '{allowance.Type}' and start date '{allowance.EffectiveStartDate.ToShortDateString()}'";
        }

        protected override async Task SanitizeEntity(Allowance allowance, Allowance oldAllowance, int changedByUserId)
        {
            await base.SanitizeEntity(
                entity: allowance,
                oldEntity: oldAllowance,
                currentlyLoggedInUserId: changedByUserId);

            if (allowance.IsOneTime)
                allowance.EffectiveEndDate = allowance.EffectiveStartDate;

            if (_allowanceRepository.GetFrequencyList().Contains(allowance.AllowanceFrequency) == false)
                throw new BusinessLogicException("Invalid frequency.");

            if (allowance.ProductID == null)
                throw new BusinessLogicException("Allowance type is required.");

            if (allowance.EffectiveStartDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

            if (allowance.EffectiveEndDate != null && allowance.EffectiveStartDate > allowance.EffectiveEndDate)
                throw new BusinessLogicException("Start date cannot be greater than end date.");
        }

        protected override async Task AdditionalDeleteValidation(Allowance allowance)
        {
            if (await CheckIfAlreadyUsedInClosedPayPeriodAsync(allowance.RowID.Value))
                throw new BusinessLogicException("This allowance has already been used and therefore cannot be deleted. Try changing its End Date instead.");
        }

        protected override async Task AdditionalSaveValidation(Allowance allowance, Allowance oldAllowance)
        {
            var product = await _productRepository.GetByIdAsync(allowance.ProductID.Value);

            if (product == null)
                throw new BusinessLogicException("The selected allowance type no longer exists.");

            if (allowance.IsMonthly && !product.Fixed)
                throw new BusinessLogicException("Only fixed allowance type are allowed for Monthly allowances.");

            // cannot create a new allowance or change the Start Date of an
            // existing allowance into a date on a "Closed" pay period.
            if (CheckIfStartDateNeedsToBeValidated(new List<Allowance>() { oldAllowance }, allowance))
            {
                await CheckIfDataIsWithinClosedPayPeriod(allowance.EffectiveStartDate, allowance.OrganizationID.Value);
            }

            if (!allowance.IsNewEntity)
            {
                // validate entities that are for update
                var payPeriods = await _allowanceRepository.GetPayPeriodsAsync(allowance.RowID.Value);
                var alreadyUsedInClosedPayroll = CheckIfDataIsWithinClosedPayPeriod(payPeriods, throwException: false);

                if (alreadyUsedInClosedPayroll)
                {
                    // only end date can be updated if allowance is already used in a closed pay period
                    ValidateAllowableEdit(
                        allowance: allowance,
                        oldAllowance: oldAllowance);

                    // end date cannot be in a closed pay period
                    if (allowance.EffectiveEndDate.HasValue)
                    {
                        if (await CheckIfDataIsWithinClosedPayPeriod(allowance.EffectiveEndDate.Value, allowance.OrganizationID.Value, throwException: false))
                            throw new BusinessLogicException("Cannot update End Date into a date in a Closed Payroll.");
                    }
                }
            }
        }

        protected override async Task AdditionalSaveManyValidation(List<Allowance> allowances, List<Allowance> oldAllowances, SaveType saveType)
        {
            var allowanceTypeIds = allowances
                .Where(x => x.ProductID.HasValue)
                .Select(x => x.ProductID.Value).Distinct()
                .ToArray();
            var allowanceTypes = await _productRepository.GetManyByIdsAsync(allowanceTypeIds);

            foreach (var allowance in allowances)
            {
                var product = allowanceTypes.FirstOrDefault(x => x.RowID == allowance.ProductID);

                if (product == null)
                    throw new BusinessLogicException("The selected allowance type no longer exists.");

                if (allowance.IsMonthly && !product.Fixed)
                    throw new BusinessLogicException("Only fixed allowance type are allowed for Monthly allowances.");
            }

            await CheckForClosedPayPeriod(allowances, oldAllowances);
        }

        protected override async Task PostDeleteAction(Allowance entity, int currentlyLoggedInUserId)
        {
            // supplying Product data for saving useractivity
            entity.Product = await _productRepository.GetByIdAsync(entity.ProductID.Value);

            await base.PostDeleteAction(entity, currentlyLoggedInUserId);
        }

        protected override async Task PostSaveAction(Allowance entity, Allowance oldEntity, SaveType saveType)
        {
            // supplying Product data for saving useractivity
            entity.Product = await _productRepository.GetByIdAsync(entity.ProductID.Value);

            if (oldEntity != null)
            {
                oldEntity.Product = await _productRepository.GetByIdAsync(oldEntity.ProductID.Value);
            }

            await base.PostSaveAction(entity, oldEntity, saveType);
        }

        protected override async Task PostSaveManyAction(
            IReadOnlyCollection<Allowance> entities,
            IReadOnlyCollection<Allowance> oldEntities,
            SaveType saveType,
            int currentlyLoggedInUserId)
        {
            if (!entities.Any()) return;

            var allowanceTypeIds = entities.Select(x => x.ProductID.Value).ToList();
            allowanceTypeIds.AddRange(oldEntities.Select(x => x.ProductID.Value).ToList());

            allowanceTypeIds = allowanceTypeIds.Distinct().ToList();

            var allowanceTypes = await _productRepository.GetManyByIdsAsync(allowanceTypeIds.ToArray());

            foreach (var entity in entities)
            {
                entity.Product = allowanceTypes.Where(x => x.RowID.Value == entity.ProductID).FirstOrDefault();
            }

            foreach (var entity in oldEntities)
            {
                entity.Product = allowanceTypes.Where(x => x.RowID.Value == entity.ProductID).FirstOrDefault();
            }

            await base.PostSaveManyAction(entities, oldEntities, saveType, currentlyLoggedInUserId);
        }

        protected override async Task RecordUpdate(Allowance newValue, Allowance oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.Type != oldValue.Type)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated type from '{oldValue.Type}' to '{newValue.Type}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.AllowanceFrequency != oldValue.AllowanceFrequency)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated frequency from '{oldValue.AllowanceFrequency}' to '{newValue.AllowanceFrequency}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.EffectiveStartDate != oldValue.EffectiveStartDate)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start date from '{oldValue.EffectiveStartDate.ToShortDateString()}' to '{newValue.EffectiveStartDate.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.EffectiveEndDate.ToString() != oldValue.EffectiveEndDate.ToString())
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated end date from '{oldValue.EffectiveEndDate?.ToShortDateString()}' to '{newValue.EffectiveEndDate?.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.Amount != oldValue.Amount)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated amount from '{oldValue.Amount}' to '{newValue.Amount}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });

            if (changes.Any())
            {
                await _userActivityRepository.CreateRecordAsync(
                    newValue.LastUpdBy.Value,
                    UserActivityName,
                    newValue.OrganizationID.Value,
                    UserActivityRepository.RecordTypeEdit,
                    changes);
            }
        }

        #endregion Overrides

        #region Private Methods

        private async Task CheckForClosedPayPeriod(List<Allowance> allowances, List<Allowance> oldAllowances)
        {
            // validate allowances that are for update
            int? organizationId = await ValidateStartDates(allowances, oldAllowances);

            var forUpdateAllowances = allowances
                .Where(x => !x.IsNewEntity)
                .ToArray();

            var ids = forUpdateAllowances.Select(x => x.RowID.Value).Distinct().ToArray();

            if (ids.Length == 0) return;

            // only end date can be updated if allowance is already used in a closed pay period
            var allowanceItems = await _allowanceRepository.GetAllowanceItemsWithPayPeriodAsync(ids);

            foreach (var allowance in forUpdateAllowances)
            {
                var oldAllowance = oldAllowances.Where(x => x.RowID == allowance.RowID).FirstOrDefault();

                var alreadyUsedInClosedPayroll = allowanceItems
                    .Where(x => x.AllowanceID == allowance.RowID)
                    .Where(x => x.PayPeriod.Status == PayPeriodStatus.Closed)
                    .Select(x => x.PayPeriod)
                    .Any();

                if (alreadyUsedInClosedPayroll)
                {
                    ValidateAllowableEdit(
                        allowance: allowance,
                        oldAllowance: oldAllowance);
                }
            }

            // end date cannot be in a closed pay period
            var endDates = allowances
                 .Where(x => !x.IsNewEntity)
                 .Where(x => x.EffectiveEndDate.HasValue)
                 .Select(x => x.EffectiveEndDate.Value)
                 .ToArray();

            if (await CheckIfDataIsWithinClosedPayPeriod(endDates, organizationId.Value, throwException: false))
                throw new BusinessLogicException("Cannot update End Date into a date in a Closed Payroll.");
        }

        private async Task<int?> ValidateStartDates(List<Allowance> allowances, List<Allowance> oldAllowances)
        {
            int? organizationId = null;
            allowances.ForEach(x =>
            {
                organizationId = ValidateOrganization(organizationId, x.OrganizationID);
            });

            // cannot create a new allowance or change the Start Date of an
            // existing allowance into a date on a "Closed" pay period.
            var validatableStartDates = allowances
                 .Where(allowance =>
                 {
                     return CheckIfStartDateNeedsToBeValidated(oldAllowances, allowance);
                 })
                 .Select(x => x.EffectiveStartDate)
                 .ToArray();

            await CheckIfDataIsWithinClosedPayPeriod(validatableStartDates, organizationId.Value);
            return organizationId;
        }

        private bool CheckIfStartDateNeedsToBeValidated(List<Allowance> oldAllowances, Allowance allowance)
        {
            // either a new allowance
            if (allowance.IsNewEntity) return true;

            // or an existing allowance where its Start Date is to be updated
            var oldAllowance = oldAllowances.Where(o => o.RowID == allowance.RowID).FirstOrDefault();

            if (allowance.EffectiveStartDate.ToMinimumHourValue() != oldAllowance.EffectiveStartDate.ToMinimumHourValue())
                return true;

            return false;
        }

        private void ValidateAllowableEdit(Allowance allowance, Allowance oldAllowance)
        {
            if (oldAllowance == null) return;

            if (allowance.ProductID != oldAllowance.ProductID ||
                allowance.AllowanceFrequency != oldAllowance.AllowanceFrequency ||
                allowance.EffectiveStartDate.ToMinimumHourValue() != oldAllowance.EffectiveStartDate.ToMinimumHourValue() ||
                allowance.Amount != oldAllowance.Amount)
            {
                throw new BusinessLogicException("The allowance is already used in a Closed Payroll. Only End Date can be edited.");
            }
        }

        #endregion Private Methods

        #region Queries

        public async Task<bool> CheckIfAlreadyUsedInClosedPayPeriodAsync(int allowanceId)
        {
            var payPeriods = await _allowanceRepository.GetPayPeriodsAsync(allowanceId);
            return CheckIfDataIsWithinClosedPayPeriod(payPeriods, throwException: false);
        }

        #endregion Queries
    }
}
