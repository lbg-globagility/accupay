using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services.Imports.Overtimes;
using AccuPay.Core.ValueObjects;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class OvertimeDataService : BaseDailyPayrollDataService<Overtime>, IOvertimeDataService
    {
        private const string UserActivityName = "Overtime";

        private readonly IOvertimeRepository _overtimeRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public OvertimeDataService(
            IOvertimeRepository overtimeRepository,
            IRoleRepository roleRepository,
            IOrganizationRepository organizationRepository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(overtimeRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Overtime")
        {
            _overtimeRepository = overtimeRepository;
            _roleRepository = roleRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task DeleteManyAsync(IEnumerable<int> overtimeIds, int currentlyLoggedInUserId)
        {
            var overtimes = await _overtimeRepository.GetManyByIdsAsync(overtimeIds.ToArray());

            await _overtimeRepository.DeleteManyAsync(overtimeIds);

            await PostDeleteManyAction(overtimes.ToList(), currentlyLoggedInUserId);
        }

        public async Task<List<Overtime>> BatchApply(IReadOnlyCollection<OvertimeImportModel> validRecords, int organizationId, int currentlyLoggedInUserId)
        {
            List<Overtime> overtimes = new List<Overtime>();

            foreach (var ot in validRecords)
            {
                var overtime = Overtime.NewOvertime(
                    organizationId: organizationId,
                    employeeId: ot.EmployeeID.Value,
                    startDate: ot.StartDate.Value,
                    startTime: ot.StartTime.Value.TimeOfDay,
                    endTime: ot.EndTime.Value.TimeOfDay,
                    status: Overtime.StatusPending);

                overtimes.Add(overtime);
            }

            await SaveManyAsync(overtimes, currentlyLoggedInUserId);

            return overtimes;
        }

        public async Task GenerateOvertimeByShift(IEnumerable<IShift> modifiedShifts, List<int> employeeIds, int organizationId, int currentlyLoggedInUserId)
        {
            if (!modifiedShifts.Any())
                return;

            var timePeriod = new TimePeriod(modifiedShifts.Min(s => s.Date), modifiedShifts.Max(s => s.Date));

            (List<Overtime> saveOvertimes, List<Overtime> deleteOvertimes) =
                CreateOvertimesByShift(modifiedShifts, organizationId, employeeIds, timePeriod);

            if (saveOvertimes.Any())
            {
                await SaveManyAsync(saveOvertimes, currentlyLoggedInUserId);
            }

            if (deleteOvertimes.Any())
            {
                await DeleteManyAsync(deleteOvertimes.Select(ot => ot.RowID.Value), currentlyLoggedInUserId);
            }
        }

        #region Overrides

        protected override string GetUserActivityName(Overtime overtime)
        {
            return UserActivityName;
        }

        protected override string CreateUserActivitySuffixIdentifier(Overtime overtime)
        {
            return $" with date '{overtime.OTStartDate.ToShortDateString()}' and time period '{overtime.OTStartTime.ToStringFormat("hh:mm tt")} to {overtime.OTEndTime.ToStringFormat("hh:mm tt")}'";
        }

        protected override async Task SanitizeEntity(Overtime overtime, Overtime oldOvertime, int currentlyLoggedInUserId)
        {
            await base.SanitizeEntity(
                entity: overtime,
                oldEntity: oldOvertime,
                currentlyLoggedInUserId: currentlyLoggedInUserId);

            if (overtime.OTStartDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753.");

            if (!_policy.UseMassOvertime)
                if (overtime.OTStartTime == null)
                    throw new BusinessLogicException("Start Time is required.");

            if (overtime.OTEndTime == null)
                throw new BusinessLogicException("End Time is required.");

            if (new string[] { Overtime.StatusPending, Overtime.StatusApproved }
                .Contains(overtime.Status) == false)
            {
                throw new BusinessLogicException("Status is not valid.");
            }

            if (overtime.OTStartTime.HasValue)
                overtime.OTStartTime = overtime.OTStartTime.Value.StripSeconds();

            overtime.OTEndTime = overtime.OTEndTime.Value.StripSeconds();

            if (overtime.OTStartTime == overtime.OTEndTime)
            {
                throw new BusinessLogicException("End Time cannot be equal to Start Time");
            }

            overtime.UpdateEndDate();
        }

        protected override async Task RecordUpdate(Overtime newValue, Overtime oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.OTStartDate != oldValue.OTStartDate)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start date from '{oldValue.OTStartDate.ToShortDateString()}' to '{newValue.OTStartDate.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.OTStartTime != oldValue.OTStartTime)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start time from '{oldValue.OTStartTime.ToStringFormat("hh:mm tt")}' to '{newValue.OTStartTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.OTEndTime != oldValue.OTEndTime)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated end time from '{oldValue.OTEndTime.ToStringFormat("hh:mm tt")}' to '{newValue.OTEndTime.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
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

        #endregion Overrides

        #region Private Methods

        private (List<Overtime> saveOvertimes, List<Overtime> deleteOvertimes) CreateOvertimesByShift(
            IEnumerable<IShift> shiftSaveList,
            int organizationId,
            List<int> employeeIds,
            TimePeriod timePeriod)
        {
            var saveOvertimes = new List<Overtime>();
            var deleteOvertimes = new List<Overtime>();

            var existingOvertimes = _overtimeRepository.GetByEmployeeIDsAndDatePeriod(organizationId, employeeIds, timePeriod);

            shiftSaveList.ToList().ForEach(shiftModel =>
            {
                var employeeID = shiftModel.EmployeeId;
                var dateValue = shiftModel.Date;

                var overtimesThisDay = existingOvertimes
                    .Where(ot => ot.EmployeeID == employeeID)
                    .Where(ot => ot.OTStartDate == dateValue)
                    .ToList();

                var overtime = overtimesThisDay.FirstOrDefault();
                if (overtime == null)
                {
                    overtime = Overtime.NewOvertime(shiftModel, organizationId);
                }
                else
                {
                    overtime = overtime.CloneJson();
                }

                var hasShiftPeriod = shiftModel.StartTime.HasValue && shiftModel.EndTime.HasValue;
                if (hasShiftPeriod)
                {
                    var shiftTimePeriod = TimePeriod.FromTime(shiftModel.StartTime.Value, shiftModel.EndTime.Value, shiftModel.Date);
                    var validOTStartTime = _policy.ShiftBasedAutomaticOvertimePolicy.GetExpectedEndTime(shiftTimePeriod.Start, shiftModel.BreakLength);

                    overtime.OTStartTimeFull = validOTStartTime;
                    overtime.OTEndTimeFull = validOTStartTime;

                    var shiftPeriod = new TimePeriod(shiftTimePeriod.Start, shiftTimePeriod.End);
                    if (shiftPeriod.TotalHours >= (_policy.ShiftBasedAutomaticOvertimePolicy.DefaultWorkHoursAndMinimumOTHours + shiftModel.BreakLength))
                    {
                        overtime.OTEndTimeFull = shiftTimePeriod.End;
                    }
                }

                var overtimeId = 0;

                if (overtime.OTStartTime != overtime.OTEndTime && hasShiftPeriod)
                {
                    if (overtime.RowID.HasValue)
                        overtimeId = overtime.RowID.Value;

                    saveOvertimes.Add(overtime);
                }

                var discardOthers = overtimesThisDay
                    .Where(ot => overtimeId != ot.RowID)
                    .ToList();
                discardOthers.ForEach(ot => deleteOvertimes.Add(ot));
            });

            return (
                saveOvertimes: saveOvertimes,
                deleteOvertimes: deleteOvertimes);
        }

        protected override async Task AdditionalSaveManyValidation(List<Overtime> entities, List<Overtime> oldEntities, SaveType saveType)
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
        }

        private async Task CheckForClosedPayPeriod(List<Overtime> entities, List<Overtime> oldEntities, List<UserRole> userRoles = null)
        {
            List<int?> organizationIds = await ValidateStartDates(entities, oldEntities, userRoles: userRoles);
        }

        private async Task<List<int?>> ValidateStartDates(List<Overtime> entities, List<Overtime> oldEntities, List<UserRole> userRoles)
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
                 .Select(x => x.OTStartDate)
                 .ToArray();

            foreach (var orgId in organizationIds)
                await CheckIfDataIsWithinClosedPayPeriod(validatableStartDates, orgId.Value);

            return organizationIds;
        }

        private bool CheckIfStartDateNeedsToBeValidated(List<Overtime> oldEntities, Overtime officialBusiness)
        {
            if (officialBusiness.IsNewEntity) return true;

            var oldAllowance = oldEntities.Where(o => o.RowID == officialBusiness.RowID).FirstOrDefault();

            if (officialBusiness.OTStartDate.ToMinimumHourValue() != oldAllowance.OTStartDate.ToMinimumHourValue())
                return true;

            return false;
        }

        #endregion Private Methods
    }
}
