using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class PaystubDataHelper
    {
        private const string UserActivityName = "Paystub";

        private readonly IPaystubRepository _paystubRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserActivityRepository _userActivityRepository;

        public PaystubDataHelper(
            IPaystubRepository paystubRepository,
            IProductRepository productRepository,
            IUserActivityRepository useractivityRepository)
        {
            _paystubRepository = paystubRepository;
            _productRepository = productRepository;
            _userActivityRepository = useractivityRepository;
        }

        #region Paystub

        internal async Task RecordCreate(int currentlyLoggedInUserId, Paystub paystub, PayPeriod payPeriod)
        {
            await Record(
                currentlyLoggedInUserId,
                new List<Paystub>() { paystub },
                "Created a paystub",
                UserActivity.RecordTypeAdd,
                payPeriod);
        }

        internal async Task RecordEdit(int currentlyLoggedInUserId, Paystub paystub, PayPeriod payPeriod)
        {
            await Record(
                currentlyLoggedInUserId,
                new List<Paystub>() { paystub },
                "Updated a paystub",
                UserActivity.RecordTypeEdit,
                payPeriod);
        }

        internal async Task RecordDelete(int currentlyLoggedInUserId, Paystub paystub, PayPeriod payPeriod)
        {
            await RecordDelete(
                currentlyLoggedInUserId,
                new List<Paystub>() { paystub },
                payPeriod);
        }

        internal async Task RecordDelete(int currentlyLoggedInUserId, IReadOnlyCollection<Paystub> paystubs, PayPeriod payPeriod)
        {
            // TODO: maybe record delete for adjustments before calling this

            await Record(
                currentlyLoggedInUserId,
                paystubs,
                "Deleted a paystub",
                UserActivity.RecordTypeDelete,
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
                    Description = $"{description} for payroll {GetPayPeriodString(payPeriod)}",
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

        #endregion Paystub

        #region Paystub Adjustments

        internal async Task RecordUpdateAdjustments<T>(
            int currentlyLoggedInUserId,
            IReadOnlyCollection<T> adjustments,
            IReadOnlyCollection<T> originalAdjustments) where T : IAdjustment
        {
            if (adjustments == null || !adjustments.Any()) return;
            if (originalAdjustments == null || !originalAdjustments.Any()) return;

            await GetAdjustmentProductProperty(adjustments, originalAdjustments);
            await GetAdjustmentPaystubProperty(adjustments, originalAdjustments);

            string entityName = GetPaystubAdjustmentEntityName<T>();

            var activityItems = new List<UserActivityItem>();

            foreach (var newValue in adjustments)
            {
                var oldValue = originalAdjustments
                    .Where(x => x.RowID == newValue.RowID)
                    .FirstOrDefault();

                if (oldValue == null) continue;

                var suffixIdentifier = $"of {entityName}{CreatePaystubAdjustmentSuffixIdentifier(oldValue)}";

                var newActivityItems = RecordUpdateAdjustment(
                    newValue,
                    suffixIdentifier,
                    oldValue);

                if (newActivityItems != null && newActivityItems.Any())
                {
                    activityItems.AddRange(newActivityItems);
                }
            }

            if (activityItems.Any())
            {
                await RecordPaystubAdjustment(
                    currentlyLoggedInUserId,
                    adjustments,
                    activityItems,
                    UserActivity.RecordTypeEdit);
            }
        }

        private static List<UserActivityItem> RecordUpdateAdjustment<T>(
            T newValue,
            string suffixIdentifier,
            T oldValue) where T : IAdjustment
        {
            if (oldValue == null) return null;

            var activityItems = new List<UserActivityItem>();

            if (newValue.ProductID != oldValue.ProductID)
            {
                activityItems.Add(new UserActivityItem()
                {
                    EntityId = newValue.RowID.Value,
                    Description = $"Updated type from '{oldValue.Product?.PartNo}' to '{newValue.Product?.PartNo}' {suffixIdentifier}",
                    ChangedEmployeeId = newValue.Paystub.EmployeeID.Value
                });
            }

            if (AccuMath.CommercialRound(newValue.Amount) != AccuMath.CommercialRound(oldValue.Amount))
            {
                activityItems.Add(new UserActivityItem()
                {
                    EntityId = newValue.RowID.Value,
                    Description = $"Updated amount from '{ oldValue.Amount }' to '{ newValue.Amount }' {suffixIdentifier}",
                    ChangedEmployeeId = newValue.Paystub.EmployeeID.Value
                });
            }

            if (newValue.Comment != oldValue.Comment)
            {
                activityItems.Add(new UserActivityItem()
                {
                    EntityId = newValue.RowID.Value,
                    Description = $"Updated comment from '{ oldValue.Comment }' to '{ newValue.Comment }' {suffixIdentifier}",
                    ChangedEmployeeId = newValue.Paystub.EmployeeID.Value
                });
            }

            return activityItems;
        }

        internal async Task RecordCreateAdjustments<T>(int currentlyLoggedInUserId, IReadOnlyCollection<T> adjustments) where T : IAdjustment
        {
            if (adjustments == null || !adjustments.Any()) return;

            await RecordSimplePaystubAdjustments(
                currentlyLoggedInUserId,
                adjustments,
                "Created a new",
                UserActivity.RecordTypeAdd);
        }

        internal async Task RecordDeleteAdjustments<T>(int currentlyLoggedInUserId, IReadOnlyCollection<T> adjustments) where T : IAdjustment
        {
            if (adjustments == null || !adjustments.Any()) return;

            await RecordSimplePaystubAdjustments(
                currentlyLoggedInUserId,
                adjustments,
                "Deleted an",
                UserActivity.RecordTypeDelete);
        }

        private async Task GetAdjustmentProductProperty<T>(
            IReadOnlyCollection<T> adjustments,
            IReadOnlyCollection<T> originalAdjustments = null) where T : IAdjustment
        {
            var productIds = adjustments.Select(x => x.ProductID.Value).ToList();

            if (originalAdjustments != null)
            {
                productIds.AddRange(originalAdjustments.Select(x => x.ProductID.Value).ToList());
            }

            productIds = productIds.Distinct().ToList();

            var products = await _productRepository.GetManyByIdsAsync(productIds.ToArray());

            foreach (var adjustment in adjustments)
            {
                adjustment.Product = products
                    .Where(x => x.RowID == adjustment.ProductID)
                    .FirstOrDefault();
            }

            if (originalAdjustments != null)
            {
                foreach (var adjustment in originalAdjustments)
                {
                    adjustment.Product = products
                        .Where(x => x.RowID == adjustment.ProductID)
                        .FirstOrDefault();
                }
            }
        }

        private async Task GetAdjustmentPaystubProperty<T>(
            IReadOnlyCollection<T> adjustments,
            IReadOnlyCollection<T> originalAdjustments = null) where T : IAdjustment
        {
            var paystubIds = adjustments.Select(x => x.PaystubID.Value).ToList();

            if (originalAdjustments != null)
            {
                paystubIds.AddRange(originalAdjustments.Select(x => x.PaystubID.Value).ToList());
            }

            paystubIds = paystubIds.Distinct().ToList();

            var paystubs = await _paystubRepository.GetWithPayPeriod(paystubIds.ToArray());

            foreach (var adjustment in adjustments)
            {
                adjustment.Paystub = paystubs
                    .Where(x => x.RowID == adjustment.PaystubID)
                    .FirstOrDefault();
            }

            if (originalAdjustments != null)
            {
                foreach (var adjustment in originalAdjustments)
                {
                    adjustment.Paystub = paystubs
                        .Where(x => x.RowID == adjustment.PaystubID)
                        .FirstOrDefault();
                }
            }
        }

        private async Task RecordSimplePaystubAdjustments<T>(
            int currentlyLoggedInUserId,
            IReadOnlyCollection<T> adjustments,
            string description,
            string recordType) where T : IAdjustment
        {
            await GetAdjustmentProductProperty(adjustments);
            await GetAdjustmentPaystubProperty(adjustments);

            string entityName = GetPaystubAdjustmentEntityName<T>();

            var activityItems = new List<UserActivityItem>();

            foreach (var adjustment in adjustments)
            {
                activityItems.Add(new UserActivityItem()
                {
                    EntityId = adjustment.RowID.Value,
                    Description = $"{description} {entityName}{CreatePaystubAdjustmentSuffixIdentifier(adjustment)}",
                    ChangedEmployeeId = adjustment.Paystub.EmployeeID.Value
                });
            }

            if (activityItems.Any())
            {
                await RecordPaystubAdjustment(
                    currentlyLoggedInUserId,
                    adjustments,
                    activityItems,
                    recordType);
            }
        }

        private static string CreatePaystubAdjustmentSuffixIdentifier<T>(T adjustment) where T : IAdjustment
        {
            if (adjustment == null) return string.Empty;
            return $" with type '{adjustment.Product?.PartNo}' for payroll { GetPayPeriodString(adjustment.Paystub?.PayPeriod)}.";
        }

        internal async Task RecordPaystubAdjustment<T>(
            int currentlyLoggedInUserId,
            IReadOnlyCollection<T> adjustments,
            List<UserActivityItem> activityItems,
            string recordType) where T : IAdjustment
        {
            var organizationId = adjustments
                .Where(x => x.OrganizationID != null)
                .Select(x => x.OrganizationID.Value)
                .First();

            await _userActivityRepository.CreateRecordAsync(
                currentlyLoggedInUserId,
                "Paystub Adjustment",
                organizationId,
                recordType,
                activityItems);
        }

        private static string GetPaystubAdjustmentEntityName<T>() where T : IAdjustment
        {
            return typeof(Adjustment).IsAssignableFrom(typeof(T)) ? "adjustment" : "actual adjustment";
        }

        #endregion Paystub Adjustments

        private static string GetPayPeriodString(PayPeriod payPeriod)
        {
            if (payPeriod == null) return string.Empty;

            return $"'{payPeriod.PayFromDate.ToShortDateString()}' to '{payPeriod.PayToDate.ToShortDateString()}'";
        }
    }
}
