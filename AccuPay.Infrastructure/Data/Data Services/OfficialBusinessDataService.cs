using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services.Imports.OfficialBusiness;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class OfficialBusinessDataService : BaseDailyPayrollDataService<OfficialBusiness>, IOfficialBusinessDataService
    {
        private const string UserActivityName = "Official Business";
        private readonly IRoleRepository _roleRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public OfficialBusinessDataService(
            IOfficialBusinessRepository officialBusinessRepository,
            IRoleRepository roleRepository,
            IOrganizationRepository organizationRepository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(officialBusinessRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Official Business",
                entityNamePlural: "Official Businesses")
        {
            _roleRepository = roleRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task<List<OfficialBusiness>> BatchApply(IReadOnlyCollection<OfficialBusinessImportModel> validRecords, int organizationId, int currentlyLoggedInUserId)
        {
            List<OfficialBusiness> officialBusinesses = new List<OfficialBusiness>();

            foreach (var ob in validRecords)
            {
                officialBusinesses.Add(new OfficialBusiness()
                {
                    EmployeeID = ob.EmployeeID,
                    OrganizationID = organizationId,
                    EndTimeFull = ob.EndTime.Value,
                    StartDate = ob.StartDate.Value,
                    StartTimeFull = ob.StartTime.Value,
                    Status = Overtime.StatusPending
                });
            }

            await SaveManyAsync(officialBusinesses, currentlyLoggedInUserId);

            return officialBusinesses;
        }

        #region Overrides

        protected override string GetUserActivityName(OfficialBusiness officialBusiness)
        {
            return UserActivityName;
        }

        protected override string CreateUserActivitySuffixIdentifier(OfficialBusiness officialBusiness)
        {
            return $" with date '{officialBusiness.StartDate.ToShortDateString()}'";
        }

        protected override async Task SanitizeEntity(OfficialBusiness officialBusiness, OfficialBusiness oldOfficialBusiness, int changedByUserId)
        {
            await base.SanitizeEntity(
                entity: officialBusiness,
                oldEntity: oldOfficialBusiness,
                currentlyLoggedInUserId: changedByUserId);

            if (officialBusiness.StartDate == null)
                throw new BusinessLogicException("Start Date is required.");

            if (officialBusiness.StartDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753.");

            if (officialBusiness.StartTime == null)
                throw new BusinessLogicException("Start Time is required.");

            if (officialBusiness.EndTime == null)
                throw new BusinessLogicException("End Time is required.");

            if (new string[] { OfficialBusiness.StatusPending, OfficialBusiness.StatusApproved }
                .Contains(officialBusiness.Status) == false)
            {
                throw new BusinessLogicException("Status is not valid.");
            }

            var doesExistQuery = _context.OfficialBusinesses
                .Where(l => l.EmployeeID == officialBusiness.EmployeeID)
                .Where(l => l.StartDate.Value.Date == officialBusiness.StartDate.Value.Date);

            if (officialBusiness.IsNewEntity == false)
            {
                doesExistQuery = doesExistQuery.Where(l => officialBusiness.RowID != l.RowID);
            }

            if (await doesExistQuery.AnyAsync())
                throw new BusinessLogicException($"Employee already has an OB for {officialBusiness.StartDate.Value.ToShortDateString()}");

            officialBusiness.StartTime = officialBusiness.StartTime.Value.StripSeconds();
            officialBusiness.EndTime = officialBusiness.EndTime.Value.StripSeconds();

            if (officialBusiness.StartTime == officialBusiness.EndTime)
                throw new BusinessLogicException("End Time cannot be equal to Start Time");

            officialBusiness.UpdateEndDate();
        }

        protected override async Task RecordUpdate(OfficialBusiness newValue, OfficialBusiness oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.StartDate != oldValue.StartDate)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start date from '{oldValue.StartDate?.ToShortDateString()}' to '{newValue.StartDate?.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.StartTime != oldValue.StartTime)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start time from '{oldValue.StartTime?.ToStringFormat("hh:mm tt")}' to '{newValue.StartTime?.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.EndTime != oldValue.EndTime)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated end time from '{oldValue.EndTime?.ToStringFormat("hh:mm tt")}' to '{newValue.EndTime?.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.Reason != oldValue.Reason)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated reason from '{oldValue.Reason}' to '{newValue.Reason}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.Comments != oldValue.Comments)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated comments from '{oldValue.Comments}' to '{newValue.Comments}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.Status != oldValue.Status)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated status from '{oldValue.Status}' to '{newValue.Status}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });

            if (changes.Any())
            {
                await _userActivityRepository.CreateRecordAsync(
                    newValue.LastUpdBy.Value,
                    UserActivityName,
                    newValue.OrganizationID.Value,
                    UserActivity.RecordTypeEdit,
                    changes);
            }
        }

        protected override async Task AdditionalSaveManyValidation(List<OfficialBusiness> entities, List<OfficialBusiness> oldEntities, SaveType saveType)
        {
            if (_policy.ImportPolicy.IsOpenToAllImportMethod &&
                (saveType == SaveType.Insert || saveType == SaveType.Update))
            {
                var createUserId = entities.Where(a => a.CreatedBy.HasValue).Select(a => a.CreatedBy).FirstOrDefault();
                var updateUserId = entities.Where(a => a.LastUpdBy.HasValue).Select(a => a.LastUpdBy).FirstOrDefault();
                var userId = updateUserId ?? createUserId;
                var userRoles = await _roleRepository.GetUserRolesByUserAsync(userId: userId ?? 0);
                await CheckForClosedPayPeriod(entities, oldEntities, userRoles: userRoles);
            }
            else
            {
                await CheckForClosedPayPeriod(entities, oldEntities);
            }

            //await AdditionalSaveManyValidation(entities, oldEntities, saveType);
        }

        private async Task CheckForClosedPayPeriod(List<OfficialBusiness> entities, List<OfficialBusiness> oldEntities, List<UserRole> userRoles = null)
        {
            List<int?> organizationIds = await ValidateStartDates(entities, oldEntities, userRoles: userRoles);
        }

        private async Task<List<int?>> ValidateStartDates(List<OfficialBusiness> entities, List<OfficialBusiness> oldEntities, List<UserRole> userRoles)
        {
            var organizationIds = new List<int?>();
            if (userRoles != null && userRoles.Any())
            {
                foreach (var x in entities)
                {
                    var userRole = userRoles.FirstOrDefault(ur => ur.OrganizationId == x.OrganizationID);

                    var hasCreatePermission = userRole.Role.HasPermission(permissionName: PermissionConstant.OFFICIALBUSINESS, action: "create");
                    if (!hasCreatePermission)
                    {
                        var organization = await _organizationRepository.GetByIdAsync(x.OrganizationID.Value);
                        throw new BusinessLogicException($"Insufficient permission. You cannot create data for company: {organization.Name}.");
                    }

                    var hasUpdatePermission = userRole.Role.HasPermission(permissionName: PermissionConstant.OFFICIALBUSINESS, action: "update");
                    if (!hasUpdatePermission)
                    {
                        var organization = await _organizationRepository.GetByIdAsync(x.OrganizationID.Value);
                        throw new BusinessLogicException($"Insufficient permission. You cannot update data for company: {organization.Name}.");
                    }

                    organizationIds.Add(x.OrganizationID);
                }
            }
            else
            {
                int? organizationId = null;
                entities.ForEach(x =>
                {
                    organizationId = ValidateOrganization(organizationId, x.OrganizationID);
                });
                organizationIds = new List<int?>() { organizationId };
            }

            var validatableStartDates = entities
                 .Where(officialBusiness =>
                 {
                     return CheckIfStartDateNeedsToBeValidated(oldEntities, officialBusiness);
                 })
                 .Select(x => x.StartDate.Value)
                 .ToArray();

            foreach (var orgId in organizationIds)
                await CheckIfDataIsWithinClosedPayPeriod(validatableStartDates, orgId.Value);

            return organizationIds;
        }

        #endregion Overrides

        private bool CheckIfStartDateNeedsToBeValidated(List<OfficialBusiness> oldEntities, OfficialBusiness officialBusiness)
        {
            if (officialBusiness.IsNewEntity) return true;

            var oldOfficialBusiness = oldEntities.Where(o => o.RowID == officialBusiness.RowID).FirstOrDefault();

            if (officialBusiness.StartDate.ToMinimumHourValue() != oldOfficialBusiness.StartDate.ToMinimumHourValue())
                return true;

            return false;
        }
    }
}
