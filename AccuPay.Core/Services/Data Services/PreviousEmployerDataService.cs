using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class PreviousEmployerDataService : BaseEmployeeDataService<PreviousEmployer>, IPreviousEmployerDataService
    {
        private const string UserActivityName = "Previous Employer";

        public PreviousEmployerDataService(
            IPreviousEmployerRepository repository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(repository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Previous Employer")
        {
        }

        protected override string CreateUserActivitySuffixIdentifier(PreviousEmployer entity)
        {
            return $" with name '{entity.Name}'";
        }

        protected override string GetUserActivityName(PreviousEmployer entity)
        {
            return UserActivityName;
        }

        protected override async Task RecordUpdate(PreviousEmployer newValue, PreviousEmployer oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.Name != oldValue.Name)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated name from '{oldValue.Name}' to '{newValue.Name}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.TradeName != oldValue.TradeName)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated trade name from '{oldValue.TradeName}' to '{newValue.TradeName}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.ContactName != oldValue.ContactName)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated contact name from '{oldValue.ContactName}' to '{newValue.ContactName}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.MainPhone != oldValue.MainPhone)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated main phone from '{oldValue.MainPhone}' to '{newValue.MainPhone}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.AltPhone != oldValue.AltPhone)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated alt phone from '{oldValue.AltPhone}' to '{newValue.AltPhone}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.FaxNumber != oldValue.FaxNumber)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated fax number from '{oldValue.FaxNumber}' to '{newValue.FaxNumber}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.EmailAddress != oldValue.EmailAddress)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated email address from '{oldValue.EmailAddress}' to '{newValue.EmailAddress}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.AltEmailAddress != oldValue.AltEmailAddress)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated alt email address from '{oldValue.AltEmailAddress}' to '{newValue.AltEmailAddress}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.URL != oldValue.URL)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated URL from '{oldValue.URL}' to '{newValue.URL}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.TINNo != oldValue.TINNo)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated TIN number from '{oldValue.TINNo}' to '{newValue.TINNo}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.JobTitle != oldValue.JobTitle)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated job title from '{oldValue.JobTitle}' to '{newValue.JobTitle}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.JobFunction != oldValue.JobFunction)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated job function from '{oldValue.JobFunction}' to '{newValue.JobFunction}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.OrganizationType != oldValue.OrganizationType)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated organization type from '{oldValue.OrganizationType}' to '{newValue.OrganizationType}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.ExperienceFrom.ToShortDateString() != oldValue.ExperienceFrom.ToShortDateString())
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated experience start date from '{oldValue.ExperienceFrom.ToShortDateString()}' to '{newValue.ExperienceFrom.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.ExperienceTo.ToShortDateString() != oldValue.ExperienceTo.ToShortDateString())
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated experience end date from '{oldValue.ExperienceTo.ToShortDateString()}' to '{newValue.ExperienceTo.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.BusinessAddress != oldValue.BusinessAddress)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated company address from '{oldValue.BusinessAddress}' to '{newValue.BusinessAddress}' {suffixIdentifier}",
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
