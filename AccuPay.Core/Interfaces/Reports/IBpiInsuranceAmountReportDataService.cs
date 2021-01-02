using AccuPay.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IBpiInsuranceAmountReportDataService
    {
        Task<IEnumerable<BpiInsuranceDataSource>> GetData(int organizationId, int userId, DateTime selectedDate);
    }
}
