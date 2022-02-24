using AccuPay.Core.Entities;
using AccuPay.Core.ReportModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces.Reports
{
    public interface IAlphaListReportDataService
    {
        Task<List<AlphalistModel>> GetData(
            int organizationId,
            bool actualSwitch,
            PayPeriod startPeriod,
            PayPeriod endPeriod);

        List<SalaryModel> GetLatestSalaries(int organizationId);

        List<SalaryModel> GetLatestSalaries2(int organizationId);
    }
}
