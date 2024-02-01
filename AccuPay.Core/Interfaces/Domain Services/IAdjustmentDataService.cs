using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces.Domain_Services
{
    public interface IAdjustmentDataService
    {
        Task AppendManyAsync(int organizationId, int userId, int payPeriodId, List<Adjustment> adjustments);

        Task AppendManyAsync(int organizationId, int userId, int payPeriodId, List<ActualAdjustment> actualAdjustments);
    }
}
