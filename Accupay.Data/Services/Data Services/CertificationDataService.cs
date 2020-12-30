using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class CertificationDataService : BaseEmployeeDataService<Certification>, ICertificationDataService
    {
        private const string UserActivityName = "Certification";

        public CertificationDataService(
            CertificationRepository repository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(repository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Certification")
        {
        }

        protected override string CreateUserActivitySuffixIdentifier(Certification entity)
        {
            return $" with type '{ entity.CertificationType}'";
        }

        protected override string GetUserActivityName(Certification entity)
        {
            return UserActivityName;
        }

        protected override async Task RecordUpdate(Certification newValue, Certification oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.CertificationType != oldValue.CertificationType)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated type from '{oldValue.CertificationType}' to '{newValue.CertificationType}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.IssuingAuthority != oldValue.IssuingAuthority)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated issuing authority from '{oldValue.IssuingAuthority}' to '{newValue.IssuingAuthority}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.CertificationNo != oldValue.CertificationNo)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated number from '{oldValue.CertificationNo}' to '{newValue.CertificationNo}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.IssueDate != oldValue.IssueDate)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated issued date from '{oldValue.IssueDate.ToShortDateString()}' to '{newValue.IssueDate.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.ExpirationDate?.ToShortDateString() != oldValue.ExpirationDate?.ToShortDateString())
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated expiration date from '{oldValue.ExpirationDate?.ToShortDateString()}' to '{newValue.ExpirationDate?.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.Comments != oldValue.Comments)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated comments from '{oldValue.Comments}' to '{newValue.Comments}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });

            if (changes.Any())
            {
                await _userActivityRepository.CreateRecordAsync(
                    newValue.LastUpdBy.Value,
                    UserActivityName,
                    newValue.OrganizationID.Value,
                    UserActivityRepository.RecordTypeEdit,
                    changes);
            }
        }
    }
}
