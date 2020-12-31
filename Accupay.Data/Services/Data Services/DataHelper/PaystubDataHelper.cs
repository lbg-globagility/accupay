using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PaystubDataHelper
    {
        private const string UserActivityName = "Paystub";

        private readonly UserActivityRepository _userActivityRepository;

        public PaystubDataHelper(UserActivityRepository useractivityRepository)
        {
            _userActivityRepository = useractivityRepository;
        }

        internal async Task RecordCreate(int currentlyLoggedInUserId, Paystub paystub, PayPeriod payPeriod)
        {
            await Record(
                currentlyLoggedInUserId,
                new List<Paystub>() { paystub },
                "Created a paystub",
                UserActivityRepository.RecordTypeDelete,
                payPeriod);
        }

        internal async Task RecordEdit(int currentlyLoggedInUserId, Paystub paystub, PayPeriod payPeriod)
        {
            await Record(
                currentlyLoggedInUserId,
                new List<Paystub>() { paystub },
                "Updated a paystub",
                UserActivityRepository.RecordTypeDelete,
                payPeriod);
        }

        public async Task RecordDelete(int currentlyLoggedInUserId, Paystub paystub, PayPeriod payPeriod)
        {
            await RecordDelete(
                currentlyLoggedInUserId,
                new List<Paystub>() { paystub },
                payPeriod);
        }

        public async Task RecordDelete(int currentlyLoggedInUserId, IReadOnlyCollection<Paystub> paystubs, PayPeriod payPeriod)
        {
            await Record(
                currentlyLoggedInUserId,
                paystubs,
                "Deleted a paystub",
                UserActivityRepository.RecordTypeDelete,
                payPeriod);
        }

        private async Task Record(
            int currentlyLoggedInUserId,
            IReadOnlyCollection<Paystub> paystubs,
            string description,
            string recordType,
            PayPeriod payPeriod)
        {
            if (paystubs == null || !paystubs.Any()) return;

            // this assumes that all paystubs has the same OrganizationID
            var organizationId = paystubs
                .Where(x => x.OrganizationID != null)
                .Select(x => x.OrganizationID.Value)
                .First();

            var activityItem = new List<UserActivityItem>();

            paystubs = paystubs
                .OrderBy(x => x.EmployeeID)
                .ToList();

            foreach (var timeEntry in paystubs)
            {
                activityItem.Add(new UserActivityItem()
                {
                    EntityId = timeEntry.RowID.Value,
                    Description = $"{description} for payroll '{payPeriod.PayFromDate.ToShortDateString()}' to '{payPeriod.PayToDate.ToShortDateString()}'",
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
