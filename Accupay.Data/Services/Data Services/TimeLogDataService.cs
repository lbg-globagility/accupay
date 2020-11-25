using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class TimeLogDataService : BaseSavableDataService<TimeLog>
    {
        private const string UserActivityName = "Time Log";

        private readonly TimeLogRepository _timeLogRepository;
        private readonly UserActivityRepository _userActivityRepository;
        private readonly BranchRepository _branchRepository;

        public TimeLogDataService(
            TimeLogRepository timeLogRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            BranchRepository branchRepository,
            PayrollContext context,
            PolicyHelper policy) :

            base(timeLogRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Time log")
        {
            _timeLogRepository = timeLogRepository;
            _userActivityRepository = userActivityRepository;
            _branchRepository = branchRepository;
        }

        #region Save

        public async Task SaveImportAsync(List<TimeLog> timeLogs)
        {
            string importId = Guid.NewGuid().ToString();

            foreach (var timeLog in timeLogs)
            {
                timeLog.TimeentrylogsImportID = importId;
            }

            await SaveManyAsync(timeLogs);
        }

        protected override Task SanitizeEntity(TimeLog timeLog, TimeLog oldTimeLog)
        {
            if (timeLog == null)
                throw new BusinessLogicException("Invalid data.");

            if (timeLog.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (timeLog.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (timeLog.LogDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

            if (timeLog.TimeIn == null && timeLog.TimeOut == null)
                throw new BusinessLogicException("Time-in and Time-out cannot be both empty.");

            if (timeLog.IsNewEntity && timeLog.CreatedBy == null)
                throw new BusinessLogicException("Created By is required.");

            if (!timeLog.IsNewEntity && timeLog.LastUpdBy == null)
                throw new BusinessLogicException("Last Updated By is required.");

            if (timeLog.TimeIn != null && timeLog.TimeStampIn == null)
            {
                timeLog.TimeStampIn = timeLog.TimeInFull;
            }

            if (timeLog.TimeOut != null && timeLog.TimeStampOut == null)
            {
                timeLog.TimeStampOut = timeLog.TimeOutFull;
            }

            return Task.CompletedTask;
        }

        protected override async Task AdditionalSaveManyValidation(List<TimeLog> entities, List<TimeLog> oldEntities, SaveType saveType)
        {
            var organizationEntities = entities.GroupBy(x => x.OrganizationID);

            foreach (var organization in organizationEntities)
            {
                if (organization.Key != null)
                    await CheckIfDataIsWithinClosedPayPeriod(organization.ToList().Select(x => x.LogDate).Distinct(), organization.Key.Value);
            }
        }

        protected override async Task PostSaveManyAction(List<TimeLog> entities, List<TimeLog> oldEntities, SaveType saveType)
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

                    var branches = await _branchRepository.GetAllAsync();
                    RecordUpdate(entities, oldEntities, branches.ToList());
                    break;

                case SaveType.Delete:

                    // user can add multiple time logs in a day specially when they import multiple times.
                    var groupedDeletedTimeLogs = entities.GroupBy(x => new { x.EmployeeID, x.LogDate });

                    foreach (var group in groupedDeletedTimeLogs)
                    {
                        foreach (var item in group.ToList())
                        {
                            _userActivityRepository.RecordDelete(
                                item.LastUpdBy.Value,
                                UserActivityName,
                                entityId: item.RowID.Value,
                                organizationId: item.OrganizationID.Value,
                                suffixIdentifier: CreateUserActivitySuffixIdentifier(item),
                                changedEmployeeId: item.EmployeeID.Value);
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        #endregion Save

        private void RecordUpdate(List<TimeLog> updatedTimeLogs, List<TimeLog> oldRecords, List<Branch> branches)
        {
            foreach (var item in updatedTimeLogs)
            {
                List<UserActivityItem> changes = new List<UserActivityItem>();
                var entityName = UserActivityName.ToLower();
                var oldValue = oldRecords
                    .Where(tl => tl.EmployeeID.Value == item.EmployeeID.Value)
                    .Where(tl => tl.LogDate == item.LogDate)
                    .FirstOrDefault();

                var suffixIdentifier = $"of time log{CreateUserActivitySuffixIdentifier(item)}.";

                if (item.TimeIn != oldValue.TimeIn)
                {
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = item.RowID.Value,
                        Description = $"Updated time in from '{oldValue.TimeIn.ToStringFormat("hh:mm tt")}' to '{item.TimeIn.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                        ChangedEmployeeId = item.EmployeeID.Value
                    });
                }
                if (item.TimeOut != oldValue.TimeOut)
                {
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = item.RowID.Value,
                        Description = $"Updated time out from '{oldValue.TimeOut.ToStringFormat("hh:mm tt")}' to '{item.TimeOut.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                        ChangedEmployeeId = item.EmployeeID.Value
                    });
                }

                var currentDateOut = item.TimeStampOut == null ? item.TimeStampOut : item.TimeStampOut.Value.Date;
                var oldDateOut = oldValue.TimeStampOut == null ? oldValue.TimeStampOut : oldValue.TimeStampOut.Value.Date;
                if (currentDateOut.NullableEquals(oldDateOut) == false)
                {
                    // TimeStampOut is null by default. It means TimeStampOut is equals to LogDate
                    var dontSave = oldValue.TimeStampOut == null && item.TimeStampOut != null && item.TimeStampOut.Value.Date == item.LogDate.Date;

                    if (dontSave == false)
                    {
                        changes.Add(new UserActivityItem()
                        {
                            EntityId = item.RowID.Value,
                            Description = $"Updated date out from '{oldValue.TimeStampOut.ToShortDateString()}' to '{item.TimeStampOut.ToShortDateString()}' {suffixIdentifier}",
                            ChangedEmployeeId = item.EmployeeID.Value
                        });
                    }
                }
                if (item.BranchID != oldValue.BranchID)
                {
                    var oldBranch = "";
                    var newBranch = "";

                    if (oldValue.BranchID.HasValue)
                        oldBranch = branches.Where(x => x.RowID == oldValue.BranchID).FirstOrDefault()?.Name;
                    if (item.BranchID.HasValue)
                        newBranch = branches.Where(x => x.RowID == item.BranchID).FirstOrDefault()?.Name;

                    changes.Add(new UserActivityItem()
                    {
                        EntityId = item.RowID.Value,
                        Description = $"Updated branch from '{oldBranch}' to '{newBranch}' {suffixIdentifier}",
                        ChangedEmployeeId = item.EmployeeID.Value
                    });
                }

                if (changes.Any())
                {
                    _userActivityRepository.CreateRecord(
                        item.LastUpdBy.Value,
                        UserActivityName,
                        item.OrganizationID.Value,
                        UserActivityRepository.RecordTypeEdit,
                        changes);
                }
            }
        }

        private static string CreateUserActivitySuffixIdentifier(TimeLog log)
        {
            return $" with date '{log.LogDate.ToShortDateString()}'";
        }
    }
}
