using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Interfaces;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services.Imports.Overtimes;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class OvertimeDataService : BaseDailyPayrollDataService<Overtime>
    {
        private const string UserActivityName = "Overtime";

        private readonly OvertimeRepository _overtimeRepository;
        private readonly UserActivityRepository _userActivityRepository;

        public OvertimeDataService(
            OvertimeRepository overtimeRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            PolicyHelper policy) :

            base(overtimeRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Overtime")
        {
            _overtimeRepository = overtimeRepository;
            _userActivityRepository = userActivityRepository;
        }

        public async Task DeleteManyAsync(IEnumerable<int> overtimeIds, int organizationId, int userId)
        {
            var overtimes = await _overtimeRepository.GetManyByIdsAsync(overtimeIds.ToArray());

            await _overtimeRepository.DeleteManyAsync(overtimeIds);

            foreach (var overtime in overtimes)
            {
                _userActivityRepository.RecordDelete(
                    userId,
                    UserActivityName,
                    entityId: overtime.RowID.Value,
                    organizationId: organizationId,
                    suffixIdentifier: CreateUserActivitySuffixIdentifier(overtime),
                    changedEmployeeId: overtime.EmployeeID.Value);
            }
        }

        public async Task<List<Overtime>> BatchApply(IReadOnlyCollection<OvertimeImportModel> validRecords, int organizationId, int userId)
        {
            List<Overtime> overtimes = new List<Overtime>();

            foreach (var ot in validRecords)
            {
                overtimes.Add(new Overtime()
                {
                    CreatedBy = userId,
                    EmployeeID = ot.EmployeeID,
                    OrganizationID = organizationId,
                    OTEndTimeFull = ot.EndTime.Value,
                    OTStartDate = ot.StartDate.Value,
                    OTStartTimeFull = ot.StartTime.Value,
                    Status = Overtime.StatusPending
                });
            }

            await _overtimeRepository.SaveManyAsync(overtimes);

            return overtimes;
        }

        public async Task GenerateOvertimeByShift(IEnumerable<IShift> modifiedShifts, List<int> employeeIds, int organizationId, int userId)
        {
            if (!modifiedShifts.Any())
                return;

            var timePeriod = new TimePeriod(modifiedShifts.Min(s => s.Date), modifiedShifts.Max(s => s.Date));

            (List<Overtime> saveOvertimes, List<Overtime> deleteOvertimes) =
                CreateOvertimesByShift(modifiedShifts, organizationId, userId, employeeIds, timePeriod);

            if (saveOvertimes.Any())
            {
                await SaveManyAsync(saveOvertimes);
            }

            if (deleteOvertimes.Any())
                await DeleteManyAsync(deleteOvertimes.Select(ot => ot.RowID.Value), organizationId, userId);
        }

        protected override Task SanitizeEntity(Overtime overtime, Overtime oldOvertime)
        {
            if (overtime.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (overtime.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (overtime.OTStartDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

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
                throw new BusinessLogicException("End Time cannot be equal to Start Time");

            overtime.UpdateEndDate();

            return Task.CompletedTask;
        }

        protected override Task PostSaveManyAction(IReadOnlyCollection<Overtime> entities, IReadOnlyCollection<Overtime> oldEntities, SaveType saveType)
        {
            switch (saveType)
            {
                case SaveType.Insert:

                    foreach (var item in entities)
                    {
                        _userActivityRepository.RecordAdd(
                            item.CreatedBy.Value,
                            UserActivityName,
                            entityId: item.RowID.Value,
                            organizationId: item.OrganizationID.Value,
                            suffixIdentifier: CreateUserActivitySuffixIdentifier(item),
                            changedEmployeeId: item.EmployeeID.Value);
                    }

                    break;

                case SaveType.Update:

                    RecordUpdate(entities, oldEntities);
                    break;

                case SaveType.Delete:

                    foreach (var item in entities)
                    {
                        _userActivityRepository.RecordDelete(
                            item.LastUpdBy.Value,
                            UserActivityName,
                            entityId: item.RowID.Value,
                            organizationId: item.OrganizationID.Value,
                            suffixIdentifier: CreateUserActivitySuffixIdentifier(item),
                            changedEmployeeId: item.EmployeeID.Value);
                    }
                    break;

                default:
                    break;
            }

            return Task.CompletedTask;
        }

        #region Private Methods

        private void RecordUpdate(IReadOnlyCollection<Overtime> updatedShifts, IReadOnlyCollection<Overtime> oldRecords)
        {
            foreach (var newValue in updatedShifts)
            {
                var changes = new List<UserActivityItem>();
                var entityName = UserActivityName.ToLower();

                var oldValue = oldRecords.Where(x => x.RowID == newValue.RowID).FirstOrDefault();
                if (oldValue == null) continue;

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
                    _userActivityRepository.CreateRecord(
                        newValue.LastUpdBy.Value,
                        UserActivityName,
                        newValue.OrganizationID.Value,
                        UserActivityRepository.RecordTypeEdit,
                        changes);
                }
            }
        }

        private static string CreateUserActivitySuffixIdentifier(Overtime overtime)
        {
            return $" with date '{overtime.OTStartDate.ToShortDateString()}' and time period '{overtime.OTStartTime.ToStringFormat("hh:mm tt")} to {overtime.OTEndTime.ToStringFormat("hh:mm tt")}'";
        }

        private (List<Overtime> saveOvertimes, List<Overtime> deleteOvertimes) CreateOvertimesByShift(
            IEnumerable<IShift> shiftSchedSaveList,
            int organizationId,
            int userId,
            List<int> employeeIds,
            TimePeriod timePeriod)
        {
            var saveOvertimes = new List<Overtime>();
            var deleteOvertimes = new List<Overtime>();

            var existingOvertimes = _overtimeRepository.GetByEmployeeIDsAndDatePeriod(organizationId, employeeIds, timePeriod);

            shiftSchedSaveList.ToList().ForEach(shiftModel =>
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
                    overtime = Overtime.NewOvertime(shiftModel, organizationId, userId);
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

                    overtime.LastUpdBy = userId;

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

        #endregion Private Methods
    }
}
