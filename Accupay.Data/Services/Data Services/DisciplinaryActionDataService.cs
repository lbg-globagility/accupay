using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
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
    }
}
