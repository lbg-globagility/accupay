using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class AwardDataService : BaseEmployeeDataService<Award>, IAwardDataService
    {
        private const string UserActivityName = "Award";

        public AwardDataService(
            IAwardRepository repository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(repository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Award")
        {
        }

        protected override string CreateUserActivitySuffixIdentifier(Award entity)
        {
            return $" with type '{ entity.AwardType}'";
        }

        protected override string GetUserActivityName(Award entity)
        {
            return UserActivityName;
        }

        protected override async Task RecordUpdate(Award newValue, Award oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.AwardType != oldValue.AwardType)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated type from '{oldValue.AwardType}' to '{newValue.AwardType}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.AwardDescription != oldValue.AwardDescription)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated description from '{oldValue.AwardDescription}' to '{newValue.AwardDescription}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.AwardDate != oldValue.AwardDate)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated date from '{oldValue.AwardDate.ToShortDateString()}' to '{newValue.AwardDate.ToShortDateString()}' {suffixIdentifier}",
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
