using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class DisciplinaryActionDataService : BaseEmployeeDataService<DisciplinaryAction>, IDisciplinaryActionDataService
    {
        private const string UserActivityName = "Disciplinary Action";
        private readonly ProductRepository _productRepository;

        public DisciplinaryActionDataService(
            DisciplinaryActionRepository repository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy,
            ProductRepository productRepository) :

            base(repository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Disciplinary Action")
        {
            _productRepository = productRepository;
        }

        protected override string CreateUserActivitySuffixIdentifier(DisciplinaryAction entity)
        {
            return $" with finding name '{entity.FindingName}'";
        }

        protected override string GetUserActivityName(DisciplinaryAction entity)
        {
            return UserActivityName;
        }

        protected override async Task PostDeleteAction(DisciplinaryAction entity, int currentlyLoggedInUserId)
        {
            // supplying Product data for saving useractivity
            entity.Finding = await _productRepository.GetByIdAsync(entity.FindingID);

            await base.PostDeleteAction(entity, currentlyLoggedInUserId);
        }

        protected override async Task PostSaveAction(DisciplinaryAction entity, DisciplinaryAction oldEntity, SaveType saveType)
        {
            // supplying Product data for saving useractivity
            entity.Finding = await _productRepository.GetByIdAsync(entity.FindingID);

            if (oldEntity != null)
            {
                oldEntity.Finding = await _productRepository.GetByIdAsync(oldEntity.FindingID);
            }

            await base.PostSaveAction(entity, oldEntity, saveType);
        }

        protected override async Task RecordUpdate(DisciplinaryAction newValue, DisciplinaryAction oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.FindingID != oldValue.FindingID)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated finding name from '{oldValue.FindingName}' to '{newValue.FindingName}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID
                });
            if (newValue.Action != oldValue.Action)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated penaty from '{oldValue.Action}' to '{newValue.Action}' {suffixIdentifier}",
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
            if (newValue.FindingDescription != oldValue.FindingDescription)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated finding description from '{oldValue.FindingDescription}' to '{newValue.FindingDescription}' {suffixIdentifier}",
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
