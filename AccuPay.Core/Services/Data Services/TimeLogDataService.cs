using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class TimeLogDataService : BaseEmployeeDataService<TimeLog>
    {
        private const string UserActivityName = "Time Log";

        private readonly IBranchRepository _branchRepository;

        public TimeLogDataService(
            ITimeLogRepository timeLogRepository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            IBranchRepository branchRepository,
            PayrollContext context,
            IPolicyHelper policy) :

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

        public async Task SaveImportAsync(List<TimeLog> timeLogs, int changedByUserId)
        {
            string importId = Guid.NewGuid().ToString();

            foreach (var timeLog in timeLogs)
            {
                timeLog.TimeentrylogsImportID = importId;
            }

            await SaveManyAsync(timeLogs, changedByUserId);
        }

        #endregion Save

        #region Overrides

        protected override string GetUserActivityName(TimeLog leave)
        {
            return UserActivityName;
        }

        protected override string CreateUserActivitySuffixIdentifier(TimeLog log)
        {
            return $" with date '{log.LogDate.ToShortDateString()}'";
        }

        protected override async Task SanitizeEntity(TimeLog timeLog, TimeLog oldTimeLog, int changedByUserId)
        {
            await base.SanitizeEntity(
                entity: timeLog,
                oldEntity: oldTimeLog,
                currentlyLoggedInUserId: changedByUserId);

            if (timeLog.LogDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753.");

            if (timeLog.TimeIn == null && timeLog.TimeOut == null)
                throw new BusinessLogicException("Time-in and Time-out cannot be both empty.");

            if (timeLog.TimeIn != null && timeLog.TimeStampIn == null)
            {
                timeLog.TimeStampIn = timeLog.TimeInFull;
            }

            if (timeLog.TimeOut != null && timeLog.TimeStampOut == null)
            {
                timeLog.TimeStampOut = timeLog.TimeOutFull;
            }
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

        protected override async Task PostDeleteAction(TimeLog entity, int currentlyLoggedInUserId)
        {
            // supplying Product data for saving useractivity
            entity.Branch = await _branchRepository.GetByIdAsync(entity.BranchID.Value);

            await base.PostDeleteAction(entity, currentlyLoggedInUserId);
        }

        protected override async Task PostDeleteManyAction(IReadOnlyCollection<TimeLog> entities, int changedByUserId)
        {
            await GetBranchProperty(entities);

            // user can add multiple time logs in a day specially when they import multiple times.
            var groupedDeletedTimeLogs = entities.GroupBy(x => new { x.EmployeeID, x.LogDate });

            foreach (var group in groupedDeletedTimeLogs)
            {
                foreach (var item in group.ToList())
                {
                    await RecordDelete(item, changedByUserId);
                }
            }
        }

        protected override async Task PostSaveAction(TimeLog entity, TimeLog oldEntity, SaveType saveType)
        {
            // supplying Branch data for saving useractivity
            await GetBranchProperty(entity);
            await GetBranchProperty(oldEntity);

            await base.PostSaveAction(entity, oldEntity, saveType);
        }

        protected override async Task PostSaveManyAction(
            IReadOnlyCollection<TimeLog> entities,
            IReadOnlyCollection<TimeLog> oldEntities,
            SaveType saveType,
            int currentlyLoggedInUserId)
        {
            if (!entities.Any()) return;

            var branches = await GetBranchProperty(entities);
            await GetBranchProperty(oldEntities, branches);

            await base.PostSaveManyAction(entities, oldEntities, saveType, currentlyLoggedInUserId);
        }

        protected override async Task RecordUpdate(TimeLog newValue, TimeLog oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

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
                changes.Add(new UserActivityItem()
                {
                    EntityId = newValue.RowID.Value,
                    Description = $"Updated branch from '{oldValue.Branch?.Name}' to '{newValue.Branch?.Name}' {suffixIdentifier}",
                    ChangedEmployeeId = newValue.EmployeeID.Value
                });
            }

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

        private async Task GetBranchProperty(TimeLog entity)
        {
            if (entity == null) return;

            if (entity.BranchID == null)
            {
                entity.Branch = null;
            }
            else
            {
                entity.Branch = await _branchRepository.GetByIdAsync(entity.BranchID.Value);
            }
        }

        private async Task<ICollection<Branch>> GetBranchProperty(IReadOnlyCollection<TimeLog> entities, ICollection<Branch> branches = null)
        {
            if (entities == null || !entities.Any())
                return new List<Branch>();

            if (branches == null)
            {
                var branchIds = entities
                    .Where(x => x.BranchID != null)
                    .Select(x => x.BranchID.Value)
                    .ToList();

                branchIds = branchIds.Distinct().ToList();

                branches = await _branchRepository.GetManyByIdsAsync(branchIds.ToArray());
            }

            foreach (var entity in entities)
            {
                entity.Branch = branches.Where(x => x.RowID.Value == entity.BranchID).FirstOrDefault();
            }

            return branches;
        }
    }
}
