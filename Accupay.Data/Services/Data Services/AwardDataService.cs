using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;

namespace AccuPay.Data.Services
{
    public class AwardDataService : BaseEmployeeDataService<Award>, IAwardDataService
    {
        private const string UserActivityName = "Award";

        public AwardDataService(
            AwardRepository repository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
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
    }
}
