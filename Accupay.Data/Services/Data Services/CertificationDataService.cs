using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;

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
    }
}
