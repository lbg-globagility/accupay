using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class SoloParentBeneficiaryDataService : BaseOrganizationDataService<SoloParentBeneficiary>, ISoloParentBeneficiaryDataService
    {
        private const string UserActivityName = "SoloParentBeneficiary";

        private readonly ISoloParentBeneficiaryRepository _soloParentBeneficiaryRepository;

        public SoloParentBeneficiaryDataService(ISoloParentBeneficiaryRepository soloParentBeneficiaryRepository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(soloParentBeneficiaryRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "SoloParentBeneficiary")
        {
            _soloParentBeneficiaryRepository = soloParentBeneficiaryRepository;
        }

        public async Task<ICollection<SoloParentBeneficiary>> GetAllByOrganizationIdAsync(int orgId)
            => await _soloParentBeneficiaryRepository.GetAllByOrganizationIdAsync(orgId);

        protected override string CreateUserActivitySuffixIdentifier(SoloParentBeneficiary entity) => UserActivityName;

        protected override string GetUserActivityName(SoloParentBeneficiary entity) => UserActivityName;
    }
}
