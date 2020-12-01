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
    public class TimeLogDataService : BaseOrganizationDataService<TimeLog>
    {
        private const string UserActivityName = "Time Log";

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
                userActivityRepository,
                context,
                policy,
                entityName: "Time log")
        {
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

        #endregion Save

        #region Overrides

        protected override string GetUserActivityName(TimeLog leave) => UserActivityName;

        protected override string CreateUserActivitySuffixIdentifier(TimeLog log) =>
            $" with date '{log.LogDate.ToShortDateString()}'";

        protected override Task SanitizeEntity(TimeLog timeLog, TimeLog oldTimeLog)
        {
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

        protected override async Task PostSaveManyAction(IReadOnlyCollection<TimeLog> entities, IReadOnlyCollection<TimeLog> oldEntities, SaveType saveType)
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

        #endregion Overrides

        private void RecordUpdate(IReadOnlyCollection<TimeLog> updatedTimeLogs, IReadOnlyCollection<TimeLog> oldRecords, IReadOnlyCollection<Branch> branches)
        {
            foreach (var newValue in updatedTimeLogs)
            {
                List<UserActivityItem> changes = new List<UserActivityItem>();
                var entityName = UserActivityName.ToLower();

                var oldValue = oldRecords
                    .Where(tl => tl.EmployeeID.Value == newValue.EmployeeID.Value)
                    .Where(tl => tl.LogDate == newValue.LogDate)
                    .FirstOrDefault();
                if (oldValue == null) continue;

                var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(newValue)}.";

                if (newValue.TimeIn != oldValue.TimeIn)
                {
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = newValue.RowID.Value,
                        Description = $"Updated time in from '{oldValue.TimeIn.ToStringFormat("hh:mm tt")}' to '{newValue.TimeIn.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                        ChangedEmployeeId = newValue.EmployeeID.Value
                    });
                }
                if (newValue.TimeOut != oldValue.TimeOut)
                {
                    changes.Add(new UserActivityItem()
                    {
                        EntityId = newValue.RowID.Value,
                        Description = $"Updated time out from '{oldValue.TimeOut.ToStringFormat("hh:mm tt")}' to '{newValue.TimeOut.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                        ChangedEmployeeId = newValue.EmployeeID.Value
                    });
                }

                var currentDateOut = newValue.TimeStampOut == null ? newValue.TimeStampOut : newValue.TimeStampOut.Value.Date;
                var oldDateOut = oldValue.TimeStampOut == null ? oldValue.TimeStampOut : oldValue.TimeStampOut.Value.Date;
                if (currentDateOut.NullableEquals(oldDateOut) == false)
                {
                    // TimeStampOut is null by default. It means TimeStampOut is equals to LogDate
                    var dontSave = oldValue.TimeStampOut == null && newValue.TimeStampOut != null && newValue.TimeStampOut.Value.Date == newValue.LogDate.Date;

                    if (dontSave == false)
                    {
                        changes.Add(new UserActivityItem()
                        {
                            EntityId = newValue.RowID.Value,
                            Description = $"Updated date out from '{oldValue.TimeStampOut.ToShortDateString()}' to '{newValue.TimeStampOut.ToShortDateString()}' {suffixIdentifier}",
                            ChangedEmployeeId = newValue.EmployeeID.Value
                        });
                    }
                }
                if (newValue.BranchID != oldValue.BranchID)
                {
                    var oldBranch = "";
                    var newBranch = "";

                    if (oldValue.BranchID.HasValue)
                        oldBranch = branches.Where(x => x.RowID == oldValue.BranchID).FirstOrDefault()?.Name;
                    if (newValue.BranchID.HasValue)
                        newBranch = branches.Where(x => x.RowID == newValue.BranchID).FirstOrDefault()?.Name;

                    changes.Add(new UserActivityItem()
                    {
                        EntityId = newValue.RowID.Value,
                        Description = $"Updated branch from '{oldBranch}' to '{newBranch}' {suffixIdentifier}",
                        ChangedEmployeeId = newValue.EmployeeID.Value
                    });
                }

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
    }
}
