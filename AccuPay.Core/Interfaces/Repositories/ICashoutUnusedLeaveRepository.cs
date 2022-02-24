using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces.Repositories
{
    public interface ICashoutUnusedLeaveRepository
    {
        Task<List<CashoutUnusedLeave>> GetByPeriodAsync(int payPeriodId);

        Task SaveManyAsync(List<CashoutUnusedLeave> data);

        Task ConsumeLeaveBalanceAsync(int organizationId, int userId, int payPeriodId, List<CashoutUnusedLeave> data);

        Task<List<CashoutUnusedLeave>> GetFromLatestPeriodAsync(int organizationId);
    }
}
