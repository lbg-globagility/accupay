using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IFiledLeaveReportDataService
    {
        Task<IEnumerable<LeaveTransaction>> GetData(int organizationId, TimePeriod selectedPeriod);
    }
}
