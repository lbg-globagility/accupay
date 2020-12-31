using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class TimeEntryDataHelper
    {
        private const string UserActivityName = "Time Entry";

        private readonly UserActivityRepository _userActivityRepository;

        public TimeEntryDataHelper(UserActivityRepository userActivityRepository)
        {
            _userActivityRepository = userActivityRepository;
        }

        internal async Task RecordCreate(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries)
        {
            await Record(
                currentlyLoggedInUserId,
                timeEntries,
                "Created a time entry",
                UserActivityRepository.RecordTypeAdd);
        }

        internal async Task RecordEdit(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries)
        {
            await Record(
                currentlyLoggedInUserId,
                timeEntries,
                "Updated a time entry",
                UserActivityRepository.RecordTypeEdit);
        }

        internal async Task RecordDelete(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries)
        {
            await Record(
                currentlyLoggedInUserId,
                timeEntries,
                "Deleted a time entry",
                UserActivityRepository.RecordTypeDelete);
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
