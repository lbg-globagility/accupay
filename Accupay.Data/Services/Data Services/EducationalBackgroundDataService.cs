using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;

namespace AccuPay.Data.Services
{
    public class EducationalBackgroundDataService : BaseEmployeeDataService<EducationalBackground>, IEducationalBackgroundDataService
    {
        private const string UserActivityName = "Educational Background";

        public EducationalBackgroundDataService(
            EducationalBackgroundRepository repository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
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
    }
}
