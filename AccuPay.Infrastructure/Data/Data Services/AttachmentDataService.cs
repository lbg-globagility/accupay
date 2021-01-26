using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;

namespace AccuPay.Infrastructure.Data
{
    public class AttachmentDataService : BaseEmployeeDataService<Attachment>, IAttachmentDataService
    {
        private const string UserActivityName = "Attachment";

        public AttachmentDataService(
            IAttachmentRepository repository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(repository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Attachment")
        {
        }

        protected override string CreateUserActivitySuffixIdentifier(Attachment entity)
        {
            return $" with type '{ entity.Type}'";
        }

        protected override string GetUserActivityName(Attachment entity)
        {
            return UserActivityName;
        }
    }
}
