using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces.Domain_Services;
using AccuPay.Core.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data.Data_Services
{
    public class AdjustmentDataService: IAdjustmentDataService
    {
        private readonly IAdjustmentRepository _adjustmentRepository;

        public AdjustmentDataService(IAdjustmentRepository adjustmentRepository)
        {
            _adjustmentRepository = adjustmentRepository;
        }

        public async Task AppendManyAsync(int organizationId, int userId, int payPeriodId, List<Adjustment> adjustments)
        {
            await _adjustmentRepository.AppendManyAsync(organizationId: organizationId, userId: userId, payPeriodId: payPeriodId, adjustments: adjustments);
        }

        public async Task AppendManyAsync(int organizationId, int userId, int payPeriodId, List<ActualAdjustment> actualAdjustments)
        {
            await _adjustmentRepository.AppendManyAsync(organizationId: organizationId, userId: userId, payPeriodId: payPeriodId, actualAdjustments: actualAdjustments);
        }
    }
}
