using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Repositories;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class DivisionDataService : BaseOrganizationDataService<Division>
    {
        private const string UserActivityDivisionName = "Division";

        private const string UserActivityDivisionLocationName = "Division Location";

        private readonly DivisionRepository _divisionRepository;
        private readonly ListOfValueRepository _listOfValueRepository;
        private readonly PositionRepository _positionRepository;

        public DivisionDataService(
            DivisionRepository divisionRepository,
            ListOfValueRepository listOfValueRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy,
            PositionRepository positionRepository) :

            base(divisionRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Division")
        {
            _divisionRepository = divisionRepository;
            _listOfValueRepository = listOfValueRepository;
            _positionRepository = positionRepository;
        }

        public async Task<Division> GetOrCreateDefaultDivisionAsync(int organizationId, int changedByUserId)
        {
            var divisionRepository = new DivisionRepository(_context);
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var defaultParentDivision = await _context.Divisions
                        .Where(x => x.OrganizationID == organizationId)
                        .Where(x => x.Name.Trim().ToLower() == Division.DefaultLocationName.ToTrimmedLowerCase())
                        .Where(x => x.IsRoot)
                        .FirstOrDefaultAsync();

                    if (defaultParentDivision == null)
                    {
                        defaultParentDivision = Division.NewDivision(organizationId);

                        defaultParentDivision.Name = Division.DefaultLocationName;
                        defaultParentDivision.ParentDivisionID = null;

                        await SanitizeEntity(defaultParentDivision, null, changedByUserId);
                        await divisionRepository.SaveAsync(defaultParentDivision);
                        // querying the new default parent division from here can already
                        // get the new row data. This can replace the context.local in leaverepository
                    }
                    if (defaultParentDivision?.RowID == null)
                        throw new Exception("Cannot create default division location.");

                    var defaultDivision = await _context.Divisions
                        .Where(x => x.OrganizationID == organizationId)
                        .Where(x => x.Name.Trim().ToLower() == Division.DefaultDivisionName.ToTrimmedLowerCase())
                        .Where(x => x.ParentDivisionID == defaultParentDivision.RowID)
                        .FirstOrDefaultAsync();

                    if (defaultDivision == null)
                    {
                        defaultDivision = Division.NewDivision(organizationId);

                        defaultDivision.Name = Division.DefaultDivisionName;
                        defaultDivision.ParentDivisionID = defaultParentDivision.RowID;

                        await SanitizeEntity(defaultDivision, null, changedByUserId);
                        await divisionRepository.SaveAsync(defaultDivision);
                    }

                    transaction.Commit();

                    return defaultDivision;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<ICollection<string>> GetSchedulesAsync()
        {
            ICollection<ListOfValue> listOfValues = await _listOfValueRepository.GetDeductionSchedulesAsync();
            return listOfValues.Select(x => x.DisplayValue).ToList();
        }

        #region Override

        public async override Task DeleteAsync(int divisionId, int currentlyLoggedInUserId)
        {
            var division = await _divisionRepository.GetByIdWithParentAsync(divisionId);

            if (division == null)
                throw new BusinessLogicException("Division does not exists.");

            // TODO: move this to repositories
            if (_context.AgencyFees.Any(a => a.DivisionID == divisionId))
                throw new BusinessLogicException("Division already has agency fees therefore cannot be deleted.");
            else if (_context.Divisions.Any(d => d.ParentDivisionID == divisionId))
                throw new BusinessLogicException("Division already has child divisions therefore cannot be deleted.");
            else if (_context.Positions.Any(p => p.DivisionID == divisionId))
                throw new BusinessLogicException("Division already has positions therefore cannot be deleted.");

            await base.DeleteAsync(
                id: divisionId,
                currentlyLoggedInUserId: currentlyLoggedInUserId);
        }

        protected override string GetUserActivityName(Division division)
        {
            return division.IsRoot ?
                UserActivityDivisionLocationName :
                UserActivityDivisionName;
        }

        protected override string CreateUserActivitySuffixIdentifier(Division division)
        {
            return $" with name '{division.Name}'";
        }

        protected override async Task PostDeleteAction(Division entity, int currentlyLoggedInUserId)
        {
            // supplying data for saving useractivity
            await SupplyNavigationData(entity);

            await base.PostDeleteAction(entity, currentlyLoggedInUserId);
        }

        protected override async Task PostSaveAction(Division entity, Division oldEntity, SaveType saveType)
        {
            // supplying data for saving useractivity
            await SupplyNavigationData(entity);

            await SupplyNavigationData(oldEntity);

            await base.PostSaveAction(entity, oldEntity, saveType);
        }

        protected override async Task SanitizeEntity(Division division, Division oldDivision, int changedByUserId)
        {
            await base.SanitizeEntity(
                entity: division,
                oldEntity: oldDivision,
                currentlyLoggedInUserId: changedByUserId);

            var doesExistQuery = _context.Divisions
                .Where(d => d.Name.Trim().ToLower() == division.Name.ToTrimmedLowerCase())
                .Where(d => d.ParentDivisionID == division.ParentDivisionID)
                .Where(d => d.OrganizationID == division.OrganizationID);

            if (division.IsNewEntity == false)
            {
                doesExistQuery = doesExistQuery.Where(d => division.RowID != d.RowID);
            }

            if (await doesExistQuery.AnyAsync())
            {
                if (division.IsRoot)
                    throw new BusinessLogicException($"Division location already exists.");
                else
                    throw new BusinessLogicException($"Division name already exists under the selected division location.");
            }
        }

        protected override async Task RecordUpdate(Division newValue, Division oldValue)
        {
            var userActivityName = GetUserActivityName(oldValue);

            var changes = new List<UserActivityItem>();
            var entityName = userActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.DivisionType != oldValue.DivisionType)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated type from '{oldValue.DivisionType}' to '{newValue.DivisionType}' {suffixIdentifier}"
                });
            if (newValue.ParentDivisionID != oldValue.ParentDivisionID)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated parent from '{oldValue.ParentDivision?.Name}' to '{newValue.ParentDivision?.Name}' {suffixIdentifier}"
                });
            }
            if (newValue.Name != oldValue.Name)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated name from '{oldValue.Name}' to '{newValue.Name}' {suffixIdentifier}"
                });
            if (newValue.TradeName != oldValue.TradeName)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated trade name from '{oldValue.TradeName}' to '{newValue.TradeName}' {suffixIdentifier}"
                });
            if (newValue.TINNo != oldValue.TINNo)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated tin from '{oldValue.TINNo}' to '{newValue.TINNo}' {suffixIdentifier}"
                });
            if (newValue.BusinessAddress != oldValue.BusinessAddress)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated business address from '{oldValue.BusinessAddress}' to '{newValue.BusinessAddress}' {suffixIdentifier}"
                });
            if (newValue.DivisionHeadID != oldValue.DivisionHeadID)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated division head from '{oldValue.DivisionHead?.Name}' to '{newValue.DivisionHead?.Name}' {suffixIdentifier}"
                });
            }
            if (newValue.GracePeriod != oldValue.GracePeriod)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated grace period from '{oldValue.GracePeriod}' to '{newValue.GracePeriod}' {suffixIdentifier}"
                });
            if (newValue.WorkDaysPerYear != oldValue.WorkDaysPerYear)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated work days per year from '{oldValue.WorkDaysPerYear}' to '{newValue.WorkDaysPerYear}' {suffixIdentifier}"
                });
            if (newValue.AutomaticOvertimeFiling != oldValue.AutomaticOvertimeFiling)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated automatic overtime filing from '{oldValue.AutomaticOvertimeFiling}' to '{newValue.AutomaticOvertimeFiling}' {suffixIdentifier}"
                });
            if (newValue.MainPhone != oldValue.MainPhone)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated main phone from '{oldValue.MainPhone}' to '{newValue.MainPhone}' {suffixIdentifier}"
                });
            if (newValue.AltPhone != oldValue.AltPhone)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated alternate phone from '{oldValue.AltPhone}' to '{newValue.AltPhone}' {suffixIdentifier}"
                });
            if (newValue.EmailAddress != oldValue.EmailAddress)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated email address from '{oldValue.EmailAddress}' to '{newValue.EmailAddress}' {suffixIdentifier}"
                });
            if (newValue.AltEmailAddress != oldValue.AltEmailAddress)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated alternate email address from '{oldValue.AltEmailAddress}' to '{newValue.AltEmailAddress}' {suffixIdentifier}"
                });
            if (newValue.ContactName != oldValue.ContactName)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated contact name from '{oldValue.ContactName}' to '{newValue.ContactName}' {suffixIdentifier}"
                });
            if (newValue.FaxNumber != oldValue.FaxNumber)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated fax number from '{oldValue.FaxNumber}' to '{newValue.FaxNumber}' {suffixIdentifier}"
                });
            if (newValue.URL != oldValue.URL)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated URL from '{oldValue.URL}' to '{newValue.URL}' {suffixIdentifier}"
                });
            if (newValue.PhilHealthDeductionSchedule != oldValue.PhilHealthDeductionSchedule)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated PhilHealth deduction schedule from '{oldValue.PhilHealthDeductionSchedule}' to '{newValue.PhilHealthDeductionSchedule}' {suffixIdentifier}"
                });
            if (newValue.SssDeductionSchedule != oldValue.SssDeductionSchedule)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated SSS deduction schedule from '{oldValue.SssDeductionSchedule}' to '{newValue.SssDeductionSchedule}' {suffixIdentifier}"
                });
            if (newValue.PagIBIGDeductionSchedule != oldValue.PagIBIGDeductionSchedule)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated PagIbig deduction schedule from '{oldValue.PagIBIGDeductionSchedule}' to '{newValue.PagIBIGDeductionSchedule}' {suffixIdentifier}"
                });
            if (newValue.WithholdingTaxSchedule != oldValue.WithholdingTaxSchedule)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated withholding tax deduction schedule from '{oldValue.WithholdingTaxSchedule}' to '{newValue.WithholdingTaxSchedule}' {suffixIdentifier}"
                });
            if (newValue.AgencyPhilHealthDeductionSchedule != oldValue.AgencyPhilHealthDeductionSchedule)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated PhilHealth deduction schedule w/ agency from '{oldValue.AgencyPhilHealthDeductionSchedule}' to '{newValue.AgencyPhilHealthDeductionSchedule}' {suffixIdentifier}"
                });
            if (newValue.AgencySssDeductionSchedule != oldValue.AgencySssDeductionSchedule)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated SSS deduction schedule w/ agency from '{oldValue.AgencySssDeductionSchedule}' to '{newValue.AgencySssDeductionSchedule}' {suffixIdentifier}"
                });
            if (newValue.AgencyPagIBIGDeductionSchedule != oldValue.AgencyPagIBIGDeductionSchedule)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated PagIbig deduction schedule w/ agency from '{oldValue.AgencyPagIBIGDeductionSchedule}' to '{newValue.AgencyPagIBIGDeductionSchedule}' {suffixIdentifier}"
                });
            if (newValue.AgencyWithholdingTaxSchedule != oldValue.AgencyWithholdingTaxSchedule)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated withholding tax deduction schedule w/ agency from '{oldValue.AgencyWithholdingTaxSchedule}' to '{newValue.AgencyWithholdingTaxSchedule}' {suffixIdentifier}"
                });
            if (newValue.DefaultVacationLeave != oldValue.DefaultVacationLeave)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated vacation leave from '{oldValue.DefaultVacationLeave}' to '{newValue.DefaultVacationLeave}' {suffixIdentifier}"
                });
            if (newValue.DefaultSickLeave != oldValue.DefaultSickLeave)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated sick leave from '{oldValue.DefaultSickLeave}' to '{newValue.DefaultSickLeave}' {suffixIdentifier}"
                });
            if (newValue.DefaultOtherLeave != oldValue.DefaultOtherLeave)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated other leave from '{oldValue.DefaultOtherLeave}' to '{newValue.DefaultOtherLeave}' {suffixIdentifier}"
                });

            if (changes.Any())
            {
                await _userActivityRepository.CreateRecordAsync(
                    newValue.LastUpdBy.Value,
                    userActivityName,
                    newValue.OrganizationID.Value,
                    UserActivityRepository.RecordTypeEdit,
                    changes);
            }
        }

        #endregion Override

        private async Task SupplyNavigationData(Division entity)
        {
            if (entity == null) return;

            if (entity.ParentDivisionID == null)
            {
                entity.ParentDivision = null;
            }
            else
            {
                entity.ParentDivision = await _divisionRepository.GetByIdAsync(entity.ParentDivisionID.Value);
            }

            if (entity.DivisionHeadID == null)
            {
                entity.DivisionHead = null;
            }
            else
            {
                entity.DivisionHead = await _positionRepository.GetByIdAsync(entity.DivisionHeadID.Value);
            }
        }
    }
}
