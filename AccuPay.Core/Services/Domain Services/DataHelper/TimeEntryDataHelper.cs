using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class TimeEntryDataHelper
    {
        private const string UserActivityName = "Time Entry";

        private readonly IUserActivityRepository _userActivityRepository;

        public TimeEntryDataHelper(IUserActivityRepository userActivityRepository)
        {
            _userActivityRepository = userActivityRepository;
        }

        internal async Task RecordCreate(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries)
        {
            await Record(
                currentlyLoggedInUserId,
                timeEntries,
                "Created a time entry",
                UserActivity.RecordTypeAdd);
        }

        internal async Task RecordEdit(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries)
        {
            await Record(
                currentlyLoggedInUserId,
                timeEntries,
                "Updated a time entry",
                UserActivity.RecordTypeEdit);
        }

        internal async Task RecordDelete(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries)
        {
            await Record(
                currentlyLoggedInUserId,
                timeEntries,
                "Deleted a time entry",
                UserActivity.RecordTypeDelete);
        }

        private async Task Record(
            int currentlyLoggedInUserId,
            IReadOnlyCollection<TimeEntry> timeEntries,
            string description,
            string recordType)
        {
            if (timeEntries == null || !timeEntries.Any()) return;

            // this assumes that all time entries has the same OrganizationID
            var organizationId = timeEntries
                .Where(x => x.OrganizationID != null)
                .Select(x => x.OrganizationID.Value)
                .First();

            var activityItem = new List<UserActivityItem>();

            timeEntries = timeEntries
                .OrderBy(x => x.EmployeeID)
                .ThenBy(x => x.Date)
                .ToList();

            foreach (var timeEntry in timeEntries)
            {
                activityItem.Add(new UserActivityItem()
                {
                    EntityId = timeEntry.RowID.Value,
                    Description = $"{description} for date '{timeEntry.Date.ToShortDateString()}'.",
                    ChangedEmployeeId = timeEntry.EmployeeID.Value
                });
            }

            if (activityItem.Any())
            {
                await _userActivityRepository.CreateRecordAsync(
                    currentlyLoggedInUserId,
                    UserActivityName,
                    organizationId,
                    recordType,
                    activityItem);
            }
        }
    }
}
