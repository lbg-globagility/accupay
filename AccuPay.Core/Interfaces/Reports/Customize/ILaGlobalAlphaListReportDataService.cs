using AccuPay.Core.ReportModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILaGlobalAlphaListReportDataService
    {
        Task<List<LaGlobalAlphaListReportModel>> GetData(int organizationId,
            bool actualSwitch,
            int startPeriodId,
            int endPeriodId,
            string saveFilePath);
    }
}
