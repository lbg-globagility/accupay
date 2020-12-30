using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;

namespace AccuPay.Data.Services
{
    public class PreviousEmployerDataService : BaseEmployeeDataService<PreviousEmployer>, IPreviousEmployerDataService
    {
        private const string UserActivityName = "Previous Employer";

        public PreviousEmployerDataService(
            PreviousEmployerRepository repository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
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
    }
}
