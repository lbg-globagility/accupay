using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class EducationalBackgroundDataService : BaseEmployeeDataService<EducationalBackground>, IEducationalBackgroundDataService
    {
        private const string UserActivityName = "Educational Background";

        public EducationalBackgroundDataService(
            IEducationalBackgroundRepository repository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(repository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Educational Background")
        {
        }

        protected override string CreateUserActivitySuffixIdentifier(EducationalBackground entity)
        {
            return $" with type '{entity.Type}' and school '{entity.School}'";
        }

        protected override string GetUserActivityName(EducationalBackground entity)
        {
            return UserActivityName;
        }

        protected override async Task RecordUpdate(EducationalBackground newValue, EducationalBackground oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.Type != oldValue.Type)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated type from '{oldValue.Type}' to '{newValue.Type}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.School != oldValue.School)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated school from '{oldValue.School}' to '{newValue.School}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.Degree != oldValue.Degree)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated degree from '{oldValue.Degree}' to '{newValue.Degree}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.Course != oldValue.Course)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated course from '{oldValue.Course}' to '{newValue.Course}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.Major != oldValue.Major)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated major from '{oldValue.Major}' to '{newValue.Major}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.DateFrom != oldValue.DateFrom)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start date from '{oldValue.DateFrom.ToShortDateString()}' to '{newValue.DateFrom.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.DateTo != oldValue.DateTo)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated end date from '{oldValue.DateTo.ToShortDateString()}' to '{newValue.DateTo.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.Remarks != oldValue.Remarks)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated remarks from '{oldValue.Remarks}' to '{newValue.Remarks}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
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
    }
}
